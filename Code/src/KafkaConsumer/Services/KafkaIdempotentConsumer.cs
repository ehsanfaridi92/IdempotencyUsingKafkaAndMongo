using Confluent.Kafka;
using KafkaConsumer.ConfigurationModels;
using KafkaConsumer.IdempotencyServices;
using System.Text;

namespace KafkaConsumer.Services;

public abstract class KafkaIdempotentConsumer(
    IDuplicateMessageHandler duplicateMessageHandler,
    KafkaConsumerConfiguration kafkaConsumerConfiguration) : BackgroundService
{
    private readonly IConsumer<string, string> _consumer =
        new ConsumerBuilder<string, string>(new ConsumerConfig
        {
            GroupId = kafkaConsumerConfiguration.GroupId,
            BootstrapServers = kafkaConsumerConfiguration.BootstrapServers,
            EnableAutoOffsetStore = kafkaConsumerConfiguration.EnableAutoOffsetStore,
            AutoOffsetReset = kafkaConsumerConfiguration.AutoOffsetReset,
            EnableAutoCommit = kafkaConsumerConfiguration.EnableAutoCommit
        }).Build();

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(kafkaConsumerConfiguration.TopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                await ReceiveMessage(consumeResult);
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
        }
    }

    private async Task ReceiveMessage(ConsumeResult<string, string> consumeResult)
    {
        try
        {
            var eventId = TryGetEventIdFromHeaders(consumeResult.Message.Headers);

            if (eventId != null)
            {
                if (await IsMessageProcessedAsync(eventId))
                {
                    Console.WriteLine($"Message with Id {eventId} has already been processed.");

                    return;
                }

                await ProcessMessageAsync(consumeResult);

                await MarkMessageAsProcessedAsync(eventId);
            }
            else
            {
                await ProcessMessageAsync(consumeResult);
            }
        }
        catch (Exception e)
        {

            Console.WriteLine(e);
        }
    }

    protected abstract Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult);

    private async Task<bool> IsMessageProcessedAsync(string eventId)
    {
        try
        {
            return await duplicateMessageHandler.HasMessageBeenProcessedBefore(eventId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return false;
        }
    }

    private async Task MarkMessageAsProcessedAsync(string eventId)
    {
        try
        {
            await duplicateMessageHandler.MarkMessageAsProcessed(eventId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            await Task.CompletedTask;
        }
    }

    private static string? TryGetEventIdFromHeaders(Headers headers)
    {
        try
        {
            headers.TryGetLastBytes("eventId", out var eventIdBytes);

            return eventIdBytes is null ? null : Encoding.UTF8.GetString(eventIdBytes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return null;
        }
    }
}