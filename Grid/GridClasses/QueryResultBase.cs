using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid.GridClasses
{
    public class QueryResultBase
    {
        public int NotFilteredTotal { get; set; }
        public int Total { get; set; }
        public IEnumerable<RowBase> List { get; set; }
        public int PageSize { get; set; }        
        public int PageIndex { get; set; }
    }
}
