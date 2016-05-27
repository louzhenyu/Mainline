function addTab(subtitle, url, index) {
    var tabs = $('#tabs').tabs("tabs");
    for (var i = tabs.length + 1; i >= index; --i) {
        $('#tabs').tabs('close', i);
    }
    $('#tabs').tabs('add', {
        title: subtitle,
        href: url,
        closable: true,
        index: index
    });
}

//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}
