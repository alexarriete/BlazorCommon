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
        public string Title { get; set; }
        public string KeyColumn { get; set; }
        public string ExcelFileName { get; set; }


        public GridConfigurationBase(QueryResultBase queryResultBase = null) 
        { 
            QueryResult = queryResultBase == null ? new QueryResultBase(): queryResultBase;
            QueryResult.GetSortedPage(this);
            ItemType = QueryResult.List.FirstOrDefault().GetType();
           
            SetGridTitle();
            SetKeyColumn();
            SetGridColumnBase();

            SetExcelFileName();
        }

        private void SetKeyColumn()
        {
            KeyColumn = ItemType.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), inherit: false).Any())?.Name;
        }

        public virtual void SetGridTitle()
        {
            Title = $"{ItemType.Name} list";
        }

        public virtual void SetExcelFileName()
        {
            ExcelFileName = $"{ItemType.Name}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx";
        }

        public virtual void SetGridColumnBase()
        {
            List<PropertyInfo> baseProperties = new RowBase().GetType().GetProperties().ToList();
            List<PropertyInfo> props = ItemType.GetProperties().Where(x => !baseProperties.Any(s => s.Name == x.Name)).ToList();
            GridColumnBases = props.Select(x => new GridColumnBase(x, props.IndexOf(x), KeyColumn)).ToList();
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
