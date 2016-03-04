/*-------------------------------------------------------------------------------------------------------------------------------------------------------2010年10月25日 ldp 添加*/

/*统一处理批量问题 从服务器读取数据验证提示,验证后回到原界面-- 2010.12.06--------------------------------------------------*/
$(function () {
    $('button[name=UnityBtn]').live("click", function () {
        var btnid = $(this).attr("id");
        $("#subAction").attr("value", $(this).attr("value"));

        var checkboxs = new Array();
        $("input[name='ids']").each(function () {
            if ($(this).attr("checked")) {
                checkboxs.push($(this).val());
            }
        });

        if (checkboxs.length > 0) {
            if (confirm("确认是否操作？")) {
                $.post($("form").attr("action"), $('#' + btnid).parents("form").serialize(), function (data) {
                    if (data.IsSuccess !== undefined && data.isSuccess !== null) {
                        if (data.IsSuccess === true) {
                            $("input[name='ids']").each(function () {
                                if ($(this).attr("checked")) {
                                    tr = $(this).parents("tr");
                                    try {
                                        tr.css({ "height": tr.height(), 'padding': 0, 'margin': 0, 'border-width': 0 });
                                        tr.children("td").each(function () {
                                            $(this).css({ "padding": 0, 'border-width': 0 }).html("");
                                        });
                                        tr.animate({ 'height': 0 }, 100);
                                    }
                                    catch (err) {
                                    }
                                }
                            });
                            operationSuccessDialog.html("操作成功！");
                            operationSuccessDialog.dialog('open');
                            submitForm();
                        } else {
                            operationFailureDialog.html(data.Message);
                            operationFailureDialog.dialog('open');
                        }
                    }
                    else {
                        operationFailureDialog.html(data.Message);
                        operationFailureDialog.dialog('open');
                    }
                });
            }
        } else {
            alert("请选择要操作的数据！");
        }
        return false;
    });
});
/*统一处理批量问题 无服务器数据验证提示-- 2010.12.06--------------------------------------------------------------------------------------*/
$(function () {
    $('button[name=NationBtn]').live("click", function () {
        var btnid = $(this).attr("id");
        var checkboxs = new Array();
        $("input[name=ids]").each(function () {
            if ($(this).attr("checked"))
                checkboxs.push($(this).val());
        });
        if (checkboxs.length > 0) {
            if (confirm("确认是否操作？")) {
                operationSuccessDialog.html("操作成功！");
                operationSuccessDialog.dialog('open');
                submitForm();
            }
        } else {
            alert("请选择要操作的数据！");
        }
        return false;
    });
});
$(function () {
    $(".pager2 .first, .pager2 .prev, .pager2 .next, .pager2 .last")
            .live("click", function () {
                $("#subAction").attr("value", "");
                var page = $(this).text();
                gotoPage2(this, page);
            });
    $(".pager2 .refresh").live("click", function () {
        $("#subAction").attr("value", "");
        submitParentForm(this);
    });
    function gotoPage2(element, page) {
        $(element).parents(".pager2").find(".page").val(page);
        submitParentForm(element);
    }
    //分页 Ajsx 提交
    $("form.gridForm")
        .live("submit", function () {
            var $form, data;
            $form = $(this);
            $form.find('.pager2 .loading').show();
            $form.find('.pager2 .info').hide();
            data = $form.serialize();
            if (this.extraData) { data = this.extraData + data; }
            $.ajax({
                url: $(this).attr("action"),
                type: $(this).attr("method"),
                data: data,
                success: function (response) {
                    var gridDiv = $form.find(".grid");
                    if (gridDiv.length == 0) {
                        gridDiv = $form.parents(".grid:first");
                    }
                    gridDiv.html(response);
                    //stripTable();
                    $form.find('.pager2 .loading').hide();
                    $form.find('.pager2 .info').show();
                }
            });
            this.extraData = "";
            return false;
        });
});

/*------a.sortableLink  清空subAction值-----------------------------*/
$(function () {
    $("a.sortableLink").live("click", function () {
        $("#subAction").attr("value", "");
    });
   
//    $('.menu_top a').live("click", function () {        
//        var url = $(this).attr('href');
//        url = url.replace(/^.*#/, '');
//        $.history.load(url);        
//        return false;
//    });
});

/*----------打印-------------------------------------*/
function startPrint(obj) {
    var oWin = window.open("", "_blank");
    var strPrint = "";
    strPrint = strPrint + "<script type=\"text/javascript\">\n";
    strPrint = strPrint + "function printWin()\n";
    strPrint = strPrint + "{";
    strPrint = strPrint + "var oWin=window.open(\"\",\"_self\");\n";
    strPrint = strPrint + "oWin.document.write(document.getElementById(\"Content\").innerHTML);\n";
    strPrint = strPrint + "oWin.focus();\n";
    strPrint = strPrint + "oWin.document.close();\n";
    strPrint = strPrint + "oWin.print()\n";
    strPrint = strPrint + "oWin.close()\n";
    strPrint = strPrint + "}\n";
    strPrint = strPrint + "<\/script>\n";
    strPrint = strPrint + "<hr size='1' />\n";
    strPrint = strPrint + "<body onload=\"printWin()\">\n"
    strPrint = strPrint + "<div id=\"Content\">\n";
    strPrint = strPrint + obj.innerHTML + "\n";
    strPrint = strPrint + "</div>\n";
    strPrint = strPrint + "<hr size='1' />\n";    
    strPrint = strPrint + "</body>\n"
    oWin.document.write(strPrint);
    oWin.focus();
    oWin.document.close();
}
/*----------初始注册打印-------------------------------------*/
function startPrintFirst(obj) {
    var oWin = window.open("", "_blank");
    var strPrint = "";
    strPrint = strPrint + "<script type=\"text/javascript\">\n";
    strPrint = strPrint + "function printWin()\n";
    strPrint = strPrint + "{";
    strPrint = strPrint + "var oWin=window.open(\"\",\"_self\");\n";
    strPrint = strPrint + "oWin.document.write(document.getElementById(\"Content\").innerHTML);\n";
    strPrint = strPrint + "oWin.focus();\n";
    strPrint = strPrint + "oWin.document.close();\n";
    strPrint = strPrint + "oWin.print()\n";
    strPrint = strPrint + "oWin.close()\n";
    strPrint = strPrint + "}\n";
    strPrint = strPrint + "<\/script>\n";
    //strPrint = strPrint + "<hr size='1' />\n";
    strPrint = strPrint + "<body onload=\"printWin()\">\n"
    strPrint = strPrint + "<div id=\"Content\">\n";
    strPrint = strPrint + obj.innerHTML + "\n";
    strPrint = strPrint + "</div>\n";
    //strPrint = strPrint + "<hr size='1' />\n";
    strPrint = strPrint + "</body>\n"
    oWin.document.write(strPrint);
    oWin.focus();
    oWin.document.close();
}
/*-------------------------省教育证书打印---------------------------------*/
function startPrintEdu(obj) {
    var oWin = window.open("", "_blank");
    var strPrint = "";
    strPrint = strPrint + "<script type=\"text/javascript\">\n";
    strPrint = strPrint + "function printWin()\n";
    strPrint = strPrint + "{";
    strPrint = strPrint + "var oWin=window.open(\"\",\"_self\");\n";
    strPrint = strPrint + "oWin.document.write(document.getElementById(\"Content\").innerHTML);\n";
    strPrint = strPrint + "oWin.focus();\n";
    strPrint = strPrint + "oWin.document.close();\n";
    strPrint = strPrint + "oWin.print()\n";
    strPrint = strPrint + "oWin.close()\n";
    strPrint = strPrint + "}\n";
    strPrint = strPrint + "<\/script>\n";
    strPrint = strPrint + "<hr size='1' />\n";
    strPrint = strPrint + "<body onload=\"printWin()\">\n"
    strPrint = strPrint + "<div id=\"Content\">\n";
    strPrint = strPrint + obj.innerHTML + "\n";
    strPrint = strPrint + "</div>\n";
    strPrint = strPrint + "<hr size='1' />\n";
    strPrint = strPrint + "</body>\n"
    oWin.document.write(strPrint);
    oWin.focus();
    oWin.document.close();
}
/*------jquery windows.hash 实现浏览器的前进、后退和刷新-----------------------------*/
(function ($) {
    var origContent = "";
    function loadContent(hash) {
        if (hash != "") {
            if (origContent == "") {
                origContent = $('#main').html();
            }
            $('#main').load(hash);
        } else if (origContent != "") {
            $('#main').html(origContent);
        }
    }
    $(document).ready(function () {
        $.history.init(loadContent);
    });
 })(jQuery);

