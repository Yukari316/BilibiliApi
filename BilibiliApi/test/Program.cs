using System;
using System.Collections.Generic;
using BilibiliApi.Dynamic;
using BilibiliApi.Dynamic.Enums;
using BilibiliApi.Dynamic.Models;
using BilibiliApi.Dynamic.Models.Card;
using BilibiliApi.Live;
using BilibiliApi.Live.Models;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region LiveAPI
            // LiveStatus liveStatus = LiveAPIs.GetLiveStatus(48039);
            // Console.WriteLine($"live status:{liveStatus.Status}");
            // Console.WriteLine($"live room id:{liveStatus.UrlId}");
            // Console.ReadLine();
            LiveInfo liveInfo = LiveAPIs.GetLiveRoomInfo(21448649);
            Console.WriteLine($"API Return Code = {liveInfo.Code}");
            Console.WriteLine($"Liver uid = {liveInfo.UserId}");
            Console.WriteLine($"Live room status = {liveInfo.LiveStatus}");
            Console.ReadLine();
            #endregion

            #region 动态API
            //获取指定用户的最新动态
            var cardData = DynamicAPIs.GetLatestDynamic(48039);
            switch (cardData.cardType)
            {
                case CardType.PlainText:
                    if (!(cardData.cardObj is PlainTextCard plainTextCard)) return;
                    //将动态转为文本
                    Console.WriteLine(plainTextCard.ToString());
                    //动态链接
                    Console.WriteLine($"DynamicUrl:{plainTextCard.GetDynamicUrl()}");
                    //下一页offset
                    Console.WriteLine($"NextOffset:{plainTextCard.NextPageOffset}");
                    //获取发送者的信息
                    UserInfo sender1 = plainTextCard.GetUserInfo();
                    //UID
                    Console.WriteLine($"SenderID:{sender1.Uid}");
                    //用户名
                    Console.WriteLine($"SenderName:{sender1.UserName}");
                    //用户头像图片链接
                    Console.WriteLine($"SenderFace:{sender1.FaceUrl}");
                    //Emoji
                    foreach (KeyValuePair<string, string> keyValuePair in plainTextCard.EmojiData)
                    {
                        Console.WriteLine($"Emoji:name={keyValuePair.Key} url={keyValuePair.Value}");
                    }
                    break;
                case CardType.TextAndPic:
                    if (!(cardData.cardObj is TextAndPicCard textAndPicCard)) return;
                    Console.WriteLine(textAndPicCard.ToString());
                    //动态链接
                    Console.WriteLine($"DynamicUrl:{textAndPicCard.GetDynamicUrl()}");
                    //下一页offset
                    Console.WriteLine($"NextOffset:{textAndPicCard.NextPageOffset}");
                    //获取发送者的信息
                    UserInfo sender2 = textAndPicCard.GetUserInfo();
                    //UID
                    Console.WriteLine($"SenderID:{sender2.Uid}");
                    //用户名
                    Console.WriteLine($"SenderName:{sender2.UserName}");
                    //用户头像图片链接
                    Console.WriteLine($"SenderFace:{sender2.FaceUrl}");
                    //Emoji
                    foreach (KeyValuePair<string, string> keyValuePair in textAndPicCard.EmojiData)
                    {
                        Console.WriteLine($"Emoji:name={keyValuePair.Key} url={keyValuePair.Value}");
                    }
                    break;
                case CardType.Forward:
                    if (!(cardData.cardObj is ForwardCard forwardCard)) return;
                    Console.WriteLine(forwardCard.ToString());
                    Console.WriteLine($"\nOrginUrl:{forwardCard.GetOrginUrl(out CardType orginType)}");
                    Console.WriteLine($"OrginType:{orginType}");
                    Console.WriteLine($"OrginJson:{forwardCard.GetOrginJson(out _)}");
                    //动态链接
                    Console.WriteLine($"DynamicUrl:{forwardCard.GetDynamicUrl()}");
                    //下一页offset
                    Console.WriteLine($"NextOffset:{forwardCard.NextPageOffset}");
                    //获取发送者的信息
                    UserInfo sender3 = forwardCard.GetUserInfo();
                    //UID
                    Console.WriteLine($"SenderID:{sender3.Uid}");
                    //用户名
                    Console.WriteLine($"SenderName:{sender3.UserName}");
                    //用户头像图片链接
                    Console.WriteLine($"SenderFace:{sender3.FaceUrl}");
                    //Emoji
                    foreach (KeyValuePair<string, string> keyValuePair in forwardCard.EmojiData)
                    {
                        Console.WriteLine($"Emoji:name={keyValuePair.Key} url={keyValuePair.Value}");
                    }
                    break;
                case CardType.Video:
                    if (!(cardData.cardObj is VideoCard videoCard)) return;
                    Console.WriteLine(videoCard.ToString());
                    //封面
                    Console.WriteLine($"Cover Link:{videoCard.CoverUrl}");
                    //动态链接
                    Console.WriteLine($"DynamicUrl:{videoCard.GetDynamicUrl()}");
                    //下一页offset
                    Console.WriteLine($"NextOffset:{videoCard.NextPageOffset}");
                    //获取发送者的信息
                    UserInfo sender4 = videoCard.GetUserInfo();
                    //UID
                    Console.WriteLine($"SenderID:{sender4.Uid}");
                    //用户名
                    Console.WriteLine($"SenderName:{sender4.UserName}");
                    //用户头像图片链接
                    Console.WriteLine($"SenderFace:{sender4.FaceUrl}");
                    //Emoji
                    foreach (KeyValuePair<string, string> keyValuePair in videoCard.EmojiData)
                    {
                        Console.WriteLine($"Emoji:name={keyValuePair.Key} url={keyValuePair.Value}");
                    }
                    break;
            }
            
            //源数据
            // Console.WriteLine($"\n\nsource data:{cardData}");
            Console.ReadLine();
            #endregion
        }   
    }
}
