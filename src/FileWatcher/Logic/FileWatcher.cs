using System;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using FileWatcher.Domain;
using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class FilesWatcher
    {
        private readonly ISubject<File[]> _filesChangedSubject = new Subject<File[]>();

        private File[] _files;
        private IntPtr _fileWatcher;
        private GuiUpdateDelegate _guiUpdateCallback;

        public FilesWatcher()
        {
            _guiUpdateCallback = new GuiUpdateDelegate(GuiUpdateHandler);
        }

        public string CurrentPath { get; private set; }
        public IObservable<File[]> FilesChanged => _filesChangedSubject;

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
                _files = new File[count];

            for (int i = 0; i < count; i++)
                _files[i] = (File)Marshal.PtrToStructure(files + i * Marshal.SizeOf<File>(), typeof(File));

            _filesChangedSubject.OnNext(_files);
        }
    }
}
