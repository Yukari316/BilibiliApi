using System;
using System.IO;
using BilibiliApi.BaseModel;
using BilibiliApi.Live.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Live.Models
{
    /// <summary>
    /// 直播间状态信息
    /// </summary>
    public class LiveStatus : BaseApiInfo
    {
        #region 属性
        /// <summary>
        /// 直播状态
        /// 字段[JSON.data.status]
        /// </summary>
        public LiveStatusType Status { get; }

        /// <summary>
        /// 直播间ID
        /// </summary>
        public long RoomId { get; }

        /// <summary>
        /// 直播间链接
        /// 字段[JSON.data.url]
        /// </summary>
        public string RoomUrl { get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        internal LiveStatus(JToken apiResponse) : base(apiResponse)
        {
            if (apiResponse == null)
            {
                throw new NullReferenceException("response data is null");
            }
            //处理json信息 
            try
            {
                //检查code值
                if (base.Code != 0) return;
                //写入数据
                Status = Enum.IsDefined(typeof(LiveStatusType),
                                        (int) (apiResponse["data"]?["status"] ?? (int) LiveStatusType.Error))
                    ? (LiveStatusType) (int) apiResponse["data"]?["status"]
                    : LiveStatusType.Unknown;
                RoomUrl = apiResponse["data"]?["url"]?.ToString() ?? string.Empty;
                RoomId  = Convert.ToInt64(Path.GetFileName(RoomUrl));
            }
            catch (Exception e)
            {
                throw new JsonException("json parse error", e);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal LiveStatus(string errorMessage) : base(-1, errorMessage, -1)
        {
            Status = LiveStatusType.Error;
        }
        #endregion
    }
}
