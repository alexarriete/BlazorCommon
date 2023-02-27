using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public class PaginatorBase : ComponentBase
    {
        [Parameter] public GridConfigurationBase GridConfig { get; set; }
        [Parameter] public EventCallback<GridConfigurationBase> GridConfigChanged { get; set; }

        protected int NumberOfPages { get; set; }

        protected async Task OnPageSelected(int pageIndex)
        {
            if (pageIndex > 0 && pageIndex <= NumberOfPages)
            {
                GridConfig.QueryResult.PageIndex = pageIndex;
                await GridConfigChanged.InvokeAsync(GridConfig);
            }
        }

        protected async Task OnPageSizeSelected(int pagesize)
        {
            GridConfig.QueryResult.PageSize = pagesize;
            GridConfig.QueryResult.PageIndex = 1;
            await GridConfigChanged.InvokeAsync(GridConfig);

        }
        protected List<int> GetPageList()
        {
            NumberOfPages = GetNumberOfPages();
            List<int> pageList = new List<int>();
            if (NumberOfPages <= 5)
            {
                for (int i = 0; i < NumberOfPages; i++)
                {
                    pageList.Add(i + 1);
                }
            }
            else if (NumberOfPages > 5)
            {
                if (NumberOfPages - GridConfig.QueryResult.PageIndex <= 5 && NumberOfPages != GridConfig.QueryResult.PageIndex)
                {
                    var ist = 5 - (NumberOfPages - GridConfig.QueryResult.PageIndex);
                    for (int i = GridConfig.QueryResult.PageIndex - ist; i <= NumberOfPages; i++)
                    {
                        pageList.Add(i);
                    }
                }
                else if (GridConfig.QueryResult.PageIndex == NumberOfPages)
                {
                    for (int i = (GridConfig.QueryResult.PageIndex - 5); i <= (NumberOfPages); i++)
                    {
                        pageList.Add(i);
                    }
                }
                else
                {
                    for (int i = GridConfig.QueryResult.PageIndex; i < (GridConfig.QueryResult.PageIndex + 5); i++)
                    {
                        pageList.Add(i);
                    }
                }
            }
            return pageList;
        }
        protected int GetNumberOfPages()
        {
            int pages = GridConfig.QueryResult.Total / GridConfig.QueryResult.PageSize;
            int mod = GridConfig.QueryResult.Total % GridConfig.QueryResult.PageSize;
            return mod > 0 ? pages + 1 : pages;
        }
    }
}
