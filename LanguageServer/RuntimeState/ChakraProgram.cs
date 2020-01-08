using JavaScriptEngineSwitcher.ChakraCore;
using LanguageServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.RuntimeState
{
    public class ChakraProgram
    {
        ChakraCoreJsEngine engine;
        public int Id { get; set; }
        public string ConsoleBuffer { get; protected set; } = "";

        public ChakraProgram(ChakraRuntimeService service)
        {
            engine = service.GetEngine();
            InstallExtensions();
        }

        public void InstallExtensions()
        {
            engine.EmbedHostObject("__cb_log", new Action<object>(LogCB));
        }

        public void Execute(string source)
        {
            try
            {
                engine.TimedExecute(new TimeSpan(0, 0, 1), source);
            }
            catch (Exception ex)
            {
                LogCB(ex);
            }

        }

        private void LogCB(object o)
        {
            ConsoleBuffer += $"{o}\r\n";
        }
    }
}
