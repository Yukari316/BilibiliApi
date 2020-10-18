using System;
using System.Collections.Generic;
using BilibiliApi;
using BilibiliApi.Dynamic;
using BilibiliApi.Dynamic.Enums;
using BilibiliApi.Dynamic.Models;
using BilibiliApi.Dynamic.Models.Card;
using BilibiliApi.Live;
using BilibiliApi.Live.Models;
using Newtonsoft.Json.Linq;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region LiveAPI
            // LiveStatus liveStatus = LiveAPIs.GetLiveStatus(339567211);
            // Console.WriteLine($"live status:{liveStatus.Status}");
            // Console.WriteLine($"live room id:{liveStatus.RoomId}");
            // Console.ReadLine();
            #endregion

            #region 动态API
            //获取指定用户的最新动态
            JObject cardData = DynamicAPIs.GetBiliDynamicJson(16836724, out CardType cardType);
            Dynamic card     = new Dynamic();
            switch (cardType)
            {
                case CardType.PlainText:
                    PlainTextCard plainTextCard = new PlainTextCard(cardData);
                    //将动态转为文本[]中的为[图片CQ码(由Mirai扩展)/图片的链接]
                    Console.WriteLine(plainTextCard.ToString());
                    //转换为父类
                    card = plainTextCard;
                    break;
                case CardType.TextAndPic:
                    TextAndPicCard textAndPicCard = new TextAndPicCard(cardData);
                    Console.WriteLine(textAndPicCard.ToString());
                    card = textAndPicCard;
                    break;
                case CardType.Forward:
                    ForwardCard forwardCard = new ForwardCard(cardData);
                    Console.WriteLine(forwardCard.ToString());
                    Console.WriteLine($"\nOrginUrl:{forwardCard.GetOrginUrl(out CardType orginType)}");
                    Console.WriteLine($"OrginType:{orginType}");
                    Console.WriteLine($"OrginJson:{forwardCard.GetOrginJson(out orginType)}");
                    card = forwardCard;
                    break;
                case CardType.Video:
                    VideoCard videoCard = new VideoCard(cardData);
                    Console.WriteLine(videoCard.ToString());
                    card = videoCard;
                    //封面
                    Console.WriteLine($"Cover Link:{videoCard.CoverUrl}");
                    break;
            }
            //动态链接
            Console.WriteLine($"DynamicUrl:{card.GetDynamicUrl()}");
            //下一页offset
            Console.WriteLine($"NextOffset:{card.NextPageOffset}");
            //获取发送者的信息
            UserInfo sender = card.GetUserInfo();
            //UID
            Console.WriteLine($"SenderID:{sender.Uid}");
            //用户名
            Console.WriteLine($"SenderName:{sender.UserName}");
            //用户头像图片链接
            Console.WriteLine($"SenderFace:{sender.FaceUrl}");
            //Emoji
            foreach (KeyValuePair<string, string> keyValuePair in card.EmojiData)
            {
                Console.WriteLine($"Emoji:name={keyValuePair.Key} url={keyValuePair.Value}");
            }
            //源数据
            // Console.WriteLine($"\n\nsource data:{cardData}");
            Console.ReadLine();
            #endregion
        }   
    }
}