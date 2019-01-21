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
        global: (global === false ? global : true),
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
        global: (global === false ? global : true),
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

function $alertWarning(message, callback) {
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
        if (callback !== undefined && typeof callback === "function") {
            try {
                callback();
            }
            catch (e) { throw e;}
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

function getDocStatusDesc(code) 
{
    switch (code) {
        case DocStatus.SAVE: return "SAVED"; break;
        case DocStatus.DELETE: return "DELETED"; break;
        case DocStatus.FORBID: return "FORBIDED" ; break;
        case DocStatus.AUDIT: return "APPROVED"; break;
        default: return "";
    }
}

ToolName = {
    SAVE:"Save",
    DELETE:"Delete",
    FREEZE:"Freeze",
    UNFREEZE:"UnFreeze",
    SUBMIT:"Submit",
    AUDIT:"Audit",
    REAUDIT: "ReAudit",
    IMPORT:"Import",
    EXPORT:"Export",
    ADD:"Add",
    GOTOP: "GoTop",
};

DocStatus = {
    SAVE: "A",
    DELETE: "D",
    FORBID: "B",
    AUDIT: "C",
};

MessType = {
    SUCCESS: 1,
    ERROR:2
};

UrlHelper = {
    getUrl: function (actionName) {
        return window.location.origin + "/api/FormContent/" + actionName;
    },
    GET: window.location.origin + "/api/FormContent/",
};

RaeClassForm = {
    formContent: {},
    formContentType: null,
    IsListMode: false,
    IsQueryMode: false,
    IsAddMode: false,
    IsHomeMode: false,
    SeletedNumbers:[],
    init: function () {
        if (!RaeClassForm.IsHomeMode) {
            RaeClassForm.addFloatingTool();
            RaeClassForm.bindToolEvent();
        }
        RaeClassForm.updateNavBannerView();
    },
    initFormContent: function (fnumber) {
        RaeClassForm.isAddMode = isNullOrEmpty(fnumber);
        var url = RaeClassForm.isAddMode ? UrlHelper.getUrl("GetEmptyFormContent") : UrlHelper.getUrl("GetFormContent");
        var data = RaeClassForm.isAddMode ? null : { fnumber: fnumber };
        $getJSONSync(url, { fnumber: fnumber }, function (data) {
            RaeClassForm.formContent = data.content;
        });
        RaeClassForm.setFormByContent();
        RaeClassForm.addPageScroll();
    },
    setFormByContent: function () {
        var $_ueditor_cn = UE.getEditor('editor_CN');
        var $_ueditor_en = UE.getEditor('editor_EN');
        $("#_id").val(RaeClassForm.formContent._id);
        $("#_openid").val(RaeClassForm.formContent._openid);
        $("#fnumber").val(RaeClassForm.formContent.fnumber);
        $("#fname").val(RaeClassForm.formContent.fname);
        $("#flevel").val(RaeClassForm.formContent.flevel);
        $("#fcreateTime").val(RaeClassForm.formContent.fcreateTime);
        $("#frecordFileId1").val(RaeClassForm.formContent.frecordFileId1);
        $("#frecordFileId2").val(RaeClassForm.formContent.frecordFileId2);
        if (RaeClassForm.formContent.fdocStatus === DocStatus.FORBID) {
            RaeClassForm.setFormHeadDisabled();
        }
        $_ueditor_cn.ready(function () {
            if (RaeClassForm.formContent !== null) {
                $_ueditor_cn.execCommand('insertHtml', RaeClassForm.formContent.fcnContent);
                if (RaeClassForm.formContent.fdocStatus === DocStatus.FORBID) {
                    $_ueditor_cn.setDisabled();
                }
            }
        });
        $_ueditor_en.ready(function () {
            if (RaeClassForm.formContent !== null) {
                $_ueditor_en.execCommand('insertHtml', RaeClassForm.formContent.fenContent);
                if (RaeClassForm.formContent.fdocStatus === DocStatus.FORBID) {
                    $_ueditor_en.setDisabled();
                }
            }
        });
    },
    getContentDataByForm : function () {
        RaeClassForm.formContent.fname = $("#fname").val();
        RaeClassForm.formContent.flevel = $("#flevel").val();
        RaeClassForm.formContent.fcreateTime = $("#fcreateTime").val();
        RaeClassForm.formContent.frecordFileId1 = $("#frecordFileId1").val();
        RaeClassForm.formContent.frecordFileId2 = $("#frecordFileId2").val();
        RaeClassForm.formContent.fcnContent = UE.getEditor('editor_CN').getContent();
        RaeClassForm.formContent.fenContent = UE.getEditor('editor_EN').getContent();
        return RaeClassForm.formContent;
    },
    setFormHeadDisabled: function () {
        if (RaeClassForm.IsListMode) return;
        $("#fname").attr("disabled", "disabled");
        $("#flevel").attr("disabled", "disabled");
        $("#fcreateTime").attr("disabled", "disabled");
        $("#frecordFileId1").attr("disabled", "disabled");
        $("#frecordFileId2").attr("disabled", "disabled");
    },
    setFormDisabled: function () {
        if (RaeClassForm.IsListMode) return;
        $("#fname").attr("disabled", "disabled");
        $("#flevel").attr("disabled", "disabled");
        $("#fcreateTime").attr("disabled", "disabled");
        $("#frecordFileId1").attr("disabled", "disabled");
        $("#frecordFileId2").attr("disabled", "disabled");
        UE.getEditor('editor_CN').setDisabled();
        UE.getEditor('editor_EN').setDisabled();
    },
    setFormEnabled: function () {
        if (RaeClassForm.IsListMode) return;
        $("#fname").removeAttr('disabled');
        $("#flevel").removeAttr('disabled');
        $("#fcreateTime").removeAttr('disabled');
        $("#frecordFileId1").removeAttr('disabled');
        $("#frecordFileId2").removeAttr('disabled');
        UE.getEditor('editor_CN').setEnabled();
        UE.getEditor('editor_EN').setEnabled();
    },
    initFloatToolVisibility: function () {
        if (RaeClassForm.IsListMode === false) {
            if (RaeClassForm.isAddMode === true) {
                RaeClassForm.hideFloatingTool([ToolName.AUDIT, ToolName.ADD, ToolName.REAUDIT, ToolName.FREEZE, , ToolName.UNFREEZE, ToolName.EXPORT]);
                return;
            }
            if (RaeClassForm.formContent.fdocStatus === DocStatus.FORBID) {
                RaeClassForm.hideFloatingTool([ToolName.SAVE, ToolName.AUDIT, ToolName.REAUDIT, ToolName.FREEZE, ToolName.ADD, ToolName.EXPORT]);
            }
            if (RaeClassForm.formContent.fdocStatus === DocStatus.AUDIT) {
                RaeClassForm.hideFloatingTool([ToolName.AUDIT, ToolName.ADD, ToolName.UNFREEZE]);
            }
            if (RaeClassForm.formContent.fdocStatus === DocStatus.SAVE) {
                RaeClassForm.hideFloatingTool([ToolName.REAUDIT, ToolName.ADD, ToolName.UNFREEZE, ToolName.EXPORT]);
            }
            return;
        }
        if (RaeClassForm.IsListMode === true) {
            if (RaeClassForm.IsListMode === true) {
                RaeClassForm.hideFloatingTool([ToolName.SAVE]);
            }
            return;
        }
        if (RaeClassForm.IsQueryMode === true) {
            RaeClassForm.hideFloatingTool([ToolName.SAVE, ToolName.ADD]);
            return;
        }
        
    },
    updateFloatToolVisibilityByDocStatus: function (docStatus) {
        if (isNullOrEmpty(docStatus) || RaeClassForm.IsListMode) return;
        if (docStatus === DocStatus.SAVE) {
            RaeClassForm.hideFloatingTool([ToolName.REAUDIT, ToolName.UNFREEZE, ToolName.EXPORT]);
            RaeClassForm.showFloatingTool([ToolName.SAVE, ToolName.AUDIT, ToolName.FREEZE]);
        }
        else if (docStatus === DocStatus.AUDIT) {
            RaeClassForm.showFloatingTool([ToolName.REAUDIT, ToolName.EXPORT]);
            RaeClassForm.hideFloatingTool([ToolName.AUDIT]);
        }
        else if (docStatus === DocStatus.REAUDIT) {
            RaeClassForm.hideFloatingTool([ToolName.REAUDIT]);
            RaeClassForm.showFloatingTool([ToolName.AUDIT]);
        }
        else if (docStatus === DocStatus.FORBID) {
            RaeClassForm.hideFloatingTool([ToolName.SAVE, ToolName.AUDIT, ToolName.REAUDIT, ToolName.FREEZE, ToolName.EXPORT]);
            RaeClassForm.showFloatingTool([ToolName.UNFREEZE]);
        }
        else {
            return;
        }
    },
    isFormDataChange: function () {
        if (RaeClassForm.formContent.fname !== $("#fname").val()) return true;
        if (RaeClassForm.formContent.flevel !== $("#flevel").val()) return true;
        if (RaeClassForm.formContent.frecordFileId1 !== $("#frecordFileId1").val()) return true;
        if (RaeClassForm.formContent.frecordFileId2 !== $("#frecordFileId2").val()) return true;
        if (RaeClassForm.formContent.fcnContent !== UE.getEditor('editor_CN').getContent()) return true;
        if (RaeClassForm.formContent.fenContent !== UE.getEditor('editor_EN').getContent()) return true;
    },
    updateSelectedNumbers: function (type, datas) {
        if (type.indexOf('uncheck') === -1) {
            $.each(datas, function (i, v) {
                // 添加时，判断一行或多行的 id 是否已经在数组里 不存则添加　
                RaeClassForm.SeletedNumbers.indexOf(v.fnumber) === -1 ? RaeClassForm.SeletedNumbers.push(v.fnumber) : -1;
            });
        } else {
            $.each(datas, function (i, v) {
                RaeClassForm.SeletedNumbers.splice(RaeClassForm.SeletedNumbers.indexOf(v.fnumber), 1);    //删除取消选中行
            });
        }
        console.log(RaeClassForm.SeletedNumbers);
    },
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
    formSave: function () {
        $postJSON(UrlHelper.getUrl("Save"), { contentType: RaeClassForm.formContentType, formContent: RaeClassForm.getContentDataByForm() }, function (data) {
            if (data.isok) {
                RaeClassForm.formContent = data.content;
                RaeClassForm.updateFloatToolVisibilityByDocStatus(DocStatus.SAVE);
                $alertWarning("保存成功！");
            }
            else {
                $alertWarning("保存失败！" + data.errmsg);
            }
        });
    },
    formAudit: function () {
        var fnumbers = [];
        if (RaeClassForm.IsListMode) {
            fnumbers = RaeClassForm.SeletedNumbers;
            if (fnumbers.length === 0) {
                $alertWarning("未选中行！");
                return;
            }
        } else {
            if (RaeClassForm.isFormDataChange()) {
                $alertWarning("页面内容已发生改变，请先保存！");
                return;
            }
            fnumbers.push(RaeClassForm.formContent.fnumber);
        }
        $postJSON(UrlHelper.getUrl("Audit"), { fnumbers: fnumbers }, function (data) {
            if (data.isok) {
                RaeClassForm.updateFloatToolVisibilityByDocStatus(DocStatus.AUDIT);
                $alertWarning("审核成功！");
            }
            else {
                $alertWarning("审核失败！" + data.errmsg);
            }
        });
    },
    formReAudit: function () {
        var fnumbers = [];
        if (RaeClassForm.IsListMode) {
            fnumbers = RaeClassForm.SeletedNumbers;
            if (fnumbers.length === 0) {
                $alertWarning("未选中行！");
                return;
            }
        } else {
            fnumbers.push(RaeClassForm.formContent.fnumber);
        }
        $postJSON(UrlHelper.getUrl("ReAudit"), { fnumbers: fnumbers }, function (data) {
            if (data.isok) {
                RaeClassForm.updateFloatToolVisibilityByDocStatus(DocStatus.REAUDIT);
                $alertWarning("反审核成功！");
            }
            else { $alertWarning("反审核失败！" + data.errmsg); }
        });
    },
    formFreeze: function () {
        var fnumbers = [];
        if (RaeClassForm.IsListMode) {
            fnumbers = RaeClassForm.SeletedNumbers;
            if (fnumbers.length === 0) {
                $alertWarning("未选中行！");
                return;
            }
        } else {
            fnumbers.push(RaeClassForm.formContent.fnumber);
        }
        $postJSON(UrlHelper.getUrl("Freeze"), { fnumbers: fnumbers }, function (data) {
            if (data.isok) {
                RaeClassForm.setFormDisabled();
                RaeClassForm.updateFloatToolVisibilityByDocStatus(DocStatus.FORBID);
                $alertWarning("冻结成功！");
            }
            else { $alertWarning("冻结失败！" + data.errmsg); }
        });
    },
    formUnFreeze: function () {
        var fnumbers = [];
        if (RaeClassForm.IsListMode) {
            fnumbers = RaeClassForm.SeletedNumbers;
            if (fnumbers.length === 0) {
                $alertWarning("未选中行！");
                return;
            }
        } else {
            fnumbers.push(RaeClassForm.formContent.fnumber);
        }
        $postJSON(UrlHelper.getUrl("UnFreeze"), { fnumbers: fnumbers }, function (data) {
            if (data.isok) {
                RaeClassForm.setFormEnabled();
                RaeClassForm.updateFloatToolVisibilityByDocStatus(DocStatus.SAVE);
                $alertWarning("解冻成功！");
            }
            else { $alertWarning("解冻失败！" + data.errmsg); }
        });
    },
    formExport: function () {
        var fnumbers = [];
        if (RaeClassForm.IsListMode) {
            fnumbers = RaeClassForm.SeletedNumbers;
            if (fnumbers.length === 0) {
                $alertWarning("未选中行！");
                return;
            }
        } else {
            fnumbers.push(RaeClassForm.formContent.fnumber);
        }
        window.location.href = UrlHelper.getUrl("DownLoadJsonFile") + "?contentType=" + RaeClassForm.formContentType +"&fnumbers=" + fnumbers.join(",");
    },
    formAdd: function () {
        window.location.href = "/RaeClassMS/FormContentDetail?contentType=" + RaeClassForm.formContentType
    },
    addFloatingTool: function (toolArr) {
        var account = [];
        if (toolArr) {
            $.each(toolArr, function(index,item){
                account.push({ "type": item, "tip": item, "text": "", "url": "" });
            });
        } else {
            account.push({ "type": ToolName.SAVE, "tip": ToolName.SAVE, "text": "", "url": "" });
            //account.push({ "type": ToolName.DELETE, "tip": ToolName.DELETE, "text": "", "url": "" });
            account.push({ "type": ToolName.FREEZE, "tip": ToolName.FREEZE, "text": "", "url": "" });
            account.push({ "type": ToolName.UNFREEZE, "tip": ToolName.UNFREEZE, "text": "", "url": "" });
            account.push({ "type": ToolName.AUDIT, "tip": ToolName.AUDIT, "text": "", "url": "" });
            account.push({ "type": ToolName.REAUDIT, "tip": ToolName.REAUDIT, "text": "", "url": "" });
            account.push({ "type": ToolName.EXPORT, "tip": ToolName.EXPORT, "text": "", "url": "" });
            account.push({ "type": ToolName.ADD, "tip": ToolName.ADD, "text": "", "url": "" });
            //account.push({ "type": ToolName.IMPORT, "tip": ToolName.IMPORT, "text": "", "url": "" });
            //account.push({ "type": ToolName.GOTOP, "tip": ToolName.GOTOP, "text": "", "url": "" });
        }
        $("body").floating(
            {
                "theme": "panel_theme_round_solid", "state": true, "moveState": true, "size": "auto", "position": "left-center", "tip": { "background-color": "#000", "color": "#fff" },"account": account
            }
        );
    },
    hideFloatingTool: function (arr) {
        $.each(arr, function (index, item) {
            $("#" + item).hide();
        });
    },
    showFloatingTool: function (arr) {
        $.each(arr, function (index, item) {
            $("#" + item).show();
        });
    },
    bindToolEvent: function () {
        //RaeClassForm.$formSaveButton.bind("click", RaeClassForm.formSave);
        //RaeClassForm.$formSubmitButton.bind("click", RaeClassForm.formSubmit);
        //RaeClassForm.$formExportButton.bind("click", RaeClassForm.formExport);
        $("#" + ToolName.SAVE).bind("click", RaeClassForm.formSave);
        $("#" + ToolName.AUDIT).bind("click", RaeClassForm.formAudit);
        $("#" + ToolName.REAUDIT).bind("click", RaeClassForm.formReAudit);
        $("#" + ToolName.EXPORT).bind("click", RaeClassForm.formExport);
        $("#" + ToolName.UNFREEZE).bind("click", RaeClassForm.formUnFreeze);
        $("#" + ToolName.FREEZE).bind("click", RaeClassForm.formFreeze);
        $("#" + ToolName.ADD).bind("click", RaeClassForm.formAdd);
    },
    addPageScroll: function () {
        var pageIndexItem1 = { indexName: "基本信息", refElementId:"formContentBaseInfo"};
        var pageIndexItem2 = { indexName: "文章内容-CN", refElementId:"formContent_CN"};
        var pageIndexItem3 = { indexName: "文章内容-EN", refElementId:"formContent_EN"};
        var pageIndexItems = [pageIndexItem1, pageIndexItem2, pageIndexItem3];
        $("body").pageIndexing(
            {
                "state": true, "moveState": true, "size": "auto", "position": "left-center", "pageIndexItems": pageIndexItems
            }
        );
    },
    updateNavBannerView: function () {
        $("a[data-contentType='" + RaeClassForm.formContentType + "']").css("color","#ffffff");
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
            if (btn.id === str) {
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

