(function ($, h, c) {
    var a = $([]),
        e = $.resize = $.extend($.resize, {}),
        i,
        k = "setTimeout",
        j = "resize",
        d = j + "-special-event",
        b = "delay",
        f = "throttleWindow";
    e[b] = 250;
    e[f] = true;
    $.event.special[j] = {
        setup: function () {
            if (!e[f] && this[k]) {
                return false;
            }
            var l = $(this);
            a = a.add(l);
            $.data(this, d, {
                w: l.width(),
                h: l.height()
            });
            if (a.length === 1) {
                g();
            }
        },
        teardown: function () {
            if (!e[f] && this[k]) {
                return false;
            }
            var l = $(this);
            a = a.not(l);
            l.removeData(d);
            if (!a.length) {
                clearTimeout(i);
            }
        },
        add: function (l) {
            if (!e[f] && this[k]) {
                return false;
            }
            var n;
            function m(s, o, p) {
                var q = $(this),
                    r = $.data(this, d);
                r.w = o !== c ? o : q.width();
                r.h = p !== c ? p : q.height();
                n.apply(this, arguments);
            }
            if ($.isFunction(l)) {
                n = l;
                return m;
            } else {
                n = l.handler;
                l.handler = m;
            }
        }
    };
    function g() {
        i = h[k](function () {
            a.each(function () {
                var n = $(this),
                    m = n.width(),
                    l = n.height(),
                    o = $.data(this, d);
                if (m !== o.w || l !== o.h) {
                    n.trigger(j, [o.w = m, o.h = l]);
                }
            });
            g();
        },
            e[b]);
    }
})(jQuery, this);

jQuery.extend({
    "callWebService": function (method, data, options, not_self_service) {
        var showMask = true;
        if (options && options.hasOwnProperty("show_mask")) {
            showMask = options.show_mask;
        }
        if (showMask) {
            $.dialog.showLoading();
        }
        
        var ssoToken = getQueryStringByName("SSOToken") ? getQueryStringByName("SSOToken") : "";
        if (!ssoToken) {
            window.location.href = "Login";
        }

        if (!method) {
            return false;
        }

        var url = "";
        if (not_self_service) {
            url = method;
        } else {
            url = (location.href + "").split("#")[0].split("?")[0];
            var tempList = url.split("/");
            var tempName = tempList[tempList.length - 1];
            var tempUrl = url.substring(0, url.length - tempName.length);
            var serviceName = tempName.split(".")[0] + "Service.asmx";
            url = tempUrl + serviceName + "/" + method;
        }

        var cusSuccess = (options && options.success) ? options.success : $.answer.success;
        var showError = true;
        if (options && options.hasOwnProperty("hide_error")) {
            showError = false;
        }
        var cusError = (options && options.error) ? options.error : $.answer.error;

        var async = true;
        if (options.async != undefined) {
            async = options.async;
        }
        $.ajax({
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            async: async,
            beforeSend: function (request) {
                request.setRequestHeader("SSOToken", ssoToken);
                request.setRequestHeader("Language", _Context.CurrentLang);
                if (getQueryStringByName("IsDebug") == "Y") {
                    request.setRequestHeader("IsDebug", "Y");
                    request.setRequestHeader("LocalDebugUrl", getQueryStringByName("LocalDebugUrl"));
                }
                if (getQueryStringByName("domain")) {
                    request.setRequestHeader("requestDomain", getQueryStringByName("domain"));
                }
            },
            success: function (d) {
                if (showMask) {
                    $.dialog.closeLoading();
                }                
                var resultData = d.d;
                if (!resultData.success && resultData.errorcode == "E0001") {
                    location.href = "Login";
                } else {
                    cusSuccess(resultData);
                }
            },
            error: function (e) {
                $.dialog.closeLoading();
                if (showError) {
                    cusError(e);
                }                
            }
        });
    },

    "answer": {
        "success": function (d) {
            alert("Success");            
        },

        "failed": function () {
            alert("Failed");

        },

        "error": function (e) {
            $.dialog.showMessage({
                "title": "error",
                "content": e.responseText
            });
        }
    }
});

jQuery.extend({
    "goto": function (url, useCurrentMap, map) {
        var finalUrl = url;
        var ssoToken = getQueryStringByName("SSOToken") ? getQueryStringByName("SSOToken") : "";
        finalUrl += (finalUrl.indexOf("?") >= 0 ? "&" : "?") + "SSOToken=" + ssoToken;

        if (useCurrentMap) {
            var currentMap = $.uriAnchor.makeAnchorMap();
            finalUrl += "#!lang=" + currentMap["lang"] + "&skin=" + currentMap["skin"];            
        } else {
            if (map) {
                finalUrl += "#!lang=" + map["lang"] + "&skin=" + map["skin"];
            }
        }
        window.location.href = finalUrl;
    }
});

var _Context = {
    "SystemMode": "M",
    "AuthMode": "WFK",
    "IsCleanVersion": false,
    "CurrentFunctionId": "",
    "CurrentLang": "EnUS",
    "CurrentSkin": "blue",
    "init": function () {
        $(".ngf-input-area").addClass("ngf-page-row row");
        $(".ngf-data-area").addClass("ngf-page-row row");
        $(".ngf-inputbar").addClass(" col-lg-12 col-md-12 col-sm-12 col-xs-12");
        $(".ngf-toolbar").addClass(" col-lg-12 col-md-12 col-sm-12 col-xs-12");       

        //op area
        $(".ngf-input-area label, .ngf-modal label").addClass("ngf-label");
        $(".ngf-form-row-2 label").addClass("col-lg-2 col-md-2 col-sm-12 col-xs-12");
        $(".ngf-form-row-2 input[type='text'], .ngf-form-row-2 select").addClass("col-lg-4 col-md-4 col-sm-12 col-xs-12");
        $(".ngf-form-row-1 input[type='text'], .ngf-form-row-2 select").addClass("col-lg-12 col-md-12 col-sm-12 col-xs-12");
        $(".ngf-input-area input[type='text'], .ngf-input-area select").addClass("ngf-form-control");

        //control
        if ($('.ngf-datepicker').length > 0) {
            $('.ngf-datepicker').each(function () {
                $(this).val($(this).val()).datepicker({
                    autoclose: true,
                    format: $(this).attr("data-format") ? $(this).attr("data-format") : "yyyy/mm/dd"
                }); 
            });
        }

        if ($('.ngf-timepicker').length > 0) {
            $('.ngf-timepicker').addClass("timepicker");
            $('.ngf-timepicker').timepicker({
                showInputs: false,
                template: 'dropdown'
            });
        }

        if ($(".ngf-bootstrap-table").length > 0) {
            $('.ngf-bootstrap-table').each(function () {
                $(this).attr({
                    "data-toggle": $(this).attr("data-toggle") ? $(this).attr("data-toggle") : "table",
                    "data-toolbar": $(this).attr("data-toolbar") ? $(this).attr("data-toolbar") : $(this).attr("id") + "_toolbar",
                    "data-height": $(this).attr("data-height") ? $(this).attr("data-height") : "410",
                    "data-pagination": $(this).attr("data-pagination") ? $(this).attr("data-pagination") : "true",
                    "data-show-refresh": $(this).attr("data-show-refresh") ? $(this).attr("data-show-refresh") : "false",
                    "data-search": $(this).attr("data-search") ? $(this).attr("data-search") : "false",
                    "data-show-toggle": $(this).attr("data-show-toggle") ? $(this).attr("data-show-toggle") : "false",
                    "data-show-refresh": $(this).attr("data-show-refresh") ? $(this).attr("data-show-refresh") : "false",
                    "data-sortable": $(this).attr("data-sortable") ? $(this).attr("data-sortable") : "true",
                    "data-striped": $(this).attr("data-striped") ? $(this).attr("data-striped") : "true",
                    "data-page-size": $(this).attr("data-page-size") ? $(this).attr("data-page-size") : "10",
                    "data-page-list": $(this).attr("data-page-list") ? $(this).attr("data-page-list") : "[5,10,20]",
                    "data-click-to-select": $(this).attr("data-click-to-select") ? $(this).attr("data-click-to-select") : "true",
                    "data-single-select": $(this).attr("data-single-select") ? $(this).attr("data-single-select") : "false"
                });
            });
        }

        //btn style
        this.initButtons();

        $.each($("[class^='ngf-btn']"), function (i, btn) {
            if (!$(btn).attr("type")) {
                $(btn).attr("type", "button");
            }
        });
        //tbx style
        $(".ngf-tbx").addClass("form-control");

        //dialog style
        $.each($(".ngf-modal"), function (i, modal) {
            $(modal).addClass("modal fade");
            var header = $(modal).find(".ngf-modal-header").prop("outerHTML");
            var body = $(modal).find(".ngf-modal-body").prop("outerHTML");
            var footer = $(modal).find(".ngf-modal-footer").prop("outerHTML");
            $(modal).html("<div class='modal-dialog'><div class='modal-content'></div></div>");
            $(modal).find(".modal-content").append(header).append(body).append(footer);

            $(modal).find(".ngf-modal-header").prepend('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>').addClass("modal-header");
            $(modal).find(".ngf-modal-body").addClass("modal-body");
            $(modal).find(".ngf-modal-footer").addClass("modal-footer");
        });

        //area
        $.each($(".ngf-data-area"), function (i, area) {
            var ch = $(area).children().detach();
            var added = $(area).append("<div style='padding:0px 15px 0px 15px'></div>");
            added.children().append(ch);
        });

        $.language.change(this.CurrentLang);
        $.skin.change(this.CurrentSkin);    
    },

    "initButtons": function () {
        $(".ngf-btn-inquiry").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_inquiry").html("<span class='glyphicon glyphicon-search'></span>&nbsp;Inquiry");
        $(".ngf-btn-add").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_add").html("<span class='glyphicon glyphicon-plus'></span>&nbsp;Add");
        $(".ngf-btn-create").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_create").html("<span class='glyphicon glyphicon-plus'></span>&nbsp;Create");
        $(".ngf-btn-delete").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-danger ngf-btn-90")
            .attr("lang", "lang_delete").html("<span class='glyphicon glyphicon-minus'></span>&nbsp;Delete");
        $(".ngf-btn-save").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_save").html("<span class='glyphicon glyphicon-floppy-saved'></span>&nbsp;Save");
        $(".ngf-btn-cancel").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-default ngf-btn-90")
            .attr("lang", "lang_cancel").html("<span class='glyphicon glyphicon-share-alt'></span>&nbsp;Cancel");
        $(".ngf-btn-close").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-default ngf-btn-90")
            .attr("lang", "lang_close").html("<span class='glyphicon glyphicon-share-alt'></span>&nbsp;Close");
        $(".ngf-btn-confirm").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_confirm").html("<span class='glyphicon glyphicon-ok'></span>&nbsp;Confirm");
        $(".ngf-btn-test").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-warning ngf-btn-90")
            .attr("lang", "lang_test").html("<span class='glyphicon glyphicon-warning-sign'></span>&nbsp;Test");
        $(".ngf-btn-export").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_export").html("<span class='glyphicon glyphicon-export'></span>&nbsp;Export");
        $(".ngf-btn-import").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_import").html("<span class='glyphicon glyphicon-import'></span>&nbsp;Import");
        $(".ngf-btn-send").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_send").html("<span class='glyphicon glyphicon-send'></span>&nbsp;Send");
        $(".ngf-btn-mail").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_mail").html("<span class='glyphicon glyphicon-envelope'></span>&nbsp;Mail");
        $(".ngf-btn-detail").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_detail").html("<span class='glyphicon glyphicon-list-alt'></span>&nbsp;Detail");
        $(".ngf-btn-edit").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_edit").html("<span class='glyphicon glyphicon-edit'></span>&nbsp;Edit");
        $(".ngf-btn-info").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-warning ngf-btn-90")
            .attr("lang", "lang_info").html("<span class='glyphicon glyphicon-info-sign'></span>&nbsp;Info");
        $(".ngf-btn-print").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_print").html("<span class='glyphicon glyphicon-print'></span>&nbsp;Print");
        $(".ngf-btn-config").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_config").html("<span class='glyphicon glyphicon-cog'></span>&nbsp;Config");

        $(".ngf-btn-copy").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_copy").html("<span class='fa fa-fw fa-copy'></span>&nbsp;Copy");
        $(".ngf-btn-view").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_view").html("<span class='glyphicon glyphicon-eye-open'></span>&nbsp;View");
        $(".ngf-btn-withdraw").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-danger ngf-btn-120")
            .attr("lang", "lang_withdraw").html("<span class='fa fa-fw fa-hand-stop-o'></span>&nbsp;Withdraw");
        $(".ngf-btn-post").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_post").html("<span class='fa fa-fw fa-send-o'></span>&nbsp;Post");

        $(".ngf-btn-reset").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_reset").html("<span class='fa fa-fw fa-repeat'></span>&nbsp;Reset");
        $(".ngf-btn-clear").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_clear").html("<span class='fa fa-fw fa-undo'></span>&nbsp;Clear");
        $(".ngf-btn-back").removeClass("btn btn-skin-primary btn-default").addClass("btn btn-skin-primary ngf-btn-90")
            .attr("lang", "lang_back").html("<span class='fa fa-fw fa-mail-reply'></span>&nbsp;Back");
    }
};
var _CurrentLang = window._Lang_ZhCN ? window._Lang_ZhCN : {};

$(function () {
    $(window).bind('hashchange', $.NGFEvent.onHashChange);

    var map = $.uriAnchor.makeAnchorMap();
    var needSetAnchor = false;
    if (!map["lang"]) {
        map["lang"] = $.language.type.default;        
        needSetAnchor = true;
    }

    if (!map["skin"]) {
        map["skin"] = $.skin.type.default;
        needSetAnchor = true;
    }

    _Context.CurrentLang = map["lang"];
    _Context.CurrentSkin = map["skin"];

    if (needSetAnchor) {
        $.uriAnchor.setAnchor(map);
    }    

    _Context.init();
});

//start proxy function
var setSkin = function (skinName) {
    _Context.CurrentSkin = skinName;
    $.skin.set(skinName);
    return false;
}

var setLanguage = function (languageName) {
    _Context.CurrentLang = languageName;
    $.language.set(languageName);
    return false;
}

var closeConfirm = function (times) {
    $.dialog.closeConfirm(times);
};

var showLoading = function (times) {
    $.dialog.showLoading(times);
};

var closeLoading = function (times) {
    $.dialog.closeLoading(times);
};

var showDialogOnTop = function (dialogId, dialogHtml, times) {
    $.dialog.showDialogOnTop(dialogId, dialogHtml, times);
};

//end proxy function

jQuery.extend({
    "NGFEvent": {
        "onHashChange": function () {
            var map = $.uriAnchor.makeAnchorMap();

            var languageName = map["lang"];
            if (!languageName) {
                languageName = $.language.type.default;
            }
            $.language.change(languageName);

            var skinName = map["skin"];
            if (!skinName) {
                skinName = $.skin.type.default;
            }
            $.skin.change(skinName);
        },

        "onUnload": function () {
            if (window.ConfirmClose || $("iframe").length <= 0) {
                window.opener = null;
                window.open('', '_self', '');
                window.close();
                return;
            }

            $.dialog.showConfirm(_CurrentLang['msg_confirm_close_window'], '', '',
                function () {
                    window.ConfirmClose = true;
                    window.opener = null;
                    window.open('', '_self', '');
                    window.close();
                },
                function () {
                    $.dialog.closeConfirm();
                    return false;
                });

            return false;
        },
    }
})

var NGFFrameworkMessage = {
    "SHOW_MESSAGE": "SHOW_MESSAGE",
    "CLOSE_MESSAGE": "CLOSE_MESSAGE",
    "SHOW_CONFIRM": "SHOW_CONFIRM",
    "CLOSE_CONFIRM": "CLOSE_CONFIRM",
    "SHOW_LOADING": "SHOW_LOADING",
    "CLOSE_LOADING": "CLOSE_LOADING",
    "NGF_CALLBACK": "NGF_CALLBACK",
    "CHANGE_STATE": "CHANGE_STATE"
};

window.onmessage = function (e) {
    e = e || event;
    try {
        var msg = $.parseJSON(e.data);
        switch (msg.message) {
            case NGFFrameworkMessage.NGF_CALLBACK:
                try {
                    eval("(" + msg.data + ")" + "()");
                } catch (e) { }
                break;
            case NGFFrameworkMessage.CHANGE_STATE:
                var state = msg.data;
                setLanguage(state.language);
                setSkin(state.skin);
                break;
        }
    } catch (e) { }
}

jQuery.extend({
    "dialog": {
        "dialog": {},
        "showMessage": function (data) {
            if ($("#messageDialog") && $("#messageDialog").html()) {
                if (!alertify.ngfAlert) {
                    alertify.dialog('ngfAlert', function () {
                        return {
                            main: function (message, title) {
                                this.message = message;
                                $(".ajs-header").html(title);
                            },
                            setup: function () {
                                return {
                                    buttons: [{
                                        text: _CurrentLang["lang_close"],
                                        key: 27/*Esc*/,
                                        className: "ngf-btn-close"
                                    }],
                                    focus: { element: 0 }
                                };
                            },
                            prepare: function () {
                                this.setContent(this.message);
                            },
                            hooks: {                               
                                onshow: function () {
                                    _Context.initButtons();
                                    $(".alertify .ngf-btn-close").setLanguage();
                                    $(".alertify .ngf-btn-close span").on("click", function () {
                                        alertify.ngfAlert().close();
                                    });
                                }
                            }
                        }
                    });                    
                }
                if (!data.title) {
                    data.title = _CurrentLang["lang_message"];
                }
                alertify.ngfAlert(data.content, data.title);
            } else {
                if (window.parent) {
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.SHOW_MESSAGE,
                        "data": data
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }
        },
        "closeMessage": function () {
            if ($("#messageDialog") && $("#messageDialog").html()) {
                $("#messageDialog").modal('hide');
            } else {
                if (window.parent) {
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.CLOSE_MESSAGE
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }
        },
        "showConfirm": function (data, wname) {
            if ($("#confirmDialog") && $("#confirmDialog").html()) {
                $("#confirmDialogContent").html("");
                $("#confirmDialogWarningContent").html("");

                if (data.content) {
                    $("#confirmDialogContent").html(data.content);
                }

                if (data.warning) {
                    $("#confirmDialogWarningContent").html(data.warning);
                }

                $("#btnConfirmDialogConfirm").off('click');
                $("#btnConfirmDialogCancel").off('click');

                if (data.oktodo) {
                    $("#btnConfirmDialogConfirm").on('click', data.oktodo);
                }

                if (data.canceltodo) {
                    $("#btnConfirmDialogCancel").on('click', data.canceltodo);
                }

                if (wname && window.frames[wname]) {
                    var win = window.frames[wname];
                    if (data.okfuncname) {                        
                        $("#btnConfirmDialogConfirm").on('click', function () {
                            var msg = {
                                "message": NGFFrameworkMessage.NGF_CALLBACK,
                                "data": data.okfuncname
                            };
                            win.postMessage(JSON.stringify(msg), "*");
                        });
                    }

                    if (data.okfunc) {
                        $("#btnConfirmDialogConfirm").on('click', function () {
                            var msg = {
                                "message": NGFFrameworkMessage.NGF_CALLBACK,
                                "data": data.okfunc
                            };
                            win.postMessage(JSON.stringify(msg), "*");
                        });
                    }

                    if (data.cancelfuncname) {
                        $("#btnConfirmDialogCancel").on('click', function () {
                            var msg = {
                                "message": NGFFrameworkMessage.NGF_CALLBACK,
                                "data": data.cancelfuncname
                            };
                            win.postMessage(JSON.stringify(msg), "*");
                        });
                    }

                    if (data.cancelfunc) {
                        $("#btnConfirmDialogCancel").on('click', function () {
                            var msg = {
                                "message": NGFFrameworkMessage.NGF_CALLBACK,
                                "data": data.cancelfunc
                            };
                            win.postMessage(JSON.stringify(msg), "*");
                        });
                    }
                }

                $("#confirmDialog .ngf-btn-90").each(function () {
                    $(this).setLanguage();
                });
                $("#confirmDialog").modal('show');
            } else {
                if (window.parent) {
                    if (data.okfunc) {
                        data.okfunc = data.okfunc.toString().replace(/\r\n/g, "");
                    }
                    if (data.cancelfunc) {
                        data.cancelfunc = data.cancelfunc.toString().replace(/\r\n/g, "");;
                    }
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.SHOW_CONFIRM,
                        "data": data
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }
        },
        "closeConfirm": function () {
            if ($("#confirmDialog") && $("#confirmDialog").html()) {
                $("#confirmDialog").modal('hide');
            } else {
                if (window.parent) {
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.CLOSE_CONFIRM
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }            
        },
        "showDialog": function (dialogId, option) {
            if (option && option.manual_close) {
                $("#" + dialogId).modal({
                    backdrop: false,//点击空白处不关闭对话框
                    keyboard: false,//键盘关闭对话框
                    show: true//弹出对话框
                });
            } else {
                $("#" + dialogId).modal('show');
            }            
        },
        "closeDialog": function (dialogId) {
            $("#" + dialogId).modal('hide');
        },
        "showDialogOnTop": function (dialogId, dialogHtml, times) {
            if (parent && parent.showDialogOnTop) {
                if (!dialogHtml) {
                    dialogHtml = $("<div></div>").append($("#" + dialogId).clone()).html();
                }
                if (!times) {
                    var times = 1;
                    parent.showDialogOnTop(dialogId, dialogHtml, times);
                    return;
                }
            }
            $("#customerDialogContainer").html(dialogHtml);
            $("#" + dialogId).modal('show');
        },
        "showLoading": function () {
            if ($("#loader") && $("#loader").html()) {
                $("#loader-mask").show();
                $("#loader").show();
            } else {
                if (window.parent) {
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.SHOW_LOADING
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }
        },
        "closeLoading": function () {
            if ($("#loader") && $("#loader").html()) {
                $("#loader-mask").hide();
                $("#loader").hide();
            } else {
                if (window.parent) {
                    var msg = {
                        "wname": window.name,
                        "message": NGFFrameworkMessage.CLOSE_LOADING
                    };
                    window.parent.postMessage(JSON.stringify(msg), "*");
                }
            }
        }
    }
});

var CustomerDialogOption = function () {
    this.ElementId = null,
    this.ElementType = null,
    this.ElementAction = null
};

jQuery.extend({
    "language": {
        "type": {
            "default": "ZhCN",
            "ZhCN": "ZhCN",
            "ZhTW": "ZhTW",
            "EnUS": "EnUS"
        },
        "set": function (languageName) {
            if (!$.language.type[languageName]) {
                languageName = $.language.type.default;
            }

            var map = $.uriAnchor.makeAnchorMap();
            if (map["lang"] != languageName) {
                map["lang"] = languageName;
                $.uriAnchor.setAnchor(map);
            }           
        },
        "change": function (languageName) {
            _Context.CurrentLang = languageName;
            switch (_Context.CurrentLang) {
                case "ZhCN":
                    _CurrentLang = window._Lang_ZhCN;
                    break;
                case "ZhTW":
                    _CurrentLang = window._Lang_ZhTW;
                    break;
                default:
                    _CurrentLang = window._Lang_EnUS;
                    break;
            }
            for (key in _CurrentLang) {
                $.each($("[lang=" + key + "]"), function (i, item) {
                    if (typeof ($(item).attr("placeholder")) == "undefined") {
                        if ($(item).children(".fa").length > 0) {
                            var icons = $(item).children(".fa").detach();
                            $(item).html("").append(icons).append("&nbsp;" + _CurrentLang[key]);
                        } else if ($(item).children(".glyphicon").length > 0) {
                            var icons = $(item).children(".glyphicon").detach();
                            $(item).html("").append(icons).append("&nbsp;" + _CurrentLang[key]);
                        } else {
                            $(item).text(_CurrentLang[key]);
                        }

                        if ($(item).attr("type") == "submit" || $(item).attr("type") == "button") {
                            $(item).val(_CurrentLang[key]);
                        }
                    } else {
                        $(item).attr("placeholder", _CurrentLang[key]);
                    }
                });                                                              
            }

            switch (_Context.CurrentLang) {
                case "ZhCN":
                    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-CN']);
                    $.each($(".ngf-bootstrap-table"), function (i, tb) {
                        if ($(tb).find("tbody").length > 0) {
                            $(tb).bootstrapTable({ locale: 'zh-CN' });
                            $(tb).bootstrapTable("changeLocale");
                            //$(tb).bootstrapTable("refreshOptions");
                        }
                    });                    
                    break;
                case "ZhTW":
                    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['zh-TW']);
                    $.each($(".ngf-bootstrap-table"), function (i, tb) {
                        if ($(tb).find("tbody").length > 0) {
                            $(tb).bootstrapTable({ locale: 'zh-TW' });
                            $(tb).bootstrapTable("changeLocale");
                            //$(tb).bootstrapTable("refreshOptions");
                        }
                    });  
                    break;
                case "EnUS":
                    $.extend($.fn.bootstrapTable.defaults, $.fn.bootstrapTable.locales['en-US']);
                    $.each($(".ngf-bootstrap-table"), function (i, tb) {
                        if ($(tb).find("tbody").length > 0) {
                            $(tb).bootstrapTable({ locale: 'en-US' });
                            $(tb).bootstrapTable("changeLocale");
                            //$(tb).bootstrapTable("refreshOptions");
                        }
                    });  
                    break;
            }
            
            //$.getScript("bootstrap-table.js", function (data, textStatus, jqxhr) {
                //console.log(data); // Data returned
                //console.log(textStatus); // Success
                //console.log(jqxhr.status); // 200
                //console.log("Load was performed.");
            //});

            if (this.onchanged) {
                this.onchanged();
            }
        },
        "onchanged": function () {
        }
    }
});

jQuery.fn.extend({
    setLanguage: function (lang) {
        if ($(this).attr("lang") && $.trim($(this).attr("lang")) != "") {
            var _TempLanguage;
            if (lang) {                
                switch (_Context.CurrentLang) {
                    case "ZhCN":
                        _TempLanguage = window._Lang_ZhCN;
                        break;
                    case "ZhTW":
                        _TempLanguage = window._Lang_ZhTW;
                        break;
                    default:
                        _TempLanguage = window._Lang_EnUS;
                        break;
                }
            } else {
                _TempLanguage = _CurrentLang;
            }
            var key = $(this).attr("lang");
            if (typeof ($(this).attr("placeholder")) == "undefined") {
                if ($(this).children(".fa").length > 0) {
                    var icons = $(this).children(".fa").detach();
                    $(this).html("").append(icons).append("&nbsp;" + _TempLanguage[key]);
                } else if ($(this).children(".glyphicon").length > 0) {
                    var icons = $(this).children(".glyphicon").detach();
                    $(this).html("").append(icons).append("&nbsp;" + _TempLanguage[key]);
                } else {
                    $(this).text(_TempLanguage[key]);
                }
            } else {
                $(this).attr("placeholder", _TempLanguage[key]);
            }
        }
    }
}); 

jQuery.extend({
    "skin": {
        "type": {
            "default": "blue-light",
            "blue": "blue",
            "blue-light": "blue-light",
            "yellow": "yellow",
            "yellow-light": "yellow-light",
            "green": "green",
            "green-light": "green-light",
            "red": "red",
            "red-light": "red-light",
            "purple": "purple",
            "purple-light": "purple-light",
            "black": "black",
            "black-light": "black-light"
        },
        "set": function (skinName) {
            if (!$.skin.type[skinName]) {
                skinName = $.skin.type.default;
            }

            var map = $.uriAnchor.makeAnchorMap();
            if (map["skin"] != skinName) {
                map["skin"] = skinName;
                $.uriAnchor.setAnchor(map);
            }
        },
        "change": function (skinName) {
            _Context.CurrentSkin = skinName;
            $("body").removeClass().addClass("skin-" + skinName).addClass("fixed");
        }
    }
});

//Common Function
function getQueryString() {
    var result = location.search.match(new RegExp("[\?\&][^\?\&]+=[^\?\&]+", "g"));
    for (var i = 0; i < result.length; i++) {
        result[i] = result[i].substring(1);
        result[i] = { "key": result[i].split("=")[0], "value": result[i].split("=")[2] };
    }
    return result;
}

function getQueryStringFromSearch(url) {
    var result = url.match(new RegExp("[\?\&][^\?\&]+=[^\?\&]+", "g"));
    if (result != null) {
        for (var i = 0; i < result.length; i++) {
            result[i] = result[i].substring(1);
            result[i] = { "key": result[i].split("=")[0], "value": result[i].split("=")[1] };
        }
    } else {
        return [];
    }
    return result;
}

//根据QueryString参数名称获取值 
function getQueryStringByName(name) {
    var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
    if (result == null || result.length < 1) {
        return "";
    }
    return result[1];
}
//根据QueryString参数索引获取值 

function getQueryStringByIndex(index) {
    if (index == null) {
        return "";
    }
    var queryStringList = getQueryString();
    if (index >= queryStringList.length) {
        return "";
    }
    var result = queryStringList[index];
    var startIndex = result.indexOf("=") + 1;
    result = result.substring(startIndex);
    return result;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}    

function escapeHtmlData(data) {
    if (typeof data == 'object' || $.isArray(data)) {
        for (p in data) {
            data[p] = escapeHtmlData(data[p]);
        }
    }
    else if (typeof data == 'string') {
        data = data.replace(/</g, "&lt;"); //把符号"<" 替换成"&lt;"
        data = data.replace(/>/g, "&gt;"); //把符号">" 替换成"&gt;"
    }
    return data;
}

function getUrlQueryStringSplit(url) {
    if (url.split("?").length > 1) {
        return "&";
    } else {
        return "?";
    }
}