﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KMA.ProgrammingInCSharp2019.Lab05.Processes;

namespace KMA.ProgrammingInCSharp2019.Lab05
{
    class ProcessViewModel : INotifyPropertyChanged
    {
        private string _sortEntry;

        private Visibility _loaderVisibility = Visibility.Hidden;
        private Visibility _dataGridVisibility = Visibility.Visible;

        private RelayCommand<object> _terminate;
        private RelayCommand<object> _openFolder;
        private RelayCommand<object> _sort;

        private Thread _workingThread1;
        private Thread _workingThread2;



        private ObservableCollection<CurrentProcess> _processes;
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _tokenSource;

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
            get
            {
                return _processes;
            }
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility DataGridVisibility
        {
            get { return _dataGridVisibility; }
            set
            {
                _dataGridVisibility = value;
                OnPropertyChanged();
            }
        }

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
            _workingThread1 = new Thread(WorkingThreadProcess1);
            _workingThread1.Start();
            _workingThread2 = new Thread(WorkingThreadProcess2);
            _workingThread2.Start();
        }

        private void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread1.Join(100);
            _workingThread1.Abort();
            _workingThread1 = null;
            _workingThread2.Join(100);
            _workingThread2.Abort();
            _workingThread2 = null;
        }

        private void WorkingThreadProcess1()
        {
            while (!_token.IsCancellationRequested)
            {
                Thread.Sleep(5000); 
                
                ProcessManager.Refresh(); 
                if (_token.IsCancellationRequested)
                    break;

                Processes=new ObservableCollection<CurrentProcess>(ProcessManager.ProcessesList);
                
            }
        }

        private void WorkingThreadProcess2()
        {
            while (!_token.IsCancellationRequested)
            {
                Thread.Sleep(2000);

                ProcessManager.RefreshMeta();
                if (_token.IsCancellationRequested)
                    break;

                Processes = new ObservableCollection<CurrentProcess>(ProcessManager.ProcessesList);

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

        private void TerminateImplementation(object obj)
        {
            if (SelectedProcess.Path != "Not available")
            {
                SelectedProcess.Terminate();
            }
            else
            {
                MessageBox.Show("Can`t commit this");
            }
        }

        private void OpenFolderImplementation(object obj)
        {
            if (SelectedProcess.Path != "Not available")
            {
                System.Diagnostics.Process.Start("explorer", SelectedProcess.Path.Substring(0,SelectedProcess.Path.LastIndexOf("\\", StringComparison.Ordinal)));
            }
            else
            {
                MessageBox.Show("Can`t commit this");
            }
        }

        private async void SortImplementation(object obj)
        {
            LoaderVisibility = Visibility.Visible;
            DataGridVisibility = Visibility.Hidden;
            ProcessManager.SortCategory = SortEntry.Substring(SortEntry.LastIndexOf(":", StringComparison.Ordinal) + 2);
            await Task.Run(() => UpdateList(), _token);
            LoaderVisibility = Visibility.Hidden;
            DataGridVisibility = Visibility.Visible;
        }
  

        private void UpdateList()
        {
            ProcessManager.Refresh();
            Processes = new ObservableCollection<CurrentProcess>(ProcessManager.ProcessesList);
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
