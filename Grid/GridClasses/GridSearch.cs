using DocumentFormat.OpenXml.Drawing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public enum PropertyType
    {
        noType,
        text,
        number,
        datetime,
        list,
        boolean,
        image,
        imageRag
    }

    public enum NumberSelectionType
    {
        Equal,
        Greaterthan,
        Lessthan,
        Between
    }
    public class GridSearch
    {        
        public string ColumnName { get; set; }
        public int Position { get; set; }
        public string SearchText { get; set; }
        public string SearchText2 { get; set; }
        public string SearchPropName { get; set; }
        public PropertyType SearchPropType { get; set; }
        public DateTime SearchDateFrom { get; set; }
        public DateTime SearchDateTo { get; set; }
        public PropertyInfo SearchProperty { get; set; }        
        public bool CaseSensitive { get; set; }
        public NumberSelectionType NumberSearchTypeSelected { get; set; }
        public bool Searched { get; set; }

        public GridSearch() { } 
        public GridSearch(GridColumnBase gridColumn)
        {
            ColumnName = gridColumn.Name;
            Position= gridColumn.Position;
            SearchProperty = gridColumn.PropertyInfo;
            SearchPropName = gridColumn.Name;
            SearchText = "";
            SearchPropType = gridColumn.PropertyType;
            SearchDateFrom = DateTime.Today.AddMonths(-1);
            SearchDateTo = DateTime.Today;
        }     

        public IEnumerable<RowBase> GetFilteredByDateTimeInterval(IEnumerable<RowBase> objects)
        {
            DateTime result = DateTime.MinValue;
            return objects.Where(n => (SearchProperty == null ? true
                     : SearchProperty.GetValue(n, null) == null)
                     || (!DateTime.TryParse(SearchProperty.GetValue(n, null).ToString(), out result) ? false
                     : Convert.ToDateTime(SearchProperty.GetValue(n, null)) > SearchDateFrom)
                        && (Convert.ToDateTime(SearchProperty.GetValue(n, null)) <= SearchDateTo));
        }

        public IEnumerable<RowBase> GetEqualsValues(IEnumerable<RowBase> objects)
        {
            return objects.Where(n => SearchProperty == null ? true
                        : SearchProperty.GetValue(n, null) == null ? false
                        : (SearchProperty.GetValue(n, null).ToString() == SearchText));
        }

        public IEnumerable<RowBase> GetGreaterThanValues(IEnumerable<RowBase> objects)
        {
            return objects.Where(n => SearchProperty == null ? true
                         : SearchProperty.GetValue(n, null) == null ? false
                         : (double.Parse(SearchProperty.GetValue(n, null).ToString()) > double.Parse(SearchText)));
        }

        public IEnumerable<RowBase> GetLessThanValues(IEnumerable<RowBase> objects)
        {
            return objects.Where(n => SearchProperty == null ? true : SearchProperty.GetValue(n, null) == null ? false 
            : (double.Parse(SearchProperty.GetValue(n, null).ToString()) < double.Parse(SearchText)));
        }

        public IEnumerable<RowBase> GetBetweenValues(IEnumerable<RowBase> objects)
        {
            return objects.Where(n => SearchProperty == null ? true : SearchProperty.GetValue(n, null) == null ? false 
            : ((double.Parse(SearchProperty.GetValue(n, null).ToString()) > double.Parse(SearchText)) && double.Parse(SearchProperty.GetValue(n, null).ToString()) < double.Parse(SearchText2)));
        }

        public IEnumerable<RowBase> GetTextContains(IEnumerable<RowBase> objects)
        {
            SearchText = SearchText.RemoveDiacritics(!CaseSensitive);
            return  objects.Where(n => SearchProperty == null ? true
                       : SearchProperty.GetValue(n, null) == null ? false
                       : (SearchProperty.GetValue(n, null).ToString() ?? "").RemoveDiacritics(!CaseSensitive).Contains(SearchText));
        }
       
    }
}
