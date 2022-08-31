using System;
using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    internal class FileWatcherInterop
    {
        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr create_file_watcher([MarshalAs(UnmanagedType.FunctionPtr)] GuiUpdateDelegate callback, [MarshalAs(UnmanagedType.LPWStr)] string path);


        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void dispose_file_watcher(IntPtr ptr);
    }
}
