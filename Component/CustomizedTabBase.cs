using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class CustomizedTabBase : HtmlComponentBase
    {        
        [Parameter] public List<TabElement> tabs { get; set; }
        [Parameter] public Theme Theme { get; set; }
        protected override async Task OnInitializedAsync()
        {
            tabs = tabs == null ? MyTabs.GetTabElements() : tabs;
            await base.OnInitializedAsync();
        }
    }
}
