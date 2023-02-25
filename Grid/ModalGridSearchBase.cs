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

        public void Open(GridSearch gridSearch)
        { 
            GridSearch = gridSearch;
            ModalTemplate.Open();
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
            await GridSearchChanged.InvokeAsync(GridSearch);
            Close();
        }

        protected void SetRagFilter(int code)
        {
            string text = ((RagColor)code).ToString();
            GridSearch.SearchText = text;
        }

        protected void SetTextFilter(string text)
        {
            GridSearch.SearchText = text;
        }
    }
}
