using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.QueueDeclare("hello-queue",true,false,false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"message : {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

    Console.WriteLine($"Mesaj Gönderilmiştir : {message}");

});



Console.ReadKey();