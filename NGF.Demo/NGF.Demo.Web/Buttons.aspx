<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Buttons.aspx.cs" Inherits="NGF.Demo.Web.Buttons" %>

<!DOCTYPE html>
<html>
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
<body class="skin-red">
    <form id="form1" runat="server">
        <!--inquiry area & toolbar-->
        <div class="ngf-input-area">
            <div class="ngf-toolbar">
                <button class="btn btn-skin-primary ngf-btn-60">60</button>
                <button class="btn btn-skin-primary ngf-btn-90">90</button>
                <button class="btn btn-skin-primary ngf-btn-120">120</button>
                <button class="ngf-btn-add"></button>
                <button class="ngf-btn-delete"></button>
                <button class="ngf-btn-edit" onclick=""></button>
                <button class="ngf-btn-test" ></button>
                <button class="ngf-btn-export" onclick=""></button>
                <button class="ngf-btn-import" onclick=""></button>
                <button class="ngf-btn-send" onclick=""></button>
                <button class="ngf-btn-mail" onclick=""></button>
                <button class="ngf-btn-confirm" onclick=""></button>
                <button class="ngf-btn-close" onclick=""></button>
                <button class="ngf-btn-cancel" onclick=""></button>
                <button class="ngf-btn-save" onclick=""></button>
                <button class="ngf-btn-inquiry" onclick=""></button>
                <button class="ngf-btn-detail" onclick=""></button>
                <button class="ngf-btn-info" onclick=""></button>
                <button class="ngf-btn-print" onclick=""></button>
                <button class="ngf-btn-config" onclick=""></button>
                <button class="ngf-btn-view" onclick=""></button>
                <button class="ngf-btn-copy" onclick=""></button>

                <button class="ngf-btn-withdraw" onclick=""></button>
                <button class="ngf-btn-post" onclick=""></button>

                <button class="ngf-btn-reset" onclick=""></button>
                <button class="ngf-btn-clear" onclick=""></button>
                <button class="ngf-btn-back" onclick=""></button>
            </div>
            <div>
                <div class="input-group">
                    <input type="text" class="form-control" value="" />
                    <span class="input-group-btn">
                        <button class="btn btn-skin-primary" type="button">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </span>
                </div>

                <div class="input-group">
                    <input type="text" class="form-control" value="" />
                    <span class="input-group-btn">
                        <button class="btn btn-skin-primary" type="button">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </span>
                </div>
            </div>
        </div>

        <!--inquiry result grid-->
        <div class="ngf-data-area">
            <table id="gridItem" class="ngf-bootstrap-table" data-sort-name="item_no" >
                <thead>
                    <tr>
                        <th data-field="state" data-checkbox="true" data-width="10%"></th>
                        <th data-field="Id" data-sortable="true" data-visible="false" data-searchable="false">ID</th>
                        <th data-field="Item_No" data-sortable="true" data-formatter="itemNoFormatter" data-search-formatter="false" lang="lang_item_no" data-width="30%">Item NO.</th>
                        <th data-field="Description" data-sortable="true" lang="lang_description" data-width="60%">Description</th>
                    </tr>
                </thead>
            </table>
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
        var _opt;
        var currentMaintainItemId = null;
        var _SelectedItemIdList;

        var itemNoFormatter = function (value, row) {
            return '<a onclick="updateItem(\'' + row.Id + '\')" style="cursor:pointer">' + value + '</a>';
        };

        var inquiryItem = function () {
            var options = {
                "success": function (d) {
                    $("#gridItem").bootstrapTable("load", d);
                }
            };

            var itemNo = $('#tbxItemNoInquiry').val();
            $.callWebService("Inquiry", { 'itemNo': itemNo }, options);
            return true;
        };

        var updateItem = function (itemId) {
            var allItemList = $("#gridItem").bootstrapTable('getData');
            $.each(allItemList, function (i, item) {
                if (item.Id == itemId) {
                    $("#tbxItemNo").val(item.Item_No);
                    $("#tbxDescription").val(item.Description);
                    return false;
                }
            });

            $("#itemMaintainDialog").modal('show');
            _opt = "Update";
            currentMaintainItemId = itemId;
        };

        var createItem = function () {
            $("#tbxItemNo").val("");
            $("#tbxDescription").val("");
            $("#itemMaintainDialog").modal('show');
            _opt = "Create";
        };

        var saveItem = function () {
            var itemNo = $("#tbxItemNo").val();
            var description = $("#tbxDescription").val();
            if (itemNo == "") {
                $.dialog.showMessage({
                    "title": _CurrentLang["lang_error"],
                    "content": _CurrentLang["msg_item_no_can_not_empty"]
                });
                return false;
            }


            var options = {
                "success": function (d) {
                    $("#itemMaintainDialog").modal('hide');
                    $.dialog.showMessage({
                        "title": _CurrentLang["lang_success"],
                        "content": _CurrentLang["msg_save_success"]
                    });
                    inquiryItem();
                }
            };

            var mydata = {
                'id': currentMaintainItemId,
                'itemNo': itemNo,
                'description': description
            };

            var serviceFuncName = "UpdateItem";
            if (_opt == "Create") {
                serviceFuncName = "CreateItem";
            }

            $.callWebService(serviceFuncName, mydata, options);

            return true;
        }

        var deleteItem = function () {
            var selectedItems = $("#gridItem").bootstrapTable("getSelections");
            if (selectedItems.length == 0) {
                $.dialog.showMessage(
                    {
                        "title": _CurrentLang['lang_error'],
                        "content": _CurrentLang['msg_must_select_one_data']
                    }
                );
                return false;
            }

            _SelectedItemIdList = [];
            $.each(selectedItems, function (i, item) {
                _SelectedItemIdList.push(item.Id);
            });

            var confirmData = {
                "title": _CurrentLang["lang_confirm"],
                "content": _CurrentLang["msg_confirm_delete_data"],
                "okfuncname": "doDeleteItem",
            };
            $.dialog.showConfirm(confirmData);
            return false;
        };

        var doDeleteItem = function () {
            var options = {
                "success": function (d) {
                    $("#itemMaintainDialog").modal('hide');
                    $.dialog.showMessage({
                        "title": 'Success',
                        "content": 'Delete success'
                    });
                    inquiryItem();
                }
            };

            var mydata = {
                'idList': _SelectedItemIdList
            };

            $.callWebService("DeleteItems", mydata, options);
            return true;
        };

        var testHttpHandlerBase = function () {
            var options = {
                "success": function (d) {
                    alert(d);
                }
            };

            var mydata = {
                'action': 'GetUserInfo'
            };

            $.post('ItemInquiryHandler.ashx', mydata, function (data) {
                alert(data);

            }, "json");

            //$.ajax({
            //    url: 'ItemInquiryHandler.ashx',
            //    dataType: "json",
            //    type: "POST",
            //    contentType: "application/json;charset=utf-8",
            //    data: mydata,
            //    success: function (d) {
            //        alert(d);
            //    },
            //    error: function (e) {
            //        alert(e);
            //    }
            //});

            return true;
        };

        var testBusinessBase = function () {
            var options = {
                "success": function (d) {
                    alert(d);
                }
            };

            var mydata = {};

            var serviceFuncName = "GetUserInfo";

            $.callWebService(serviceFuncName, mydata, options);

            return true;
        };

        var pickDate = function (ctrl) {
            $.dialog.showMessage({ "content": $(ctrl).val()});
        }
    </script>
</body>
</html>
