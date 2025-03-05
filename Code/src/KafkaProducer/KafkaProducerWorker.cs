using KafkaProducer.Services;
using Newtonsoft.Json;

namespace KafkaProducer;

public class KafkaProducerWorker(IKafkaProducer kafkaProducer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (var i = 1; i <= 10; i++)
        {
            var message = new
            {
                Name = "Ehsan",
                Family = "Faridi",
                Id = i.ToString()
            };

            await kafkaProducer.ProduceAsync(message.Id, JsonConvert.SerializeObject(message), message.Id, stoppingToken);

            Console.WriteLine($"message {i} sent.");

        }
    }
}