using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;


namespace BlazorCommon.Modal
{
    public class ModalCommonBase : HtmlComponentBase
    {
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Text { get; set; }
        public ModalType ModalType { get; set; }
        public MessageType MessageType { get; set; }
        protected string Body { get; set; }

        [Parameter] public bool OutResult { get; set; }
        [Parameter] public EventCallback<bool> OutResultChanged { get; set; }


        public void Open(MessageType messageType, ModalType modalType)
        {
            ModalType = modalType;
            MessageType = messageType;
            SelectTitleAndSubtitle();

            CreateBody();

            ModalDisplay = "block;";
            ModalClass = "Show";
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
                stringBuilder.Append("<p class='text-center'>Escriba el valor</p>");
            }
            Body = stringBuilder.ToString();
        }

        public async Task Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            await OutResultChanged.InvokeAsync(OutResult);
        }

        public async Task Confirm()
        {
            Text = await new JsHelper().JsGetTextById("textId");
            OutResult = true;
            await Close();
        }

        private void SelectTitleAndSubtitle()
        {
            Title = SubTitle = string.Empty;
            switch (MessageType)
            {
                case MessageType.error:
                    Title = "Error!";
                    break;
                case MessageType.warning:
                    Title = "Advertencia";
                    break;
                case MessageType.success:
                    Title = "Éxito";
                    break;             
                default:
                    break;
            }
        }

    }
}
