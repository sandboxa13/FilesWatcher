using FileWatcher.Domain;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace FileWatcher.Logic
{
    internal class FileSystem
    {
        private readonly PermissionsChecker _permissionsChecker;
        private readonly PermissionsElevator _permissionsElevator;
        private readonly FilesWatcher _filesWatcher;
        private readonly ISubject<string> _directoryChangedSubjet;

        public FileSystem()
        {
            _permissionsChecker = new PermissionsChecker();
            _permissionsElevator = new PermissionsElevator();
            _filesWatcher = new FilesWatcher();

            _directoryChangedSubjet = new Subject<string>();
        }

        public string CurrentDirectory => _filesWatcher.CurrentPath;
        public IObservable<FileModel[]> FilesUpdated => _filesWatcher.FilesUpdated;
        public IObservable<string> DirectoryChanged => _directoryChangedSubjet;
        
        public void RunAsAdmin() => _permissionsElevator.RunAsAdmin();
        public bool IsUserAdmin() => _permissionsChecker.IsAdmin;
        public void Dispose() => _filesWatcher.Dispose();

        public void ObserveDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            _filesWatcher.Stop();
            _filesWatcher.Observe(path);

            _directoryChangedSubjet.OnNext(path);
        }

        public void Open(FileModel file)
        {
            if (file.Path == null)
                return;

            if (file.IsDirectory)
            {
                ObserveDirectory(file.Path);
            }
            else
            {
                Task.Run(() =>
                {
                    if (File.Exists(file.Path))
                    {
                        Process.Start("explorer.exe", file.Path);
                    }
                }).ConfigureAwait(false);
            }
        }
    }
}
