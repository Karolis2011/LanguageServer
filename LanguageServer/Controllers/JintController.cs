using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LanguageServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LanguageServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JintController : ControllerBase
    {
        JintRuntimeService jintRuntime;

        public JintController(JintRuntimeService _jr)
        {
            jintRuntime = _jr;
        }



        [HttpGet("Version")]
        public object Version()
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(typeof(Jint.Engine).Assembly.Location);
            FileVersionInfo fvi2 = FileVersionInfo.GetVersionInfo(typeof(JintController).Assembly.Location);
            return new
            {
                Message = $"NTSLv3 running Jint {fvi.FileVersion}",
                Version = 3,
                ServerVersion = fvi2.FileVersion,
                JintVersion = fvi.FileVersion
            };
        }

        [HttpPost("New")]
        public object InitializeProgram()
        {
            var program = jintRuntime.CreateProgram();
            return new
            {
                Message = $"Created a new program with ID {program.Id}",
                Program = program
            };
        }

        [HttpGet("{id:int:min(0)}")]
        public object GetProgram(int id)
        {
            var program = jintRuntime.GetProgram(id);
            return new
            {
                Message = $"Program has been sucessfully retireved.",
                Program = program
            };
        }

        [HttpPost("{id:int:min(0)}/exec")]
        public object ExecuteProgram(int id)
        {
            var source = Request.Form["code"];
            if (string.IsNullOrEmpty(source))
                return BadRequest();
            var program = jintRuntime.GetProgram(id);
            program.Execute(source);
            return new
            {
                Message = $"Executed provided code.",
                Program = program
            };
        }

        [HttpPost("{id:int:min(0)}/delete")]
        public object DeleteProgram(int id)
        {
            jintRuntime.DeleteProgram(id);
            return new
            {
                Message = $"Program has been removed from system.",
            };
        }
    }
}