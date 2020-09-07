namespace BilibiliApi.Dynamic.Models
{
    public class UserInfo
    {
        #region 属性
        /// <summary>
        /// <para>发送者ID</para>
        /// <para>[字段:JSON.data.cards[n].desc.uid]</para>
        /// </summary>
        public long Uid { internal set; get; }
        /// <summary>
        /// <para>动态所属用户名称</para>
        /// <para>[字段:JSON.data.cards[n].desc.user_profile.info.uname]</para>
        /// </summary>
        public string UserName { internal set; get; }
        /// <summary>
        /// <para>用户头像的图片链接</para>
        /// <para>[字段:JSON.data.cards[n].desc.user_profile.info.face]</para>
        /// </summary>
        public string FaceUrl { internal set; get; }
        #endregion
    }
}
