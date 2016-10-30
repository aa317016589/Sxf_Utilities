using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Text.RegularExpressions;

namespace Sxf_Utilities
{
    public class CacheHelper
    {
        /// <summary>
        /// 建立缓存
        /// </summary>
        public static void TryAddCache(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemovedCallback)
        {
            if ((!String.IsNullOrWhiteSpace(key)) && value != null)
            {
                HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemovedCallback);
            }
        }

        public static void TryAddCache(string key, object value, DateTime enddate)
        {
            if ((!String.IsNullOrWhiteSpace(key)) && value != null)
            {
                HttpRuntime.Cache.Insert(key, value, null, enddate, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        public static void TryAddCache(string key, object value, TimeSpan timeSpan)
        {
            if ((!String.IsNullOrWhiteSpace(key)) && value != null)
            {
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, timeSpan, CacheItemPriority.Normal, null);
            }
        }

        public static void TryAddCache(string key, object value, CacheDependency dependencies)
        {
            if ((!String.IsNullOrWhiteSpace(key)) && value != null)
            {
                HttpRuntime.Cache.Insert(key, value, dependencies);
            }
        }


        public static object GetCache(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return null;
            }

            return HttpRuntime.Cache[key];
        }

        public static object Update(String key, DateTime enddate)
        {
            var v = GetCache(key);

            if (v == null)
            {
                return null;
            }

            TryAddCache(key, v, enddate);

            return v;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public static void TryRemoveCache(string key)
        {
            if (GetCache(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

  
        /// <summary>
        /// 移除键中带某关键字的缓存
        /// </summary>
        public static void RemoveMultiCache(string keyInclude)
        {
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key.ToString().Contains(keyInclude.ToString()))
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public static void RemoveMultiCache(Regex regex)
        {
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (regex.IsMatch(cacheEnum.Key.ToString()))
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());

            }
        }


        /// <summary>
        /// 移除所有缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}
 
