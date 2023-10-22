using Microsoft.Extensions.Hosting;
using Stock.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class StockRealTime : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQHelper.Receive((model, ea) =>
            {
                byte[] message = ea.Body.ToArray(); // 接收到的消息

                string msg = Encoding.UTF8.GetString(message);
                Console.WriteLine(msg);
            });
            return Task.CompletedTask;
        }
    }
}
