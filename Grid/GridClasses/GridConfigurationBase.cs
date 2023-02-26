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
        public List<GridColumnBase> GridColumnBases { get; set; }
        public List<RowBase> ItemList { get; set; }
        public List<RowBase> ItemListUploaded { get; set; }
        public Type ItemType { get; set; }
        public int PageSize { get; set; }
        private int total;
        public int Total { get { return total; } set { total = value; TotalChanged(); } }        
        public int PreviousTotal { get; set; }
        public int PageIndex { get; set; }
        public string GridTitle { get; set; }
        public string KeyColumn { get; set; }             
        public string ExcelFileName { get; set; }        
        public string UrlFolder { get; set; }
        

        public GridConfigurationBase()
        {
            Inizialice();
        }

        private async Task Inizialice()
        {
            PageSize = 10;
            PageIndex = 1;
            ItemType = await GetObjectTypeAsync();
            GridTitle = $"{ItemType.Name} list";

            KeyColumn = (await GetSourceListAsync()).FirstOrDefault().GetType().GetProperties()
               .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), inherit: false).Any())?.Name;

            GridColumnBases = GetGridColumnBase();
            ExcelFileName = $"{ItemType.Name}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx";
            UrlFolder = ItemType.Name.ToLower();
        }

        public virtual void TotalChanged()
        {
            PreviousTotal = PreviousTotal == 0 ? Total : PreviousTotal;
        }

        public async Task<IEnumerable<RowBase>> GetListAsync()
        {
            List<GridSearch> gridSearches = GridColumnBases.Select(x=>x.GridSearch).Where(x=>x != null).ToList();
            if (!gridSearches.Any())
            {
                var result = (await GetSourceListAsync());
                Total = result.ToList().Count;
                return result;
            }
            else
            {
                List<RowBase> rows = (await GetSourceListAsync()).ToList();
                foreach (GridSearch search in gridSearches)
                {
                    if (search.SearchPropType == PropertyType.datetime)
                    {
                        rows = await search.GetFilteredByDateTimeInterval(rows);
                        continue;
                    }
                    else if (search.SearchPropType == PropertyType.number)
                    {
                        switch (search.NumberSearchTypeSelected)
                        {
                            case NumberSelectionType.Greaterthan:
                                rows = await search.GetGreaterThanValues(rows);
                                break;
                            case NumberSelectionType.Lessthan:
                                rows = await search.GetLessThanValues(rows);
                                break;
                            case NumberSelectionType.Between:
                                rows = await search.GetBetweenValues(rows);
                                break;
                            default:
                                rows = await search.GetEqualsValues(rows);
                                break;
                                
                        }
                        continue;
                    }
                    rows = await search.GetTextContains(rows);
                    
                    if (!rows.Any())
                        return rows;
                }               
                Total = rows.ToList().Count;
                return rows;
            }            
        }


        public async Task<List<RowBase>> GetPageAsync(SortChangedEvent sort)
        {
            if (sort == null)
            {
                return (await GetListAsync())
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                PropertyInfo prop = ItemType.GetProperty(sort.SortId);
                if (sort.Direction == SortDirection.Desc)
                {

                    return (await GetListAsync()).OrderByDescending(x => prop.GetValue(x, null))
                        .Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                }
                else
                {
                    return (await GetListAsync()).OrderBy(x => prop.GetValue(x, null))
                        .Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                }
            }
        }

        private async Task<Type> GetObjectTypeAsync()
        {
            return (await GetSourceListAsync()).FirstOrDefault().GetType();
        }

        public virtual List<GridColumnBase> GetGridColumnBase()
        {
            List<PropertyInfo> baseProperties = new RowBase().GetType().GetProperties().ToList();
            List<PropertyInfo> props = ItemType.GetProperties().Where(x=> !baseProperties.Any(s=>s.Name ==x.Name )).ToList();
            return  props.Select(x => new GridColumnBase(x, props.IndexOf(x), KeyColumn)).ToList();
        }      

        public async virtual Task<IEnumerable<RowBase>> GetSourceListAsync()
        {
            return await Animal.GetAll();
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



        public virtual async Task TestAsync()
        {
            //await TestPaginatorAsync(10);
            await TestColumnOrderAsync();
        }


         

        private List<object> ConvertDataTable(DataTable dt)
        {
            List<object> data = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                object item = GetItem(row);
                data.Add(item);
            }
            return data;
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
                        if(pt == PropertyType.number)
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


      

        private async Task TestPaginatorAsync(int numberOfPages)
        {
            for (int i = 0; i < numberOfPages; i++)
            {                
                Random rand = new Random();
                int rInt = rand.Next(0, Total / PageSize);
                PageIndex = rInt;
               
                ItemList =  await GetPageAsync(null);
            }
        }

        private async Task TestColumnOrderAsync()
        {
            foreach (GridColumnBase column in GridColumnBases)
            {
                SortChangedEvent sort = new SortChangedEvent() { SortId = column.PropertyInfo.Name, Direction = SortDirection.Asc };
                ItemList = await GetPageAsync(sort);
            }
        }

    }

   
}
