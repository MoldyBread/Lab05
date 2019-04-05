using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcess
    {
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _tokenSource;

        private Thread _workingThread;

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
                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;
                StartWorkingThread();
                ProcessManager.StopThreads += StopWorkingThread;
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
                //RefreshModules();
                return _modules;
            }
        }



        private HashSet<CurrentProcessThread> _threads;

        public HashSet<CurrentProcessThread> Threads
        {
            get
            {
                //RefreshThreads();
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

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(100);
            _workingThread.Abort();
            _workingThread = null;
        }

        private void WorkingThreadProcess()
        {
            while (!_token.IsCancellationRequested)
            {
               
                RefreshModules();
                RefreshThreads();
                if (_token.IsCancellationRequested)
                    break;

                Thread.Sleep(2000);
            }
        }

    }
}
