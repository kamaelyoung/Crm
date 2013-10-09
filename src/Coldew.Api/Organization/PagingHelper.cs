using System;
using System.Collections.Generic;

using System.Text;

namespace Coldew.Api.Organization
{
    /// <summary>
    /// 分页帮助类
    /// </summary>
    public sealed class PagingHelper
    {
        public PagingHelper()
        {
        }

        public PagingHelper(int pageIndex, int pageSize, string sortField, string dir)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.SortField = sortField;
            this.Dir = dir;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示多少条记录
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 总共有多少条记录
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling((double)this.TotalCount / (double)this.PageSize);
            }
        }
    }
}
