using System;
using System.Web;

namespace Sxf_Utilities
{
    public class ContextHelper
    {
        public static HttpContext Current
        {
            get { return HttpContext.Current; }
        }

        public static HttpRequest Request
        {
            get { return Current.Request; }
        }

        public static HttpResponse Response
        {
            get { return Current.Response; }
        }

        public static string Q(string name)
        {
            return Q(name, String.Empty);
        }

        public static string Q(string name, String defaults)
        {
            return Request.QueryString[name] == null ? defaults : Request.QueryString[name].Trim();
        }
    }
}