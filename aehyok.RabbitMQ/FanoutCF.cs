using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.RabbitMQ
{
    public class FanoutCF
    {
        public static IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "101.200.243.192",
                Port = 5672,
                DispatchConsumersAsync = true,  //如果使用AsyncEventingBasicConsumer,
                UserName = "lqm",
                Password = "sunlight",
                VirtualHost = "lqm_virtual"  //虚拟主机，要在服务器上创建，如果不创建，默认使用"/"，但是不建议使用"/"，因为"/"会和默认的vhost冲突
            };

            // 使用工厂创建连接
            var connection = factory.CreateConnection("aehyok");
            return connection;
        }

        public static void Publish()
        {
            var connection = CreateConnection();
            // 创建信道
            using var channel = connection.CreateModel();

            // 参数autoDelete: false 默认为false，true表示当没有消费者的时候自动删除
            channel.ExchangeDeclare("sun", ExchangeType.Fanout,durable:true);



            foreach (var index in Enumerable.Range(0, 100))
            {
                var message = $"Hello, RabbitMQ! {index}";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "sun", routingKey: string.Empty, basicProperties: null, body: body);
            }

            channel.Close();
            connection.Close();
        }

        public static void Subscrber()
        {
            var connection = CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "sunlight", true, false, false);
            channel.QueueBind(queue: "sunlight", exchange: "sun", routingKey: string.Empty, arguments: null);

            // AsyncEventingBasicConsumer继承了 IBasicConsumer，则会在消息到达时自动推送，而无需主动请求，参考https://www.cnblogs.com/hsyzero/p/6297644.html

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"接收到消息：{message}");


                //// 如果消息处理成功，则发送确认
                channel.BasicAck(e.DeliveryTag, false);

                //// 如果消息处理失败，requeue设置为true，表示重新放回队列，如果设置为false，则表示丢弃该消息
                //channel.BasicReject(e.DeliveryTag, false);
                await Task.Yield();
            };

            // autoAck: true 表示自动把发送出去的消息设置为确认，然后从内存或者硬盘中删除，而不管消费者是否消费到了消息。
            // autoAck: false 表示手动确认
            channel.BasicConsume(queue: "sunlight",
                     autoAck: false,
                     consumer: consumer);
            
            //channel.Close();
            //connection.Close();
        }
    }
}
