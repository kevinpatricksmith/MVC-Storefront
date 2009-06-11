using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Services;
using Commerce.Data;

namespace Commerce.Tests {
    public class TestLogger:ILogger {

        public Dictionary<string, string> Logs;

        public TestLogger() {
            Logs = new Dictionary<string, string>();
        }

        public void Info(string message) {
            Logs.Add("info", message);
        }

        public void Warn(string message) {
            Logs.Add("warn", message);
        }

        public void Debug(string message) {
            Logs.Add("debug", message);
        }

        public void Error(string message) {
            Logs.Add("error", message);
        }

        public void Error(Exception x) {
            Logs.Add("exception", x.Message);
        }
        public void Fatal(Exception x) {
            Logs.Add("fatalexception", x.Message);
        }

        public void Fatal(string message) {
            Logs.Add("fatal", message);
        }


    }
}
