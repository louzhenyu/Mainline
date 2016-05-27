using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Utility
{
    public class PageItem
    {
        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int totalCount;

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
    }
}
