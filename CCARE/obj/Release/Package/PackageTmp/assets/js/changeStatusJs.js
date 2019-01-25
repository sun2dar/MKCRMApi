function init() {
    $(".form-horizontal").find("input").attr('disabled', 'disabled');//.css('background-color', 'white');
    $('a>i').click(function () {
        $(this).toggleClass('fa-rotate-180');
    });
}

function initHideStatus() {
    init();
    $("#statusDiv").hide();
}

function getValueByKey(obj, key) {
    return (key == '' ? '' : obj[key]);
}

function getDDlLabel(entityType, statusList) {
    $("#statusDdl").empty();
    $("#statusDdl").append($('<option>').text('--Select Status--').attr('value', '-1'));
    getLabel("#statusDdl", entityType, statusList);
}

function clearStatusDiv() {
    $("#messagebox").html("");
    clearStatusDdl();
    clearNextStatusDdl();
    clearReasonDdl();
    $("#sendBtn").attr('disabled', 'disabled');
    $("#cancelBtn").removeAttr('disabled');
}

function showStatusDiv() {
    clearStatusDiv();
    $("#statusDiv").show();
}

function showContentDiv() {
    $("#contentHeaderDiv").show();
    $("#collapseContentDiv").show();
}

function showAll() {
    showStatusDiv();
    showContentDiv();
};

function hideStatusDiv() {
    clearStatusDiv();
    $("#statusDiv").hide();
}

function hideContentDiv() {
    $("#contentHeaderDiv").hide();
    $("#collapseContentDiv").hide();
}

function hideAll() {
    hideStatusDiv();
    //hideContentDiv();
};

function showResynchDiv() {
    $("#resynchDiv").show();
}

function showResynchAll() {
    showResynchDiv();
    showContentDiv();
};

function clearStatusDdl() {
    $("#statusDdl").val('-1');
}

function clearNextStatusDdl() {
    $("#nextStatusDdl").val('');
    $("#nextStatusDdl").attr('disabled', 'disabled');
}

function clearReasonDdl() {
    $("#reasonDdl").val('');
    $("#reasonDdl").attr('disabled', 'disabled');
}

function ddlLabelChange(entityType, selected, currentStatus) {
    $("#messagebox").html("");
    if (selected == '-1') {
        clearNextStatusDdl();
        clearReasonDdl();
    } else {
        $("#nextStatusDdl").removeAttr('disabled');
        $("#nextStatusDdl").empty();
        $("#nextStatusDdl").append($('<option>').text('--Select Status--').attr('value', ''));
        getNewStatus("#nextStatusDdl", entityType, selected, currentStatus);
    }
    $("#sendBtn").attr('disabled', 'disabled');
};

function ddlLabelChangeEmpty() {
    $("#nextStatusDdl").attr('disabled', 'disabled');
    $("#nextStatusDdl").empty();
    $("#nextStatusDdl").append($('<option>').text('--Select Status--').attr('value', ''));
    $("#sendBtn").attr('disabled', 'disabled');
};

function ddlStatusChange() {
    var selectedStatus = $("#nextStatusDdl").val();
    var selectedReason = $("#reasonDdl").val();
    if (selectedStatus == '') {
        clearReasonDdl();
        $("#sendBtn").attr('disabled', 'disabled');
    } else {
        if (selectedStatus == null) {
            $("#sendBtn").removeAttr('disabled');
        } else {
            $("#sendBtn").removeAttr('disabled');
            $("#reasonDdl").removeAttr('disabled');
            if (selectedStatus == 'BukaBlokir') {
                $("#bukablokirTxt").val("");
                $("#bukablokirDiv").show();
            } else {
                $("#bukablokirDiv").hide();
            }
        }
    }
};

function ddlReasonChange() {
    var selectedReason = $("#reasonDdl").val();
    if (selectedReason == '') {
        $("#sendBtn").attr('disabled', 'disabled');
    } else {
        $("#sendBtn").removeAttr('disabled');
    }
};

function createRequest(entityType, statusType, previousStatus, newStatus,
                       customerId, accountNo, cardNo, mobileNo, customerNo, userId, emailAddress, snKeyBCA, corpId, category, reason,
                       additionalSummary) {
    var createreq = { status: RequestFail, requestId: "", ticketNumber: "" };
    $.ajax({
        type: "POST",
        url: "/Request/CreateRequestGetData",
        data: {
            entityType: entityType,
            statusType: statusType,
            previousStatus: previousStatus,
            newStatus: newStatus,
            customerId: customerId,
            accountNo: accountNo,
            cardNo: cardNo,
            mobileNo: mobileNo,
            customerNo: customerNo,
            userId: userId,
            emailAddress: emailAddress,
            snKeyBCA: snKeyBCA,
            corpId: corpId,
            category: category,
            reason: reason,
            additionalSummary: additionalSummary
        },
        dataType: 'json',
        async: false,
        //beforeSend: function () {
        //    $.blockUI({ message: 'Changing process in progress...' });
        //},
        success: function (data) {
            createreq.status = data.state;
            if (data.flag) {
                createreq.requestId = data.requestId;
                createreq.ticketNumber = data.ticketNumber;
            }
        },
        error: function (errorData) {
            //onError(errorData);
        //},
        //complete: function () {
        //    $.unblockUI();
        }
    });
    return createreq;
}

function updateRequest(status, requestId, updateRec) {
    var updatereq = { status: false };
    $.ajax({
        type: "POST",
        url: "/Request/CreateRequestGetData",
        data: {
            status: status,
            requestID: requestId,
            updatedById: updateRec.updatedbyid,
            updatedByName: updateRec.updatedbyname,
            updateTime: updateRec.updatetime
        },
        dataType: 'json',
        async: false,
        //beforeSend: function () {
        //    $.blockUI({ message: 'Changing process in progress...' });
        //},
        success: function (data) {
            updatereq.status = data.flag;
        },
        error: function (errorData) {
            //onError(errorData);
        //},
        //complete: function () {
        //    $.unblockUI();
        }
    });
    return updatereq;
}

function changeKeyBCAStatus(entityType, theId, oldStatus, newStatus, reason) {
    var change = { status: false, updatetime: "", updatedbyid: "", updatedbyname: "" };
    $.ajax({
        url: "/KeyBCA/ChangeKeyBCAStatus",
        data: {
            EntityType: entityType,
            TheId: theId,
            OldStatus: oldStatus,
            NewStatus: newStatus,
            Reason: reason
        },
        dataType: 'json',
        async: false,
        beforeSend: function () {
            $("#sendBtn").button('loading');
        },
        success: function (data) {
            if (data.Status == 1) {
                change.status = true;
                change.updatetime = data.UpdateTime;
                change.updatedbyid = data.UpdatedBy.ID;
                change.updatedbyname = data.UpdatedBy.Name;
            }
        },
        complete: function () {
            setTimeout(function () {
                $("#sendBtn").button('reset');
            }, 1000);
        }
    });
    return change;
}

function keyBCABukaBlokir(entityType, keyId, tokenType, random) {
    var change = { status: false, updatetime: "", updatedbyid: "", updatedbyname: "", kodeblokir: "" };
    $.ajax({
        url: "/KeyBCA/KeyBCABukaBlokir",
        data: {
            EntityType: entityType,
            KeyId: keyId,
            TokenType: tokenType,
            Random: random
        },
        dataType: 'json',
        async: false,
        beforeSend: function () {
            $("#sendBtn").button('loading');
        },
        success: function (data) {
            if (data.Status == 1) {
                change.status = true;
                change.updatetime = data.UpdateTime;
                change.updatedbyid = data.UpdatedBy.ID;
                change.updatedbyname = data.UpdatedBy.Name;
                change.kodeblokir = data.Result;
            }
        },
        complete: function () {
            setTimeout(function () {
                $("#sendBtn").button('reset');
            }, 1000);
        }
    });
    return change;
}

function keyBCAWriteToLog(entityType, createdDate, keyId, updateOfficer, lastUpdatedDate, snToken, atmNo, firstName, applicationCode, description, corporateName, action) {
    var change = { status: false, updatetime: "", updatedbyid: "", updatedbyname: "", result: 0 };
    $.ajax({
        url: "/KeyBCA/KeyBCAWriteToLog",
        data: {
            EntityType: entityType,
            CreatedDate: createdDate,
            KeyId: keyId,
            UpdateOfficer: updateOfficer,
            LastUpdatedDate: lastUpdatedDate,
            SNToken: snToken,
            ATMNo: atmNo,
            FirstName: firstName,
            ApplicationCode: applicationCode,
            Description: description,
            CorporateName: corporateName,
            Action: action
        },
        dataType: 'json',
        async: false,
        //beforeSend: function () {
        //    $.blockUI({ message: 'Changing process in progress...' });
        //},
        success: function (data) {
            change.status = true;
            change.updatetime = data.UpdateTime;
            change.updatedbyid = data.UpdatedBy.ID;
            change.updatedbyname = data.UpdatedBy.Name;
            change.result = data.Status;
        //},
        //complete: function () {
        //$.unblockUI();
        }
    });
    return change;
}

var RequestFail = -1;
var RequestNotCreated = 0;
var RequestSuccess = 1;

var StatusChangeFail = '-1';
var StatusChangeCRMError = '0';
var StatusChangeSuccess = '1';
var StatusChangeEAIError = '2';
var StatusChangeRequestNotCreated = '3';
var StatusChangePinReset = '4';
var StatusChangeKBILoginReset = '5';
var StatusChangeBukaBlokir = '6';
var StatusChangeKBBLoginReset = '7';

var StatusChangeSuccessButRequestCantClose = '8';
var StatusChangeFailRequestCancel = '9';
var StatusChangeFailButRequestCantCancel = '10';
var StatusChangeNoRequest = '11';
var StatusChangeSuccessViaInquiry = '12';
var StatusChangeBukaBlokirLogFail = '13';
var StatusMalwareChange = '14';

var ExceptionUserLogin = 0;
var ExceptionBlockVsTandem = 1;

// Next this should be initialized from DB
var entityLabelList = {
    1: "KlikBCA Individu",
    2: "m-BCA",
    3: "SMS Top Up",
    4: "BCA By Phone",
    5: "KlikBCA Bisnis",
    6: "SMS BCA",
    7: "Koneksi Kartu Kredit",
    8: "Status Permohonan Kartu Kredit",
    9: "Koneksi KeyBCA Individu",
    10: "Status Permohonan KeyBCA Individu",
    11: "Koneksi KeyBCA Bisnis",
    12: "Status Permohonan KeyBCA Bisnis",
    13: "Kartu Kredit",
    14: "Deposit",
    17: "Merchant",
    18: "Corporate Card"
};

function ChangeStatusMessageBox(messageBoxId, statusChange, requestId, theNumber) {
    var message = '';
    switch (statusChange) {
        case StatusChangeFail:
            message = 'Change status failed.';
            $(messageBoxId).html(MessageText('error', message));
            break;
        case StatusChangeCRMError:
            message = "Unexpected error occured. Please contact System Administrator.";
            $(messageBoxId).html(MessageText('error', message));
            break;
        case StatusChangeSuccess:
            var url = "/Request/Edit?id=" + requestId;
            message = "Status has been successfully changed. A Request has been created in CRM. To access Request, Click here ";
            message += "<a href=\"JavaScript:openModal('" + url + "')\"><font color=\"blue\"><u>" + theNumber + "</u><font/></a>";
            $(messageBoxId).html(MessageText('info', message));
            break;
        case StatusChangeEAIError:
            message = "Error occured. Please contact System Administrator.";
            $(messageBoxId).html(MessageText('error', message));
            break;
        case StatusChangeRequestNotCreated:
            message = 'Status has been changed but request creation failed (No RCM).';
            $(messageBoxId).html(MessageText('alert', message));
            break;
        case StatusChangePinReset:
            message = "Pin has been successfully reset.";
            $(messageBoxId).html(MessageText('info', message));
            break;
        case StatusChangeKBILoginReset:
            break;
        case StatusChangeBukaBlokir:
            message = 'KeyBCA is successfully unblocked. Kode Buka Blokir : ' + theNumber;
            $(messageBoxId).html(MessageText('info', message));
            break;
        case StatusChangeBukaBlokirLogFail:
            message = 'KeyBCA is successfully unblocked. Write to log failed. Kode Buka Blokir : ' + theNumber;
            $(messageBoxId).html(MessageText('info', message));
            break;
        case StatusChangeKBBLoginReset:
            break;
        case StatusChangeSuccessButRequestCantClose:
            var url = "/Request/Edit?id=" + requestId;
            message = "Status has been successfully changed. A Request has been created in CRM but fail to close. To access Request, Click here ";
            message += "<a href=\"JavaScript:openModal('" + url + "')\"><font color=\"blue\"><u>" + theNumber + "</u><font/></a>";
            $(messageBoxId).html(MessageText('alert', message));
            break;
        case StatusChangeFailRequestCancel:
            var url = "/Request/Edit?id=" + requestId;
            message = "Change status failed. A Request has been canceled. To access Request, Click here ";
            message += "<a href=\"JavaScript:openModal('" + url + "')\"><font color=\"blue\"><u>" + theNumber + "</u><font/></a>";
            $(messageBoxId).html(MessageText('error', message));
            break;
        case StatusChangeFailButRequestCantCancel:
            var url = "/Request/Edit?id=" + requestId;
            message = "Change status failed. A Request has been created in CRM but fail to cancel. To access Request, Click here ";
            message += "<a href=\"JavaScript:openModal('" + url + "')\"><font color=\"blue\"><u>" + theNumber + "</u><font/></a>";
            $(messageBoxId).html(MessageText('error', message));
            break;
        case StatusChangeNoRequest:
            message = 'Status has been changed but request creation failed.';
            $(messageBoxId).html(MessageText('alert', message));
            break;
        case StatusChangeSuccessViaInquiry:
            message = 'Status has been successfully changed.';
            $(messageBoxId).html(MessageText('info', message));
            break;
        case StatusMalwareChange:
            message = 'Malware Status has been successfully changed.';
            $(messageBoxId).html(MessageText('info', message));
            break;
    }
}

function ChangeStatusException(messageBoxId, theException) {
    var message = '';
    switch (theException) {
        case ExceptionUserLogin:
            message = 'Status Login user tidak aktif.';
            $(messageBoxId).html(MessageText('error', message));
            break;
        case ExceptionBlockVsTandem :
            message = 'Tidak bisa ubah status, Status dan Status Tandem berbeda.';
            $(messageBoxId).html(MessageText('error', message));
            break;
    }
}

function writeLog(entityType, statusType, oldValue, newValue, statusChange, requestId, theNumber) {
    var entityTypeLabel = getValueByKey(entityLabelList, entityType);

    var message = '';
    switch (statusChange) {
        case StatusChangeFail:
            message = 'Change status failed.';
            break;
        case StatusChangeCRMError:
            break;
        case StatusChangeSuccess:
            message = "Status has been successfully changed. A Request has been created in CRM."
            message += "\r\n\tRequest ID: " + requestId;
            message += "\r\n\tTicket Number: " + theNumber;
            break;
        case StatusChangeEAIError:
            break;
        case StatusChangeRequestNotCreated:
            message = 'Status has been changed but request creation failed (No RCM).';
            break;
        case StatusChangePinReset:
            break;
        case StatusChangeKBILoginReset:
            break;
        case StatusChangeBukaBlokir:
            message = 'KeyBCA is successfully unblocked. Kode Buka Blokir : ' + theNumber;
            break;
        case StatusChangeKBBLoginReset:
            break;
        case StatusChangeSuccessButRequestCantClose:
            message = "Status has been successfully changed. A Request has been created in CRM but fail to close.";
            message += "\r\n\tRequest ID: " + requestId;
            message += "\r\n\tTicket Number: " + theNumber;
            break;
        case StatusChangeFailRequestCancel:
            message = "Change status failed. A Request has been canceled.";
            message += "\r\n\tRequest ID: " + requestId;
            message += "\r\n\tTicket Number: " + theNumber;
            break;
        case StatusChangeFailButRequestCantCancel:
            message = "Change status failed. A Request has been created in CRM but fail to cancel.";
            message += "\r\n\tRequest ID: " + requestId;
            message += "\r\n\tTicket Number: " + theNumber;
            break;
        case StatusChangeNoRequest:
            message = 'Status has been changed but request creation failed.';
            break;
        case StatusChangeSuccessViaInquiry:
            message = 'Status has been successfully changed.';
            break;
    }

    $.ajax({
        url: "/ChangeStatus/WriteMessageToLog",
        data: {
            EntityType: entityTypeLabel,
            StatusType: statusType,
            OldValue: oldValue,
            NewValue: newValue,
            Message: message
        },
        dataType: 'json',
        async: false,
        //beforeSend: function () {
        //    $.blockUI({ message: 'Changing process in progress...' });
        //},
        success: function (data) {

            //},
            //complete: function () {
            //$.unblockUI();
        }
    });
}

