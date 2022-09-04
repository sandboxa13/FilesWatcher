using System;
using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    internal class FileWatcherInterop
    {
        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr create_file_watcher([MarshalAs(UnmanagedType.FunctionPtr)] FilesUpdateDelegate callback);


        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void dispose_file_watcher(IntPtr ptr);

        
        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void stop(IntPtr ptr);


        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void observe(IntPtr ptr, [MarshalAs(UnmanagedType.LPWStr)] string path);
    }
}
