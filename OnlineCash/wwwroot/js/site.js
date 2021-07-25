// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toFloat(str) {
    if (str == "" || isNaN(parseFloat(str)))
        return 0;
    else
        return parseFloat(str);
}

function toInteger(str) {
    if (str == "" || isNaN(parseInt(str)))
        return 0;
    else
        return parseInt(str);
}

function unitToStr(unit) {
    let str = "";
    if (isNaN(parseInt(unit)))
        switch (unit) {
            case "PCE":
                str = "шт";
                break;
            case "Litr":
                str = "л";
                break;
            case "KG":
                str = "кг";
                break;
        }
    return str;
}