using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class Liuchengtu
    {
        int stepHeight = 45;
        int stepWdith = 30;
        int fontKuangWidth = 160;
        int fontKuangHeight = 20;
        int hengxianChangdu = 120;
        int jiantouHeight = 10;
        int jiantouLineWidth = 20;
        int shuxianChangdu = 100;
        Point dangqianWeizhi;
        Bitmap grphImage = null;
        Pen pen = new Pen(Color.Blue, 1);
        SolidBrush jiangtouSolidBrush = new SolidBrush(Color.Blue);
        Font font = new Font("宋体", 12, FontStyle.Regular, GraphicsUnit.Point);
        Brush blackBrush = new SolidBrush(Color.Black);
        Brush greenBrush = new SolidBrush(Color.Green);
        Brush limeGreenBrush = new SolidBrush(Color.Lime);
        Graphics grph = null;

        Stack<Point> _huabiGuiji;

        public Liuchengtu(int width, int height)
        {
            grphImage = new Bitmap(width, height);
            dangqianWeizhi = new Point(30, (height - (stepHeight + fontKuangHeight*3))/2);
            grph = Graphics.FromImage(grphImage);
            _huabiGuiji = new Stack<Point>();
        }

        public void HuaJiedian(Xingdong renwu, Image image)
        {
            this.HuaJiedian(renwu.Mingcheng, 
                renwu.RenwuList.Where(x => x.Zhuangtai == RenwuZhuangtai.Chulizhong).Select(x => x.Chuliren ).ToList(),
                renwu.RenwuList.Where(x => x.Zhuangtai == RenwuZhuangtai.Wanchengle).Select(x => x.ShijiChuliren).ToList(),
                image);
        }

        public void HuaJiedian(string mingcheng, Image image)
        {
            this.HuaJiedian("结束", null, null, image);
        }

        public void HuaJiedian(string mingcheng, List<User> chulizhongYonghuList, List<User> wanchengdeYonghuList, Image image)
        {
            Point point = new Point(dangqianWeizhi.X + 10, dangqianWeizhi.Y);
            grph.DrawImage(image, point);

            point.X = point.X - fontKuangWidth / 2 + stepWdith / 2;
            point.Y = point.Y + stepHeight;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            grph.DrawString(mingcheng, font, blackBrush, new Rectangle(point.X, point.Y, fontKuangWidth, fontKuangHeight), stringFormat);

            if (chulizhongYonghuList != null)
            {
                string gaoliangXingming = "";
                chulizhongYonghuList.ForEach(x => gaoliangXingming += x.Name + ",");
                gaoliangXingming = gaoliangXingming.TrimEnd(',');
                if (!string.IsNullOrEmpty(gaoliangXingming))
                {
                    point.Y += fontKuangHeight;
                    grph.DrawString(gaoliangXingming, font, limeGreenBrush, new Rectangle(point.X, point.Y, fontKuangWidth, fontKuangHeight), stringFormat);
                }
            }
            if (wanchengdeYonghuList != null)
            {
                string xingming = "";
                wanchengdeYonghuList.ForEach(x => xingming += x.Name + ",");
                xingming = xingming.TrimEnd(',');
                if (!string.IsNullOrEmpty(xingming))
                {
                    point.Y += fontKuangHeight;
                    grph.DrawString(xingming, font, greenBrush, new Rectangle(point.X, point.Y, fontKuangWidth, fontKuangHeight), stringFormat);
                }
            }
        }

        public void HuaGuaijiaoJiantou(int yPianyiliang)
        {
            this.XiayiBangeBuzhou();
            this.Huahengxian();
            this.Huashuxian(yPianyiliang);
            this.Huajiantouxian();
            this.ShangyiBangeBuzhou();
        }

        public void HuaJiantou()
        {
            this.XiayiBangeBuzhou();
            this.Huahengxian();
            this.Huajiantouxian();
            this.ShangyiBangeBuzhou();
        }

        protected void ShangyiBangeBuzhou()
        {
            dangqianWeizhi.Y -= stepHeight / 2;
        }

        protected void XiayiBangeBuzhou()
        {
            dangqianWeizhi.Y += stepHeight / 2;
        }

        protected void Huashuxian(int yPianyiliang)
        {
            Point endPoint = new Point(dangqianWeizhi.X, dangqianWeizhi.Y + shuxianChangdu * yPianyiliang);
            grph.DrawLine(pen, dangqianWeizhi, endPoint);
            this.Yidong(endPoint, false);
        }

        protected void Huahengxian()
        {
            Point point1 = new Point(dangqianWeizhi.X + stepWdith + 10, dangqianWeizhi.Y);
            Point point2 = new Point(point1.X + hengxianChangdu, point1.Y);
            grph.DrawLine(pen, point1, point2);
            this.Yidong(point1, false);
            this.Yidong(point2, false);
        }

        protected void Huajiantouxian()
        {
            Point point1 = new Point(dangqianWeizhi.X, dangqianWeizhi.Y);
            Point point2 = new Point(point1.X + jiantouLineWidth, point1.Y);
            grph.DrawLine(pen, point1, point2);
            this.Yidong(point2, false);
            Point[] jiantouPoint = new Point[3];
            jiantouPoint[0] = new Point(point2.X, point2.Y - jiantouHeight / 2);
            jiantouPoint[1] = new Point(point2.X, point2.Y + jiantouHeight / 2);
            jiantouPoint[2] = new Point(point2.X + jiantouHeight, point2.Y);
            grph.FillPolygon(jiangtouSolidBrush, jiantouPoint);
        }

        public void Save(Stream stream, ImageFormat imageFormat)
        {
            grphImage.Save(stream, ImageFormat.Png);
        }

        protected void Yidong(Point point, bool jiluWeizhi)
        {
            dangqianWeizhi.X = point.X;
            dangqianWeizhi.Y = point.Y;
            if (jiluWeizhi)
            {
                this.Biaoji();
            }
        }

        public void Biaoji()
        {
            Point point = new Point(dangqianWeizhi.X, dangqianWeizhi.Y);
            this._huabiGuiji.Push(point);
        }

        public void Huigun()
        {
            if (this._huabiGuiji.Count > 0)
            {
                Point point = this._huabiGuiji.Pop();
                dangqianWeizhi.X = point.X;
                dangqianWeizhi.Y = point.Y;
            }
        }

        public void Huigun(int cishu)
        {
            for (int i = 0; i < cishu; i++)
            {
                if (this._huabiGuiji.Count > 0)
                {
                    Point point = this._huabiGuiji.Pop();
                    dangqianWeizhi.X = point.X;
                    dangqianWeizhi.Y = point.Y;
                }
            }
        }
    }
}