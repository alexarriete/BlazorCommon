using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid.GridClasses
{
    /// <summary>
    /// Contains the properties required to obtain the rows. Overwrite this class and the GetList() method to get the desired row list.
    /// The object that conforms to the row must be instances of a class that inherits from RowBase.
    /// More info: https://blazorcommon.acernuda.com
    /// </summary>
    public class QueryResultBase
    {
        public int NotFilteredTotal { get; set; }
        public int Total { get; set; }
        public IEnumerable<RowBase> List { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public SortChangedEvent Sort { get; set; }

        public QueryResultBase() 
        {
            PageSize = 10;
            PageIndex = 1;
        }

        public virtual void GetList()
        {
            List = Animal.GetAll();
        }
       
        public virtual void GetFilteredList(List<GridColumnBase> gridColumnBases)
        {
            GetList();
            List<GridSearch> gridSearches = gridColumnBases?.Select(x => x.GridSearch).Where(x => x != null).ToList();
            if (gridSearches != null && gridSearches.Any())
            {
                foreach (GridSearch search in gridSearches)
                {
                    if (search.SearchPropType == PropertyType.datetime)
                    {
                        List = search.GetFilteredByDateTimeInterval(List);
                        continue;
                    }
                    else if (search.SearchPropType == PropertyType.number)
                    {
                        switch (search.NumberSearchTypeSelected)
                        {
                            case NumberSelectionType.Greaterthan:
                                List = search.GetGreaterThanValues(List);
                                break;
                            case NumberSelectionType.Lessthan:
                                List = search.GetLessThanValues(List);
                                break;
                            case NumberSelectionType.Between:
                                List = search.GetBetweenValues(List);
                                break;
                            default:
                                List = search.GetEqualsValues(List);
                                break;

                        }
                        continue;
                    }
                    List = search.GetTextContains(List);
                }
            }
        }

        public virtual void SortColumn(GridConfigurationBase gridConfig, GridColumnBase thisColumn)
        {
            RemoveSymbols(gridConfig, thisColumn);

            foreach (GridColumnBase gridColumn in gridConfig.GridColumnBases.Where(x => x == thisColumn))
            {
                if (gridColumn.SortSymbol == "&#8593;")
                {
                    gridColumn.SortSymbol = "";
                    Sort = null;
                }
                else if (gridColumn.SortSymbol == "&#8595;")
                {
                    gridColumn.SortSymbol = "&#8593;";
                    Sort = new SortChangedEvent() { Prop = gridColumn.PropertyInfo, Direction = SortDirection.Desc };
                }
                else
                {
                    gridColumn.SortSymbol = "&#8595;";
                    Sort = new SortChangedEvent() { Prop = gridColumn.PropertyInfo, Direction = SortDirection.Asc };
                }
            }
            PageIndex = 1;
            GetSortedPage(gridConfig);
        }

        private void RemoveSymbols(GridConfigurationBase gridConfig, GridColumnBase thisColumn)
        {
            foreach (GridColumnBase gridColumn in gridConfig.GridColumnBases
                .Where(x => x.Name != thisColumn.Name && x.Position != thisColumn.Position))
            {
                gridColumn.SortSymbol = "";
            }
        }

        public virtual void GetSortedPage(GridConfigurationBase gridConfig)
        {
            GetFilteredList(gridConfig.GridColumnBases);
            if (Sort != null)
            {                
                if (Sort.Direction == SortDirection.Desc)
                {
                    List = List.OrderByDescending(x => Sort.Prop.GetValue(x, null));
                }
                else
                {
                    List = List.OrderBy(x => Sort.Prop.GetValue(x, null));
                }                           
            }
            PaginatedQueryResult();

        }

        private void PaginatedQueryResult()
        {
            Total = List.Count();
            NotFilteredTotal = NotFilteredTotal == 0 ? Total : NotFilteredTotal;
            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize);
        }
    }


}
