using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcessThread
    {
        private readonly ProcessThread _thread;

        public int Id
        {
            get { return _thread.Id; }
        }
        public ThreadState State
        {
            get { return _thread.ThreadState; }
        }
        public DateTime LaunchTime
        {
            get { return _thread.StartTime; }
        }

        internal CurrentProcessThread(ProcessThread thread)
        {
            _thread = thread;
        }
    }
}
