namespace KafkaProducer.ConfigurationModels;

public class KafkaProducerConfiguration
{
    public string BootstrapServers { get; set; }
    public string TopicName { get; set; }
}