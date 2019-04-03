using System;

namespace SystemTrayApp.DataObjects
{
    [Serializable]
    public class Configuration
    {
        public MqttConnectionInfo MqttConnection;

        public Hotkeyable[] Bindings;
    }
}
