<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemInquiry.aspx.cs" Inherits="NGF.Demo.Web.ItemInquiry" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/bootstrap.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/AdminLTE/AdminLTE.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/AdminLTE/skins/_all-skins.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/datepicker/datepicker3.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/timepicker/bootstrap-timepicker.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/bootstrap-table.min.css">
    <link rel="stylesheet" href="http://aic0-s2.benq.corp.com:8023/NGF/Content/ngf.framework.css">
</head>
<body class="skin-red">
    <form id="form1" runat="server">
        <!--inquiry area & toolbar-->
        <div class="ngf-input-area">
            <div class="ngf-inputbar">                
                <div class="ngf-form-row-1">
                    <label for="tbxItemNoInquiry" lang="lang_item_no"></label>
                    <%--<div class="input-group">
                        <input type="text" class="form-control"
                            id="tbxItemNoInquiry" value="" />
                        <span class="input-group-btn">
                            <button class="ngf-btn-inquiry" onclick="return inquiryItem();"></button>
                        </span>
                    </div>--%>
                    <input type="text" class="ngf-tbx"
                            id="tbxItemNoInquiry" value="" />
                </div>
                <div class="ngf-form-row-1">
                    <label for="tbxDescriptionInquiry" lang="lang_description"></label>
                    <input type="text" class="ngf-tbx"
                            id="tbxDescriptionInquiry" value="" />
                </div>
            </div>
            <div class="ngf-toolbar">
                <button class="ngf-btn-inquiry" onclick="return inquiryItem();"></button>
                <button class="ngf-btn-add" onclick="return createItem();"></button>
                <button class="ngf-btn-delete" onclick="return deleteItem();"></button>
            </div>
        </div>

        <!--inquiry result grid-->
        <div class="ngf-data-area">
            <table id="gridItem" class="ngf-bootstrap-table" data-sort-name="item_no" data-url="" >
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

        <!--edit dialog-->
        <div id="itemMaintainDialog" class="ngf-modal">
            <div class="ngf-modal-header">
                <h1 class="modal-title" lang="lang_edit"></h1>
            </div>
            <div class="ngf-modal-body">
                <label for="tbxItemNo" lang="lang_item_no"></label>
                <input type="text" class="ngf-tbx ngf-tbx-required" id="tbxItemNo" value="" />
                <label for="tbxDescription" lang="lang_description"></label>
                <input type="text" class="ngf-tbx" id="tbxDescription" value="" />
                <!--<label for="">Date From</label>
                <input type="text" class="ngf-tbx" id="" value="" />
                <label for="">Date To</label>
                <input type="text" class="ngf-tbx" id="" value="" />
                <label for="">Time From</label>
                <input type="text" class="ngf-tbx" id="" value="" />
                <label for="">Time To</label>
                <input type="text" class="ngf-tbx" id="" value="" />
                <label for="">Note</label>
                <input type="text" class="ngf-tbx" id="" value="" />-->
            </div>
            <div class="ngf-modal-footer">
                <button class="ngf-btn-save" onclick="return saveItem()"></button>
                <button class="ngf-btn-cancel" data-dismiss="modal"></button>
            </div>
        </div>


    </form>
    <script src="http://aic0-s2.benq.corp.com:8023/NGF/Scripts/jquery-2.2.3.min.js"></script>
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
    </script>
</body>
</html>
