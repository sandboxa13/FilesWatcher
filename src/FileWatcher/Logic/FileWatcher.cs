using System;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using FileWatcher.Domain;
using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class FilesWatcher
    {
        private readonly ISubject<FileModel[]> _filesChangedSubject = new Subject<FileModel[]>();

        private FileModel[] _files;
        private IntPtr _fileWatcher;
        private GuiUpdateDelegate _guiUpdateCallback;

        public FilesWatcher()
        {
            _guiUpdateCallback = new GuiUpdateDelegate(GuiUpdateHandler);
        }

        public string CurrentPath { get; private set; }
        public IObservable<FileModel[]> FilesChanged => _filesChangedSubject;

        public void Dispose()
        {
            FileWatcherInterop.dispose_file_watcher(_fileWatcher);
            _fileWatcher = IntPtr.Zero;
        }

        public void StartWatch(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            if (_fileWatcher != IntPtr.Zero)
            {
                FileWatcherInterop.dispose_file_watcher(_fileWatcher);
            }

            _fileWatcher = FileWatcherInterop.create_file_watcher(_guiUpdateCallback, path);
        }

        private void GuiUpdateHandler(IntPtr files, int count)
        {
            if (count == 0)
                return;

            if (_files == null || _files.Length != count)
                _files = new FileModel[count];

            for (int i = 0; i < count; i++)
                _files[i] = (FileModel)Marshal.PtrToStructure(files + i * Marshal.SizeOf<FileModel>(), typeof(FileModel));

            _filesChangedSubject.OnNext(_files);
        }
    }
}
