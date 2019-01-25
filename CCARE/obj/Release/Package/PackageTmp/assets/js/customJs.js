//Popup windows properties
var winProp = "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,width=700,height=500,left=400,top=100";

//Alert windows properties
var alertProp = "toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,dialogHeight:250; center:yes ,resizable=no";

// Pop up Windows for Broadcast Message
var broadcast = false;

function OpenBBP(url) {
    $('#blockbbp').customFadeIn();
    $('#iframebbp').attr('src', url);
    $('#containerbbp').customFadeIn();
}

function CloseBBP() {
    $('#blockbbp').customFadeOut();
    $('#containerbbp').customFadeOut();
}

function UnescapeHtml(unsafe) {
    return unsafe
         .replace(new RegExp("&amp;", "g"), "&")
         .replace(new RegExp("&lt;", "g"), "<")
         .replace(new RegExp("&gt;", "g"), ">")
         .replace(new RegExp("&#47;", "g"), "/")
         .replace(new RegExp("&quot;", "g"), '"')
         .replace(new RegExp("&#39;", "g"), "'")
         .replace(new RegExp("&#039;", "g"), "'");

}

function CopyClipboard(){
    TextToLabel();
    // creating new textarea element and giveing it id 't'
    var t = document.createElement('textarea');
    t.id = 't';
    // Optional step to make less noise in the page, if any!
    t.style.height = 0;
    // You have to append it to your page somewhere, I chose <body>
    document.querySelector('#detail-info').appendChild(t);
    // Copy whatever is in your div to our new textarea
    t.value = document.getElementById('detail-info').innerText;
    // Now copy whatever inside the textarea to clipboard
    var selector = document.querySelector('#t');
    selector.select();
    document.execCommand('copy');
	
    LabelToText();
    // Remove the textarea
    document.getElementById('detail-info').removeChild(t);
}

function IsEmptyOrSpaces(str) {
    return str === null || str.match(/^\s*$/) !== null;
}

function TextToLabel() {
    $('#detail-info').find('.form-control').replaceWith(function () {
        return '<span class=' + this.className + '>' + this.value + '</span>'
    });
}

function LabelToText() {
    $('#detail-info').find('.form-control').replaceWith(function () {
        return "<input type='text' class='" + this.className + "' value='" + this.innerText + "' disabled='disabled' />";
    });
}

(function ($) {
    $.fn.customFadeIn = function (speed, callback) {
        $(this).fadeIn(speed, function () {
            if (jQuery.browser.msie)
                $(this).get(0).style.removeAttribute('filter');
            if (callback != undefined)
                callback();
        });
    };
    $.fn.customFadeOut = function (speed, callback) {
        $(this).fadeOut(speed, function () {
            if (jQuery.browser.msie)
                $(this).get(0).style.removeAttribute('filter');
            if (callback != undefined)
                callback();
        });
    };
})(jQuery);


function ddmmyyyy_yyyymmdd(d) {
    if (d == "") {
        return "";
    }
    else {
        var dd = d.split("/");
        return dd[2] + "-" + dd[1] + "-" + dd[0];
    }
}

function CheckRangeDate(startDate, endDate) {
    var s = startDate.split("/");
    var e = endDate.split("/");
    var ss = s[1] + '/' + s[0] + '/' + s[2];
    var ee = e[1] + '/' + e[0] + '/' + e[2];
    if (Date.parse(ss) > Date.parse(ee)) {
        return true;
    } else {
        return false;
    }
}

function LimitRangeDate(count, startDate, endDate) {
    if (endDate == "") {
        return true;
    } else {
        var s = startDate.split("/");
        var e = endDate.split("/");

        var SDate = new Date(s[2], (s[1] - 1), s[0]);
        var EDate = new Date(e[2], (e[1] - 1), e[0]);
        var LDate = 1000 * 60 * 60 * 24 * count;
        if ((EDate - SDate) <= LDate) {
            return true;
        } else {
            return false;
        }
    }   
}

function JsonDate(jsonDate) {
    var re = /-?\d+/;
    var m = re.exec(jsonDate);
    var today = new Date(parseInt(m[0]));

    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = dd + '/' + mm + '/' + yyyy;

    return today;
}


function BroadcastMessage(url) {
    $.ajax({
        type: "GET",
        url: url,
        cache: false,
        beforeSend: function (jqXHR, settings) {
        },
        success: function (data, textStatus, jqXHR) {
            $("#broadcast-modal").html(data);
            script = $(this).text();
            $.globalEval(script);
        }
    })
}

function BroadcastMessageWindow(url) {
    openWindowBroadcast(url, "Broadcast Messages", 625, 575);
}

function openWindowBroadcast(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var winFeature = [
        'toolbar=no',
        'location=no',
        'directories=no',
        'status=no',
        'menubar=no',
        'scrollbars=no',
        'resizable=yes',
        'copyhistory=no',
        'width=' + w,
        'height=' + h,
        'top=' + top,
        'left=' + left
    ].join(',');

    /* NEW CODE, Pop Up Only Once */
    /* Checks to see if window is open */
    /* If get Error, Please Comment this Code */
    /* And uncomment OLD CODE */
    if (broadcast && !broadcast.closed) {
        broadcast.close();
    }
    broadcast = window.open(url, "BM", winFeature);

    /* OLD CODE, Always Created Pop Up When Broadcast Message is Coming */
    // return window.open(url, "", winFeature);
}

// Created by DRS
function DateDDMMYYYY(date) {
    var parts = date.split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

function NoRecordFound() {
    var error = '';
    error += "<div class='alert alert-danger alert-dismissible fade in' role='alert'>";
    error += "    <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button>";
    error += "    <img src='/assets/images/error/err_48_4.gif' height='16'/>";
    error += "    &nbsp;";
    error += "    <strong>No Record Found</strong>";
    error += "</div>";
    return error;
}


// Created by Engga
// Date Formatter for Organization Closure

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement /*, fromIndex */) {
        'use strict';
        if (this == null) {
            throw new TypeError();
        }
        var n, k, t = Object(this),
            len = t.length >>> 0;

        if (len === 0) {
            return -1;
        }
        n = 0;
        if (arguments.length > 1) {
            n = Number(arguments[1]);
            if (n != n) { // shortcut for verifying if it's NaN
                n = 0;
            } else if (n != 0 && n != Infinity && n != -Infinity) {
                n = (n > 0 || -1) * Math.floor(Math.abs(n));
            }
        }
        if (n >= len) {
            return -1;
        }
        for (k = n >= 0 ? n : Math.max(len - Math.abs(n), 0) ; k < len; k++) {
            if (k in t && t[k] === searchElement) {
                return k;
            }
        }
        return -1;
    };
}

function stringToDate(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

function js_yyyy_mm_dd_hh_mm_ss(xdate) {
    now = new Date(xdate);
    year = "" + now.getFullYear();
    month = "" + (now.getMonth() + 1); if (month.length == 1) { month = "0" + month; }
    day = "" + now.getDate(); if (day.length == 1) { day = "0" + day; }
    hour = "" + now.getHours(); if (hour.length == 1) { hour = "0" + hour; }
    minute = "" + now.getMinutes(); if (minute.length == 1) { minute = "0" + minute; }
    second = "" + now.getSeconds(); if (second.length == 1) { second = "0" + second; }
    return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
}

//Created by Sumardi
//Submit form Action
function formSubmit(type, formName, gridId) {
    disabledId($("#btn_save"));
    var action = formName.attr("action");
    var serializedForm = formName.serialize();
    //alert(action);
    $.ajax({
        type: "POST",
        url: action,
        data: serializedForm,
        dataType: "json",
        success: function (data) {
            if (data.flag == true) {

                triggerReloadGrid(); /*Added By Ardi For Auto Reload Grid After Submission (20151207)*/

                if (type == "save") {
                    window.location.replace(data.Message);
                }
                else if (type == "saveNClose") {
                    window.close();
                }
                else if (type == "saveNOpen") {
                    window.location.replace(data.urlNew);
                }
                else if (type == "saveNOpenMapping") {
                    window.location.replace(data.urlNew + "?_category=" + $("#_category").val());
                }
            }
            else {
                alert(data.Message);
                //setTimeout(function () { $(".divFormMessage").text(data.Message); }, 1000);
                $(".divFormMessage").text(data.Message);
            }
        },
        error: function (xhr, status, err) {
            alert("Action Failed");
            //alert(xhr);
        }
    });
    return false;
}

function SaveRequest(formName) {
    var regexNumber = /^\d+$/;

    if ($("#CustomerID").val() == "") {
        if ($("#CustomerName").val() == "") {
            alert("@Resources.Request.RequestMandatoryCustomer");
            $("#CustomerName").focus();
        }
        else
            alert("@Resources.Request.RequestMandatoryCustomer2");
        return false;
    }

    if ($("#Location").val() == "") {
        alert("@Resources.Request.RequestMandatoryAddress");
        $("#Location").focus();
        return false;
    }
    if ($("#Location").val() == "200001") {
        if ($("#Address1").val() == "") {
            alert("@Resources.Request.RequestMandatoryAddress1");
            $("#Address1").focus();
            return false;
        }
        if ($("#City").val() == "") {
            alert("@Resources.Request.RequestMandatoryCity");
            $("#City").focus();
            return false;
        }
        if ($("#Address2").val() == "") {
            alert("@Resources.Request.RequestMandatoryAddress2");
            $("#Address2").focus();
            return false;
        }
        if ($("#PostalCode").val() == "") {
            alert("@Resources.Request.RequestMandatoryZip");
            $("#PostalCode").focus();
            return false;
        }
        if (regexNumber.test($("#PostalCode").val()) == false || $("#PostalCode").val().length != 5) {
            alert("@Resources.Request.RequestZipLength");
            $("#PostalCode").focus();
            return false;
        }
    }

    if (regexNumber.test($("input[name=CommunicationPhone]").val()) == false || $("#input[name=CommunicationPhone]").length < 0) {
        alert("@Resources.Request.RequestNumericCommunicationPhoneNumber");
        $("input[name=CommunicationPhone]").focus();
        return false;
    }
    if ($("#CategoryID").val() == "") {
        if ($("#CategoryName").val() == "") {
            alert("@Resources.Request.RequestMandatoryCategory");
            $("#CategoryID").focus();
        }
        else
            alert("@Resources.Request.RequestMandatoryCategory2");
        return false;
    }
    else if ($("#ProductID").val() == "") {
        if ($("#ProductName").val() == "") {
            alert("@Resources.Request.RequestMandatoryProduct");
            $("#ProductID").focus();
        }
        else
            alert("@Resources.Request.RequestMandatoryProduct2");
        return false;
    }
    if ($("#Title").val() == "") {
        alert("@Resources.Request.RequestMandatorySummary");
        $("#Title").focus();
        return false;
    }
    if ($("#CaseOriginCode").val() == "") {
        alert("@Resources.Request.RequestMandatoryChannel");
        $("#CaseOriginCode").focus();
        return false;
    }
    if ($("#OwnerName").val() == "") {
        alert("@Resources.Request.RequestMandatoryOwner");
        $("#OwnerName").focus();
        return false;
    }

    if (regexNumber.test($("input[name=InteractionCount]").val()) == false && $("#input[name=InteractionCount]").length > 0) {
        alert("@Resources.Request.RequestInteractionCountNumber");
        $("input[name=InteractionCount]").focus();
        return false;
    }
    if (regexNumber.test($("input[name=IncomingPhoneNumber]").val()) == false && $("#input[name=IncomingPhoneNumber]").length > 0) {
        alert("@Resources.Request.RequestNumericIncomingPhoneNumber");
        $("input[name=IncomingPhoneNumber]").focus();
        return false;
    }

    if (isNaN($("#TransactionTimeHour").val()))
        $("#TransactionTimeHour").val("");

    if (isNaN($("#TransactionTimeMin").val()))
        $("#TransactionTimeMin").val("");

    if (isNaN($("#TransactionTimeSec").val()))
        $("#TransactionTimeSec").val("");

    if ($("#RefTicketNumber").val() == "" && ($("#RefRequest").val() != "" || $("#RefRequest-label").val() != "")) {
        alert("@Resources.Request.RefRequest");
        return false;
    }
    if ($("#WsIdID").val() == "" && ($("#WsIdName").val() != "" || $("#WsIdName-label").val() != "")) {
        alert("@Resources.Request.WSID");
        return false;
    }
    if ($("#CauseID").val() == "" && ($("#CauseName").val() != "" || $("#CauseName-label").val() != "")) {
        alert("@Resources.Request.Cause");
        return false;
    }
    if ($("#BranchID").val() == "" && ($("#BranchName").val() != "" || $("#BranchName-label").val() != "")) {
        alert("@Resources.Request.Branch");
        return false;
    }

    var decimal = moneyToDecimal($("#TransactionAmount").val());
    $("#TransactionAmount").val(decimal);

    disabledId($("#btn_save"));
    var action = formName.attr("action");
    var serializedForm = formName.serialize();
    $.ajax({
        type: "POST",
        url: action,
        data: serializedForm,
        dataType: "json",
        success: function (data) {
        },
        error: function (xhr, status, err) {
            alert("Action Failed");
        }
    });
    return false;
}

function reloadGrid(gridId) { /*Added by Ardi For Auto Reload Grid After Submission (20151103)*/
    var last = jQuery(gridId).getGridParam("page");
    jQuery(gridId).setGridParam({ page: last });
    jQuery(gridId).trigger("reloadGrid");
}

function triggerReloadGrid() { /*Added By Ardi For Auto Reload Grid After Submission (20151207)*/
    if (window.opener) {
        window.opener.refreshGrid();
    }
}

function otherAction(formName, action, type, postData) {
    var action = action;
    var serializedForm = formName.serialize();
    $.ajax({
        type: "POST",
        url: action,
        data: serializedForm + "&Resolution=" + postData._resolution + "&Description=" + postData._description,
        dataType: "json",
        success: function (data) {
            if (data.flag == true) {

                triggerReloadGrid(); /*Added By Ardi For Auto Reload Grid After Submission (20151207)*/

                if (type == "changestatus") {
                    window.location.replace(data.Message);
                }
                else if (type == "delete") {
                    window.close();
                }
            }
            else {
                if (data.resultsKey == 3)
                    alert("Failed to set status, this Request still has an active Task");
                $(".divFormMessage").text(data.Message);
            }
        },
        error: function (xhr, status, err) {
            alert("Error");
        }
    });
    return false;
}

//Created by Sumardi
//set control attribute to readonly
function setReadOnly(e) {
    e.attr('readonly', true);
    e.addClass("readOnlyText");
}

function removeReadOnly(e) {
    e.removeAttr('readonly', true);
    e.removeClass("readOnlyText");
}

//Created by Sumardi
//disabled
function disabledId(e) {
    $(e).attr("disabled", "true");
    e.addClass("readOnlyText");
}

//Enable
function removeDisabledId(e) {
    $(e).removeAttr("disabled");
    e.removeClass("readOnlyText");
}

//Created by Sumardi
//Check valid email format
function isValidEmail(email) {
    var pattern = new RegExp("[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\\.[a-zA-Z]{2,4}");
    if (email != '') {
        if (!pattern.test(email)) {
            return false;
        }
        else { return true; }
    }
}

//Created by Sumardi
//Check valid date format
function isDate(dateTime) {
    var datePattern = new RegExp("^(?:(?:31(\\/|-|\\.)(?:0?[13578]|1[02]))\\1|(?:(?:29|30)(\\/|-|\\.)(?:0?[1,3-9]|1[0-2])\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:29(\\/|-|\\.)0?2\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\\d|2[0-8])(\\/|-|\\.)(?:(?:0?[1-9])|(?:1[0-2]))\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$");
    if (!datePattern.test(dateTime)) {
        return false;
    }
    else { return true; }
}

//Created by Giovanni
//Delete every char except [0123456789/]
function isValidDate(input, target) {

    var txt = input;
    var found = false;
    var validChars = "0123456789/"; //List of valid characters

    for (j = 0; j < txt.length; j++) { //Will look through the value of text
        var c = txt.charAt(j);
        found = false;
        for (x = 0; x < validChars.length; x++) {
            if (c == validChars.charAt(x)) {
                found = true;
                break;
            }
        }
        if (!found) {
            //If invalid character is found remove it and return the valid character(s).
            $(target).val(input.substring(0, input.length - 1));
            break;
        }
    }
}

//Created by Giovanni
//Delete every char except number
function isNumber(input, target) {
    var txt = input;
    var found = false;
    var validChars = "0123456789"; //List of valid characters

    for (j = 0; j < txt.length; j++) { //Will look through the value of text
        var c = txt.charAt(j);
        found = false;
        for (x = 0; x < validChars.length; x++) {
            if (c == validChars.charAt(x)) {
                found = true;
                break;
            }
        }
        if (!found) {
            //If invalid character is found remove it and return the valid character(s).
            $(target).val(input.substring(0, input.length - 1));
            break;
        }
    }
}

//Created by Giovanni
//Delete non money format
function isMoney(input, target) {
    var txt = input;
    var found = false;
    var validChars = "0123456789.,"; //List of valid characters

    for (j = 0; j < txt.length; j++) { //Will look through the value of text
        var c = txt.charAt(j);
        found = false;
        for (x = 0; x < validChars.length; x++) {
            if (c == validChars.charAt(x)) {
                found = true;
                break;
            }
        }
        if (!found) {
            //If invalid character is found remove it and return the valid character(s).
            $(target).val(input.substring(0, input.length - 1));
            break;
        }
    }
}

//created by Giovanni
//Delete every char outside the given list of chars
function isValidChar(format, input, target) {
    var txt = input;
    var found = false;
    var validChars = format; //List of valid characters

    for (j = 0; j < txt.length; j++) { //Will look through the value of text
        var c = txt.charAt(j);
        found = false;
        for (x = 0; x < validChars.length; x++) {
            if (c == validChars.charAt(x)) {
                found = true;
                break;
            }
        }
        if (!found) {
            //If invalid character is found remove it and return the valid character(s).
            $(target).val(input.substring(0, input.length - 1));
            break;
        }
    }
}

//created by Giovanni
function IsEmail(emailAddress) {
    var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
}

//Created by Giovanni
//textbox placeholder
function shadowFormat(action, target, stringShadow, condition) {
    if (action == "apply") {
        if (condition == "conditional") {
            if ($(target).val() == "") {
                $(target).val(stringShadow)
                $(target).css("color", "grey");
            }
        }
        else {
            $(target).val(stringShadow)
            $(target).css("color", "grey");
        }
    }
    else if (action == "remove") {
        if ($(target).val() == stringShadow) {
            $(target).val("");
            $(target).css("color", "black");
        }
    }
}

//Created by Giovanni
//ubah format decimal jadi uang
function decimalToMoney(amount) {
    var firstDigits, temp, result = "";

    if (amount.indexOf(".") >= 0) {
        temp = amount.substring(0, amount.indexOf("."));
        firstDigits = temp.length % 3;
        if (firstDigits == 0) firstDigits = 3;
        for (var i = 0; i < temp.length; i++) {
            if (i == firstDigits) result += ".";
            else if (i >= firstDigits)
                if ((i - firstDigits) % 3 == 0) result += ".";
            result += temp.charAt(i);
        }
        result += ",00";
        return result;
    }
    else if (amount != "") {
        temp = amount;
        firstDigits = temp.length % 3;
        if (firstDigits == 0) firstDigits = 3;
        for (var i = 0; i < temp.length; i++) {
            if (i == firstDigits) result += ".";
            else if (i >= firstDigits)
                if ((i - firstDigits) % 3 == 0) result += ".";
            result += temp.charAt(i);
        }
        result += ",00";
        return result;
    }
    else {
        return "";
    }
}

function LimitCardFormat(amount) {
    if (amount == 0)
        return "-";
    return decimalToMoney(amount).split(',')[0];
}

//Created by Giovanni
//ubah format uang jadi decimal
function moneyToDecimal(amount) {
    if (amount.indexOf(",") >= 0) {
        var temp = amount.substring(0, amount.indexOf(","));
        while (temp.indexOf(".") >= 0)
            temp = temp.replace('.', '');
        var result = temp;

        return result;
    }
    else
        return amount;
}

//Created by Sumardi
//Get query string value
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

//Created by Sumardi
//Check Active session
function checkSessionEnd() {
    $.ajax({
        type: "POST",
        url: "/Login/isActiveSession",
        dataType: "json",
        success: function (data) {
            if (data.flag == false) {
                location.reload();
                return false;
            }
        }
    });
}

//Created by Giovanni
//Reload body
function ReloadBody(target) {
    $.ajax({
        url: target,
        type: 'GET',
        cache: false,
        success: function (result) {
            $("#content").html(result);
        }
    });
    return false;
}

//Created by Giovanni
//Set max length search by channel
function setSearchChannelMaxLength(parameterType) {
    if (parameterType == "Handphone No") {
        $("#tEbankingParamVal").attr('maxLength', '12');
    }
    else if (parameterType == "S/N Key BCA") {
        $("#tEbankingParamVal").attr('maxLength', '10');
    }
    else if (parameterType == "Card No") {
        $("#tEbankingParamVal").attr('maxLength', '16');
    }
    else if (parameterType == "Account No") {
        $("#tEbankingParamVal").attr('maxLength', '11');
    }
    else if (parameterType == "Corporate ID") {
        $("#tEbankingParamVal").attr('maxLength', '10');
    }
    else if (parameterType == "E-Mail") {
        $("#tEbankingParamVal").attr('maxLength', '40');
    }
    else {
        $("#tEbankingParamVal").attr('maxLength', '100');
    }
}


// By DRS
function convertToRupiah(angka) {
    var rupiah = '';
    angka = angka + "";
    var angkarev = angka.split('').reverse().join('');
    for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
    return "Rp " + rupiah.split('', rupiah.length - 1).reverse().join('') + ",00";
}

function convertToNumeric(angka) {
    var rupiah = '';
    angka = angka + "";
    var angkarev = angka.split('').reverse().join('');
    for (var i = 0; i < angkarev.length; i++) if (i % 3 == 0) rupiah += angkarev.substr(i, 3) + '.';
    return rupiah.split('', rupiah.length - 1).reverse().join('') + ",00";
}

function convertCCTRXdate(tanggal){
    var Tanggal = tanggal.split('T')[0].split('-');
    tanggal = Tanggal[2] + "/" + Tanggal[1] + "/" + Tanggal[0];
    return tanggal;
}

function ddMMMyyyy(tanggal) {
    
    var pattern = /(.*?)\/(.*?)\/(.*?)$/;
    var result = tanggal.replace(pattern, function (match, p1, p2, p3) {
        var months = ['Jan', 'Feb', 'Mar', 'Apr', 'Mei', 'Jun', 'Jul', 'Agust', 'Sep', 'Okt', 'Nop', 'Des'];
        //return (p1 < 10 ? "0" + p1 : p1) + " " + months[(p2 - 1)] + " " + p3;
        return p1 + " " + months[(p2 - 1)] + " " + p3;
    });
    return result;
}

function numFormat(cellvalue, options, rowObject) {
    return cellvalue.replace(".",",");
}

function numUnformat(cellvalue, options, rowObject) {
    return cellvalue.replace(",", ".");
}



//Open new window
function openWindow(url) {
    var winFeature = [
        'height=' + (screen.height - 74),
        'width=' + (screen.width - 12),
        'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=yes,left=0px,top=0px' // only works in IE, but here for completeness
    ].join(',');

    var win = window.open(url, "", winFeature);
    win.moveTo(0, 0);
    win.focus();
    return false;
}

function popupCenter(url, title, w, h) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}

//Refresh the same popup windows when there are others windows with the same name
function openOneWindow(url,name) {
    var winFeature = [
        'height=' + (screen.height - 74),
        'width=' + (screen.width - 12),
        'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=yes,left=0px,top=0px' // only works in IE, but here for completeness
    ].join(',');

    var win = window.open(url, name, winFeature);
    win.moveTo(0, 0);
    win.focus();
    return false;
}

/*Added by Ardi, for Print Ticket (20151104)*/
function openPrintWindow(url) {
    var winFeature = [
        'height=' + (screen.height - 74),
        'width=' + (screen.width - 12),
        'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=no,left=0px,top=0px' // only works in IE, but here for completeness
    ].join(',');

    var win = window.open(url, "", winFeature);
    win.moveTo(0, 0);
    return false;
}


//Modif jqgrid
//Entityname begin with uppercase
function getColModels(entityName) {
    var col;
    switch (entityName) {
        case 'Request':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Request ID', name: 'requestId', index: 'requestId', width: 200 },
                { label: 'Product', name: 'productIdName', width: 250 },
                { label: 'Category', name: 'categoryIdName', width: 250 },
                { label: 'Status', name: 'status', width: 200 },
                { label: 'Created On', name: 'createdOn', width: 250, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } },
                { label: 'Due Date', name: 'dueDate', width: 250, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }
            ];
            break;
        case 'Requesta':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Request Id', name: 'requestId', width: 250 },
              //  { label: 'Customer Name', name: 'CustomerName', width: 250 },
                { label: 'CIS Number', name: 'CISNumber', width: 250 },
                { label: 'Product', name: 'productIdName', width: 200 },
                { label: 'Category', name: 'categoryIdName', width: 200 },
                { label: 'Status', name: 'Status', width: 200 },
                { label: 'Created On', name: 'CreatedOn', width: 250, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }
            ];
            break;
        case 'RequestCreationMatrix':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'eBankingKey', name: 'eBankingKey', index: 'eBankingKey', width: 160, hidden: true },
                { label: 'E-Banking & Products', name: 'eBanking', index: 'eBanking', width: 160 },
                { label: 'Status Type', name: 'StatusType', index: 'StatusType', width: 160 },
                { label: 'Previous Status Value', name: 'PrevStatus', index: 'PrevStatus', width: 140 },
                { label: 'New Status Value', name: 'NewStatus', index: 'NewStatus', width: 120 },
                { label: 'Category', name: 'CategoryName', index: 'CategoryName', width: 200 },
                { label: 'Product', name: 'ProductName', index: 'ProductName', width: 200 },
                { label: 'Request Status', name: 'RequestStatus', index: 'RequestStatus', width: 100 },
                { label: 'Summary', name: 'Summary', index: 'Summary', width: 500 }
            ];
            break;

        case 'Category':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Category Name', name: 'Name', width: 300 },
                { label: 'Parent Category', name: 'ParentName', width: 300 },
                { label: 'Description', name: 'Description', width: 400 },
            ];
            break;

        case 'Product':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Product Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Parent Product', name: 'ParentName', width: 300 },
                { label: 'Description Tree', name: 'Description', width: 400 },
            ];
            break;
        case 'BusinessUnit':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'name', width: 300 },
                { label: 'Main Phone', name: 'mainPhone', width: 250 },
                { label: 'Web Site', name: 'webSite', width: 250 },
                { label: 'Parent Business', name: 'parentBusiness', width: 300 }
            ];
            break;

        case 'ServiceLevel':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Category', name: 'CategoryName', width: 300 },
                { label: 'Product', name: 'ProductName', width: 300 },
                { label: 'SLA', name: 'SLADays', width: 75 },
                { label: 'Workgroup', name: 'WorkgroupName', width: 200 },
                { label: 'ParentID', name: 'ParentID', width: 10, hidden: true },
                { label: 'SegmentationID', name: 'SegmentationID', width: 10, hidden: true },
                { label: 'Segmentation', name: 'SegmentationName', width: 100 },
                { label: 'KotaID', name: 'KotaID', width: 10, hidden: true },
                { label: 'Kota', name: 'KotaName', width: 100 },
            ];
            break;
        case 'SecurityRole':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 200 },
                { label: 'Business Unit', name: 'BusinessUnit', index: 'BusinessUnit', width: 200 }
            ];
            break;
        case 'PrivilegeException':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'EntityType', name: 'EntityType', index: 'EntityType', width: 200, hidden: true },
                { label: 'Entity', name: 'EntityName', index: 'EntityName', width: 170 },
                { label: 'StatusType', name: 'StatusType', index: 'StatusType', width: 200, hidden: true },
                { label: 'Action', name: 'StatusName', index: 'StatusName', width: 170 },
                { label: 'ExKey', name: 'ExKey', index: 'ExKey', width: 200, hidden: true },
                { label: 'Value', name: 'ExValue', index: 'ExValue', width: 350 },
                { label: 'NewKey', name: 'NewKey', index: 'NewKey', width: 200, hidden: true },
                { label: 'New Value', name: 'NewValue', index: 'NewValue', width: 350 },
                { label: 'Inclusive', name: 'Inclusive', index: 'Inclusive', width: 350, hidden: true },
                { label: 'Inclusive', name: 'InclusiveLabel', index: 'InclusiveLabel', width: 200 },
                { label: 'StatusChangeID', name: 'StatusChangeID', index: 'StatusChangeID', width: 200, hidden: true }
            ];
            break;
        case 'AccountPayment':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Jenis Pembayaran / Pembelian', name: 'JenisPembayaran', width: 200 },
                { label: 'Nama Perusahaan', name: 'NamaPerusahaan', index: 'Name', width: 200 },
                { label: 'Kode Perusahaan', name: 'KodePerusahaan', width: 100 },
                { label: 'No Rek Perusahaan', name: 'NoRekPerusahaan', width: 150 },
                { label: 'Cabang Koordinator', name: 'CabangKoordinator', width: 200 },
                { label: 'Jenis Kerjasama', name: 'JenisKerjasama', width: 150 },
                { label: 'Alur Transaksi di ATM', name: 'AlurTransaksiATM', width: 150 },
                { label: 'e-Banking', name: 'EBanking', width: 150 },
                { label: 'Sandi Perusahaan m-BCA', name: 'SandiPerusahaanMBCA', width: 150 },
                { label: 'Limit', name: 'Limit', width: 200 },
                { label: 'Denominasi Voucher', name: 'DenominasiVoucher', width: 200 }
            ];
            break;

        case 'OrganizationClosure':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 500 },
                { label: 'Start Date', name: 'StartDate', width: 300, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'End Date', name: 'EndDate', width: 300, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } }
            ];
            break;

        case 'LetterTemplate':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 400 },
                { label: 'ID', name: 'AutoID', index: 'AutoID', width: 150 },
                { label: 'Type', name: 'TypeLabel', index: 'TypeLabel', width: 200 },
                { label: 'Created On', name: 'CreatedOn', index: 'CreatedOn', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } }
            ];
            break;

        case 'CallWrapUpCategory':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 },
                { label: 'Created On', name: 'CreatedOn', index: 'CreatedOn', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } }
            ];
            break;

        case 'Branch':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Kode Kantor', name: 'OfficeCode', index: 'OfficeCode', width: 300 },
                { label: 'Inisial', name: 'Initials', width: 300 },
                { label: 'Nama Kantor', name: 'Name', width: 300 },
                { label: 'Kanwil', name: 'RegionName', width: 200 },
                { label: 'Alamat', name: 'Address', width: 100 },
                { label: 'Kota', name: 'City', width: 100 },
                { label: 'Telepon 1', name: 'Telephone1', width: 100 },
                { label: 'Telepon 2', name: 'Telephone2', width: 100 },
                { label: 'Fax', name: 'Fax', width: 100 }
            ];
            break;

        case 'Cause':  // for menu Action
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Code', name: 'Code', index: 'Code', width: 400 },
                { label: 'Name', name: 'Name', index: 'Name', width: 400 }
            ];
            break;

        case 'WSID':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'WSID', name: 'Location', index: 'Location', width: 100 },
                { label: 'Lokasi', name: 'Name', width: 300 },
                { label: 'Alamat', name: 'Address', width: 300 },
                { label: 'Kota', name: 'City', width: 200 }
            ];
            break;

        case 'Queue':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'Name', width: 300 },
                { label: 'E-Mail', name: 'eMail', width: 200 },
                { label: 'Business Unit', name: 'BusinessUnitName', width: 200 }
            ];
            break;

        case 'RequestQueue':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'name', width: 290 },
                { label: 'Entered On', name: 'enteredon', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
                { label: 'Type', name: 'type', width: 138 }
            ];
            break;

        case 'LetterEntry':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'TemplateID', name: 'TemplateID', index: 'TemplateID', hidden: true },
                { label: 'Template', name: 'TemplateName', index: 'TemplateName', width: 300, formatter: returnHyperLink },
                { label: 'Date', name: 'LetterDate', index: 'LetterDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Request ID', name: 'RequestID', index: 'RequestID', width: 138 },
                { label: 'Full Name', name: 'FullName', index: 'FullName', width: 300 },
                { label: 'Account No', name: 'AccountNo', index: 'AccountNo', width: 138 },
                { label: 'Card No', name: 'CardNo', index: 'CardNo', width: 138 },
                { label: 'Created On', name: 'CreatedOn', index: 'CreatedOn', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }
            ];
            break;

        case 'StatusChangeRCM':  // Codes and Mappers - Change Status RCM
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Entity', name: 'ObjectName', index: 'ObjectName', width: 180 },
                { label: 'Attribute', name: 'AttributeName', index: 'AttributeName', width: 180 },
                { label: 'Code', name: 'Code', index: 'Code', width: 200 },
                { label: 'Description', name: 'Label', index: 'Label', width: 200 },
                { label: 'New Code', name: 'NewCodeList', index: 'NewCodeList', width: 200 }
            ];
            break;

        case 'CurrencyCode':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Currency Code', name: 'Code', index: 'Code', width: 250 },
                { label: 'Currency Name', name: 'Label', index: 'Label', width: 250 }
            ];
            break;

        case 'ResponseCode':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Type', name: 'AttributeName', index: 'AttributeName', width: 250 },
                { label: 'Code', name: 'Code', index: 'Code', width: 250 },
                { label: 'Name', name: 'Label', index: 'Label', width: 250 }
            ];
            break;

        case 'TransactionType':  // for menu Jenis Transaksi
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Type', name: 'AttributeName', index: 'AttributeName', width: 250 },
                { label: 'Code', name: 'Code', index: 'Code', width: 250 },
                { label: 'Name', name: 'Label', index: 'Label', width: 250 }
            ];
            break;

        case 'AccountType':  // for menu Deposit Type
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Account Code', name: 'Code', index: 'Code', width: 350 },
                { label: 'Account Type', name: 'Label', index: 'Label', width: 350 }
            ];
            break;

        case 'AccountCardType':  // for menu ATM Card Types
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'CardNo', name: 'Code', index: 'Code', width: 350 },
                { label: 'CardType', name: 'Label', index: 'Label', width: 350 }
            ];
            break;

        case 'LoanType':  // for menu Loan Types
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Category', name: 'AttributeName', index: 'AttributeName', width: 350 },
                { label: 'Code', name: 'Code', index: 'Code', width: 350 },
                { label: 'Name', name: 'Label', index: 'Label', width: 350 }
            ];
            break;

        case 'TransactionAttributeMapping':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Channel', name: 'ObjectName', index: 'ObjectName', width: 350 },
                { label: 'Attribute', name: 'AttributeName', index: 'AttributeName', width: 350 },
                { label: 'Key', name: 'Code', index: 'Code', width: 350 },
                { label: 'Value', name: 'Label', index: 'Label', width: 350 }
            ];
            break;

        case 'StatusMapper':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Entity Type', name: 'ObjectName', index: 'ObjectName', width: 350 },
                { label: 'Status Type', name: 'AttributeName', index: 'AttributeName', width: 350 },
                { label: 'Key', name: 'Code', index: 'Code', width: 350 },
                { label: 'Value', name: 'Label', index: 'Label', width: 350 }
            ];
            break;

        case 'StringMap':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Entity Name', name: 'ObjectName', index: 'ObjectName', width: 350 },
                { label: 'Attribute Name', name: 'AttributeName', index: 'AttributeName', width: 350 },
                { label: 'Key', name: 'Code', index: 'Code', width: 350 },
                { label: 'Value', name: 'Label', index: 'Label', width: 350 }
            ];
            break;
        case 'SystemUser':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Full Name', name: 'fullName', index: 'fullName', width: 200 },
                { label: 'Domain Name', name: 'domainName', index: 'domainName', width: 170 },
                { label: 'Business Unit', name: 'businessUnit', index: 'businessUnit', width: 200 },
                { label: 'Role', name: 'role', index: 'role', width: 170 },
                { label: 'Status', name: 'status', index: 'status', width: 200 }
            ];
            break;
        case 'ATMTransactionHistory':
            col = [
                { label: 'No Kartu', name: 'CardNo', index: 'CardNo', width: 350 },
                { label: 'Tanggal', name: 'TransactionDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'Dari Rekening', name: 'FromAccount', index: 'FromAccount', width: 350 },
                { label: 'Ke Rekening', name: 'ToAccount', index: 'ToAccount', width: 350 },
                { label: 'Terminal', name: 'Terminal', index: 'Terminal', width: 350 },
                { label: 'Lokasi', name: 'Location', index: 'Location', width: 350 },
                { label: 'Kode Tran', name: 'TransactionCode', index: 'TransactionCode', width: 350 },
                { label: 'Kode Response', name: 'ResponseCode', index: 'ResponseCode', width: 350 },
                { label: 'Response', name: 'Response', index: 'Response', width: 350 },
                { label: 'Mata Uang', name: 'Currency', index: 'Currency', width: 350 },
                { label: 'Kurs', name: 'Rate', index: 'Rate', width: 350 },
                { label: 'Jumlah Rupiah', name: 'Amount', index: 'Amount', width: 350 },
                { label: 'Jumlah Valas', name: 'Forex', index: 'Forex', width: 350 },
                { label: 'Perusahaan', name: 'Company', index: 'Company', width: 350 },
                { label: 'Sequence', name: 'Sequence', index: 'Sequence', width: 350 },
                { label: 'RequestID', name: 'RequestID', index: 'RequestID', width: 75 },
                { label: 'TransactionDescription', name: 'TransactionDescription', index: 'TransactionDescription', width: 75, hidden: true },
                { label: 'DateOnly', name: 'DateOnly', index: 'DateOnly', width: 75, hidden: true },
                { label: 'Time', name: 'Time', index: 'Time', width: 75, hidden: true },
                { label: 'Date2', name: 'Date2', index: 'Date2', width: 75, hidden: true }
            ];
            break;

        case 'DebitTransactionHistory':
            col = [
                { label: 'Merchant ID', name: 'MerchantID', index: 'MerchantID', width: 350 },
                { label: 'Merchant Name', name: 'Merchant', index: 'Merchant', width: 350 },
                { label: 'Terminal ID', name: 'TerminalID', index: 'TerminalID', width: 350 },
                { label: 'Batch', name: 'Batch', index: 'Batch', width: 350 },
                { label: 'Tanggal', name: 'Date', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'Trtp', name: 'TrTp', index: 'TrTp', width: 350 },
                { label: 'No Kartu', name: 'CardNo', index: 'CardNo', width: 350 },
                { label: 'No Rekening', name: 'AccountNo', index: 'AccountNo', width: 350 },
                { label: 'Jumlah', name: 'Amount', index: 'Amount', width: 350 },
                { label: 'Tunai', name: 'Cash', index: 'Cash', width: 350 },
                { label: 'Resp', name: 'ResponseCode', index: 'ResponseCode', width: 350 },
                { label: 'Appr CD', name: 'ApprCd', index: 'ApprCd', width: 350 },
                { label: 'Trace No', name: 'TraceNo', index: 'TraceNo', width: 350 },
                { label: 'Cashier', name: 'Cashier', index: 'Cashier', width: 350 },
                { label: 'Ref No', name: 'RefNo', index: 'RefNo', width: 350 },
                { label: 'RequestID', name: 'RequestID', index: 'RequestID', width: 75 },
                { label: 'DateOnly', name: 'DateOnly', index: 'DateOnly', width: 75, hidden: true },
                { label: 'Time', name: 'Time', index: 'Time', width: 75, hidden: true },
                { label: 'Date2', name: 'Date2', index: 'Date2', width: 75, hidden: true }
            ];
            break;

        case 'CirrusTransaction':
            col = [
                { label: 'No Kartu', name: 'CardNo', index: 'CardNo', width: 200 },
                { label: 'Tanggal', name: 'Date', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'Terminal', name: 'TerminalID', index: 'TerminalID', width: 200 },
                { label: 'No Rekening', name: 'AccountNo', index: 'AccountNo', width: 200 },
                { label: 'Transaksi', name: 'Transaction', index: 'Transaction', width: 200 },
                { label: 'Isis', name: 'Isis', index: 'Isis', width: 200 },
                { label: 'Mata Uang', name: 'Currency', index: 'Currency', width: 200 },
                { label: 'Reversal', name: 'Reversal', index: 'Reversal', width: 200 },
                { label: 'Jumlah IDR', name: 'AmountIDR', index: 'AmountIDR', width: 200 },
                { label: 'Jumlah USD', name: 'AmountUSD', index: 'AmountUSD', width: 200 },
                { label: 'Kurs USD', name: 'Rate', index: 'Rate', width: 200 },
                { label: 'Jumlah (MU Asal)', name: 'MUAsal', index: 'MUAsal', width: 200 },
                { label: 'Response', name: 'ResponseCode', index: 'ResponseCode', width: 200 },
                { label: 'Sequence', name: 'SequenceNo', index: 'SequenceNo', width: 200 },
                { label: 'Trace', name: 'Trace', index: 'Trace', width: 200 },
                { label: 'Cabang', name: 'Branch', index: 'Branch', width: 220 },
                { label: 'Cirrus', name: 'Cirrus', index: 'Cirrus', width: 200 },
                { label: 'Cab lok', name: 'Location', index: 'Location', width: 220 },
                { label: 'Partial', name: 'Partial', index: 'Partial', width: 200 },
                { label: 'Keluhan', name: 'Complaint', index: 'Complaint', width: 200 },
                { label: 'Reason', name: 'Reason', index: 'Reason', width: 200 },
                { label: 'User ID', name: 'UserID', index: 'UserID', width: 200 },
                { label: 'Request ID', name: 'RequestID', index: 'RequestID', width: 200 },
                { label: 'DateOnly', name: 'DateOnly', index: 'DateOnly', width: 75, hidden: true },
                { label: 'Time', name: 'Time', index: 'Time', width: 75, hidden: true },
                { label: 'Date2', name: 'Date2', index: 'Date2', width: 75, hidden: true }
            ];
            break;

        case 'ATMSwitchingTransaction':
            col = [
                { label: 'No Kartu', name: 'CardNo', index: 'CardNo', width: 200 },
                { label: 'Tanggal', name: 'Date', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'No Rekening', name: 'AccountNo', index: 'AccountNo', width: 200 },
                { label: 'Terminal', name: 'TerminalID', index: 'TerminalID', width: 200 },
                { label: 'Transaksi', name: 'TransactionCode', index: 'TransactionCode', width: 200 },
                { label: 'Isis', name: 'Isis', index: 'Isis', width: 200 },
                { label: 'Mata Uang', name: 'RateIDR', index: 'RateIDR', width: 200 },
                { label: 'Reversal', name: 'Reversal', index: 'Reversal', width: 200 },
                { label: 'Jumlah IDR', name: 'AmountIDR', index: 'AmountIDR', width: 200 },
                { label: 'Jumlah USD', name: 'AmountUSD', index: 'AmountUSD', width: 200 },
                { label: 'Kurs USD', name: 'RateUSD', index: 'RateUSD', width: 200 },
                { label: 'Jumlah (MU Asal)', name: 'AmountAsal', index: 'AmountAsal', width: 200 },
                { label: 'Response', name: 'Response', index: 'Response', width: 200 },
                { label: 'Reference', name: 'Sequence', index: 'Sequence', width: 200 },
                { label: 'Trace', name: 'Trace', index: 'Trace', width: 200 },
                { label: 'Cabang', name: 'Branch', index: 'Branch', width: 220 },
                { label: 'Switching', name: 'Cirrus', index: 'Cirrus', width: 200 },
                { label: 'Cab lok', name: 'Location', index: 'Location', width: 220 },
                { label: 'Partial', name: 'Partial', index: 'Partial', width: 200 },
                { label: 'Reason', name: 'Reason', index: 'Reason', width: 200 },
                { label: 'DateOnly', name: 'DateOnly', index: 'DateOnly', width: 75, hidden: true },
                { label: 'Time', name: 'Time', index: 'Time', width: 75, hidden: true },
                { label: 'Date2', name: 'Date2', index: 'Date2', width: 75, hidden: true }
            ];
            break;

        case 'CorrectionTransaction':
            col = [
                { label: 'No Rekening', name: 'AccountNo', index: 'AccountNo', width: 200 },
                { label: 'Nama', name: 'Name', index: 'Name', width: 200 },
                { label: 'Cabang', name: 'Branch', index: 'Branch', width: 220 },
                { label: 'Mata Uang', name: 'Currency', index: 'Currency', width: 200 },
                { label: 'Tanggal Transaksi', name: 'TransactionDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'Tanggal Input', name: 'InputDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'Db/Cr', name: 'DebitCredit', index: 'DebitCredit', width: 200 },
                { label: 'Tanggal Proses', name: 'ProsesDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d M Y H:i:s" } },
                { label: 'WSID', name: 'WSID', index: 'WSID', width: 200 },
                { label: 'Slid', name: 'Slid', index: 'Slid', width: 200 },
                { label: 'Kt', name: 'Kt', index: 'Kt', width: 200 },
                { label: 'Kurs', name: 'Rate', index: 'Rate', width: 200 },
                { label: 'Nilai Kor IDR', name: 'NilaiIDR', index: 'NilaiIDR', width: 200 },
                { label: 'Nilai Kor VAL', name: 'NilaiVAL', index: 'NilaiVAL', width: 200 },
                { label: 'Hasil Kor IDR', name: 'HasilIDR', index: 'HasilIDR', width: 200 },
                { label: 'Hasil Kor VAL', name: 'HasilVAL', index: 'HasilVAL', width: 200 },
                { label: 'Gagal Kor IDR', name: 'GagalIDR', index: 'GagalIDR', width: 200 },
                { label: 'Gagal Kor VAL', name: 'GagalVAL', index: 'GagalVAL', width: 220 },
                { label: 'Keterangan', name: 'Description', index: 'Description', width: 200 },
                { label: 'Request ID', name: 'RequestID', index: 'RequestID', width: 200 },
                { label: 'Row No', name: 'RowNo', index: 'RowNo', width: 200 },
                { label: 'DateOnly', name: 'DateOnly', index: 'DateOnly', width: 75, hidden: true },
                { label: 'Time', name: 'Time', index: 'Time', width: 75, hidden: true },
                { label: 'Date2', name: 'Date2', index: 'Date2', width: 75, hidden: true },
                { label: 'Tanggal Proses2', name: 'ProsesDate2', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y h:i:s" }, hidden: true },
                { label: 'Tanggal Input2', name: 'InputDate2', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y h:i:s" }, hidden: true },
                { label: 'Tanggal Transaksi2', name: 'TransactionDate2', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y h:i:s" }, hidden: true }
            ];
            break;

        case 'ATMTransactionCurrent':
            col = [
                { label: 'No', name: 'Number', index: 'Number', width: 200 },
                { label: 'TC', name: 'TransactionCode', index: 'TransactionCode', width: 200 },
                { label: 'Tanggal Transaksi', name: 'TransactionDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m H:i" } },
                { label: 'Jumlah 1', name: 'Amount1', index: 'Amount1', width: 200 },
                { label: 'RespCd', name: 'ResponseCode', index: 'ResponseCode', width: 200 },
                { label: 'Resp Desc', name: 'ResponseDescription', index: 'ResponseDescription', width: 200 },
                { label: 'Trans Desc', name: 'TransactionDescription', index: 'TransactionDescription', width: 200 },
                { label: 'Mata Uang', name: 'Currency', index: 'Currency', width: 200 },
                { label: 'Terminal', name: 'Terminal', index: 'Terminal', width: 200 },
                { label: 'Rate', name: 'Rate', index: 'Rate', width: 200 },
                { label: 'Jumlah Konversi', name: 'ConversionAmount', index: 'ConversionAmount', width: 200 },
                { label: 'Jumlah 2', name: 'Amount2', index: 'Amount2', width: 200 },
                { label: 'Rek. Asal', name: 'FromAccount', index: 'FromAccount', width: 200 },
                { label: 'Rek. Tujuan', name: 'ToAccount', index: 'ToAccount', width: 200 },
                { label: 'PayeeCd', name: 'PayeeCode', index: 'PayeeCode', width: 200 },
                { label: 'Payee No.', name: 'PayeeNumber', index: 'PayeeNumber', width: 200 },
                { label: 'Seq No.', name: 'SequenceNumber', index: 'SequenceNumber', width: 220 }
            ];
            break;

        case 'DebitTransactionCurrent':
            col = [
                { label: 'No', name: 'Number', index: 'Number', width: 200 },
                { label: 'Trans. Code', name: 'TransactionCode', index: 'TransactionCode', width: 200 },
                { label: 'Tanggal Trans.', name: 'TransactionDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m" } },
                { label: 'Jam Trans.', name: 'TransactionTime', index: 'TransactionTime', width: 200 },
                { label: 'Jumlah', name: 'Amount', index: 'Amount', width: 200 },
                { label: 'Resp. Code', name: 'ResponseCode', index: 'ResponseCode', width: 200 },
                { label: 'Appr. Code', name: 'ApprovalCode', index: 'ApprovalCode', width: 200 },
                { label: 'No ATM', name: 'AtmCardNo', index: 'AtmCardNo', width: 200 },
                { label: 'Jenis Kartu', name: 'CardType', index: 'CardType', width: 200 },
                { label: 'Terminal ID', name: 'TerminalId', index: 'TerminalId', width: 200 },
                { label: 'Jumlah Tunai', name: 'CashAmount', index: 'CashAmount', width: 200 },
                { label: 'No Rekening', name: 'AccountNumber', index: 'AccountNumber', width: 200 },
                { label: 'Retailer', name: 'Retailer', index: 'Retailer', width: 200 },
                { label: 'Trace No/Invoice', name: 'TraceNo', index: 'TraceNo', width: 200 },
                { label: 'Batch', name: 'Batch', index: 'Batch', width: 200 },
                { label: 'Sequence No.', name: 'SequenceNo', index: 'SequenceNo', width: 220 }
            ];
            break;

        case 'SMSBCATransaction':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', index: 'TransactionDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening', name: 'AccountNumber', width: 150 },
                { label: 'Jenis Transaksi', name: 'TransactionType', width: 200 },
                { label: 'Response Code', name: 'ResponseCode', width: 350 },
            ];
            break;

        case 'SMSBCATransactionPayment':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', index: 'TransactionDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening', name: 'AccountNumber', width: 150 },
                { label: 'No Referensi', name: 'ReferenceNumber', width: 150 },
                { label: 'Jenis Transaksi', name: 'TransactionType', width: 350 },
                { label: 'Nominal (IDR)', name: 'Amount', width: 150, align: 'right' },
                { label: 'Status', name: 'ResponseCode', width: 250 },
                { label: 'Pembayar', name: 'Biller', width: 200 },
                { label: 'Penerima', name: 'Receiver', width: 200 },
                { label: 'Jumlah', name: 'Total', width: 200 },
                { label: 'Informasi Lain', name: 'Other', width: 200 }
            ];
            break;

        case 'SMSBCATransactionall':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', index: 'TransactionDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening', name: 'AccountNumber', width: 150 },
                { label: 'Jenis Transaksi', name: 'TransactionType', width: 200 },
                { label: 'Response Code', name: 'ResponseCode', width: 350 },
                { label: 'Amount', name: 'Amount', width: 150, align: 'right' }
            ];
            break;

        case 'SMSTopUpTransaction':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', index: 'TransactionDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No HP', name: 'RequestId', width: 150 },
                { label: 'No Kartu', name: 'AtmCardNumber', width: 150 },
                { label: 'No Rekening', name: 'AccountNumber', width: 150 },
                { label: 'Nilai Transaksi', name: 'ValueOfTransactions', width: 200, align: 'right' },
                { label: 'Response Code', name: 'ResponseCode', width: 350 },
            ];
            break;

        case 'MBTrxPayment':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: 200 },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: 200 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 250 },
                { label: 'No Rek Pembayar', name: 'PaymentAccountNumber', width: 250 },
                { label: 'No Pelanggan', name: 'CustomerNumber', width: 150 },
                { label: 'Nominal (IDR)', name: 'Nominal', width: 150, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status', name: 'Status', width: 125 },
                { label: 'Desc Code', name: 'DescCode', width: 150 },
                { label: 'No Referensi', name: 'ReferenceNumber', width: 150 },
                { label: 'Utk Pembayaran', name: 'PaymentFor', width: 250 },
                { label: 'Keterangan', name: 'Information', width: 250 }
            ];
            break;

        case 'MBTrxTransfer':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: 200 },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: 200 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 250 },
                { label: 'No rek Pengirim', name: 'AccountSendersNumber', width: 250 },
                { label: 'No Rek tujuan', name: 'ToAccountNumber', width: 250 },
                { label: 'MU Rek Tujuan', name: 'ToAccountMU', width: 150 },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: 250 },
                { label: 'Mata Uang', name: 'Currency', width: 150 },
                { label: 'Jumlah Rp', name: 'Amount', width: 125, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Jumlah Valas', name: 'AmountForex', width: 125, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Kurs', name: 'ExchangesRate', width: 125, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status', name: 'Status', width: 125 },
                { label: 'Desc Code', name: 'DescCode', width: 150 },
                { label: 'No Referensi', name: 'ReferenceNumber', width: 150 },
                { label: 'Keterangan', name: 'Information', width: 250 },
                { label: 'Kode dan nama Bank', name: 'CodeAndBankName', width: 250 },
            ];
            break;

        case 'MBTrxCommerce':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: 200 },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: 200 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 250 },
                { label: 'No Rekening', name: 'AccountNumber', width: 250 },
                { label: 'No Pelanggan', name: 'CustomerNumber', width: 250 },
                { label: 'Nominal Pembelian (IDR)', name: 'Nominal', width: 275, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status', name: 'Status', width: 125 },
                { label: 'Desc Code', name: 'DescCode', width: 150 },
                { label: 'No Referensi', name: 'ReferenceNumber', width: 150 },
                { label: 'Jenis pembelian', name: 'PaymentType', width: 250 },
                { label: 'Keterangan', name: 'Information', width: 250 }
            ];
            break;

        case 'MBTrxInfo':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: 200 },
                { label: 'Jenis', name: 'TransactionType', width: 300 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 250 },
                { label: 'No Rekening', name: 'AccountNumber', width: 250 },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 150 },
                { label: 'Status', name: 'Status', width: 125 },
                { label: 'Desc Code', name: 'DescCode', width: 150 },
                { label: 'Keterangan', name: 'Information', width: 250 }
            ];
            break;

        case 'MBTrxAdmin':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: 200 },
                { label: 'Jenis Admin', name: 'AdminType', width: 250 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 250 },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 250 },
                { label: 'Status', name: 'Status', width: 125 },
                { label: 'Desc Code', name: 'DescCode', width: 150 },
                { label: 'Keterangan', name: 'Information', width: 250 }
            ];
            break;

        case 'MBTrxOTP':
            col = [
                { label: 'Tanggal Create OTP', name: 'MiddlewareDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Status OTP', name: 'Status', width: 150 },
                { label: 'No Kartu', name: 'ATMCardNumber', width: 200 },
                { label: 'Tanggal Transaksi', name: 'TandemDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening', name: 'AccountNumber', width: 150 },
                { label: 'Nominal (IDR)', name: 'Amount', width: 150, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } }
            ];
            break;

        case 'AccountStatement':
            col = [
                { label: 'Tanggal', name: 'TransactionDate', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m" } },
                { label: 'Keterangan Transaksi', name: 'TransactionDescription', width: 600 },
                { label: 'Cabang', name: 'Branch', width: 150 },
                { label: 'Jumlah', name: 'Amount', width: 150, align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'D / K', name: 'TransactionType', width: 50 }
            ];
            break;

        case 'CCMS':
            col = [
                { label: 'Full Name', name: 'CustomerName', width: 250, align: 'left' },
                { label: 'Birth Date', name: 'BirthDate', width: 150, align: 'left' },
                { label: 'Birth Place', name: 'Birthplace', width: 150 },
                { label: 'Customer No', name: 'CustomerNumber', width: 150 },
                { label: 'Application ID', name: 'ApplicationId', width: 150 },
                { label: 'Reference No', name: 'ReferenceNo', width: 0, align: 'left' },
                { label: 'Purpose of Application', name: 'Purpose', width: 250 },
                { label: 'Status', name: 'Status', width: 150 },
                { label: 'Current Holder', name: 'CurrentHolder', width: 250 },
                { label: 'Date Received', name: 'DateReceived', width: 150, align: 'left' },
                { label: 'Original Branch', name: 'OriginatingBranch', width: 250 },
                { label: 'Remark', name: 'Remark', width: 250, align: 'left' },
                { label: 'BusinessSource', name: 'SourceCode', width: 250 },
                { label: 'Date Created', name: 'DateCreated', width: 150, align: 'left' },
                { label: 'Date Recommended', name: 'DateRecommended', width: 150, align: 'left' },
                { label: 'Date Canceled', name: 'DateCanceled', width: 150, align: 'left' },
                { label: 'Date Verified', name: 'DateVerified', width: 150, align: 'left' },
                { label: 'Date Approved', name: 'DateApproved', width: 150, align: 'left' },
                { label: 'Date Rejected', name: 'DateReject', width: 150, align: 'left' },
                { label: 'Comment', name: 'Comment', width: 250 },
                { label: 'Creator', name: 'Creator', width: 150 }
            ];
            break;



        case 'KBITRXPayment':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rek Pembayar', name: 'PaymentAccountNumber', width: '150' },
                { label: 'No Pelanggan', name: 'CustomerNumber', width: '150' },
                { label: 'Utk Pembayaran', name: 'PaymentFor', width: '150' },
                { label: 'Nominal (IDR)', name: 'Nominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '125' },
                { label: 'Status Token', name: 'TokenStatus', width: '125' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXEcommercePayment':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rek Pembayar', name: 'PaymentAccountNumber', width: '150' },
                { label: 'No Pelanggan', name: 'CustomerNumber', width: '150' },
                { label: 'Utk Pembayaran', name: 'PaymentFor', width: '150' },
                { label: 'Nominal (IDR)', name: 'Amount', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '125' },
                { label: 'Status Token', name: 'TokenStatus', width: '125' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXAccountInformation':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis Informasi', name: 'InformationType', width: '200' },
                { label: 'No Rekening', name: 'AccountNumber', width: '150' },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '150' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXBCAAccountTransfer':
            col = [
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'No rek Pengirim', name: 'FromAccountNumber', width: '150' },
                { label: 'No Rek Tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'MU Transaksi', name: 'MUTransaction', width: '150' },
                { label: 'R', name: 'LateChargeAmount', width: '100' },
                { label: 'Nominal Transfer', name: 'TransferNominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Kurs', name: 'ExchangeRate', width: '100', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Nominal Konversi', name: 'ConversiNominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'MU Rek Tujuan', name: 'MUFromAccountNumber', width: '150' },
                { label: 'Berita', name: 'News', width: '250', align: 'left' },
                { label: 'Status', name: 'Status', width: '125' },
                { label: 'Alasan', name: 'Cause', width: '250' },
                { label: 'Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXOtherBankAccountTransfer':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'No Rek Pengirim', name: 'FromAccountNumber', width: '150' },
                { label: 'No Rek Tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'Kota', name: 'City', width: '150' },
                { label: 'Bank', name: 'Bank', width: '300' },
                { label: 'Cabang', name: 'Branch', width: '300' },
                { label: 'Penduduk', name: 'Citizen', width: '150' },
                { label: 'WNI', name: 'WNI', width: '150' },
                { label: 'Mata Uang', name: 'Currency', width: '100' },
                { label: 'Nominal Transfer', name: 'TransferNominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Biaya', name: 'Cost', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Berita', name: 'News', width: '300', align: 'left' },
                { label: 'Layanan transfer', name: 'TransferService', width: '150' },
                { label: 'Status', name: 'Status', width: '150' },
                { label: 'Alasan', name: 'Reason', width: '350', align: 'left' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXPurchase':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Tandem', name: 'TandemDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening Pembeli', name: 'AccountPaymentNumber', width: '150' },
                { label: 'Untuk Pembelian', name: 'PaymentFor', width: '200' },
                { label: 'Nominal (IDR)', name: 'Nominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '125' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '125' },
                { label: 'Token', name: 'Token', width: '100' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXTransferReject':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No. PPU', name: 'PPUNumber', width: '200', align: 'left' },
                { label: 'No. Rek Pengirim', name: 'FromAccountNumber', width: '200' },
                { label: 'No. Rek Tujuan', name: 'ToAccountNumber', width: '200' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'Kota', name: 'City', width: '200' },
                { label: 'Bank', name: 'Bank', width: '200' },
                { label: 'Nominal Transfer', name: 'NominalTransfers', width: '200', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Alasan', name: 'Reason', width: '200', align: 'left' }
            ];
            break;

        case 'KBITRXTransferStatusBCAAccountInput':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'Berkala', name: 'Periodic', width: '150' },
                { label: 'Tanggal Expired', name: 'ExpiredDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No rek Pengirim', name: 'AccountSendersNumber', width: '150' },
                { label: 'No Rek tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Nominal Transfer', name: 'TransferNominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Berita', name: 'News', width: '250', align: 'left' },
                { label: 'Status', name: 'Status', width: '150' }
            ];
            break;

        case 'KBITRXTransferStatusBCAAccountTransaction':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'Berkala', name: 'Periodic', width: '150' },
                { label: 'Tanggal Expired', name: 'ExpiredDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No rek Pengirim', name: 'AccountSendersNumber', width: '150' },
                { label: 'No Rek tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Nominal Transfer', name: 'TransferNominal', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Berita', name: 'News', width: '250', align: 'left' },
                { label: 'Status', name: 'Status', width: '150' }
            ];
            break;

        case 'KBITRXTransferStatusOtherBankAccountInput':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'No rek Pengirim', name: 'FromAccountNumber', width: '150' },
                { label: 'No Rek tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '250', align: 'left' },
                { label: 'Nominal Transfer', name: 'NominalTransfers', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Berita', name: 'News', width: '250', align: 'left' },
                { label: 'Layanan transfer', name: 'ServiceTransfer', width: '150' },
                { label: 'Penduduk', name: 'Citizen', width: '150' },
                { label: 'WNI', name: 'WNI', width: '150' },
                { label: 'Kota', name: 'City', width: '150' },
                { label: 'Bank', name: 'Bank', width: '250' },
                { label: 'Cabang', name: 'Branch', width: '200' },
                { label: 'Biaya', name: 'Fee', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'Status', name: 'Status', width: '150' }
            ];
            break;

        case 'KBITRXTransferStatusOtherBankAccountTransaction':
            col = [
                { label: 'Tanggal Input', name: 'InputDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Jenis transfer', name: 'TransferType', width: '150' },
                { label: 'No rek Pengirim', name: 'FromAccountNumber', width: '150' },
                { label: 'No Rek tujuan', name: 'ToAccountNumber', width: '150' },
                { label: 'Nama Rek Tujuan', name: 'ToAccountName', width: '200', align: 'left' },
                { label: 'Kota', name: 'City', width: '150' },
                { label: 'Bank', name: 'Bank', width: '250' },
                { label: 'Cabang', name: 'Branch', width: '200' },
                { label: 'Penduduk', name: 'Citizen', width: '150' },
                { label: 'WNI', name: 'WNI', width: '150' },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Nominal Transfer', name: 'NominalTransfers', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Biaya', name: 'Fee', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Berita', name: 'News', width: '250', align: 'left' },
                { label: 'Layanan transfer', name: 'ServiceTransfer', width: '150' },
                { label: 'Status', name: 'Status', width: '150' },
            ];
            break;

        case 'KBITRXCreditCustomerInformation':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis Informasi', name: 'InformationType', width: '150' },
                { label: 'No Rekening', name: 'AccountNumber', width: '150' },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '150' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXInvestmentProductInformation':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis Informasi', name: 'InformationType', width: '250' },
                { label: 'No Rekening', name: 'AccountNumber', width: '150' },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '150' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXUserSession':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', width: '350', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Sign On', name: 'SignOnDate', width: '350', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Sign Off', name: 'SignOffDate', width: '350', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
            ];
            break;

        case 'KBITRXVirtualAccountTransfer':
            col = [
                { label: 'Tanggal Transfer', name: 'TransferDate', width: '200', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Input', name: 'InputDate', width: '200', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Jenis transaksi', name: 'TransactionType', width: '200' },
                { label: 'No Rek Pengirim', name: 'SenderAccountNo', width: '200' },
                { label: 'No Rek Tujuan', name: 'TransferToAccount', width: '200' },
                { label: 'Nama Rek Tujuan', name: 'TransferToAccountName', width: '200', align: 'left' },
                { label: 'MU Transaksi', name: 'Currency', width: '150' },
                { label: 'R', name: 'LateChargeAmount', width: '50' },
                { label: 'Nominal Transfer', name: 'TransferAmount', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Kurs', name: 'Forex', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Nominal Konversi', name: 'AmountIDR', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'MU Rek Tujuan', name: 'ToAccountType', width: '150' },
                { label: 'Berita', name: 'SendToSubject', width: '200', align: 'left' },
                { label: 'Status', name: 'Status', width: '150' },
                { label: 'Alasan', name: 'Reason', width: '200', align: 'left' },
                { label: 'Referensi', name: 'Reference', width: '300', align: 'left' }
            ];
            break;

        case 'KBITRXCreditCardInformation':
            col = [
                { label: 'Tanggal Middleware', name: 'MiddlewareDate', width: '200', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Jenis Transaksi', name: 'Process', width: '200' },
                { label: 'Nomor Customer', name: 'FromAccountId', width: '200' },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'No Referensi', name: 'ReferenceNo', width: '320', align: 'left' }
            ];
            break;

        case 'KBITRXAKSesFundWithdrawal':
            col = [
                { label: 'Tanggal Middleware', name: 'TransactionDate', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'No Rekening', name: 'FromAccountNumber', width: '150' },
                { label: 'User ID', name: 'UserId', width: '150' },
                { label: 'Nama Nasabah', name: 'CustomerName', width: '250', align: 'left' },
                { label: 'Email', name: 'Email', width: '150' },
                { label: 'Jenis Informasi', name: 'TransactionType', width: '250' },
                { label: 'Kode PE', name: 'BillerId', width: '150' },
                { label: 'Nama PE', name: 'BillerRefInfo', width: '250' },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Nominal', name: 'Amount', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '150' },
                { label: 'Alasan', name: 'Reason', width: '250', align: 'left' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'KBITRXTopUpWallet':
            col = [
                { label: 'Tanggal Transaksi', name: 'TransactionDate', width: '150', align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Transaction Type', name: 'TransactionType', width: '250' },
                { label: 'No Rekening', name: 'FromAccountNumber', width: '150' },
                { label: 'User ID', name: 'ToAccountName', width: '150' },
                { label: 'No Hp', name: 'ToAccountNoHp', width: '150' },
                { label: 'Nominal', name: 'Amount', width: '150', align: 'right', formatter: 'number', formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                { label: 'Status Middleware', name: 'MiddlewareStatus', width: '150' },
                { label: 'Status Tandem', name: 'TandemStatus', width: '125' },
                { label: 'Status Token', name: 'FlagToken', width: '125' },
                { label: 'Mata Uang', name: 'Currency', width: '150' },
                { label: 'Description', name: 'Description', width: '300', align: 'left' },
                { label: 'No Referensi', name: 'ReferenceNumber', width: '250', align: 'left' }
            ];
            break;

        case 'Customer':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Full Name', name: 'FullName', index: 'FullName', width: 200 },
                { label: 'Birth Place', name: 'BirthPlace', width: 100 },
                { label: 'Birth Date', name: 'BirthDate', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Telephone No.', name: 'TelephoneNo', width: 100 },
                { label: 'Handphone No.', name: 'HandphoneNo', width: 100 },
                { label: 'Mother\'s Maiden Name', name: 'MothersMaidenName', width: 150 },
                { label: 'Gender', name: 'Gender', width: 100 }
            ];
            break;
        case 'CustomerAddress':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Address Type', name: 'AddressTypeLabel', width: 100 },
                { label: 'Street 1', name: 'Street1', width: 200 },
                { label: 'Street 2', name: 'Street2', width: 200 },
                { label: 'Street 3', name: 'Street3', width: 200 },
                { label: 'City', name: 'City', width: 150 },
                { label: 'ZIP/Postal Code', name: 'PostalCode', width: 100 },
                { label: 'Main Phone', name: 'TelephoneNo', width: 100 },
                { label: 'Credit Card Customer No', name: 'CreditCardCustomerNo', width: 150 },
                { label: 'Modified On', name: 'ModifiedOn', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } }
            ];
            break;
        case 'CreditCard':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'CustomerId', name: 'CustomerId', hidden: true },
                { label: 'Customer', name: 'CustomerIdName', hidden: true },
                { label: 'Credit Card Number', name: 'CreditCardNumber', width: 100 },
                { label: 'Cardholder Name', name: 'CardholderName', width: 100 },
                { label: 'Card Type', name: 'CardType', width: 100 },
                { label: 'CCType', name: 'CCType', hidden: true },
                { label: 'Customer Number', name: 'CreditCardCustomerNo', width: 100 },
                { label: 'Status', name: 'Status', width: 50 }
            ];
            break;
        case 'Deposit':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'CustomerId', name: 'CustomerId', hidden: true },
                { label: 'Customer', name: 'CustomerIdName', width: 200 },
                { label: 'Account Number', name: 'AccountNo', width: 100 },
                { label: 'Account Type', name: 'AccountTypeIdName', width: 100 },
                { label: 'Card Number', name: 'CardNo', width: 150 },
                { label: 'Card Type', name: 'CardTypeIdName', width: 150 },
                { label: 'Relation Type', name: 'RelationType', width: 150 },
                { label: 'Owner Type', name: 'OwnerTypeLabel', width: 100 },
                { label: 'Relationship With', name: 'RelationshipWith', width: 150 }
            ];
            break;
        case 'Loan':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'CustomerId', name: 'CustomerId', hidden: true },
                { label: 'Customer', name: 'CustomerIdName', hidden: true },
                { label: 'Account Number', name: 'AccountNo', width: 100 },
                { label: 'Loan Number', name: 'LoanNumber', width: 100 },
                { label: 'Loan Type', name: 'LoanTypeIdName', width: 100 }
            ];
            break;
        case 'UnpaidLoanInstallment':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                //{ label: 'Installment Date', name: 'InstallmentDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
                { label: 'Installment Date', name: 'InstallmentDate', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Unpaid Installment Amount', name: 'UnpaidInstallmentAmount', width: 300, formatter: "number" },
            ];
            break;
        case 'Channel':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'CustomerId', name: 'CustomerId', hidden: true },
                { label: 'CustomerIdName', name: 'CustomerIdName', hidden: true },
                { label: 'ChannelTypeId', name: 'ChannelTypeId', hidden: true },
                { label: 'Channel Type', name: 'ChannelType', width: 100 },
                { label: 'No Kartu', name: 'CardNo', width: 100 },
                { label: 'User ID', name: 'UserId', width: 100 },
                { label: 'Email Address', name: 'EmailAddress', width: 100 },
                { label: 'No HP', name: 'HpNo', width: 100 },
                { label: 'Corp ID', name: 'CorpId', width: 100 },
                { label: 'No Rekening', name: 'AccountNo', width: 100 },
                { label: 'Customer ID', name: 'CustomerNo', width: 100 },
                { label: 'S/N KeyBCA', name: 'SNKeyBCA', width: 100 },
                { label: 'Category', name: 'Category', width: 100 }
            ];
            break;
        case 'CCAutopay':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'No', name: 'Number', index: 'Number', width: 100 },
                { label: 'Nama Merchant', name: 'MerchantName', width: 300 },
                { label: 'No.Pelanggan', name: 'CustomerNumber', width: 200 }
            ];
            break;
        case 'CCConnection':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Key ID', name: 'KeyId', width: 200 },
                { label: 'No Customer', name: 'CustomerNoCreditCard', width: 200 },
                { label: 'No Kartu', name: 'AtmCardNo', width: 100 },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 100 },
                { label: 'Jenis Koneksi', name: 'ConnectionType', width: 100 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 100 },
                { label: 'Tanggal Koneksi', name: 'ConnectionDate', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'ID 1', name: 'Id1', width: 250 },
                { label: 'ID 2', name: 'Id2', width: 250 }
            ];
            break;
        case 'CCApplication':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Key ID yang meregister', name: 'RegisteredKeyId', width: 200 },
                { label: 'No Customer', name: 'CustomerNo', width: 200 },
                { label: 'No Kartu', name: 'CreditCardNo', width: 100 },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 100 },
                { label: 'Jenis Koneksi', name: 'ConnectionType', width: 100 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 100 },
                { label: 'Tanggal Permohonan', name: 'RequestDate', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'Tindakan', name: 'Action', width: 100 },
                { label: 'Status Permohonan', name: 'Status', width: 100 },
                { label: 'Alasan', name: 'Reason', width: 100 },
                { label: 'Tanggal Koneksi', name: 'ConnectionDate', width: 100, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'ID 1', name: 'Id1', width: 250 },
                { label: 'ID 2', name: 'Id2', width: 250 },
                { label: 'Alamat 1', name: 'Address1', width: 100 },
                { label: 'Alamat 2', name: 'Address2', width: 100 },
                { label: 'Alamat 3', name: 'Address3', width: 100 },
                { label: 'Alamat 4', name: 'Address4', width: 100 },
                { label: 'Alamat 5', name: 'Address5', width: 100 },
                { label: 'Alamat 6', name: 'Address6', width: 100 },
                { label: 'Alamat 7', name: 'Address7', width: 100 },
                { label: 'Alamat 8', name: 'Address8', width: 100 }
            ];
            break;
        case 'KeyBCAConnection':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Key ID', name: 'KeyId', width: 200 },
                { label: 'Sn Token', name: 'SNToken', width: 200 },
                { label: 'No Kartu', name: 'CardNo', width: 200 },
                { label: 'StatusKey', name: 'StatusKey', hidden: true },
                { label: 'Status', name: 'Status', width: 100 },
                { label: 'Cabang KeyBCA', name: 'BranchIssuingToken', width: 200 },
                { label: 'Merk KeyBCA', name: 'TokenType', width: 100 },
                { label: 'Diupdate Oleh', name: 'UpdatedBy', width: 200 },
                { label: 'Diaktifkan Oleh', name: 'ActivatedBy', width: 200 },
                { label: 'Tanggal Dikoneksikan', name: 'ConnectedOn', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Update', name: 'UpdatedDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Update terakhir', name: 'LastUpdateDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 200 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 200 },
                { label: 'Jenis Koneksi', name: 'ConnectionType', width: 200 },
                { label: 'ID 1', name: 'UserId1', width: 250 },
                { label: 'ID 2', name: 'UserId2', width: 250 },
            ];
            break;
        case 'KeyBCAApplication':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Key ID', name: 'UserIdClickBCA', width: 200 },
                { label: 'No Kartu', name: 'AtmCardNo', width: 200 },
                { label: 'Status', name: 'StatusKey', width: 100 },
                { label: 'Cabang KeyBCA', name: 'BranchIssuingToken', width: 200 },
                { label: 'TokenTypeKey', name: 'TokenTypeKey', hidden: true },
                { label: 'Merk KeyBCA', name: 'TokenType', width: 100 },
                { label: 'Diupdate Oleh', name: 'UpdatedBy', width: 200 },
                { label: 'Diaktifkan Oleh', name: 'ActivatedBy', width: 200 },
                { label: 'Tanggal Koneksi / Hapus', name: 'ConnectionOrDeletedDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'Tanggal Update Terakhir', name: 'LastUpdatedDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'Tanggal Permohonan', name: 'RequestDate', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i" } },
                { label: 'Tindakan', name: 'Action', width: 170 },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 200 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 200 },
                { label: 'Alasan Penolakan', name: 'RejectionReason', width: 200 },
                { label: 'Jenis Koneksi', name: 'ConnectionType', width: 200 },
                { label: 'ID 1', name: 'Id1', width: 250 },
                { label: 'ID 2', name: 'Id2', width: 250 }
            ];
            break;
        case 'KBBKeyBCAConnection':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'S/N KeyBCA', name: 'SNKeyBca', width: 200 },
                { label: 'UserID', name: 'UserID', width: 200 },
                { label: 'Nama User', name: 'UserName', width: 200 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 200 },
                { label: 'KeyBCAStatusKey', name: 'KeyBCAStatusKey', hidden: true },
                { label: 'KeyBCAStatus', name: 'KeyBCAStatus', hidden: true },
                { label: 'Status KeyBCA', name: 'KeyBCAStatusLabel', width: 170 },
                { label: 'Diaktifkan oleh', name: 'ActivatedBy', width: 200 },
                { label: 'Diupdate oleh', name: 'UpdatedBy', width: 200 },
                { label: 'Tanggal update', name: 'UpdatedOn', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'ID 1', name: 'ID1', width: 250, align: 'left' },
                { label: 'ID 2', name: 'ID2', width: 250, align: 'left' },
                { label: 'TokenTypeKey', name: 'TokenTypeKey', hidden: true },
                { label: 'CreatedDate', name: 'CreatedDate', hidden: true },
                { label: 'CorpID', name: 'CorpID', hidden: true },
                { label: 'CorpName', name: 'CorpName', hidden: true }
            ];
            break;
        case 'KBBKeyBCAApplication':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'S/N KeyBCA', name: 'SNKeyBca', width: 200 },
                { label: 'UserID', name: 'UserID', width: 200 },
                { label: 'Nama User', name: 'UserName', width: 200 },
                { label: 'Nama Aplikasi', name: 'ApplicationName', width: 200 },
                { label: 'Tindakan', name: 'Action', width: 200 },
                { label: 'Diaktifkan oleh', name: 'ActivatedBy', width: 225 },
                { label: 'Diupdate oleh', name: 'UpdatedBy', width: 220 },
                { label: 'ID 1', name: 'ID1', width: 250, align: 'left' },
                { label: 'ID 2', name: 'ID2', width: 250, align: 'left' },
                { label: 'Tanggal update', name: 'UpdatedOn', width: 200, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } }
            ];
            break;

        case 'NonCustomer':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'FullName', width: 290 },
                { label: 'Telephone No', name: 'PhoneNo', width: 290 },
                { label: 'Created On', name: 'CreatedOn', width: 150, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
            ];
            break;

        case 'QueueItem': //yeos
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Name', name: 'name', width: 180 },
                { label: 'Entered On', name: 'enteredon', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i A" } },
                { label: 'Type', name: 'type', width: 70 },
                { label: 'Product', name: 'product', width: 230 },
                { label: 'Category', name: 'category', width: 280 }
            ];
            break;
      

        case 'MBCA':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Nama Nasabah', name: 'CustomerName', width: 200 },
                { label: 'No Kartu', name: 'AtmNo', width: 200 },
                { label: 'No Handphone', name: 'MobileNo', width: 200 },
                { label: 'Tanggal Aktivasi', name: 'ActivationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Txn Terakhir', name: 'LastTransactionDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Update PIN Terakhir (Nasabah)', name: 'LastUpdateDate', width: 250, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Diupdate oleh', name: 'UpdateBy', width: 200 },
                { label: 'PIN Salah', name: 'WrongPinCounter', width: 100 },
                { label: 'BlockStatusKey', name: 'BlockStatusKey', hidden: true },
                { label: 'Status Blokir', name: 'BlockStatus', width: 100 },
                { label: 'Nama Nasabah', name: 'TandemCustomerName', width: 200 },
                { label: 'Status MBLF', name: 'TandemBlockStatus', width: 100 },
                { label: 'No Kartu MBLF', name: 'TandemCardNo', width: 200 },
                { label: 'No Handphone MBLF', name: 'TandemHpNo', width: 200 },
                { label: 'Tanggal Registrasi MBLF', name: 'TandemRegistrationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y" } },
                { label: 'Jam Registrasi MBLF', name: 'TandemRegistrationTime', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "H:i:s" } },
                { label: 'Aktivasi Fin', name: 'PinActivation', width: 100 },
                { label: 'Tanggal Aktivasi Fin', name: 'ActivationFinDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Aktivasi Fin Oleh', name: 'ActivationFin', width: 200 },
                { label: 'Ganti PIN ke', name: 'ChangePinCounter', width: 100 },
                { label: 'Disclaimer', name: 'Disclaimer', width: 100 },
                { label: 'Jenis Bahasa', name: 'Language', width: 100 }
            ];
            break;

        case 'SMSBCA':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'No Kartu', name: 'AtmCardNo', width: 200 },
                { label: 'No Handphone', name: 'MobileNumber', width: 200 },
                { label: 'Nama Pemilik Kartu', name: 'CardOwnerName', width: 200 },
                { label: 'Tanggal Registrasi', name: 'RegistrationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Registrasi Terakhir', name: 'LastRegistrationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transaksi Terakhir', name: 'LastTransactionDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'StatusKey', name: 'StatusKey', hidden: true },
                { label: 'Status SMS BCA', name: 'Status', width: 150 },
                { label: 'Counter Code Akses', name: 'AccessCodeCounter', width: 150 },
                { label: 'Update Officer', name: 'UpdateOfficer', width: 200},
                { label: 'Tanggal Update', name: 'UpdateDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
            ];
            break;

        case 'SMSTopUp':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'No Kartu', name: 'AtmCardNo', width: 200 },
                { label: 'No Handphone', name: 'MobileNumber', width: 200 },
                { label: 'Nama Pemilik Kartu', name: 'CardholderName', width: 200 },
                { label: 'Tanggal Registrasi', name: 'RegistrationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Registrasi Terakhir', name: 'LastRegistrationDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'Tanggal Transaksi Terakhir', name: 'LastTransactionDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
                { label: 'smsTopUpKey', name: 'smsTopUpKey', hidden: true },
                { label: 'Status SMS Top Up', name: 'smsTopUp', width: 150 },
                { label: 'Counter Code Akses', name: 'CounterCodeAccess', width: 150 },
                { label: 'Kode Provider', name: 'ProviderCode', width: 150 },
                { label: 'Update Officer', name: 'UpdateOfficer', width: 200 },
                { label: 'Tanggal Update', name: 'UpdateDate', width: 200, align: 'left', formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/Y H:i:s" } },
            ];
            break;

        case 'KBB':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Corporate ID', name: 'CorpID', width: 200 },
                { label: 'Nama Corporate', name: 'CorpName', width: 200 },
                { label: 'Email Address 1', name: 'Email1', width: 200 },
                { label: 'Email Address 2', name: 'Email2', width: 200 },
                { label: 'LastStatusKey', name: 'LastStatusKey', hidden: true },
                { label: 'LastStatus', name: 'LastStatus', hidden: true },
                { label: 'UserID', name: 'UserID', hidden: true },
                { label: 'CorpID', name: 'CorpID', hidden: true },
                { label: 'CardNo', name: 'CardNo', hidden: true },
                { label: 'AccountNo', name: 'AccountNo', hidden: true }
            ];
            break;

        case 'KBI':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'User ID', name: 'UserID', width: 200 },
                { label: 'Name', name: 'Name', width: 200 },
                { label: 'No Kartu', name: 'AtmCardNo', width: 200 },
                { label: 'Alamat E-mail', name: 'EmailAddress', width: 200 },
                { label: 'Tgl Registrasi', name: 'RegistrationDate', width: 200 },
                { label: 'Tgl Dibuat', name: 'CreatedDate', width: 200 },
                { label: 'Tgl Login Terakhir', name: 'LastLoginDate', width: 200 },
                { label: 'Tgl Update Terakhir', name: 'LastUpdateDate', width: 200 },
                { label: 'Diupdate oleh', name: 'UpdateBy', width: 200 },
                { label: 'PIN Salah', name: 'WrongPinCounter', width: 200 },
                { label: 'Status Blokir', name: 'BlockStatus', width: 200 },
                { label: 'Status UserID', name: 'UserIdIBankStatus', width: 200 },
                { label: 'Status Blokir Tandem', name: 'TandemStatus', width: 200 },
                { label: 'Ganti Password Ke', name: 'ChangePasswordCounter', width: 200 },
                { label: 'Disclaimer', name: 'Disclaimer', width: 200 },
                { label: 'Jenis Bahasa', name: 'Language', width: 200 },
                { label: 'Status E-mail', name: 'EmailStatus', width: 200 },
                { label: 'No Referensi', name: 'ReferenceNo', width: 200 },
                { label: 'Malware Status', name: 'MalwareStatus', width: 200 },
                { label: 'Malware Created Date', name: 'MalwareBlockedDate', width: 200 },
                { label: 'Malware Last Update', name: 'MalwareLastUpdate', width: 200 },
                { label: '', name: 'UserIdIBankStatusKey', hidden: true },
                { label: '', name: 'BlockStatusKey', hidden: true },
                { label: '', name: 'TandemStatusKey', hidden: true },
                { label: '', name: 'MalwareStatusKey', hidden: true }
            ];
            break;

        case 'Task':
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Task ID', name: 'TaskID', width: 150 },
                { label: 'Subject', name: 'Subject', width: 250 },
                { label: 'Request ID', name: 'TicketNumber', width: 150 },
                { label: 'Regarding', name: 'Regarding', width: 250 },
                { label: 'Activity Status', name: 'ActivityStatusLabel', width: 250 },
                { label: 'Start Date', name: 'StartDate', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i:s" } },
                { label: 'Due Date', name: 'DueDate', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y H:i:s" } }
            ];
            break;

        case 'CallWrapUp': //yeos
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Customer', name: 'customername', width: 180 },
                { label: 'Non Customer', name: 'noncustomername', width: 180 },
                { label: 'Agent', name: 'agent', width: 180 },
                { label: 'Call Start Time', name: 'callstarttime', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } },
                { label: 'Call End Time', name: 'callendtime', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }
            ];
            break;
        case 'CallType': //yeos
            col = [
                { label: 'ID', name: 'Id', index: 'Id', key: true, width: 75, hidden: true },
                { label: 'Summary', name: 'summary', width: 180 },
                { label: 'Category', name: 'category', width: 180 },
                { label: 'Created On', name: 'createdon', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } },
                { label: 'Modified On', name: 'modifiedon', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }
            ];
            break;
            // go jqgrid

        case 'StatusChange':  // Gio
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Key', name: 'Key', index: 'Key', width: 350 },
                { label: 'Value', name: 'Value', index: 'Value', width: 350 },
                {
                    name: '', width: 22, sortable: false, search: false,
                    formatter: function () {
                        return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                    }
                }
            ];
            break;

        case 'CompanyList':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Kode Perusahaan', name: 'CorporateId', index: 'CorporateId', width: 350 },
                { label: 'Nama Perusahaan', name: 'CorporateName', index: 'CorporateName', width: 350 }
            ];
            break;

        case 'ReportMapping':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { name: 'ReportID', width: 50, hidden: true },
                { label: 'Report Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;

        case 'ReportMappingNew':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { name: 'RoleID', index: 'RoleID', width: 50, key: true, hidden: true },
                { name: 'ReportID', width: 50, hidden: true },
                { label: 'Report Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;

        case 'Report':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { name: 'RdlName', width: 50, hidden: true },
                { name: 'ReportID', width: 50, hidden: true },
                { label: 'Report Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Report Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;
        case 'ReportNew':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { name: 'ReportID', width: 50, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;
        case 'ReportAccess':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 300 }
            ];
            break;
        case 'ReportView':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { name: 'ReportID', width: 50, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 300 },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;
        case 'MasterReport':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Name', name: 'Name', index: 'Name', width: 300 },
                //{ label: 'RDL', name: 'RdlName', index: 'RdlName', width: 300 },
                { label: 'Description', name: 'Description', index: 'Description', width: 500 }
            ];
            break;
        case 'ReportFilters':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Type', name: 'Type', index: 'Type', width: 200 },
                { label: 'Value', name: 'Value', index: 'Value', width: 200 },
                { label: 'Start Date', name: 'startdate', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } },
                { label: 'End Date', name: 'enddate', width: 130, formatter: "date", formatoptions: { srcformat: "ISO8601Long", newformat: "m/d/Y h:i:s A" } }

            ];
            break;
        case 'ReportFilter':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Type', name: 'Type', index: 'Type', width: 200 },
                { label: 'Value', name: 'Value', index: 'Value', width: 200 }
                //Swap Sequence
                //{
                //    name: '', width: 22, sortable: false, search: false,
                //    formatter: function () {
                //        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                //        return "<span class='glyphicon glyphicon-triangle-top' title='Move Up' style='cursor:pointer;'></span>"
                //    }
                //},
                //{
                //    name: '', width: 22, sortable: false, search: false,
                //    formatter: function () {
                //        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                //        return "<span class='glyphicon glyphicon-triangle-bottom' title='Move Down' style='cursor:pointer;'></span>"
                //    }
                //}

            ];
            break;
        case 'ReportColumn':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Label', name: 'Label', index: 'Type', width: 300 },
                {
                    name: '', width: 22, sortable: false, search: false,
                    formatter: function () {
                        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                        return "<span class='glyphicon glyphicon-triangle-top' title='Move Up' style='cursor:pointer;'></span>"
                    }
                },
                {
                    name: '', width: 22, sortable: false, search: false,
                    formatter: function () {
                        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                        return "<span class='glyphicon glyphicon-triangle-bottom' title='Move Down' style='cursor:pointer;'></span>"
                    }
                }
            ];
            break;
        case 'LoadGridFilterMaster':
            col = [
                { name: 'Id', index: 'Id', width: 50, key: true, hidden: true },
                { label: 'Type', name: 'Type', index: 'Type', width: 300 },
                { label: 'Value', name: 'Value', index: 'Value', width: 300 },
                { label: 'Editable', name: 'Editable', width: 100, align: 'center' }
            ];
            break;
        case 'Workflow':
            col = [
                { name: 'ID', index: 'ID', width: 50, key: true, hidden: true },
                { name: 'ServiceLevelID', index: 'ServiceLevelID', width: 50, hidden: true },
                { name: 'WgID', index: 'WgID', width: 50, hidden: true },
                { label: 'Workgroup Name', name: 'WgName', index: 'WgName', width: 300, sortable: false },
                { label: 'SL Days', name: 'WorkflowSLADays', index: 'WorkflowSLADays', width: 100, align: 'center', sortable: false },
                { label: 'Sequence No', name: 'SeqNo', index: 'SeqNo', width: 100, align: 'center', sortable: false },
                { label: 'Keterangan', name: 'Keterangan', index: 'Keterangan', width: 400, sortable: false },
                {
                    name: 'Up', width: 45, sortable: false, search: false,
                    formatter: function () {
                        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                        return "<span class='glyphicon glyphicon-triangle-top' title='Move Up' style='cursor:pointer;'></span>"
                    }, sortable: false
                },
                {
                    name: 'Down', width: 45, sortable: false, search: false,
                    formatter: function () {
                        //return "<span class='ui-icon ui-icon-trash' title='Delete' style='cursor:pointer;'></span>"
                        return "<span class='glyphicon glyphicon-triangle-bottom' title='Move Down' style='cursor:pointer;'></span>"
                    }, sortable: false
                }
            ];
            break;
    }

    return col;
}

/*Created by Ardi, for making hyperlink cell in LetterEntry grid*/
function returnHyperLink(cellValue, options, rowdata, action) {
    var entity = 'LetterTemplate/Edit?id=' + rowdata.TemplateID;
    return "<a href='javascript:void(0)' onclick='openWindow(\"" + entity + "\")'>" + cellValue + "</a>";
}

function getCustomVal(entityName) {
    var panelHeight = $(document).height();
    var custom;
    switch (entityName) {
        case 'ChannelTrx':
            custom =
                {
                    height: (panelHeight - 400),
                    rowNum: 20
                }
            break;

        case 'SMSTopUpTransaction':
            custom =
                {
                    height: (panelHeight - 400),
                    rowNum: 20
                }
            break;
        case 'CCMS':
            custom =
                {
                    height: 150,//(panelHeight - 900),
                    rowNum: 20
                }
            break;
        case 'ATMTransactionHistory':
            custom =
                {
                    height: 400,//(panelHeight - 700),
                    rowNum: 20
                }
            break;
        case 'DebitTransactionHistory':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'ATMTransactionCurrent':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'DebitTransactionCurrent':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;

        case 'ATMSwitchingTransaction':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;

        case 'CorrectionTransaction':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;

        case 'CirrusTransaction':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;

        case 'PrivilegeException':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;

        case 'Report':
            custom =
                {
                    height: (panelHeight - 340),
                    rowNum: 20
                }
            break;
        case 'ReportNew':
            custom =
                {
                    height: (panelHeight - 340),
                    rowNum: 20
                }
            break;
        case 'ReportView':
            custom =
                {
                    height: (panelHeight - 340),
                    rowNum: 20
                }
            break;
        case 'ReportMapping':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'ReportMappingNew':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'ReportFilters':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'ReportFilter':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'ReportColumn':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;
        case 'LoadGridFilterMaster':
            custom =
                {
                    height: 400,
                    rowNum: 20
                }
            break;            
        default:
            custom =
                {
                    height: (panelHeight - 340),
                    rowNum: 20
                }
            break;
    }
    return custom;
}


function loadJqgrid(gridId, postData, entityname) {
    $.jgrid.gridUnload(gridId);
    var colModels = getColModels(entityname);
    var customVal = getCustomVal(entityname);
    
    if (entityname == "SMSBCATransaction" ||
        entityname == "MBCATransaction" ||
        entityname == "KBITransaction" ||
        entityname == "AccountStatement") {
        colModels = getColModels(postData._grid);
        customVal = getCustomVal("ChannelTrx");
    }

    if (entityname == "ATMTransaction" || entityname == "DebitTransaction") {
        colModels = getColModels(entityname + postData.Tab);
        customVal = getCustomVal(entityname + postData.Tab);
    }

    var url = "";

    

    if (entityname == "CallType") {
        url = '/CallWrapUp/' + 'LoadGrid2';
    }
    else if (entityname == "Requesta") {
        url = '/Customer/' + 'LoadArchive';
    }
    else if (entityname == "LoadGridFilterMaster") {
        url = '/ReportFilters/' + 'LoadGridMaster';
    }
    else if (entityname == "Workflow") {
        url = '/ServiceLevel/' + 'LoadGridWorkflow';
    }
    else {
        url = '/' + entityname + '/' + 'LoadGrid';
    }
    
    if (entityname == "Mapping")
        colModels = getColModels(postData._category);

    $(gridId).jqGrid({
        url: url,
        mtype: "POST",
        datatype: "json",
        postData: postData,
        colModel: colModels,
        shrinkToFit: false,
        width: '100%',
        autowidth: true,
        //height: '100%',
        height: customVal.height,
        rowNum: customVal.rowNum,
        rowList: [20, 30],
        rownumbers: true,
        pager: $("#jqGridPager"),
        sortname: 'Id',
        viewrecords: true,
        sortorder: "desc",
        loadComplete: function (data) {
            /* Remove total page on footer */
            $("#sp_1_jqGridPager").hide();
            $("#input_jqGridPager").contents().filter(function () { return this.nodeType == 3; }).remove();
            $("#input_jqGridPager").prepend("<text>Page  </text>");

            /* Hide First and last on paging */
            $("#first_jqGridPager").hide();
            $("#last_jqGridPager").hide();

            /* Disabled input page number in jqgrid paging */
            $("#input_jqGridPager").find("input").attr('disabled', 'disabled');

            /* Remove rm padding from selbox */
            $(".ui-pg-selbox").removeClass("rm-padding");

            /* Hide records per page */
            // $(".ui-pg-selbox").hide();

            // if (data.records == 0) { 
            if (data.rows.length == 0) {
                var emptyText = '';

                if (entityname == "AccountStatement") {
                    var restricted = data.additional.split("<@z>")[2] == "" ? "No Records Found" : data.additional.split("<@z>")[2];
                    emptyText += '<table class="ui-jqgrid-btable" id="tableNoRecordFound"><tr><td>&nbsp;&nbsp;&nbsp;</td><td><center><h2>' + restricted + '</h2><center></td></tr></table>';
                } else {
                    emptyText += '<table class="ui-jqgrid-btable" id="tableNoRecordFound"><tr><td>&nbsp;&nbsp;&nbsp;</td><td><center><h2>No Records Found</h2><center></td></tr></table>';
                }

                var grid = $(gridId);
                var container = grid.parents('.ui-jqgrid-view');

                container.find('.ui-jqgrid-bdiv').children("table").remove();
                container.find('.ui-jqgrid-bdiv').append(emptyText);
            }
            
            if (data.additional != null) {
                var dt = data.additional.split("<@z>");

                if (entityname == "AccountStatement") {
                    $("#additionalName").val(dt[0]);
                    $("#additionalCurrency").val(dt[1]);
                }

                if (entityname == "KBITransaction") {
                    $("#additionalName").val(dt[0]);
                    $("#additionalCard").val(dt[1]);

                    if (data.records == 0) {
                        $("#information").hide();
                    }
                    else {
                        $("#information").show();
                    }
                }

                if (entityname == "SMSBCATransaction" ||
                    entityname == "MBCATransaction" ||
                    entityname == "SMSTopUpTransaction") {
                    $("#additionalName").val(dt[0]);
                    $("#additionalCard").val(dt[1]);
                    $("#additionalStatus").val(dt[2]);

                    if (data.records == 0) {
                        $("#information").hide();
                    }
                    else {
                        $("#information").show();
                    }
                }

                if (entityname == "MBCATransaction") {
                    $("#additionalNoHpTandem").val(dt[3]);
                    $("#additionalPemilikTandem").val(dt[4]);
                    $("#additionalStatusTandem").val(dt[5]);
                }
            }
        },
        jsonreader: {
            root: "rows", //array containing actual data
            page: "page", //current page
            total: "total", //total pages for the query
            records: "records", //total number of records
            cell: 'cell',
            repeatitems: false,
            id: "Id" //index of the column with the pk in it
        },
        ondblClickRow: function (rowid, iRow, iCol) {
            if (entityname == "SMSTopUpTransaction" ||
                entityname == "SMSBCATransaction" ||
                entityname == "MBCATransaction" ||
                entityname == "KBITransaction" ||
                entityname == "CCMS" ||
                entityname == "ATMTransaction" ||
                entityname == "DebitTransaction" ||
                entityname == "ATMSwitchingTransaction" ||
                entityname == "CorrectionTransaction" ||
                entityname == "CirrusTransaction" ||
                entityname == "AccountStatement" ||
                entityname == "StatusChange") {
                return false;
            }
            else if (entityname == "CustomerAddress")
            {
                openWindow('/' + entityname + '/Edit?id=' + rowid);
            }
            else if (entityname == "CallType")
            {
                openWindow('/CallWrapUp/EditCallType?id=' + rowid);
            }
            else if (entityname == "Requesta") {
                openWindow('/Customer/ArchiveDetail?id=' + rowid);
            }
            else if (entityname == "CallWrapUp") {
                openWindow('/CallWrapUp/Edit?id=' + rowid);
            }
            else if (entityname == "Report") {
                var rowData = jQuery(gridId).getRowData(rowid);
                var reportId = rowData['ReportID'].replace(/^\s+|\s+$/g, '');
                var reportName = rowData['Name'].replace(/^\s+|\s+$/g, '');
                var rdlName = rowData['RdlName'].replace(/^\s+|\s+$/g, '');
                $("#main-results").hide();
                $("#ReportParameter").show();
                
                $("#Selected-ReportID").val(reportId);
                $("#Selected-Report").val(reportName);
                $("#Selected-Rdl").val(rdlName);
                SetReportFilters();
            }
            else if (entityname == "ReportMapping") {
                var rowData = jQuery(gridId).getRowData(rowid);
                var reportId = rowData['ReportID'].replace(/^\s+|\s+$/g, '');
                openWindow('/MasterReport/Filter?ReportID=' + reportId);
            }
            else if (entityname == "ReportView") {
                var rowData = jQuery(gridId).getRowData(rowid);
                console.log(rowData['ReportID']);
                var reportId = rowData['ReportID'].replace(/^\s+|\s+$/g, '');
                var reportName = rowData['Name'].replace(/^\s+|\s+$/g, '');
                $("#main-results").hide();
                $("#ReportParameter").show();
                $("#Selected-ReportID").val(reportId);
                $("#Selected-Report").val(reportName);
                getReportData(reportId);
                //SetReportFilters();
            }
            else if (entityname == "ReportNew") {
                openWindow('ReportNew/Edit?id=' + rowid);
                //openWindow('@Url.Action("Edit", "ReportNew")?id=' + rowid);
                //var a = Url.Action("Edit", "ReportNew", new { id = rowid }));
                //var w = '@Url.Action("Edit", "ReportNew")?id=' + rowid;
                
                //openWindow(w);
            }
            else if (entityname == "ReportMappingNew") {
                var rowData = jQuery(gridId).getRowData(rowid);
                var roleId = rowData['RoleID'].replace(/^\s+|\s+$/g, '');
                openWindow('ReportMappingNew/Reports?RoleID=' + roleId);
            }
            else if (entityname == "ReportAccess") {
                return false;
            }
            else if (entityname == "ServiceLevel") {
                var rowData = jQuery(gridId).getRowData(rowid);
                var parentID = rowData['ParentID'];

                //Load parent if child is clicked in index list
                if (parentID == '' || parentID == 'null') {
                    openWindow(entityname + '/Edit?id=' + rowid);
                }
                else {
                    openWindow(entityname + '/Edit?id=' + parentID);
                }
            }
            else {
                openWindow(entityname + '/Edit?id=' + rowid);
            }
        },
        onSelectRow: function (rowid, status, e) {
            var iCol = $.jgrid.getCellIndex(e.target);
            if (entityname == "ReportFilter") {
                $("#hiddenrow").val(rowid);
            }
            if (entityname == "ReportColumn") {
                $("#hiddenrow").val(rowid);
            }
            if (entityname == "ReportAccess") {
                $("#hiddenrow").val(rowid);
                $("#btnDeleteRole").attr('disabled', false);
            }
            var reportid = $("#reportid").val();
            //var iCol = $.jgrid.getCellIndex(e.target);
            if (entityname == "ReportColumn") {
                $("#btnDeleteColumn").attr('disabled', false);
                if (iCol == 3) {
                    $.ajax({
                        type: "POST",
                        url: "/ReportColumn/SwapUp?ID=" + rowid + "&ReportID=" + reportid,
                        dataType: "json",
                        success: function (data) {
                            var postData = { _val: reportid };
                            $("#messagebox").html(MessageText(data.flag == false ? 'error' : 'info', data.Message));
                            loadJqgrid("#gridTable", postData, "ReportColumn");
                            $("#btnDeleteColumn").attr('disabled', 'disabled');
                        },
                        error: function (xhr, status, err) {
                            alert("Error");
                        }
                    });
                }
                if (iCol == 4) {
                    $.ajax({
                        type: "POST",
                        url: "/ReportColumn/SwapDown?ID=" + rowid + "&ReportID=" + reportid,
                        dataType: "json",
                        success: function (data) {
                            var postData = { _val: reportid };
                            $("#messagebox").html(MessageText(data.flag == false ? 'error' : 'info', data.Message));
                            loadJqgrid("#gridTable", postData, "ReportColumn");
                            $("#btnDeleteColumn").attr('disabled', 'disabled');
                        },
                        error: function (xhr, status, err) {
                            alert("Error");
                        }
                    });
                }
            }

            if (entityname == "ReportFilter") {
                $("#btnDeleteFilter").attr('disabled', false);
                if (iCol == 4) {
                    $.ajax({
                        type: "POST",
                        url: "/ReportFilter/SwapUp?ID=" + rowid + "&ReportID=" + reportid,
                        dataType: "json",
                        success: function (data) {
                            var postData = { _val: reportid };
                            $("#messagebox").html(MessageText(data.flag == false ? 'error' : 'info', data.Message));
                            loadJqgrid("#gridTable", postData, "ReportFilter");
                            $("#btnDeleteFilter").attr('disabled', 'disabled');
                        },
                        error: function (xhr, status, err) {
                            alert("Error");
                        }
                    });
                }
                if (iCol == 5) {
                    $.ajax({
                        type: "POST",
                        url: "/ReportFilter/SwapDown?ID=" + rowid + "&ReportID=" + reportid,
                        dataType: "json",
                        success: function (data) {
                            var postData = { _val: reportid };
                            $("#messagebox").html(MessageText(data.flag == false ? 'error' : 'info', data.Message));
                            loadJqgrid("#gridTable", postData, "ReportFilter");
                            $("#btnDeleteFilter").attr('disabled', 'disabled');
                        },
                        error: function (xhr, status, err) {
                            alert("Error");
                        }
                    });
                }
            }

            //Gio
            if (entityname == "StatusChange") {
                if (iCol == 4) {
                    var grid = jQuery("#gridSurvey").getRowData(rowid);
                    
                    $("#confirm-delete").show();
                    $("#confirm-delete").find('.btn-ok').unbind().click(function () {
                        $.ajax({
                            type: "POST",
                            url: "/StatusChange/formSubmit?actionType=Delete&StatusChangeID=" + rowid,//?eBanking=" + eBanking + "&StatusType=" + StatusType + "CurrentStatus=" + CurrentStatus,
                            dataType: "json",
                            success: function (data) {
                                var postData = { _mappingID: $("#CurrentStatus").val() };
                                loadJqgrid("#gridTable", postData, "StatusChange");
                            },
                            error: function (xhr, status, err) {
                                alert("Error");
                            }
                        });
                        $("#confirm-delete").hide();
                    });
                    $("#confirm-delete").find('#btn_cancel2').click(function () {
                        $("#confirm-delete").hide();
                    });
                    
                }
            }
        },
        onCellSelect: function (rowid, iRow, iCol) {
            var data = jQuery("#gridTable").getRowData(rowid);

            if (entityname == "CCMS") {
                $("#fullname").val(data.CustomerName);
                $("#dobDetails").val(data.BirthDate);
                $("#birthplace").val(data.Birthplace);
                $("#custNo").val(data.CustomerNumber);
                $("#appID").val(data.ApplicationId);
                $("#refNo").val(data.ReferenceNo);
                $("#purpose").val(data.Purpose);
                $("#status").val(data.Status);
                $("#currentHolder").val(data.CurrentHolder);
                $("#dateReceived").val(data.DateReceived);
                $("#origBranch").val(data.OriginatingBranch);
                $("#remark").val(data.Remark);
                $("#sourcecode").val(data.SourceCode);
                $("#dateCreated").val(data.DateCreated);
                $("#dateRecommended").val(data.DateRecommended);
                $("#dateCanceled").val(data.DateCanceled);
                $("#dateVerified").val(data.DateVerified);
                $("#dateApproved").val(data.DateApproved);
                $("#dateRejected").val(data.DateReject);
                $("#comment").val(data.Comment);
                $("#Creator").val(data.Creator);

                $("#information").show();
            }

            if (entityname == "ATMTransaction") {
                if (postData.Tab == "History") {
                    //$("input[name=RequestID]").show();
                    $("#Sequence").val(data.Sequence);
                    $("#CardNo").val(data.CardNo);
                    $("#FromAccount").val(data.FromAccount);
                    $("#ToAccount").val(data.ToAccount);
                    $("#Terminal").val(data.Terminal);
                    $("#Location").val(data.Location);
                    $("#Forex").val(data.Forex);
                    $("#Amount").val(data.Amount);
                    $("#Currency").val(data.Currency);
                    $("#Rate").val(data.Rate);
                    $("#TransactionDescription").val(data.TransactionDescription);
                    $("#TransactionCode").val(data.TransactionCode);
                    $("#ResponseCode").val(data.ResponseCode);
                    $("#Response").val(data.Response);
                    $("#Company").val(data.Company);
                    $("#TransactionDate").val(data.DateOnly + " " + data.Time);
                    $("input[name=RequestID]").val(data.RequestID);
                    $("#TicketNumber-label").text(data.RequestID);
                    $("#Date2").val(data.Date2);
                    $("#divDateTextBox").show();
                    $("#divDateTimePicker").hide();
                    $("#linkUpdate").show();
                    $("#labelUpdate").hide();
                }
                $("#linkTerminalSearch").show();
                $("#labelTerminalSearch").hide();
            }

            if (entityname == "DebitTransaction") {
                if (postData.Tab == "History") {
                    $("#CardNo").val(data.CardNo);
                    $("#AccountNo").val(data.AccountNo);
                    $("#MerchantID").val(data.MerchantID);
                    $("#Amount").val(data.Amount);
                    $("input[name=RequestID]").val(data.RequestID);
                    $("#TicketNumber-label").text(data.RequestID);
                    $("#Date").val(data.Date);
                    $("#Merchant").val(data.Merchant);
                    $("#Cash").val(data.Cash);
                    $("#TerminalID").val(data.TerminalID);
                    $("#TrTp").val(data.TrTp);
                    $("#ApprCD").val(data.ApprCD);
                    $("#ResponseCode").val(data.ResponseCode);
                    $("#RefNo").val(data.RefNo);
                    $("#ApprCd").val(data.ApprCd);
                    $("#TraceNo").val(data.TraceNo);
                    $("#Cashier").val(data.Cashier);
                    $("#Batch").val(data.Batch);
                    $("#TransactionDate").val(data.DateOnly + " " + data.Time);
                    $("#Date2").val(data.Date2);
                    $("#divDateTextBox").show();
                    $("#divDateTimePicker").hide();
                    $("#linkUpdate").show();
                    $("#labelUpdate").hide();
                }
            }
            if (entityname == "ATMSwitchingTransaction") {
                $("#CardNo").val(data.CardNo);
                $("#AccountNo").val(data.AccountNo);
                $("#AmountIDR").val(data.AmountIDR);
                $("#AmountUSD").val(data.AmountUSD);
                $("#Branch").val(data.Branch);
                $("#CardNo").val(data.CardNo);
                $("#Cirrus").val(data.Cirrus);
                $("#Date").val(data.Date);
                $("#Isis").val(data.Isis);
                $("#Location").val(data.Location);
                $("#AmountAsal").val(data.AmountAsal);
                $("#Partial").val(data.Partial);
                $("#RateUSD").val(data.RateUSD);
                $("#RateIDR").val(data.RateIDR);
                $("#Reason").val(data.Reason);
                $("#Response").val(data.Response);
                $("#Reversal").val(data.Reversal);
                $("#Sequence").val(data.Sequence);
                $("#TerminalID").val(data.TerminalID);
                $("#Trace").val(data.Trace);
                $("#TransactionCode").val(data.TransactionCode);
                $("#TransactionDate").val(data.DateOnly + " " + data.Time);
                $("#Date2").val(data.Date2);
                $("#divDateTextBox").show();
                $("#divDateTimePicker").hide();
            }
            if (entityname == "CorrectionTransaction") {
                $("#AccountNo").val(data.AccountNo);
                $("#Name").val(data.Name);
                $("#Currency").val(data.Currency);
                $("#Branch").val(data.Branch);
                $("#TransactionDate").val(data.TransactionDate2);
                $("#InputDate").val(data.InputDate2);
                $("#DebitCredit").val(data.DebitCredit);
                $("#ProsesDate").val(data.ProsesDate2);
                $("#WSID").val(data.WSID);
                $("#Slid").val(data.Slid);
                $("#Kt").val(data.Kt);
                $("#Rate").val(data.Rate);
                $("#NilaiIDR").val(data.NilaiIDR);
                $("#NilaiVAL").val(data.NilaiVAL);
                $("#HasilIDR").val(data.HasilIDR);
                $("#HasilVAL").val(data.HasilVAL);
                $("#GagalIDR").val(data.GagalIDR);
                $("#GagalVAL").val(data.GagalVAL);
                $("#Description").val(data.Description);
                $("#RowNo").val(data.RowNo);
                $("input[name=RequestID]").val(data.RequestID);
                $("#TicketNumber-label").text(data.RequestID);
                $("#Date2").val(data.Date2);
                $("#divDateTextBox").show();
                $("#divDateTimePicker").hide();
                $("#linkUpdate").show();
                $("#labelUpdate").hide();
            }
            if (entityname == "CirrusTransaction") {
                $("#CardNo").val(data.CardNo);
                $("#AccountNo").val(data.AccountNo);
                $("#AmountIDR").val(data.AmountIDR);
                $("#AmountUSD").val(data.AmountUSD);
                $("#Branch").val(data.Branch);
                $("#CardNo").val(data.CardNo);
                $("#Cirrus").val(data.Cirrus);
                $("#Complaint").val(data.Complaint);
                $("#Currency").val(data.Currency);
                $("#Date").val(data.Date);
                $("#Isis").val(data.Isis);
                $("#Location").val(data.Location);
                $("#MUAsal").val(data.MUAsal);
                $("#Partial").val(data.Partial);
                $("#Rate").val(data.Rate);
                $("#Reason").val(data.Reason);
                $("#ResponseCode").val(data.ResponseCode);
                $("#Reversal").val(data.Reversal);
                $("#SequenceNo").val(data.SequenceNo);
                $("#TerminalID").val(data.TerminalID);
                $("#Trace").val(data.Trace);
                $("#Transaction").val(data.Transaction);
                $("#UserID").val(data.UserID);
                $("#TransactionDate").val(data.DateOnly + " " + data.Time);
                $("input[name=RequestID]").val(data.RequestID);
                $("#TicketNumber-label").text(data.RequestID);
                $("#Date2").val(data.Date2);
                $("#divDateTextBox").show();
                $("#divDateTimePicker").hide();
                $("#linkUpdate").show();
                $("#labelUpdate").hide();
            }
            
        }

    })
    .navGrid("#jqGridPager", { edit: false, add: false, del: false, search: false });
}

// Adopted by Dwi Marti A R
function loadLocalJqgrid(gridId, postData, entityname) {
    $.jgrid.gridUnload(gridId);

    var colModels = getColModels(entityname);

    var shrink = true;

    switch (entityname)
    {
        case 'MBCA':
            shrink = false;
            break;
        case 'SMSBCA':
            shrink = false;
            break;
        case 'SMSTopUp':
            shrink = false;
            break;
        case 'KeyBCAConnection':
            shrink = false;
            break;
        case 'KeyBCAApplication':
            shrink = false;
            break;
        case 'KBBKeyBCAConnection':
            shrink = false;
            break;
        case 'KBBKeyBCAApplication':
            shrink = false;
            break;
        case 'CompanyList':
            shrink = false;
            break;
        case 'KBI':
            shrink = false;
            break;
    }

    $(gridId).jqGrid({
        datatype: "local",
        data: postData,
        colModel: colModels,
        shrinkToFit: shrink,
        //shrinktofit: true,
        width: '100%',
        autowidth: true,
        //forcefit: true,
        //scroll: true,
        height: 250,
        rowNum: 20,
        rowList: [20, 30],
        rownumbers: true,
        //viewsortcols: [true],
        pager: $(gridId + "Pager"),
        sortname: 'Id',
        viewrecords: true,
        sortorder: "desc",
        jsonreader: {
            root: "rows", //array containing actual data
            page: "page", //current page
            total: "total", //total pages for the query
            records: "records", //total number of records
            cell: 'cell',
            repeatitems: false,
            id: "Id" //index of the column with the pk in it
        },
        ondblClickRow: function (rowid, iRow, iCol) {
            var rowData = jQuery(this).getRowData(rowid);
            var corpId = rowData['CorporateId'];
            var corpName = rowData['CorporateName'];
            if (entityname == "CompanyList") {
                $("#inquiry-detail").show();
                $("#_corpId").val(corpId);
                $("#_corpName").val(corpName);
                $("#_uniqueKey").removeAttr("disabled");
                $("#partialViewList").html('');
            }
        }
    })
    .navGrid(gridId + "Pager", { edit: false, add: false, del: false, search: false });
}

// Created by Dwi Marti A R
function leftPadding(str, chr, len) {
    tmp = str;
    if (tmp.length > 0) {
        while (tmp.length < len) {
            tmp = chr + tmp;
        }
    }
    return tmp;
}

function set(value) {
    return value;
}

function getLabel(ddlId, entityType, statusList) {
    $.ajax({
        url: "/General/GetLabel",
        data: {
            EntityType: entityType
        },
        dataType: 'json',
        success: function (data) {
            $.each(data, function (i, value) {
                if (value.Value in statusList) {
                    $(ddlId).append($('<option>').text(value.Text).attr('value', value.Value));
                }
            });
        }
    });
}

function getNewStatus(ddlId, entityType, statusType, currentStatus) {
    $.ajax({
        url: "/General/GetNewStatus",
        data: {
            EntityType: entityType,
            StatusType: statusType,
            Status: currentStatus
        },
        dataType: 'json',
        success: function (data) {
            $.each(data, function (i, value) {
                $(ddlId).append($('<option>').text(value.Text).attr('value', value.Value));
            });
        }
    });
}

function MessageText(messageType, message) {
    var text = '';
    switch (messageType)
    {
        case 'info':
            text += "<div class='alert alert-success alert-dismissible fade in' role='alert'>";
            text += "    <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button>";
            text += "    <img src='/assets/images/error/notif_icn_info.png' height='16'/>";
            break;
        case 'alert':
            text += "<div class='alert alert-info alert-dismissible fade in' role='alert'>";
            text += "    <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button>";
            text += "    <img src='/assets/images/error/notif_icn_warn.png' height='16'/>";
            break;
        case 'error':
            text += "<div class='alert alert-danger alert-dismissible fade in' role='alert'>";
            text += "    <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button>";
            text += "    <img src='/assets/images/error/notif_icn_critical.png' height='16'/>";
            break;
        default:
            text += "<div class='alert alert-success alert-dismissible fade in' role='alert'>";
            text += "    <button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button>";
            text += "    <img src='/assets/images/error/notif_icn_info.png' height='16'/>";
            break;
    }
    text += "    &nbsp;";
    text += "    <strong>" + message + "</strong>";
    text += "</div>";
    return text;
}

function openModal(url) {
    var iMyWidth;
    var iMyHeight;
    //half the screen width minus half the new window width (plus 5 pixel borders).
    iMyWidth = (window.screen.width / 2) - (500 + 10);
    //half the screen height minus half the new window height (plus title and status bars).
    iMyHeight = (window.screen.height / 2) - (300);
    //Open the window.

    window.open(url, null, 'height=600,width=1000,toolbar=no,directories=no,status=yes,continued from previous linemenubar=no,scrollbars=no,resizable=yes ,modal=yes,left=' + iMyWidth + ',top=' + iMyHeight + ',screenX=' + iMyWidth + ',screenY=' + iMyHeight + "'");
}


var ParentPopup = {
    reloadGridParent: function (gridId) {
        try {
            jQuery('#'+gridId).trigger('reloadGrid');
        } catch (e) {
            alert(e);
        }
    }
};

ParentPopup.prototype = {
    reloadGridParent: function () {        
        try {
            //alert(this.gridId);
            jQuery('#gridTable').trigger('reloadGrid');
        } catch (e) {
            alert(e);
        }        
    }
}

function retrievePopupChild() {
    try {
        //var myGrid = $('#gridTable'),
        //                selRowId = myGrid.jqGrid('getGridParam', 'selrow'),
        //                celValue = myGrid.jqGrid('getCell', selRowId, 'columnName');
        //alert(selRowId);
        jQuery('#gridTable').trigger('reloadGrid');
    } catch (e) {
        alert(e);
    }
}

var colModelsName = null;
var urlModalPopup = "";

var PopupClass = function () {
    var self = this;
    this.hiddenId = "";
    this.id = "";
    this.url = "";
    this.bloodhound = "";
    this.depend = null;
    this.dependId = null;
    this.dependVal = null;
    this.typeaheadOptions = "";
};

PopupClass.prototype = {
    getHiddenId: function () {
        return this.hiddenId;
    },
    setHiddenId: function (str) {
        return this.hiddenId = str;
    },
    getId: function () {
        return this.id;
    },
    setId: function (str) {
        return this.id = str;
    },
    getUrl: function () {
        return this.url;
    },
    setUrl: function (str) {
        return this.url = str;
    },
    getDepend: function () {
        return this.depend;
    },
    setDepend: function (str) {
        return this.depend = str;
    },
    getDependId: function () {
        return this.dependId;
    },
    setDependId: function (str) {
        return this.dependId = str;
    },
    getDependVal: function () {
        return this.dependVal;
    },
    setDependVal: function (str) {
        return this.dependVal = str;
    },
    onPopup: function () {
        var dependId = this.dependId;
        var focusedPopupBtn = $('.input-group-addon.btn:focus');
        var focusedInput = focusedPopupBtn.parent().find('input:first');
        var popupLabel = focusedPopupBtn.parent().find('div.form-control:first');
        var hiddenInput = focusedPopupBtn.parent().prev('input');
        var url = urlModalPopup;
        
        if (!focusedPopupBtn.attr('id')) {

            return false;
        } 
        //else {
        //    focusedPopupBtn.trigger("click");
        //}
        var jsonTextColModels = JSON.stringify(colModelsName, null, 2);

        $.ajax({
            type: "GET",
            url: url,
            data: {
                'colModels': jsonTextColModels,
                'searchText': focusedInput.val(),
                'hiddenInputId': hiddenInput.val(),
                'hiddenParentId': hiddenInput.attr("id"),
                //'dependVal': $("#" + dependId).val()
                'dependVal': $(dependId).val()
            },
            cache: false,
            beforeSend: function (jqXHR, settings) {
            },
            success: function (data, textStatus, jqXHR) {
                $("#response-modal").html(data);
                script = $(this).text();
                $.globalEval(script);
                //hiddenInput.trigger("change");
                //alert('onpopup');
            }
        });
    },
    onSelect: function () {
        var transformer = this.transformer;
        var focusedInput = $('#' + this.id);
        var popupLabel = focusedInput.closest('input').next('div.form-control');
        var hiddenInput = $('#' + this.hiddenId);
        return $('#' + this.id).bind('typeahead:select', function (ev, suggestion) {

            var data = {
                "input": $('#' + this.id),
                "classInputGroup": "has-success"
            };

            /* Wrap Contennt Text Box */
            if (focusedInput.val() != '') popupLabel.addClass("wrap-pop");
            else popupLabel.removeClass("wrap-pop");

            popupLabel.text(focusedInput.val());
            hiddenInput.val(suggestion.id);
            hiddenInput.trigger("change");
            //alert('select');
            transformer(data);
        });
    },
    onBlur: function () {
        var bloodhound = this.bloodhound;
        var transformer = this.transformer;
        var focusedInput = $('#' + this.id);
        var popupLabel = focusedInput.closest('input').next('div.form-control');
        var hiddenInput = $('#' + this.hiddenId);
        var onPopup = this.onPopup;
        return focusedInput.blur(function () {

            var popupLabel = $('#' + this.id).closest('input').next('div.form-control');
            var isVisible = popupLabel.is(':visible');
            var isHidden = popupLabel.is(':hidden');
            //if (isVisible == "true") {
            //    //onPopup();
            //    return false;
            //}
            var focusedInput = $('#' + this.id);
            //if (focusedInput.val() == "" && hiddenInput.val() == "") {
            //    //onPopup();
            //    return;
            //}
            bloodhound.search(
                focusedInput.typeahead('val'),
                sync = function (suggestions) {
                },
                async = function (suggestions) {
                    if (suggestions.length > 1) {
                        if (focusedInput.val() == "" && hiddenInput.val() == "") {
                            //onPopup();
                            //return;
                        } else {
                            var data = {
                                "input": focusedInput,
                                "classInputGroup": "has-warning"
                            };
                            //popupLabel.text(focusedInput.val());
                            var label = focusedInput.parent().next('div.form-control');
                            label.text(focusedInput.val());
                            transformer(data);
                        }
                        
                    } else if (suggestions.length < 1) {
                        var data = {
                            "input": focusedInput,
                            "classInputGroup": "has-error"
                        };
                        popupLabel.text(focusedInput.val())
                        transformer(data);
                        hiddenInput.val("");
                        hiddenInput.trigger("change");
                        //alert('onblur1');
                    } else if (suggestions.length == 1) {
                        var data = {
                            "input": focusedInput,
                            "classInputGroup": "has-success"
                        };
                        transformer(data);
                        hiddenInput.val(suggestions[0].id);
                        hiddenInput.trigger("change");
                        //alert('onblur2');
                    }
                    onPopup();
                }
            )
        });
    },
    labelOnKeypress: function () {
        var typeaheadOptions = this.typeaheadOptions;
        var focusedInput = $('#' + this.id);
        var popupLabel = focusedInput.closest('input').next('div.form-control');
        var inputGroup = popupLabel.parent();
        var hiddenInput = $('#' + this.hiddenId);
        return popupLabel.keydown(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which;
            
            switch (keyCode) {
                case 9:
                    e.preventDefault();
                    break;
                case 46:
                    
                    if ($(this).attr('disabled') == "disabled") return false;
                    $(this).text("");
                    $(this).addClass("hide");
                    focusedInput.show();
                    focusedInput.typeahead(typeaheadOptions.options, typeaheadOptions.datasets);
                    if (inputGroup.hasClass("has-success")) {
                        inputGroup.removeClass("has-success");
                    }
                    if (inputGroup.hasClass("has-warning")) {
                        inputGroup.removeClass("has-warning");
                        focusedInput.typeahead('val', '');
                    }
                    if (inputGroup.hasClass("has-error")) {
                        inputGroup.removeClass("has-error");
                        focusedInput.typeahead('val', '');
                    }
                    hiddenInput.val('');
                    hiddenInput.trigger("change");

                    /* Wrap Contennt Text Box */
                    popupLabel.removeClass("wrap-pop");

                    //alert('onkeypress');
                    focusedInput.typeahead('val', '');
                    focusedInput.focus();
                    focusedInput.focus();
                    focusedInput.trigger("click");
                    break;
                default:
                    //alert(e.keyCode);
                    e.preventDefault();
                    break;
            }
            return false;
        });
    },
    transformer: function (data) {
        var input = data.input;
        var label = input.parent().next('div.form-control');
        var inputGroup = label.parent();
        input.typeahead('destroy');
        input.hide();
        if (label.hasClass("hide")) {
            label.removeClass("hide");
        }
        if (!inputGroup.hasClass(data.classInputGroup)) {
            inputGroup.addClass(data.classInputGroup);
        }
        return true;
    },
    test: function () {
        var transformer = this.transformer;
        var popupId = this.id;
        var focusedInput = $('#' + popupId);
        var popupLabel = focusedInput.closest('input').next('div.form-control');
        var hiddenInput = $('#' + this.hiddenId);
        var depend = this.depend;
        var dependId = this.dependId;
        var dependVal = this.dependVal;
        this.bloodhound = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: this.url,
                cache: false,
                prepare: function (query, settings) {
                    var data = {
                        "searchText": query,
                        "attr": {
                            "id": focusedInput.attr("id"),
                            "val": focusedInput.val(),
                            "depend": depend,
                            "dependId": dependId,
                            "dependVal": $("#" + dependId).val()
                        }
                    };
                    settings.type = "POST";
                    settings.contentType = "application/json; charset=UTF-8";
                    settings.data = JSON.stringify(data);
                    return settings;
                },
                transform: function (response) {
                    if (response.length == 1) {
                        var data = {
                            "input": focusedInput,
                            "classInputGroup": "has-success"
                        };
                        popupLabel.text(response[0].value);
                        transformer(data);
                        focusedInput.val(response[0].value);
                        hiddenInput.val(response[0].id);
                        hiddenInput.trigger("change");
                        //alert('ontransform1');
                        
                    } else if (response.length == 0) {
                        var data = {
                            "input": focusedInput,
                            "classInputGroup": "has-error"
                        };
                        popupLabel.text(focusedInput.val());
                        transformer(data);
                        hiddenInput.val("");
                        hiddenInput.trigger("change");
                        //alert('ontransform2');
                    }
                    return response;
                },

            },
            limit: 10,
            ttl: 0
        });
        this.typeaheadOptions = {
            options: {
                hint: "true",
                highlight: true,
                minLength: 1
            },
            datasets: {
                name: 'bloodhound',
                limit: 10,
                displayKey: 'value',
                source: this.bloodhound.ttAdapter(),
            }
        };

    },
    init: function () {
        this.test();
        this.onSelect();
        this.onBlur();
        this.labelOnKeypress();
        var zone = $('#' + this.id);
        var zontypehead = zone.closest('input').next('div.form-control');
        if (zone.val() != '')
            zontypehead.addClass("wrap-pop");
        else
            zontypehead.removeClass("wrap-pop");
        
        return $('#' + this.id).typeahead(this.typeaheadOptions.options, this.typeaheadOptions.datasets);
    }
};

function popupCheckFirst(attrId, guid, name) {
    var input = $("#" + attrId);
    var label = input.parent().next('div.form-control');
    var inputGroup = label.parent();
    var hiddenInput = inputGroup.prev("input");

    input.typeahead('destroy');
    input.hide();
    hiddenInput.val(guid);
    input.val(name);
    label.text(name);

    if (label.hasClass("hide")) {
        label.removeClass("hide");
    }
    if (inputGroup.hasClass("has-success")) {
        inputGroup.removeClass("has-success");
    }
    if (inputGroup.hasClass("has-warning")) {
        inputGroup.removeClass("has-warning");
    }
    if (inputGroup.hasClass("has-error")) {
        inputGroup.removeClass("has-error");
    }
    inputGroup.addClass("has-success");
}

//Created by Giovanni
//Delete every char except number
function isNumber(input, target) {
    var txt = input;
    var found = false;
    var validChars = "0123456789"; //List of valid characters

    for (j = 0; j < txt.length; j++) { //Will look through the value of text
        var c = txt.charAt(j);
        found = false;
        for (x = 0; x < validChars.length; x++) {
            if (c == validChars.charAt(x)) {
                found = true;
                break;
            }
        }
        if (!found) {
            //If invalid character is found remove it and return the valid character(s).
            $(target).val(input.substring(0, input.length - 1));
            break;
        }
    }
}

function applyPlaceholder(target, value) {
    target.val(value);
    target.css('color', 'grey');
    target.css('font-style', 'italic');
}

function removePlaceholder(target) {
    target.val("");
    target.css('color', 'black');
    target.css('font-style', 'normal');
}

//SUN
//Function to serialize input form to JSON string so it can be retreive as an object in controller
//*Input form without html helper cannot directly serialized to JSON object
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


////Format CTI date to mm/dd/yyyy hh:mm
function ctiFormatDate(str) {
    if (str == '') {
        return '';
    }
    //var formattedDate = str.substring(6, 8) + '/' + str.substring(4, 6) + '/' + str.substring(0, 4) + ' ' + str.substring(8, 10) + ':' + str.substring(10, 12);
    var formattedDate = str.substring(4, 6) + '/' + str.substring(6, 8) + '/' + str.substring(0, 4) + ' ' + str.substring(8, 10) + ':' + str.substring(10, 12);
    return formattedDate;
}


////Format CTI date to dd/mm/yyyy hh:mm
function ctiFormatDateStartDay(str) {
    if (str == '') {
        return '';
    }
    var formattedDate = str.substring(6, 8) + '/' + str.substring(4, 6) + '/' + str.substring(0, 4) + ' ' + str.substring(8, 10) + ':' + str.substring(10, 12);
    //var formattedDate = str.substring(4, 6) + '/' + str.substring(6, 8) + '/' + str.substring(0, 4) + ' ' + str.substring(8, 10) + ':' + str.substring(10, 12);
    return formattedDate;
}



$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

$.ajaxSetup({
    cache: false
});
