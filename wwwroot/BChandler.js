// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}

export function CreateToast(message, style, classes, time, buttonText) {
    var toast = document.createElement('div');
    toast.id = "toasts";
    toast.style.cssText = "position: fixed; bottom: 20px; display: flex; flex - direction: column; align - items: flex - end;width:100% "

    const notif = document.createElement("div");
    notif.style.cssText = style;
    notif.classList = classes;
    notif.innerHTML = message;
    if (buttonText != "") {
        const button = document.createElement("span");
        button.classList = "btn btn-primary";
        button.style.cssText = "float:right";
        button.onclick = function () { toast.remove(); };
        button.textContent = "x";
        notif.appendChild(button);
    } else {
        setTimeout(() => {
            toast.remove();
        }, time);
    }
    toast.appendChild(notif);
    document.body.appendChild(toast);
}

export function CreateToastAds(message, style, classes, url) {
    var toast = document.createElement('div');
    toast.id = "toasts";
    toast.style.cssText = "position: fixed; bottom: 20px; display: flex; flex - direction: column; align - items: flex - end;width:100% "
        
   

    const notif = document.createElement("div");
    notif.style.cssText = style;
    notif.classList = classes;
    const sp = document.createElement("span");
    sp.innerHTML = message;
    sp.style.cssText = "cursor:pointer";
    sp.onclick = function () { window.open(url, '_blank'); toast.remove(); };
    notif.appendChild(sp);

    const button = document.createElement("span");
    button.classList = "btn btn-default";
    button.style.cssText = "float:right";
    button.onclick = function () { toast.remove(); };
    button.textContent = "x";
    notif.appendChild(button);

    setTimeout(() => {
        toast.remove();
    }, 15000);

    
    toast.appendChild(notif);
   // toast.appendChild(button);
    document.body.appendChild(toast);
}


export function SetTextbyId(id, text) {
    var e = document.getElementById(id); return e.innerText = text;
}

export function InvokeClick(id) {
    var elem = document.getElementById(id); elem.click();
}

export function RemoveClass(id, className) {
    var element = document.getElementById(id); element.classList.remove(className);
}

export function AddClass(id, className) {
    var element = document.getElementById(id);
    element.classList.add(className);
}

export function GetTextbyId(id) {
    var e = document.getElementById(id); return e.value;
}

export function SetSessionStorage(key, object) {
    sessionStorage.setItem(key, object);
    var obj = sessionStorage.key;
}

export function GetSessionStorage(key) {
    return sessionStorage.getItem(key);
}

export function RemoveSessionStorage(key) {
    return sessionStorage.removeItem(key);
}
export function ClearSessionStorage() {
    return sessionStorage.clear();
}

export function SetLocalStorage(key, object) {
    localStorage.setItem(key, object);
    var obj = localStorage.key;
}

export function GetLocalStorage(key) {
    return localStorage.getItem(key);
}

export function RemoveLocalStorage(key) {
    return localStorage.removeItem(key);
}

export function ClearLocalStorage() {
    return localStorage.clear();
}

export function GetSelectedElement(id) {
    var e = document.getElementById(id);
    var aux = e.options[e.selectedIndex].text;
    return aux;
}

export function SelectElement(element) {
    element.selectedIndex = 0;
}

export function GetClasses(id) {
    var e = document.getElementById(id);
    return e.classList;
}

export function CopyClipboard(id) {    
    var copyText = document.getElementById(id);    
    copyText.select();    
    document.execCommand("copy");   
}


export function CopyStringToClipboard(text) {
    var p = document.createElement('textarea');
    p.value = text;        
    document.body.appendChild(p);
    p.select();
    document.execCommand("copy");
    p.remove();
}

export async function DownloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

export function IsDevice() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}