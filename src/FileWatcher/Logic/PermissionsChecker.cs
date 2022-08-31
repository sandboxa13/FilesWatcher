using FileWatcher.Logic.Interop;

namespace FileWatcher.Logic
{
    internal class PermissionsChecker
    {
        public PermissionsChecker()
        {
            CheckPermissions();
        }

        public bool IsAdmin { get; private set; }

        private void CheckPermissions()
        {
            IsAdmin = PermissionsCheckerInterop.is_user_admin();
        }
    }
}
