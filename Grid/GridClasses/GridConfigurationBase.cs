using BlazorCommon.Grid.GridClasses;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public class GridConfigurationBase
    {
        public QueryResultBase QueryResult { get; set; }
        public List<GridColumnBase> GridColumnBases { get; set; }
        public List<RowBase> ItemListUploaded { get; set; }
        public Type ItemType { get; set; }
        public string GridTitle { get; set; }
        public string KeyColumn { get; set; }
        public string ExcelFileName { get; set; }


        private GridConfigurationBase() { }

        public static GridConfigurationBase GetInstance()
        {
            GridConfigurationBase GridConfig = new GridConfigurationBase();
            GridConfig.QueryResult = GridConfig.GetSourceList();
            GridConfig.ItemType = GridConfig.QueryResult.List.FirstOrDefault().GetType();
            GridConfig.GridTitle = GridConfig.SetGridTitle();

            GridConfig.KeyColumn = GridConfig.ItemType.GetProperties()
               .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), inherit: false).Any())?.Name;

            GridConfig.GridColumnBases = GridConfig.GetGridColumnBase();
            GridConfig.ExcelFileName = GridConfig.SetExcelFileName();

            return GridConfig;
        }

        public virtual IEnumerable<RowBase> GetList()
        {
            return Animal.GetAll();
        }

        public static async Task<GridConfigurationBase> GetInstanceAsync()
        {
            return await Task.Run(() => GetInstance());
        }

        public virtual string SetGridTitle()
        {
            return $"{ItemType.Name} list";
        }

        public virtual string SetExcelFileName()
        {
            return $"{ItemType.Name}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx";
        }

        private QueryResultBase PaginatedQueryResult()
        {
            QueryResult.Total = QueryResult.List.Count();
            QueryResult.NotFilteredTotal =QueryResult.NotFilteredTotal == 0 ? QueryResult.Total: QueryResult.NotFilteredTotal;
            QueryResult.List = QueryResult.List.Skip((QueryResult.PageIndex - 1) * QueryResult.PageSize).Take(QueryResult.PageSize);
            return QueryResult;
        }



        public virtual QueryResultBase GetSortedPage(SortChangedEvent sort)
        {            
            
            if (sort != null)
            {
                QueryResult.List = GetList();
                PropertyInfo prop = ItemType.GetProperty(sort.SortId);
                if (sort.Direction == SortDirection.Desc)
                {
                    QueryResult.List = QueryResult.List.OrderByDescending(x => prop.GetValue(x, null));
                }
                else
                {
                    QueryResult.List = QueryResult.List.OrderBy(x => prop.GetValue(x, null));
                }
                QueryResult.PageIndex = 1;
                QueryResult = PaginatedQueryResult();
            }
            else
                QueryResult = GetSourceList();

            
            return QueryResult;


        }


        public virtual List<GridColumnBase> GetGridColumnBase()
        {
            List<PropertyInfo> baseProperties = new RowBase().GetType().GetProperties().ToList();
            List<PropertyInfo> props = ItemType.GetProperties().Where(x => !baseProperties.Any(s => s.Name == x.Name)).ToList();
            return props.Select(x => new GridColumnBase(x, props.IndexOf(x), KeyColumn)).ToList();
        }

        public virtual QueryResultBase GetSourceList()
        {
            //first time
            IEnumerable<RowBase> rows = new List<RowBase>();
            if (QueryResult == null)
            {
                rows = GetList();
                QueryResult = new QueryResultBase();
                QueryResult.List = rows;                                
                QueryResult.PageSize = 10;
                QueryResult.PageIndex = 1;
                return PaginatedQueryResult();
            }
            else
            {
                List<GridSearch> gridSearches = GridColumnBases.Select(x => x.GridSearch).Where(x => x != null).ToList();
                if (!gridSearches.Any())
                {
                    QueryResult.List = GetList();
                    return PaginatedQueryResult();
                }
                else
                {
                    rows = GetList();
                    foreach (GridSearch search in gridSearches)
                    {
                        if (search.SearchPropType == PropertyType.datetime)
                        {
                            rows = search.GetFilteredByDateTimeInterval(rows);
                            continue;
                        }
                        else if (search.SearchPropType == PropertyType.number)
                        {
                            switch (search.NumberSearchTypeSelected)
                            {
                                case NumberSelectionType.Greaterthan:
                                    rows = search.GetGreaterThanValues(rows);
                                    break;
                                case NumberSelectionType.Lessthan:
                                    rows = search.GetLessThanValues(rows);
                                    break;
                                case NumberSelectionType.Between:
                                    rows = search.GetBetweenValues(rows);
                                    break;
                                default:
                                    rows = search.GetEqualsValues(rows);
                                    break;

                            }
                            continue;
                        }
                        rows = search.GetTextContains(rows);
                    }
                }
                QueryResult.List = rows;                
                return PaginatedQueryResult();
            }

        }


        public async virtual Task<byte[]> DownloadExcel(IEnumerable<object> itemList)
        {
            GridToExcelBase gridToExcel = new GridToExcelBase();
            return await Task.Run(() => gridToExcel.GenerateReport(itemList.ToList(), this));
        }

        public async virtual Task<string> ProcessItemsUploaded(List<object> objects)
        {
            return "You must overrride ProcessItemsUploaded in GridConfiguration";
        }

        private object GetItem(DataRow dr)
        {
            var obj = Activator.CreateInstance(ItemType);

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (GridColumnBase cb in GridColumnBases)
                {
                    if (cb.PropertyInfo.Name == column.ColumnName)
                    {
                        PropertyType pt = cb.PropertyType;
                        if (pt == PropertyType.number)
                        {
                            cb.PropertyInfo.SetValue(obj, Int32.Parse(dr[column.ColumnName].ToString()), null);
                        }
                        else
                        {
                            cb.PropertyInfo.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }





    }


}
