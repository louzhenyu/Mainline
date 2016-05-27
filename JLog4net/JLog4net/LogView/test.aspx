<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="LogView.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnGetOrderCount" runat="server" Text="获取" OnClick="btnGetOrderCount_Click" />
        <asp:Label ID="lblGetCount" runat="server" Text=""></asp:Label>
    
    &nbsp;&nbsp;&nbsp;&nbsp; 表名： 
        <asp:TextBox ID="txtCollection" runat="server"></asp:TextBox>
        <asp:Label ID="lblCollections" runat="server" Text="表大小:"></asp:Label>
        <asp:Button ID="btnGetConnlections" runat="server" OnClick="btnGetConnlections_Click" Text="表大小" />
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnGetIndex" runat="server" OnClick="btnGetIndex_Click" Text="Get Indexs" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <br />
        <br />
        collection name:<asp:ListBox ID="listBoxCollections" runat="server" Height="344px" Width="129px"></asp:ListBox>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblDataBaseCount" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; indexs:
        <asp:TextBox ID="txtGetIndexs" runat="server" Height="322px" TextMode="MultiLine"></asp:TextBox>
    
    </div>
    </form>
</body>
</html>
