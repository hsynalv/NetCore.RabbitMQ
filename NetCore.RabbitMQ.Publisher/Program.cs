using System.Runtime.InteropServices.ComTypes;using RabbitMQ.Client;
using System.Text;

// ------------ Publisher ------------ //



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange",  type: ExchangeType.Headers, durable: true);

Dictionary<string, object> header = new();
header.Add("format","pdf");
header.Add("shape","a4");

var properties = channel.CreateBasicProperties();
    properties.Headers = header;


channel.BasicPublish("header-exchange", "", properties, Encoding.UTF8.GetBytes("Header mesajı"));

Console.WriteLine("Mesaj gönderilmiştir");
Console.ReadKey();



