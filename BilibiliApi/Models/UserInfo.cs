using System;
using BilibiliApi.BaseModel;
using BilibiliApi.Live.Enums;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Models;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo : BaseApiInfo
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

    /// <summary>
    /// 直播间ID
    /// </summary>
    public long LiveId { internal set; get; }

#endregion

    internal UserInfo(JToken apiResponse, long uid)
        : base(apiResponse)
    {
        try
        { 
            if (apiResponse == null) throw new NullReferenceException("response data is null");
            //检查code值
            string code = apiResponse["code"]?.ToString();
            if (code is not "0")
            {
                return;
            }

            Uid      = uid;
            UserName = apiResponse["data"]?["info"]?["uname"]?.ToString() ?? string.Empty;
            FaceUrl  = apiResponse["data"]?["info"]?["face"]?.ToString() ?? string.Empty;
            LiveId   = Convert.ToInt64(apiResponse["data"]?["room_id"] ?? 0);
        }
        catch (Exception e)
        {
            Code    = -1;
            Message = e.Message;
            TTL     = -1;
        }
    }

    internal UserInfo(string errorMessage)
        : base(-1, errorMessage, -1)
    {
    }
}