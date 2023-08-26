using Python.Runtime;

namespace Stock.Common
{
    public static class Tools
    {
        [PropertyInjection]
        public static IConfigService configs { get; set; }
        public static DateTime DateStr2DateTime(string DateStr,string Format="")
        {
            if (string.IsNullOrEmpty(Format))
            {
                return Convert.ToDateTime(DateStr);
            }
            else
            {
                return DateTime.ParseExact(DateStr, Format, System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        public static T RunPython<T>(Func<dynamic, T> func)
        {
            Runtime.PythonDLL = configs.ReadPythonPath();
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic ak = Py.Import("akshare");
                return func(ak);
            }
        }
    }

}
