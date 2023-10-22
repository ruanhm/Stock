using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Common
{
    public static class RabbitMQHelper
    {
        private static IConfigService configs { get; set; } = IocManager.Resolve<IConfigService>();
        private static IConnectionFactory GetConnection()
        {
            var mqConfig=configs.ReadRabbitMQConfig();
            return new ConnectionFactory//创建连接工厂对象
            {
                HostName = mqConfig.Host,//IP地址
                Port = mqConfig.Port,//端口号
                UserName = mqConfig.User,//用户账号
                Password = mqConfig.Password//用户密码
            };
        }

        public static void Receive(string queueName,string exchangeName,string routeKey, EventHandler<BasicDeliverEventArgs> basicDeliverEvent)
        {
            var mqConfig = configs.ReadRabbitMQConfig();
            IConnection conn = GetConnection().CreateConnection();

            IModel channel = conn.CreateModel();
                    //声明一个队列
            channel.QueueDeclare(
                queue: queueName,//消息队列名称
                durable: mqConfig.Durable,//是否缓存
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            channel.QueueBind(queueName, exchangeName, routeKey);// 队列绑定给指定的交换机
            channel.BasicQos(0, 1, false); // 设置消费者每次只接收一条消息
            //创建消费者对象
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += basicDeliverEvent;
            //消费者开启监听
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                
            
        }

        public static void Receive(EventHandler<BasicDeliverEventArgs> basicDeliverEvent)
        {
            var mqConfig = configs.ReadRabbitMQConfig();
            Receive(mqConfig.Queue, mqConfig.Exchange, mqConfig.RoutingKey, basicDeliverEvent);
        }
    }
}
