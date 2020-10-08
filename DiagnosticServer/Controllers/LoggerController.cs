using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DiagnosticServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggerController : ControllerBase
    {
        private readonly ILogger<LoggerController> _logger;

        public LoggerController(ILogger<LoggerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Logger is ready";
        }

        [HttpPost(Name = "Log")]
        public void Log([FromBody] string message)
        {
            _logger.Log(LogLevel.Information, message);
        }
    }
}
