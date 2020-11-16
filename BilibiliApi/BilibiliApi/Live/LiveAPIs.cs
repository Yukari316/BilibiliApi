using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using BilibiliApi.Live.Models;
using PyLibSharp.Requests;

namespace BilibiliApi.Live
{
    /// <summary>
    /// 直播间相关API
    /// </summary>
    public class LiveAPIs
    {
        /// <summary>
        /// 获取直播状态链接
        /// </summary>
        /// <param name="uid">UID</param>
        internal static string GetLiveStatusUrl(long uid)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("http://api.live.bilibili.com/bili/living_v2/");
            urlBuilder.Append(uid);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// 获取直播间信息链接
        /// </summary>
        /// <param name="roomId">房间id(直播间真实ID)</param>
        public static LiveInfo GetLiveRoomInfo(long roomId)
        {
            ReqResponse response = Requests.Get("https://api.live.bilibili.com/xlive/web-room/v1/index/getRoomPlayInfo",new ReqParams
            {
                Params = new Dictionary<string, string>
                {
                    {"room_id", roomId.ToString()}
                }
            });
            return response.StatusCode != HttpStatusCode.OK
                ? new LiveInfo($"net workerror code[{(int) response.StatusCode}]")
                : new LiveInfo(response.Json());
        }

        /// <summary>
        /// 获取直播间的最新状态
        /// </summary>
        /// <param name="uid">UID</param>
        public static LiveStatus GetLiveStatus(long uid)
        {
            try
            {
                ReqResponse response = Requests.Get(GetLiveStatusUrl(uid));
                return response.StatusCode != HttpStatusCode.OK
                    ? new LiveStatus($"Net error code[{(int) response.StatusCode}]")
                    : new LiveStatus(response.Json());
            }
            catch (Exception e)
            {
                return new LiveStatus(e.Message);
            }
        }
    }
}
