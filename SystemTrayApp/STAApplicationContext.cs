using System;
using System.Windows.Forms;
using System.Windows.Input;
using SystemTrayApp.DataObjects;
using GlobalHotKey;

namespace SystemTrayApp
{
    public class STAApplicationContext : ApplicationContext
    {
        public STAApplicationContext()
        {
            HotKey key = new HotKey(Key.NumPad8, ModifierKeys.Control | ModifierKeys.Alt);
            HotKeyManager manager = new HotKeyManager();
            manager.Register(key);
            manager.KeyPressed += (sender, data) =>
            {
                if (data.HotKey.Equals(key))
                {
                    MessageBox.Show("Top Kek");
                }
            };

            IConfigLoader loader = new ConfigLoader();
            bool newConfigCreated = false;
            Configuration config = loader.TryGetStoredConfig(out newConfigCreated);
            if (newConfigCreated)
            {
                MessageBox.Show("First run, new configuration created", "First Run");
            }
            HotkeyInitializer initaInitializer = new HotkeyInitializer();
            initaInitializer.InitializeWithConfig(config);
            Console.Read();
        }

        // Called from the Dispose method of the base class
        protected override void Dispose(bool disposing)
        {
        }
    }
}
