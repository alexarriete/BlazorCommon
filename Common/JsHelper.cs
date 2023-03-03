using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorCommon;
using BlazorCommon.Modal;

using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace BlazorCommon
{
    public class JsHelper
    {
        private IJSRuntime JSRuntime { get; set; }
        public JsHelper() { }
        public JsHelper(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public IJSObjectReference jSObject { get; set; }

        private async Task SetIJSObject()
        {
            if (jSObject == null)
            {
                try
                {
                    jSObject = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorCommon/BChandler.js");
                }
                catch (Exception ex)
                {
                    if (!ex.Message.ToLower().Contains("prerendering"))
                        throw;
                }
            }
        }

        /// <summary>
        /// Adds a class to an html element. One class each call.
        /// </summary>
        /// <param name="className">CSS class to add</param>
        /// <param name="selectedId">Element id</param>
        /// <returns></returns>
        public async Task AddClassAsync(string className, string selectedId)
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeAsync<object>("AddClass", selectedId, string.Join(" ", className));
        }
        /// <summary>
        /// Remove class from Html element. One class each call
        /// </summary>
        /// <param name="className">CSS class to remove</param>
        /// <param name="id">Element id</param>
        /// <returns></returns>
        public async Task RemoveClassAsync(string className, string id)
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeAsync<object>("RemoveClass", id, className);
        }
        public async Task<string> JsGetTextById(string id)
        {
            await SetIJSObject();
            if (jSObject != null)
                return (await jSObject.InvokeAsync<object>("GetTextbyId", id)).ToString();

            return null;
        }
        public async Task<string> JsSetTextById(string id, string text)
        {
            await SetIJSObject();
            if (jSObject != null)
                return (await jSObject.InvokeAsync<object>("SetTextbyId", id, text)).ToString();

            return null;
        }
        public async Task InvokeClick(string id)
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeAsync<object>("InvokeClick", id);
        }

        public async Task<string> GetSelectedElement(string id)
        {
            await SetIJSObject();
            if (jSObject != null)
                return (await jSObject.InvokeAsync<object>("GetSelectedElement", id)).ToString();

            return null;
        }

        public async Task SelectElement(ElementReference element)
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("SelectElement", element);
        }

        private async Task CreateToast(string text, string style, string classes, int time, bool showButtonClose)
        {
            await SetIJSObject();
            if (jSObject != null)
            {
                string buttonText = showButtonClose ? BlazorDic.Accept : "";
                await jSObject.InvokeVoidAsync("CreateToast", text, style, classes, time, buttonText);
            }
                
        }
        
        internal async Task SetToast(MessageType messageType, string text,bool showButtonClose)
        {
            int time = text.Length * 60;
            time = time < 3000 ? 3000 : time;
            int length = text.Length < 20 ? 300 : text.Length < 50 ? 450 : text.Length < 75 ? 700 : text.Length < 100 ? 900 : text.Length < 150 ? 1200 : 1500;
            string style = "";
            var classes = new List<string> { "alert", "alert-dismissable", "mt-4", "container" };
            switch (messageType)
            {
                case MessageType.error:
                    classes.Add("alert-danger");
                    style = $"width: {length}px;height: 70px;";                    
                    text = $"&#9940; {text}";
                    break;
                case MessageType.warning:
                    classes.Add("alert-warning");
                    style = $"width: {length}px;height: 70px;";                    
                    text = $"&#9889; {text}";
                    break;
                case MessageType.success:
                    classes.Add("alert-success");
                    style = $"width: {length}px;height: 70px;";                    
                    text = $"&#9989; {text}";
                    break;
                case MessageType.info:
                    classes.Add("alert-info");
                    style = $"width: {length}px;height: 70px;";
                    text = $"&#9200; {text}";
                    break;
            }
            await CreateToast(text, style, string.Join(" ",classes), time, showButtonClose);
        }

       
        public async Task SetSessionStorage(string key, object obj)
        {
            await SetIJSObject();
            var json = JsonSerializer.Serialize(obj);
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("SetSessionStorage", key, json);
        }

        public async Task<T> GetSessionStorage<T>(string key)
        {
            await SetIJSObject();
            if (jSObject != null)
            {
                string obj = (await jSObject.InvokeAsync<object>("GetSessionStorage", key))?.ToString();
                if (obj != null)
                    return JsonSerializer.Deserialize<T>(obj);
            }

            return default(T);
        }

        public async Task RemoveSessionStorage(string key)
        {
            await SetIJSObject();            
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("RemoveSessionStorage", key);
        }
        public async Task ClearSessionStorage()
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("ClearSessionStorage");
        }


        public async Task SetLocalStorage(string key, object obj)
        {
            await SetIJSObject();
            var json = JsonSerializer.Serialize(obj);
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("SetLocalStorage", key, json);
        }

        public async Task<T> GetLocalStorage<T>(string key)
        {
            await SetIJSObject();
            if (jSObject != null)
            {
                string obj = (await jSObject.InvokeAsync<object>("GetLocalStorage", key))?.ToString();
                if (obj != null)
                    return JsonSerializer.Deserialize<T>(obj);
            }

            return default(T);
        }

        public async Task RemoveLocalStorage(string key)
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("RemoveLocalStorage", key);
        }
        public async Task ClearLocalStorage()
        {
            await SetIJSObject();
            if (jSObject != null)
                await jSObject.InvokeVoidAsync("ClearLocalStorage");
        }






        public async ValueTask<bool> ConfirmAsync(string message)
        {
            await SetIJSObject();
            return await jSObject.InvokeAsync<bool>("confirm", message);
        }

        public async ValueTask<bool> AlertAsync(string message)
        {
            await SetIJSObject();
            return await jSObject.InvokeAsync<bool>("alert", message);
        }



        public async ValueTask<string> DownloadExcelAsync(byte[] bytes, string fileName)
        {
            try
            {
                await SetIJSObject();
                fileName = fileName.Contains(".xlsx") ? fileName : $"{fileName}.xlsx";

                await jSObject.InvokeVoidAsync("downloadFromByteArray"
                    , new { ByteArray = bytes, FileName = fileName, ContentType = "application /x-msdownload" });

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


    }
}
