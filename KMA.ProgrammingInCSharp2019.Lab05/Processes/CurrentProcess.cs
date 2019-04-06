using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcess
    {

        private float _cpu;
        private long _memoryUsage;
        private int _threadsCount;

        private readonly PerformanceCounter _cpuCount;
        private readonly PerformanceCounter _ramInMBCount;


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
                return _cpu;
            }
            set { _cpu = value; }
        }
        public long MemoryUsage
        {
            get { return _memoryUsage; }
            set { _memoryUsage = value; }
        }

        public int ThreadsCount
        {
            get { return _threadsCount; }
            set { _threadsCount = value; }
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
                catch (Exception)
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
                catch (Exception)
                {
                    return "Not available";
                }
            }
        }

        internal CurrentProcess(Process process)
        {
                _process = process;
            //ThreadsCount = _process.Threads.Count;
            //MemoryUsage = _process.PrivateMemorySize64 / 1024;
            //PerformanceCounter performance = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);
            //CPU=performance.NextValue() / 100;
            _cpuCount = new PerformanceCounter("Process", "% Processor Time", Name, true);
            _ramInMBCount = new PerformanceCounter("Process", "Working Set", Name, true);
            RefreshMetaData();
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
                RefreshModules();
                return _modules;
            }
        }



        private HashSet<CurrentProcessThread> _threads;

        public HashSet<CurrentProcessThread> Threads
        {
            get
            {
                RefreshThreads();
                return _threads;
            }
        }

        private void RefreshModules()
        {
            if (_modules == null)
                _modules = new HashSet<CurrentProcessModule>();

            try
            {
                ProcessModuleCollection processModules = _process.Modules;
                for (int i = 0; i < processModules.Count; i++)
                {
                    _modules.Add(new CurrentProcessModule(processModules[i]));
                }
            }
            catch (Exception)
            {
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



        internal void RefreshMetaData()
        {
            try
            {
                CPU = _cpuCount.NextValue() / Environment.ProcessorCount;
            }
            catch (InvalidOperationException) { }

            try
            {
                MemoryUsage = Convert.ToInt32(_ramInMBCount.NextValue()) / (1024 * 1024);
            }
            catch (InvalidOperationException) { }
            try
            {
                ThreadsCount = Process.GetProcessById(Id).Threads.Count;
            }
            catch (Exception) { }

        }
    }
}
