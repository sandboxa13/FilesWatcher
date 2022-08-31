using System;
using System.Windows.Forms;
using System.Windows.Input;
using System.Reactive.Linq;
using FileWatcher.Logic;
using FileWatcher.ViewModels.Commands;
using FileWatcher.Domain;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;

namespace FileWatcher.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly PermissionsElevator _permissionsElevator;
        private readonly FilesWatcher _filesWatcher;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private string _currentPath;
        private bool _isAdmin;
        private ObservableCollection<FileViewModel> _files = new();

        public MainWindowViewModel(
            PermissionsChecker permissionsChecker,
            PermissionsElevator permissionsElevator,
            FilesWatcher filesWatcher)
        {
            _permissionsElevator = permissionsElevator;
            _filesWatcher = filesWatcher;

            OpenFolderSelectWindowCommand = new RelayCommand(o => { OpenFolderSelectWindowHandler(); }, o => true);
            RunAsAdminCommand = new RelayCommand(obj => { RunAsAdminHandler(); }, o => !IsAdmin);

            IsAdmin = permissionsChecker.IsAdmin;

            _disposables.Add(_filesWatcher.FilesChanged.Subscribe(OnFilesChanged));
        }

        public ICommand OpenFolderSelectWindowCommand { get; set; }
        public ICommand RunAsAdminCommand { get; set; }

        public string CurrentPath
        {
            get
            {
                return _currentPath;
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
            _filesWatcher.Dispose();
            _disposables.Dispose();
        }

        private void OnFilesChanged(File[] files)
        {
            Files.Clear();

            Files = new ObservableCollection<FileViewModel>(files.Select(file => new FileViewModel(file)));
        }

        private void OpenFolderSelectWindowHandler()
        {
            using var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                CurrentPath = dialog.SelectedPath;

                _filesWatcher.StartWatch(CurrentPath);
            }
        }

        private void RunAsAdminHandler()
        {
            if (IsAdmin)
                return;
            
            _permissionsElevator.RunAsAdmin();
        }
    }
}
