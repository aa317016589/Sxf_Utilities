﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace System.Web.Mvc
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
        }

        public JsonNetResult(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet, string contentType = null, Encoding contentEncoding = null)
        {
            this.Data = data;
            this.JsonRequestBehavior = behavior;
            this.ContentEncoding = contentEncoding;
            this.ContentType = contentType;
            this.Settings = new JsonSerializerSettings();
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            string jsonp = context.HttpContext.Request.QueryString["callback"] ?? "";

            IsoDateTimeConverter iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";



            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.Settings.Converters = new List<JsonConverter>()
            {
                iso,
                new StringEnumConverter()
            };

            var scriptSerializer = JsonSerializer.Create(this.Settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);

                if (String.IsNullOrEmpty(jsonp))
                {
                    response.Write(sw.ToString());
                }
                else
                {
                    response.Write(jsonp + "(" + sw.ToString() + ")");
                }
            }
        }
    }
}