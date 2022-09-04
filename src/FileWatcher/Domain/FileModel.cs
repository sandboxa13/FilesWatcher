using System;
using System.Runtime.InteropServices;

namespace FileWatcher.Domain
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct FileModel
    {
        public FileModel(string name,
                    string path,
                    string lastWriteTime,
                    int size,
                    bool isDirectory)
        {
            Name = name;
            Path = path;
            LastWriteTime = lastWriteTime;
            Size = size;
            IsDirectory = isDirectory;
        }

        [MarshalAs(UnmanagedType.LPStr)] public string Name;
        [MarshalAs(UnmanagedType.LPStr)] public string Path;
        [MarshalAs(UnmanagedType.LPStr)] public string LastWriteTime;

        public int Size { get; private set; }
        public bool IsDirectory { get; private set; }

        internal void Update(FileModel newFile)
        {
            Name = newFile.Name;
            Path = newFile.Path;
            LastWriteTime = newFile.LastWriteTime;
            Size = newFile.Size;
            IsDirectory = newFile.IsDirectory;
        }

        internal bool Equals(FileModel newFile)
        {
            bool result = true;

            if(newFile.Name != Name)
                result = false;

            if (newFile.Path != Path)
                result = false;

            if (newFile.Size != Size)
                result = false;

            if (newFile.LastWriteTime != LastWriteTime)
                result = false;

            return result;
        }
    }
}
