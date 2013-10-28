using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class ShijianFanwei
    {
        public ShijianFanwei()
        {
        }

        public ShijianFanwei(DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            this.KaishiShijian = kaishiShijian;
            this.JieshuShijian = jieshuShijian;
        }

        public DateTime? KaishiShijian { set; get; }

        public DateTime? JieshuShijian { set; get; }

        public bool ZaiFanweinei(DateTime? date)
        {
            if (!this.KaishiShijian.HasValue && !this.JieshuShijian.HasValue)
            {
                return true;
            }

            if (!date.HasValue)
            {
                return false;
            }

            if (this.JieshuShijian.HasValue && this.KaishiShijian.HasValue)
            {
                if (date < this.KaishiShijian || date > this.JieshuShijian)
                {
                    return false;
                }
            }
            else if (this.JieshuShijian.HasValue)
            {
                if (date > this.JieshuShijian)
                {
                    return false;
                }
            }
            else if (this.KaishiShijian.HasValue)
            {
                if (date < this.KaishiShijian)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
