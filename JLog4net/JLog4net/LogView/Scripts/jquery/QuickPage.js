$.fn.quickPage = function (method, option) {
    if (typeof (method) == "object") {
        option = method || null;
        method = "init";
    }
    if (method == "") {
        method = "init";
    }
    var pagebox = this;
    var methods = {
        "init": function () { init(); },
        "getOption": function () { return pagebox.data("option"); }
    };

    var showNumSize = 10;
    return methods[method]();

    //实例化分页控件
    function init() {
        option = option || {};
        option.size = option.size || 10; //每页显示多少条记录
        option.maxCount = option.maxCount || 0; //总共多少记录
        option.curIndex = option.curIndex || 1; //当前第几页
        option.click = option.click || null; //分页触发方法
        if (option.maxCount % option.size == 0) {
            option.pageCount = option.maxCount / option.size;
        }
        else {
            option.pageCount = parseInt(option.maxCount / option.size) + 1;
        }
        if (option.pageCount <= 0) {
            option.pageCount = 1;
        }
        if (option.curIndex < 1) {
            option.curIndex = 1;
        }
        if (option.curIndex > option.pageCount) {
            option.curIndex = option.pageCount;
        }
        //创建分页插件
        create();
    }

    //创建分页插件
    function create() {
        var op = pagebox.data("option");
        if (typeof (op) == "object") {
            option = op;
        }
        pagebox.addClass("pagination");

        var table = $("<table border='0' cellspacing='0' cellpadding='0'></table>").appendTo(pagebox);

        //第一页
        //-------------------------------
        var table_tr = $("<tr></tr>").appendTo(table);
        var first_td = $("<td></td>").appendTo(table_tr);
        if (option.curIndex > 1) {
            var first_a = $("<a class='l-btn l-btn-plain' href='javascript:void(0)' title='第一页'></a>").appendTo(first_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-first'>第一页</span></span></span>").appendTo(first_a);
            first_a.bind("click", { pageIndex: 1, size: option.size }, linkClickHandler);
        }
        else {
            var first_a = $("<a class='l-btn l-btn-plain l-btn-disabled' href='javascript:void(0)' title='第一页'></a>").appendTo(first_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-first'>第一页</span></span></span>").appendTo(first_a);
        }
        //-------------------------------

        //上一页
        //-------------------------------
        var prev_td = $("<td></td>").appendTo(table_tr);
        if (option.curIndex > 1) {
            var prev_a = $("<a class='l-btn l-btn-plain' href='javascript:void(0)' title='上一页'></a>").appendTo(prev_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-prev'>上一页</span></span></span>").appendTo(prev_a);
            prev_a.bind("click", { pageIndex: (option.curIndex - 1), size: option.size }, linkClickHandler);
        }
        else {
            var prev_a = $("<a class='l-btn l-btn-plain l-btn-disabled' href='javascript:void(0)' title='上一页'></a>").appendTo(prev_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-prev'>上一页</span></span></span>").appendTo(prev_a);
        }
        //-------------------------------

        //分割线、信息
        //-------------------------------
        $("<div class='pagination-btn-separator'/>").appendTo($("<td></td>").appendTo(table_tr));
        $("<span style='padding-left:6px;'>第</span>").appendTo($("<td></td>").appendTo(table_tr));
        $("<input class='pagination-num' type='text' size='2' value='" + option.curIndex + "' readonly='readonly'/>").appendTo($("<td></td>").appendTo(table_tr));
        $("<span style='padding-right:6px;'>页 共" + option.pageCount + "页</span>").appendTo($("<td></td>").appendTo(table_tr));
        $("<div class='pagination-btn-separator'/>").appendTo($("<td></td>").appendTo(table_tr));
        //-------------------------------

        //下一页
        //-------------------------------
        var next_td = $("<td></td>").appendTo(table_tr);

        if (option.curIndex < option.pageCount) {
            var next_a = $("<a class='l-btn l-btn-plain' href='javascript:void(0)' title='下一页'></a>").appendTo(next_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-next'>下一页</span></span></span>").appendTo(next_a);
            next_a.bind("click", { pageIndex: (option.curIndex + 1), size: option.size }, linkClickHandler);
        }
        else {
            var next_a = $("<a class='l-btn l-btn-plain l-btn-disabled' href='javascript:void(0)' title='下一页'></a>").appendTo(next_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-next'>下一页</span></span></span>").appendTo(next_a);
        }
        //-------------------------------

        //最后一页
        //-------------------------------
        var last_td = $("<td></td>").appendTo(table_tr);

        if (option.curIndex < option.pageCount) {
            var last_a = $("<a class='l-btn l-btn-plain' href='javascript:void(0)' title='最后一页'></a>").appendTo(last_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-last'>最后一页</span></span></span>").appendTo(last_a);
            last_a.bind("click", { pageIndex: option.pageCount, size: option.size }, linkClickHandler);
        }
        else {
            var last_a = $("<a class='l-btn l-btn-plain l-btn-disabled' href='javascript:void(0)' title='最后一页'></a>").appendTo(last_td);
            $("<span class='l-btn-left'><span class='l-btn-text'><span class='l-btn-empty pagination-last'>最后一页</span></span></span>").appendTo(last_a);
        }
        //-------------------------------
        var from = ((option.curIndex - 1) * (option.size) + 1);
        var to = (((option.curIndex) * (option.size)) > option.maxCount) ? option.maxCount : ((option.curIndex) * (option.size));
        $("<div class='pagination-info'>当前显示从" + from + "-" + to + "共" + option.maxCount + "记录</div>").appendTo(pagebox);

        $("<div style='clear:both;'/>").appendTo(pagebox);

    }

    //分页按钮方法
    function linkClickHandler(evt) {
        if (evt.data.pageIndex < 1) {
            evt.data.pageIndex = 1;
        }
        if (evt.data.pageIndex > option.pageCount) {
            evt.data.pageIndex = option.pageCount;
        }
        option.curIndex = evt.data.pageIndex;
        var v = true;
        if (option.click && $.isFunction(option.click)) {
            v = option.click(evt.data.pageIndex, evt.data.size);
        }
        if (v) {
            pagebox.html("");
            pagebox.data("option", option);
            create();
        }
        pagebox.data("option", option);
        return false;
    }
}