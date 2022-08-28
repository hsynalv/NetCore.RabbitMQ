using RabbitMQ.Client;

namespace NetCore.RabbitMQ.Watermark.Web.Services
{
    public class RabbitMQClientServices : IDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";
        private readonly ILogger<RabbitMQClientServices> _logger;

        public RabbitMQClientServices(ConnectionFactory factory, ILogger<RabbitMQClientServices> logger)
        {
            _factory = factory;
            _logger = logger;
            Connect();
        }

        public IModel Connect()
        {
            _connection = _factory.CreateConnection();
            if (_channel is { IsOpen: true }) 
                return _channel;

            _channel = _connection.CreateModel();
            
            _channel.ExchangeDeclare(ExchangeName,ExchangeType.Direct, true, false);

            _channel.QueueDeclare(QueueName, true, false, false, null);
            
            _channel.QueueBind(ExchangeName,QueueName,RoutingWatermark);
            _logger.LogInformation("RabbitMq ile bağlantı kuruldu");

            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu");
        }
    }
}
