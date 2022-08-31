using System;
using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class PermissionsElevator
    {
        public void RunAsAdmin()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory + "FileWatcher.exe";

            PermissionsElevatorInterop.restart_as_admin(exePath);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
