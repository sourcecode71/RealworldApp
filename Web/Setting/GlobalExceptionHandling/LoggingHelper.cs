using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;

namespace Web.Setting.GlobalExceptionHandling
{
    public class LoggingHelper
    {
        public static Logger DatabaseLogger { get; set; }
        private readonly IConfiguration _configuration;

        public LoggingHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static Logger GetLogger()
        {
            Logger log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                     .WriteTo.File("logs/log-" + DateTime.Today.ToString("yyyyMMdd") + ".txt").CreateLogger();

            DatabaseLogger = log;
            return log;
        }
    }
}
