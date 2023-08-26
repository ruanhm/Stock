
namespace Stock.Common
{
    public interface IConfigService
    {
        public string ReadPythonPath();
        public string ReadSqlServerConnStr();
    }
}
