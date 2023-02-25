using BlazorCommon.Grid;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon
{
    public class TabElement
    {        
        public string Title { get; set; }
        public string CssId { get; set; }
        public string ButtonName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Footer { get; set; }
        public bool Active { get; set; }
        public TabType Type { get; set; }
        public GridConfigurationBase GridConfig { get; set; }
    }
    public class TabsBase : HtmlComponentBase
    {
        [Parameter] public RenderFragment<TabElement> TabContent { get; set; }        
        [Parameter] public List<TabElement> tabs { get; set; }
        [Parameter] public Theme Theme { get; set; }
        public int SelectedId { get; set; }
        protected override async Task OnInitializedAsync()
        {            
            Theme = Theme== null ? new Theme() : Theme;
            tabs = tabs == null ? MyTabs.GetTabElements() : tabs;
            await base.OnInitializedAsync();
        }

        public virtual void OnClick(TabElement tab)
        {
            foreach (var item in tabs)
            {
                item.Active = false;
            }
            tab.Active = true;         
        }

      

    }
}
