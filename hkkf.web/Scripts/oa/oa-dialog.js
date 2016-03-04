
var formParent = null;
$(function () {

    $("[data-opt]").live("click", function () {
        var _thisOpt = $(this).attr("data-opt");
        var _thisHref = $(this).attr("href");
        formParent = $(this).closest("form");
        if (_thisOpt == "delete") {
            var _msg = $(this).attr("data-msg");
            if (confirm(_msg) == true) {
                $.ajax({
                    url: _thisHref,
                    type: "POST",
                    dataType: "JSON",
                    success: function (data) {
                        data = $.parseJSON(data);
                        alert(data.message);
                        if (data.state == true) { 
                            if (formParent != null) {
                                againParentForm(formParent);
                                formParent = null;
                            }
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("操作失败--" + XMLHttpRequest.status + "--" + textStatus);
                    }
                })
            }
        } else {
            //add detail
            $.ajax({
                url: _thisHref,
                success: function (html) {
                    var wd = $(window).width() * 70 / 100;
                    FormDialog({
                        width: wd,
                        html: html,
                        opt: _thisOpt
                    });

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("操作失败--" + XMLHttpRequest.status + "--" + textStatus);
                }
            });
        }

        return false;

    })

    //表单提交
    $("form.operatInfoDiv").live("submit", function () {
        var form = $(this);
        $.ajax({
            url: form.attr("action"),
            data: form.serialize(),
            type: "POST",
            dataType: "JSON",
            success: function (data) {
                data = $.parseJSON(data);
                alert(data.message);
                if (data.state == true) {
                    $("#formTable").dialog('close');
                    if (formParent != null) {
                        againParentForm(formParent);
                        formParent = null;
                    }
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("操作失败--" + XMLHttpRequest.status + "--" + textStatus);
            }
        })

        return false;
    })


})



/*
操作弹出层
option:参数集合
*/
function FormDialog(option) {
    //$("#formTable").remove();
    var _formTable = $("#formTable");
    if (_formTable.length == 0) {
        _formTable = $("<div id='formTable' style='padding-bottom:30px;' />")
    }
    if (typeof option.title == "undefined") option.title = "操作";
    if (typeof option.width == "undefined") option.width = 700;
    var info = {
        title: option.title,
        width: option.width,
        isInitialize: true //自定义是否初始化原配置
    };
    if (option.opt == "add") {
        info.buttons= { 
                "取消": function () {
                    $(this).dialog("close"); 
                },
                  "确定": function () {
                    _formTable.find("form.operatInfoDiv").submit();
                }
          }
 
    } else {
            info.buttons = {
                "关闭": function () {
                    $(this).dialog("close");
                }
            }
    }

    var dgParam = CommonDialogParam(info, 200);

    _formTable
        .html(option.html)
        .dialog(dgParam)

    uiWidgetOverlay(_formTable, 200);
}


/*
Dialog
dtTime:等待时间
*/
function CommonDialogParam(option, dtTime) {
    if (typeof dtTime == "undefined") dtTime = 300;
    var opt = {
        autoOpen: true,
        modal: true,
        closeText: "关闭",
        show: {
            effect: "scale",
            duration: dtTime
        },
        hide: {
            effect: "scale",
            duration: dtTime
        },
        close: function () {
            $(this).dialog('destroy');
            $(this).remove(); 
        }
    };
    opt = $.extend(opt, option);
    return opt;
}

/*
重写ui-widget-overlay阴影层z-index大小  最上层等于下一层的z-index   dtTime:等待时间
*/
function uiWidgetOverlay($ID, dtTime) {
    if (typeof dtTime == "undefined") dtTime = 300;
    dtTime = dtTime + 20;
    //由于生成ui-widget-overlay阴影层需要300ms 所以时间定为>300  300是显示的时候自己定义的
    setTimeout(function () {
        var _uiDg = $ID.closest(".ui-dialog");
        var zIndex = parseInt(_uiDg.css("z-index")) - 1;
        _uiDg.next(".ui-widget-overlay.ui-front").css({ "z-index": zIndex });
    }, dtTime)
}

//重新加载父页面
function againParentForm(formParent) {
    $.ajax({
        url: formParent.attr("action"),
        data: formParent.serialize(),
        type: "POST",
        success: function (html) {
            $("#main").html(html);
        }
    })
}