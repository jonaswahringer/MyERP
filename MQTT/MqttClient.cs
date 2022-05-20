using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Threading;
using _4_06_EF_ERP.Model;
using System.Windows;

namespace _4_06_EF_ERP.MQTT
{
    class MqttClient
    {
        private IMqttFactory factory = new MqttFactory();
        private IMqttClient mqttClient;
        public string ClientId { get; set; }
        public string ServerURL { get; set; }
        public int Port { get; set; }

        public async void Init()
        {
            mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
            .WithClientId(ClientId)
            .WithTcpServer(ServerURL)
            .WithCleanSession()
            .Build();

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"{DateTime.Now} " + $"Topic={e.ApplicationMessage.Topic}; " + $"Message={Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}; " + $"QoS={e.ApplicationMessage.QualityOfServiceLevel}");
                // Task.Run(() => mqttClient.PublishAsync("daheim/hmi/update", "refresh"));
            });

            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");
                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("invoice/rechnung").Build());
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("invoice/position").Build());
                Console.WriteLine("### SUBSCRIBED ###");
             });

            try
            {
                await mqttClient.ConnectAsync(options, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBoxResult messageBoxResult =
                    MessageBox.Show("ES KONNTE KEINE VERBINDUNG ZU MQTT HERGESTELLT WERDEN!", "MQTT", MessageBoxButton.OK);
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task<bool> SendInvoice(Invoice invoice)
        {
            Console.WriteLine("SEND INVOICE");
            if(mqttClient.IsConnected)
            {

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("invoice/rechnung")
                    .WithPayload(invoice.ToString())
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();

                await Task.Run(() => mqttClient.PublishAsync(message, CancellationToken.None));

                foreach (Position pos in invoice.Positions)
                {
                    await SendInvoicePosition(pos);
                }
                return true;
            }
            
            return false;
        }

        public async Task<bool> SendInvoicePosition(Position position)
        {
            Console.WriteLine("SEND POSITION");
            if (mqttClient.IsConnected)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("invoice/position")
                    .WithPayload(position.ToString())
                    .WithExactlyOnceQoS()
                    .WithRetainFlag()
                    .Build();

                await Task.Run(() => mqttClient.PublishAsync(message, CancellationToken.None));
                
                return true;
            }
            return false;
        }

    }
}
