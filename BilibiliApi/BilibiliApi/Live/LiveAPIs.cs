using System;
using System.Text;
using BilibiliApi.Live.Models;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Live
{
    public class LiveAPIs
    {
        /// <summary>
        /// 获取直播状态链接
        /// </summary>
        /// <param name="uid">UID</param>
        internal static string GetLiveStatusUrl(ulong uid)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("http://api.live.bilibili.com/bili/living_v2/");
            urlBuilder.Append(uid);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// 获取直播间信息链接
        /// </summary>
        /// <param name="uid">UID</param>
        internal static string GetLiveRoomInfoUrl(ulong uid)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("http://api.live.bilibili.com/room/v1/Room/getRoomInfoOld?mid=");
            urlBuilder.Append(uid);
            return urlBuilder.ToString();
        }

        /// <summary>
        /// 获取直播间的最新动态
        /// </summary>
        /// <param name="uid">UID</param>
        public static LiveStatus GetLiveStatus(ulong uid)
        {
            try
            {
                JObject dataJObject = JObject.Parse(Util.GetHttpResponse(GetLiveStatusUrl(uid)));
                return new LiveStatus(dataJObject);
            }
            catch (Exception e)
            {
                return new LiveStatus(e.Message);
            }
        }
    }
}
