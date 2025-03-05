using KafkaProducer;
using KafkaProducer.ConfigurationModels;
using KafkaProducer.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer.Services.KafkaProducer>();

builder.Services.AddHostedService<KafkaProducerWorker>();

var kafkaProducerConfiguration = builder.Configuration.GetSection("KafkaProducerConfiguration").Get<KafkaProducerConfiguration>();

builder.Services.AddSingleton(kafkaProducerConfiguration);

var host = builder.Build();
host.Run();
