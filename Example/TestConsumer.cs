using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example
{
    public class TestConsumer : IConsumer<TestMessage>
    {
        private readonly ILogger _logger;
        public TestConsumer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Consumer");
        }

        public Task Consume(ConsumeContext<TestMessage> context)
        {
            try
            {
                var consumeCounter = Counter.IncrementConsume();
                _logger.LogInformation($"Consumed [{consumeCounter}] : {context.Message}");

                if (context.Message.Counter != consumeCounter)
                {
                    _logger.LogWarning("Counters do not match!!");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consume Exception " + e.Message);
            }

            return Task.CompletedTask;

        }
    }
}
