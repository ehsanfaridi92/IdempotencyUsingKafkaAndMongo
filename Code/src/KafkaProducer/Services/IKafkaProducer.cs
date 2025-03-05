using Confluent.Kafka;

namespace KafkaProducer.Services;

public interface IKafkaProducer
{
    Task<DeliveryResult<string, string>> ProduceAsync(string messageKey, string message, string eventId,
        CancellationToken cancellationToken);
}