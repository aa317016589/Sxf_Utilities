using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Sxf_Utilities
{
    public class DataHelper
    {
        public static int StrToInt(string sValue)
        {
            return StrToInt(sValue, 0);
        }

        public static int StrToInt(string sValue, int defaultValue)
        {
            int iVal = 0;

            if (String.IsNullOrWhiteSpace(sValue))
            {
                return defaultValue;
            }

            if (!Int32.TryParse(sValue.Trim(), out iVal))
            {
                return defaultValue;
            }
            return iVal;
        }


        public static long StrToLong(string sValue)
        {
            return StrToLong(sValue, 0);
        }

        public static long StrToLong(string sValue, int defaultValue)
        {
            long iVal = 0;

            if (String.IsNullOrWhiteSpace(sValue))
            {
                return defaultValue;
            }

            if (!Int64.TryParse(sValue.Trim(), out iVal))
            {
                return defaultValue;
            }
            return iVal;
        }


        public static Guid StrToGuid(string sValue)
        {
            Guid guid = default(Guid);
            Guid.TryParse(sValue, out guid);
            return guid;
        }
        public static Encoding SetEncoding(string coding)
        {
            string codingName = (String.IsNullOrWhiteSpace(coding) || coding == "GBK") ? "GBK" : "UTF-8";

            return Encoding.GetEncoding(codingName);
        }


        public static bool QuickValidate(string express, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            Regex myRegex = new Regex(express);

            return myRegex.IsMatch(value);
        }

        public static bool IsNumberId(string value)
        {
            return QuickValidate("^[a-zA-Z0-9_]*$", value);
        }

        public static bool IsUrl(string value)
        {

            String check = @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
            return QuickValidate(check, value);
        }


        public static string GetString(object value)
        {
            string stringValue = string.Empty;
            if (value != null && !Convert.IsDBNull(value))
            {
                stringValue = value.ToString().Trim();
            }
            return stringValue;
        }

        public static DateTime? GetNullableDateTime(object value)
        {
            DateTime? dateTimeValue = null;
            DateTime dbDateTimeValue;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (DateTime.TryParse(value.ToString(), out dbDateTimeValue))
                {
                    dateTimeValue = dbDateTimeValue;
                }
            }
            return dateTimeValue;
        }

        //public static int? GetNullableInteger(object value)
        //{
        //    int? integerValue = null;
        //    int parseIntegerValue = 0;
        //    if (value != null && !Convert.IsDBNull(value))
        //    {
        //        if (int.TryParse(value.ToString(), out parseIntegerValue))
        //        {
        //            integerValue = parseIntegerValue;
        //        }
        //    }
        //    return integerValue;
        //}

        public static Boolean StrToBool(String sValue)
        {
            return StrToBool(sValue, false);
        }

        public static Boolean StrToBool(String sValue, Boolean defaultValue)
        {
            Boolean iVal = false;

            if (String.IsNullOrWhiteSpace(sValue))
            {
                return defaultValue;
            }

            if (!Boolean.TryParse(sValue.Trim(), out iVal))
            {
                return defaultValue;
            }
            return iVal;
        }
    }
}