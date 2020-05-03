using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.JSAPI
{
    public class BasicConsoleInterface
    {
        [JsonRequired]
        internal string Buffer { get; set; } = "";

        public void Log(object o)
        {
            Buffer += $"{o}\r\n";
        }

        public void Clear()
        {
            Buffer = "";
        }
    }
}
