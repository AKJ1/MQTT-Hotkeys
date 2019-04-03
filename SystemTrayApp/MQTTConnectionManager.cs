using System;
using System.Threading.Tasks;
using SystemTrayApp.DataObjects;
using MQTTnet;
using MQTTnet.Client;

namespace SystemTrayApp
{
    public class MQTTConnectionManager
    {
        public bool IsConnected => activeClient.IsConnected;

        private MqttConnectionInfo connectionDetails;

        private IMqttClient activeClient;

        private bool retainConnection;

        private bool isInitialized = false;

        public MQTTConnectionManager(MqttConnectionInfo connDeets)
        {
            this.connectionDetails = connDeets;
        }

        public void Connect(bool retainConnection = true)
        {
            Console.WriteLine("Conencting...");
            this.retainConnection = retainConnection;
            Task<IMqttClient> connectionTask = EstablishMqttConnection(connectionDetails);
            connectionTask.GetAwaiter().OnCompleted(OnConnectionProceed);
            if (!isInitialized)
            {
                isInitialized = true;
                SetupConnectionHandlingListeners();
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                activeClient.DisconnectAsync();
            }
        }

        private void SetupConnectionHandlingListeners()
        {
            activeClient.Disconnected += (sender, args) =>
            {
                Console.WriteLine("Disconnected!");
                if (retainConnection)
                {
                    Connect(true);
                }
            };

            activeClient.Connected += (sender, args) => { Console.WriteLine("Connected!"); };
        }

        private void OnConnectionProceed()
        {
            if (!activeClient.IsConnected)
            {
                Connect(true);
            }
        }

        public void SendMessage(MqttApplicationMessage msg)
        {
            if (IsConnected)
            {
                activeClient.PublishAsync(msg);
            }
        }

        private async Task<IMqttClient> EstablishMqttConnection(MqttConnectionInfo info)
        {
            var factory = new MqttFactory();
            activeClient = factory.CreateMqttClient();
            var builder = new MqttClientOptionsBuilder()
                .WithClientId("Desktop")
                .WithTcpServer(info.ServerAddress, int.Parse(info.ServerPort))
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(90))
//                .WithTls()
                .WithCleanSession(true);
            
            if (!string.IsNullOrEmpty(info.Username))
            {
                builder.WithCredentials(info.Username, info.Password);
            }

            var options = builder.Build();
            await activeClient.ConnectAsync(options);
            return activeClient;
        }
    }
}
