using System;
using System.IO;
using BilibiliApi.Live.Enums;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Live.Models
{
    public class LiveStatus
    {
        /// <summary>
        /// 直播状态
        /// 字段[JSON.data.status]
        /// </summary>
        public LiveStatusType Status { get; } 

        /// <summary>
        /// 消息
        /// 字段[JSON.message]
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 房间号
        /// </summary>
        public ulong RoomId { get; }

        /// <summary>
        /// 直播间链接
        /// 字段[JSON.data.url]
        /// </summary>
        public string RoomUrl { get; }

        internal LiveStatus(JObject jsonInfo)
        {
            if (jsonInfo == null)
            {
                throw new NullReferenceException("json is null");
            }
            //处理json信息 
            try
            {
                //检查code值
                string code = jsonInfo["code"]?.ToString();
                if (code == null || !code.Equals("0"))
                {
                    Status  = LiveStatusType.Error;
                    Message = $"code {code ?? "-2"}";
                    return;
                }
                //写入数据
                Status = Enum.IsDefined(typeof(LiveStatusType),
                                        (int) (jsonInfo["data"]?["status"] ?? (int) LiveStatusType.Error))
                    ? (LiveStatusType) ((int) jsonInfo["data"]?["status"])
                    : LiveStatusType.Unknown;
                Message = jsonInfo["message"]?.ToString()      ?? string.Empty;
                RoomUrl = jsonInfo["data"]?["url"]?.ToString() ?? string.Empty;
                RoomId  = Convert.ToUInt64(Path.GetFileName(RoomUrl));
            }
            catch (Exception e)
            {
                Status  = LiveStatusType.Error;
                Message = e.Message;
            }
        }

        internal LiveStatus(string errorMessage)
        {
            Status  = LiveStatusType.Error;
            Message = errorMessage;
        }
    }
}
