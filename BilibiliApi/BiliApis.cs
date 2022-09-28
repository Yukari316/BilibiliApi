using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BilibiliApi.Models;
using BilibiliApi.Video.Models;
using Newtonsoft.Json.Linq;
// ReSharper disable PossibleMultipleEnumeration

namespace BilibiliApi;

/// <summary>
/// 叔叔API
/// </summary>
public static class BiliApis
{
    /// <summary>
    /// <para>获取视频信息</para>
    /// <para>自动识别AV号和BV号</para>
    /// </summary>
    /// <param name="videoId">AV号/BV号</param>
    public static async ValueTask<VideoInfo> GetVideoInfo(string videoId)
    {
        var queryStr = videoId[..2].ToLower() switch
                       {
                           "av" => $"?aid={videoId[2..]}",
                           "bv" => $"?bvid={videoId}",
                           _    => throw new ArgumentOutOfRangeException(nameof(videoId), "unknown id type")
                       };

        try
        {
            HttpResponseMessage response =
                await Util.PubHttpClient
                          .GetAsync($"https://api.bilibili.com/x/web-interface/view{queryStr}");

            if (response.StatusCode != HttpStatusCode.OK)
                return new VideoInfo($"net error code[{(int)response.StatusCode}]");

            JToken responseData = JToken.Parse(await response.Content.ReadAsStringAsync());
            if (responseData["code"]?.ToString() != "0")
                return new VideoInfo($"api error code[{responseData["code"]}]");

            return new VideoInfo(JToken.Parse(await response.Content.ReadAsStringAsync()));
        }
        catch (Exception e)
        {
            return new VideoInfo($"net error message:{e}");
        }
    }

    /// <summary>
    /// 获取直播间信息链接
    /// </summary>
    /// <param name="roomId">房间id(直播间真实ID)</param>
    public static async ValueTask<LiveInfo> GetLiveRoomInfo(long roomId)
    {
        try
        {
            HttpResponseMessage response =
                await Util.PubHttpClient
                          .GetAsync($"https://api.live.bilibili.com/room/v1/Room/get_info?room_id={roomId}");

            if (response.StatusCode != HttpStatusCode.OK)
                return new LiveInfo($"net error code[{(int)response.StatusCode}]");

            JToken responseData = JToken.Parse(await response.Content.ReadAsStringAsync());
            if (responseData["code"]?.ToString() != "0") 
                return new LiveInfo($"api error code[{responseData["code"]}]");

            return new LiveInfo(JToken.Parse(await response.Content.ReadAsStringAsync()));
        }
        catch (Exception e)
        {
            return new LiveInfo($"net error message:{e}");
        }
    }

    /// <summary>
    /// 从服务器获取最新的动态数据
    /// </summary>
    /// <param name="uid">用户ID</param>
    /// <returns>
    /// 动态数据和动态类型
    /// </returns>
    public static async ValueTask<(ulong dId, long pubTs)> GetLatestDynamicId(long uid)
    {
        //响应JSON
        try
        {
            HttpResponseMessage response =
                await Util.PubHttpClient
                          .GetAsync($"https://api.bilibili.com/x/polymer/web-dynamic/v1/feed/space?host_mid={uid}");

            if (response.StatusCode != HttpStatusCode.OK) return (0, 0);

            JToken responseData = JToken.Parse(await response.Content.ReadAsStringAsync());
            if (responseData["code"]?.ToString() != "0") return (0, 0);

            JArray items = responseData["data"]?["items"] as JArray;
            if (items is null) return (0, 0);

            //移除置顶
            if (responseData["data"]?["items"]?[0]?["modules"]?["module_tag"]?["text"]?.ToString() == "置顶")
            {
                items.RemoveAt(0);
            }
            //移除直播动态
            IEnumerable<JToken> exp = items.Where(Util.IsLiveDynamic);
            if (exp.Any())
            {
                var rmItems = exp.ToList();
                foreach (JToken item in rmItems)
                    items.Remove(item);
            }

            return (Convert.ToUInt64(responseData["data"]?["items"]?[0]?["id_str"] ?? 0),
                Convert.ToInt64(responseData["data"]?["items"]?[0]?["modules"]?["module_author"]?["pub_ts"] ?? 0));
        }
        //出现错误时将重构json信息
        catch
        {
            return (0, 0);
        }
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static async ValueTask<UserInfo> GetLiveUserInfo(long uid)
    {
        try
        {
            HttpResponseMessage response =
                await Util.PubHttpClient
                          .GetAsync($"https://api.live.bilibili.com/live_user/v1/Master/info?uid={uid}");

            if (response.StatusCode != HttpStatusCode.OK) return null;

            JToken responseData = JToken.Parse(await response.Content.ReadAsStringAsync());

            if (responseData["code"]?.ToString() != "0") return null;

            return new UserInfo
            {
                Uid      = uid,
                UserName = responseData["data"]?["info"]?["uname"]?.ToString() ?? string.Empty,
                FaceUrl  = responseData["data"]?["info"]?["face"]?.ToString() ?? string.Empty,
                LiveId   = Convert.ToInt64(responseData["data"]?["room_id"] ?? 0)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}