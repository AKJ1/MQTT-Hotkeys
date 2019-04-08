using System;

namespace SystemTrayApp.DataObjects
{
    [Serializable]
    public class MqttConnectionInfo
    {
        public string ServerAddress;

        public string ServerPort;

        public string Username;

        public string Password; 
    }
}
