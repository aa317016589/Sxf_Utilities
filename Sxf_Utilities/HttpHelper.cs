using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sxf_Utilities
{
    public static class HttpHelper
    {
        private static readonly string _defaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        #region 异步
        public static async Task<String> CreateHttpResponseResultAsync(String url, HttpTypes httpType, String parameters, Encoding requestEncoding, int? timeout = null, string userAgent = "", CookieCollection cookies = null, Boolean isJson = true)
        {
            try
            {
                var request = await CreateHttpResponseAsync(url, httpType, parameters, requestEncoding, timeout, userAgent, cookies,
                    isJson);


                String result = await GetStreamAsync(request, requestEncoding);



                return result;
            }
            catch (AggregateException ex)
            {
                foreach (Exception inner in ex.InnerExceptions)
                {

                }


                return "";
            }
        }

        public static async Task<HttpWebResponse> CreateHttpResponseAsync(String url, HttpTypes httpType,
            String parameters, Encoding requestEncoding, int? timeout = null, string userAgent = "",
            CookieCollection cookies = null, Boolean isJson = true)
        {

            return await CreateHttpWebRequest(url, httpType, parameters, requestEncoding, timeout, userAgent, cookies,
                isJson).GetResponseAsync() as HttpWebResponse;
        }


        public static async Task<String> CreateHttpResponseResultAsync(String url, HttpTypes httpType, String parameters)
        {
            return await CreateHttpResponseResultAsync(url, httpType, parameters, Encoding.UTF8);
        }

        public static async Task<String> CreateHttpResponseResultAsync(String url)
        {
            return await CreateHttpResponseResultAsync(url, HttpTypes.Get, "", Encoding.UTF8);
        }

        public static async Task<String> GetStreamAsync(HttpWebResponse response, Encoding encoding)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await StreamToStringAsync(response, encoding);
                }
                else
                {
                    response.Close();
                    return "";
                }
            }
            catch (AggregateException ex)
            {
                foreach (Exception inner in ex.InnerExceptions)
                {

                }

                return "";
            }
        }

        private static async Task<String> StreamToStringAsync(HttpWebResponse response, Encoding encoding)
        {
            using (response)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                {
                    string result = await sr.ReadToEndAsync();

                    return result;
                }
            }
        }
        #endregion

        #region 同步
        public static String CreateHttpResponseResult(String url)
        {
            return GerHttpResponseResult(url, HttpTypes.Get, "", Encoding.UTF8);
        }

        public static String CreateHttpResponseResult(String url, HttpTypes httpType, String parameters)
        {
            return GerHttpResponseResult(url, httpType, parameters, Encoding.UTF8);
        }

        private static String GetStream(HttpWebResponse response, Encoding encoding)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return StreamToString(response, encoding);
                }
                else
                {
                    response.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return "";


        }

        public static String GerHttpResponseResult(String url, HttpTypes httpType, String parameters,
            Encoding requestEncoding, int? timeout = null, string userAgent = "", CookieCollection cookies = null,
            Boolean isJson = false)
        {

            HttpWebResponse request = null;

            try
            {
                request = CreateHttpWebRequest(url, httpType, parameters, requestEncoding, timeout, userAgent, cookies,
  isJson).GetResponse() as HttpWebResponse;


            }
            catch (Exception ex)
            {

                return "";
            }


            String result = GetStream(request, requestEncoding);



            return result;
        }


        private static String StreamToString(HttpWebResponse response, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
            {
                string result = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                response.Close();
                return result;
            }
        }



        #endregion

        #region 构造对象

        private static HttpWebRequest CreateHttpWebRequest(String url, HttpTypes httpType, String parameters,
Encoding requestEncoding, int? timeout = null, string userAgent = "", CookieCollection cookies = null,
Boolean isJson = true)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }


            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method = httpType.ToString().ToUpper();

            request.ContentType = isJson ? "application/json;charset=UTF-8" : "application/x-www-form-urlencoded";

            request.UserAgent = string.IsNullOrEmpty(userAgent) ? _defaultUserAgent : userAgent;


            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            if (!String.IsNullOrEmpty(parameters))
            {
                byte[] data = requestEncoding.GetBytes(parameters);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            // request.GetResponse() as HttpWebResponse;

            return request;
        }
        #endregion

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        #region 辅助
        public static String FormatPostParameters(IDictionary<String, String> parameters)
        {
            return String.Join("&", parameters.Select(s => String.Format("{0}={1}", s.Key, s.Value)));
        }

        public static IDictionary<String, String> FormatToDictionary<T>(T t)
        {
            IDictionary<String, String> dic = new Dictionary<String, String>();

            Type type = t.GetType();

            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                dic.Add(propertyInfo.Name, propertyInfo.GetValue(t, null).ToString());
            }

            return dic;
        }

        public static String FormatToGet(object param)
        {
            if (param == null)
            {
                return "";
            }

            IDictionary<String, String> dic = new Dictionary<String, String>();

            Type t = param.GetType();

            PropertyInfo[] pi = t.GetProperties();

            foreach (PropertyInfo item in pi)
            {
                dic.Add(item.Name, item.GetValue(param, null).ToString());
            }

            return FormatPostParameters(dic);
        }
        #endregion
    }

    public enum HttpTypes
    {
        Get = 0,
        Post,
        Put,
        Patch,
        Delete
    }
}