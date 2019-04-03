using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using SystemTrayApp.DataObjects;
using SystemTrayApp.Utility;
using GlobalHotKey;
using Newtonsoft.Json;

namespace SystemTrayApp
{
    public class ConfigLoader : IConfigLoader
    {
        public static string[] ConfigFilePaths => new [ ]
        {
            AppDomain.CurrentDomain.BaseDirectory, // executing dir
            homePath + Path.DirectorySeparatorChar + ".lightControl" // home dir
        };

        static string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                           Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        public static string fileName = ".config";

        public ConfigLoader()
        {
        }

        public Configuration CreateBaseConfig()
        {
            Configuration cfg = new Configuration();
            cfg.MqttConnection = new MqttConnectionInfo();
            cfg.MqttConnection.ServerAddress = "192.168.1.2";
            cfg.MqttConnection.ServerPort = "1883";
            cfg.MqttConnection.Username = "replace-me";
            cfg.MqttConnection.Password = "replace-me";

            object examplePayload = new { your = "payload" , goes = "here"};
            HotKey targetHotkey = new HotKey(Key.P, ModifierKeys.Alt | ModifierKeys.Control);

            cfg.Bindings = new Hotkeyable[2];

            cfg.Bindings[0] = new Hotkeyable();
            cfg.Bindings[0].Type = HotkeyableActionType.HTTP;
            cfg.Bindings[0].Method = "(GET/POST/PUT/DELETE)";
            cfg.Bindings[0].TargetUrl = "/your/target/url";
            cfg.Bindings[0].Hotkey = HotkeyEncoder.HotkeyToString(targetHotkey);
            cfg.Bindings[0].Data = JsonConvert.SerializeObject(examplePayload);

            cfg.Bindings[1] = new Hotkeyable();
            cfg.Bindings[1].Type = HotkeyableActionType.MQTT;
            cfg.Bindings[1].Topic = "/your/mqtt/topic";
            cfg.Bindings[1].Hotkey = HotkeyEncoder.HotkeyToString(targetHotkey);
            cfg.Bindings[1].Data = JsonConvert.SerializeObject(examplePayload);

            return cfg;
        }

        private void WriteBaseConfigToFile(Configuration config)
        {
            FileStream fs = File.Create(ConfigFilePaths[0] + Path.DirectorySeparatorChar + fileName);
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);

            StreamWriter writer = new StreamWriter(fs);
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }

        public Configuration TryGetStoredConfig(out bool newConfigCreated)
        {
            bool configFound = false;
            Configuration activeConfig = null;
            newConfigCreated = false;
            foreach (var path in ConfigFilePaths.Select(path => path + fileName))
            {
                if (File.Exists(path))
                {
                    configFound = true;
                    activeConfig = LoadConfig(path);
                }
            }

            if (!configFound)
            {
                activeConfig = CreateBaseConfig();
                WriteBaseConfigToFile(activeConfig);
                newConfigCreated = true;
            }

            return activeConfig;
        }

        public Configuration LoadConfig(string path)
        {
            Configuration cfg = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
            return cfg;
        }
    }
}
