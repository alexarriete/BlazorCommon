using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid.GridClasses
{
    public class QueryResultBase
    {
        public int Total { get; set; }
        public List<object> List { get; set; }
    }
}
