using System;
using System.Collections.Generic;
using System.Net;
using BilibiliApi.Video.Models;
using PyLibSharp.Requests;

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
        public static VideoInfo GetVideoInfo(string videoId)
        {
            ReqResponse response;
            var paraName = videoId[..2].ToLower() switch
            {
                "av" => "aid",
                "bv" => "bvid",
                _ => throw new ArgumentOutOfRangeException(nameof(videoId), "unknown id type")
            };

            try
            {
                response = Requests.Get("http://api.bilibili.com/x/web-interface/view", new ReqParams
                {
                    Params = new Dictionary<string, string>
                    {
                        {paraName, videoId}
                    }
                });
            }
            catch (Exception e)
            {
                return new VideoInfo($"net error message:{e}");
            }

            return response.StatusCode != HttpStatusCode.OK
                ? new VideoInfo($"net error code[{(int) response.StatusCode}]")
                : new VideoInfo(response.Json());
        }
    }
}