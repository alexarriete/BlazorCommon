
using Microsoft.AspNetCore.Components;

namespace BlazorCommon.Grid
{
    public class GridBase : HtmlComponentBase
    {      
        [Parameter] public GridConfigurationBase GridConfig { get; set; }
        [Parameter] public Theme Theme { get; set; }
        protected string ErrorMessage { get; set; }        
        public ModalGridSearch ModalGridSearch { get; set; }
        private bool Filtered { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Theme = Theme == null ? new Theme(): Theme;           
            await base.OnInitializedAsync();
        }

        


        protected void OnPageChanged(GridConfigurationBase gridConfiguration)
        {
            GridConfig.QueryResult.GetSortedPage(gridConfiguration);
        }



        //protected async Task DownloadExcel()
        //{

        //    byte[] bytes = await GridConfig.DownloadExcel(await GridConfig.GetListAsync());
        //    string fileName = string.IsNullOrWhiteSpace(GridConfig.ExcelFileName)
        //        ? $"{GridConfig.GridTitle}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx"
        //        : GridConfig.ExcelFileName;
        //    JsHelper jsHelper = new JsHelper();
        //    ErrorMessage = await jsHelper.DownloadExcelAsync(bytes, fileName);
        //}

        //protected async Task DownloadSample()
        //{
        //    byte[] bytes = await GridConfig.DownloadExcel(new List<object>());
        //    string fileName = "Sample_" + (string.IsNullOrWhiteSpace(GridConfig.ExcelFileName)
        //        ? $"{GridConfig.GridTitle}{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.xlsx"
        //        : GridConfig.ExcelFileName);

        //    JsHelper jsHelper = new JsHelper();
        //    ErrorMessage = await jsHelper.DownloadExcelAsync(bytes, fileName);
        //}


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

        protected void GetFilteredList(bool result)
        {
            if (result)
            {
                Filtered = true;
                GridConfig.QueryResult.GetSortedPage(GridConfig);
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
                    GridConfig.QueryResult.GetSortedPage(GridConfig);
                }
            }
        }

        protected async Task OnRowClick(RowBase item)
        {
            if (!item.RowExpanded)
            {
                foreach (var il in GridConfig.QueryResult.List)
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
