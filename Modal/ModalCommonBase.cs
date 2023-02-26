using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;


namespace BlazorCommon.Modal
{
    public class ModalCommonBase : ComponentBase
    {
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        private IJSRuntime JSRuntime { get; set; }

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public ModalType ModalType { get; set; }
        public MessageType MessageType { get; set; }
        protected string Body { get; set; }
                
        [Parameter] public EventCallback<bool> OutResultChanged { get; set; }


        public void Open(IJSRuntime jSRuntime, ModalType modalType)
        {
            JSRuntime = jSRuntime;
            ModalType = modalType;
            CreateBody();

            ModalDisplay = "block;";
            ModalClass = "show  alert-dark";
            ShowBackdrop = true;
            InvokeAsync(() => StateHasChanged());
        }

        private void CreateBody()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (ModalType == ModalType.AcceptDecline || ModalType == ModalType.Alert)
            {
                stringBuilder.Append($"<p class='text-center'>{Text}</p>");
            }
            else if (ModalType == ModalType.GetSingleText)
            {
                Text = string.Empty;
                stringBuilder.Append($"<input type='text' class='form-control' id='textId' autocomplete='off' />");                
            }
            Body = stringBuilder.ToString();
        }

        public async Task Close(bool accepted)
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            if (!accepted)
            {
                await OutResultChanged.InvokeAsync(false);
            }
        }

        public async Task Confirm()
        {
            if (ModalType == ModalType.GetSingleText)
                Text = await new JsHelper(JSRuntime).JsGetTextById("textId");            
            await OutResultChanged.InvokeAsync(true);
            await Close(true);
        }



    }
}
