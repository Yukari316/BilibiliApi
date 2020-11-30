using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic.Models.Card
{
    /// <summary>
    /// 视频动态
    /// </summary>
    public class VideoCard : Dynamic
    {
        #region 属性
        /// <summary>
        /// <para>AV号</para>
        /// <para>[字段:JSON.aid]</para>
        /// </summary>
        public long AvID{ get; }
        /// <summary>
        /// <para>视频标题</para>
        /// <para>[字段:JSON.title]</para>
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// <para>封面链接</para>
        /// <para>[字段:JSON.pic]</para>
        /// </summary>
        public string CoverUrl { get; }
        /// <summary>
        /// <para>视频简介</para>
        /// <para>[字段:JSON.desc]</para>
        /// </summary>
        public string Desc { get; }
        /// <summary>
        /// <para>视频话题</para>
        /// <para>[字段:JSON.dynamic]</para>
        /// </summary>
        public List<string> CardDynamic { get; }
        #endregion

        #region 构造函数

        internal VideoCard(JToken apiResponse, int index) : base(apiResponse, index)
        {
            if(base.Code != 0) return;
            //写入动态信息
            //AV号
            AvID = (long) (Card["aid"] ?? 0);
            //标题
            Title = Card["title"]?.ToString();
            //视频封面链接
            CoverUrl = Card["pic"]?.ToString();
            //视频简介
            Desc = Card["desc"]?.ToString();
            //视频话题
            MatchCollection QuotationMarkMatch = Regex.Matches(Card["dynamic"]?.ToString() ?? string.Empty, @"#.*?#");
            CardDynamic = QuotationMarkMatch.Select(match => match.Value.Substring(1, match.Value.Length - 2))
                                            .ToList();
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 将动态转换为格式化文本
        /// </summary>
        public override string ToString()
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(Title);
            messageBuilder.Append(Desc);
            messageBuilder.Append("\n视频话题：");
            messageBuilder.Append(string.Join("|", CardDynamic));
            messageBuilder.Append("\n\nLink：");
            messageBuilder.Append(GetAvUrl());
            return messageBuilder.ToString();
        }
        /// <summary>
        /// 获取视频链接
        /// </summary>
        public string GetAvUrl() => $"https://www.bilibili.com/video/av{AvID}";

        #endregion
    }
}