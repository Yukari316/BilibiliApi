using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.User.Models
{
    /// <summary>
    /// 直播间信息
    /// </summary>
    public class LiveRoomInfo
    {
        #region 属性

        /// <summary>
        /// 是否有直播间
        /// </summary>
        public bool HaveLiveRoom { get; private set; }

        /// <summary>
        /// 直播间在线状态
        /// </summary>
        public bool Online { get; private set; }
        
        /// <summary>
        /// 轮播状态
        /// </summary>
        public bool RoundStatus { get; private set; }

        /// <summary>
        /// 直播间链接
        /// </summary>
        public string LiveUrl { get; private set; }

        /// <summary>
        /// 直播间标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 封面链接
        /// </summary>
        public string CoverUrl { get; private set; }

        /// <summary>
        /// 人气
        /// </summary>
        public long OnlineUser { get; private set; }

        /// <summary>
        /// 短ID
        /// </summary>
        public int ShortId { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        internal LiveRoomInfo(JToken liveroomData)
        {
            if (liveroomData == null || !liveroomData.HasValues) return;

            HaveLiveRoom = Convert.ToBoolean(liveroomData["roomStatus"]  ?? false);
            Online       = Convert.ToBoolean(liveroomData["liveStatus"]  ?? false);
            RoundStatus  = Convert.ToBoolean(liveroomData["roundStatus"] ?? false);
            LiveUrl      = liveroomData["url"]?.ToString()   ?? string.Empty;
            Title        = liveroomData["title"]?.ToString() ?? string.Empty;
            CoverUrl     = liveroomData["cover"]?.ToString() ?? string.Empty;
            OnlineUser   = Convert.ToInt64(liveroomData["online"] ?? -1);
            ShortId      = Convert.ToInt32(liveroomData["roomid"] ?? -1);
        }
        #endregion
    }
}
