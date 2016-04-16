
var ALERT_TITLE = "Sukces!";
var ALERT_BUTTON_TEXT = "Ok";
var SUCCESS = true;

function ErrorAlert()
{
    ALERT_TITLE = "Błąd!";
    SUCCESS = false;
}

if (document.getElementById) {
    window.alert = function (txt) {
        createCustomAlert(txt);
    }
}
function createCustomAlert(txt) {
    d = document;
    if (d.getElementById("modalContainer")) return;

    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "modalContainer";
    mObj.style.height = d.documentElement.scrollHeight + "px";

    alertObj = mObj.appendChild(d.createElement("div"));
    alertObj.id = "alertBox";
    if (d.all && !window.opera) alertObj.style.top = document.documentElement.scrollTop + "px";
    alertObj.style.left = (d.documentElement.scrollWidth - alertObj.offsetWidth) / 2 + "px";
    alertObj.style.visiblity = "visible";

    h1 = alertObj.appendChild(d.createElement("h1"));
    if (!SUCCESS) {
        h1.style.backgroundColor = "red"
    }
    h1.appendChild(d.createTextNode(ALERT_TITLE));
    ALERT_TITLE = "Sukces!";
    SUCCESS = true;
    msg = alertObj.appendChild(d.createElement("p"));
    //msg.appendChild(d.createTextNode(txt));
    msg.innerHTML = txt;

    btn = alertObj.appendChild(d.createElement("a"));
    btn.id = "closeBtn";
    btn.appendChild(d.createTextNode(ALERT_BUTTON_TEXT));
    btn.href = "#";
    btn.focus();    
    btn.onclick = function () {
        removeCustomAlert();
        return false;
    }

    alertObj.style.display = "block";

}

function removeCustomAlert() {
    document.getElementsByTagName("body")[0].removeChild(document.getElementById("modalContainer"));
}

function ful() {
    alert('Alert this page');
}