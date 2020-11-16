using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic.Models.Card
{
    /// <summary>
    /// 图片动态
    /// </summary>
    public class TextAndPicCard : Dynamic
    {
        #region 属性
        /// <summary>
        /// <para>动态内容</para>
        /// <para>[字段:JSON.item.description]</para>
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// <para>图片链接</para>
        /// <para>[字段:JSON.item.pictures]</para>
        /// </summary>
        public List<string> ImgList { get; }
        #endregion

        #region 构造方法
        internal TextAndPicCard(JObject apiResponse, int index) : base(apiResponse, index)
        {
            if(base.Code != 0) return;
            //写入动态信息
            //描述
            Description = Card["item"]?["description"]?.ToString();
            //图片
            ImgList = new List<string>();
            JToken[] pictures = Card["item"]?["pictures"]?.ToArray() ?? JArray.Parse("[]").ToArray();
            foreach (JToken url in pictures)
            {
                ImgList.Add(url["img_src"]?.ToString() ?? "");
            }
        }
        #endregion

        #region 公有方法

        /// <summary>
        /// 获取动态的文本
        /// </summary>
        /// <returns>将数据转换为文本</returns>
        public override string ToString() => Description;
        #endregion
    }
}
