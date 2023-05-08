using System.Text;
using DocumentFormat.OpenXml.Bibliography;
using Models;
using MongoDB.Bson.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services;

internal class Program
{
    private static void Main(string[] args)
    {
        const string QUEUE_NAME = "Fila1";
        const string QUEUE_NAME2 = "Fila2";

        var factory = new ConnectionFactory() { HostName = "localhost" };

        HttpClient cityClient = new HttpClient();

        // Consumo da Fila1

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
                        //var sale = JsonConvert.DeserializeObject<Sale>(returnMessage);

                        try
                        {
                            //new SaleService().Insert(sale);
                            //Console.WriteLine(sale.Description);
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

        // Consumo da Fila2

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
                       // var sale = JsonConvert.DeserializeObject<Sale>(returnMessage);

                        try
                        {
                            //new SaleService().Insert(sale);
                            //Console.WriteLine(sale.Description);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            throw;
                        }



                    };

                    channel.BasicConsume(queue: QUEUE_NAME2,
                                         autoAck: true,
                                         consumer: consumer);

                    Thread.Sleep(2000); // a cada 2 segundos eu olho a fila
                }
            }
        }
    }
}