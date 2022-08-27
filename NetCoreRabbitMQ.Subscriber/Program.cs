using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

// ------------ Subscriber ------------ //

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://zlkovbxc:GsFqD2e-IEBd4QGyNBS6R0nTgv7SvMe8@shark.rmq.cloudamqp.com/zlkovbxc");

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

var randomQueueName = channel.QueueDeclare().QueueName + "randomQueueName"; // => Random queue ismi oluşturmak için

//var randomQueueName = "log-database-save-queue";
//channel.QueueDeclare(randomQueueName, true, false, false);  => Kalicı kuyruk edinmek için kuyruk decalre etmek gerek


channel.QueueBind(randomQueueName, "logs-fanout", "",null); // bu durumda subscriber düştüğü anda kuyruk da düşer

channel.BasicQos(0,1,false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName, false, consumer);

Console.WriteLine("Loglar dinleniyor...");

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen mesaj : {message}");
    channel.BasicAck(e.DeliveryTag, false);
};

Console.ReadKey();