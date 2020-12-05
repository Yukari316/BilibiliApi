using System.ComponentModel;

namespace BilibiliApi.Dynamic.Enums
{
    /// <summary>
    /// 动态卡片类型
    /// </summary>
    [DefaultValue(Unknown)]
    public enum CardType
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = -1,
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 转发
        /// </summary>
        Forward = 1,
        /// <summary>
        /// 图片动态
        /// </summary>
        TextAndPic = 2,
        /// <summary>
        /// 纯文本动态
        /// </summary>
        PlainText = 4,
        /// <summary>
        /// 视频动态
        /// </summary>
        Video = 8,
        // /// <summary>
        // /// 直播间动态
        // /// </summary>
        // Live = 4200
    }
}
