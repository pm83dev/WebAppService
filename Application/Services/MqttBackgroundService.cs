using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MqttBackgroundService : BackgroundService
    {
        private readonly MqttClientService _mqttClientService;

        public MqttBackgroundService(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Gestisce la connessione iniziale
            await _mqttClientService.ConnectAsync();

            // Esegue finché non viene richiesta la cancellazione
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_mqttClientService.IsConnected)
                {
                    // Tenta di riconnettersi in caso di disconnessione
                    Console.WriteLine("Tentativo di riconnessione...");
                    await _mqttClientService.ConnectAsync();
                }

                // Attende un po' di tempo prima di ricontrollare lo stato della connessione
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            // Disconnessione quando il servizio viene fermato
            await _mqttClientService.DisconnectAsync();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // Effettua la disconnessione quando il servizio viene fermato
            await _mqttClientService.DisconnectAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
