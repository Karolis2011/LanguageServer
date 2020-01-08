﻿using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.Services
{
    public class JintRuntimeService
    {

        Dictionary<int, RuntimeState.JintProgram> programs = new Dictionary<int, RuntimeState.JintProgram>();
        
        public JintRuntimeService()
        {
            
        }

        public Engine GetEngine() =>
            new Engine(o => o
            .LimitRecursion(25)
            .MaxStatements(1000)
            .TimeoutInterval(new TimeSpan(0, 0, 2))
        );

        public RuntimeState.JintProgram CreateProgram()
        {
            var newProgram = new RuntimeState.JintProgram(this);
            newProgram.Id = GetAvaivableId();
            programs.Add(newProgram.Id, newProgram);
            return newProgram;
        }

        public RuntimeState.JintProgram GetProgram(int id)
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
