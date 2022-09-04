using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using FileWatcher.Logic;
using FileWatcher.ViewModels.Commands;
using FileWatcher.Domain;

namespace FileWatcher.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly FileSystem _fileSystem;

        private string _currentPath;
        private bool _isAdmin;
        private ObservableCollection<FileViewModel> _files = new();

        public MainWindowViewModel(FileSystem fileSystem)
        {
            _fileSystem = fileSystem;


            OpenFolderSelectWindowCommand = new RelayCommand(o => { OpenFolderSelectWindowHandler(); }, o => true);
            RunAsAdminCommand = new RelayCommand(obj => { RunAsAdminHandler(); }, o => !IsAdmin);

            IsAdmin = _fileSystem.IsUserAdmin();

            _disposables.Add(_fileSystem.FilesUpdated.Subscribe(OnFilesChanged));
            _disposables.Add(_fileSystem.DirectoryChanged.Subscribe(OnDirectoryChanged));
        }

        public ICommand OpenFolderSelectWindowCommand { get; set; }
        public ICommand RunAsAdminCommand { get; set; }

        public string CurrentPath
        {
            get
            {
                return _fileSystem.CurrentDirectory;
            }
            set
            {
                if (value != _currentPath)
                {
                    _currentPath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                if (value != _isAdmin)
                {
                    _isAdmin = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ObservableCollection<FileViewModel> Files
        {
            get => _files;
            set
            {
                if (_files != value)
                {
                    _files = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override void Dispose()
        {
            _fileSystem.Dispose();
            _disposables.Dispose();
        }

        private void OnFilesChanged(FileModel[] files)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Files.Clear();

                Files = new ObservableCollection<FileViewModel>(files.Select(file => new FileViewModel(_fileSystem, file)));
            }));
        }

        private void OnDirectoryChanged(string path)
        {
            CurrentPath = path; 
        }

        private void OpenFolderSelectWindowHandler()
        {
            using var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                _fileSystem.ObserveDirectory(dialog.SelectedPath);

                CurrentPath = _fileSystem.CurrentDirectory;
            }
        }

        private void RunAsAdminHandler()
        {
            if (IsAdmin)
                return;

            _fileSystem.RunAsAdmin();
        }
    }
}
