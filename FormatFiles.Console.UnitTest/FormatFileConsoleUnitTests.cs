using System;
using System.IO;
using System.Text;
using FormatFiles.Model.Interfaces;
using FormatFiles.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FormatFiles.Console.UnitTest
{
    [TestClass]
    public class FormatFileConsoleUnitTests
    {
        private FileParser m_fileParser;
        private Mock<IStreamReader> m_streamReader;
        private Mock<IFactory> m_factory;
        private Mock<IDirectoryInfo> m_directoryInfo;
        private BootStrapper bootStrapper;

        [TestInitialize]
        public void TestInitialize()
        {
            m_streamReader = new Mock<IStreamReader>();
            m_streamReader.Setup(x => x.ReadtoEnd()).Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855");
            m_streamReader.SetupSequence(x => x.ReadLine()).Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns(null);
            m_streamReader.Setup(x => x.SetupStreamReaderWrapper()).Returns(new StreamReader(new MemoryStream(Encoding.Default.GetBytes("33\r\n1\r\n16\r\n5\r\n7"))));

            m_directoryInfo = new Mock<IDirectoryInfo>();
            m_directoryInfo.Setup(x => x.GetParent(It.IsAny<string>())).Returns(new DirectoryInfo(@"C:\what\ever\the\value\is\"));
            m_directoryInfo.Setup(x => x.GetFiles(It.IsAny<string>())).Returns(new string[3] { "Comma", "Pip", "Space" });
            m_directoryInfo.Setup(x => x.GetCurrentDirectory()).Returns(@"C:\what\ever\the\value\is");

            m_factory = new Mock<IFactory>();
            m_factory.Setup(x => x.CustomStreamReader).Returns(m_streamReader.Object);
            m_factory.Setup(x => x.CustomDirectoryInfo).Returns(m_directoryInfo.Object);

            m_fileParser = new FileParser(m_factory.Object);
            bootStrapper = new BootStrapper(m_factory.Object);
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
        public void BootStrapper_SortByGender_ShouldNotThrow()
        {
            SetupgenderStream();
            bootStrapper.Sort("gender");
            m_streamReader.Verify(x=>x.ReadtoEnd(), Times.Exactly(3));
            m_streamReader.Verify(x => x.ReadLine(), Times.Exactly(12));
        }

        [TestMethod]
        public void BootStrapper_SortByBirth_ShouldNotThrow()
        {
            SetupBirthStream();
            bootStrapper.Sort("birth");
            m_streamReader.Verify(x => x.ReadtoEnd(), Times.Exactly(3));
            m_streamReader.Verify(x => x.ReadLine(), Times.Exactly(12));
        }

        [TestMethod]
        public void BootStrapper_SortByLastName_ShouldNotThrow()
        {
            SetupLastNameStream();
            bootStrapper.Sort("lastname");
            m_streamReader.Verify(x => x.ReadtoEnd(), Times.Exactly(3));
            m_streamReader.Verify(x => x.ReadLine(), Times.Exactly(12));
        }
        
    }
}
