using System.Text;
using Confluent.Kafka;
using KafkaProducer.ConfigurationModels;

namespace KafkaProducer.Services;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly KafkaProducerConfiguration _configurations;

    public KafkaProducer(KafkaProducerConfiguration configurations)
    {
        _configurations = configurations;
        var config = new ProducerConfig()
        {
            BootstrapServers = configurations.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task<DeliveryResult<string, string>> ProduceAsync(string messageKey, string message, string eventId, CancellationToken cancellationToken)
    {
        return await _producer.ProduceAsync(_configurations.TopicName, new Message<string, string>
        {
            Value = message,
            Key = messageKey,
            Headers = GetHeaders(eventId)
        }, cancellationToken);
    }
    private static Headers GetHeaders(string eventId)
    {
        return [new Header("eventId", Encoding.UTF8.GetBytes(eventId))];
    }
}