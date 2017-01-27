using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FormatFiles.API.UnitTest
{
    [TestClass]
    public class RecordsControllerUnitTests
    {
        private FileParser m_fileParser;
        private Mock<IStreamReader> m_streamReader;

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
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
