using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    internal class PermissionsElevatorInterop
    {
        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void restart_as_admin([MarshalAs(UnmanagedType.LPWStr)] string path);
    }
}
