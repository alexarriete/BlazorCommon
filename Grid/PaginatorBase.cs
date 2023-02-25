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
                GridConfig.PageIndex = pageIndex;
                await GridConfigChanged.InvokeAsync(GridConfig);
            }
        }

        protected async Task OnPageSizeSelected(int pagesize)
        {
            GridConfig.PageSize = pagesize;
            GridConfig.PageIndex = 1;
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
                if (NumberOfPages - GridConfig.PageIndex <= 5 && NumberOfPages != GridConfig.PageIndex)
                {
                    var ist = 5 - (NumberOfPages - GridConfig.PageIndex);
                    for (int i = GridConfig.PageIndex - ist; i <= NumberOfPages; i++)
                    {
                        pageList.Add(i);
                    }
                }
                else if (GridConfig.PageIndex == NumberOfPages)
                {
                    for (int i = (GridConfig.PageIndex - 5); i <= (NumberOfPages); i++)
                    {
                        pageList.Add(i);
                    }
                }
                else
                {
                    for (int i = GridConfig.PageIndex; i < (GridConfig.PageIndex + 5); i++)
                    {
                        pageList.Add(i);
                    }
                }
            }
            return pageList;
        }
        protected int GetNumberOfPages()
        {
            int pages = GridConfig.Total / GridConfig.PageSize;
            int mod = GridConfig.Total % GridConfig.PageSize;
            return mod > 0 ? pages + 1 : pages;
        }
    }
}
