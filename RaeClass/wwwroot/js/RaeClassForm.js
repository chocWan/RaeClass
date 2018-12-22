/** Common Utilities **/

function $postJSON(url, json, successCallback, failCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: json,
        dataType: 'json',
    }).done(successCallback).fail(failCallback);
}

function $postJSONSync(url, data, callback, global) {
    var requstUrl = url + window.location.search;
    $.ajax({
        url: requstUrl,
        type: 'requstUrl',
        dataType: 'json',
        data: data,
        async: false, //是否异步
        global: (global == false ? global : true),
        success: callback
    });
}

function $getJSON(url,data, callback) {
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        data: data,
        global: false,
        success: callback
    });
}

function $getJSONSync(url, data, callback, global) {
    var requstUrl = url + window.location.search;
    $.ajax({
        url: requstUrl,
        type: 'GET',
        dataType: 'json',
        data: data,
        async: false, //是否异步
        global: (global == false ? global : true),
        success: callback
    });
}

function getUrlVars(name) {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars[name];
}

function $alertWarning(type, message, callback) {
    var msgWindow = '<div class=\"modal fade\" data-backdrop=\"static\">'
        + '  <div class=\"modal-dialog\">'
        + '    <div class=\"modal-content\">'
        + '      <div class=\"modal-header\">'
        + '        <button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>'
        + '        <h4 class=\"modal-title\">系统提示</h4>'
        + '      </div>'
        + '      <div class=\"modal-body\">'
        + '        <p>' + message + '</p>'
        + '      </div>'
        + '      <div class=\"modal-footer\">'
        + '        <button type=\"button\" class=\"btn btn-primary\" data-dismiss=\"modal\">确定</button>'
        + '      </div>'
        + '    </div>'
        + '  </div>'
        + '</div>';

    $(msgWindow).modal('show').on('hidden.bs.modal', function (e) {
        if (type == 1) {//1:Exec Success
            if (callback != undefined && typeof callback == "function") {
                try {
                    callback();
                }
                catch (e) { }
            }
            KStarForm.autoClose();
        }
        else {
            if (callback != undefined && typeof callback == "function") {
                try {
                    callback();
                }
                catch (e) { }
            }
        }
    });
}

function isNullOrEmpty() {
    var args = Array.prototype.slice.apply(arguments);
    var flag = false;
    String.prototype.trim = function () {    //对字符串扩展
        var regEx = /\s+/g;
        return this.replace(regEx, '');
    };
    $.each(args, function (index, val) {
        if (val === null || val === undefined || val === "") flag = true;
        else if (typeof (val) === "string" && val.trim() === "") flag = true;
        return !flag;
    });
    return flag;
}

ToolName = {
    SAVE:"Save",
    DELETE:"Delete",
    FREEZE:"Freeze",
    UNFREEZE:"UnFreeze",
    SUBMIT:"Submit",
    IMPORT:"Import",
    EXPORT:"Export",
    GOTOP:"GoTop",
};

DocStatus = {
    SAVE: "A",
    DELETE: "D",
    FORBID: "B",
    SUBMIT: "C",
}

UrlHelper = {
    getUrl = function (actionName) {
        return "http://localhost:49347/api/FormContent/" + actionName;
    },
}

RaeClassForm = {
    init: function () {

    },
    FormContent: {},
    /* ToolButtons */
    $formSaveButton: $('#' + ToolName.SAVE),
    $formDeleteButton: $('#' + ToolName.DELETE),
    $formFreezeButton: $('#' + ToolName.FREEZE),
    $formUnFreezeButton: $('#' + ToolName.UNFREEZE),
    $formSubmitButton: $('#' + ToolName.SUBMIT),
    $formImportButton: $('#' + ToolName.IMPORT),
    $formExportButton: $('#' + ToolName.EXPORT),
    $formGoTopButton: $('#' + ToolName.GOTOP),
    /* ToolButtonsEvent */
    formSave: function (url) {
        $postJSON(url, RaeClassForm.FormContent, function (data) {
            if (data.isOk) { alert("ok"); }
            else { alert("error"); }
        });
    },
    
    AddFloatingTool: function (toolArr) {
        var account = [];
        if (toolArr) {
            $.each(toolArr, function(index,item){
                account.push({ "type": item, "tip": item, "text": "", "url": "" });
            });
        } else {
            account.push({ "type": ToolName.Save, "tip": ToolName.Save, "text": "", "url": "" });
            account.push({ "type": ToolName.Delete, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.Freeze, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.UnFreeze, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.Submit, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.Export, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.Import, "tip": ToolName.Delete, "text": "", "url": "" });
            account.push({ "type": ToolName.GoTop, "tip": ToolName.Delete, "text": "", "url": "" });
        }
        $("body").floating(
            {
                "theme": "panel_theme_round_solid", "state": true, "moveState": true, "size": "auto", "position": "left-center", "tip": { "background-color": "#000", "color": "#fff" },"account": account
            }
        );
    },
    BindToolEvent: function () {
        $("#Save").bind("click", save);
        $("#Export").bind("click", downLoadReadJson);
    },
};

UEditorUtils = {
    isFocus: function (e) {
    alert(UE.getEditor('editor').isFocus());
    UE.dom.domUtils.preventDefault(e)
},
    setblur: function(e) {
        UE.getEditor('editor').blur();
        UE.dom.domUtils.preventDefault(e)
    },
    insertHtml: function (value) {
        UE.getEditor('editor').execCommand('insertHtml', value)
    },
    createEditor: function() {
        enableBtn();
        UE.getEditor('editor');
    },
    getAllHtml: function() {
        alert(UE.getEditor('editor').getAllHtml())
    },
    getContent: function() {
        var arr = [];
        arr.push("使用editor.getContent()方法可以获得编辑器的内容");
        arr.push("内容为：");
        arr.push(UE.getEditor('editor').getContent());
        alert(arr.join("\n"));
    },
    getPlainTxt: function() {
        var arr = [];
        arr.push("使用editor.getPlainTxt()方法可以获得编辑器的带格式的纯文本内容");
        arr.push("内容为：");
        arr.push(UE.getEditor('editor').getPlainTxt());
        alert(arr.join('\n'))
    },
    setContent: function(isAppendTo) {
        var arr = [];
        arr.push("使用editor.setContent('欢迎使用ueditor')方法可以设置编辑器的内容");
        UE.getEditor('editor').setContent('欢迎使用ueditor', isAppendTo);
        alert(arr.join("\n"));
    },
    setDisabled: function() {
        UE.getEditor('editor').setDisabled('fullscreen');
        disableBtn("enable");
    },
    setEnabled: function() {
        UE.getEditor('editor').setEnabled();
        enableBtn();
    },
    getText: function() {
        //当你点击按钮时编辑区域已经失去了焦点，如果直接用getText将不会得到内容，所以要在选回来，然后取得内容
        var range = UE.getEditor('editor').selection.getRange();
        range.select();
        var txt = UE.getEditor('editor').selection.getText();
        alert(txt)
    },
    getContentTxt: function() {
        var arr = [];
        arr.push("使用editor.getContentTxt()方法可以获得编辑器的纯文本内容");
        arr.push("编辑器的纯文本内容为：");
        arr.push(UE.getEditor('editor').getContentTxt());
        alert(arr.join("\n"));
    },
    hasContent: function() {
        var arr = [];
        arr.push("使用editor.hasContents()方法判断编辑器里是否有内容");
        arr.push("判断结果为：");
        arr.push(UE.getEditor('editor').hasContents());
        alert(arr.join("\n"));
    },
    setFocus: function() {
        UE.getEditor('editor').focus();
    },
    deleteEditor: function() {
        disableBtn();
        UE.getEditor('editor').destroy();
    },
    disableBtn: function(str) {
        var div = document.getElementById('btns');
        var btns = UE.dom.domUtils.getElementsByTagName(div, "button");
        for (var i = 0, btn; btn = btns[i++];) {
            if (btn.id == str) {
                UE.dom.domUtils.removeAttributes(btn, ["disabled"]);
            } else {
                btn.setAttribute("disabled", "true");
            }
        }
    },
    enableBtn: function() {
        var div = document.getElementById('btns');
        var btns = UE.dom.domUtils.getElementsByTagName(div, "button");
        for (var i = 0, btn; btn = btns[i++];) {
            UE.dom.domUtils.removeAttributes(btn, ["disabled"]);
        }
    },
    getLocalData: function() {
        alert(UE.getEditor('editor').execCommand("getlocaldata"));
    },
    clearLocalData: function() {
        UE.getEditor('editor').execCommand("clearlocaldata");
        alert("已清空草稿箱")
    },
};

