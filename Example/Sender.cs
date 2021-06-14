using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Example
{
    public class Sender : IHostedService
    {
        readonly IBusControl _bus;
        readonly IBusHealth _busHealth;

        readonly ILogger _logger;

        private static Timer _timer;

        private List<Exception> _exceptionList = new List<Exception>();

        public Sender(ILoggerFactory loggerFactory, IBusControl bus, IBusHealth busHealth)
        {
            _logger = loggerFactory.CreateLogger("Publishd");
            _bus = bus;
            _busHealth = busHealth;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await WaitForHealthyBus(cancellationToken);

            _logger.LogInformation("Publishing Start...");

            _timer = new Timer((e) => Publish(), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(.5));

            //for (var i = 1; i < 1000; i++)
            //    await _bus.Publish(new TestMessage
            //    {
            //        A1 = Guid.NewGuid(),
            //        A2 = Guid.NewGuid(),
            //        A3 = Guid.NewGuid(),
            //        A4 = Guid.NewGuid(),
            //    });
            //;
            _logger.LogInformation("Pushing has ended");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        async Task WaitForHealthyBus(CancellationToken cancellationToken)
        {
            HealthResult result;
            do
            {
                result = _busHealth.CheckHealth();

                await Task.Delay(100, cancellationToken);
            } while (result.Status != BusHealthStatus.Healthy);
        }

        private async void Publish()
        {
            try
            {
                // Message without routing key
                var count = Counter.IncrementPublish();
                var message = new TestMessage {
                    Counter = count,
                    Timestamp = DateTime.Now
                };
                await _bus.Publish(message);
                //_logger.LogInformation(message.ToString());
                Console.WriteLine($"[{count}] Publish : " + message.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Publish Execption! " + e.Message);
                _exceptionList.Add(e);
            }
        }
    }
}