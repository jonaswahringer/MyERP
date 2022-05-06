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

namespace _4_06_EF_ERP.MQTT
{
    class MqttClient
    {
        private IMqttFactory factory = new MqttFactory();
        private IMqttClient mqttClient;
        public async void Init()
        {
            mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
            .WithClientId("Client1")
            .WithTcpServer("localhost")
            .WithCleanSession()
            .Build();

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine($"{DateTime.Now} " + $"Topic={e.ApplicationMessage.Topic}; " + $"Message={Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}; " + $"QoS={e.ApplicationMessage.QualityOfServiceLevel}");
            
                Task.Run(() => mqttClient.PublishAsync("daheim/hmi/update", "refresh"));
            });

            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");
                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("invoice/rechnung").Build());
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("invoice/position").Build());
                Console.WriteLine("### SUBSCRIBED ###");
             });
               await mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        async public Task<bool> SendInvoice(Invoice invoice)
        {
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

        async public Task<bool> SendInvoicePosition(Position position)
        {
            if(mqttClient.IsConnected)
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
