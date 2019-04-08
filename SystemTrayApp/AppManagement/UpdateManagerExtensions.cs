using Microsoft.Win32;
using Squirrel;
using System.IO;

namespace SystemTrayApp.AppManagement
{
    public static class UpdateManagerExtensions 
    {
        private static RegistryKey OpenRunAtWindowsStartupRegistryKey() => Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static void CreateRunAtWindowsStartupRegistry(this UpdateManager updateManager)
        {
            SquirrelAwareApp.HandleEvents();
            using (var startupRegistryKey = OpenRunAtWindowsStartupRegistryKey())
                startupRegistryKey.SetValue( updateManager.ApplicationName, Path.Combine(updateManager.RootAppDirectory, $"{updateManager.ApplicationName}.exe") );
        }

        public static void RemoveRunAtWindowsStartupRegistry(this UpdateManager updateManager)
        {
            using (var startupRegistryKey = OpenRunAtWindowsStartupRegistryKey())
                startupRegistryKey.DeleteValue(updateManager.ApplicationName);
        }
    }
}
