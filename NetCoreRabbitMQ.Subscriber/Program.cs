using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

channel.BasicQos(0,1,false);

// channel.QueueDeclare("hello-queue",true,false,false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen mesaj : {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();