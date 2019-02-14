using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqSender
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Person person = new Person() { Name = "Emrah", SurName = "ozguner", ID = 1, BirthDate = new DateTime(1978, 6, 3), Message = "İlgili aday yakınımdır :)" };
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Innova",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(person);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "Innova",
                    basicProperties: null,
                    body: body);
                Console.WriteLine($"Gönderilen kişi: {person.Name}-{person.SurName}");
            }

            Console.WriteLine(" İlgili kişi gönderildi...");
            Console.ReadLine();
        }
    }

    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Message { get; set; }
    }
}