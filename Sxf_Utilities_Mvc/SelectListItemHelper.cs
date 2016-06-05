using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sxf_Utilities;

namespace Sxf_Utilities_Mvc
{
    public class SelectListItemHelper
    {
        public static IEnumerable<SelectListItem> FindByEnum<T>(Int32? def = null, Boolean pleaseCheck = true, IEnumerable<T> ignores = null, IEnumerable<T> enums = null) where T : struct
        {
            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new Exception();
            }

            if (enums == null || !enums.Any())
            {
                enums = Enum.GetValues(typeof(T)).Cast<T>();
            }

            if (ignores != null)
            {
                enums = enums.Where(s => !ignores.Contains(s));
            }

            List<SelectListItem> vat = new List<SelectListItem>();

            foreach (T item in enums)
            {
                Enum en = (Enum)Enum.Parse(type, item.ToString());

                Int32 v = Convert.ToInt32(en);

                Boolean selected = def.HasValue ? def == v : false;

                vat.Add(new SelectListItem() { Text = en.GetDescription(), Value = v.ToString(), Selected = selected });
            }

            if (pleaseCheck)
            {
                vat.Insert(0, new SelectListItem() { Text = "全部", Value = "" });
            }

            return vat;
        } 
    }
}