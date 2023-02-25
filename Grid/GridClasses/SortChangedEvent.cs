using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public enum SortDirection
    {
        None = 0,
        Asc = 1,
        Desc = 2
    }
    public class SortChangedEvent
    {
        public string SortId { get; set; }
        public SortDirection Direction { get; set; }
        public PropertyInfo Prop { get; set; }
    }
}
