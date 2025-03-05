
# Idempotency Using Kafka and MongoDB

This project is a simple implementation of the **Idempotency** pattern using **Kafka** and **MongoDB**. The goal of this project is to prevent the duplicate processing of messages in **Event-Driven Architecture** based systems. It includes both producer and consumer components for sending and receiving messages from Kafka, with **Idempotency** features for storing and preventing the reprocessing of messages.

## Project Structure

- **Producer**: Sends messages to Kafka and uses MongoDB to store data.
- **Consumer**: Receives messages from Kafka and processes them using **Idempotency** to prevent duplicate processing.
- **MongoDB**: Stores the processing status of messages to detect duplicate messages.
- **Kafka**: Used as a messaging system to transfer data between the Producer and Consumer.

## Prerequisites

To run this project, you need the following:

- .NET 8.0
- Kafka (with the required bootstrap servers and topic name configuration)
- MongoDB to store message processing status
- Install the following NuGet packages:
  - **Confluent.Kafka**
  - **MongoDB.Driver**
  - **Newtonsoft.Json**
  - **Humanizer.Core**

## Setting up the Project

### 1. Kafka Configuration

Before starting, make sure that Kafka is installed and running on your system. You will need the following settings:
- `BootstrapServers`: The address of your Kafka server.
- `TopicName`: The name of the topic where messages will be sent and received from.

### 2. MongoDB Configuration

Ensure MongoDB is up and running to store the processing status of messages. The `DuplicateMessageHandler` model is used to store the status of messages.

### 3. Running the Project

#### Running the Producer:
To run the Producer that sends messages to Kafka, simply execute the `KafkaProducerWorker` class. This class sends messages to Kafka and includes a unique identifier in the message headers.

#### Running the Consumer:
To consume messages from Kafka, execute the `KafkaConsumerWorker` class. This class receives messages from Kafka and checks if they have already been processed. If the message is not duplicate, it is processed and the message ID is stored in MongoDB as processed.

## Project Structure

```
- KafkaProducer
  - KafkaProducerWorker.cs
  - KafkaProducer.cs
  - IConfiguration (Kafka configurations)
  - Models (Message models)

- KafkaConsumer
  - KafkaConsumerWorker.cs
  - KafkaIdempotentConsumer.cs
  - IDuplicateMessageHandler.cs
  - Models (Message models)

- MongoDB
  - Database connection setup
  - DuplicateMessageHandler (for storing message processing status)
```

## How to Use

1. **Configure Kafka and MongoDB**: 
   - Kafka configuration is located in the project’s configuration classes (`KafkaProducerConfiguration` and `KafkaConsumerConfiguration`).
   - MongoDB is used to store the processed message status and prevent reprocessing of messages.

2. **Running the Producer**: 
   To send messages to Kafka, use `KafkaProducerWorker`. This class sends messages to Kafka and includes a unique identifier (EventId) in the headers.

3. **Running the Consumer**: 
   To consume messages from Kafka and process them, use `KafkaConsumerWorker`. This class ensures that messages are not processed more than once.

## Class Descriptions

### `KafkaProducerWorker`
This class is responsible for sending messages to Kafka. The messages are serialized to **JSON** format and sent to Kafka with a unique identifier (EventId) in the message headers.

### `KafkaProducer`
This class connects to Kafka and sends messages to the specified topic. Kafka configurations are set through the `KafkaProducerConfiguration`.

### `KafkaConsumerWorker`
This class consumes messages from Kafka and processes them using the **Idempotency** pattern. It ensures that each message is processed only once by checking its EventId against a MongoDB store.

### `KafkaIdempotentConsumer`
A base class for all Kafka consumers that use the Idempotency pattern. This class uses **MongoDB** to store the EventIds of processed messages.

### `IDuplicateMessageHandler`
An interface for handling and storing processed messages. It uses MongoDB to track whether a message has been processed before.

## Example

### Producer

```csharp
var message = new
{
    Name = "Ehsan",
    Family = "Faridi",
    Id = "1"
};

await kafkaProducer.ProduceAsync(message.Id, JsonConvert.SerializeObject(message), message.Id, cancellationToken);
```

### Consumer

```csharp
await ProcessMessageAsync(consumeResult);
```

## Conclusion

This project serves as a simple example of implementing **Idempotency** using Kafka and MongoDB to prevent duplicate processing of messages. This pattern is useful in **Event-Driven** and **Microservices** architectures to guarantee that messages are processed only once.
