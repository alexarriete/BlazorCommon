
using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Grid
{
    public enum ExpandedRowOption
    {
        Detail,
        Edit,
        Delete,
        Create
    }
    public class RowBase
    {        
        public string RowBaseBackGroundColor { get; set; }
        public string RowBaseFontColor { get; set; }
        public string RowBaseId { get; set; }
        public bool RowBaseSelected { get; set; }
        public bool RowBasePreviousSelected { get; set; }
        public bool RowBaseVisible { get; set; }
        public List<ExpandedRowOption> ExpandedRowOptions { get; set; }
        public bool RowExpanded { get; set; }

        public RowBase()
        {
            RowBaseBackGroundColor = "white";
            RowBaseFontColor = "black";
            Random r = new Random();
            int rInt = r.Next(0, 100000000);
            RowBaseId = $"row_{rInt}";
            RowBaseVisible= true;
        }

        public virtual void SetExpandedRowOptions()
        {
            ExpandedRowOptions = Enum.GetValues(typeof(ExpandedRowOption)).Cast<ExpandedRowOption>().ToList();
        }

        public virtual async Task OnRowClick(JsHelper jsHelper)
        {
            RowExpanded = !RowExpanded;
            if(RowExpanded) 
            { 
                SetExpandedRowOptions();
                await jsHelper.SetSessionStorage("row", this);
            }
            else
            {
                await jsHelper.RemoveSessionStorage("row");
            }            
        }
    
    }
}
