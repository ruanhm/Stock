using Python.Runtime;

namespace Stock.Common
{
    public static class Tools
    {
        private static IConfigService configs { get; set; } = IocManager.Resolve<IConfigService>();

        //static Tools()
        //{
        //    Runtime.PythonDLL = configs.ReadPythonPath();
        //    PythonEngine.Initialize();
        //}
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
        public static double? Str2Double(string? Str)
        {
            double d;
            var rs=double.TryParse(Str, out d);
            return rs ? d : null;
        }
        public static async Task<T> RunPythonAsync<T>(Func<dynamic, T> func)
        {
            T rs = default(T);
            try
            {
                Runtime.PythonDLL = configs.ReadPythonPath();
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    dynamic ak = Py.Import("akshare");
                    rs =await func(ak);
                    var a = 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                if (PythonEngine.IsInitialized)
                    PythonEngine.Shutdown();
            }
            return rs;
        }
        public static T RunPython<T>(Func<dynamic, T> func)
        {
            T rs=default(T);
            try
            {
                
                using (Py.GIL())
                {
                    dynamic ak = Py.Import("akshare");
                    rs = func(ak);
                    var a = 1;
                    return rs;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                //if (PythonEngine.IsInitialized)
                //    PythonEngine.Shutdown();
            }
            return rs;
        }
        public static async Task RunPython(Action<dynamic> action)
        {
            try
            {
                Runtime.PythonDLL = configs.ReadPythonPath();
                PythonEngine.Initialize();
                using (Py.GIL())
                {
                    dynamic ak = Py.Import("akshare");
                    await action(ak);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                if (PythonEngine.IsInitialized)
                    PythonEngine.Shutdown();
            }
        }
    }

}
