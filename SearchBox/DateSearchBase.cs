using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.SearchBox
{
    public class DateSearchBase : HtmlComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public DateTime Min { get; set; }
        protected string MinDate { get; set; }
        [Parameter] public DateTime Max { get; set; }
        protected string MaxDate { get; set; }
        private DateTime date { get; set; }
        [Parameter] public DateTime Date { get { return date; } set { date = value; _ = DateChangeAsync(); } }
        private DateTime AuxDate { get; set; }
        [Parameter] public EventCallback<DateTime> DateChanged { get; set; }
        [Parameter] public EventCallback<bool> EnabledButtonSearchChanged { get; set; }
        [Parameter] public string DivClass { get; set; }
        private bool Check { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SetMaxMin();
            await base.OnInitializedAsync();
        }

        private void SetMaxMin()
        {            
            Min = DateTime.MinValue== Min || DateTime.MaxValue == Min ? DateTime.MinValue : Min;
            Max = DateTime.MinValue == Max || DateTime.MaxValue == Max ? DateTime.MaxValue : Max;
            MinDate = ConvertDate(Min);
            MaxDate = ConvertDate(Max);
            if(Date== DateTime.MinValue) Date = Min;
            Check = true;
        }

        private string ConvertDate(DateTime date)
        {
            string day = date.Day < 10 ? $"0{date.Day}" : date.Day.ToString();
            string month = date.Month < 10 ? $"0{date.Month}" : date.Month.ToString();
            return $"{date.Year}-{month}-{day}";
        }

        private async Task DateChangeAsync()
        {
            if (AuxDate == Date)
                return; 
            
            if(Check)
            {
                if (Date.Date < Min.Date)
                {
                    await SetToast(MessageType.error, $"{BlazorDic.ErrorDateMin} {MinDate}");
                    return;
                }
                if (Date.Date > Max.Date)
                {
                    await SetToast(MessageType.error, $"{BlazorDic.ErrorDateMax} {MaxDate}");
                    return;
                }
                await EnabledButtonSearchChanged.InvokeAsync(true);
                await DateChanged.InvokeAsync(Date);
                AuxDate = Date;
            }
        }


    }
}
