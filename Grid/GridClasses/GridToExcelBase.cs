using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public class GridToExcelBase
    {
        public List<char> Columns { get; set; }

        private List<char> GetColumns(int columnNumber)
        {
            List<char> characters = new List<char>();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                characters.Add(letter);
            }
            return characters.Take(columnNumber).ToList();
        }
        public byte[] GenerateReport(List<object> itemList, GridConfigurationBase gridConfig)
        {
            List<GridColumnBase> columnList = gridConfig.GridColumnBases.Where(x => !x.KeyColumn).ToList();
            this.Columns = GetColumns(columnList.Count());

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet ws = workbook.Worksheets.Add("Info General");

                CreateHeader(columnList, ws);

                foreach (object obj in itemList)
                {
                    int file = itemList.IndexOf(obj) + 2;
                    foreach (GridColumnBase cName in columnList)
                    {
                        int column = columnList.IndexOf(cName);
                        var value = GridColumnBase.GetKeyValue(obj, cName.PropertyInfo);
                        ws.Cell($"{Columns[column]}{file}")
                            .Value = value.ToString();
                    }
                }

                ws.Columns().AdjustToContents();


                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        private void CreateHeader(List<GridColumnBase> columNameList, IXLWorksheet ws)
        {
            foreach (GridColumnBase cName in columNameList)
            {
                ws.Cell($"{Columns[columNameList.IndexOf(cName)]}{1}").Value = cName.Name;                
            }

            var rngTable = ws.Range($"{Columns[0]}{1}:{Columns.Last()}{1}");
            var rngHeaders = rngTable.Range($"{Columns[0]}1:{Columns.Last()}1"); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
            rngHeaders.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


        }

        private XLDataType GetDataType(GridColumnBase gridColumn)
        {
            PropertyType propertyType = gridColumn.PropertyType;
            switch (propertyType)
            {
                case PropertyType.number:
                    return XLDataType.Number;
                case PropertyType.datetime:
                    return XLDataType.DateTime;
                default:
                    return XLDataType.Text;
            }
        }     

        private static void AddDatatableRow(System.Data.DataTable dataResult, string readRange, IXLRow row)
        {
            dataResult.Rows.Add();
            int cellIndex = 0;
            foreach (IXLCell cell in row.Cells(readRange))
            {
                dataResult.Rows[dataResult.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                cellIndex++;
            }
        }

        private static string GetColumnsNames(List<GridColumnBase> columnList, System.Data.DataTable dataResult, IXLRow row)
        {
            string readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
            foreach (IXLCell cell in row.Cells(readRange))
            {
                GridColumnBase gridColumn = columnList.FirstOrDefault(x => x.Name == cell.Value.ToString());
                if(gridColumn != null)
                {
                    var columName = gridColumn.PropertyInfo.Name;
                    dataResult.Columns.Add(columName);
                }
                else
                {
                    throw new System.Exception($"Column Name {cell.Value.ToString()} does not match any property in object");
                }
                
            }

            return readRange;
        }

    }


}
