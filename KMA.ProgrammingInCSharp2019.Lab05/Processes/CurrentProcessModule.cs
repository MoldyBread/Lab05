﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcessModule
    {
        private readonly ProcessModule _module;

        public string Name
        {
            get { return _module.ModuleName; }
        }
        public string Path
        {
            get { return _module.FileName; }
        }

        internal CurrentProcessModule(ProcessModule module)
        {
            this._module = module;
        }
    }
}