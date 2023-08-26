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
        public string ReadPythonPath()
        {
            return c.Value.PythonPath;
        }

        public string ReadSqlServerConnStr()
        {
            return c.Value.SQLServer;
        }
    }
}