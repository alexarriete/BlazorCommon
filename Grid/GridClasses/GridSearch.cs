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

        public async Task<List<RowBase>> GetFilteredByDateTimeInterval(List<RowBase> objects)
        {
            DateTime result = DateTime.MinValue;
            return await Task.Run(() => objects.Where(n => (SearchProperty == null ? true
                     : SearchProperty.GetValue(n, null) == null)
                     || (!DateTime.TryParse(SearchProperty.GetValue(n, null).ToString(), out result) ? false
                     : Convert.ToDateTime(SearchProperty.GetValue(n, null)) > SearchDateFrom)
                        && (Convert.ToDateTime(SearchProperty.GetValue(n, null)) <= SearchDateTo)).ToList());
        }

        public async Task<List<RowBase>> GetEqualsValues(List<RowBase> objects)
        {
            return await Task.Run(()=> objects.Where(n => SearchProperty == null ? true
                        : SearchProperty.GetValue(n, null) == null ? false
                        : (SearchProperty.GetValue(n, null).ToString() == SearchText)).ToList());
        }

        public async Task<List<RowBase>> GetGreaterThanValues(List<RowBase> objects)
        {
            return await Task.Run(() => objects.Where(n => SearchProperty == null ? true
                         : SearchProperty.GetValue(n, null) == null ? false
                         : (double.Parse(SearchProperty.GetValue(n, null).ToString()) > double.Parse(SearchText))).ToList());
        }

        public async Task<List<RowBase>> GetLessThanValues(List<RowBase> objects)
        {
            return await Task.Run(() => objects.Where(n => SearchProperty == null ? true 
            : SearchProperty.GetValue(n, null) == null ? false 
            : (double.Parse(SearchProperty.GetValue(n, null).ToString()) < double.Parse(SearchText))).ToList());
        }

        public async Task<List<RowBase>> GetBetweenValues(List<RowBase> objects)
        {
            return await Task.Run(() => objects.Where(n => SearchProperty == null ? true
                         : SearchProperty.GetValue(n, null) == null ? false
                          : ( (double.Parse(SearchProperty.GetValue(n, null).ToString()) > double.Parse(SearchText)) 
                               && double.Parse(SearchProperty.GetValue(n, null).ToString()) < double.Parse(SearchText2))).ToList());
        }

        public async Task<List<RowBase>> GetTextContains(List<RowBase> objects)
        {
            SearchText = SearchText.RemoveDiacritics(!CaseSensitive);
            return  await Task.Run(()=> objects.Where(n => SearchProperty == null ? true
                       : SearchProperty.GetValue(n, null) == null ? false
                       : (SearchProperty.GetValue(n, null).ToString() ?? "").RemoveDiacritics(!CaseSensitive).Contains(SearchText)).ToList());
        }
       
    }
}
