
//2010年5月28日15:15:02
/// <reference name="MicrosoftAjax.js"/>
var tr_over_backColor, tr_odd_backColor, tr_even_backColor, operationSuccessDialog, operationFailureDialog,
        slCtl, tempMessage, confirmDeleteDiv, confirmDeleteMessage, autoShowDetailsInfoDialog,lookdetailDiv,lookdetailContent;





//初始化  
// totalPage: 是否初始化整个页面（否则初始局部）
function init(totalPage)
{    
    stripTable();
    valuebox();
    $(".loading").hide();
    if(totalPage === true){
        //activeMainMenu();
        //leftTreeEngineers();
        //leftTreeLimits();
        detailsDiv = $("<div id='detailsDiv'>");
    }
}

$.fn.ajaxForm.defaults.complete = function(){
    init(false);
}

$.ajaxSetup({
    cache: false,
    //    success: function()    {
    //    },
    complete: function (xMLHttpRequest, textStatus) {
        init(false);
        //$(
    },
    error: function (xhr, textStatus, errorThrown) {
        var location, errorDiv, errorDialog;
        if (xhr.status == 403) {
            location = xhr.getResponseHeader("Location");
            if (location) {
                window.location.replace(location);
                return;
            }
        }
        errorDiv = $('<div>')
                .attr("title", 'Error: ' + xhr.status)
                .html(xhr.statusText);

        errorDialog = errorDiv.dialog({
            modal: true,
            buttons: {
                "确定": function () {
                    $(this).dialog('destroy');
                },
                "查看": function () {
                    errorDiv.html(xhr.responseText.replace(/<style(.|\s)*?\/style>/gi, ""));
                    errorDialog
                    .dialog('option', 'width', 800)
                    .dialog('option', 'height', 600)
                    .dialog('option', 'buttons', { "确定": function () { $(this).dialog('destroy'); } })
                    .dialog('option', 'position', 'center');
                }
            }
        });
    }
});

$(function () {
    init(true);
    liveFrameLikeLink();    
    liveActionButtonForm();
    liveActionSubmitButton();
});
$(function () {
    //    /* 一级菜单标题，点击时 折叠或展开开部二级菜单切换
    //    如有展开的，首次点击时折叠
    //    --------------------------------------------------------------------------------*/
    //    $(".menu_top > p").bind("click", function () {
    //        var menuLevel2 = $(this).parents(".menu_top:first").find("ul > li > ul");
    //        if (menuLevel2.find(":visible").length > 0) {
    //            menuLevel2.slideUp();
    //        }
    //        else {
    //            menuLevel2.slideDown();
    //        }
    //    });
    //    /* 隐藏除第一个之外的二级菜单明细
    //    --------------------------------------------------------------------------------*/
    //    $(".menu_top > ul > li > ul:gt(0)").hide();
    //    /* 二级菜单标题，点击时折叠、展开切换
    //    --------------------------------------------------------------------------------*/
    //    $(".menu_top > ul > li > p").bind("click", function () {
    //        $(this).next().slideToggle();
    //    });

    /* 隐藏除第一个之外的二级菜单明细
    --------------------------------------------------------------------------------*/
    $(".menu_top > ul > li > ul:gt(0)").hide();
    /* 二级菜单标题，点击时折叠、展开切换
    --------------------------------------------------------------------------------*/
    $(".menu_top > ul > li > p").bind("click", function () {
        $(this).parents(".menu_top:first").find("ul > li > ul").slideUp();
        var menuLevel2 = $(this).parent().find("ul");
        if (menuLevel2.find(":visible").length > 0) {
            menuLevel2.slideUp();
        }
        else {
            menuLevel2.slideDown();
        }
    });

});

function changeLink(sth) {
    var mainlink = $("a.mainlink");
    for (var i = 0; i < mainlink.length; i++) {
        $(mainlink[i]).removeClass("mainlink");
    }
    $(sth).addClass("mainlink");
}

function liveFrameLikeLink()
{
    /* 若 a标识 target 属性值以#开始
    则将其指向的页面，在id为 #后部分的html元素中展示
    此操作大大简化了Ajax调用，实现框架页面的
    --------------------------------------------------------------------------------*/
    $("a[target^=#]").live("click", function () {
        var targetSelector = $(this).attr("target")
        $.ajax({
            url: $(this).attr("href"),
            success: function(data){ 
                $(targetSelector).html(data);
             }
        });
        return false;
    });
}

function liveActionButtonForm()
{
    $(function(){
        $('form.actionButtonForm').live('submit', function(){
            var success = $(this).attr('success');
            $.ajax({
                url: $(this).attr('action'),
                type: 'post',
                success: function(result){ 
                     if (result.IsSuccess === true && success !== null){
                        eval(success);
                     }
                    handleOperationResult(result); 
                }
            });
            return false;
        });
    });
}
    //    $(".mainmenu a, a.menuItem").bind("click", function () {
    //        $("#mainContent").load($(this).attr("href"));
    //        return false;
//    });


function liveActionSubmitButton()
{
    $('input[type=submit].actionSubmitButton').live('click', function(){
        var name = $(this).attr("name");
        if(name !== null) {
            var value = $(this).val();
            $(this).parents("form:first").attr("_additionalInput", name+"="+value);
        }
    });
}

/* 2010年9月16日 ldp */
$(function(){
    $('input[type=submit].ajaxActionSubmitButton').live('click', function(){
        var form = $(this).parents("form:first");
        var name = $(this).attr("name");
        var additionalInput = "";
        if(name !== null) {
            var value = $(this).val();
            additionalInput = name+"="+value;
        }
        var type = form.attr("type");
        if(type === undefined || type === ""){
         type = "post";}
        $.ajax({
            url: form.attr("action"),
            data: form.serialize() + "&" + additionalInput,
            type: type,
            success: function(data){
                $($(this).attr("updateTarget")).html(data);
            }
        });
        return false;
    });
});

/*自动显示明细信息窗口*/
autoShowDetailsInfoDialog = true;

tr_over_backColor = "#eef";
tr_odd_backColor = "#f8f8f8";
tr_even_backColor = "#fff";

/* 提交窗体*/
function submitForm($form) {
    $form.submit();
}

function submit(id) {
    submitForm($("#" + id));
}

function submitParentForm(element) {
    var $form = $(element).parents("form");
    $form.find(".loading").show();
    submitForm($form);
}

function submitSelForm(element) {
    var $form = $(element).parents("form");
    submitForm($form);
}

function valuebox() {
    $("input[type=text].valuebox").each(function (index, element) {
        var serviceUrl, scanMode;
        serviceUrl = $(element).attr("serviceUrl");
        scanMode = $(element).hasClass("scanMode");
        if (typeof serviceUrl === "string") {
            $(element).valuebox({ scanMode: scanMode, getDataUrl: serviceUrl, getTextUrl: serviceUrl.replace("Query", "GetText") });
        }
        else {
            $(element).valuebox({ scanMode: scanMode });
        }
    });
}


//infoTable td 列宽相等
function averageInfoTableTdWidth() {
    $("table.infoTable").each(function (index, element) {
        var table, tr, th, td;
        table = $(element);
        tr = table.find("tr:first");
        th = tr.children("th");
        td = tr.children("td");
        td.width((table.width() - th.width() * th.length) / td.length);
    });
}

function stripTable() {
    //    $("tbody > tr:even").css("background-color", tr_even_backColor)
    //    $("tbody > tr:odd").css("background-color", tr_odd_backColor)

    // table 在此项目中用作布局，需禁用以下两行
    //$("tbody").find("tr:even").css("background-color", tr_even_backColor);
    //$("tbody").find("tr:odd").css("background-color", tr_odd_backColor);

}

function gradualChangeBkColor($this, color, ms) {
    try {
        if (ms === null) { ms = 800; }
        $this.stop().animate({ "background-color": color }, ms);
    } catch (err) { }
}

function gotoPage(element, page) {
    $(element).parents(".pager").find(".page").val(page);
    submitParentForm(element);
}



function submit2(form, target, inputToFocus) {
    //stripTable();
    var $form = $(form);
    $.ajax({
        url: $form.attr("action"),
        type: $form.attr("method"),
        data: $form.serialize(),
        success: function (data) {
            $(target).html(data);
            //            var dialog = $('.validation-summary-errors').dialog({
            //                modal: true,
            //                title: "错误",
            //                buttons: { "确定": function() { $(this).dialog('close').remove(); $(inputToFocus).focus().select(); } }
            //            });            
            $(form).find(".loading").hide();
            //stripTable();
            //            if (dialog.length === 0)
            $(inputToFocus).focus().select();
        }
    });
    return false;
}

function handleOperationResult(result) {
    if (result) {
        if (result.IsSuccess === true) { operationSuccessDialog.dialog('open'); }
        else { operationFailureDialog.html(result.Message); operationFailureDialog.dialog('open'); }
    }
}

function deleteFun(a) {
    var url, tr;
    url = $(a).attr("href");
    $.post(url, function (data, textStatus) {
        if (data.IsSuccess !== undefined && data.isSuccess !== null) {
            if (data.IsSuccess === true) {
                tr = $(a).parents("tr");
                try {
                    tr.css({ "height": tr.height(), 'padding': 0, 'margin': 0, 'border-width': 0 });
                    tr.children("td").each(function () {
                        $(this).css({ "padding": 0, 'border-width': 0 }).html("");
                    });
                    tr.animate({ 'height': 0 }, 100, function () { tr.parents("form").submit(); });
                }
                catch (err) {//alert(err);//tr.hide();
                    tr.parents("form").submit();
                }                
            }
            else {
                operationFailureDialog.html(data.Message);
                operationFailureDialog.dialog('open');
            }
        }
        else {
            operationFailureDialog.html(data);
            operationFailureDialog.dialog('open');
        }
    });
}


function getChineseSpell(source, target) {
    $("#" + source).blur(
        function () {
            $.post("/Common/GetChineseSpell", { str: $(this).val() }, function (data) { $("#" + target).val(data); });
        });
}

function getAuto(source, target) {
        $("#" + source).change(
        function () {
            $.post("/Common/getAuto", { str:$("#" + source).val()}, function (data) { 
                 $("#" + target).empty();
                 $.each(data,function(i,n){  
                    $("#" + target).append("<option value='"+i+"'>"+n+"</option>");
                 });
             });
        });
}

function DropDownListInteract(source, target,url) {
        $("#" + source).change(
        function () {
            $.post(url, { engineerTypeStr:$("#" + source).val()}, function (data) { 
                 $("#" + target).empty();
                 if($("#" + source).val()==""){
                  $("#" + target).append("<option value=''>全部</option>");
                 }
                 $.each(data,function(i,n){  
                    $("#" + target).append("<option value='"+i+"'>"+n+"</option>");
                 });
             });
        });
}

//针对继续教育 监理工程师 下拉选单级联
function DropDownListShow(source, target) {
        $("#" + source).change(function () {
             if("6"==$("#" + source).val()){
                 $("#" + target).show();
             }else{
                 $("#" + target).hide();
             }
        });
}

//针对继续教育 未结业原因 其它原因显示
function TextBoxShow(source, target) {
        $("#" + source).change(function () {
             if("4"==$("#" + source).val()){
                 $("#" + target).show();
             }else{
                 $("#" + target).hide();
             }
        });
}

//针对材料省后台 显示材料补正内容
function MaterialContentShow(source, target) {
        $("#" + source).change(function () {
             if("16"==$("#" + source).val()){
                 $("#" + target).show();
             }else{
                 $("#" + target).hide();
             }
        });
}

// 给全选按钮添加全选功能
$(function () {
  $("#btnAllCheck").live('click', function () {
        $("input[name='ids']").each(function () {
            $(this).attr('checked', true);
        });
    });
});

// 给取消按钮添加取消功能
$(function () {
    $("#btnAllCancel").live('click', function () {
        $("input[name='ids']").each(function () {
            $(this).attr('checked', false);
        });
    });
});

// 给反选按钮添加反选功能
$(function () {
    $("#btnAllOther").live('click', function () {
        $("input[name='ids']").each(function () {
            if ($(this).attr("checked"))
                $(this).attr('checked', false);
            else
                $(this).attr('checked', true);
        });
    });
});

//弹出提示消息
function showAlertMessage() {   
    var alertMessageContainer = $(".alertMessageContainer");
    var message = $(".alertMessageContainer").html();
    if (message !== null && message.length > 0) {
       operationSuccessDialog.html(message);
       operationSuccessDialog.dialog('open');
       alertMessageContainer.remove();   
        //   alertMessageContainer
        //    .dialog({modal: true, buttons: { "确定": function () { $(this).dialog('close');} }});
     }
  }

  function batchMsg(btn) {
      $('#' + btn).click(function () {
          var checkboxs = new Array();
          $("input[name='ids']").each(function () {
              if ($(this).attr("checked")) {
                  checkboxs.push($(this).val());
              }
          });
          if (checkboxs.length > 0) {
              if (confirm("确认是否操作？") == true) {
                  return true;
              }
              else {
                  return false;
              }
          } else {
              alert("请选择要操作的数据！");
              return false;
          }
      });
  }
// 给全选按钮添加全选功能
function allCheck(btn, groupId) {
    $('#' + btn).click(function () {
        $("input[name='" + groupId + "']").each(function () {
            $(this).attr('checked', true);
        });
    });
}

// 给取消按钮添加取消功能
function allCancel(btn, groupId) {
    $('#' + btn).click(function () {
        $("input[name='" + groupId + "']").each(function () {
            $(this).attr('checked', false);
        });
    });
}

// 给反选按钮添加反选功能
function allOther(btn, groupId) {
    $('#' + btn).click(function () {
        $("input[name='" + groupId + "']").each(function () {
            if ($(this).attr("checked")) {
                $(this).attr('checked', false);
            } else {
                $(this).attr('checked', true);
            }
        });
    });
}

//删除
function operate(btn, groupId,url) {
    $('#' + btn).click(function () {
        var checkboxs = new Array();
        $("input[name='" + groupId + "']").each(function () {
            if ($(this).attr("checked")) {
               checkboxs.push($(this).val());
            }
            
        });
        if (checkboxs.length > 0) {
            if(confirm("确认是否操作？")){
                $.post(url, $("#"+btn).parents("form").serialize(), function (data) {
                   if (data.IsSuccess !== undefined && data.isSuccess !== null) {
                        if (data.IsSuccess === true) {
                            $("input[name='" + groupId + "']").each(function () {
                                if ($(this).attr("checked")) {
                                   tr = $(this).parents("tr");
                                    try {
                                        tr.css({ "height": tr.height(), 'padding': 0, 'margin': 0, 'border-width': 0 });
                                        tr.children("td").each(function () {
                                            $(this).css({ "padding": 0, 'border-width': 0 }).html("");
                                        });
                                        tr.animate({ 'height': 0 }, 500);
                                    }
                                    catch (err) {//alert(err);//tr.hide();
                                        //tr.parents("form").submit();
                                    }
                                }
                            });
                            operationSuccessDialog.html("操作成功！");
                            operationSuccessDialog.dialog('open');
                            submitForm();
                        }else {
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
}

//批量审核
function batchCheck(btn, groupId,url,i,msg) {
    $('#' + btn).click(function () {
        var checkboxs = new Array();
        $("input[name='" + groupId + "']").each(function () {
            if ($(this).attr("checked")) {
               checkboxs.push($(this).val());
            }
            
        });
        if (checkboxs.length > 0) {
            if(confirm("确认是否操作？")){
                $.post(url, $("#"+btn).parents("form").serialize(), function (data) {
                   if (data.IsSuccess !== undefined && data.isSuccess !== null) {
                        if (data.IsSuccess === true) {
                            $("input[name='" + groupId + "']").each(function () {
                                if ($(this).attr("checked")) {
                                   tr = $(this).parents("tr");
                                    try {
                                        //tr.css({ "height": tr.height(), 'padding': 0, 'margin': 0, 'border-width': 0 });
                                        //tr.children("td").each(function () {
                                        tr.find("td").eq(i).html(msg);
                                        //});
                                        //tr.animate({ 'height': 0 }, 500);
                                    }
                                    catch (err) {
                                        //tr.parents("form").submit();
                                    }
                                }
                            });
                            operationSuccessDialog.html("操作成功！");
                            operationSuccessDialog.dialog('open');
                        }else {
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
            return false;
        }
        
    });
}


function openDialog(dia,btn){
    $("#"+dia).dialog({
        autoOpen: false,//如果设置为true，则默认页面加载完毕后，就自动弹出对话框；相反则处理hidden状态。 
        bgiframe: true, //解决ie6中遮罩层盖不住select的问题  
        width: 400,
        height:400,
        modal:true,//这个就是遮罩效果   
        resizable:false,
        dialogClass: 'alert' 
     
    });
    
    $("#"+btn).click(function(){
        $("#"+dia).dialog('open');
        return false;
    });

}

function actionButtons_SaveCreate(formId) {
    var hiddenInput = $('<input>').attr('id', 'redirectToCreate').attr('name', 'redirectToCreate').attr('type', 'hidden').attr('value', 'true');
    $('#' + formId).append(hiddenInput).submit();
}


 
//
//$.fn.ajaxForm.defaults = { complete: ajaxComplete };
//$.fn.ajaxLink.defaults = { complete: ajaxComplete };

// 修改 valuebox 默认值
$.fn.valuebox.defaults.notEnterStopFilter = '[id=Remark]';

//执行 MicrosoftMvcValidation 客户端验证，验证通过返回True
function msMvcValidate(formElement) {
    var e, formContext;
    e = $.Event('submit');
    formContext = Sys.Mvc.FormContext.getValidationForForm(formElement); //formElement[Sys.Mvc.FormContext._formValidationTag];
    formContext.$E(e);  //formContext._form_OnSubmit(e);
    return !e.isDefaultPrevented();
}

$(function () {
    /* 解决 jQuery 提交不触发 MicrosoftMvcValidation 事件 -------------------------------------------------------------------------------*/
    $("form").bind("submit", function (e) {
        var formContext = Sys.Mvc.FormContext.getValidationForForm(this); //this[Sys.Mvc.FormContext._formValidationTag]; 
        if (formContext !== null && formContext !== undefined) {
            formContext.$E(e); // formContext._form_OnSubmit(e);
        }
    });
    $(document).ajaxSuccess(function (event, XMLHttpRequest, ajaxOptions) {
        stripTable();
        valuebox();
        showAlertMessage();
    });
    /*stripTable--------------------------------------------------------------------------------*/
    /*stripTable--------------------------------------------------------------------------------*/
    stripTable();
    /*accordion--------------------------------------------------------------------------------*/
    //$("#accordion").accordion();
    /*queryButton--------------------------------------------------------------------------------*/
    $(".queryButton, .submitLink").bind("click", function () {
        var form = $(this).parents("form:first");
        $("#QueryInfo_Page").val('1');
        form.submit();
        return false;

    });
    /*表格颜色渐变--------------------------------------------------------------------------------*/
    $(".dataTable tbody > tr, .infoTable tbody td")
        .live("mouseover", function () { gradualChangeBkColor($(this), tr_over_backColor); });
    $(".dataTable tbody > tr:odd, .infoTable tbody tr:odd td")
        .live("mouseout", function () { gradualChangeBkColor($(this), tr_odd_backColor); });
    $(".dataTable tbody > tr:even, .infoTable tbody tr:even td")
        .live("mouseout", function () { gradualChangeBkColor($(this), tr_even_backColor); });
    /*表格颜色渐变--------------------------------------------------------------------------------*/
    //$("th").textShadow({ x: 2, y: 2, radius: 2, color: "#000" });
    /*button--------------------------------------------------------------------------------*/
    $('.button,.button4,.button6,.button8,.button10,.operatefun, .actionButtons a')
            .live("mouseover", function () { gradualChangeBkColor($(this), "#8bb0ff"); })
            .live("mouseout", function () { gradualChangeBkColor($(this), "#f4f4f4"); });
    /*日期控件设置区域--------------------------------------------------------------------------------*/
    $.datepicker.setDefaults({
        //showMonthAfterYear: true,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        numberOfMonths: 1,
        showCurrentAtPos: 0,
        constrainInput: true,
        ///gotoCurrent: true,
        hideIfNoPrevNext: true,
        minDate: new Date(1940, 1 - 1, 1),
        yearRange: '1940:2040'
        //showOn: 'none'
        //showOtherMonths: true
    });
    /* valuebox--------------------------------------------------------------------------------*/
    valuebox();
    /*回车提交--------------------------------------------------------------------------------*/
    $('input.submitOnEnterKeyPress').live("keydown", function (e) {
        if (e.keyCode == 13) {
            submitParentForm(this);
            return false;
        }
    });
    $('input.tabOnEnterKeyPress').live("keydown", function (e) {
        var inputs, idx;
        if (e.keyCode != 13) { return; }
        inputs = $(this).parents("form").eq(0).find(":input:visible");
        idx = inputs.index(this);

        if (idx == inputs.length - 1) {
            inputs[0].select();
        } else {
            try {
                inputs[idx + 1].focus(); //  handles submit buttons
                inputs[idx + 1].select();
            } catch (ex) { }
        }
        return false;
    });
    /*分页--------------------------------------------------------------------------------*/
    $(".pager input")
        .live("keydown", function (event) {
            if (event.keyCode >= 96 && event.keyCode <= 105) { return true; } //小键盘数字
            if (event.keyCode == 37 || event.keyCode == 39) { return true; } // 左箭头、右箭头
            if (event.keyCode >= 48 && event.keyCode <= 57) { return true; }
            if (event.keyCode == 8) { return true; } //后退
            if (event.keyCode == 13) {
                submitParentForm(this);
            }
            return false;
        });
    /*每页数量修改后 ，马上提交*/
    /* IE 不支持 live change, 事件直接写入了分页控件中
    $("#pager select")
    .live("change", function() {
    submitParentForm(this);
    });*/
    $(".pager .first, .pager .prev, .pager .next, .pager .last")
            .live("click", function () {
                $("#subAction").attr("value", "");
                var page = $(this).text();
                gotoPage(this, page);
            });
    $(".pager .refresh").live("click", function () {
        $("#subAction").attr("value", "");
        submitParentForm(this);
    });
    //分页 Ajsx 提交
    $("form.gridForm")
        .live("submit", function () {
            var $form, data;
            $form = $(this);
            $form.find('.pager .loading').show();
            $form.find('.pager .info').hide();
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
                    $form.find('.pager .loading').hide();
                    $form.find('.pager .info').show();
                }
            });
            this.extraData = "";
            return false;
        });
    //分页中的输入控 自动提交
    $(".autosubmit input[type=text]").live("keyup", function () {
        submitParentForm(this);
    });
    $(".autosubmit input[type=checkbox]").live("change", function () {
        submitParentForm(this);
    });
    /*删除--------------------------------------------------------------------------------*/
    confirmDeleteDiv = $("#confirmDelete");
    if (confirmDeleteDiv.length === 0) {
        confirmDeleteDiv = $("<div id='confirmDeleteDiv' title='提示'>确认删除 {0}？</div>");
    }
    confirmDeleteMessage = confirmDeleteDiv.html();
    confirmDeleteDiv.dialog({ modal: true, bgiframe: true, autoOpen: false });
    $("a.delete").live("click", function (e) {
        var a, tr;
        a = this;
        tr = $(a).parents("tr:first");
        confirmDeleteDiv.html(confirmDeleteMessage.replace("{0}", a.title));
        confirmDeleteDiv.dialog("option",
        {
            buttons: {
                "取消": function () { $(this).dialog("close"); tr.removeClass("deleteHighlight"); },
                "确定": function () { deleteFun(a); $(this).dialog("close"); }
            }
        });
        tr.addClass("deleteHighlight");
        confirmDeleteDiv.dialog('open');
        return false;
    });
    /*操作成功窗口--------------------------------------------------------------------------------*/
    operationSuccessDialog = $("<div>")
    .attr("title", "消息")
    .html("操作成功！")
    .dialog({
        autoOpen: false,
        modal: true,
        minWidth: 300,
        buttons: { "确定": function () { $(this).dialog('close'); } }
    });
    /*删操作失败窗口--------------------------------------------------------------------------------*/
    operationFailureDialog = $("<div>")
    .attr("title", "操作失败")
    .dialog({
        autoOpen: false,
        modal: true,
        minWidth: 300,
        buttons: { "确定": function () { $(this).dialog('close'); } }
    });
    /*focus--------------------------------------------------------------------------------*/
    if ($(".focus:first").length == 1) {
        $(".focus:first").focus();
    }
    else {
        $(":input:first:[type!=hidden]").focus().select();
    }
    /*sortableLink 2010年7月21日 */
    $("a.sortableLink").live("click", function () {
        var form = $(this).parents("form:first");
        if (form.length == 0) {
            return;
        }
        $("#QueryInfo_OrderBy", form).val($(this).attr('orderBy'));
        $("#QueryInfo_IsDesc", form).val($(this).attr('isDesc'));
    });

    /*-------------------------------------------------------------------------------------*/
    averageInfoTableTdWidth();
    /* 显示tempMessage --------------------------------------------------------------------*/
    tempMessage = $(".tempMessage");
    if (tempMessage.length > 0 && tempMessage.html().length > 0) {
        tempMessage
            .css("left", ($(document).width() - tempMessage.width()) / 2 + "px")
            .show(100)
            .fadeOut(5000);
    }
    /* # link--------------------------------------------------------------------------------*/
    //    $("a[href=#]").live("click", function () { return false; });   
    $("a.valuebox_img").live("click", function () { return false; }); /*ie6下[href=#]识别不到*/
    /*-------------------------------------------------------------------------------------------*/
    $(".showDetailsInfoLinkContainer")
        .live("mouseenter", function () { $(this).children(".showDetailsInfoLink").css("visibility", "visible"); })
        .live("mouseleave", function () { $(this).children(".showDetailsInfoLink").css("visibility", "hidden"); });
    $(".showDetailsInfoLink").live("click", function () { showModelInfo($(this).attr('href'), true); return false; });
});

$(function () {
    $("table thead th a")
        .live("click", function () {
            var url, form;
            url = $(this).attr("href");
            form = $(this).parents("form")[0];
            form.extraData = url.match(/[^?]*$/)[0] + "&";
            submitForm($(form));
            return false;
        });
});

function showModelInfo(url, openIfClose) {
    var div = $("#domainModelInfoDiv");
    if (div.length == 0) {
        div = $("<div>")
                .attr("id", "domainModelInfoDiv");
    }
    if ($("#domainModelInfoDiv").dialog("widget").length > 0) {
        if ($("#domainModelInfoDiv").dialog("isOpen") == false && openIfClose === true) {
            $("#domainModelInfoDiv").dialog("open");
        }
    }
    else {
        div.dialog({ width: 520, height: 320, position: ['right', 'top'], autoOpen: autoShowDetailsInfoDialog });
    }
    if ($("#domainModelInfoDiv").dialog("isOpen")) {
        $.post(url, function (data) {
            if (typeof data === 'string' && data.length > 0) {
                div.html(data);
                div.dialog("option", "title", div.children("div:first").attr("title"));
            }
            else {
                div.html("");
                div.dialog("option", "title", "无");
            }
        });
    }
    div.dialog("widget").css("position", "fixed");
}


// 展开合拢层
function onFlod(btn, divId) {
    $("#"+btn).click(function (event) {
        event.preventDefault();
        $("#"+divId).slideToggle();
    });

    $("#"+divId+" a").click(function (event) {
        event.preventDefault();
        $("#"+divId).slideUp();
    });
}

/*-------------------------------------------------------------------------------------------------------------------------------------------------------2010年10月25日 ldp 添加*/
$(function () {
    // button 提交按钮在提交前将 form 中的  <input type="submit" name="subAction"/> 的值设置为 button 的值
    $('form button[type=submit]').live("click", function () {
        var form = $(this).parent('form:first');
        var subAction = $("input[name=subAction][type=hidden]");
        if (subAction.length === 0)
            subAction = $('<input type="submit" name="subAction"  id="subAction"/>').appendTo(form);
        var runMethodOrValue, s, Sys = {};
        var browserName = navigator.userAgent.toLowerCase(); //判断浏览器类型       
        (s = browserName.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
           (s = browserName.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] : 0;
        if (Sys.ie)
            runMethodOrValue = $(this).attr("methodBtn"); //ie下的自定义methodBtn值   
        else
            runMethodOrValue = $(this).val(); //非ie下的value值   
        subAction.attr("value", runMethodOrValue);
    });
})
/*-------------------------------------------------------------------------------------------------------------------------------------------------------2010年10月25日 ldp 添加*/
/* 材料设置时检查是否勾选 2010.12.03-------------------------------------------------------------------------------------------------------------------------------------------------------2010年10月25日 ldp 添加*/

$(function () {
    $(".btn101203").live("click", function () {

        var checkboxes = $(this).parents("form").find("table.dataTable :checked");
        if (checkboxes.length < 1) {
            var dialog = $("<div>")
            .attr("title", "提示")
            .html("请选择记录!")
            .dialog({
                buttons: { "确定": function () { dialog.dialog('close') } }
            });
        }
        else {          
            $("#subAction").attr("value", this.val());
        }
        return false;
    });
});
/*----------------------------------------------------------------------------------------------------*/
  $(function(){    
     $("a#operatefun").live("click",function(){           
       var a=this;
       operateFun(a);
       return false;
     });
  });

  function operateFun(a) {
    var url, tr;
    url = $(a).attr("href");
    $.post(url, function (data, textStatus) {
        if (data.IsSuccess !== undefined && data.isSuccess !== null) {
            if (data.IsSuccess === true) {
                tr = $(a).parents("tr");
                try {
                        
                        tr.animate( 50, function () { tr.parents("form").submit(); });
                }
                catch (err) {
                    tr.parents("form").submit();
                }                
            }
            else {
                operationFailureDialog.html(data.Message);
                operationFailureDialog.dialog('open');
            }
        }
        else {
            operationFailureDialog.html(data);
            operationFailureDialog.dialog('open');
        }
    });
}

/*-----------------------------------------------------------------------------*/
function vv(element, notValidMessage){
    var form = $(element).parents("form");
    if ($("input:checked", form).length == 0)
        return notValidMessage || "请选择";
}

function ss(element){
    var message = $("#data-ajax-message").val();
    if (message) {
        operationSuccessDialog.html(message);
        operationSuccessDialog.dialog('open');
        return false;
     }
}
/*-----------------------------------------------------------------------------*/
function leftTreeEngineers()
{
    $.post("/admin/CompetenceManage/LeftTreeEngineers",{},function (data) { 
       if (data !== undefined && data !== null) {
           var p = $(".menu_top p");
            $.each(data,function(x,y){
                for(var i=0;i<p.length;i++)
                {
                    if(p[i].innerHTML==y)
                    {
                        p[i].parentNode.parentNode.removeChild(p[i].parentNode);
                    }
                }
            });
        }
    });
}

function leftTreeLimits() {
    $.post("/admin/CompetenceManage/LeftTreeLimits", {}, function (data) {
        if (data !== undefined && data !== null) {
            var li_a = $(".menu_top li a");
            $.each(data, function (x, y) {
                for (var j = 0; j < li_a.length; j++) { 
                        if (li_a[j].innerHTML == y[1] && $(li_a[j]).parents(".menu_tilte").find("p").html() == y[0]) {
                            li_a[j].parentNode.parentNode.removeChild(li_a[j].parentNode);
                        }                  
                }
            });
            var uls = $(".menu_top ul");
            for (var n = 0; n < uls.length; n++) {
                if ($.trim(uls[n].innerHTML).length == 0) {
                    uls[n].parentNode.parentNode.removeChild(uls[n].parentNode);
                }
            }
        }
    });
}

/*-弹出详细信息窗口----------------------------------------------------------------------------*/
$(function () {
    $("a#lookDetails").live("click", function (e) {
        var a = this;
        operateFunShow(a);
        return false;
    });
});
function operateFunShow(a) {
    var url, tr;
    url = $(a).attr("href");
    $.post(url, function (data, textStatus) {
        detailsDiv
        .attr("title", "信息详细")
        .html(data)
        .dialog({
            autoOpen: true,
            modal: true,
            width: window.screen.width - 380,
            height: window.screen.height - 280,
            bgiframe: true, //解决ie6中遮罩层盖不住select的问题  
            buttons: {
                "关闭": function () {
                    $(this).dialog("close");
                }
            }
        });

    });
}
/*--选项卡功能--------------------------------*/
function tabCad(a, b, c) {
    for (i = 1; i <= b; i++) {
        if (c == i) {
            // 判断选择模块
            document.getElementById(a + "_mo_" + i).style.display = "block";  // 显示模块内容
            document.getElementById(a + "_to_" + i).className = "no";   // 改变菜单为选中样式
        }
        else {
            // 没有选择的模块
            document.getElementById(a + "_mo_" + i).style.display = "none"; // 隐藏没有选择的模块
            document.getElementById(a + "_to_" + i).className = "q";  // 清空没有选择的菜单样式
        }
    }
}

/*--日期控件获取当前时间-------------------------------------*/
function getDayTime_valuebox(inst) {   
    var myDate = new Date();
    var Year = myDate.getFullYear();
    var Month = myDate.getMonth();
    var Day = myDate.getDate();
    if ((Month + 1) < 10)
        Month = "0" + (Month + 1);
    else
        Month = Month + 1;
    if (Day < 10)
        Day = "0" + Day; 
    $("#" + inst).attr("value",Year+"-"+Month+"-"+Day);
}
/*
2011年9月21日 ldp
文本输入框按回车时，使用 查询 按钮提交表单
*/
$(function () {
    $('form').each(function (index) {
        var submitButtons = $(':submit', $(this));
        var firstQuerySubmitbutton = null;
        submitButtons.each(function () {
            if ($(this).val() == "查询" || $(this).text() == "查询") {
                firstQuerySubmitbutton = $(this);
                return;
            }
        });
        var firstSubmitButton = $(':submit:first', $(this));
        if (firstQuerySubmitbutton != null && firstSubmitButton.length > 0
            && firstQuerySubmitbutton[0] != firstSubmitButton[0]) {
            $(":text", $(this)).live('keydown', function (e) {
                if (e.keyCode == 13) {
                    firstQuerySubmitbutton.trigger("click");
                    return false;
                }
            });
        }
    })
});