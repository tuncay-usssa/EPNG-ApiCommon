using System;
using System.Collections.Generic;
using System.Text;

namespace EPNG_ApiCommon.Messages
{
    public class Filter
    {
        public Filter()
        {
            PageIndex = 1;
            PageSize = 50;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string FlexiSearch { get; set; }
    }
}
