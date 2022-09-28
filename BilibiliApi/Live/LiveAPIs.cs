using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BilibiliApi.Live.Models;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Live
{
    /// <summary>
    /// 直播间相关API
    /// </summary>
    public class LiveAPIs
    {
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

                return response.StatusCode != HttpStatusCode.OK
                    ? new LiveInfo($"net error code[{(int)response.StatusCode}]")
                    : new LiveInfo(JToken.Parse(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                return new LiveInfo($"net error message:{e}");
            }
        }
    }
}