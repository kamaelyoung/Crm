using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;
using Coldew.Core.Workflow;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.NnitTest
{
    public class LiuchengCeshi:UnitTestBase
    {
        [Test]
        public void Test()
        {
            DemoLiuchengYinqing yinqing = new DemoLiuchengYinqing(this.ColdewManager);
            DemoLiuchengMoban liuchengMoban = yinqing.DemoLiuchengMoban;

            User ceshiYonghu1 = yinqing.GetYonghu("user1");
            User ceshiYonghu2 = yinqing.GetYonghu("user2");
            User ceshiYonghu3 = yinqing.GetYonghu("user3");
            Liucheng liucheng = liuchengMoban.Faqi(ceshiYonghu1, "测试内容", true);

            liucheng.XingdongList[1].RenwuList[0].Chuli(ceshiYonghu1, RenwuChuliJieguo.Tongguo, "");

            byte[] bytes = liucheng.GetLiuchengtu();
            FileStream stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "liuchengtu.png"));
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        [Test]
        public void TuihuiTest()
        {
            
            DemoLiuchengYinqing yinqing = new DemoLiuchengYinqing(this.ColdewManager);
            DemoLiuchengMoban liuchengMoban = yinqing.DemoLiuchengMoban;

            User ceshiYonghu1 = yinqing.GetYonghu("user1");
            User ceshiYonghu2 = yinqing.GetYonghu("user2");
            User ceshiYonghu3 = yinqing.GetYonghu("user3");
            Liucheng liucheng = liuchengMoban.Faqi(ceshiYonghu1, "测试内容", true);

            liuchengMoban.CeshiRenwuChuli(liucheng.XingdongList[1].RenwuList[0], ceshiYonghu1, RenwuChuliJieguo.Tongguo, "", "");
            liuchengMoban.CeshiRenwuChuli(liucheng.XingdongList[1].RenwuList[1], ceshiYonghu2, RenwuChuliJieguo.Tuihui, "", "");
            liuchengMoban.CeshiRenwuChuli(liucheng.XingdongList[1].RenwuList[2], ceshiYonghu3, RenwuChuliJieguo.Tuihui, "", "");

            byte[] bytes = liucheng.GetLiuchengtu();
            FileStream stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "liuchengtu.png"));
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        [Test]
        public void ZhipaiTest()
        {
            
            DemoLiuchengYinqing yinqing = new DemoLiuchengYinqing(this.ColdewManager);
            DemoLiuchengMoban liuchengMoban = yinqing.DemoLiuchengMoban;

            User ceshiYonghu1 = yinqing.GetYonghu("user1");
            User ceshiYonghu4 = yinqing.GetYonghu("user4");
            Liucheng liucheng = liuchengMoban.Faqi(ceshiYonghu1, "测试内容", false);
            Renwu xingdong = liucheng.XingdongList[1].RenwuList[0];
            xingdong.Zhipai(ceshiYonghu4);
            Assert.AreEqual(xingdong.Chuliren, ceshiYonghu4);

            xingdong.QuxiaoZhipai();
            Assert.AreEqual(xingdong.Chuliren, ceshiYonghu1);
            byte[] bytes = liucheng.GetLiuchengtu();
            FileStream stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "liuchengtu.png"));
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        [Test]
        public void JianglaiZhipaiTest()
        {
            
            DemoLiuchengYinqing yinqing = new DemoLiuchengYinqing(this.ColdewManager);
            DemoLiuchengMoban liuchengMoban = yinqing.DemoLiuchengMoban;

            User ceshiYonghu1 = yinqing.GetYonghu("user1");
            User ceshiYonghu4 = yinqing.GetYonghu("user4");
            yinqing.JianglaiZhipaiManager.SetJianglaiRenwuZhipai(ceshiYonghu1, ceshiYonghu4, null, DateTime.Now.AddDays(1));
            Liucheng liucheng = liuchengMoban.Faqi(ceshiYonghu1, "测试内容", false);
            Renwu xingdong = liucheng.XingdongList[1].RenwuList[0];
            Assert.AreEqual(xingdong.Chuliren, ceshiYonghu4);

            xingdong.QuxiaoZhipai();
            Assert.AreEqual(xingdong.Chuliren, ceshiYonghu1);
            byte[] bytes = liucheng.GetLiuchengtu();
            FileStream stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "liuchengtu.png"));
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        [Test]
        public void Faqi20geLiucheng()
        {
            
            DemoLiuchengYinqing yinqing = new DemoLiuchengYinqing(this.ColdewManager);
            DemoLiuchengMoban liuchengMoban = yinqing.GetLiuchengMoban(1) as DemoLiuchengMoban;

            User ceshiYonghu1 = yinqing.GetYonghu("user1");
            User ceshiYonghu2 = yinqing.GetYonghu("user2");
            User ceshiYonghu3 = yinqing.GetYonghu("user3");
            Liucheng liucheng = liuchengMoban.Faqi(ceshiYonghu1, "测试内容", false);
            for (int i = 0; i < 20; i++)
            {
                liuchengMoban.Faqi(ceshiYonghu1, "测试内容", false);
            }
        }
    }
}
