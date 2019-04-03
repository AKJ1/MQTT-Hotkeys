using System;

namespace SystemTrayApp.DataObjects
{
    [Serializable]
    public class MqttConnectionInfo
    {
        public string ServerAddress;

        public string ServerPort;

        public string Username;

        public string Password; // yes, it's a string, it's literally read from a config file, deal with it.
    }
}
