using BlazorCommon.Grid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlazorCommon.SearchBox
{
    public class TextSearchBase : HtmlComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string InputType { get; set; }
        [Parameter] public List<Tuple<int, string>> Elements { get; set; }
        protected List<Tuple<int, string>> SelectedElements { get; set; }        
        [Parameter] public EventCallback<int> ElementIdChanged { get; set; }
        [Parameter] public EventCallback<string> TextChanged { get; set; }
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }
        [Parameter] public string DivClass { get; set; }        
        [Parameter] public string Text { get; set; }
        [Parameter] public int MinNumber { get; set; }
        [Parameter] public int MaxNumber { get; set; }
        protected string RandomId { get; set; }
        protected bool SelectedText { get; set; }
        

        protected override async Task OnInitializedAsync()
        {
            InputType = string.IsNullOrEmpty(InputType) ? "text" : InputType;         
            RandomId = GetRandomId();
            await base.OnInitializedAsync();
        }
        private string GetRandomId()
        {
            return Guid.NewGuid().ToString("N");
        }
        public async Task OnChangeTextBox(EventArgs e, string id)
        {            
            string text = await GetTextById(id);
            if(Elements != null && Elements.Count > 0)
            {
                text = text.RemoveDiacritics(true);
                SelectedElements = Elements.Where(x => x.Item2.RemoveDiacritics(true) == text).ToList();
            }    
            await TextChanged.InvokeAsync(text);
            await EnabledButtonSearchChanged.InvokeAsync(true);
        }
     
        protected async Task ResetValue()
        {
            Text = "";
            SelectedElements= null;
            SelectedText= false;
        }

        public async Task SelectTupleAsync(Tuple<int, string> tObj)
        {
            await SetTextById(RandomId, tObj.Item2);
            await TextChanged.InvokeAsync(tObj.Item2);
            await ElementIdChanged.InvokeAsync(tObj.Item1);
            await EnabledButtonSearchChanged.InvokeAsync(true);

        }

    }
}
