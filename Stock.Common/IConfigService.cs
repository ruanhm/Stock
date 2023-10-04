
namespace Stock.Common
{
    public interface IConfigService: ISingletonDenpendency
    {
        public string ReadPythonPath();
        public string ReadSqlServerConnStr();

        public string ReadDataDataSourceUrl();
    }
}
