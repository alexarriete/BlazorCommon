using BlazorCommon.Dummy;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.SearchBox
{
    public class MultipleSelectorBase: HtmlComponentBase
    {
        [Parameter] public List<OptionElement> OptionElements { get; set; }
        [Parameter] public string DivClass { get; set; }
        [Parameter] public EventCallback<List<OptionElement>> OptionElementChanged { get; set; }
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public string Placeholder  { get; set; }        
        protected string ListContainer { get; set; }

        protected List<string> SelectCityList = new List<string>();
        protected ElementReference MySelect { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if(OptionElements == null) { OptionElements = OptionElementTest.GetElements(); }
            Placeholder = string.IsNullOrEmpty(Placeholder) ? BlazorDic.Select : Placeholder;
            ListContainer = Placeholder;
            

            await base.OnInitializedAsync();
        }
        protected async Task OnChangeCitiesSelected()
        {
            BlazorCommon.JsHelper jsHelper = new BlazorCommon.JsHelper(jSRuntime);
            string elementSelected = await jsHelper.GetSelectedElement("slt"); 
            await CreateMultipleValue(elementSelected);
            await jsHelper.SelectElement(MySelect);
            
        }

        private async Task CreateMultipleValue(string elementSelected)
        {
            OptionElement optionElement = OptionElements.FirstOrDefault(x => x.Name == elementSelected);
            if (optionElement != null)
            {
                optionElement.Active = !optionElement.Active;
            }
            ListContainer = OptionElements.Any(x => !x.Active)
                ? string.Join(" , ", OptionElements.Where(x => !x.Active).Select(n => n.Name))
                : Placeholder;

            OptionElements = OptionElements.OrderByDescending(x => x.Active).ThenBy(x => x.Name).ToList();

            await OptionElementChanged.InvokeAsync(OptionElements.Where(x => !x.Active).ToList());
            await EnabledButtonSearchChanged.InvokeAsync(true);
        }
    }
}
