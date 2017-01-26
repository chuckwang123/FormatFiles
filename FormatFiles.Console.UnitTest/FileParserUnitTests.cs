using System.IO;
using FormatFiles.Console.Interfaces;
using FormatFiles.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FormatFiles.Console.UnitTest
{
    [TestClass]
    public class FileParserUnitTests
    {
        private FileParser m_fileParser;
        private Mock<TextReader> m_textReader;
        private Mock<IFileOpener> m_fileOpener;   

        [TestInitialize]
        public void TestInitialize()
        {
            m_fileOpener = new Mock<IFileOpener>();

           // m_fileOpener.Setup(x => x.OpenFile(It.IsAny<string>())).Returns(TextReader.Null);

            m_textReader = new Mock<TextReader>();
            m_textReader.Setup(m => m.ReadLine()).Returns("Brown Peter Male Pink 9/29/1998");

            m_fileParser = new FileParser(m_textReader.Object) {fileOpener = m_fileOpener.Object};
        }

        [TestMethod]
        public void ParseFile_WithCorrectData_ShouldReturnListofPeople()
        {
            var result=  m_fileParser.ParseFile("", ' ');

        }
    }
}
