using BlazorCommon.Modal;
using BlazorCommon.SearchBox;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public class ModalGridSearchBase : HtmlComponentBase
    {
        protected ModalTemplate ModalTemplate { get; set; }
        protected GridSearch GridSearch { get; set; }
        [Parameter] public EventCallback<GridSearch> GridSearchChanged { get; set; }        
        
        private GridSearch PreviousGridSearch { get; set; }
        public void Open(GridSearch gridSearch)
        { 
            GridSearch = gridSearch;
            CreatePreviousGridSearch();
            ModalTemplate.Open();
        }

        private void CreatePreviousGridSearch()
        {
            PreviousGridSearch = new GridSearch();
            PreviousGridSearch.ColumnName= GridSearch.ColumnName;
            PreviousGridSearch.NumberSearchTypeSelected = GridSearch.NumberSearchTypeSelected;
            PreviousGridSearch.Position = GridSearch.Position;
            PreviousGridSearch.SearchPropName = GridSearch.SearchPropName;
            PreviousGridSearch.SearchDateFrom= GridSearch.SearchDateFrom;
            PreviousGridSearch.SearchDateTo = GridSearch.SearchDateTo;
            PreviousGridSearch.SearchText= GridSearch.SearchText;
            PreviousGridSearch.SearchText2= GridSearch.SearchText2;
            
        }

        private bool GridSearchHaveChanged(GridSearch gridSearch)
        {
            return PreviousGridSearch.SearchText != GridSearch.SearchText || PreviousGridSearch.SearchText2 != GridSearch.SearchText2
                || PreviousGridSearch.SearchDateFrom != GridSearch.SearchDateFrom || PreviousGridSearch.SearchDateTo != GridSearch.SearchDateTo;
        }
        protected void NumberSelectionChanged(ChangeEventArgs args)
        {
            GridSearch.NumberSearchTypeSelected = (NumberSelectionType)Enum.Parse(typeof(NumberSelectionType), args.Value.ToString().Replace(" ", ""));
        }

        public void Close()
        {
            ModalTemplate.Close();
        }
        public async Task Accept()
        {
            if(GridSearchHaveChanged(GridSearch))
            {
                await GridSearchChanged.InvokeAsync(GridSearch);
                Close();
            }            
        }

        protected void SetRagFilter(int code)
        {
            string text = ((TrafficlightColor)code).ToString();
            GridSearch.SearchText = text;
        }

        protected void SetTextFilter(string text)
        {
            GridSearch.SearchText = text;
        }
    }
}
