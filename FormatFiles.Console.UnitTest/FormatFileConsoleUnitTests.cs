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
        private Mock<IUserInput> m_userInput;
        private BootStrapper bootStrapper;
        [TestInitialize]
        public void TestInitialize()
        {
            m_streamReader = new Mock<IStreamReader>();
            m_streamReader.Setup(x => x.ReadtoEnd()).Returns("Foster,Thomas,Male,Khaki,3/1/1836\r\nLewis,Kathryn,Female,Green,11/14/1855");
            m_streamReader.SetupSequence(x => x.ReadLine()).Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns("Foster,Thomas,Male,Khaki,3/1/1836").Returns(null);
            m_streamReader.Setup(x => x.SetupStreamReaderWrapper(It.IsAny<string>())).Returns(new StreamReader(new MemoryStream(Encoding.Default.GetBytes("33\r\n1\r\n16\r\n5\r\n7"))));
            m_fileParser = new FileParser("whatever")
            {
                StreamReader = m_streamReader.Object
            };

            m_userInput = new Mock<IUserInput>();
            m_userInput.Setup(x => x.GetInput()).Returns("gender");
            bootStrapper = new BootStrapper();
        }

        #region Unittest need to work more

        [TestMethod]
        public void BootStrapper_WithCorrectPath_ShouldReturn_ListofObject()
        {
            try
            {
                bootStrapper.Sort("gender");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BootStrapper_SortByBirth_ShouldNotThrow()
        {
            try
            {
                bootStrapper.Sort("birth");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BootStrapper_SortByLastName_ShouldNotThrow()
        {
            try
            {
                bootStrapper.Sort("lastname");
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            Assert.IsTrue(true);
        }
        #endregion

    }
}
