using Jint;
using LanguageServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.RuntimeState
{
    public class JintProgram
    {
        Engine mainEngine;
        public int Id { get; set; }
        public string ConsoleBuffer { get; protected set; } = "";

        public JintProgram(JintRuntimeService service)
        {
            mainEngine = service.GetEngine();
            InstallExtensions();
        }

        public void InstallExtensions()
        {
            mainEngine.SetValue("__cb_log", new Action<object>(LogCB));
        }

        public void Execute(string source)
        {
            try
            {
                mainEngine.Execute(source);
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
