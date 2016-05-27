//指定合并的列 以逗号分割
//例如 1,3,5列合并的依据相同就是1,3,5
//2,4,6列合并的依据相同就是2,4,6
$.fn.TableSplic2 = function (first, second) {

    var firstArr = $.trim(first).split(",");
    var secondArr = $.trim(second).split(",");

    var arr_node1 = new Array();
    var arr_node2 = new Array();

    //计算数值在数组中出现的次数
    var arr_num = new Array();
    var arr_num2 = new Array();
    $(this).find("tbody tr").each(function () {

        var node1 = $.trim($(this).attr("node1"));
        var node2 = $.trim($(this).attr("node2"));

        if (node1 && node1.length > 0) {
            if (!gnNoExistsInArray(arr_node1, node1)) {
                arr_node1.push(node1);
                arr_num[node1] = 1;
            }
            else {
                if (arr_num[node1]) {
                    arr_num[node1] = parseInt(arr_num[node1]) + 1;
                }
            }

            if (node2 && node2.length > 0) {
                if (typeof arr_node2[node1] == "undefined") {
                    arr_node2[node1] = new Array();
                }
                if (typeof arr_num2[node1] == "undefined") {
                    arr_num2[node1] = new Array();
                }
                if (!gnNoExistsInArray(arr_node2[node1], node2)) {
                    arr_node2[node1].push(node2);
                    arr_num2[node1][node2] = 1;
                }
                else {
                    arr_num2[node1][node2] = parseInt(arr_num2[node1][node2]) + 1;
                }
            }
        }
    });
    if (arr_node1 && arr_node1.length > 0) {
        for (var j = 0; j < arr_node1.length; j++) {
            var node1 = arr_node1[j]
            var rowspan1 = arr_num[node1];
            if (rowspan1 > 1) {

                if (firstArr && firstArr.length > 0) {
                    var one = "";
                    for (var kk = 0; kk < firstArr.length; kk++) {
                        one += ".node_" + firstArr[kk] + "_" + node1 + ",";
                    }
                    one = one.substring(0, one.length - 1);
                    $(".row_" + node1 + ":gt(0)").find(one).remove();
                    $(".row_" + node1 + ":eq(0)").find(one).attr("rowspan", rowspan1);
                }

                if (arr_node2[node1] && arr_node2[node1].length > 0) {
                    var index = 0;
                    for (var k = 0; k < arr_node2[node1].length; k++) {
                        var node2 = arr_node2[node1][k]
                        var rowspan2 = arr_num2[node1][node2];
                        if (rowspan2 > 1) {
                            if (secondArr && secondArr.length > 0) {
                                var two = "";
                                for (var gg = 0; gg < secondArr.length; gg++) {
                                    two += ".node_" + secondArr[gg] + "_" + node1 + ",";
                                }
                                two = two.substring(0, two.length - 1);
                                $(".row_" + node1 + ":gt(" + index + ")").find(two).remove();
                                $(".row_" + node1 + ":eq(" + index + ")").find(two).attr("rowspan", rowspan2);
                            }
                        }
                        index += rowspan2;
                    }
                }
            }
        }
    }

};
//判断一个值是否存在一个数组中 
function gnNoExistsInArray(arr, v) {
    if (typeof arr == "undefined") {
        return false;
    }
    var flag = false;
    if (arr && arr.length > 0) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == v) {
                flag = true;
                break;
            }
        }
    }
    else {
        flag = false;
    }
    return flag;
}