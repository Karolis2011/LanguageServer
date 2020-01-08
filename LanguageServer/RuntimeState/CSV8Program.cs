using LanguageServer.Services;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.RuntimeState
{
    public class CSV8Program
    {
        V8ScriptEngine engine;
        public int Id { get; set; }
        public string ConsoleBuffer { get; protected set; } = "";

        public CSV8Program(CSV8RuntimeService service)
        {
            engine = service.GetEngine();
            //mainEngine = service.GetEngine();
            InstallExtensions();
        }

        public void InstallExtensions()
        {
            engine.AddHostObject("__cb_log", new Action<object>(LogCB));
        }

        public void Execute(string source)
        {
            try
            {
                engine.Execute(source);
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
