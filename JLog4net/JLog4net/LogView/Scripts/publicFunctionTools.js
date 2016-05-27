
//对话框(带IFRAME）
function OpenFrameDialog(id, title, cancelClickHanler, width, height, url) {
    var w = width || 800;
    var h = height || 600;
    var frame = $("<iframe id='" + id + "' frameborder='0' name='" + id + "' src=''></iframe>").appendTo($("BODY"));
    var obj = {
        autoOpen: true,
        resizable: false,
        width: w,
        height: h,
        title: title,
        modal: true,
        bgiframe: true,
        open: function (evt, ui) {
            frame.attr("src", url).width(w - 10).height(h);
        },
        close: function (evt, ui) {
            frame.dialog("destroy");
            frame.html("").remove();
            if (cancelClickHanler && $.isFunction(cancelClickHanler)) {
                cancelClickHanler();
            }
        }
    };
    return frame.dialog(obj);
}

//Dialog展现方法
function DialogShow(id, title, cancelClickHanler, width, height, url) {
    var w = width || 800;
    var h = height || 600;
    var alertdiv = $("<div id='" + id + "' title='" + title + "'></div>").appendTo($("BODY"));
    var content = alertdiv.load(url);
    alertdiv.dialog({
        autoOpen: true,
        height: h,
        width: w,
        modal: true,
        resizable: false,
        bgiframe: true,
        close: function (evt, ui) {
            alertdiv.dialog("destroy");
            alertdiv.empty().remove();
            if (cancelClickHanler && $.isFunction(cancelClickHanler)) {
                cancelClickHanler();
            }
        }
    });
}

var Request =
        {
            QueryString: function (val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "=([^&?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }

document.onkeydown = function (e) {
    var elementType = window.event.srcElement.type;
    if (event.keyCode == 8) {
        if (elementType != "text" && elementType != "textarea" && elementType != "password") {
            event.keyCode = 0;
            event.returnvalue = false;
        }
        else if (event.srcElement.readOnly) {
            event.keyCode = 0;
            event.returnvalue = false;
        }
    }
    //F5刷新键
    else if (event.keyCode == 116) {
    }
}