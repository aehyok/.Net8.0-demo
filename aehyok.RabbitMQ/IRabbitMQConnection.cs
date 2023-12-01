using RabbitMQ.Client;

namespace aehyok.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IConnection CreateConnection();

        IModel CreateModel();
    }
}