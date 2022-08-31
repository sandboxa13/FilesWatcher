using System.Runtime.InteropServices;

namespace FileWatcher.Logic.Interop
{
    internal class PermissionsCheckerInterop
    {
        [DllImport(@"FileWatcherDll.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool is_user_admin();
    }
}
