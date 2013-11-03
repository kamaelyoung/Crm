using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    public interface IRenwuFuwu
    {
        List<RenwuXinxi> GetChulizhongdeRenwu(string chulirenZhanghao, string mobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size, out int count);

        List<RenwuXinxi> GetZhipaideRenwu(string zhipairen, int start, int size, out int count);

        void SetJianglaiZhipai(string zhipairenZhanghao, string dailirenZhanghao, DateTime? kaishiShijian, DateTime? jieshuShijian);

        JianglaiZhipaiXinxi GetJianglaiZhipai(string zhipairenZhanghao);

        void XiugaiRenwuChuliren(string renwuId, string chulirenZhanghao);

        void ZhipaiRenwu(string[] renwuId, string dailirenZhanghao);

        void ZhipaiSuoyouRenwu(string zhipairenZhanghao, string dailirenZhanghao);

        void QuxiaoZhipai(string[] renwuId);

        List<RenwuXinxi> GetWanchengdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count);

        List<RenwuXinxi> GetGuidangdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count);

        XingdongXinxi GetXingdong(string xingdongId);

        RenwuXinxi GetRenwu(string liuchengId, string renwuId);

        List<RenwuXinxi> GetLiuchengRenwu(string liuchengId);

        RenwuXinxi WanchengRenwu(string liuchengId, string chulirenAccount, string renwuId, string shuoming);

        XingdongXinxi ChuangjianXingdong(string liuchengId, string code, string name, List<string> chulirenAccounts, string zhaiyao, DateTime? qiwangWanchengShijian);
    }
}
