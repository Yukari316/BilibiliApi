using System;
using System.Net;
using BilibiliApi.Dynamic.Enums;
using BilibiliApi.Dynamic.Models.Card;
using PyLibSharp.Requests;

namespace BilibiliApi.Dynamic
{
    /// <summary>
    /// 动态类的API
    /// </summary>
    public static class DynamicAPIs
    {
        /// <summary>
        /// 从服务器获取最新的动态数据
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="index">想要获取动态的位置(最大值11),默认为0(最新动态)</param>
        /// <param name="pageOffset">动态页offset</param>
        /// <returns>
        /// 动态数据和动态类型
        /// </returns>
        public static (object cardObj, CardType cardType) GetLatestDynamic(
            long uid, int index = 0, string pageOffset = "0")
        {
            if (string.IsNullOrEmpty(pageOffset)) throw new NullReferenceException("pageOffset is null");
            if (index > 11) throw new ArgumentOutOfRangeException(nameof(index));
            //响应JSON
            try
            {
                var response = Requests.Get("https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history",
                                            new ReqParams
                                            {
                                                Params =
                                                {
                                                    {"host_uid", uid.ToString()},
                                                    {"offset_dynamic_id", pageOffset}
                                                }
                                            });

                if (response.StatusCode != HttpStatusCode.OK) return (null, CardType.Error);

                var responseData = response.Json();
                var cardType     = Models.Dynamic.GetCardType(responseData, index);

                return cardType switch
                {
                    CardType.PlainText => (new PlainTextCard(responseData, index), cardType),
                    CardType.TextAndPic => (new TextAndPicCard(responseData, index), cardType),
                    CardType.Forward => (new ForwardCard(responseData, index), cardType),
                    CardType.Video => (new VideoCard(responseData, index), cardType),
                    _ => (responseData, CardType.Unknown)
                };
            }
            //出现错误时将重构json信息
            catch (Exception e)
            {
                return (e, CardType.Error);
            }
        }
        
        /// <summary>
        /// 通过动态ID获取指定动态的内容
        /// </summary>
        /// <param name="dynamicId">动态ID</param>
        public static (object cardObj, CardType cardType) GetDynamic(ulong dynamicId)
        {
            try
            {
                var response = Requests.Get("https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail",
                                            new ReqParams
                                            {
                                                Params =
                                                {
                                                    {"dynamic_id", dynamicId.ToString()}
                                                }
                                            });
                if (response.StatusCode != HttpStatusCode.OK) return (null, CardType.Error);

                var responseData = response.Json();
                var cardType     = Models.Dynamic.GetCardType(responseData);
                
                return cardType switch
                {
                    CardType.PlainText => (new PlainTextCard(responseData), cardType),
                    CardType.TextAndPic => (new TextAndPicCard(responseData), cardType),
                    CardType.Forward => (new ForwardCard(responseData), cardType),
                    CardType.Video => (new VideoCard(responseData), cardType),
                    _ => (responseData, CardType.Unknown)
                };
            }
            catch (Exception e)
            {
                return (e, CardType.Error);
            }
        }
    }
}