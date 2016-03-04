/*!
* valuebox 1.4.3
* Copyright 2010, 济南捷诺软件技术有限公司
* 陆大鹏
* 2010年6月3日14:00:21
*/

/*
1.2     弹出窗口在界面局部刷新后点击无效；点击事件绑定在刷新前的文本框上
1.3     增加 getID getText find 
1.4     删除 wrapItem addDropDownButton 方法，改由服务器生成，以使用更细节的控制(如每个列可设置一个图标)
1.4.1   增加 nextEnterStop、notEnterStopFilter
1.4.2 根据 jslint 整理代码
*/

/* options 参数
 
nextEnterStop: 
类型： jQuery集合，如 $('#packCode')
说明： 按回车后跳转到的 input（不控制 tab 跳转）， 跳转后选择全部文本

notEnterStopFilter:
类型： 字符串，jQuery selector，如 "#packCode"
说明： 按回车后不会跳转到这些 input
*/

/* 2011年9月21日 已禁用回车 tab */
/* enterStop
valueBox 按回车时将忽略以下 input
1. 不可见的
2. 具有 notEnterStop 样式的
3. 满足 notEnterStopFilter 选择条件的
4. 位于当前控件及 nextEnterStop 之间的，如果 指定了 nextEnterStop
*/


(function ($) {

    function getTypeName(typeFullName) {
        var reg, match;
        reg = /(?!\_)[a-zA-Z0-9]+?$/;
        match = reg.exec(typeFullName);
        if (match === null) { return ""; }
        else { return match[0]; }
    }

    function setState(element, state) {
        var options = $.data(element, 'valuebox');
        options.input.attr("readonly", true);
        $.each(options.states, function (name, value) {
            options.input.removeClass(value.cssClass);
        });
        if (state.text !== null && state.text !== undefined) {
            options.input.val(state.text);
            options.input.addClass(state.cssClass);
        }
        options.state = state;
        //imageLink 链接
        if (options.scanMode === false) {
            options.imageLink.attr("style", "cursor:text");
            options.imageLink.attr("href", "#");
            //            if (state == options.states.success) {
            //                options.imageLink.attr("href", "/" + options.typeName + "/Details/" + options.hiddenInput.val());
            //            }
            //            else {
            //                options.imageLink.attr("href", "#");
            //            }
        }
        //
        if (state == options.states.success) {
            if (options.onSuccessState) {
                options.onSuccessState(options.hiddenInput.val(), options.input.val());
            }
        }
        else if (state == options.states.empty) {
            if (options.onEmptyState) {
                options.onEmptyState(options.hiddenInput.val(), options.input.val());
            }
        }
        else if (state == options.states.loading) {
            if (options.onLoadingState) {
                options.onLoadingState(options.hiddenInput.val(), options.input.val());
            }
        }
        else if (state == options.states.notfound) {
            if (options.onNotfoundState) {
                options.onNotfoundState(options.hiddenInput.val(), options.input.val());
            }
        }
        else if (state == options.states.error) {
            if (options.onErrorState) {
                options.onErrorState(options.hiddenInput.val(), options.input.val());
            }
        }
    }
    /* dialog
    ---------------------------------------------------------------------------------*/
    function loadDialog(options) {
        var dialogID, _div, dialog;
        dialogID = options.input.attr("id") + '_dialog';
        if ($("#" + dialogID) > 0) { return; } //1.2
        _div = $("<div>")
                .attr("id", dialogID);
        dialog = _div.dialog({
            autoOpen: false,
            bgiframe: true,
            title: "请选择",
            width: 680,
            modal: true,
            //show: 'slide'
            buttons: {
                "关闭": function () {
                    $(this).dialog("close");
                },
                "不限": function () {
                    options.hiddenInput.val("");
                    options.textValue = "";
                    options.input.val("");
                    $(this).dialog("close");
                }
            }
        });
        $.post(
                options.getDataUrl,
                { gridOnly: false, type: options.typeName },
                function (data) {
                    _div.html(data);
                    _div.find("input#value").focus();
                    if (options.onFirstLoaded) {
                        options.onFirstLoaded();
                    }
                    options.dialog.dialog('option', 'position', 'center');
                });
        return dialog;
    }

    function dialog_td_click(e) {
        var td, target, options, input, tr, idHtml, textHtml, id, text;
        target = $(e.target);
        if (target.is("td")) { td = target; }
        else { td = $(target.parents("td")[0]); }

        if (td.children("a").length > 0) { return false; }
        options = e.options;
        input = e.options.input[0];
        tr = td.parent();
        idHtml = tr.find("td.idProperty").html();
        textHtml = tr.find("td.textProperty").html();

        if (idHtml && textHtml) {
            id = idHtml.replace(/<[^<>]*>/g, "").trim();
            text = textHtml.replace(/<[^<>]*>/g, "").trim();

            if (options.scanMode) {
                options.input.val(id);
            }
            else {
                options.hiddenInput.val(id);
                options.textValue = text;
                options.input.val(text);
                setState(input, options.states.success); //state
            }
        }
        else {
            alert("配置不正确");
        }
        options.dialog.dialog('close');
    }


    /*dropDownButtonClick
    ---------------------------------------------------------------------------------*/
    function dropDownButtonClick(e) {
        var options = e.options;
        if (options.readonly) { return; }
        if (e.options.typeFullName == "System_DateTime") {
            options.input.datepicker('show'); return;
        }
        if (options.dialog === null) { options.dialog = loadDialog(); }
        options.dialog.dialog('open');
        options.dialog.find("input#value").focus().select();
    }

    function keypress(e) {
        var input, value, type;
        input = e.target;
        value = e.target.value;
        type = $.data(input, 'valuebox').typeFullName;
        if (type == "System_String") { return true; }
        else if (type.match(/^System_((U?Int(64|32|16))|Single|Double|Decimal)$/)) {
            //允许数字
            if (e.keyCode >= 48 && e.keyCode <= 57) { return true; }
            //允许一个小数点
            if (type.match(/^System_(Single|Double|Decimal)/)) {
                if (e.keyCode == 46 && value.match(/[.]/) === null) { return true; }
            }
            //负号 切换
            if (type.match(/^System_((Int(64|32|16))|Single|Double|Decimal)$/)) {
                if (e.keyCode == 45) {
                    if (value.match(/-/) === null) { e.target.value = '-' + value; }
                    else { e.target.value = value.replace('-', ''); }
                }
            }
            return false;
        }
        return true;
    }

    function mouseup(e) {
        /*保持文本选择*/
        if (e.button === 1 || e.button === 0) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    }

    /* focus blur
    ---------------------------------------------------------------------------------*/
    function focus(e) {
        var input, options;
        input = e.target;
        options = $.data(input, 'valuebox');
        options.inputFocused = true;
        if (options.scanMode === true) { return; }
        options.textValue = options.input.val();
        options.input.val(options.hiddenInput.val()).select();
    }

    function blur(e) {
        var input, options, newIdValue;
        input = e.target;
        options = $.data(input, 'valuebox');
        options.inputFocused = false;
        if (options.scanMode === true) { return; }
        //        if (e.options.typeFullName == "System_DateTime") {
        //            e.options.dialog.dialog('hide');
        //        }
        newIdValue = options.input.val();
        if (newIdValue.length === 0) {
            options.hiddenInput.val("");
            setState(input, options.states.empty); //state
            return;
        }
        if (options.hiddenInput.val() != newIdValue) {
            setState(input, options.states.loading); //state
            options.hiddenInput.val(newIdValue);
            $.ajax({
                url: options.getTextUrl,
                type: 'POST',
                data: {
                    type: options.typeName,
                    id: newIdValue
                },
                success: function (data) {
                    if (data) {
                        options.hiddenInput.val(newIdValue);
                        options.textValue = data;
                        if (document.activeElement.id != input.id) {
                            options.input.val(options.textValue);
                        }
                        setState(input, options.states.success); //state
                    }
                    else {
                        ////////options.textValue = options.input.val();??????
                        setState(input, options.states.notfound); //state
                    }
                }, //successs
                error: function () {
                    /////options.textValue = options.input.val();??????
                    setState(input, options.states.error); //state
                }
            }); //ajax
        }
        else {
            options.input.val(options.textValue);
        }
    }


    function setOptions(element, newOptions) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        $.each(newOptions, function (key, value) { options[key] = value; });
    }

    function refreshState(index, element) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        setState(element, options.state);
    }

    function submitDialogForm(index, element) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        $("form", options.dialog).submit();
    }


    function submitParentForm(index, element) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        options.input.parents("form").submit();
    }

    function setValues(element, hiddenValue, value) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        options.hiddenInput.val(hiddenValue);
        if (options.inputFocused == false) {
            options.input.val(value);
        }
        else {
            options.input.val(hiddenValue);
            options.textValue = value;
        }
        setState(element, options.states.success);
    }

    function getDialog(element) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return null; }
        else { return options.dialog; }
    }

    function getValue(element, name) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return null; }
        if (name == "id") { return options.hiddenInput.val(); }
        else if (name == "name") { return options.input.val(); }
        return null;
    }

    /* handleItem
    ---------------------------------------------------------------------------------*/
    function handleItem(element, options) {
        var dialogID;
        //
        options.input = $(element);
        options.inputID = $(element).attr("id");
        options.parent = options.input.parent();
        options.hiddenInput = $("#" + options.input.attr("id") + options.hiddenInputNameSuffix, options.parent);
        options.readonly = options.input.attr("readonly");
        options.imageLink = $(".valuebox_img", options.parent);
        options.dropDownButton = $(".valuebox_button", options.parent);
        options.inputFocused = false;
        //
        options.typeFullName = options.input.attr("datatype");
        options.typeName = getTypeName(options.typeFullName);
        //

        //$.data(element, 'valuebox', options);
        //
        options.input.bind("keypress", keypress);
        //
        if (options.typeFullName.match(/^(System)?_/) && options.typeFullName != "System_DateTime") {
            return;
        }
        //
        options.dropDownButton.bind('click', function (e) { e.options = options; dropDownButtonClick(e); });
        //
        if (options.typeFullName == "System_DateTime") {
            if (options.readonly === false) { options.input.datepicker(); }
            return;
        }
        //
        options.dialog = loadDialog(options);
        dialogID = options.dialog.attr("id");
        $("#" + dialogID + " .dataTable tbody tr td")
            .die()//1.2
            .live("click", function (e) { e.options = options; dialog_td_click(e); });
        //
        options.textValue = options.input.val();
        if (options.textValue.length === 0) {
            setState(element, options.states.empty);
        }
        else {
            if (options.hiddenInput.val().length > 0) {
                setState(element, options.states.success);
            }
        }
        if (options.scanMode === true) {
            setState(element, options.states.success);
        }
        //
        options.input.bind("keypress", keypress);
        options.input.bind("mouseup", mouseup);
        options.input.bind("focus", focus);
        options.input.bind("blur", blur);
        //
    }

    $.fn.valuebox = function (args0, args1, args2) {
        if (typeof args0 === 'string') {
            if (args0 == 'options' && typeof arguments[1] == "object") {
                var newOptions = args1;
                return this.each(function () { setOptions(this, newOptions); });
            }
            if (args0 == "refreshState") { return this.each(refreshState); }
            else if (args0 == "submitDialogForm") { return this.each(submitDialogForm); }
            else if (args0 == "submitParentForm") { return this.each(submitParentForm); }
            else if (args0 == "setValues") { return this.each(function () { setValues(this, args1, args2); }); }
            else if (args0 == "getValue") {
                if (this.length > 0 && typeof args1 === "string") { return getValue(this[0], args1); }
            }
            else if (args0 == "getId") {
                if (this.length > 0) { return getValue(this[0], "id"); }
            }
            else if (args0 == "getName") {
                if (this.length > 0) { return getValue(this[0], "name"); }
            }
            else if (args0 == "getDialog") {
                if (this.length > 0) { return getDialog(this[0]); }
                return null;
            }
            else if (args0 == "find") {
                if (this.length > 0) {
                    return $(args1, getDialog(this[0]));
                }
            }
            return this;
        }
        else {
            return this.each(function () {
                if ($.data(this, 'valuebox') !== null && $.data(this, 'valuebox') !== undefined) { return; } // return if valueboxed
                var _options = $.extend({}, $.fn.valuebox.defaults, args0 || {});
                $.data(this, 'valuebox', _options);
                handleItem(this, _options);
            });
        }
    };

    $.fn.valuebox.defaults = {
        getDataUrl: "/Common/Query",
        getTextUrl: "/Common/GetText",
        hiddenInputNameSuffix: "_bid",
        states: {
            empty: { text: "", cssClass: "valuebox_empty" },
            notfound: { text: "", cssClass: "valuebox_notfound" },
            loading: { text: "加载中，请稍候...", cssClass: "valuebox_loading" },
            success: { cssClass: "valuebox_success" },
            error: { text: "", cssClass: "valuebox_error" }
        }
    };
})(jQuery);