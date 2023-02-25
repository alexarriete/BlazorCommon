using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.SearchBox
{   
    public enum RagColor
    {
        NA,
        Red,
        Amber,
        Green
    }
    public class FilterResult
    {
        public int RagFilterValue { get; set; }
        public int RagValue { get; set; }
    }

    public class RagSearchBase : HtmlComponentBase
    {
        [Parameter] public string Label { get; set; }
                
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }
        [Parameter] public string DivClass { get; set; }
        [Parameter] public EventCallback<int> ApplyFilters { get; set; }
        [Parameter] public EventCallback<bool> ClearFilters { get; set; }
        protected Dictionary<string, object> Attributes { get; set; }
        private bool IsClearFilter { get; set; }
        private int Code { get; set; }
        protected int RagCode
        {
            get => Code;
            set
            {
                if (value != Code)
                {
                    Code = value;
                    _=ApplyFilterAsync(Code);
                }
            }
        }


        protected override async Task OnInitializedAsync()
        {          
            LoadData();
            await base.OnInitializedAsync();
        }

        private void LoadData()
        {
            Attributes = new();
            Attributes.Add("class", "btn-check");
            Attributes.Add("autocomplete", "off");
            Attributes.Add("hidden", "hidden");
            Attributes.Add("styles", "border-color: linear-gradient(to top,#81e021, #d1f3ae );");
        }

        protected string IsSelected(int code)
        {
            return (!IsClearFilter && code > 0 && code == RagCode) ? "selected-circle" : string.Empty;

        }

        protected async Task ApplyFilterAsync(int Code)
        {
            IsClearFilter = false;
            await ApplyFilters.InvokeAsync(Code);
            await EnabledButtonSearchChanged.InvokeAsync(true);
        }
    }
}
