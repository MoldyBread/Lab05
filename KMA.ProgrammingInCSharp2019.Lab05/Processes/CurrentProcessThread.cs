using System;
using System.Diagnostics;


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
        public String LaunchTime
        {
            get
            {
                try
                {
                    return _thread.StartTime.ToString();
                }
                catch (Exception)
                {
                    return "Not available";
                }
                
            }
        }

        internal CurrentProcessThread(ProcessThread thread)
        {
            _thread = thread;
        }
    }
}
