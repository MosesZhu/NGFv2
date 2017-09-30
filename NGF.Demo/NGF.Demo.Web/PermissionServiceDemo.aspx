<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermissionServiceDemo.aspx.cs" Inherits="NGF.Demo.Web.PermissionServiceDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/bootstrap.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/AdminLTE/AdminLTE.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/AdminLTE/skins/_all-skins.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/font-awesome.min.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/datepicker/datepicker3.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/timepicker/bootstrap-timepicker.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/bootstrap-table.min.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/ngf.framework.css">
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <button class="btn btn-skin-primary ngf-btn-120" onclick="return GetAuthorizedStaticDataList();">
                GetStaticData</button>
        </div>
    </form>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/jquery-1.12.4.min.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/ngf.framework.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/jquery.uriAnchor.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/jquery.slimscroll.min.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/bootstrap.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/timepicker/bootstrap-timepicker.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/datepicker/bootstrap-datepicker.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/input-mask/jquery.inputmask.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/bootstrap-table.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/bootstrap-table-zh-TW.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/bootstrap-table-zh-CN.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/bootstrap-table-en-US.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/AdminLTE.js"></script>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/jquery.wresize.js"></script>
    <script>
        var GetAuthorizedStaticDataList = function () {
            var options = {
                "success": function (d) {
                    alert(d);
                },
                "error": function (e) {
                    alert(e);
                }
            };
            $.callWebService("GetAuthorizedStaticDataList", {}, options);
            return true;
        };
    </script>
</body>
</html>
