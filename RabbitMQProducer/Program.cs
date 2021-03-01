using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQProducer
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;
            var factory = new ConnectionFactory {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using (var connection = factory.CreateConnection()) {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("demo-queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    while (!salir)
                    {
                        Console.WriteLine("Escribe un mensaje para enviar a cola:(S) Salir");
                        string mensaje = Console.ReadLine().ToString();
                        if (mensaje.ToLower().Equals("s"))
                        {
                            salir = true;
                        }
                        var message = new { mensaje };
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                        channel.BasicPublish("", "demo-queue", null, body);
                        
                        
                    }
                    
                }
            }
            
        }
    }
}
