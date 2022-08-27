using System.Runtime.InteropServices.ComTypes;using RabbitMQ.Client;
using System.Text;

// ------------ Publisher ------------ //





var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-direct",  type: ExchangeType.Direct, durable: true);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var queueName = $"direct-queue-{x}";
    var routeKey = $"route-{x}";
    channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queueName, "logs-direct", routeKey, null);
});

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    LogNames log = (LogNames)new Random().Next(1, 5);

    string message = $"log-type : {log}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    var routeKey = $"route-{log}";

    channel.BasicPublish("logs-direct", routeKey, null, messageBody);

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