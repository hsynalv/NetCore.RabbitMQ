using System.Net.Http.Headers;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Shared;

// ------------ Subscriber ------------ //

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);
var queueName = channel.QueueDeclare().QueueName;

Dictionary<string, object> headers = new();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
headers.Add("x-match", "any"/*"all"*/);

channel.QueueBind(queueName, "header-exchange", string.Empty, headers);
channel.BasicConsume(queueName, false, consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Product product = JsonSerializer.Deserialize<Product>(message);
    
    Console.WriteLine($"Gelen mesaj : {product.Id} {product.Name}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();