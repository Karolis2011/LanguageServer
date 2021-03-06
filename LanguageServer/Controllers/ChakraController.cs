﻿using System;
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
    public class ChakraController : ControllerBase
    {
        ChakraRuntimeService runtimeService;

        public ChakraController(ChakraRuntimeService _r)
        {
            runtimeService = _r;
        }



        [HttpGet("Version")]
        public object Version()
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(typeof(JavaScriptEngineSwitcher.ChakraCore.ChakraCoreJsEngine).Assembly.Location);
            FileVersionInfo fvi2 = FileVersionInfo.GetVersionInfo(typeof(ChakraController).Assembly.Location);
            var ver = new JavaScriptEngineSwitcher.ChakraCore.ChakraCoreJsEngine().Version;
            return new
            {
                Message = $"NTSLv3 running ChakraCore {ver}",
                Version = 3,
                ServerVersion = fvi2.FileVersion,
                ChakraCoreVersion = ver,
                WraperVersion = fvi.FileVersion
            };
        }

        [HttpPost("New")]
        public object InitializeBaseProgram()
        {
            var program = runtimeService.CreateProgram();
            return new
            {
                Message = $"Created a new program with ID {program.Id}",
                Program = program
            };
        }

        [HttpGet("{id:int:min(0)}")]
        public object GetProgram(int id)
        {
            var program = runtimeService.GetProgram(id);
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
            var program = runtimeService.GetProgram(id);
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
            runtimeService.DeleteProgram(id);
            return new
            {
                Message = $"Program has been removed from system.",
            };
        }
    }
}