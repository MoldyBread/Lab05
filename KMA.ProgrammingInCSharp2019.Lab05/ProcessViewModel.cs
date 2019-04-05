﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using KMA.ProgrammingInCSharp2019.Lab05.Processes;

namespace KMA.ProgrammingInCSharp2019.Lab05
{
    class ProcessViewModel : INotifyPropertyChanged
    {
        private string _sortEntry;

        private RelayCommand<object> _terminate;
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _sort;

        private ObservableCollection<CurrentProcess> _processes;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;

        public CurrentProcess SelectedProcess { get;set; }

        public string SortEntry
        {
            get { return _sortEntry; }
            set
            {
                _sortEntry = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CurrentProcess> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

       

        private Thread _workingThread;

        internal ProcessViewModel()
        {
            _processes =new ObservableCollection<CurrentProcess>(ProcessManager.ProcessesList);
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            ProcessManager.StopThreads += StopWorkingThread;
        }

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }

        private void WorkingThreadProcess()
        {
            while (!_token.IsCancellationRequested)
            {
                Thread.Sleep(5000);
                
                ProcessManager.Refresh();
                if (_token.IsCancellationRequested)
                    break;
                else
                {
                    Processes=new ObservableCollection<CurrentProcess>(ProcessManager.ProcessesList);
                }
                
            }
        }

        public RelayCommand<object> Sort
        {
            get
            {
                return _sort ?? (_sort = new RelayCommand<object>(
                           SortImplementation));
            }
        }

        public RelayCommand<object> Terminate
        {
            get
            {
                return _terminate ?? (_terminate = new RelayCommand<object>(
                           TerminateImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<Object> OpenFolder
        {
            get
            {
                return _openFolder ?? (_openFolder= new RelayCommand<object>(
                           OpenFolderImplementation,o => CanExecuteCommand()));
            }
        }

        private bool CanExecuteCommand()
        {
            return SelectedProcess != null;
        }

        public void TerminateImplementation(object obj)
        {
            if (SelectedProcess.Path != "Not available")
            {
                SelectedProcess.Terminate();
                ProcessManager.Refresh();
            }
            else
            {
                MessageBox.Show("Can`t commit this");
            }
        }

        public void OpenFolderImplementation(object obj)
        {
            if (SelectedProcess.Path != "Not available")
            {
                System.Diagnostics.Process.Start("explorer", SelectedProcess.Path.Substring(0,SelectedProcess.Path.LastIndexOf("\\")));

            }
            else
            {
                MessageBox.Show("Can`t commit this");
            }
        }

        public void SortImplementation(object obj)
        {
            //TODO: Make show loader
           ProcessManager.SortCategory=SortEntry.Substring(SortEntry.LastIndexOf(":") + 2);
           ProcessManager.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
