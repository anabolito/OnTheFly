using System.Text;
using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using QueuePersistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


const string QUEUE_NAME = "reserved";

var factory = new ConnectionFactory() { HostName = "localhost" };


using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: QUEUE_NAME,
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

        while (true)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var returnMessage = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Sale>(returnMessage);
                var bson = BsonDocument.Parse(returnMessage);
                try
                {
                    new Persistence().InsertSale(bson, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }



            };

            channel.BasicConsume(queue: QUEUE_NAME,
                                 autoAck: true,
                                 consumer: consumer);

            Thread.Sleep(2000); // a cada 2 segundos eu olho a fila
        }
    }
}