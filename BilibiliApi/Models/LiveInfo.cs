using System;
using System.Globalization;
using BilibiliApi.BaseModel;
using BilibiliApi.Live.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Models;

/// <summary>
/// 直播类型
/// </summary>
public class LiveInfo : BaseApiInfo
{
    #region 属性

    /// <summary>
    /// 直播间真实ID
    /// </summary>
    public long RoomId { get; }

    /// <summary>
    /// 直播间短ID
    /// </summary>
    public long ShortId { get; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// 直播间标题
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 封面
    /// </summary>
    public string Cover { get; }

    /// <summary>
    /// 直播状态
    /// </summary>
    public LiveStatusType LiveStatus { get; }


    /// <summary>
    /// 直播开始时间
    /// </summary>
    public DateTime LiveStartTime { get; }

    #endregion

    #region 构造函数

    /// <summary>
    /// 初始化
    /// </summary>
    internal LiveInfo(JToken apiResponse)
        : base(apiResponse)
    {
        if (apiResponse == null) throw new NullReferenceException("response data is null");

        try
        {
            //检查code值
            string code = apiResponse["code"]?.ToString();
            if (code == null || !code.Equals("0"))
            {
                LiveStatus = LiveStatusType.Unknown;
                return;
            }

            RoomId = Convert.ToInt64(apiResponse["data"]?["room_id"] ?? -1);
            ShortId = Convert.ToInt64(apiResponse["data"]?["short_id"] ?? -1);
            UserId = Convert.ToInt64(apiResponse["data"]?["uid"] ?? -1);
            Title = apiResponse["data"]?["title"]?.ToString() ?? string.Empty;
            Cover = apiResponse["data"]?["user_cover"]?.ToString() ?? string.Empty;
            Console.WriteLine(apiResponse["data"]?["live_time"]?.ToString());
            if (DateTime.TryParseExact(apiResponse["data"]?["live_time"]?.ToString() ?? "0001-01-01 01:01:01",
                                       "yyyy-MM-dd HH:mm:ss",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None,
                                       out DateTime time))
                LiveStartTime = time;

            LiveStatus = Enum.IsDefined(typeof(LiveStatusType),
                                        (int)(apiResponse["data"]?["live_status"] ?? (int)LiveStatusType.Error))
                ? (LiveStatusType)(int)apiResponse["data"]?["live_status"]
                : LiveStatusType.Unknown;
        }
        catch (Exception e)
        {
            throw new JsonException("json parse error", e);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    internal LiveInfo(string errorMessage)
        : base(-1, errorMessage, -1)
    {
    }

    #endregion
}