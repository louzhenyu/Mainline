//必输字段判断
function check_code() {
    var result = true;
    $('.txt').each(function (index, elem) {
        var code = $(elem).val();
        if (code.length == 0) {
            var name = $(elem).attr('title');
            alert(name + "不能为空！");
            result = false;
            return false;
        }
    });
    return result;
}