using Python.Runtime;
using Newtonsoft.Json;
using System.Web;
using System.Buffers.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO.Compression;

namespace Stock.Common
{
    public static class Tools
    {
        private static IConfigService configs { get; set; } = IocManager.Resolve<IConfigService>();
        private static IHttpClientFactory httpClientFactory { get; set; }= IocManager.Resolve<IHttpClientFactory>();

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
                    return rs;
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

        public static string ToUrlParams(this object obj)
        {
            var arr = obj.GetType().GetProperties().Select(x => new { x.Name, value = x.GetValue(obj, null) })
            .Where(x => x.value != null).Select(x => $"{x.Name}={HttpUtility.UrlEncode(x.value.ToString())}");
            return string.Join("&", arr);
        }
        public static string Base64Decrypt(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);

        }
        public static string DecompressGZip(string strSource)
        {
            try
            {

                byte[] data = Convert.FromBase64String(strSource);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream gZipStream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[40960];
                        int n;
                        while ((n = gZipStream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            stream.Write(bytes, 0, n);
                        }
                        gZipStream.Close();
                    }
                    var arr=stream.ToArray();
                    return System.Text.Encoding.UTF8.GetString(arr, 0, arr.Length);
                }
            }
            catch
            {
                string bnull = "";
                return bnull;
            }
        }
        public static string ResponseHandler(string response)
        {
            if (response != null)
            {
                response = Tools.DecompressGZip(response.Trim('"'));
            }
            return response;
        }
        public static async Task<T> SendGetRequestAsync<T>(string url, object? ps =null,Func<string,string>? func=null, CancellationToken cancellationToken=default)
        {
            if (ps != null)
            {
                if (!url.EndsWith("?"))
                {
                    url += "?";
                }
                url += ps.ToUrlParams();
            }
            var httpClient=httpClientFactory.CreateClient();
            var res =await httpClient.GetStringAsync(url,cancellationToken);
            if(func != null)
            {
                res= func(res);
            }
            return JsonConvert.DeserializeObject<T>(res);
        }

    }

}
