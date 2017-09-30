<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PortalNewsDetail.aspx.cs"
    Inherits="NGF.Web.PortalNewsDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="padding: 1% 6% 1% 6%;">
    <form id="form1" runat="server">
    <div>
        <div id="divSubject" style="text-align: center;">
            <span id="spanSubject" style="font-size: large; font-weight: bold;"></span>
        </div>
        <br />
        <div id="divPublish" style="text-align: center;">
            <span id="spanPublisher"></span><span></span><span id="spanPublishDate" style="margin-left: 20%">
            </span>
        </div>
        <br />
        <div id="divContent">
        </div>
    </div>    
    <script type="text/javascript" src="Scripts\jquery-2.2.3.min.js"></script>
        <script type="text/javascript" src="Scripts\jquery.json-2.4.js" ></script>
    <script type="text/javascript" src="Scripts\jquery.url-1.8.1.js" ></script>
    <script type="text/javascript" src="PortalNewsDetail.js"></script>
    </form>
</body>
</html>
