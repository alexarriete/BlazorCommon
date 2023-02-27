using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public class SearchBarBase : HtmlComponentBase
    {
        [Parameter] public GridConfigurationBase GridConfiguration { get; set; }
        [Parameter] public Theme Theme { get; set; }
        protected List<GridSearch> GridSearches { get; set; }
        [Parameter] public EventCallback<bool> OnSearch { get; set; }
        [Parameter] public EventCallback<GridColumnBase> OnRemoveFilter { get; set; }
        protected bool EnableButtonSearch { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        protected async override Task OnParametersSetAsync()
        {
            if (GridConfiguration != null && GridConfiguration.GridColumnBases != null && GridConfiguration.GridColumnBases.Any())
            {
                GridSearches = GridConfiguration.GridColumnBases.Select(x => x.GridSearch).Where(n => n != null).ToList();
                
                EnableButtonSearch = EnableButtonSearch ? EnableButtonSearch: GridSearches.Any(x => !x.Searched);
                 
               
            }
            await base.OnParametersSetAsync();
        }

        protected async Task Search()
        {
            foreach (GridSearch gs in GridSearches)
            {
                gs.Searched = true;
            }
            EnableButtonSearch = false;
            await OnSearch.InvokeAsync(true);
        }

        protected async Task RemoveFilter(GridSearch gridSearch)
        {
            var gridcolumbase = GridConfiguration.GridColumnBases.FirstOrDefault(x => x.Name == gridSearch.ColumnName && x.Position == gridSearch.Position);
            GridSearches.Remove(gridSearch);            
            EnableButtonSearch = !EnableButtonSearch || GridSearches.Any(x => !x.Searched) ;


            await OnRemoveFilter.InvokeAsync(gridcolumbase);

        }

        protected string GetLabelString(GridSearch gridSearch)
        {
            if (gridSearch.SearchPropType == PropertyType.number && gridSearch.NumberSearchTypeSelected == NumberSelectionType.Between)
            {
                return $"{BlazorDic.Between} {gridSearch.SearchText} {BlazorDic.And.ToLower()} {gridSearch.SearchText2}";
            }
            if (gridSearch.SearchPropType == PropertyType.datetime)
            {
                return $"{BlazorDic.Between} {gridSearch.SearchDateFrom.ToShortDateString()} {BlazorDic.And.ToLower()} {gridSearch.SearchDateTo.ToShortDateString()}";
            }
            return gridSearch.SearchText;
        }
    }
}
