using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KMA.ProgrammingInCSharp2019.Lab05.Processes;

namespace KMA.ProgrammingInCSharp2019.Lab05
{
    internal static class ProcessManager
    {
        private static string _sortCategory;

        public static event Action StopThreads;

        private static List<CurrentProcess> _processesList;

        internal static List<CurrentProcess> ProcessesList
        {
            get { return _processesList; }
        }

        internal static string SortCategory
        {
            get { return _sortCategory;}
            set { _sortCategory = value; }
        }

        internal static void Initialize()
        {
            _processesList = new List<CurrentProcess>();
            _sortCategory = "Name";
            Refresh();
            RefreshMeta();
        }

        internal static void Refresh()
        {
            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process != null)
                {
                    try
                    {
                        if (!Contains(new CurrentProcess(process)))
                            _processesList.Add(new CurrentProcess(process));
                    }
                    catch (Exception e)
                    {
                        Refresh();
                    }
                    
                }
                    
            }
            Sort();
        }

        internal static void RefreshMeta()
        {
            foreach (var process in _processesList)
            {
                process.RefreshMetaData();
            }
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
            if (_sortCategory != "")
            {
                if (_sortCategory == "Name")
                {
                    _processesList =
                       (from currentProcess in _processesList
                        orderby currentProcess.Name
                        select currentProcess).ToList();

                }

                else if (_sortCategory == "Id")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.Id
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "Active")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.IsActive
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "CPU usage")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.CPU
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "Memory usage")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.MemoryUsage
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "Threads count")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.ThreadsCount
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "User")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.User
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "Path")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.Path
                            select currentProcess).ToList();
                }

                else if(_sortCategory == "Launch time")
                {
                    _processesList =
                        (from currentProcess in _processesList
                            orderby currentProcess.LaunchTime
                            select currentProcess).ToList();
                }

            }
            
        }

        internal static void CloseApp()
        {
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}
