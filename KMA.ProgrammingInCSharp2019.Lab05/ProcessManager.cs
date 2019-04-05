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
            CurrentProcess current;
            foreach (var process in Process.GetProcesses())
            {
                if (process != null)
                {
                    if (!Contains(new CurrentProcess(process)))
                        _processesList.Add(new CurrentProcess(process));
                }
                    
            }

            Sort();
        }

        private static bool Contains(CurrentProcess current)
        {
            foreach (var process in _processesList)
            {
                if (process.Id == current.Id)
                    return true;
            }



            return false;
        }

        private static void Sort()
        {
            var result =
                from currentProcess in _processesList
                orderby currentProcess.Name
                select currentProcess;

            _processesList = result.ToList();
        }

        internal static void CloseApp()
        {
            MessageBox.Show("Shutting down");
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
