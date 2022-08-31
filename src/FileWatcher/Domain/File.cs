using System.Runtime.InteropServices;

namespace FileWatcher.Domain
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct File
    {
        public File(string name,
                    string path,
                    int size,
                    bool isDirectory)
        {
            Name = name;
            Path = path;
            Size = size;
            IsDirectory = isDirectory;
        }

        [MarshalAs(UnmanagedType.LPStr)] public string Name;
        [MarshalAs(UnmanagedType.LPStr)] public string Path;
        public int Size { get; private set; }
        public bool IsDirectory { get; private set; }
    }
}
