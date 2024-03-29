using System;
using System.Threading.Tasks;
using BilibiliApi;
using BilibiliApi.Models;
using BilibiliApi.Video.Models;

namespace test;

internal class Program
{
    private static async Task Main(string[] args)
    {
#region LiveAPI

        (LiveInfo liveInfo, _) = await BiliApis.GetLiveRoomInfo(213);
        Console.WriteLine($"API Return Code = {liveInfo.Code}");
        Console.WriteLine($"Liver uid = {liveInfo.UserId}");
        Console.WriteLine($"Live room status = {liveInfo.LiveStatus}");
        Console.WriteLine($"Live room t = {liveInfo.Title}");
        Console.WriteLine($"Live room c = {liveInfo.Cover}");
        Console.WriteLine($"Live room s_time = {liveInfo.LiveStartTime}");

#endregion

#region 动态API

        //获取指定用户的最新动态ID
        (ulong did, _, _) = await BiliApis.GetLatestDynamicId(48039);
        Console.WriteLine($"did = {did}");

#endregion

#region VideoAPI

        (VideoInfo vInfo, _) = await BiliApis.GetVideoInfo("BV1xe4y1b7N3");
        Console.WriteLine(vInfo.Title);

#endregion

#region User

        (UserInfo uInfo, _) = await BiliApis.GetLiveUserInfo(5817596);
        Console.WriteLine(uInfo.LiveId);

#endregion
    }
}