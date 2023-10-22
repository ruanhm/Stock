using Microsoft.Extensions.Options;
namespace Stock.Common
{
    public class ConfigService : IConfigService
    {
        private readonly IOptionsSnapshot<Configs> c;
        public ConfigService(IOptionsSnapshot<Configs> c)
        {
            this.c = c;
        }

        public string ReadDataDataSourceUrl()
        {
            return c.Value.DataDataSourceUrl;
        }

        public string ReadPythonPath()
        {
            return c.Value.PythonPath;
        }

        public RabbitMQConfig ReadRabbitMQConfig()
        {
            return c.Value.RabbitMQ;
        }

        public string ReadSqlServerConnStr()
        {
            return c.Value.SQLServer;
        }
    }
}