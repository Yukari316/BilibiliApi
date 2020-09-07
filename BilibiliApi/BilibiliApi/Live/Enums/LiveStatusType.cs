namespace BilibiliApi.Live.Enums
{
    public enum LiveStatusType
    {
        /// <summary>
        /// 处理错误
        /// </summary>
        Error = -2,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 0,
        /// <summary>
        /// 正在直播
        /// </summary>
        Online = 1,
        /// <summary>
        /// 轮播
        /// </summary>
        Carousel = 2
    }
}
