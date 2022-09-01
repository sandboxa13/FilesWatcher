using System.Runtime.InteropServices;

namespace FileWatcher.Domain
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct File
    {
        public File(string name,
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
    }
}
