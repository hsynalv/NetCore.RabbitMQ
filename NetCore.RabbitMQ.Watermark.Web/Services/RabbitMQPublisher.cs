using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace NetCore.RabbitMQ.Watermark.Web.Services;

public class RabbitMQPublisher
{
    private readonly RabbitMQClientService _rabbitMQClientService;

    public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
    {
        _rabbitMQClientService = rabbitMQClientService;
    }

    public void Publish(productImageCreatedEvent productImageCreatedEvent)
    {
        var channel = _rabbitMQClientService.Connect();

        var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

        var bodyByte = Encoding.UTF8.GetBytes(bodyString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName,  routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);

    }
}