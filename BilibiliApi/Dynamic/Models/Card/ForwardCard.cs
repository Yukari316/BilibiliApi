using System;
using System.Text;
using BilibiliApi.Dynamic.Enums;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic.Models.Card
{
    /// <summary>
    /// 转发动态
    /// </summary>
    public class ForwardCard : Dynamic
    {
        #region 属性

        /// <summary>
        /// <para>动态内容</para>
        /// <para>[字段:JSON.item.content]</para>
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// <para>源动态链接</para>
        /// <para>[字段:JSON.item.orig_dy_id]</para>
        /// </summary>
        private string OrginDynamicId { get; }

        /// <summary>
        /// <para>源动态类型</para>
        /// <para>[字段:JSON.item.orig_type]</para>
        /// </summary>
        private CardType OrginType { get; }

        /// <summary>
        /// <para>源动态JSON</para>
        /// <para>[字段:JSON.origin]</para>
        /// </summary>
        private JObject OrginJson { get; }

        #endregion

        #region 构造方法

        internal ForwardCard(JToken apiResponse, int index) : base(apiResponse, index)
        {
            if (base.Code != 0) return;
            //写入动态信息
            //描述
            Content = Card["item"]?["content"]?.ToString();
            //源动态ID
            OrginDynamicId = Card["item"]?["orig_dy_id"]?.ToString();
            //源动态类型
            OrginType = Enum.IsDefined(typeof(CardType), (int) (Card["item"]?["orig_type"] ?? 0))
                ? (CardType) (int) (Card["item"]?["orig_type"] ?? 0)
                : CardType.Unknown;
            //源动态数据
            OrginJson = JObject.Parse(Card["origin"]?.ToString() ?? "{}");
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 将动态转换为格式化文本
        /// </summary>
        public override string ToString()
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append("转发的动态:\n");
            messageBuilder.Append(GetUserInfo().UserName);
            messageBuilder.Append(":");
            messageBuilder.Append(Content);
            return messageBuilder.ToString();
        }

        /// <summary>
        /// 获取源动态的链接
        /// </summary>
        /// <param name="orginCardType">源动态类型</param>
        public string GetOrginUrl(out CardType orginCardType)
        {
            orginCardType = OrginType;
            if (Content == null || OrginDynamicId == null || CardType == CardType.Unknown) return null;
            return $"https://t.bilibili.com/{OrginDynamicId}";
        }

        /// <summary>
        /// 获取源动态的数据
        /// </summary>
        /// <param name="orginCardType">源动态类型</param>
        public JObject GetOrginJson(out CardType orginCardType)
        {
            orginCardType = OrginType;
            if (Content == null || OrginDynamicId == null || CardType == CardType.Unknown) return null;
            return OrginJson;
        }

        #endregion
    }
}