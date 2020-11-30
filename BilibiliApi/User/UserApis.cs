using System.Collections.Generic;
using System.Net;
using BilibiliApi.User.Models;
using PyLibSharp.Requests;

namespace BilibiliApi.User
{
    /// <summary>
    /// 用户类API
    /// </summary>
    public class UserApis
    {
        /// <summary>
        /// 获取直播间信息链接
        /// </summary>
        /// <param name="userId">用户ID</param>
        public static UserSpaceInfo GetLiveRoomInfo(long userId)
        {
            ReqResponse response = Requests.Get("https://api.bilibili.com/x/space/acc/info",new ReqParams
            {
                Params = new Dictionary<string, string>
                {
                    {"mid", userId.ToString()}
                }
            });
            return response.StatusCode != HttpStatusCode.OK
                ? new UserSpaceInfo($"net workerror code[{(int) response.StatusCode}]")
                : new UserSpaceInfo(response.Json());
        }
    }
}
