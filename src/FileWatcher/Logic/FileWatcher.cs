using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using FileWatcher.Domain;
using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class FilesWatcher
    {
        private readonly ISubject<FileModel[]> _filesUpdatedSubject;
        private readonly Dictionary<string, FileModel> _filesCache;

        private IntPtr _fileWatcherPtr;
        private FilesUpdateDelegate _filesUpdateCallback;

        public FilesWatcher()
        {
            _filesUpdateCallback = new FilesUpdateDelegate(FilesUpdateHandler);
            _filesUpdatedSubject = new Subject<FileModel[]>();

            _fileWatcherPtr = FileWatcherInterop.create_file_watcher(_filesUpdateCallback);
            _filesCache = new Dictionary<string, FileModel>(); 
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

            FileModel[] newFiles = new FileModel[count];

            for (int i = 0; i < count; i++)
                newFiles[i] = (FileModel)Marshal.PtrToStructure(files + i * Marshal.SizeOf<FileModel>(), typeof(FileModel));

            var needUpdate = CheckNewFiles(newFiles);

            if(count != _filesCache.Values.Count)
                needUpdate = true;

            if (needUpdate)
            {
                _filesCache.Clear();

                foreach (var newFiile in newFiles)
                {
                    _filesCache.Add(newFiile.Name, newFiile);
                }

                _filesUpdatedSubject.OnNext(_filesCache.Values.ToArray());
            }
        }

        private bool CheckNewFiles(FileModel[] newFiles)
        {
            bool needUpdate = false;

            foreach (var newFile in newFiles)
            {
                if (needUpdate)
                    return needUpdate;

                var hasValue = _filesCache.TryGetValue(newFile.Name, out var oldFile);

                if(!hasValue)
                    needUpdate = true;

                if (oldFile.Equals(newFile))
                {
                    continue;
                }
                else
                {
                    needUpdate = true;
                }
            }

            return needUpdate;
        }
    }
}
