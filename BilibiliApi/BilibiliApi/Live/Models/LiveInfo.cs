using System;
using BilibiliApi.BaseModel;
using BilibiliApi.Live.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Live.Models
{
    /// <summary>
    /// 直播类型
    /// </summary>
    public class LiveInfo : BaseApiInfo
    {
        #region 属性
        /// <summary>
        /// 直播间真实ID
        /// </summary>
        public long RoomId { get; }

        /// <summary>
        /// 直播间短ID
        /// </summary>
        public long ShortId { get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// 是否p2p
        /// </summary>
        public int NeedP2P { get; }

        /// <summary>
        /// 直播状态
        /// </summary>
        public LiveStatusType LiveStatus { get; }

        /// <summary>
        /// 房间是否加密
        /// </summary>
        public bool Encrypted { get; }

        /// <summary>
        /// 加密房间是否通过密码验证
        /// </summary>
        public bool PasswdVerified { get; }

        /// <summary>
        /// <para>直播开始时间戳</para>
        /// <para>[字段:JSON.data.cards[n].desc.timestamp]</para>
        /// <para>仅用于设置时间，会自动转换为UpdateTime[DateTime]</para>
        /// </summary>
        private long LiveStartTimeStemp {
            set => LiveStartTime = DateTimeOffset
                                   .FromUnixTimeSeconds(value).AddHours(8).DateTime;
        }
        /// <summary>
        /// 直播开始时间
        /// </summary>
        public DateTime LiveStartTime { private set; get; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        internal LiveInfo(JObject apiResponse) : base(apiResponse)
        {
            if(apiResponse == null) throw new NullReferenceException("response data is null");

            try
            {
                //检查code值
                string code = apiResponse["code"]?.ToString();
                if (code == null || !code.Equals("0"))
                {
                    this.LiveStatus = LiveStatusType.Unknown;
                    return;
                }

                this.RoomId         = Convert.ToInt64(apiResponse["data"]?["room_id"]        ?? -1);
                this.ShortId        = Convert.ToInt64(apiResponse["data"]?["short_id"]       ?? -1);
                this.UserId         = Convert.ToInt64(apiResponse["data"]?["uid"]            ?? -1);
                this.NeedP2P        = Convert.ToInt32(apiResponse["data"]?["need_p2p"]       ?? -1);
                this.Encrypted      = Convert.ToBoolean(apiResponse["data"]?["encrypted"]    ?? false);
                this.PasswdVerified = PasswdVerified && Convert.ToBoolean(apiResponse["data"]?["pwd_verified"] ?? false);
                this.LiveStartTimeStemp = Convert.ToInt64(apiResponse["data"]?["live_time"]);
                this.LiveStatus = Enum.IsDefined(typeof(LiveStatusType), 
                                                 (int) (apiResponse["data"]?["live_status"] ?? (int) LiveStatusType.Error))
                    ? (LiveStatusType) (int) apiResponse["data"]?["live_status"]
                    : LiveStatusType.Unknown;
            }
            catch (Exception e)
            {
                throw new JsonException("json parse error", e);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal LiveInfo(string errorMessage) : base(-1, errorMessage, -1)
        { }
        #endregion
    }
}
