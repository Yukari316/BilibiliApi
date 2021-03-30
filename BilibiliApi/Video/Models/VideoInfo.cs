using System;
using BilibiliApi.BaseModel;
using Newtonsoft.Json.Linq;
using YukariToolBox.Time;

namespace BilibiliApi.Video.Models
{
    /// <summary>
    /// 视频信息
    /// </summary>
    public class VideoInfo : BaseApiInfo
    {
        #region 属性

        /// <summary>
        /// av号
        /// </summary>
        public long Aid { get; private set; }

        /// <summary>
        /// bv号
        /// </summary>
        public string Bid { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// 作者名
        /// </summary>
        public string AuthName { get; private set; }

        /// <summary>
        /// 作者UID
        /// </summary>
        public long AuthUid { get; private set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; private set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCount { get; private set; }

        /// <summary>
        /// 投币数
        /// </summary>
        public int CoinCount { get; private set; }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        internal VideoInfo(JToken apiResponse) : base(apiResponse)
        {
            Aid         = Convert.ToInt64(apiResponse["data"]?["aid"] ?? -1);
            Bid         = apiResponse["data"]?["bvid"]?.ToString()           ?? string.Empty;
            Title       = apiResponse["data"]?["title"]?.ToString()          ?? string.Empty;
            Desc        = apiResponse["data"]?["desc"]?.ToString()           ?? string.Empty;
            AuthName    = apiResponse["data"]?["owner"]?["name"]?.ToString() ?? string.Empty;
            AuthUid     = Convert.ToInt64(apiResponse["data"]?["owner"]?["mid"] ?? -1);
            PublishTime = Convert.ToInt64(apiResponse["data"]?["pubdate"]       ?? -1).ToDateTime();
            LikeCount   = Convert.ToInt32(apiResponse["data"]?["stat"]?["like"] ?? -1);
            CoinCount   = Convert.ToInt32(apiResponse["data"]?["stat"]?["coin"] ?? -1);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        internal VideoInfo(string errorMessage) : base(-1, errorMessage, -1)
        {
        }
    }
}