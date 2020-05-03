using JavaScriptEngineSwitcher.ChakraCore;
using LanguageServer.JSAPI;
using LanguageServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.RuntimeState
{
    public class ChakraBaseProgram
    {
        ChakraCoreJsEngine engine;
        public int Id { get; set; }
        public BasicConsoleInterface Console { get; set; }

        public ChakraBaseProgram(ChakraRuntimeService service)
        {
            engine = service.GetEngine();
            Console = new BasicConsoleInterface();
            InitializeEngine();
        }

        public void InitializeEngine()
        {
            // Todo add proper logging
            engine.EmbedHostObject("Console", Console);
        }

        public void Execute(string source)
        {
            try
            {
                engine.TimedExecute(new TimeSpan(0, 0, 1), source);
            }
            catch (Exception ex)
            {
                Console.Log(ex);
            }

        }
    }
}
