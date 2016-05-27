<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLog.aspx.cs" Inherits="JinRi.Log4NetDemo.AddLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       
        <asp:Button ID="btnSystemLog" runat="server" Text="系统日志" OnClick="btnSystemLog_Click" />
    &nbsp;<asp:Button ID="btnBussLog" runat="server" OnClick="btnBussLog_Click" Text="业务日志" />
&nbsp;
        <asp:Button ID="btnFileLog" runat="server" OnClick="btnFileLog_Click" Text="文本日志" />
    &nbsp;    获取时间：<asp:Label ID="lblTime" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
