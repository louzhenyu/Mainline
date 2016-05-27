<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogSearch.aspx.cs" Inherits="LogView.LogSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>日志查询</title>
    <link href="Styles/QuickPage.css" rel="stylesheet" type="text/css" />
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/My97DatePicker/skin/WdatePicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery/jquery-ui-1.8.13/jquery-1.5.2.js"></script>
    <script type="text/javascript" src="../Scripts/jquery/QuickPage.js"></script>
    <link type="text/css" rel="Stylesheet" href="Styles/jquery-ui-themes-1.8.10/themes/base/jquery.ui.all.css" />
    <script type="text/javascript" src="../Scripts/jquery/jquery-ui-1.8.13/jquery-ui-1.8.13.custom.min.js"></script>
    <script type="text/javascript" src="Scripts/publicFunctionTools.js"></script>
    <script type="text/javascript">

        //检测日期控件
        function checkForm() {
            var startime = document.getElementById("<%=txt_ApplyStartTime.ClientID%>").value;
            if (startime == "") {
                alert('对不起，请输入开始时间！');
                document.getElementById("<%=txt_ApplyStartTime.ClientID%>").focus();
                return false;
            }

            var endtime = document.getElementById("<%=txt_ApplyEndTime.ClientID%>").value;
            if (endtime == "") {
                alert('对不起，请输入结束时间！');
                document.getElementById("<%=txt_ApplyEndTime.ClientID%>").focus();
                return false;
            }
        }

        function display(id) {

            var traget = document.getElementById(id);

            if (traget.style.display == "none") {
                traget.style.display = "";
            } else {
                traget.style.display = "none";
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <div class="header">
                <h1>
                    <a href="LogSearch.aspx"><font color="white">今日天下通.日志查询系统V2.0</font> </a></h1>
                <div class="logout">
                </div>

            </div>
            <div class="cc">
            </div>
         <%--   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                    <table width="100%">
                        <tr align="center">
                            <td style="width: 2%"></td>
                            <td valign="top" style="width: 60%">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 15%; font-size: large">App ID：
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAppid" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                TargetControlID="txtAppid" ServiceMethod="GetAppidList" CompletionSetCount="10" MinimumPrefixLength="2" EnableCaching="true"
                                                UseContextKey="True">
                                            </cc1:AutoCompleteExtender>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" style="width: 15%; font-size: large">信息：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMessage" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr id="trIP" runat="server" visible="false">
                                        <td align="left" style="width: 15%; font-size: large">
                                            <asp:Label ID="Label1" runat="server" Text="服务器IP："></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIP" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trClassName" runat="server" visible="false">
                                        <td align="left" style="width: 15%; font-size: large">类名：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtClassName" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trMethod" runat="server" visible="false">
                                        <td align="left" style="width: 15%; font-size: large">执行方法名：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMethod" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr id="trTraceid" runat="server" visible="false">
                                        <td align="left" style="width: 15%; font-size: large">TraceID：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTraceid" runat="server" Width="350px" Height="20px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 15%; font-size: large">日志级别：
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dplistLevl" runat="server" Width="350px" Height="20px">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Debug</asp:ListItem>
                                                <asp:ListItem>Info</asp:ListItem>
                                                <asp:ListItem>Warn</asp:ListItem>
                                                <asp:ListItem>Error</asp:ListItem>
                                                <asp:ListItem>Fatal</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" style="width: 15%; font-size: large">开始时间：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ApplyStartTime" Width="350px" Height="20px" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',isShowClear:true})" />
                                        </td>
                                    </tr>
                                    <tr>

                                        <td align="left" style="width: 15%; font-size: large;">结束时间：
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txt_ApplyEndTime" Width="350px"  Height="20px" runat="server" onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',isShowClear:true,readOnly:false})" />
                                            &nbsp; &nbsp; &nbsp; &nbsp;
                                            
                                        </td>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnSearch" runat="server" Text="查 询" OnClick="btnSearch_Click"
                                                OnClientClick="return checkForm();" CssClass="btn_02_mout" />
                                            &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:Label ID="lblTimeSpan" runat="server"></asp:Label>
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                           <asp:LinkButton ID="btnSeniorSearch" runat="server" OnClick="btnSeniorSearch_Click" >高级查询^</asp:LinkButton>
                                            <asp:LinkButton ID="btnSenior" runat="server" OnClick="btnSenior_Click" Visible="false" >高级查询v</asp:LinkButton>
                                        </td>
                                    </tr>
                                   
                                </table>
                                <table width="100%" class="HYGrid" cellspacing="0" id="tbRoles">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; width: 90px;">日期
                                            </th>
                                            <th style="text-align: center; width: 50px;">日志级别
                                            </th>
                                            <th style="text-align: center; width: 60px;">服务器ip
                                            </th>
                                            <th style="text-align: center; width: 60px;">应用appid
                                            </th>
                                            <th style="text-align: center; width: 100px;">执行方法名
                                            </th>
                                            <th style="text-align: center; width: 60px;">详细日志
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptUserInfoList">

                                            <ItemTemplate>

                                                <tr runat="server" id="TrID" title='<%# Eval("_id")%>'>
                                                    <td style="text-align: center; width: 90px;">
                                                        <%# Eval("timestamp")%>
                                                    </td>
                                                    <td style="text-align: center; width: 50px;">
                                                        <%# Eval("level")%>
                                                    </td>
                                                    <td style="text-align: center; width: 60px;">
                                                        <%# Eval("ip")%>
                                                    </td>
                                                    <td style="text-align: center; width: 60px;">
                                                        <%# Eval("appid")%>
                                                    </td>
                                                    <td style="text-align: center; width: 100px;">
                                                        <%# Eval("method")%>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("_id")%>' Visible='false'></asp:Label>
                                                        <asp:LinkButton ID="btnInfo" runat="server" Text="详细日志" OnClick="btnInfo_Click" CommandArgument='<%# Eval("_id")%>' />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <% if (this.rptUserInfoList.Items.Count <= 0)
                                           { %>
                                        <tr>
                                            <td colspan="6" style="text-align: center;">暂无数据！
                                            </td>
                                        </tr>
                                        <%} %>
                                        <tr>
                                            <td colspan="6" style="text-align: center;">
                                                <asp:Label ID="Lb_PageShow" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td style="width: 1%"></td>
                            <td valign="top" style="width: 35%">
                                <table width="100%" class="HYGrid" runat="server" id="tbInfo">
                                    <tr>
                                        <td style="width: 100px; text-align: right; font-size: large; color: #990099">信息:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:TextBox ID="lblMessage" runat="server" Width="100%" TextMode="MultiLine" Height="500px"
                                                    CssClass="message"></asp:TextBox>
                                            </font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">日期:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblTime" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">日志级别:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblLevel" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">行号:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblLineNumber" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">类名:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblCassName" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">文件路径名:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">服务器IP:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblIP" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">应用APPID:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblAppid" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">执行方法名:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblMethod" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td style="text-align: right; width: 100px; font-size: large; color: #990099">TraceID:
                                        </td>
                                        <td>
                                            <font color="black" size="3">
                                                <asp:Label ID="lblTraceID" runat="server" Text=""></asp:Label></font>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 2%"></td>
                        </tr>
                    </table>

                    <div id="div_UserPage">
                    </div>
           <%--     </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>

    </form>
</body>
</html>
