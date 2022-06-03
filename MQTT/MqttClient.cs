using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Threading;
using _4_06_EF_ERP.Model;
using System.Windows;
using Newtonsoft.Json;

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
                    MessageBox.Show("ES KONNTE KEINE VERBINDUNG ZU MQTT INITIIERT WERDEN!", "MQTT", MessageBoxButton.OK);
                Console.WriteLine(ex.ToString());
            }

            await SendMessage("Willkommen Abonnent! Du befindest dich im Rechnungsbereich", "invoice/rechnung", true);
            await SendMessage("Willkommen Abonnent! Du befindest dich im Positionsbereich", "invoice/position", true);

        }

        public bool SendInvoiceJson(Invoice invoice)
        {
            // JSON konvertieren
            string json = convertToJson(invoice);

            //via MQTT senden
            Console.WriteLine(SendMessage(json, "topic/rechnung", false).Result.ToString());
            Console.WriteLine("MESSAGE SENT");
            return true;

            if (SendMessage(json, "topic/rechnung", false).Result  == false)
            {
                Console.WriteLine("SEND MESSAGE FALSE");
                return false;
            }
            else
            {
                Console.WriteLine("SEND MESSAGE TRUE; SENDING POSITIONS ...");
                foreach (Position pos in invoice.Positions)
                {
                    if (SendInvoicePositionJson(pos).Result == false)
                    {
                        return false;
                    }
                }
                Console.WriteLine("SUCCESS");
            }
            return true;
        }

        private string convertToJson(object objToSerialize)
        {
            try
            {
                return JsonConvert.SerializeObject(objToSerialize, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }

        public Task<bool> SendInvoicePositionJson(Position position)
        {
            // json konvertieren
            string json = convertToJson(position);
            return SendMessage(json, "topic/position", false);
        }

        public Task<bool> SendInvoiceString(Invoice invoice)
        {
            // string konvertieren
            string payload = invoice.ToString();

            // via MQTT senden
            return SendMessage(payload, "topic/rechnung", false);
        }

        public Task<bool> SendInvoicePositionString(Position position)
        {
            // string konvertieren
            string payload = position.ToString();

            // via MQTT senden
            return SendMessage(payload, "topic/position", false);
        }

        private async Task<bool> SendMessage(string payload, string topic, bool retainFlag)
        {
            //isConnected
            if (mqttClient.IsConnected)
            {
                try
                {
                    // message erzeugen
                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithExactlyOnceQoS()
                    .WithRetainFlag(retainFlag)
                    .Build();

                    // message senden
                    await Task.Run(() => mqttClient.PublishAsync(message, CancellationToken.None));
                } 
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            return true;
        }

    }
}
