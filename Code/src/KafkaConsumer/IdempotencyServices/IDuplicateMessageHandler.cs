using System.Text.Json;

namespace KafkaConsumer.IdempotencyServices;

public interface IDuplicateMessageHandler
{
    Task<bool> HasMessageBeenProcessedBefore(string eventId);
    Task MarkMessageAsProcessed(string eventId);
}