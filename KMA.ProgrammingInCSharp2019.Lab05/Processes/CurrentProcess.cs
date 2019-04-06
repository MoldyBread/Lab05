using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace KMA.ProgrammingInCSharp2019.Lab05.Processes
{
    class CurrentProcess
    {

        private float _cpu;
        private long _memoryUsage;
        private int _threadsCount;

        private readonly PerformanceCounter _cpuPerformance;
        private readonly PerformanceCounter _ramPerformance;


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
            get { return GetProcessUser(_process); }
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
            _cpuPerformance = new PerformanceCounter("Process", "% Processor Time", Name, true);
            _ramPerformance = new PerformanceCounter("Process", "Working Set", Name, true);
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
                CPU = _cpuPerformance.NextValue() / Environment.ProcessorCount;
            }
            catch (Exception) { }

            try
            {
                MemoryUsage = Convert.ToInt32(_ramPerformance.NextValue()) / (1024 * 1024);
            }
            catch (Exception) { }
            try
            {
                ThreadsCount = Process.GetProcessById(Id).Threads.Count;
            }
            catch (Exception) { }

        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return "Not available";
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}
