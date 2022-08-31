using System;
using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GuiUpdateDelegate(IntPtr files, int count);
}
