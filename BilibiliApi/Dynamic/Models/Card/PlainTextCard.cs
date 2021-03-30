using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic.Models.Card
{
    /// <summary>
    /// 纯文本类型动态
    /// </summary>
    public class PlainTextCard : Dynamic
    {
        #region 属性

        /// <summary>
        /// <para>动态内容</para>
        /// <para>[字段:JSON.item.content]</para>
        /// </summary>
        public string Content { get; }

        #endregion

        #region 构造函数

        internal PlainTextCard(JToken apiResponse, int index) : base(apiResponse, index)
        {
            if (base.Code != 0) return;
            //写入动态信息
            Content = Card["item"]?["content"]?.ToString();
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 获取动态的文本
        /// </summary>
        /// <returns>将数据转换为格式化文本</returns>
        public override string ToString() =>
            this.Content;

        #endregion
    }
}