using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KMA.ProgrammingInCSharp2019.Lab05.Processes;

namespace KMA.ProgrammingInCSharp2019.Lab05
{
    internal static class ProcessManager
    {
        public static event Action StopThreads;

        private static List<CurrentProcess> _processesList;

        internal static List<CurrentProcess> ProcessesList
        {
            get { return _processesList; }
        }

        internal static void Initialize()
        {
            _processesList = new List<CurrentProcess>();
            Refresh();
        }


        internal static void Refresh()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process != null)
                    if(!_processesList.Contains(new CurrentProcess(process)))
                    _processesList.Add(new CurrentProcess(process));
            }
        }

        internal static void CloseApp()
        {
            MessageBox.Show("Shutting down");
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
