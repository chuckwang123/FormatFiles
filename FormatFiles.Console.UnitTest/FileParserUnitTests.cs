using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using FluentAssert;
using FormatFiles.Model.Interfaces;
using FormatFiles.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FormatFiles.Console.UnitTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FileParserUnitTests
    {
        private FileParser m_fileParser;
        private Mock<IStreamReader> m_streamReader;
        private Mock<IFactory> m_factory;

        [TestInitialize]
        public void TestInitialize()
        {
            m_streamReader = new Mock<IStreamReader>();
            m_streamReader.Setup(x => x.ReadtoEnd()).Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855");
            m_streamReader.SetupSequence(x => x.ReadLine()).Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns(null);
            m_streamReader.Setup(x => x.SetupStreamReaderWrapper()).Returns(new StreamReader(new MemoryStream(Encoding.Default.GetBytes("33\r\n1\r\n16\r\n5\r\n7"))));

            m_factory = new Mock<IFactory>();
            m_factory.Setup(x => x.CustomStreamReader).Returns(m_streamReader.Object);

            m_fileParser = new FileParser(m_factory.Object);
        }

        [TestMethod]
        public void ParseFile_WithCorrectData_ShouldReturnListofPeople()
        {
            var result=  m_fileParser.ParseFile("Comma");
            result.Count.ShouldBeEqualTo(1);
            result[0].LastName.ShouldBeEqualTo("Foster");
            result[0].FirstName.ShouldBeEqualTo("Thomas");
            result[0].Gender.ShouldBeEqualTo("Male");
            result[0].FavoriteColor.ShouldBeEqualTo("Khaki");
            result[0].DateofBirth.ShouldBeEqualTo(Convert.ToDateTime("3/1/1836"));
        }

        [TestMethod]
        public void DetermineDelimiterType_Should_ReturnComma()
        {
            var result = m_fileParser.DetermineDelimiterType();
            result.ShouldBeEqualTo("Comma");
        }
    }
}
