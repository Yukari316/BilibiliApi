using System;
using System.Collections.Generic;
using System.Linq;
using BilibiliApi.BaseModel;
using BilibiliApi.Dynamic.Enums;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Dynamic.Models
{
    /// <summary>
    /// 动态数据类型
    /// </summary>
    public abstract class Dynamic : BaseApiInfo
    {
        #region 属性
        /// <summary>
        /// <para>发送者ID</para>
        /// <para>[字段:JSON.data.cards[n].desc.uid]</para>
        /// </summary>
        protected long Uid { set; get; }
        /// <summary>
        /// <para>动态的ID</para>
        /// <para>[字段:JSON.data.cards[n].desc.dynamic_id_str]</para>
        /// <para>这个值实在是太长了就用字符串存吧</para>
        /// </summary>
        protected string DynamicId { set; get; }
        /// <summary>
        /// <para>动态更新时间</para>
        /// <para>[需要由时间戳转换]</para>
        /// </summary>
        public DateTime UpdateTime { private set; get; }
        /// <summary>
        /// <para>动态更新时间戳</para>
        /// <para>[字段:JSON.data.cards[n].desc.timestamp]</para>
        /// <para>仅用于设置时间，会自动转换为UpdateTime[DateTime]</para>
        /// </summary>
        protected long UpdateTimeStemp {
            set => UpdateTime = DateTimeOffset
                                .FromUnixTimeSeconds(value).AddHours(8).DateTime;
        }
        /// <summary>
        /// <para>动态所属用户名称</para>
        /// <para>[字段:JSON.data.cards[n].desc.user_profile.info.uname]</para>
        /// </summary>
        protected string UserName { set; get; }
        /// <summary>
        /// <para>用户头像的图片链接</para>
        /// <para>[字段:JSON.data.cards[n].desc.user_profile.info.face]</para>
        /// </summary>
        protected string FaceUrl { set; get; }
        /// <summary>
        /// <para>动态中Emoji的数据</para>
        /// <para>Key:EmojiText[JSON.data.cards[0].display.emoji_info.emoji_details[n].text]</para>
        /// <para>Value:EmojiUrl[JSON.data.cards[0].display.emoji_info.emoji_details[n].url]</para>
        /// </summary>
        public Dictionary<string,string> EmojiData { protected set; get; }
        /// <summary>
        /// <para>动态类型</para>
        /// <para>[字段:JSON.data.cards[0].desc.type]</para>
        /// </summary>
        protected CardType CardType { set; get; }
        /// <summary>
        /// <para>动态内容</para>
        /// <para>[字段:JSON.data.cards[n].card]</para>
        /// </summary>
        protected JObject Card { set; get; }
        /// <summary>
        /// <para>额外信息</para>
        /// <para>[字段:JSON.data.cards[n].extend_json]</para>
        /// </summary>
        protected JObject ExtendJson { set; get; }
        /// <summary>
        /// <para>第二页动态的offset</para>
        /// <para>[字段:JSON.data.next_page]</para>
        /// <para>此字段由框架自动添加</para>
        /// </summary>
        public string NextPageOffset{ set; get; }
        #endregion

        #region 公有方法
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                Uid      = this.Uid,
                UserName = this.UserName,
                FaceUrl  = this.FaceUrl
            };
        }

        /// <summary>
        /// 获取动态URL
        /// </summary>
        /// <returns>URL</returns>
        public string GetDynamicUrl()
        {
            return $"https://t.bilibili.com/{DynamicId}";
        }
        #endregion

        #region 类方法
        /// <summary>
        /// 初始化父数据
        /// </summary>
        protected Dynamic(JToken apiResponse, int index) : base(apiResponse)
        {
            if(base.Code != 0) return;
            JObject cardJObject;
            //处理选择的动态数据
            if (index == 0)
            {
                //去除置顶动态
                cardJObject = (int)apiResponse["data"]?["cards"]?[0]?["extra"]?["is_space_top"] == 0
                    ? JObject.Parse(apiResponse["data"]?["cards"]?[0]?.ToString() ?? "{}")
                    : JObject.Parse(apiResponse["data"]?["cards"]?[1]?.ToString() ?? "{}");
            }
            else
            {
                cardJObject = JObject.Parse(apiResponse["data"]?["cards"]?[index]?.ToString() ?? "{}");
            }
            //避免空json
            if(cardJObject.Count == 0) return; 

            Dictionary<string, string> emojiData = new Dictionary<string, string>();
            //判断是否存在emoji
            JToken[] emojis = cardJObject["display"]?["emoji_info"]?["emoji_details"]?.ToArray();
            if (emojis        != null &&
                emojis.Length != 0)
            {
                int unknownCount = 0;
                //写入emoji信息
                foreach (JToken emoji in emojis)
                {
                    emojiData.Add(emoji["text"]?.ToString() ?? $"unknown{unknownCount++}", emoji["url"]?.ToString());
                }
            }
            //写入数据
            this.Uid             = (long)(cardJObject["desc"]?["uid"] ?? 0);
            this.DynamicId       = cardJObject["desc"]?["dynamic_id_str"]?.ToString();
            this.UpdateTimeStemp = (long)(cardJObject["desc"]?["timestamp"] ?? 0);
            this.UserName        = cardJObject["desc"]?["user_profile"]?["info"]?["uname"]?.ToString();
            this.FaceUrl         = cardJObject["desc"]?["user_profile"]?["info"]?["face"]?.ToString();
            this.EmojiData       = emojiData;
            this.CardType        = (CardType)(int)(cardJObject["desc"]?["type"]         ?? 0);
            this.Card            = JObject.Parse(cardJObject["card"]?.ToString()        ?? "{}");
            this.ExtendJson      = JObject.Parse(cardJObject["extend_json"]?.ToString() ?? "{}");
            this.NextPageOffset  = cardJObject["next_offset"]?.ToString();
        }
        #endregion

        #region 工具方法
        /// <summary>
        /// 获取动态类型
        /// </summary>
        internal static CardType GetCardType(JObject sourceData, int index)
        {
            int realIndex;
            if (index == 0)
            {
                realIndex = (int) sourceData["data"]?["cards"]?[0]?["extra"]?["is_space_top"] == 0 ? 0 : 1;
            }
            else
            {
                realIndex = index;
            }
            return Enum.IsDefined(typeof(CardType), (int) (sourceData["data"]?["cards"]?[realIndex]?["desc"]?["type"] ?? 0))
                ? (CardType) (int) sourceData["data"]?["cards"]?[realIndex]?["desc"]?["type"]
                : CardType.Unknown;
        }
        #endregion
    }
}
