using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BilibiliApi.Video.Models;
using Newtonsoft.Json.Linq;

namespace BilibiliApi.Video
{
    /// <summary>
    /// 视频相关API
    /// </summary>
    public static class VideoApis
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

                return response.StatusCode != HttpStatusCode.OK
                    ? new VideoInfo($"net error code[{(int)response.StatusCode}]")
                    : new VideoInfo(JToken.Parse(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception e)
            {
                return new VideoInfo($"net error message:{e}");
            }
        }
    }
}