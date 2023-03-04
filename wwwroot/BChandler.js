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
        const button = document.createElement("button");
        button.classList = "btn btn-primary";
       // button.style.cssText = "float:right";
        button.onclick = function () { toast.remove(); };
        button.textContent = buttonText;
        notif.appendChild(button);
    } else {
        setTimeout(() => {
            toast.remove();
        }, time);
    }
    toast.appendChild(notif);
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

