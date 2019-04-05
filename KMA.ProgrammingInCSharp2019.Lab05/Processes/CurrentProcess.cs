using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcess
    {
        private readonly Process _process;

        public string Name
        {
            get { return _process.ProcessName; }
        }

        public int Id
        {
            get { return _process.Id; }
        }

        public bool IsActive
        {
            get { return _process.Responding;}
        }

        public float CPU
        {
            get
            {
                PerformanceCounter performance = new PerformanceCounter("Process", "% Processor Time",_process.ProcessName);
                return performance.NextValue() / 100;
            }
        }
        public long MemoryUsage
        {
            get { return _process.PrivateMemorySize64 / 1024; }
        }

        public int ThreadsCount
        {
            get { return _process.Threads.Count; }
        }

        public string User
        {
            get { return _process.MachineName; }
        }

        public string Path
        {
            get
            {
                try
                {
                    return _process.MainModule.FileName;
                }
                catch (Exception e)
                {
                    return "Not available";
                }
                
            }
        }

        public String LaunchTime
        {
            get {
                try
                {
                    return _process.StartTime.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    return "Not available";
                }
            }
        }

        internal CurrentProcess(Process process)
        {
                _process = process;
        }

        public void Terminate()
        {
            _process.Kill();
        }

        private HashSet<CurrentProcessModule> _modules;

        public HashSet<CurrentProcessModule> Modules
        {
            get
            {
                if (_modules == null)
                    RefreshModules();
                return _modules;
            }
        }

        private HashSet<CurrentProcessThread> _threads;

        public HashSet<CurrentProcessThread> Threads
        {
            get
            {
                if (_threads == null)
                    RefreshThreads();
                return _threads;
            }
        }

        private void RefreshModules()
        {
            if (_modules == null)
                _modules = new HashSet<CurrentProcessModule>();
            foreach (ProcessModule m in _process.Modules)
            {
                _modules.Add(new CurrentProcessModule(m));
            }
        }

        private void RefreshThreads()
        {
            if (_threads == null)
                _threads = new HashSet<CurrentProcessThread>();
            foreach (ProcessThread t in _process.Threads)
            {
                _threads.Add(new CurrentProcessThread(t));
            }
        }

    }
}
