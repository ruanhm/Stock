using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Common
{
    public class Configs
    {
        public string SQLServer { get; set; }
        public string Oracle { get; set; }
        public string MySql { get; set; }
        public string PythonPath { get; set; }
        public string DataDataSourceUrl { set; get; }
        public RabbitMQConfig RabbitMQ { set; get; }
    }
}
