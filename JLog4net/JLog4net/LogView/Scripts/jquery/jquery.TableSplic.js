/// <reference path="jquery-1.7.1.min.js" />
$.fn.tbsplic = function (startponit, endpoint, third) {
    obj = $(this);
    if (startponit.indexOf(",") == -1 || endpoint.indexOf(",") == -1) {
        alert("开始或结束坐标格式错误!");
        return "";
    }
    spr = startponit.split(",")[0];
    spc = startponit.split(",")[1];
    epr = endpoint.split(",")[0];
    epc = endpoint.split(",")[1];
    if (spr != epr && spc != epc) {
        alert("您要合并的单元格不在同一行或同一列!");
        return "";
    }
    if (spr == epr) { //如果rows行索引相等,则合并为横向合并
        var cells = parseInt(epc) - parseInt(spc) + 1; //获得需要合并的列数
        var trobj = obj.find("TBODY").find("tr").eq(spr); //获取要修改的行
        for (var i = parseInt(epc); i > parseInt(spc); i--) {
            trobj.find("td").eq(i).remove();
        }
        trobj.find("td").eq(spc).attr("colspan", cells);
    }
    else if (spc == epc) { //如果cells列索引相等,则合并为纵向
        if (("," + third + ",").indexOf("," + spc + ",") != -1) {
            var innerHtml = "";
            var rows = parseInt(epr) - parseInt(spr) + 1; //获得需要合并的行数
            for (var i = parseInt(epr); i > parseInt(spr); i--) {
                if ((innerHtml + ",").indexOf("," + obj.find("TBODY").find("tr").eq(i).find("td").eq(spc).html() + ",") == -1)
                    innerHtml = "," + obj.find("TBODY").find("tr").eq(i).find("td").eq(spc).html() + innerHtml;
                obj.find("TBODY").find("tr").eq(i).find("td").eq(spc).remove();
            }
            if ((innerHtml + ",").indexOf("," + obj.find("TBODY").find("tr").eq(spr).find("td").eq(spc).html() + ",") == -1)
                innerHtml = "," + obj.find("TBODY").find("tr").eq(spr).find("td").eq(spc).html() + innerHtml;
            innerHtml = innerHtml.length > 0 ? innerHtml.substring(1) : innerHtml;
            obj.find("TBODY").find("tr").eq(spr).find("td").eq(spc).attr("rowspan", rows).html(innerHtml);
        }
        else {
            var rows = parseInt(epr) - parseInt(spr) + 1; //获得需要合并的行数
            for (var i = parseInt(epr); i > parseInt(spr); i--) {
                obj.find("TBODY").find("tr").eq(i).find("td").eq(spc).remove();
            }
            obj.find("TBODY").find("tr").eq(spr).find("td").eq(spc).attr("rowspan", rows);
        }
    }
}

$.fn.TableSplic = function (first, Second, third) {
    third = third == null ? "" : third;
    var obj = $(this);
    var n1 = first.split(":")[0];
    var n2 = Second.split(":")[0];

    var cells = "";

    var firstArray = first.split(":")[1].split(",");
    var SecondArray = Second == "" ? "" : Second.split(":")[1].split(",");

    for (var i = 0; i < firstArray.length; i++) {
        cells += n1 + "," + firstArray[i] + "&";
    }
    for (var i = 0; i < SecondArray.length; i++) {
        cells += n2 + "," + SecondArray[i] + "&";
    }

    cells = cells.length > 0 ? cells.substring(0, cells.length - 1) : "";

    var cellsArray = cells.split("&");
    var tempText = bublesort(cellsArray);


    //1.读取序号1
    var no1 = "";
    var index1 = "";
    obj.find("TBODY tr").each(function (i) {
        var nowstr = $(this).find("td").eq(parseInt(n1)).html() + "&";
        if (i == 0) {
            index1 += "0," + n1;
        }
        else {
            if (no1.indexOf(nowstr) == -1) {
                index1 += "-" + ($(this).index() - 1) + "," + n1 + "&" + $(this).index() + "," + n1 + "";
            }
        }
        i++;
        no1 += no1.indexOf(nowstr) == -1 ? nowstr : "";
    });
    index1 += "-" + (obj.find("TBODY tr").length - 1) + "," + n1;
    no1 = no1.length > 0 ? no1.substring(0, no1.length - 1) : "";


    //2.读取序号2
    var no2 = "";
    var index2 = "";

    var indexarry = index1.split("&");
    if (Second != "") {
        for (var w = 0; w < indexarry.length; w++) {
            obj.find("TBODY tr").each(function (i) {
                if (parseInt(indexarry[w].split("-")[0].split(",")[0]) <= $(this).index() && parseInt(indexarry[w].split("-")[1].split(",")[0]) >= $(this).index()) {
                    var nowstr = $(this).find("td").eq(parseInt(n2)).html() + "&";
                    if ($(this).index() == 0) {
                        index2 += $(this).index() + "," + n2;
                    }
                    else {
                        if (no2.indexOf(nowstr) == -1) {
                            index2 += "-" + ($(this).index() - 1) + "," + n2 + "&" + $(this).index() + "," + n2;
                        }
                    }
                    no2 += no2.indexOf(nowstr) == -1 ? nowstr : "";
                }
            });
            no2 = "";
        }

        index2 += "-" + (obj.find("TBODY tr").length - 1) + "," + n2;
        var index2array = index2.split("&");
    }
    //合并单元格
    for (var j = tempText.length - 1; j >= 0; j--) {
        if (tempText[j].split(",")[0] == n1) {
            for (var i = indexarry.length - 1; i >= 0; i--) {
                obj.tbsplic(indexarry[i].split("-")[0].split(",")[0] + "," + tempText[j].split(",")[1], indexarry[i].split("-")[1].split(",")[0] + "," + tempText[j].split(",")[1], third);
            }
        }
        else if (tempText[j].split(",")[0] == n2 && Second != "") {
            for (var i = index2array.length - 1; i >= 0; i--) {
                obj.tbsplic(index2array[i].split("-")[0].split(",")[0] + "," + tempText[j].split(",")[1], index2array[i].split("-")[1].split(",")[0] + "," + tempText[j].split(",")[1], third);
            }
        }
    }
}

//冒泡排序
function bublesort(arr) {
    for (var i = arr.length - 1; i > 0; i--) {
        for (var j = 0; j < i; j++) {
            if (parseInt(arr[j].split(",")[1]) > parseInt(arr[j + 1].split(",")[1])) {
                tmp = arr[j]; arr[j] = arr[j + 1]; arr[j + 1] = tmp;
            }
        }
    }
    return arr;
}
  
