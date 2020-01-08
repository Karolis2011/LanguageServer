using JavaScriptEngineSwitcher.ChakraCore;
using LanguageServer.RuntimeState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.Services
{
    public class ChakraRuntimeService
    {

        Dictionary<int, ChakraProgram> programs = new Dictionary<int, ChakraProgram>();
        
        public ChakraRuntimeService()
        {
        }

        public ChakraCoreJsEngine GetEngine() =>
            new ChakraCoreJsEngine(new ChakraCoreSettings() {
            });


        public ChakraProgram CreateProgram()
        {
            var newProgram = new ChakraProgram(this);
            newProgram.Id = GetAvaivableId();
            programs.Add(newProgram.Id, newProgram);
            return newProgram;
        }

        public ChakraProgram GetProgram(int id)
        {
            return programs.GetValueOrDefault(id);
        }

        public void DeleteProgram(int id)
        {
            programs.Remove(id);
        }

        private int GetAvaivableId()
        {
            var maxID = programs.Keys.Count > 0 ? programs.Keys.Max() : 0;
            var potential = Enumerable.Range(0, maxID + 2);
            return potential.Except(programs.Keys).First();
        }
    }
}
