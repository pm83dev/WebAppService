using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using System.Text;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace Application.Services
{
    public class MqttClientService
    {
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _mqttClientOptions;
        private readonly List<string> _mqttVariables;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MqttClientService(string clientId, string server, int port, string username, string password, IServiceScopeFactory serviceScopeFactory)
        {
            _mqttVariables = new List<string>();
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(server, port)
                .WithCredentials(username, password)
                .WithTls()
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to MQTT broker" + " @ " + DateTime.Now);
                // Sottoscrizione ai topic dopo la connessione
                await SubscribeToTopics();
            });

            _mqttClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from MQTT broker." + " @ " + DateTime.Now);
                if (e.Exception != null)
                {
                    Console.WriteLine($"Exception: {e.Exception.Message}");
                }
            });

            _mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Received message on topic '{topic}': {payload}");
                await ProcessReceivedMessage(topic, payload); // Modificato per includere il topic
            });

            _serviceScopeFactory = serviceScopeFactory;
        }

        // Proprietà per verificare se il client è connesso
        public bool IsConnected => _mqttClient.IsConnected;

        // Funzione per connettersi e sottoscrivere ai topic
        public async Task ConnectAsync()
        {
            try
            {
                // Tentiamo di connetterci solo se non siamo già connessi
                if (!_mqttClient.IsConnected)
                {
                    await _mqttClient.ConnectAsync(_mqttClientOptions);
                    Console.WriteLine("Successfully connected to the MQTT broker");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during connection: {ex.Message}");
            }
        }

        // Funzione per sottoscrivere ai topic
        private async Task SubscribeToTopics()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dppMachineRepo = scope.ServiceProvider.GetRequiredService<IDppMachineRepository>();
                var serialNumbers = await dppMachineRepo.GetAllDeviceSerialNumbersAsync();

                foreach (var serialNumber in serialNumbers)
                {
                    if (_mqttClient.IsConnected)
                    {
                        // Sottoscrivi ai topic solo se connesso
                        await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("plasmac/" + serialNumber).Build());
                        Console.WriteLine("Attempt subscribing to topic: " + serialNumber);
                    }
                    else
                    {
                        Console.WriteLine("MQTT client is not connected. Cannot subscribe to topic: " + serialNumber);
                    }
                }

                Console.WriteLine("Successfully subscribed to topics.");
            }
        }

        // Funzione per disconnettersi dal broker
        public async Task DisconnectAsync()
        {
            try
            {
                await _mqttClient.DisconnectAsync();
                Console.WriteLine("Successfully disconnected from MQTT broker.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while disconnecting from broker: {ex.Message}");
            }
        }

        private async Task ProcessReceivedMessage(string topic, string payload)
        {
            try
            {
                // Estrai il serial number dal topic
                var serialNumber = topic.Split('/').Last();

                // Converte il payload JSON in un oggetto BsonDocument
                var bsonData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(payload);

                // Aggiungi informazioni aggiuntive al documento
                bsonData.Add("SerialNumber", serialNumber);

                bsonData.Add("Timestamp", DateTime.UtcNow);

                // Salva il documento nel repository MongoDB
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var deviceDataRepo = scope.ServiceProvider.GetRequiredService<IDeviceDataRepository>();
                    await deviceDataRepo.AddDeviceDataBson(bsonData);
                }

                Console.WriteLine($"Data for serial {serialNumber} saved to MongoDB.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }





    }
}
