using System;
using System.Collections.Generic;
using System.Text;

namespace EPNG_ApiCommon.Messages
{
    public class FilterItem<TType>
    {
        public FilterItem()
        {
            PossibleValues = new List<TType>();
            SortDescending = false;
            OperatorType = OperatorType.AND;
        }
        public List<TType> PossibleValues { get; set; }
        public OperatorType OperatorType { get; set; }
        public int SortPosition { get; set; }
        public bool SortDescending { get; set; }
    }

    public enum OperatorType
    {
        AND = 0,
        OR = 1,
        CONTAINS = 2,
    }
}
