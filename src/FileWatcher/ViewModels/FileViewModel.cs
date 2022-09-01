using FileWatcher.Domain;
using FileWatcher.ViewModels.Commands;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileWatcher.ViewModels
{
    internal class FileViewModel : ViewModelBase
    {
        private readonly FileModel _file;

        public FileViewModel(FileModel file)
        {
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
        public int Size
        {
            get
            {
                if (_file.IsDirectory)
                {
                    return 0;
                }
                return _file.Size;
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

        private Task DoubleClickHandler()
        {
            return Task.Run(() => 
            {
                if (File.Exists(_file.Path))
                {
                    Process.Start("explorer.exe", _file.Path);
                }
            });
        }
    }
}
