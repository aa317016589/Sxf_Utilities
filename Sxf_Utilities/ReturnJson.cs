using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sxf_Utilities
{
    public class ReturnJson
    {
        private const Int32 Success = 2000;

        private const Int32 Failure = 1041;

        private readonly String K = "code", V = "mes";

        public const String List = "data", PageCount = "pageCount", PageIndex = "pageIndex", PageSize = "pageSize", TotalCount = "totalCount";

        private readonly String _defaultForAjaxSuccess;

        private readonly String _defaultForAjax;

        Dictionary<String, object> _messagesDic;

        #region 设置成功后需要返回的信息


        public virtual void SetSuccess(object suceessData)
        {
            _messagesDic[K] = Success;

            _messagesDic[V] = suceessData;
        }

        public virtual void SetSuccess()
        {
            SetSuccess(_defaultForAjaxSuccess);
        }

        #endregion

        #region 设置错误消息


        /// <summary>
        /// 通过设置错误文本里的xml节点来获取相应的错误信息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetErrorMessage(String message)
        {
            _messagesDic[V] = message;
        }

        /// <summary>
        /// 设置错误列表集合
        /// </summary>
        /// <param name="errors"></param>
        public virtual void SetErrorMessage(IEnumerable<String> errors)
        {
            _messagesDic[V] = String.Join("<br />", errors);
        }


        #endregion

        #region 输出Json

        /// <summary>
        /// 输出Json 
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> ResponseMessage()
        {
            if (_messagesDic[V] == null)
            {
                SetErrorMessage(_defaultForAjax);
            }

            return _messagesDic;
        }
        #endregion

        #region 添加

        public ReturnJson AddDic(Dictionary<String, Object> otherData)
        {
            _messagesDic = _messagesDic.Concat(otherData).ToDictionary(k => k.Key, v => v.Value);

            return this;
        }


        public ReturnJson AddDic(String key, object value)
        {
            if (_messagesDic.ContainsKey(key))
            {
                _messagesDic[key] = value;
                return this;
            }

            _messagesDic.Add(key, value);

            return this;
        }


        public void AddDic(object data)
        {
            if (data == null)
            {
                return;
            }

            Type type = data.GetType();

            PropertyInfo[] propertyInfos = type.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                _messagesDic.Add(propertyInfo.Name, propertyInfo.GetValue(data, null));
            }
        }

        #endregion

        #region 构造器

        /// <summary>
        /// 返回json格式的状态信息
        /// </summary>
        public ReturnJson()
            : this(false)
        {

        }

        /// <summary>
        /// 返回json格式的状态信息
        /// </summary>
        /// <param name="flag">初始化状态对错</param>
        public ReturnJson(Boolean flag)
        {
            _defaultForAjaxSuccess = "操作成功";
            _defaultForAjax = "操作失败";


            _messagesDic = new Dictionary<string, object>() { };
            _messagesDic.Add(K, flag ? Success : Failure);
            _messagesDic.Add(V, (flag ? _defaultForAjaxSuccess : _defaultForAjax));
        }

        #endregion
    }
}