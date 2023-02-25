using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCommon.Modal
{
    public class ModalTemplateBase : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string FadeClass { get; set; }
        [Parameter] public string ModalClass { get; set; }
        [Parameter] public string HeaderClass { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }

        public string ModalDisplay = "none;";
        public string VisibilityClass = "";

        public async Task OnInitializeAsync()
        {
            ModalClass = string.Empty;
            FadeClass = string.Empty;
            HeaderClass = string.Empty;
            await base.OnInitializedAsync();
        }

        public void Open()
        {
            ModalDisplay = "block;";
            VisibilityClass = "show";
            StateHasChanged();
        }

        protected async Task CloseAsync()
        {
            ModalDisplay = "none";
            VisibilityClass = "";
            await OnClose.InvokeAsync(true);
            await InvokeAsync(() => StateHasChanged());
        }

        public void Close()
        {
            ModalDisplay = "none";
            VisibilityClass = "";
            StateHasChanged();
        }
    }
}
