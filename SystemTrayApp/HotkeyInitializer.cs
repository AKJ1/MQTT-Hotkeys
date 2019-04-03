using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using SystemTrayApp.DataObjects;
using SystemTrayApp.Utility;
using GlobalHotKey;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace SystemTrayApp
{
    public class HotkeyInitializer
    {
        public void InitializeWithConfig(Configuration config)
        {
            MQTTConnectionManager connection = new MQTTConnectionManager(config.MqttConnection);
            connection.Connect();
            
            // Create a new MQTT client.
            foreach (var bind in config.Bindings)
            {
                if (bind.Type == HotkeyableActionType.MQTT)
                {
                    HotKeyManager man = new HotKeyManager();
                    HotKey key = HotkeyEncoder.StringToHotkey(bind.Hotkey);
                    man.Register(key);
                    man.KeyPressed += (sender, k) =>
                    {
                        if (k.HotKey.Equals(key))
                        {
                            Console.WriteLine("Hotkey detected!");
//                            MessageBox.Show("DETECTEDDDDDDDDDDDD");
                            var message = new MqttApplicationMessageBuilder()
                                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                                .WithTopic(bind.Topic)
                                .WithPayload(bind.Data)
                                .WithRetainFlag(true)
                                .Build();
                            
                            if (connection.IsConnected)
                            {
                                connection.SendMessage(message);
                            }
                        }
                    };
                }
            }
        }

    }
}
