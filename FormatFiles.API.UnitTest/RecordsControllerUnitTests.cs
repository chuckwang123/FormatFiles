using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using FormatFiles.API.Controllers;
using FormatFiles.Model.Interfaces;
using FormatFiles.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FormatFiles.API.UnitTest
{
    [TestClass]
    public class RecordsControllerUnitTests
    {
        private FileParser m_fileParser;
        private Mock<IStreamReader> m_streamReader;
        private Mock<IFactory> m_factory;
        private Mock<IHostingEnvironment> m_hosting;
        private Mock<IDirectoryInfo> m_directoryInfo;
        private RecordsController m_recordsController;
        
        [TestInitialize]
        public void TestInitialize()
        {
            m_streamReader = new Mock<IStreamReader>();
            m_streamReader.Setup(x => x.ReadtoEnd()).Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855");
            m_streamReader.SetupSequence(x => x.ReadLine()).Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns(null);
            m_streamReader.Setup(x => x.SetupStreamReaderWrapper()).Returns(new StreamReader(new MemoryStream(Encoding.Default.GetBytes("33\r\n1\r\n16\r\n5\r\n7"))));

            m_hosting = new Mock<IHostingEnvironment>();
            m_hosting.Setup(x => x.MapPath(It.IsAny<string>())).Returns(@"C:\what\ever\the\value\is\");

            m_directoryInfo = new Mock<IDirectoryInfo>();
            m_directoryInfo.Setup(x => x.GetParent(It.IsAny<string>())).Returns(new DirectoryInfo(@"C:\what\ever\the\value\is\"));
            m_directoryInfo.Setup(x => x.GetFiles(It.IsAny<string>())).Returns(new string[3] {"Comma","Pip","Space"});

            m_factory = new Mock<IFactory>();
            m_factory.Setup(x => x.CustomStreamReader).Returns(m_streamReader.Object);
            m_factory.Setup(x => x.CustomHostingEnvironment).Returns(m_hosting.Object);
            m_factory.Setup(x => x.CustomDirectoryInfo).Returns(m_directoryInfo.Object);

            m_fileParser = new FileParser(m_factory.Object);
            
            m_recordsController = new RecordsController(m_factory.Object);
        }


        private void SetupgenderStream()
        {
            m_streamReader.SetupSequence(x => x.ReadtoEnd())
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855")
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Foste Thomas Male Khaki 3/1/1836");


            m_streamReader.SetupSequence(x => x.ReadLine())
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836")
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836")
                .Returns("Foster,Thomas,Female,Khaki,3/1/1836")
                .Returns(null)
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Apple|Thomas|Male|Khaki|3/1/1836")
                .Returns(null)
                .Returns("Foster Thomas Male Khaki 3/1/1836")
                .Returns("Foster Thomas Male Khaki 3/1/1836")
                .Returns("Apple Thomas Male Khaki 3/1/1836")
                .Returns(null);
        }

        private void SetupBirthStream()
        {
            m_streamReader.SetupSequence(x => x.ReadtoEnd())
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855")
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Foste Thomas Male Khaki 3/1/1836");


            m_streamReader.SetupSequence(x => x.ReadLine())
                .Returns("Foster,Thomas,Male,Khaki,3/2/1836")
                .Returns("Foster,Thomas,Male,Khaki,3/4/1836")
                .Returns("Foster,Thomas,Female,Khaki,6/1/1836")
                .Returns(null)
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Foster|Thomas|Male|Khaki|3/10/1836")
                .Returns("Apple|Thomas|Male|Khaki|3/1/1836")
                .Returns(null)
                .Returns("Foster Thomas Male Khaki 3/2/1836")
                .Returns("Foster Thomas Male Khaki 3/1/1836")
                .Returns("Apple Thomas Male Khaki 3/1/1836")
                .Returns(null);
        }

        private void SetupLastNameStream()
        {
            m_streamReader.SetupSequence(x => x.ReadtoEnd())
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855")
                .Returns("Foster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Foste Thomas Male Khaki 3/1/1836");


            m_streamReader.SetupSequence(x => x.ReadLine())
                .Returns("Foster,Thomas,Male,Khaki,3/1/1836")
                .Returns("loster,Thomas,Male,Khaki,3/1/1836")
                .Returns("poster,Thomas,Female,Khaki,3/1/1836")
                .Returns(null)
                .Returns("poster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Flster|Thomas|Male|Khaki|3/1/1836")
                .Returns("Apple|Thomas|Male|Khaki|3/1/1836")
                .Returns(null)
                .Returns("Foster Thomas Male Khaki 3/1/1836")
                .Returns("Foister Thomas Male Khaki 3/1/1836")
                .Returns("Apple Thomas Male Khaki 3/1/1836")
                .Returns(null);
        }

        [TestMethod]
        public void SortByGender_theRecord_ShouldSort()
        {
            SetupgenderStream();
            var result = m_recordsController.SortByGender();
            var expectCommaList = new List<Person>
            {
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Female",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            };
            result.commaResult.ShouldBeEquivalentTo(expectCommaList);
            result.pipResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            });
            result.spaceResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            });
        }

        [TestMethod]
        public void SortByBirth_theRecord_ShouldSort()
        {
            SetupBirthStream();
            var result = m_recordsController.SortByBirth();
            var expectCommaList = new List<Person>
            {
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/4/1836")
                },
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Female",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("6/1/1836")
                }
            };
            result.commaResult.ShouldBeEquivalentTo(expectCommaList);
            result.pipResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/10/1836")
                }
            });
            result.spaceResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Foster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            });
        }

        [TestMethod]
        public void SortByName_theRecord_ShouldSort()
        {
            SetupLastNameStream();
            var result = m_recordsController.SortByName();
            var expectCommaList = new List<Person>
            {
                new Person
                {
                    LastName = "loster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "poster",
                    FirstName = "Thomas",
                    Gender = "Female",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            };
            result.commaResult.ShouldBeEquivalentTo(expectCommaList);
            result.pipResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Flster",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            });
            result.spaceResult.ShouldBeEquivalentTo(new List<Person>
            {
                new Person
                {
                    LastName = "Foister",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                },
                new Person
                {
                    LastName = "Apple",
                    FirstName = "Thomas",
                    Gender = "Male",
                    FavoriteColor = "Khaki",
                    DateofBirth = DateTime.Parse("3/1/1836")
                }
            });
        }


    }
}
