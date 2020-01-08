﻿using Jint;
using LanguageServer.RuntimeState;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.Services
{
    public class CSV8RuntimeService
    {

        Dictionary<int, CSV8Program> programs = new Dictionary<int, CSV8Program>();
        
        public CSV8RuntimeService()
        {
        }

        public V8ScriptEngine GetEngine() =>
            new V8ScriptEngine();


        public CSV8Program CreateProgram()
        {
            var newProgram = new CSV8Program(this);
            newProgram.Id = GetAvaivableId();
            programs.Add(newProgram.Id, newProgram);
            return newProgram;
        }

        public CSV8Program GetProgram(int id)
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
