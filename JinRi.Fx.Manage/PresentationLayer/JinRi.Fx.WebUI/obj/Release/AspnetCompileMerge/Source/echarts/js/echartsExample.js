var myChart;
var domCode = document.getElementById('sidebar-code');
var domGraphic = document.getElementById('graphic');
var domMain = document.getElementById('main');
var domMessage = document.getElementById('wrong-message');
var iconResize = document.getElementById('icon-resize');
var needRefresh = false;

var enVersion = location.hash.indexOf('-en') != -1;
var hash = location.hash.replace('-en', '');
hash = hash.replace('#', '') || (needMap() ? 'default' : 'macarons');
hash += enVersion ? '-en' : '';

var curTheme;
function requireCallback(ec, defaultTheme) {
    curTheme = themeSelector ? defaultTheme : {};
    echarts = ec;
    refresh();
    window.onresize = myChart.resize;
}

var themeSelector = $('#theme-select');
if (themeSelector) {
    themeSelector.html(
        '<option selected="true" name="macarons">macarons</option>'
        + '<option name="infographic">infographic</option>'
        + '<option name="shine">shine</option>'
        + '<option name="dark">dark</option>'
        + '<option name="blue">blue</option>'
        + '<option name="green">green</option>'
        + '<option name="red">red</option>'
        + '<option name="gray">gray</option>'
        + '<option name="helianthus">helianthus</option>'
        + '<option name="roma">roma</option>'
        + '<option name="mint">mint</option>'
        + '<option name="macarons2">macarons2</option>'
        + '<option name="sakura">sakura</option>'
        + '<option name="default">default</option>'
    );
    $(themeSelector).on('change', function () {
        selectChange($(this).val());
    });
    function selectChange(value) {
        var theme = value;
        myChart.showLoading();
        $(themeSelector).val(theme);
        if (theme != 'default') {
            window.location.hash = value + (enVersion ? '-en' : '');
            require(['theme/' + theme], function (tarTheme) {
                curTheme = tarTheme;
                setTimeout(refreshTheme, 500);
            })
        }
        else {
            window.location.hash = enVersion ? '-en' : '';
            curTheme = {};
            setTimeout(refreshTheme, 500);
        }
    }
    function refreshTheme() {
        myChart.hideLoading();
        myChart.setTheme(curTheme);
    }
    if ($(themeSelector).val(hash.replace('-en', '')).val() != hash.replace('-en', '')) {
        $(themeSelector).val('macarons');
        hash = 'macarons' + enVersion ? '-en' : '';
        window.location.hash = hash;
    }
}

function autoResize() {
    if ($(iconResize).hasClass('glyphicon-resize-full')) {
        focusCode();
        iconResize.className = 'glyphicon glyphicon-resize-small';
    }
    else {
        focusGraphic();
        iconResize.className = 'glyphicon glyphicon-resize-full';
    }
}

function focusCode() {
    domCode.className = 'col-md-8 ani';
    domGraphic.className = 'col-md-4 ani';
}

function focusGraphic() {
    domCode.className = 'col-md-4 ani';
    domGraphic.className = 'col-md-8 ani';
    if (needRefresh) {
        myChart.showLoading();
        setTimeout(refresh, 1000);
    }
}

var editor = CodeMirror.fromTextArea(
    document.getElementById("code"),
    { lineNumbers: true }
);
editor.setOption("theme", 'monokai');


editor.on('change', function () { needRefresh = true; });

function refresh(isBtnRefresh) {
    if (isBtnRefresh) {
        needRefresh = true;
        focusGraphic();
        return;
    }
    needRefresh = false;
    if (myChart && myChart.dispose) {
        myChart.dispose();
    }
    myChart = echarts.init(domMain, curTheme);
    window.onresize = myChart.resize;
    loadOptions();
    var appid = getParameter("AppId");
    appid = appid == "" ? -1 : appid;
    var apiAddress = "http://" + window.location.host + "/api/appdependent?AppId=" + appid;
    $.ajax({
        type: "GET",
        url: apiAddress,
        dataType: "jsonp",
        jsonpCallback: "callback"
    });
}
function getParameter(param) {
    var query = window.location.search;//获取URL地址中？后的所有字符  
    var iLen = param.length;//获取你的参数名称长度  
    var iStart = query.indexOf(param);//获取你该参数名称的其实索引  
    if (iStart == -1)//-1为没有该参数  
        return "";
    iStart += iLen + 1;
    var iEnd = query.indexOf("&", iStart);//获取第二个参数的其实索引  
    if (iEnd == -1)//只有一个参数  
        return query.substring(iStart);//获取单个参数的参数值  
    return query.substring(iStart, iEnd);//获取第二个参数的值  
}

function callback(result) {
    option.legend.data = [];
    var seriesTemp = option.series[0];
    seriesTemp.categories = [];
    seriesTemp.nodes = [];
    seriesTemp.links = [];
    var appType = [];
    $(result.AppType).each(function (i) {
        var item = { name: '' };
        item.name = result.AppType[i].TypeName;
        appType.push(result.AppType[i].AppTypeId);
        seriesTemp.categories.push(item);
        option.legend.data.push(item);
    });

    $(result.Applications).each(function (i) {
        var item = { category: 0, name: '', value: 5 + i };
        item.category = getIndex(appType, result.Applications[i].AppType);
        item.name = result.Applications[i].AppEName;
        seriesTemp.nodes.push(item);
    });
    $(result.AppDependentInfo).each(function (i) {
        var item = { source: '', target: '', weight: 50 + i };
        item.source = result.AppDependentInfo[i].Source.AppEName;
        item.target = result.AppDependentInfo[i].Target.AppEName;
        seriesTemp.links.push(item);
    });
    option.series[0] = seriesTemp;
    //(new Function(editor.doc.getValue()))();
    myChart.setOption(option, true)
    domMessage.innerHTML = '';
}

function getIndex(array, item) {
    for (var index = 0; index < array.length; index++) {
        if (array[index] == item) {
            return index;
        }
    }
    return -1;
}

function loadOptions() {
    option = {
        tooltip: {
            trigger: 'item',
            formatter: '{a} : {b}',
            textStyle: { fontSize: 12 }
        },
        toolbox: {
            show: true,
            feature: {
                restore: { show: true },
                magicType: { show: true, type: ['force', 'chord'] },
                saveAsImage: { show: true }
            }
        },
        legend: {
            x: 'left',
            data: [{ name: "" }]//['WebSite', 'WebService', 'OpenApi', 'WinForm', 'WinService', 'Mobile', 'JobApp', 'Lib']
        },
        series: [
        {
            type: 'force',
            name: "应用",
            ribbonType: false,
            categories:
                [
            //{ name: 'WebSite' },
            //{ name: 'WebService' },
            //{ name: 'OpenApi' },
            //{ name: 'WinForm' },
            //{ name: 'WinService' },
            //{ name: 'Mobile' },
            //{ name: 'JobApp' },
            //{ name: 'Lib' }
                ],
            itemStyle: {
                normal: {
                    label: {
                        show: true,
                        textStyle: {
                            color: '#333'
                        }
                    },
                    nodeStyle: {
                        brushType: 'both',
                        borderColor: 'rgba(255,215,0,0.4)',
                        borderWidth: 1
                    }
                },
                emphasis: {
                    label: {
                        show: false
                        // textStyle: null      // 默认使用全局文本样式，详见TEXTSTYLE
                    },
                    nodeStyle: {
                        //r: 30
                    },
                    linkStyle: {}
                }
            },
            minRadius: 25,
            maxRadius: 25,
            gravity: 1.1,
            scaling: 1.2,
            draggable: true,
            linkSymbol: 'arrow',
            steps: 10,
            coolDown: 0.9,
            //preventOverlap: true,
            nodes: [
            { category: 7, name: 'JMetrics', value: 5 },
            { category: 7, name: 'JRedis', value: 5 },
            { category: 7, name: 'JEtermClient', value: 5 },
            { category: 1, name: 'Flight.Booking.SOA', value: 5 },
            { category: 0, name: 'Flight.Booking.Web', value: 5 },
            { category: 1, name: 'Flight.Product.SOA', value: 5 },
            { category: 0, name: 'Flight.Product.WebSite', value: 5 },
            { category: 4, name: 'Flight.User.SOA', value: 5 },
            { category: 1, name: 'Flight.Order.SOA', value: 5 },
            { category: 7, name: 'JQuartZ', value: 5 },
            { category: 7, name: 'JRabbitMQ', value: 5 },
            { category: 7, name: 'JLog', value: 5 },
            { category: 0, name: 'JinRi.Fx.Manage', value: 5 },
            ],
            links: [
            { source: 'JEtermClient', target: 'JRedis', weight: 50 },
            { source: 'JEtermClient', target: 'JMetrics', weight: 50 },
            { source: 'Flight.Booking.SOA', target: 'JRedis', weight: 50 },
            { source: 'Flight.Booking.SOA', target: 'JMetrics', weight: 50 },
            { source: 'Flight.Booking.SOA', target: 'JEtermClient', weight: 50 },
            { source: 'Flight.Booking.Web', target: 'JRedis', weight: 50 },
            { source: 'Flight.Booking.Web', target: 'JMetrics', weight: 50 },
            { source: 'Flight.Product.SOA', target: 'JRedis', weight: 50 },
            { source: 'Flight.Product.SOA', target: 'JMetrics', weight: 50 },
            { source: 'Flight.Product.SOA', target: 'JEtermClient', weight: 50 },
            { source: 'Flight.Order.SOA', target: 'Flight.User.SOA', weight: 50 },
            { source: 'Flight.Order.SOA', target: 'JRedis', weight: 50 },
            { source: 'Flight.Order.SOA', target: 'JLog', weight: 50 },
            { source: 'Flight.Order.SOA', target: 'JMetrics', weight: 50 },
            { source: 'Flight.Order.SOA', target: 'JEtermClient', weight: 50 },
            ]
        }
        ]
    };

    var ecConfig = require('echarts/config');
    function focus(param) {
        //var data = param.data;
        //var links = option.series[0].links;
        //var nodes = option.series[0].nodes;
        //if (
        //data.source != null
        //&& data.target != null
        //) { //点击的是边
        //    var sourceNode = nodes.filter(function (n) { return n.name == data.source })[0];
        //    var targetNode = nodes.filter(function (n) { return n.name == data.target })[0];
        //    console.log("选中了边 " + sourceNode.name + ' -> ' + targetNode.name + ' (' + data.weight + ')');
        //} else { // 点击的是点
        //    console.log("选中了" + data.name + '(' + data.value + ')');
        //}
    }
    myChart.on(ecConfig.EVENT.CLICK, focus)

    myChart.on(ecConfig.EVENT.FORCE_LAYOUT_END, function () {
        console.log(myChart.chart.force.getPosition());
    });
}

function needMap() {
    var href = location.href;
    return href.indexOf('map') != -1
           || href.indexOf('mix3') != -1
           || href.indexOf('mix5') != -1
           || href.indexOf('dataRange') != -1;

}

var echarts;
var developMode = false;

if (developMode) {
    window.esl = null;
    window.define = null;
    window.require = null;
    (function () {
        var script = document.createElement('script');
        script.async = true;

        var pathname = location.pathname;

        var pathSegs = pathname.slice(pathname.indexOf('doc')).split('/');
        var pathLevelArr = new Array(pathSegs.length - 1);
        script.src = pathLevelArr.join('../') + 'asset/js/esl/esl.js';
        if (script.readyState) {
            script.onreadystatechange = fireLoad;
        }
        else {
            script.onload = fireLoad;
        }
        (document.getElementsByTagName('head')[0] || document.body).appendChild(script);

        function fireLoad() {
            script.onload = script.onreadystatechange = null;
            setTimeout(loadedListener, 100);
        }
        function loadedListener() {
            // for develop
            require.config({
                packages: [
                    {
                        name: 'echarts',
                        location: '../../src',
                        main: 'echarts'
                    },
                    {
                        name: 'zrender',
                        // location: 'http://ecomfe.github.io/zrender/src',
                        location: '../../../zrender/src',
                        main: 'zrender'
                    }
                ]
            });
            launchExample();
        }
    })();
}
else {
    // for echarts online home page
    require.config({
        paths: {
            echarts: './www/js'
        }
    });
    launchExample();
}

var isExampleLaunched;
function launchExample() {
    if (isExampleLaunched) {
        return;
    }

    // 按需加载
    isExampleLaunched = 1;
    require(
        [
            'echarts',
            'theme/' + hash.replace('-en', ''),
            'echarts/chart/line',
            'echarts/chart/bar',
            'echarts/chart/scatter',
            'echarts/chart/k',
            'echarts/chart/pie',
            'echarts/chart/radar',
            'echarts/chart/force',
            'echarts/chart/chord',
            'echarts/chart/gauge',
            'echarts/chart/funnel',
            'echarts/chart/eventRiver',
            'echarts/chart/venn',
            'echarts/chart/treemap',
            'echarts/chart/tree',
            'echarts/chart/wordCloud',
            needMap() ? 'echarts/chart/map' : 'echarts'
        ],
        requireCallback
    );
}

