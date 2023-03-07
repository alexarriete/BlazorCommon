using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.SearchBox
{
    public class ListSearchBase : HtmlComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public List<OptionElement> Elements { get; set; }
        private int elementId { get; set; }
        public int ElementId { get { return elementId; } set { elementId = value; _=ElementChangeAsync(); } }
        [Parameter] public EventCallback<OptionElement> ElementSelected { get; set; }
        
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }       
        [Parameter] public string DivClass { get; set; }

        protected override async Task OnInitializedAsync()
        {            
            await base.OnInitializedAsync();
        }     

        private async Task ElementChangeAsync()
        {
            var element = Elements.FirstOrDefault(x=>x.Value == ElementId);
            await ElementSelected.InvokeAsync(element);            
            await EnabledButtonSearchChanged.InvokeAsync(true);
        }


    }
}
