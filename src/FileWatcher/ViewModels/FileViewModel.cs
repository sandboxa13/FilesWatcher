using FileWatcher.Domain;
using FileWatcher.Logic;
using FileWatcher.ViewModels.Commands;
using System.Windows.Input;

namespace FileWatcher.ViewModels
{
    internal class FileViewModel : ViewModelBase
    {
        private readonly FileSystem _fileSystem;
        private readonly FileModel _file;

        public FileViewModel(FileSystem fileSystem, FileModel file)
        {
            _fileSystem = fileSystem;
            _file = file;

            DoubleClickCommand = new RelayCommand(o => { DoubleClickHandler(); }, o => true);
        }

        public ICommand DoubleClickCommand { get; set; }

        public string Name
        {
            get
            {
                return _file.Name;
            }
        }
        public string Size
        {
            get
            {
                if (_file.IsDirectory)
                {
                    return "Folder";
                }
                return _file.Size.ToString();
            }
        }
        public string Path
        {
            get
            {
                return _file.Path;
            }
        }

        public string LastWriteTime
        {
            get
            {
                return _file.LastWriteTime;
            }
        }

        private void DoubleClickHandler()
        {
            _fileSystem.Open(_file);
        }
    }
}
