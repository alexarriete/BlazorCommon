
using Microsoft.AspNetCore.Components;

namespace BlazorCommon.Grid
{
    public class GridBase : HtmlComponentBase
    {      
        [Parameter] public GridConfigurationBase GridConfig { get; set; }
        [Parameter] public Theme Theme { get; set; }
        protected string ErrorMessage { get; set; }
        private SortChangedEvent Sort { get; set; }
        public ModalGridSearch ModalGridSearch { get; set; }
        private bool Filtered { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Theme = Theme == null ? new Theme(): Theme;
            if (GridConfig != null)
            {
                await ConfigureProperties();
            }
            await base.OnInitializedAsync();
        }

        private async Task ConfigureProperties()
        {
            GridConfig.Total = (await GridConfig.GetListAsync()).Count();
            GridConfig.ItemList = await GridConfig.GetPageAsync(Sort);
        }

        protected async Task SortColumn(GridColumnBase thisColumn)
        {
            foreach (GridColumnBase gridColumn in GridConfig.GridColumnBases)
            {
                if (gridColumn == thisColumn)
                {
                    if (gridColumn.SortSymbol == "&#8593;")
                    {
                        gridColumn.SortSymbol = "";
                        Sort = null;
                    }
                    else if (gridColumn.SortSymbol == "&#8595;")
                    {
                        gridColumn.SortSymbol = "&#8593;";
                        Sort = new SortChangedEvent() { SortId = gridColumn.PropertyInfo.Name, Direction = SortDirection.Desc };
                    }
                    else
                    {
                        gridColumn.SortSymbol = "&#8595;";
                        //Sort = new SortChangedEvent();
                        Sort = new SortChangedEvent() { SortId = gridColumn.PropertyInfo.Name, Direction = SortDirection.Asc };
                    }
                }
                else
                {
                    gridColumn.SortSymbol = "";
                }
            }
            GridConfig.ItemList = await GridConfig.GetPageAsync(Sort);
        }


        protected async Task OnPageChanged(GridConfigurationBase gridConfiguration)
        {
            GridConfig.ItemList = await GridConfig.GetPageAsync(Sort);
        }



        protected async Task Test()
        {
            try
            {
                await GridConfig.TestAsync();
                await SetToast(MessageType.success, "It's working");

            }
            catch (Exception ex)
            {
                await SetToast(MessageType.error, ex.Message);
            }


        }

        protected async Task DownloadExcel()
        {
            byte[] bytes = await GridConfig.DownloadExcel(await GridConfig.GetListAsync());
            string fileName = string.IsNullOrWhiteSpace(GridConfig.ExcelFileName)
                ? $"{GridConfig.GridTitle}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx"
                : GridConfig.ExcelFileName;
            JsHelper jsHelper = new JsHelper();
            ErrorMessage = await jsHelper.DownloadExcelAsync(bytes, fileName);
        }

        protected async Task DownloadSample()
        {
            byte[] bytes = await GridConfig.DownloadExcel(new List<object>());
            string fileName = "Sample_" + (string.IsNullOrWhiteSpace(GridConfig.ExcelFileName)
                ? $"{GridConfig.GridTitle}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx"
                : GridConfig.ExcelFileName);

            JsHelper jsHelper = new JsHelper();
            ErrorMessage = await jsHelper.DownloadExcelAsync(bytes, fileName);
        }


        protected void OpenCloseFilter(GridColumnBase gridColumn)
        {
            var gridSearch = new GridSearch(gridColumn);
            ModalGridSearch.Open(gridSearch);
        }

        protected async Task RemoveFilter(GridColumnBase gridColumn)
        {
            gridColumn.GridSearch = null;
        }

        protected async Task AddSearch(GridSearch gridSearch)
        {
            GridConfig.GridColumnBases.FirstOrDefault(x => x.Name == gridSearch.ColumnName && x.Position == gridSearch.Position).GridSearch = gridSearch;
        }

        protected async Task GetFilteredList(bool result)
        {
            if (result)
            {
                Filtered = true;
                GridConfig.ItemList = await GridConfig.GetPageAsync(Sort);
            }
            else
            {
                foreach (GridColumnBase gcb in GridConfig.GridColumnBases)
                {
                    gcb.GridSearch = null;
                }
                if (Filtered)
                {
                    Filtered = false;
                    GridConfig.ItemList = await GridConfig.GetPageAsync(Sort);
                }
            }
        }

        protected async Task OnRowClick(RowBase item)
        {
            if (!item.RowExpanded)
            {
                foreach (var il in GridConfig.ItemList)
                {
                    il.RowExpanded = false;
                }               
            }
            await item.OnRowClick(new JsHelper(jSRuntime));

        }

        protected async Task GoToPage(ExpandedRowOption ero)
        {
            bool result = await GotoPageAsync($"{UrlHelper.Uri}/{ero}");
            if (!result)
                await SetToast(MessageType.error, "La página no existe");
        }

    }
}
