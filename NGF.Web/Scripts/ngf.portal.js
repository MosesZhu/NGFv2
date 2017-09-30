jQuery.extend({
	"ngf": {
		"portal": {
			"ui": {
				"resetContentSize": function () {
					var headerHeight = $("header").height();
					if (!_PortalContext.HeaderVisible) {
						headerHeight = 0;
                    }
                    if (headerHeight == 100 && $(window).width() >= 768) {
                        headerHeight = 50;
                    }

                    $(".main-sidebar").animate({ "padding-top": headerHeight + "px" }, 200);
                    $("#bookmark_sidebar").animate({ "padding-top": headerHeight + "px" }, 200);
                    $("#control_sidebar").animate({ "padding-top": headerHeight + "px" }, 200);

					var footerHeight = $(".main-footer").height();
					$("section.content").height($(window).height() - headerHeight - footerHeight - 72);

                    $(".content-wrapper").css("padding-top", headerHeight + "px");
        
					//$(".content-wrapper").animate({
					//	"padding-top": headerHeight + "px"
					//}, 200);
                    $("aside").height($(window).height() - headerHeight);


                    $(".frm_loading_mask").each(function (i, mask) {
                        var fid = $(mask).parent(".tab-pane").first().attr("id");
                        $(mask).height($("#" + fid).height());
                        $(mask).css("top", headerHeight + 56 + "px");
                    });                    
				},
				"header": {
					"toggleHeader": function () {
						if (_PortalContext.HeaderVisible) {
							this.hideHeader();
						} else {
							this.showHeader();
						}
					},

					"hideHeader": function () {
						var headerHeight = $("header").height();
						$(".main-sidebar").animate({ "padding-top": "0px" }, 200);
						$("#bookmark_sidebar").animate({ "padding-top": "0px" }, 200);
						$("#control_sidebar").animate({ "padding-top": "0px" }, 200);
						var contentHeight = $("section.content").height();
						$("section.content").height(contentHeight + headerHeight - 30);
						var menuHeight = $("aside > .slimScrollDiv").height();
						$("aside > .slimScrollDiv").height(menuHeight + headerHeight);
						$("section.sidebar").height(menuHeight + headerHeight);

						$(".content-wrapper").animate({
							"padding-top": "0px"
						}, 200);

						$("header").slideUp(200, function () {
							$("#btnShowHeader").animate({ "top": "0px" }, 200);
							_PortalContext.HeaderVisible = false;
							_ui.resetContentSize();
						});
					},

					"showHeader": function () {
						var headerHeight = $("header").height();
						$("#btnShowHeader").animate({ "top": "-50px" }, 200, function () {
							$(".main-sidebar").animate({ "padding-top": headerHeight + "px" }, 200);
							$("#bookmark_sidebar").animate({ "padding-top": headerHeight + "px" }, 200);
							$("#control_sidebar").animate({ "padding-top": headerHeight + "px" }, 200);
							var contentHeight = $("section.content").height();
							$("section.content").height(contentHeight - headerHeight - 30);
							var menuHeight = $("aside > .slimScrollDiv").height();
							$("aside > .slimScrollDiv").height(menuHeight - headerHeight);
							$("section.sidebar").height(menuHeight - headerHeight);

							$(".content-wrapper").animate({
								"padding-top": headerHeight + "px"
							}, 200);

							$("header").slideDown(200, function () {
								_PortalContext.HeaderVisible = true;
								_ui.resetContentSize();
							});
						});
					}
				},
                "menu": {
                    "_BeforeSearchMenu": null,

                    "_LastSearchKeywords": null,

					"init": function () {
						var options = {
							"success": function (d) {
								if (d.success) {
									//menu muliti language
                                    if (d.data) {
                                        _PortalContext.SystemMode = d.data.NGFSystemMode;                                        
                                        $.each(d.data.LanguageList, function (i, lang) {
                                            _Lang_ZhCN[lang.Language_Key] = lang.Zh_Cn;
                                            _Lang_ZhTW[lang.Language_Key] = lang.Zh_Tw;
                                            _Lang_EnUS[lang.Language_Key] = lang.En_Us;
                                        });

                                        _PortalContext.MenuList = d.data.ProductList;
                                        _PortalContext.BookmarkList = d.data.BookmarkList;
                                        _menu.refresh();
                                    }									
								}
							}
						};

						$.callWebService("getMenu", {}, options);
					},

					"refresh": function () {
                        var menuHtml = "";
                        if (_PortalContext.SystemMode == "Mulity") {
                            try {
                                $.each(_PortalContext.MenuList, function (i, product) {
                                    if (!_PortalContext.IsCleanVersion) {
                                        menuHtml
                                            += '<li class="header">'
                                            + '<i class="fa fa-share-alt"></i>'
                                            + '<span lang="' + product.LanguageID + '" style="padding-left:5px;">' + product.Name + '</span>'
                                            + '</li>';
                                    } 

                                    $.each(product.DomainList, function (j, domainMenu) {
                                        menuHtml += _menu.getDomainMenuHtml(domainMenu);
                                    });

                                    $.each(product.SystemList, function (j, systemMenu) {
                                        menuHtml += _menu.getSystemMenuHtml(systemMenu);
                                    });

                                    $("#_FunctionMenu").removeClass("SingleModeMenu");
                                    if (_PortalContext.IsCleanVersion) {
                                        $("#_FunctionMenu").addClass("SingleModeMenu");
                                    }
                                });
                            } catch (error) { }
                            
                        } else {
                            try {
                                var system = _PortalContext.MenuList[0].DomainList[0].SystemList[0];
                                $.each(system.FunctionList, function (k, functionMenu) {
                                    menuHtml += _menu.getFunctionMenuHtml(functionMenu);
                                });
                                if (_PortalContext.MenuList[0].DomainList.length > 1) {
                                    var permissionDomain = _PortalContext.MenuList[0].DomainList[1];
                                    menuHtml += _menu.getDomainMenuHtml(permissionDomain);
                                }
                                $("#_FunctionMenu").addClass("SingleModeMenu");
                            } catch (error) { }
                            
                        }                       
						
						$("#_FunctionMenu").html(menuHtml);

						var bookmarkMenuHtml = _menu.getBookmarkMenuHtml();						
						$("#_BookmarkMenu").html(bookmarkMenuHtml);

						_cmenu.bindMenuContextMenu();

						$.language.change(_Context.CurrentLang);
					},

                    "getDomainMenuHtml": function (domainMenu) {
                        if (!_PortalContext.IsCleanVersion) {
                            var menuHtml = '<li class="treeview">'
                                + '<a>'
                                + '<i class="fa fa-bank text-blue"></i>'
                                + '<span lang="' + domainMenu.Language_Key + '">' + domainMenu.Code + '</span>'
                                + '<span class="pull-right-container">'
                                + '<i class="fa fa-angle-left pull-right"></i>'
                                + '</span>'
                                + '</a>';
                            var menuHtml = "";
                            menuHtml += '<ul class="treeview-menu">';
                            $.each(domainMenu.SystemList, function (k, systemMenu) {
                                menuHtml += _menu.getSystemMenuHtml(systemMenu);
                            });
                            menuHtml += '</ul></li>';
                            return menuHtml;
                        } else {
                            var menuHtml = "";
                            $.each(domainMenu.SystemList, function (k, systemMenu) {
                                menuHtml += _menu.getSystemMenuHtml(systemMenu);
                            });
                            return menuHtml;
                        }
					},

					"getSystemMenuHtml": function (systemMenu) {
						var menuHtml = '<li class="treeview">'
							+ '<a>'
							+ '<i class="fa fa-laptop text-blue"></i>'
							+ '<span lang="' + systemMenu.Language_Key + '">' + systemMenu.Code + '</span>'
							+ '<span class="pull-right-container">'
							+ '<i class="fa fa-angle-left pull-right"></i>'
							+ '</span>'
							+ '</a>';
						menuHtml += '<ul class="treeview-menu">';
						$.each(systemMenu.FunctionList, function (k, functionMenu) {
							menuHtml += _menu.getFunctionMenuHtml(functionMenu);
						});
						menuHtml += '</ul></li>';
						return menuHtml;
					},

					"getFunctionMenuHtml": function (functionMenu) {
						var menuHtml = '';
						if (functionMenu.SubFunctionList && functionMenu.SubFunctionList.length > 0) {
							menuHtml += '<li class="treeview">'
								+ '<a>'
								+ '<i class="fa fa-puzzle-piece text-blue"></i>'
								+ '<span lang="' + functionMenu.Language_Key + '" data-toggle="tooltip" data-placement="top" title="' + functionMenu.Code + '">' + functionMenu.Code + '</span>'
								+ '<span class="pull-right-container">'
								+ '<i class="fa fa-angle-left pull-right"></i>'
								+ '</span>'
								+ '</a>';
							menuHtml += '<ul class="treeview-menu">';
							$.each(functionMenu.SubFunctionList, function (k, subFunctionMenu) {
								menuHtml += _menu.getFunctionMenuHtml(subFunctionMenu);
							});
							menuHtml += '</ul></li>';
						} else {
							menuHtml += '<li onclick="return _form.openForm(this);" functionid="' + functionMenu.Id + '" ';
							if (functionMenu.Url) {
								menuHtml += ' functionurl="'//' functionurl="http://'
									+ functionMenu.Url + '" ';
							}
							menuHtml += '>'
								+ '<a class="function_menu_item">'
								+ '<i class="fa fa-circle-o text-light-blue"></i>'
								+ '<span lang="' + functionMenu.Language_Key + '" data-toggle="tooltip" data-placement="top" title="' + functionMenu.Code + '">' + functionMenu.Code + '</span>'
								+ '</a>'
								+ '</li>';
						}

						return menuHtml;
					},

					"getBookmarkMenuHtml": function () {
						var bookmarkMenuHtml = "";
						var tempBookmarkSystemList = new Array();
						$.each(_PortalContext.BookmarkList, function (i, bookmark) {
							var bookmarkSystemId = bookmark.System_Id;
							$.each(_PortalContext.MenuList, function (i, product) {
								$.each(product.DomainList, function (j, domain) {
									$.each(domain.SystemList, function (j, system) {
										if (system.Id == bookmarkSystemId) {
											var exist = false;
											for (var x = 0; x < tempBookmarkSystemList.length; x++) {
												if (tempBookmarkSystemList[x].System.Id == bookmarkSystemId) {
													exist = true;
													tempBookmarkSystemList[x].BookmarkList.push(bookmark);
													break;
												}
											}
											if (!exist) {
												tempBookmarkSystemList.push({
													"System": system,
													"BookmarkList": [bookmark]
												});
											}
											return false;
										}
									});
								});
							});
							//bookmarkMenuHtml += _menu.getBookmarkMenuItemHtml(bookmark);
						});

						$.each(tempBookmarkSystemList, function (i, system) {
							bookmarkMenuHtml += '<li class="header text-blue">'
								+ '<i class="fa fa-laptop text-blue"></i>'
								+ '<span lang="' + system.System.Language_Key + '" style="padding-left:5px;">' + system.System.Code + '</span>'
								+ '</li>';
							$.each(system.BookmarkList, function (k, bookmark) {
								bookmarkMenuHtml += _menu.getBookmarkMenuItemHtml(bookmark);
							});
							bookmarkMenuHtml += '</ul></li>';
						});

						return bookmarkMenuHtml;
					},

					"getBookmarkMenuItemHtml": function (bookmarkMenu) {
						var menuHtml = "";
						menuHtml += '<li class="bookmark-item" onclick="return _form.openForm(this);" functionid="bk_' + bookmarkMenu.Id + '" ';
						if (bookmarkMenu.Url) {
							menuHtml += ' functionurl="'
								+ bookmarkMenu.Url + '" ';
						}
						menuHtml += '>'
							+ '<a style="padding-left: 30px;">'
							+ '<i class="fa fa-circle-o text-light-blue"></i>'
							+ '<span lang="' + bookmarkMenu.Language_Key + '" data-toggle="tooltip" data-placement="top" title="' + bookmarkMenu.Code + '">' + bookmarkMenu.Code + '</span>'
							+ '</a>'
							+ '</li>';
						return menuHtml;
					},

                    "searchMenu": function () {
                        if (this._LastSearchKeywords == null || this._LastSearchKeywords == "") {
                            this._BeforeSearchMenu = $("#_FunctionMenu").html();
                        }
                        var searchKeywords = $("#tbxSearchMenu").val().toUpperCase();
                        if (searchKeywords.length > 0) {
							var tempMenuList = $.extend(true, {}, _PortalContext.MenuList);
							$.each(tempMenuList, function (i, product) {
                                if (product.Name.toUpperCase().indexOf(searchKeywords) >= 0) {
                                    product.bingo = true;
                                    $.each(product.DomainList, function (i1, domain) {
                                        domain.bingo = true;
                                        $.each(domain.SystemList, function (i2, system) {
                                            system.bingo = true;
                                            $.each(system.FunctionList, function (i3, func) {
                                                _menu.searchSetFunctionMenuFound(func);
                                            });
                                        });
                                    });
                                    return true;
                                }

                                $.each(product.DomainList, function (j, domain) {
                                    if (domain.Code.toUpperCase().indexOf(searchKeywords) >= 0) {
                                        product.bingo = true;
                                        domain.bingo = true;
                                        $.each(domain.SystemList, function (j1, system) {
                                            system.bingo = true;
                                            $.each(system.FunctionList, function (j2, func) {
                                                _menu.searchSetFunctionMenuFound(func);
                                            });
                                        });
                                        return true;
                                    }

                                    $.each(domain.SystemList, function (k, system) {
                                        if (system.Code.toUpperCase().indexOf(searchKeywords) >= 0) {
                                            product.bingo = true;
                                            domain.bingo = true;
                                            system.bingo = true;
                                            $.each(system.FunctionList, function (k1, func) {
                                                _menu.searchSetFunctionMenuFound(func);
                                            });
                                            return true;
                                        }

                                        $.each(system.FunctionList, function (l, func) {
                                            if (func.Code.toUpperCase().indexOf(searchKeywords) >= 0
                                                || (_Lang_ZhCN[func.Language_Key] && _Lang_ZhCN[func.Language_Key].indexOf(search) >= 0)
                                                || (_Lang_ZhTW[func.Language_Key] && _Lang_ZhTW[func.Language_Key].indexOf(search) >= 0)
                                                || (_Lang_EnUS[func.Language_Key] && _Lang_EnUS[func.Language_Key].indexOf(search) >= 0)) {
                                                product.bingo = true;
                                                domain.bingo = true;
                                                system.bingo = true;
                                                func.bingo = true;
                                                _menu.searchSetFunctionMenuFound(func);
                                                return true;
                                            }

                                            if (func.SubFunctionList && func.SubFunctionList.length > 0) {
                                                $.each(func.SubFunctionList, function (i, subFunc) {
                                                    _menu.searchFunctionMenu(searchKeywords, subFunc, func, product, domain, system);
                                                });
                                            }
                                        });
                                    });
                                });								
							});

                            _menu.refreshSearchMenu(tempMenuList);
                            _menu.expandAllMenu();
                        } else {
                            if (this._BeforeSearchMenu != null) {
                                $("#_FunctionMenu").html(this._BeforeSearchMenu);
                            } else {
                                _menu.refresh();
                            }							
                        }

                        this._LastSearchKeywords = searchKeywords;
                    },

                    "searchSetFunctionMenuFound": function (functionMenu) {
                        functionMenu.bingo = true;
                        if (functionMenu.SubFunctionList && functionMenu.SubFunctionList.length > 0) {
                            $.each(functionMenu.SubFunctionList, function (i, subFunction) {
                                _menu.searchSetFunctionMenuFound(subFunction);
                            });
                        }
                    },

					"refreshSearchMenu": function (menuList) {
						var menuHtml = "";
						$.each(menuList, function (i, product) {
                            if (product.bingo) {
                                if (!_PortalContext.IsCleanVersion) {
                                    menuHtml
                                        += '<li class="header">'
                                        + '<i class="fa fa-bank"></i>'
                                        + '<span lang="' + product.LanguageID + '" style="padding-left:5px;">' + product.Name + '</span>'
                                        + '</li>';
                                } 								
                                $.each(product.DomainList, function (j, domain) {
                                    menuHtml += _menu.getSearchDomainMenuHtml(domain);

								});
							}
						});

						$("#_FunctionMenu").html(menuHtml);

						_cmenu.bindMenuContextMenu();

						$.language.change(_Context.CurrentLang);
					},

                    "getSearchDomainMenuHtml": function (domain) {
                        if (!_PortalContext.IsCleanVersion) {
                            var menuHtml = '';
                            if (domain.bingo) {
                                menuHtml = '<li class="treeview">'
                                    + '<a>'
                                    + '<i class="fa fa-bank text-blue"></i>'
                                    + '<span lang="' + domain.Language_Key + '">' + domain.Code + '</span>'
                                    + '<span class="pull-right-container">'
                                    + '<i class="fa fa-angle-left pull-right"></i>'
                                    + '</span>'
                                    + '</a>';
                                menuHtml += '<ul class="treeview-menu">';
                                $.each(domain.SystemList, function (i, system) {
                                    menuHtml += _menu.getSearchSystemMenuHtml(system);
                                });
                                menuHtml += '</ul></li>';
                            }
                            return menuHtml;
                        } else {
                            var menuHtml = "";
                            if (domain.bingo) {
                                $.each(domain.SystemList, function (i, system) {
                                    menuHtml += _menu.getSearchSystemMenuHtml(system);
                                });
                            }
                            return menuHtml;
                        }
					},

					"getSearchSystemMenuHtml": function (system) {
						var menuHtml = '';
                        if (system.bingo) {
							menuHtml = '<li class="treeview">'
								+ '<a>'
								+ '<i class="fa fa-laptop text-blue"></i>'
                                + '<span lang="' + system.Language_Key + '">' + system.Code + '</span>'
								+ '<span class="pull-right-container">'
								+ '<i class="fa fa-angle-left pull-right"></i>'
								+ '</span>'
								+ '</a>';
							menuHtml += '<ul class="treeview-menu">';
                            $.each(system.FunctionList, function (i, func) {
                                if (func.bingo) {
                                    menuHtml += _menu.getSearchFunctionMenuHtml(func);
                                }                                
							});
							menuHtml += '</ul></li>';
						}
						return menuHtml;
					},

                    "getSearchFunctionMenuHtml": function (func) {
						var menuHtml = '';
                        if (func.bingo) {
                            if (func.SubFunctionList && func.SubFunctionList.length > 0) {
								menuHtml += '<li class="treeview">'
									+ '<a>'
									+ '<i class="fa fa-puzzle-piece text-blue"></i>'
                                    + '<span lang="' + func.Language_Key + '">' + func.Code + '</span>'
									+ '<span class="pull-right-container">'
									+ '<i class="fa fa-angle-left pull-right"></i>'
									+ '</span>'
									+ '</a>';
								menuHtml += '<ul class="treeview-menu">';
                                $.each(func.SubFunctionList, function (i, subFunc) {
                                    if (subFunc.bingo) {
                                        menuHtml += _menu.getFunctionMenuHtml(subFunc);
                                    }                                    
								});
								menuHtml += '</ul></li>';
							} else {
                                menuHtml += '<li onclick="return _form.openForm(this);" functionid="' + func.Id + '" ';
                                if (func.Url) {
									menuHtml += ' functionurl="'
                                        + func.Url + '" ';
								}
								menuHtml += '>'
									+ '<a class="function_menu_item">'
									+ '<i class="fa fa-circle-o text-light-blue"></i>'
                                    + '<span lang="' + func.Language_Key + '">' + func.Code + '</span>'
									+ '</a>'
									+ '</li>';
							}
						}
						return menuHtml;
					},

                    "searchFunctionMenu": function (searchKeywords, subFunc, func, product, domain, system) {
                        if (subFunc.Code.toUpperCase().indexOf(searchKeywords) >= 0
                            || (_Lang_ZhCN[subFunc.Language_Key] && _Lang_ZhCN[subFunc.Language_Key].toUpperCase().indexOf(searchKeywords) >= 0)
                            || (_Lang_ZhTW[subFunc.Language_Key] && _Lang_ZhTW[subFunc.Language_Key].toUpperCase().indexOf(searchKeywords) >= 0)
                            || (_Lang_EnUS[subFunc.Language_Key] && _Lang_EnUS[subFunc.Language_Key].toUpperCase().indexOf(searchKeywords) >= 0)) {
                            product.bingo = true;
                            domain.bingo = true;
                            system.bingo = true;
                            func.bingo = true;
                            subFunc.bingo = true;
                            _menu.searchSetFunctionMenuFound(subFunc);
                            return true;
                        }

                        if (subFunc.SubFunctionList && subFunc.SubFunctionList.length > 0) {
                            $.each(subFunc.SubFunctionList, function (i, subSubFunc) {
                                _menu.searchFunctionMenu(searchKeywords, subSubFunc, subFunc, product, domain, system);
							});
						}
					},

                    "expandOneMenu": function (menu) {
                        if ($(menu).is("li.treeview")) {
                            $(menu).children("ul.treeview-menu").slideDown(200, function () {
                                _menu.expandOneMenu(this);
                            });
                        } else {
                            $.each($(menu).children("li.treeview"), function (i, l) {
                                $(l).children("ul.treeview-menu").slideDown(200, function () {
                                    _menu.expandOneMenu(this);
                                });
                            });
                        }						
                    },

                    "expandOneMenuByFunctionId": function (functionid) {
                        $.each($("#_FunctionMenu").find("ul.treeview-menu"), function (i, menu) {
                            if ($(menu).find("li[functionid = " + functionid + "]").length > 0) {
                                if ($(menu).css("display") == "block") {
                                    _menu.expandOneSubMenuByFunctionId(menu, functionid);
                                } else {
                                    $(menu).slideDown(500, function () {
                                        _menu.expandOneSubMenuByFunctionId(menu, functionid);
                                    });
                                }                                
                                return false;
                            }
                        });                   
                    },

                    "expandOneSubMenuByFunctionId": function (pMenu, functionid) {
                        $.each($(pMenu).find("ul.treeview-menu"), function (i, menu) {
                            if ($(menu).find("li[functionid = " + functionid + "]").length > 0) {
                                if ($(menu).css("display") == "block") {
                                    _menu.expandOneSubMenuByFunctionId(menu, functionid);
                                } else {
                                    $(menu).slideDown(200, function () {
                                        _menu.expandOneSubMenuByFunctionId(menu, functionid);
                                    });
                                }
                                return false;
                            }
                        });  
                    },

                    "expandOneMenuByContext": function (context) {
                        if ($(context).hasClass("header")) {
                            var current = $(context);
                            while (true) {
                                current = current.next("li");
                                if (!current || current.hasClass("header")) {
                                    break;
                                }
                                $(current).children("ul.treeview-menu").slideDown(200, function () {
                                    _menu.expandOneMenu(this);
                                });
                            }
                        } else {
                            $(context).children("ul.treeview-menu").slideDown(200, function () {
                                _menu.expandOneMenu(this);
                            });
                        }
                    },

                    "expandAllMenu": function () {
                        $.each($('#_FunctionMenu').children("li.treeview"), function (i, l) {
                            $(l).children("ul.treeview-menu").slideDown(200, function () {
                                _menu.expandOneMenu(this);
                            });
                        });
                    },

                    "collapseOneMenuByContext": function (context) {
                        if ($(context).hasClass("header")) {
                            var current = $(context);
                            while (true) {
                                current = current.next("li");
                                if (!current || current.hasClass("header")) {
                                    break;
                                }
                                current.find("ul").slideUp(500);
                            }
                        } else {
                            $(context).find("ul").slideUp(500);
                        }
                    },

                    "collapseAllMenu": function () {
                        $('#_FunctionMenu').find("ul").slideUp(500);
                    },

					"activeMenu": function (functionid) {
						if (functionid.substring(0, 3) == "bk_") {
							functionid = functionid.substring(3);
                        }
                        //_menu.collapseAllMenu();
                        $("#_FunctionMenu li").removeClass("active");
                        //_menu.expandOneMenu($("#_FunctionMenu li[functionid=" + functionid + "]").parents(".treeview"));
                        _menu.expandOneMenuByFunctionId(functionid);
						$("#_FunctionMenu li[functionid=" + functionid + "]").parents(".treeview").addClass("active");
						$("#_FunctionMenu li[functionid=" + functionid + "]").addClass("active");

						var bkFunctionId = "bk_" + functionid;
						$("#_BookmarkMenu li").removeClass("active");						
						$("#_BookmarkMenu li[functionid=" + bkFunctionId + "]").addClass("active");
					},

					"getFunctionArray": function (targetFunctionId, functionMenu, searchFlag) {
						searchFlag.founctionArray.push(functionMenu);
						if (functionMenu.Id == targetFunctionId) {
							searchFlag.found = true;
							return;
						}
						if (functionMenu.SubFunctionList && functionMenu.SubFunctionList.length > 0) {
							$.each(functionMenu.SubFunctionList, function (j, subFunctionMenu) {
								var currentIndex = searchFlag.founctionArray.length - 1;
								_menu.getFunctionArray(targetFunctionId, subFunctionMenu, searchFlag);
								if (searchFlag.found) {
									return false;
								} else {
									for (var i = searchFlag.founctionArray.length - 1; i > currentIndex; i--) {
										searchFlag.founctionArray.pop(i);
									}
								}
							});
						}
                    }
				},
				"context_menu": {
					"bindTabContextMenu": function () {
						$('.form-tab').contextmenu({
							target: '#tab-context-menu',
							onItem: function (context, e) {
								var menuIndex = $(e.target).attr('menuindex');
								var functionid = $(context).find("a").attr("functionid");
								if (menuIndex == "MENU_OPEN_IN_NEW_WINDOW") {
									var url = $("#" + functionid).find("iframe").attr("src");
									window.open(url);
                                } else if (menuIndex == "MENU_CLOSE") {
                                    if (_settings.OPT_SHOW_CONFIRM_WHEN_CLOSE) {
                                        var confirmData = {
                                            "content": _CurrentLang['msg_confirm_close_tab'],
                                            "oktodo": function () {
                                                _form.closeFormByFunctionId(functionid);
                                                _breadcrumb.toggleBreadcrumb();
                                            }
                                        };
                                        $.dialog.showConfirm(confirmData);
                                    } else {
                                        _form.closeFormByFunctionId(functionid);
                                        _breadcrumb.toggleBreadcrumb();
                                    }                               
                                } else if (menuIndex == "MENU_CLOSE_OTHERS") {
                                    if (_settings.OPT_SHOW_CONFIRM_WHEN_CLOSE) {
                                        var confirmData = {
                                            "content": _CurrentLang['msg_confirm_close_tab'],
                                            "oktodo": function () {
                                                $("#_FormTabs a").each(function (i, tab) {
                                                    if ($(tab).attr("functionid") != functionid) {
                                                        _form.closeFormByFunctionId($(tab).attr("functionid"));
                                                    }
                                                });
                                                _breadcrumb.toggleBreadcrumb();
                                            }
                                        };
                                        $.dialog.showConfirm(confirmData);
                                    } else {
                                        $("#_FormTabs a").each(function (i, tab) {
                                            if ($(tab).attr("functionid") != functionid) {
                                                _form.closeFormByFunctionId($(tab).attr("functionid"));
                                            }
                                        });
                                        _breadcrumb.toggleBreadcrumb();
                                    }                                    
                                } else if (menuIndex == "MENU_CLOSE_ALL") {
                                    if (_settings.OPT_SHOW_CONFIRM_WHEN_CLOSE) {
                                        var confirmData = {
                                            "content": _CurrentLang['msg_confirm_close_tab'],
                                            "oktodo": function () {
                                                $("#_FormTabs a").each(function (i, tab) {
                                                    _form.closeFormByFunctionId($(tab).attr("functionid"));
                                                });
                                                _breadcrumb.toggleBreadcrumb();
                                            }
                                        };
                                        $.dialog.showConfirm(confirmData);                                        
                                    } else {
                                        $("#_FormTabs a").each(function (i, tab) {
                                            _form.closeFormByFunctionId($(tab).attr("functionid"));
                                        });
                                        _breadcrumb.toggleBreadcrumb();
                                        $("#tab-context-menu").hide();
                                    }
                                    									
								} else if (menuIndex == "MENU_REFRESH") {
									var frameName = "frm_" + functionid;
									var url = $("iframe[name=" + frameName + "]").attr("src");
									$("iframe[name=" + frameName + "]").attr("src", url);
									_form.showFormByFunctionId(functionid);
								} else if (menuIndex == "MENU_ADD_TO_FAVORITES") {
									_bookmark.addToBookmark(functionid);
								}
							}
						});
					},

					"bindMenuContextMenu": function () {
						$('#_FunctionMenu').find(".treeview,.header").contextmenu({
							target: '#menu-context-menu',
							onItem: function (context, e) {
								var menuIndex = $(e.target).attr('menuindex');
                                if (menuIndex == "MENU_EXPAND") {
                                    _menu.expandOneMenuByContext(context);
                                } else if (menuIndex == "MENU_EXPAND_ALL") {
                                    _menu.expandAllMenu();
                                } else if (menuIndex == "MENU_COLLAPSE") {
                                    _menu.collapseOneMenuByContext(context);                                    
                                } else if (menuIndex == "MENU_COLLAPSE_ALL") {
                                    _menu.collapseAllMenu();
                                } else if (menuIndex == "MENU_REFRESH") {
                                    _menu.init();
                                }
							}
						});

						_cmenu.bindBookmarkContextMenu();
					},

					"bindBookmarkContextMenu": function () {
						$('#_BookmarkMenu').find(".bookmark-item").contextmenu({
							target: '#bookmark-context-menu',
							onItem: function (context, e) {
								var menuIndex = $(e.target).attr('menuindex');
								if (menuIndex == "MENU_REMOVE_FROM_FAVORITES") {
									var functionid = $(context).attr("functionid").substring(3);;
									_bookmark.removeFromBookmark(functionid);
								}
							}
						});
					}
                },
                "setting": {
                    "toggleSettingSidebar": function (btn) {
                        if ($("#control_sidebar").hasClass("control-sidebar-open")) {
                            $(btn).addClass("active");
                        } else {
                            $(btn).removeClass("active");
                        }
                    }
                },
				"bookmark": {
					"addToBookmark": function (functionid) {
						if (functionid.substring(0, 3) == "bk_") {
							functionid = functionid.substring(3);
						}
						var options = {
							"success": function (d) {
								if (d.success) {
                                    $.dialog.showMessage({
                                        "title": _CurrentLang['lang_success'], 
                                        "content": _CurrentLang['msg_save_success']
                                    });
									_menu.init();
								}
							}
						};
						var data = {
							'functionId': functionid
						};

                        $.callWebService("addToBookmark", data, options);
					},

					"removeFromBookmark": function (functionid) {
						var options = {
							"success": function (d) {
                                if (d.success) {
                                    $.dialog.showMessage({
                                        "title": _CurrentLang['lang_success'],
                                        "content": _CurrentLang['msg_save_success']
                                    });
									_menu.init();
								}
							}
						};
						var data = {
							'functionId': functionid
						};

                        $.callWebService("removeFromBookmark", data, options);
                    },

                    "toggleBookmarkSidebar": function (btn) {
                        if ($("#bookmark_sidebar").hasClass("control-sidebar-open")) {
                            $(btn).addClass("active");
                        } else {
                            $(btn).removeClass("active");
                        }
                    }
				},
				"breadcrumb": {
					"toggleBreadcrumb": function () {
						var hasForm = ($("#_FormTabs>li").length > 0);
						var breadcrumbHeight = 0;
						if (hasForm) {
							breadcrumbHeight = 31;
							$("#_BreadcrumbContent").show(200);
						} else {
							$("#_BreadcrumbContent").hide(200);
						}
						$("#_BreadcrumbBar").animate({ "height": breadcrumbHeight + "px" }, 200);
					},

					"changeBreadCrumb": function () {
                        var bread = "<i class='fa fa-dashboard'></i>&nbsp;&nbsp;";
                        if (_PortalContext.CurrentFunctionId) {
                            if (_PortalContext.IsCurrentFunctionPortalLink) {
                                bread +=
                                    "<span lang='lang_portal_link'>Portal Link</span>";
                                var linkItem = _portalLink.getLinkItem(_PortalContext.CurrentFunctionId);
                                if (linkItem != null) {
                                    bread += "<span class='ngf-text-gray'> > </span><span>" + linkItem.Name+ "</span>";
                                }                                
                            } else {
                                $.each(_PortalContext.MenuList, function (i, product) {
                                    var found = false;
                                    $.each(product.DomainList, function (j, domainMenu) {
                                        if (found) {
                                            return false;
                                        }
                                        $.each(domainMenu.SystemList, function (j, systemMenu) {
                                            if (found) {
                                                return false;
                                            }
                                            $.each(systemMenu.FunctionList, function (j, functionMenu) {
                                                var searchFlag = {
                                                    "found": false,
                                                    "founctionArray": new Array()
                                                };
                                                _menu.getFunctionArray(_PortalContext.CurrentFunctionId, functionMenu, searchFlag);
                                                if (searchFlag.found) {
                                                    found = true;
                                                    if (_PortalContext.SystemMode != "Single") {
                                                        if (_PortalContext.IsCleanVersion) {
                                                            bread +="<span lang='" + domainMenu.Language_Key + "'>" + domainMenu.Code + "</span><span class='ngf-text-gray'> > </span>";
                                                        } else {
                                                            bread +=
                                                                "<span lang='" + product.Language_Key + "'>" + product.Name + "</span>"
                                                                + "<span class='ngf-text-gray'> > </span><span lang='" + domainMenu.Language_Key + "'>" + domainMenu.Code + "</span><span class='ngf-text-gray'> > </span>";
                                                        }                                                        
                                                    }
                                                    bread += "<span lang='" + systemMenu.Language_Key + "'>" + systemMenu.Code + "</span>";
                                                    $.each(searchFlag.founctionArray, function (k, f) {
                                                        bread += "<span class='ngf-text-gray'> > </span><span lang='" + f.Language_Key + "'>" + f.Code + "</span>";
                                                    });
                                                    return false;
                                                }
                                            });
                                        });
                                    });

                                    if (!found) {
                                        $.each(product.SystemList, function (i, systemMenu) {
                                            if (found) {
                                                return false;
                                            }
                                            $.each(systemMenu.FunctionList, function (j, functionMenu) {
                                                var searchFlag = {
                                                    "found": false,
                                                    "founctionArray": new Array()
                                                };
                                                _menu.getFunctionArray(_PortalContext.CurrentFunctionId, functionMenu, searchFlag);
                                                if (searchFlag.found) {
                                                    found = true;
                                                    bread +=  //"<span class='text-gray'> > </span>
                                                        "<span lang= '" + product.Language_Key + "'> " + product.Name + "</span> "
                                                        + "<span class='text-gray'> > </span><span lang='" + systemMenu.Language_Key + "'>" + systemMenu.Code + "</span>";
                                                    $.each(searchFlag.founctionArray, function (k, f) {
                                                        bread += "<span class='text-gray'> > </span><span lang='" + f.Language_Key + "'>" + f.Code + "</span>";
                                                    });
                                                    return false;
                                                }
                                            });
                                        });
                                    } else {
                                        return false;
                                    }
                                });
                            }							
						}
						//$("#_BreadcrumbContent").hide().html(bread).show(300);
                        
                        $("#_BreadcrumbContent").html(bread);
						$.language.change(_Context.CurrentLang);
					}
				},
				"form": {
					"openForm": function (menu) {
						var functionurl = $(menu).attr("functionurl");
						if (functionurl) {
							var functionname = $(menu).text();
							var functionid = $(menu).attr("functionid");
							$("#_FunctionMenu .treeview li").removeClass("active");
							$("#_BookmarkMenu li").removeClass("active");

							$(menu).addClass("active");

							var opend = this.showFormByFunctionId(functionid);

							if (!opend) {
								var tabHtml = '<li class="nav-item form-tab">'
									+ '<a class="nav-link" data-toggle="tab" href="#' + functionid + '" onclick="_form.showFormByFunctionId(\'' + functionid + '\'); return false;" role="tab" aria-controls="' + functionid + '" functionid="' + functionid + '">'
									+ '<table>'
									+ '<tr>'
									+ '<td>'
									+ '<div';

								var tabLang = $(menu).find('span').attr('lang');
								if (tabLang) {
									tabHtml += ' lang="' + tabLang + '"';
								}
								tabHtml += ">" + functionname + '</div>'
									+ '</td>'
									+ '<td style="padding-left:5px;">'
									+ '<button type="button" class="close" data-dismiss="alert" aria-hidden="true" onclick="return _form.closeForm(this);">&times;</button >'//<span class="fa fa-times icon_close_form" onclick="return closeForm(this);">'
									+ '</span>'
									+ '</td>'
									+ '</tr>'
									+ '</table>'
									+ '</a>'
									+ '</li>';
								$("#_FormTabs").append(tabHtml);

                                var frameHtml = '<div class="tab-pane" id="' + functionid + '" role="tabpanel" style="height: 100%; padding: 0px;">'
                                    + '<iframe onload="return _form.closeLoading(this);" name="frm_' + functionid + '" src="' + functionurl + getUrlQueryStringSplit(functionurl)  + 'SSOToken=' + getQueryStringByName('SSOToken')
                                    + "#!lang=" + _Context.CurrentLang + "&skin=" + _Context.CurrentSkin
                                    + '" class="col-md-12 col-lg-12 col-sm-12" style="height: 100%; width:100%;padding: 0px;border:0px;" ></iframe>';
                                frameHtml += "<div class='frm_loading_mask' id='frm_loading_mask_" + functionid + "'><div class='frm_loading_mask_inner'>"
                                    + '<div class="frm-loading">Loading...</div>'
                                  + "</div></div>"; //loading mask
                                frameHtml += "</div>";
                                $("#_FormTabContent").append(frameHtml);                                
								_cmenu.bindTabContextMenu();
                                this.showFormByFunctionId(functionid);
                                //$("#frm_loading_mask_" + functionid).height($("#" + functionid).height());
                                _ui.resetContentSize();
							}
						}
						_breadcrumb.toggleBreadcrumb();
						return false;
					},

					"showFormByFunctionId": function (functionid) {
						if (functionid.substring(0, 3) == "bk_") {
							functionid = functionid.substring(3);
						}
						var opened = false;
						$("#_FormTabs a").each(function (i, tab) {
							var thisFunctionId = $(tab).attr("functionid");
							if (thisFunctionId.substring(0, 3) == "bk_") {
								thisFunctionId = thisFunctionId.substring(3);
							}
							if (thisFunctionId == functionid) {
								opened = true;
								$(tab).tab("show");
                                _PortalContext.CurrentFunctionId = functionid;
                                _PortalContext.IsCurrentFunctionPortalLink = false;
								return false;
							}
						});
						_breadcrumb.changeBreadCrumb();
						return opened;
					},

					"closeFormByFunctionId": function (functionid) {
						var preTab = $("a[functionid=" + functionid + "]").parent().prev();
						var nextTab = $("a[functionid=" + functionid + "]").parent().next();
                        _PortalContext.CurrentFunctionId = null;
                        _PortalContext.IsCurrentFunctionPortalLink = false;
						$("a[functionid=" + functionid + "]").parent().remove();
						$("#" + functionid).remove();
						_breadcrumb.changeBreadCrumb();

						if (preTab.length > 0) {
							var preFunctionId = preTab.find("a").attr("functionid");
							this.showFormByFunctionId(preFunctionId);
						} else if (nextTab.length > 0) {
							var nextFunctionId = nextTab.find("a").attr("functionid");
							this.showFormByFunctionId(nextFunctionId);
						}
					},

                    "closeForm": function (ctrl) {
                        if (_settings.OPT_SHOW_CONFIRM_WHEN_CLOSE) {
                            var confirmData = {
                                "content": _CurrentLang['msg_confirm_close_tab'],
                                "oktodo": function () {
                                    var functionid = $(ctrl).parents('a').attr("functionid");
                                    _form.closeFormByFunctionId(functionid);
                                    _breadcrumb.toggleBreadcrumb();
                                }
                            };
                            $.dialog.showConfirm(confirmData);
                        } else {
                            var functionid = $(ctrl).parents('a').attr("functionid");
                            _form.closeFormByFunctionId(functionid);
                            _breadcrumb.toggleBreadcrumb();
                        }                        

						return false;
                    },

                    "closeLoading": function (frame) {
                        $("#frm_loading_mask_" + $(frame).parent(".tab-pane").first().attr("id")).hide();
                    },

                    "options": {
                        "SHOW_CONFIRM_WHEN_CLOSE" : true
                    }
                },
                "footer": {
                    "resetCopyrightLocation": function () {
                        var footerWidth = $("footer").width();
                        if (_PortalContext.MenuVisible()) {
                            footerWidth -= $(".sidebar").width();
                        }
                        $("#footerContainer").animate({
                            "padding-left": (footerWidth - $("#footerContainer").width()) / 2
                        }, 200);
                    }
                }
			},
			"user": {
				"init": function () {
					var options = {
						"success": function (d) {
							if (d.success) {
								_PortalContext.UserInfo = d.data;
								_user.refresh();
							}
						}
					};

                    $.callWebService("getUserInfo", {}, options);
				},
                "refresh": function () {                   
					$("#lblUserName").text(_PortalContext.UserInfo.Name);
                    $("#lblLoginTime").text(_PortalContext.UserInfo.LoginTime);

                    var deptStr = "";
                    if (_PortalContext.UserInfo.DepartmentList && _PortalContext.UserInfo.DepartmentList.length > 0) {
                        $.each(_PortalContext.UserInfo.DepartmentList, function (i, dept) {
                            deptStr += dept + "; ";
                        });
                        if (deptStr.length > 0) {
                            deptStr = deptStr.substring(0, deptStr.length - 2);
                        }
                    }                    
                    $("#lblUserDepartment").text(_PortalContext.UserInfo.Name + "-" + deptStr);

                    if (_PortalContext.UserInfo.Extension)
                    {
                        $("#lblUserTel").text(_PortalContext.UserInfo.Extension);
                    }

                    var imgBinary = _PortalContext.UserInfo.ImageUrl;
                    if (imgBinary && imgBinary.length > 0) {
                        $(".userImage").hide(200).attr("src", imgBinary).show(200);
                    }

                    if (_PortalContext.UserInfo.IsAdmin) {
                        $("#btnAdminLogin").show();
                    } else {
                        $("#btnAdminLogin").remove();
                    }

                    if (!_PortalContext.UserInfo.IsInternal) {
                        $("#btnChangePassword").show();
                        if (_PortalContext.UserInfo.IsAdmin) {
                            $(".user-footer .pull-right, .user-footer .btn").css("width", "100%");
                            $(".user-footer .btn-default").css("margin-top", "10px");                            
                        }
                    } else {
                        $("#btnChangePassword").remove();
                    }
				}
			},
			"news": {
                "init": function () {
                    if (_Context.AuthMode == "WSC") {
                        $("#navNews").hide();
                    } else {
                        var options = {
                            "success": function (d) {
                                if (d.success) {
                                    _PortalContext.News = d.data;
                                    _news.refresh();
                                }
                            },
                            "show_mask": false,
                            "hide_error": true
                        };
                        var serviceUrl = "getAllNews";
                        if (this.status == this.STATUS.UNREAD) {
                            serviceUrl = "getUnreadNews";
                        }
                        $.callWebService(serviceUrl, {}, options);

                        if (_settings.OPT_NEWS_REFRESH_TIMER_ENABLE) {
                            clearInterval(_news.timer);
                            this.timer = setInterval("_news.init()", _settings.OPT_NEWS_REFRESH_INTERVAL);
                        }
                    }					
				},
				"refresh": function () {
					var newsCount = _PortalContext.News.length;
					if (newsCount > 0) {
						if ($("#lblNewsCount").text() != newsCount) {
							$("#lblNewsCount").hide(200).text(newsCount).show(300);
						}						
						$("#lblTotalNews").text(newsCount);

						var html = "";
						$.each(_PortalContext.News, function (i, news) {
							html += _news.getNewsItemHtml(news);
						});
						$("#newsList").html(html);
                    } else {
                        $("#newsList").html("");
						$("#lblNewsCount").text(newsCount).hide(200);
					}
				},
				"getNewsItemHtml": function (news) {
					var date = new Date(parseInt(news.Created_Date.replace("/Date(", "").replace(")/", ""), 10));
					var html = "<li>"
                        + "<a onclick='return _news.openNews(\"" + news.Id + "\")' >"
						//+ "<h4 style='overflow:hidden;'>" + news.Subject + "<small><i class='fa fa-clock-o'></i>&nbsp;" + (new Date(date)).Format("yyyy-MM-dd") + "</small>"
						//+ "</h4>"
                        //+ "<p><i class='fa fa-user'></i>&nbsp;" + news.Created_By + "</p>"
                        + "<div>"
                        + "<h4 style='overflow:hidden;text-overflow:ellipsis;'>" + escapeHtmlData(news.Subject) + "</h4>"
                        + "</div>"
                        + "<small><i class='fa fa-clock-o'></i>&nbsp;" + (new Date(date)).Format("yyyy-MM-dd") + "</small>"
                        + "&nbsp;&nbsp;<small><i class='fa fa-user'></i>&nbsp;" + news.Created_By + "</small>"
                        + "</a></li>";
					return html;
				},
				"openNews": function (newsId) {
					var url = "PortalNewsDetail.aspx?" + $.param({ "id": newsId });
					window.open(url, "PortalNewsDetail", '', "_blank");
                },
                "toggleNewsList": function () {
                    if (this.status == this.STATUS.UNREAD) {
                        this.status = this.STATUS.ALL;
                        $("#btnShowUnreadNews").show();
                        $("#btnShowAllNews").hide();
                    } else {
                        this.status = this.STATUS.UNREAD;
                        $("#btnShowUnreadNews").hide();
                        $("#btnShowAllNews").show();
                    }
                    this.init();
                },
                "timer": null,
                "status": "UNREAD",
                "STATUS": {
                    "ALL": "ALL",
                    "UNREAD": "UNREAD"
                }
            },
            "portalLink": {
                "init": function () {
                    if (_Context.AuthMode == "WSC") {
                        $("#tabPortalLink").hide();
                    } else {
                        var options = {
                            "success": function (d) {
                                if (d.success) {
                                    _PortalContext.WfkResourceUrl = d.data.WfkResourceUrl;
                                    _PortalContext.PortalLinkList = d.data.PortalLinkList;
                                    _portalLink.refresh();
                                }
                            },
                            "show_mask": false
                        };

                        $.callWebService("getPortalLinkList", {}, options);
                    }                                        
                },
                "refresh": function () {                   
                    var portalLinksHtml = "";
                    $.each(_PortalContext.PortalLinkList, function (i, link) {
                        portalLinksHtml += _portalLink.getPortalLinkItemHtml(link);
                    });
                    $("#portal_link_container").html(portalLinksHtml);
                },
                "getPortalLinkItemHtml": function (link) {
                    var resUrl = _PortalContext.WfkResourceUrl + "res.its?icon=";
                    var html = "";
                    html += '<li onclick="return _portalLink.openLink(this);" linktarget="'
                         + link.Target
                         + '"  linkid="' + link.Id + '" ';
                    if (link.Navigate_Url) {
                        html += ' linkurl="'
                            + link.Navigate_Url + '" ';
                    }
                    html += '>'
                        + '<a class="function_menu_item">';
                    if (link.System_Icon_Name && link.System_Icon_Name.length > 0) {
                        html += '<img src="' + resUrl + link.System_Icon_Name + '"></img>&nbsp;&nbsp;';
                    } else {
                        html += '<i class="fa fa-circle-o text-light-blue"></i>';
                    }
                    html += '<span data-toggle="tooltip" class="link_name_container" data-placement="top" title="'
                        + link.Name + '">'
                        + link.Name + '</span>'
                        + '</a>'
                        + '</li>';
                    return html;
                },
                "openLink": function (link) {
                    var linkid = $(link).attr("linkid");
                    var linkurl = $(link).attr("linkurl");
                    var linktarget = $(link).attr("linktarget");
                    var linkname = $(link).find(".link_name_container").first().text();
                    if (linktarget == "NewWindow") {
                        window.open(linkurl);
                    } else if (linktarget == "FullScreen") {
                        window.open(linkurl, '', 'width=' + (window.screen.availWidth - 10) + ',height=' + (window.screen.availHeight - 30) + ',top=0,left=0,resizable=yes,status=yes,menubar=no,scrollbars=yes');
                    } else if (linktarget == "Tab") {
                        var opend = _portalLink.showPortalLinkFormById(linkid);

                        if (!opend) {
                            var tabHtml = '<li class="nav-item form-tab">'
                                + '<a class="nav-link" data-toggle="tab" isportallink="true" href="#' + linkid + '" onclick="_portalLink.showPortalLinkFormById(\''
                                + linkid + '\'); return false;" role="tab" aria-controls="' + linkid + '" functionid="' + linkid + '">'
                                + '<table>'
                                + '<tr>'
                                + '<td>'
                                + '<div>'
                                + linkname + '</div>'
                                + '</td>'
                                + '<td style="padding-left:5px;">'
                                + '<button type="button" class="close" data-dismiss="alert" aria-hidden="true" onclick="return _form.closeForm(this);">&times;</button >'//<span class="fa fa-times icon_close_form" onclick="return closeForm(this);">'
                                + '</span>'
                                + '</td>'
                                + '</tr>'
                                + '</table>'
                                + '</a>'
                                + '</li>';
                            $("#_FormTabs").append(tabHtml);

                            $("#_FormTabContent").append('<div class="tab-pane" id="'
                                + linkid
                                + '" role="tabpanel" style="height: 100%; padding: 0px;">'
                                + '<iframe name="frm_' + linkid + '" src="' + linkurl + getUrlQueryStringSplit(linkurl) + 'SSOToken=' + getQueryStringByName('SSOToken')
                                + "#!lang=" + _Context.CurrentLang
                                + '" class="col-md-12 col-lg-12 col-sm-12" style="height: 100%; width:100%;padding: 0px;border:0px;"></iframe></div>');
                            _cmenu.bindTabContextMenu();
                            _portalLink.showPortalLinkFormById(linkid);

                            _breadcrumb.toggleBreadcrumb();
                        }
                    }
                },
                "showPortalLinkFormById": function (linkid) {
                    var opened = false;
                    $("#_FormTabs a").each(function (i, tab) {
                        var thisFunctionId = $(tab).attr("functionid");
                        if (thisFunctionId == linkid) {
                            opened = true;
                            $(tab).tab("show");
                            _PortalContext.CurrentFunctionId = linkid;
                            _PortalContext.IsCurrentFunctionPortalLink = true;
                            return false;
                        }
                    });
                    _breadcrumb.changeBreadCrumb();
                    return opened;
                },
                "getLinkItem": function (linkid) {
                    var linkitem = null;
                    $.each(_PortalContext.PortalLinkList, function (i, item) {
                        if (item.Id == linkid) {
                            linkitem = item;
                            return false;
                        }
                    });
                    return linkitem;
                }
            },
			"init": function () {
				this.ui.resetContentSize();
				//$(window).on("resize", function () {
				//	_ui.resetContentSize();
                //});
                $(window).wresize(function () {
                    _ui.resetContentSize();
                });

                _footer.resetCopyrightLocation();
                //$(".content-wrapper").resize(function () {
                //    _footer.resetCopyrightLocation();
                //});
                $(".content-wrapper").wresize(function () {
                    _footer.resetCopyrightLocation();
                });

                window.onmessage = function (e) {                    
                    e = e || event;
                    try {
                        var msg = $.parseJSON(e.data);
                        switch (msg.message) {
                            case NGFFrameworkMessage.SHOW_MESSAGE:
                                $.dialog.showMessage(msg.data);
                                break;
                            case NGFFrameworkMessage.CLOSE_MESSAGE:
                                $.dialog.closeMessage();
                                break;
                            case NGFFrameworkMessage.SHOW_CONFIRM:                                
                                $.dialog.showConfirm(msg.data, msg.wname);
                                break;
                            case NGFFrameworkMessage.CLOSE_CONFIRM:
                                $.dialog.closeConfirm();
                                break;
                            case NGFFrameworkMessage.SHOW_LOADING:
                                $.dialog.showLoading();
                                break;
                            case NGFFrameworkMessage.CLOSE_LOADING:
                                $.dialog.closeLoading();
                                break;
                        }
                    } catch (e) {}
                }

				$("#search-btn").on("click", function () {
					_menu.searchMenu();
				});

				$("#ddlLanguage").val($.uriAnchor.makeAnchorMap()["lang"]);

				this.ui.menu.init();

				//Tabs Event
				$('#_FormTabs').on('shown.bs.tab', function (e) {
                    var functionid = $(e.target).attr("functionid");
                    var isPortalLink = $(e.target).attr("isportallink");
                    if (!isPortalLink) {
                        _menu.activeMenu(functionid);
                    }					
				});

				$('#tbxSearchMenu').on('keydown', function (e) {
					if (e.keyCode == 13) {
						_menu.searchMenu();
						return false;
					}
					return true;
				});

				$('#_FormTabsContainer').slimscroll({
					height: '32px',
					width: '100%',
					axis: 'x',
					alwaysVisible: false,
					opacity: .2, //滚动条透明度
					borderRadius: '7px', //滚动条圆角
				});

				this.user.init();
                this.news.init();
                this.portalLink.init();
                this.settings.init();
			},
			"logout": function () {
				var options = {
					"success": function (d) {
						if (d.success) {
							$.cookie("SSOToken", null);
							$.goto("Login?action=logout");
						}
					}
				};
                $.callWebService("logout", {}, options);
				return false;
            },
            "adminLogin": function () {
                window.location = "SSOAdminSimulate.aspx?SSOToken=" + getQueryStringByName("SSOToken");
            },
            "changePassword": function () {
                $.dialog.showDialog("dlgChangePassword");
            },
            "doChangePassword": function () {
                var options = {
                    "success": function (d) {
                        if (d.success) {
                            $.dialog.showMessage({ "content": _CurrentLang["msg_change_pwd_successed"] });
                        } else {
                            $.dialog.showMessage({ "content": _CurrentLang["msg_change_pwd_failed"] + d.message });
                        }
                    },

                    "error": function () {
                        $.dialog.showMessage({ "content": _CurrentLang["msg_change_pwd_failed"] });
                    }
                };
                $.callWebService("changePassword", {
                    "oldPwd": $("#tbxOldPassword").val(),
                    "newPwd": $("#tbxNewPassword").val()
                }, options);
                return false;
            },
            "state": {
                "setLanguage": function () {
                    setLanguage($("#ddlLanguage").val());
                    this.syncFramesState();
                },
                "setSkin": function (color) {
                    setSkin(color);
                    this.syncFramesState();
                },
                "syncFramesState": function () {
                    var msg = {
                        "message": NGFFrameworkMessage.CHANGE_STATE,
                        "data": {
                            "language": _Context.CurrentLang,
                            "skin": _Context.CurrentSkin
                        }
                    };
                    var msgStr = JSON.stringify(msg);
                    for (var i = 0; i < window.frames.length; i++)
                    {
                        var win = window.frames[i];
                        win.postMessage(msgStr, "*");
                    }

                    //$("iframe").each(function (i, f) {
                        //var oldSrc = $(f).attr("src");
                        //var oldUrl = oldSrc.split("#!")[0];
                        //var map = {};
                        //map = $.uriAnchor.makeAnchorMap(oldSrc);
                        //map["lang"] = _Context.CurrentLang;
                        //var mapStr = $.uriAnchor.makeAnchorString(map);
                        //$(f).attr("src", oldUrl + "#!" + mapStr);
                    //});
                }
            },
            "settings": {
                "OPT_SHOW_CONFIRM_WHEN_CLOSE": false,
                "OPT_NEWS_REFRESH_TIMER_ENABLE": true,
                "OPT_NEWS_REFRESH_INTERVAL": 5000,
                "init": function () {
                    _settings.refresh();
                },
                "refresh": function () {
                    $("#cbxShowConfirmWhenCloseTab").prop("checked", this.OPT_SHOW_CONFIRM_WHEN_CLOSE);
                    $("#cbxNewsRefreshEnable").prop("checked", this.OPT_NEWS_REFRESH_TIMER_ENABLE);
                },
                "change": function (ctrl) {
                    var option = $(ctrl).attr("target");
                    this[option] = $(ctrl).prop("checked");
                    if (option == "OPT_NEWS_REFRESH_TIMER_ENABLE") {
                        clearInterval(_news.timer);
                        if (_settings.OPT_NEWS_REFRESH_TIMER_ENABLE) {                            
                            _news.timer = setInterval("_news.init()", _settings.OPT_NEWS_REFRESH_INTERVAL);
                        }
                    }
                    return false;
                },
                "save": function () {
                }
            }
		}
	}
});

var _portal = $.ngf.portal;
var _ui = _portal.ui;
var _header = _ui.header;
var _menu = _ui.menu;
var _cmenu = _ui.context_menu;
var _bookmark = _ui.bookmark;
var _setting = _ui.setting;
var _breadcrumb = _ui.breadcrumb;
var _form = _ui.form;
var _user = _portal.user;
var _news = _portal.news;
var _portalLink = _portal.portalLink;
var _state = _portal.state;
var _footer = _ui.footer;
var _settings = _portal.settings;