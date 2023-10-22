import pika
import json
class RabbitMQHerper:
    def __init__(self):
        self.username = 'stock'
        self.password = '123456'
        self.host = '127.0.0.1'
        self.port = '5672'
        self.exchange='stock'
        self.queue='stock_real_time_quotes'
        self.routing_key='stock_real_time_quotes'
        self.get_connect() 

    def get_connect(self):
        credentials = pika.PlainCredentials(username=self.username, password=self.password)  # 登录凭证
        self.connect = pika.BlockingConnection(pika.ConnectionParameters(host=self.host, port=self.port, credentials=credentials))
        self.channel = self.connect.channel()  # 客户端连接rabbitmq服务端后开辟管道，每个channel代表一个会话任务
 
    
    def send(self, body):
        # 创建exchange消息交换机， 并且exchange持久化
        self.channel.exchange_declare(
                                        exchange=self.exchange, 
                                        exchange_type='direct', 
                                        durable=True)
        # 声明队列， 并且队列持久化
        self.channel.queue_declare(queue=self.queue, durable=True,)
        # 通过routing_key 绑定 消息交换机和队列
        self.channel.queue_bind(self.queue, self.exchange, self.routing_key)
        # 发消息
        message = json.dumps({'message': body, 'expiration': 10000})
        self.channel.basic_publish(
                                    exchange=self.exchange,
                                    routing_key=self.routing_key,  # 根据exchange和routing进而指定 队列
                                    body=message  # 发送的数据
                                    #properties=pika.BasicProperties(delivery_mode=2)  # 消息持久化存到硬盘， 1是非持久化
                                )
    def receive(self, exchange, queue, callback):
        print('消费任务启动')  
        self.channel.exchange_declare(exchange=exchange, exchange_type='direct', durable=True)
        self.channel.queue_declare(queue=queue, durable=True)
        self.channel.basic_consume(
                                    queue=queue,  # 队列名
                                    on_message_callback=callback,  # 指定回调函数
                                    auto_ack=False,  # 关闭自动ack采用手动应答
                                )
        self.channel.start_consuming()  # 开始接收信息，并进入阻塞状态，队列里有信息才会调用on_message_callback进行处理
