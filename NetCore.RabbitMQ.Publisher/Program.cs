using System.Runtime.InteropServices.ComTypes;using RabbitMQ.Client;
using System.Text;

// ------------ Publisher ------------ //



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic",  type: ExchangeType.Direct, durable: true);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    var random = new Random();
    LogNames log1 = (LogNames)random.Next(1, 5);
    LogNames log2 = (LogNames)random.Next(1, 5);
    LogNames log3 = (LogNames)random.Next(1, 5);
    
    var routeKey = $"{log1}.{log2}.{log3}";
    string message = $"log-type : {log1}-{log2}-{log3}";
    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-topic", routeKey, null, messageBody);

    Console.WriteLine($"Log Gönderilmiştir : {message}");

});



Console.ReadKey();




public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4
}