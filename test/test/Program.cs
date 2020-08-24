using System;
using BilibiliApi;
using BilibiliApi.Dynamic;
using BilibiliApi.Dynamic.CardEnum;
using BilibiliApi.Dynamic.DynamicData;
using BilibiliApi.Dynamic.DynamicData.Card;
using Newtonsoft.Json.Linq;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //获取指定用户的最新动态
            JObject cardData = NetUtils.GetBiliDynamicJson(386900246, out CardType cardType);
            Dynamic card     = new Dynamic();
            switch (cardType)
            {
                case CardType.PlainText:
                    PlainTextCard plainTextCard = new PlainTextCard(cardData);
                    //将动态转为文本[]中的为[图片CQ码(由Mirai扩展)/图片的链接]
                    plainTextCard.ContentType = ContentType.CQCode; //设置内容的图片格式（默认为URL格式不需要修改）
                    Console.WriteLine(plainTextCard.ToString());
                    //转换为父类
                    card = plainTextCard;
                    break;
                case CardType.TextAndPic:
                    TextAndPicCard textAndPicCard = new TextAndPicCard(cardData);
                    textAndPicCard.ContentType = ContentType.CQCode;
                    Console.WriteLine(textAndPicCard.ToString());
                    card = textAndPicCard;
                    break;
                case CardType.Forward:
                    ForwardCard forwardCard = new ForwardCard(cardData);
                    forwardCard.ContentType = ContentType.CQCode;
                    Console.WriteLine(forwardCard.ToString());
                    Console.WriteLine($"\nOrginUrl:{forwardCard.GetOrginUrl(out CardType orginType)}");
                    Console.WriteLine($"OrginType:{orginType}");
                    Console.WriteLine($"OrginJson:{forwardCard.GetOrginJson(out orginType)}");
                    card = forwardCard;
                    break;
                case CardType.Video:
                    VideoCard videoCard = new VideoCard(cardData);
                    videoCard.ContentType = ContentType.CQCode;
                    videoCard.FullInfo = true;
                    Console.WriteLine(videoCard.ToString());
                    card = videoCard;
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
            //源数据
            Console.WriteLine($"\n\nsource data:{cardData}");
            Console.ReadLine();
        }   
    }
}