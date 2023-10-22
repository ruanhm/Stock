import Controllers as c
from tools import to_json_callbak
from rabbitmq_helper import RabbitMQHerper
from apscheduler.schedulers.background import BackgroundScheduler
from datetime import datetime

# 初始化后台调度器
scheduler = BackgroundScheduler()

@scheduler.scheduled_job('interval',seconds=3)
def job1():
    print("运行job1:"+datetime.now().strftime("%Y-%m-%d %H:%M:%S"))
    mq=RabbitMQHerper()
    sender=to_json_callbak(c.get_all_stock_real_time_quotes_for_ak)
    mq.send(sender)


