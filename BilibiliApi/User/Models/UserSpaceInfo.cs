using System;
using BilibiliApi.BaseModel;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.User.Models
{
    /// <summary>
    /// 用户详细信息(空间)
    /// (不完整信息解析)
    /// </summary>
    public class UserSpaceInfo : BaseApiInfo
    {
        #region 属性
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; private set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string FaceUrl { get; private set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; private set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 封禁状态
        /// </summary>
        public bool Silence { get; private set; }

        /// <summary>
        /// 账户直播间信息
        /// </summary>
        public LiveRoomInfo LiveRoomInfo { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化
        /// </summary>
        internal UserSpaceInfo(JObject apiResponse) : base(apiResponse)
        {
            Name         = apiResponse["data"]?["name"]?.ToString() ?? string.Empty;
            Sex          = apiResponse["data"]?["sex"]?.ToString()  ?? string.Empty;
            FaceUrl      = apiResponse["data"]?["face"]?.ToString() ?? string.Empty;
            Sign         = apiResponse["data"]?["sign"]?.ToString() ?? string.Empty;
            Level        = Convert.ToInt32(apiResponse["data"]?["level"]     ?? -1);
            Silence      = Convert.ToBoolean(apiResponse["data"]?["silence"] ?? false);
            LiveRoomInfo = new LiveRoomInfo(apiResponse["data"]?["live_room"]);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal UserSpaceInfo(string errorMessage) : base(-1, errorMessage, -1)
        { }
        #endregion
    }
}
