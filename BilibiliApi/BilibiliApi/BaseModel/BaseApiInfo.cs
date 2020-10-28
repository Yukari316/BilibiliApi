using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.BaseModel
{
    /// <summary>
    /// API信息基类
    /// </summary>
    public abstract class BaseApiInfo
    {
        #region 属性
        /// <summary>
        /// API执行返回值
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// API消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// TTL
        /// </summary>
        public int TTL { get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        internal BaseApiInfo(JObject apiResponse)
        {
            if (apiResponse == null)
            {
                throw new NullReferenceException("response data is null");
            }
            //转换JSON数据
            try
            {
                this.Code    = Convert.ToInt32(apiResponse["code"] ?? -1);
                this.Message = apiResponse["message"]?.ToString() ?? string.Empty;
                this.TTL     = Convert.ToInt32(apiResponse["ttl"] ?? -1);
            }
            catch (Exception e)
            {
                throw new JsonException("json parse error", e);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal BaseApiInfo(int code, string message, int ttl)
        {
            this.Code    = code;
            this.Message = message;
            this.TTL     = ttl;
        }
        #endregion
    }
}
