using System;
using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FilesUpdateDelegate(IntPtr files, int count);
}
