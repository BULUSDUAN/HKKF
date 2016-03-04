/*!
* tooltip 1.0
* Copyright 2010, 济南捷诺软件技术有限公司
* 陆大鹏
* 2010年6月3日14:09:17
*/

/*
1.0 2010年6月3日
*/



(function ($) {

    function onhover(e) {
        var url, link, linkOffset, options, width, height, left, top, bodyWidth;
        link = e.target;
        options = $.data(link, 'tooltip');

        url = options.link.attr('href');
        if (url === "" || url === null || url === "#") {
            options.tooltip = null;
            return;
        }
        if (options.url != url && options.tooltip !== null) {
            options.tooltip.remove();
            options.tooltip = null;
        }
        options.url = url;

        if (options.tooltip === null) {
            options.tooltip = $("<div>")
                .addClass("tooltip")
                .append($("<div>").addClass("head"))
                .append($("<div>").addClass("body"))
                .appendTo('body');

            $.post(url, function (data) { options.tooltip.find('.body').html(data); });
        }

        linkOffset = options.link.offset();
        bodyWidth = $('body').outerWidth();

        width = options.tooltip.outerWidth();
        height = options.tooltip.outerHeight();

        left = linkOffset.left + options.link.outerWidth();
        if (left + width > bodyWidth) {
            left -= width - $(link).outerWidth();
        }

        top = linkOffset.top + link.offsetHeight;


        options.tooltip
            .css({ left: left, top: top }).stop().fadeIn().fadeIn();
    }

    function onleave(e) {

        var link, options;
        link = e.target;
        options = $.data(link, 'tooltip');

        if (options.tooltip) {
            options.tooltip.stop().delay(100).fadeOut().css({ opacity: "show" });
        }
    }

    function handleItem(element, options) {
        options.url = "";
        options.link = $(element);
        options.tooltip = null;
        options.link.hover(onhover, onleave);
    }

    function setOptions(element, newOptions) {
        var options = $.data(element, 'valuebox');
        if (options === null) { return; }
        $.each(newOptions, function (key, value) { options[key] = value; });
    }

    $.fn.tooltip = function (args0, args1, args2) {
        if (typeof args0 === 'string') {
            if (args0 == 'options' && typeof arguments[1] == "object") {
                var newOptions = args1;
                return this.each(function () { setOptions(this, newOptions); });
            }
            //if (args0 == "refreshState") { return this.each(refreshState); }
            return this;
        }
        else {
            return this.each(function () {
                if ($.data(this, 'tooltip') !== null && $.data(this, 'tooltip') !== undefined) { return; } // return if tooltiped
                var _options = $.extend({}, $.fn.tooltip.defaults, args0 || {});
                $.data(this, 'tooltip', _options);
                handleItem(this, _options);
            });
        }
    };

    $.fn.tooltip.defaults = {};
})(jQuery);