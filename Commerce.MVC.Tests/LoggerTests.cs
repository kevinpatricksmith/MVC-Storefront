using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for LoggerTests
    /// </summary>
    [TestClass]
    public class LoggerTests:TestBase {


        [TestMethod]
        public void Logger_Service_Should_Log_Error_Info_Warn_Fatal_Debug() {

            TestLogger logger = new TestLogger();
            logger.Debug("debug this");
            logger.Error("this is an error");
            logger.Error(new Exception("exceptional"));
            logger.Fatal("fatal error");
            logger.Warn("warning!");
            logger.Info("info for you");

            Dictionary<string, string> logs = logger.Logs;
            Assert.AreEqual("debug this", logs["debug"]);
            Assert.AreEqual("this is an error", logs["error"]);
            Assert.AreEqual("exceptional", logs["exception"]);
            Assert.AreEqual("fatal error", logs["fatal"]);
            Assert.AreEqual("warning!", logs["warn"]);
            Assert.AreEqual("info for you", logs["info"]);

        }



    }
}
