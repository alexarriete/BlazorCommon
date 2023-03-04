
using BlazorCommon.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;


namespace BlazorCommon
{
    public class HtmlComponentBase : LayoutComponentBase
    {        
        [Inject] public IJSRuntime jSRuntime { get; set; }
        [Inject] public NavigationManager UrlHelper { get; set; }
        public ModalCommon ModalCommon { get; set; }

        private bool aceptDeclineResult;
        protected bool AceptDeclineResult { get { return aceptDeclineResult; } set { aceptDeclineResult = value; AceptDeclineResultChanged(); } }


        public virtual bool AceptDeclineResultChanged()
        {
            StateHasChanged();
            return aceptDeclineResult;
        }

        public virtual void ModalResultChanged(bool outResult)
        {
            StateHasChanged();            
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        /// <summary>
        /// Show message. Bottom. 
        /// </summary>
        /// <param name="messageType">error, success, warning, info</param>
        /// <param name="text">mesage text</param>
        /// <param name="showButtonClose">If false message will desapear in a few seconds, if true message will close by user action</param>
        /// <returns></returns>
        public async Task SetToast(MessageType messageType, string text, bool showButtonClose = false)
        {
            JsHelper jsHelper = new(jSRuntime);
            await jsHelper.SetToast(messageType, text, showButtonClose);
        }

        /// <summary>
        /// Show message. Bottom. 
        /// </summary>        
        /// <param name="text">mesage text</param>
        /// <param name="url">A click into the component will redirect to this url (target=_blank)</param>
        /// <returns></returns>
        public async Task SetToastAds(string text, string url)
        {
            JsHelper jsHelper = new(jSRuntime);
            await jsHelper.SetToast(MessageType.ads, text, false, url);
        }

        /// <summary>
        /// Get element inner text by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetTextById(string id)
        {
            JsHelper jsHelper = new(jSRuntime);
            return await jsHelper.JsGetTextById(id);
        }

        public async Task<string> SetTextById(string id, string text) { JsHelper jsHelper = new(jSRuntime); return await jsHelper.JsSetTextById(id, text); }

        #region session and local storage
        public async Task SetSessionStorage(string key, object obj) { JsHelper jsHelper = new(jSRuntime); await jsHelper.SetSessionStorage(key, obj); }
        public async Task<T> GetSessionStorage<T>(string key) { JsHelper jsHelper = new(jSRuntime); return await jsHelper.GetSessionStorage<T>(key); }
        public async Task RemoveSessionStorage(string key) { JsHelper jsHelper = new(jSRuntime); await jsHelper.RemoveSessionStorage(key); }
        public async Task ClearSessionStorage() { JsHelper jsHelper = new JsHelper(jSRuntime); await jsHelper.ClearSessionStorage(); }

        public async Task SetLocalStorage(string key, object obj) { JsHelper jsHelper = new(jSRuntime); await jsHelper.SetLocalStorage(key, obj); }
        public async Task<T> GetLocalStorage<T>(string key) { JsHelper jsHelper = new(jSRuntime); return await jsHelper.GetLocalStorage<T>(key); }
        public async Task RemoveLocalStorage(string key) { JsHelper jsHelper = new(jSRuntime); await jsHelper.RemoveLocalStorage(key); }
        public async Task ClearLocalStorage() { JsHelper jsHelper = new(jSRuntime); await jsHelper.ClearLocalStorage(); }
        #endregion

        #region urlredirect
        /// <summary>
        /// Force current page reload.
        /// </summary>
        /// <returns></returns>
        public async Task ForceReloadAsync() => await Task.Run(() => UrlHelper.NavigateTo(UrlHelper.Uri, true));

        /// <summary>
        /// Go to page
        /// </summary>
        /// <param name="url">New page url.</param>
        /// <returns></returns>
        public async Task<bool> GotoPageAsync(string url)
        {
            bool exists = Validator.RemoteFileExists(url);
            if (exists)
                await Task.Run(() => UrlHelper.NavigateTo(url, true));
            return exists;
        }

        /// <summary>
        /// Go to page blank
        /// </summary>
        /// <param name="jSRuntime"></param>
        /// <param name="url">New page url</param>
        /// <returns></returns>
        public async Task<bool> GotoPageBlankAsync(IJSRuntime jSRuntime, string url)
        {
            bool exists = Validator.RemoteFileExists(url);
            if (exists)
                await jSRuntime.InvokeVoidAsync("open", url, "_blank");
            return exists;
        }
        #endregion

        #region CSS Classes
        public async Task AddClassAsync(string className, string id) { JsHelper jsHelper = new(jSRuntime); await jsHelper.AddClassAsync(className, id); }
        public async Task RemoveClassAsync(string className, string id) { JsHelper jsHelper = new(jSRuntime); await jsHelper.RemoveClassAsync(className, id); }
        public async Task TogleClassAsync(string className, string id) { JsHelper jsHelper = new(jSRuntime); await jsHelper.TogleClassAsync(className, id); }
        public async Task<List<string>> GetClasses(string id) { JsHelper jsHelper = new(jSRuntime); return await jsHelper.GetClasses(id); }

        #endregion
    }
}

