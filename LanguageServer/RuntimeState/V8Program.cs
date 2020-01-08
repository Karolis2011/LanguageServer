﻿using LanguageServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V8.Net;

namespace LanguageServer.RuntimeState
{
    public class V8Program
    {
        V8Engine engine;
        public int Id { get; set; }
        public string ConsoleBuffer { get; protected set; } = "";

        public V8Program(V8RuntimeService service)
        {
            engine = service.GetEngine();
            InstallExtensions();
        }

        public void InstallExtensions()
        {
            //engine.CreateFunctionTemplate().
            //engine.GlobalObject.SetProperty()
            //engine.DynamicGlobalObject.__cb_log = new Action<object>(LogCB);
        }

        public void Execute(string source)
        {
            try
            {
                engine.Execute(source ,"", true, 2);
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
