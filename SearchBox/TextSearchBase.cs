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
    public enum InputType
    {
        text,
        number
    }
    public class TextSearchBase : HtmlComponentBase
    {
        [Parameter] public string? Label { get; set; }
        [Parameter] public string? Placeholder { get; set; }
        [Parameter] public InputType InputType { get; set; }
        [Parameter] public List<OptionElement>? Elements { get; set; }
        protected List<OptionElement>? SelectedElements { get; set; }        
        [Parameter] public EventCallback<int> ElementIdChanged { get; set; }
        [Parameter] public EventCallback<string> TextChanged { get; set; }
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }
        [Parameter] public string? DivClass { get; set; }        
        [Parameter] public string? Text { get; set; }
        [Parameter] public int MinNumber { get; set; }
        [Parameter] public int MaxNumber { get; set; }
        protected string? RandomId { get; set; }
        protected bool SelectedText { get; set; }
        

        protected override async Task OnInitializedAsync()
        {
            if (MinNumber == 0 && MaxNumber == 0)
            {
                MinNumber = int.MinValue;
                MaxNumber = int.MaxValue;
            }
                
                        
            RandomId = GetRandomId();
            await base.OnInitializedAsync();
        }
        private string GetRandomId()
        {
            return Guid.NewGuid().ToString("N");
        }
        public async Task OnChangeTextBox(EventArgs e, string id)
        {            
            Text = await GetTextById(id);
            if(Elements != null && Elements.Count > 0)
            {
                var text = Text.RemoveDiacritics(true);
                SelectedElements = Elements.Where(x => x.Name.RemoveDiacritics(true).Contains(text)).ToList();
            }    
            await TextChanged.InvokeAsync(Text);
            await EnabledButtonSearchChanged.InvokeAsync(true);
           
        }
     
        protected async Task ResetValue()
        {
            Text = "";
            SelectedElements= null;
            SelectedText= false;
        }

        public async Task SelectOptionElementAsync(OptionElement tObj)
        {
            SelectedText = true;
            Text = tObj.Name;
            await SetTextById(RandomId, tObj.Name);
            await TextChanged.InvokeAsync(tObj.Name);
            await ElementIdChanged.InvokeAsync(tObj.Value);
            await EnabledButtonSearchChanged.InvokeAsync(true);

        }

    }
}
