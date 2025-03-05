﻿using Confluent.Kafka;

namespace KafkaConsumer.ConfigurationModels;

public class KafkaConsumerConfiguration
{
    public string BootstrapServers { get; set; }
    public string TopicName { get; set; }
    public string GroupId { get; set; }
    public bool EnableAutoOffsetStore { get; set; }
    public bool EnableAutoCommit { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; }
}