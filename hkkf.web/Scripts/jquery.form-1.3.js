/*!
* jquery form 1.3.1
* Copyright 2010, 济南捷诺软件技术有限公司
* 陆大鹏
* 2010年9月10日
*/
/*
  2010年4月17日
  2010年4月28日 修改 getData
  2010年5月25日 增加 defaults
  2010年6月11日 增加 confirm
  2010年6月21日 修正删除在谷歌浏览器中删除的问题
  2010年9月10日 Form提交数据包含_additionalInput
*/
(function ($) {

    function getData(options) {
        var data;
        if (options.data === undefined || options.data === null) { data = ""; }
        else { data = options.data; }
        if (options.getData) { data += options.getData(); }
        return data;
    }

    function ajax(options) {
        var loadingElement;
        if (typeof options.type !== "string" || options.type.length === 0) { options.type = "POST"; }
        if (options.loadingElement) {
            loadingElement = $(options.loadingElement);
        }

        $.ajax({
            url: options.url,
            type: options.type,
            data: getData(options),
            beforeSend: function () {
                if (loadingElement) { loadingElement.show(); }
                if (options.beforeSend) { options.beforeSend(); }
            },
            success: function (data) {
                var target = $(options.updateTarget);
                if (options.updateTarget) {
                    if (options.insertionMode == "InsertBefore") { $(data).prependTo(target); }
                    else if (options.insertionMode == "InsertAfter") { $(data).appendTo(target); }
                    else { target.html(data); }
                }
                if (options.success) { options.success(); }
            },
            complete: function (data) {
                if (loadingElement) { loadingElement.hide(); }
                if (options.complete) { options.complete(); }
                if (options.inputToFocus) { $(options.inputToFocus).focus().select(); }
            }
            //,
            //            error: function(XMLHttpRequest, textStatus, errorThrown) {
            //                if (options.error) options.error();
            //            }
        });
    }
    function ajaxWithConfirm(options) {
        if (typeof options.confirm !== 'string') {
            ajax(options);
            return;
        }
        var $this = $(options.element);
        //$this.unbind('click');
        //$this.bind("click", function (e) {
        tr = $(options.element).parents("tr:first");
        tr.addClass("deleteHighlight");
        $("<div title='提示'></div>")
                .html(options.confirm)
                .dialog({
                    modal: true,
                    buttons: {
                        "取消": function () { $(this).dialog('destroy').remove(); tr.removeClass("deleteHighlight"); },
                        "确定": function () { $(this).dialog('destroy').remove(); ajax(options); }
                    }
                });
        tr.addClass("deleteHighlight");
        //});
    }


    $.fn.ajaxForm = function (options) {
        return this.each(function () {
            var _options, form;
            _options = $.extend({}, $.fn.ajaxForm.defaults, options || {});
            if (_options.precondition && _options.precondition() !== true) { return; }
            form = $(this);
            _options.element = this;
            _options.url = form.attr("action");
            _options.type = form.attr("method");
            if ((_options.data === null || _options.data === undefined) && (_options.getData === null || _options.getData === undefined)) {
                _options.data = form.serialize();
                var addtionalInput = form.attr("_additionalInput");
                if (addtionalInput !== null) {
                    if (!_options.data) { _options.data = addtionalInput; }
                    else { _options.data += "&" + addtionalInput; }
                }
            }
            ajax(_options);
        });
    };

    $.fn.ajaxLink = function (options) {
        return this.each(function () {
            var _options, link, href;
            _options = $.extend({}, $.fn.ajaxLink.defaults, options || {});
            if (_options.precondition && _options.precondition() !== true) { return; }
            link = $(this);
            href = link.attr("href");
            if (href !== null && href !== "#" && href !== "") {
                _options.url = href;
            }
            _options.element = this;
            ajaxWithConfirm(_options);
        });
    };

    $.fn.ajaxForm.defaults = {};

    $.fn.ajaxLink.defaults = {};

})(jQuery);