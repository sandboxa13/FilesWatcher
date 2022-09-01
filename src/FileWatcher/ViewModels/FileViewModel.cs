using FileWatcher.Domain;
using FileWatcher.ViewModels.Commands;
using System.Threading.Tasks;

namespace FileWatcher.ViewModels
{
    internal class FileViewModel : ViewModelBase
    {
        private readonly File _file;

        public FileViewModel(File file)
        {
            _file = file;

            DoubleClickCommand = new AsyncCommand(async () => { await DoubleClickHandler(); });
        }

        public IAsyncCommand DoubleClickCommand { get; set; }

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

        private async Task DoubleClickHandler()
        {

        }
    }
}
