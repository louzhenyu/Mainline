<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>JinRi NuGet Server</title>
    <style>
        body { font-family: Calibri; }
    </style>
</head>
<body>
    <div>
        <h2>Version：<%= typeof(NuGet.Server.DataServices.Package).Assembly.GetName().Version %></h2>
        <p>
            <a href="<%= VirtualPathUtility.ToAbsolute("~/nuget/Packages") %>">Packages</a>
        </p>
        <fieldset style="width:800px">
            <legend><strong>Repository URLs</strong></legend>
            Package Sources
            <blockquote>
                <strong><%= Helpers.GetRepositoryUrl(Request.Url, Request.ApplicationPath) %></strong>
            </blockquote>
            <% if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["apiKey"])) { %>
            To enable pushing packages to this feed using the nuget command line tool (nuget.exe). Set the api key appSetting in web.config.
            <% } %> 
            <% else { %>
            Use the command below to push packages to this feed using the nuget command line tool (nuget.exe).
            <% } %>
            <blockquote>
                <strong>nuget push {package file} -s <%= Helpers.GetPushUrl(Request.Url, Request.ApplicationPath) %> {apikey}</strong>
            </blockquote>            
        </fieldset>
        <div style="display:none"><%= ConfigurationManager.AppSettings["apiKey"] %></div>
    </div>
</body>
</html>
