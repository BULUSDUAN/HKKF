function openDialog(dia, btn,form,url,width,height) {
    $("#" + dia).dialog({
        autoOpen: false, //如果设置为true，则默认页面加载完毕后，就自动弹出对话框；相反则处理hidden状态。 
        bgiframe: true, //解决ie6中遮罩层盖不住select的问题  
        width: width,
        height: height,
        modal: true, //这个就是遮罩效果   
        resizable: false,
        dialogClass: 'alert',
        buttons: {
            "保存": function () {
                //test();//在这里调用函数
                dialogAdd(dia,url,form);
            },
            "取消": function () {
                $("#" + form)[0].reset();
                $(this).dialog("close");
            }
        }
    });

    $("#" + btn).click(function () {
        $("#" + dia).dialog('open');
        return false;
    });

}

function dialogAdd(dia,url,form){
    $.post(url, $("#" + form).serialize(), function (data) {
        if (data.IsSuccess !== undefined && data.IsSuccess !== null) {
            if (data.IsSuccess === true) {
                //alert($("#SpecialityType_ID").find("option:selected").text());
                
                operationSuccessDialog.dialog('open');
                $("#" + form)[0].reset();
                $("#" + dia).dialog('close');
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
// 给全选按钮添加全选功能(全选和取消在一个按钮)
function allCheck(btn, groupId) {
    $('#' + btn).toggle(function () {
        $("input[name='" + groupId + "']").each(function () {
            $(this).attr('checked', true);
        });
        $(this).attr('value', '取消');
    }, function () {
        $("input[name='" + groupId + "']").each(function () {
            $(this).attr('checked', false);
        });
        $(this).attr('value', '全选');
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

//// 给反选按钮添加反选功能
//function allOther(btn, groupId) {
//    $('#' + btn).click(function () {
//        $("input[name='" + groupId + "']").each(function () {
//            if ($(this).attr("checked")) {
//                $(this).attr('checked', false);
//            } else {
//                $(this).attr('checked', true);
//            }
//        });
//    });
//}



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