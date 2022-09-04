using System;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using FileWatcher.Domain;
using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class FilesWatcher
    {
        private readonly ISubject<FileModel[]> _filesUpdatedSubject;

        private FileModel[] _files;
        private IntPtr _fileWatcherPtr;
        private FilesUpdateDelegate _filesUpdateCallback;

        public FilesWatcher()
        {
            _filesUpdateCallback = new FilesUpdateDelegate(FilesUpdateHandler);
            _filesUpdatedSubject = new Subject<FileModel[]>();

            _fileWatcherPtr = FileWatcherInterop.create_file_watcher(_filesUpdateCallback);
        }

        public string CurrentPath { get; private set; }
        public IObservable<FileModel[]> FilesUpdated => _filesUpdatedSubject;

        public void Dispose()
        {
            FileWatcherInterop.dispose_file_watcher(_fileWatcherPtr);
            _fileWatcherPtr = IntPtr.Zero;
        }

        public void Stop()
        {
            if (_fileWatcherPtr != IntPtr.Zero)
            {
                FileWatcherInterop.stop(_fileWatcherPtr);
            }
        }

        public void Observe(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            CurrentPath = path;

            if (_fileWatcherPtr != IntPtr.Zero)
            {
                FileWatcherInterop.observe(_fileWatcherPtr, path);
            }
        }

        private void FilesUpdateHandler(IntPtr files, int count)
        {
            if (count == 0)
                return;

            if (_files == null || _files.Length != count)
                _files = new FileModel[count];

            for (int i = 0; i < count; i++)
                _files[i] = (FileModel)Marshal.PtrToStructure(files + i * Marshal.SizeOf<FileModel>(), typeof(FileModel));

            _filesUpdatedSubject.OnNext(_files);
        }
    }
}
