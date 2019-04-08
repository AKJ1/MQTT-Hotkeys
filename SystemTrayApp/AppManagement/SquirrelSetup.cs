using System;
using Squirrel;

namespace SystemTrayApp.AppManagement
{
    public static class SquirrelSetup
    {
        public static string UpdateURL = @"C:\Projects\SystemTrayApp\SquirrelRelease\";
         
        public static UpdateManager updateManager = new UpdateManager(UpdateURL);

        public static void SetupSquirrel()
        {
            SquirrelAwareApp.HandleEvents(onFirstRun: OnFirstRun, onInitialInstall: OnInitialInstall, onAppUninstall: OnAppUninstall);
        }

        public static void OnInitialInstall(Version ver)
        {
            updateManager.CreateUninstallerRegistryEntry();
        }

        public static void OnAppUninstall(Version ver)
        { 
            updateManager.RemoveUninstallerRegistryEntry();
            updateManager.RemoveRunAtWindowsStartupRegistry();
        }

        public static void OnFirstRun()
        {
            updateManager.CreateRunAtWindowsStartupRegistry();
        }

    }
}
