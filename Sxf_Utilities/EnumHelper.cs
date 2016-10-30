using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;


namespace Sxf_Utilities
{
    public static class EnumHelper
    {
        /// <summary>
        ///  获取枚举类型上面的描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            return GetDescriptions(obj).LastOrDefault();
        }

        public static IList<String> GetDescriptions(this Enum obj)
        {
            if (obj == null)
            {
                return new List<String>();
            }

            String[] flags = new Regex(", ").Split(obj.ToString());

            Type t = obj.GetType();

            List<String> descriptions = new List<string>(flags.Length);

            flags.ForEach(s =>
            {
                FieldInfo fi = t.GetField(s);

                DescriptionAttribute[] arrDesc =
                    (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);


                descriptions.Add(arrDesc.Length == 1 ? arrDesc[0].Description : "");

            });

            return descriptions;
        }

        public static IEnumerable<T> GetEnums<T>(this Enum obj)
        {
            String[] flags = new Regex(", ").Split(obj.ToString());

            Type t = typeof (T);

            return flags.Select(s => (T) Enum.Parse(t, s));
        }

        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            return Enum.GetValues(typeof (T)).Cast<T>();
        }


        public static T ClearFlag<T>(this Enum variable, T flag)
        {
            return ClearFlags(variable, flag);
        }

        public static T ClearFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            foreach (T flag in flags)
                result &= ~Convert.ToUInt64(flag);
            return (T) Enum.Parse(variable.GetType(), result.ToString());
        }


        public static T SetFlag<T>(this Enum variable, T flag)
        {
            return SetFlags(variable, flag);
        }

        public static T SetFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            foreach (T flag in flags)
                result |= Convert.ToUInt64(flag);
            return (T) Enum.Parse(variable.GetType(), result.ToString());
        }

        public static bool HasFlags<E>(this E variable, params E[] flags)
            where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof (E).IsEnum)
                throw new ArgumentException("variable must be an Enum", "variable");

            foreach (var flag in flags)
            {
                if (!Enum.IsDefined(typeof (E), flag))
                    return false;

                ulong numFlag = Convert.ToUInt64(flag);
                if ((Convert.ToUInt64(variable) & numFlag) != numFlag)
                    return false;
            }

            return true;
        }
    }
}