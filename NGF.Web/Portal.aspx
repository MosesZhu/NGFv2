<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="NGF.Web.Portal" %>

<asp:Content ID="PageStyleContent" ContentPlaceHolderID="PageHeadContentHolder" runat="server">
    <script>
        var _PortalContext = {
            "SystemMode": null,
            "MenuList": null,
            "BookmarkList": null,
            "CurrentFunctionId": null,
            "IsCurrentFunctionPortalLink": false,
            "UserInfo": null,
            "News": null,
            "PortalLinkList": null,
            "HeaderVisible": true,
            "WfkResourceUrl": "",
            "MenuVisible": function () {
                return !$("body").hasClass("sidebar-collapse");
            },
            "IsCleanVersion": false,
            "CleanVersionDomain": null
        };
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="wrapper" style="height: 100%; background-color: transparent;">
        <a class="header-toggle" data-toggle="" role="button" id="btnShowHeader" onclick="return _header.toggleHeader();">
            <i class="fa fa-chevron-down"></i>
        </a>
        <header class="main-header">
            <!-- Logo -->
            <div class="logo" style="padding: 0px; height: 51px;">
                <span class="ngf-header">
                    <asp:Literal ID="textHeaderInfo" runat="server"></asp:Literal>
                </span>
                <span class="ngf-environment">
                    <asp:Literal ID="textEnvironmentInfo" runat="server"></asp:Literal>
                </span>
            </div>

            <!-- Header Navbar -->
            <nav class="navbar navbar-static-top" role="navigation">
                <!-- Sidebar toggle button-->
                <a class="sidebar-toggle" data-toggle="offcanvas" role="button">
                    <span class="sr-only">Toggle navigation</span>
                </a>
                <!-- Navbar Right Menu -->
                <div class="navbar-custom-menu">
                    <ul class="nav navbar-nav">
                        <!-- News -->
                        <li class="dropdown messages-menu" id="navNews">
                            <!-- Menu toggle button -->
                            <a class="dropdown-toggle" data-toggle="dropdown">
                                <i class="fa fa-envelope-o"></i>
                                <span class="label label-success" id="lblNewsCount" style="display: none;"></span>
                            </a>
                            <ul class="dropdown-menu" id="ngf-news-dropdown-menu">
                                <li>
                                    <ul class="menu" id="newsList">
                                    </ul>
                                </li>
                                <li class="footer">
                                    <a onclick="return _news.toggleNewsList();" id="btnShowAllNews">
                                        <span lang="lang_show_all">Show All News</span>
                                        <!--<span>&nbsp;</span>
                                        <span lang="lang_total" style="float:right;"></span><span style="float:right;">: </span><span style="float:right;" id="lblTotalNews">0</span>
                                        -->
                                    </a>
                                    <a onclick="return _news.toggleNewsList();" style="display: none;" id="btnShowUnreadNews">
                                        <span lang="lang_show_unread">Show All News</span>
                                    </a>
                                </li>
                            </ul>
                        </li>
                        <!-- User -->
                        <li class="dropdown user user-menu" id="navUser">
                            <a class="dropdown-toggle" data-toggle="dropdown">
                                <img src="img/user.jpg" class="user-image userImage" alt="User Image">
                                <span class="hidden-xs" id="lblUserName"></span>
                            </a>
                            <ul class="dropdown-menu" id="ngf-user-dropdown-menu">
                                <li class="user-header">
                                    <img src="img/user.jpg" class="img-circle userImage" alt="User Image">
                                    <p>
                                        <span id="lblUserDepartment">User Name - Department</span>
                                        <small><span lang="lang_extension"></span>: <span id="lblUserTel">+00000000</span></small>
                                        <small><span lang="lang_last_logon_time"></span>: <span id="lblLoginTime">N/A</span></small>
                                    </p>
                                </li>
                                <li class="user-footer">
                                    <div class="pull-right">
                                        <a class="btn btn-flat btn-skin-primary" lang="lang_admin_login" id="btnAdminLogin" style="display: none;" onclick="return _portal.adminLogin();">Admin Login</a>
                                        <a class="btn btn-flat btn-default" lang="lang_logout" onclick="return _portal.logout();">Logout</a>
                                        <a class="btn btn-flat btn-default" lang="lang_change_password" id="btnChangePassword" onclick="return _portal.changePassword();" style="display: none;">Change Password</a>
                                    </div>
                                </li>
                            </ul>
                        </li>
                        <!-- Right Sidebar Button -->
                        <li id="btnToggleSettingSidebar" class="sidebar_button" onclick="return _setting.toggleSettingSidebar(this);">
                            <a data-toggle="control-sidebar" data-target="control_sidebar"><i class="fa fa-gears"></i></a>
                        </li>
                        <li id="btnToggleBookmarkSidebar" class="sidebar_button" onclick="return _bookmark.toggleBookmarkSidebar(this);">
                            <a data-toggle="control-sidebar" data-target="bookmark_sidebar"><i class="fa fa-star"></i></a>
                        </li>
                        <li>
                            <a class="header-toggle" data-toggle="" role="button" id="btnHideHeader" onclick="return _header.toggleHeader();">
                                <i class="fa fa-chevron-up"></i>
                            </a>
                        </li>
                    </ul>
                </div>
            </nav>
        </header>
        <!-- Left side column. contains the logo and sidebar -->
        <aside class="main-sidebar" style="top:-1px">
            <!-- sidebar: style can be found in sidebar.less -->
            <section class="sidebar" id="scrollspy">

                <!-- search form (Optional) -->
                <div class="input-group" style="position: absolute; top: 0px;">
                    <input type="text" name="q" id="tbxSearchMenu" class="form-control" placeholder="Search...">
                    <span class="input-group-btn">
                        <button type="button" name="search" id="search-btn" class="btn btn-flat">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <!-- /.search form -->

                <!-- Sidebar Menu -->
                <ul class="nav sidebar-menu" id="_FunctionMenu">
                </ul>
                <!-- /.sidebar-menu -->
            </section>
            <!-- /.sidebar -->
        </aside>

        <!-- Content Wrapper. Contains page content -->
        <div class="content-wrapper" style="overflow: hidden;">
            <!-- Main content -->
            <section class="content body">
                <div id="_FormTabsContainer">
                    <ul class="nav nav-tabs" role="tablist" id="_FormTabs"
                        style="display: -webkit-inline-box;">
                    </ul>
                </div>
                <div id="_BreadcrumbBar" style="height: 0px;">
                    <div id="_BreadcrumbContent"></div>
                </div>
                <div class="tab-content" style="height: 100%;" id="_FormTabContent">
                </div>

            </section>
            <!-- /.content -->
        </div>
        <!-- /.content-wrapper -->

        <!-- Main Footer -->
        <footer class="main-footer" style="position: fixed; bottom: 0px; width: 100%; padding: 8px;">
            <div id="footerContainer" style="display: inline-block;">
                <asp:Literal ID="textFooterInfo" runat="server"></asp:Literal></div>
        </footer>

        <!-- Bookmark Sidebar -->
        <aside class="control-sidebar control-sidebar-light" id="bookmark_sidebar">
            <ul class="nav sidebar-menu" id="_BookmarkMenu" style="overflow: hidden;">
            </ul>
        </aside>
        <div class="control-sidebar-bg" data-target="bookmark_sidebar"></div>

        <!-- Control Sidebar -->
        <aside class="control-sidebar control-sidebar-dark" id="control_sidebar">
            <!-- Create the tabs -->
            <ul class="nav nav-tabs nav-justified control-sidebar-tabs">
                <li class="active"><a href="#control-sidebar-home-tab" data-toggle="tab"><i class="fa fa-home"></i></a></li>
                <li id="tabPortalLink"><a href="#control-sidebar-portallink-tab" data-toggle="tab"><i class="fa fa-link"></i></a></li>
                <li><a href="#control-sidebar-settings-tab" data-toggle="tab"><i class="fa fa-gears"></i></a></li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content" style="padding: 0px;">
                <!-- Home tab content -->
                <div class="tab-pane active" id="control-sidebar-home-tab" style="margin: 20px;">
                    <h3 class="control-sidebar-heading" lang="lang_language">Language</h3>
                    <ul class="control-sidebar-menu">
                        <li>
                            <a href="javascript:;">
                                <div class="input-group">
                                    <select class="form-control select2" style="width: 100%;" id="ddlLanguage">
                                        <option selected="selected" value="EnUS">English</option>
                                        <option value="ZhCN">中文简体</option>
                                        <option value="ZhTW">中文繁體</option>
                                    </select>
                                    <div class="input-group-btn">
                                        <button type="button" class="btn btn-flat btn-skin-primary" onclick="return _state.setLanguage();" lang="lang_confirm">Confirm</button>
                                    </div>
                                </div>
                            </a>
                        </li>
                    </ul>
                    <!-- /.control-sidebar-menu -->

                    <h3 class="control-sidebar-heading" lang="lang_themes">Themes</h3>
                    <ul class="control-sidebar-menu">
                        <li>
                            <a>
                                <div class="btn-group" style="width: 100%; padding: 20px;">
                                    <!--<button type="button" id="color-chooser-btn" class="btn btn-info btn-block dropdown-toggle" data-toggle="dropdown">Color <span class="caret"></span></button>-->
                                    <ul class="fc-color-picker" id="color-chooser">
                                        <li>
                                            <div class="text-blue skin_button" onclick="_state.setSkin('blue')"><i class="fa fa-square skin_button"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-blue alpha60 skin_button" onclick="_state.setSkin('blue-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-orange skin_button" onclick="_state.setSkin('yellow')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-orange alpha60 skin_button" onclick="_state.setSkin('yellow-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-green skin_button" onclick="_state.setSkin('green')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-green alpha60 skin_button" onclick="_state.setSkin('green-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-red skin_button" onclick="_state.setSkin('red')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-red alpha60 skin_button" onclick="_state.setSkin('red-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-purple skin_button" onclick="_state.setSkin('purple')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-purple alpha60 skin_button" onclick="_state.setSkin('purple-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-black skin_button" onclick="_state.setSkin('black')"><i class="fa fa-square"></i></div>
                                        </li>
                                        <li>
                                            <div class="text-black alpha60 skin_button" onclick="_state.setSkin('black-light')"><i class="fa fa-square"></i></div>
                                        </li>
                                    </ul>
                                </div>
                            </a>
                        </li>
                    </ul>
                    <!-- /.control-sidebar-menu -->

                </div>
                <!-- PortalLink tab content -->
                <div class="tab-pane" id="control-sidebar-portallink-tab">
                    <h3 class="control-sidebar-heading" lang="lang_portal_link" style="margin: 20px;">Portal Link</h3>
                    <div class="nav sidebar-menu" id="portal_link_container">
                    </div>

                </div>
                <!-- Settings tab content -->
                <div class="tab-pane" id="control-sidebar-settings-tab" style="margin: 20px;">
                    <form>
                        <h3 class="control-sidebar-heading" lang="lang_general_settings">General Settings</h3>

                        <div class="form-group">
                            <label class="control-sidebar-subheading">
                                <span lang="lang_show_confirm_when_close_tab">Report panel usage</span>
                                <input type="checkbox" class="pull-right" checked id="cbxShowConfirmWhenCloseTab"
                                    target="OPT_SHOW_CONFIRM_WHEN_CLOSE" onchange="return _settings.change(this);">
                            </label>
                            <p lang="lang_show_confirm_when_close_tab_info">
                                If you want to show a confirm dialog when close form tab.
                            </p>

                            <label class="control-sidebar-subheading">
                                <span lang="lang_news_refresh_enable">News auto refresh</span>
                                <input type="checkbox" class="pull-right" checked id="cbxNewsRefreshEnable"
                                    target="OPT_NEWS_REFRESH_TIMER_ENABLE" onchange="return _settings.change(this);">
                            </label>
                            <p lang="lang_news_refresh_enable_info">
                                If you want to show a confirm dialog when close form tab.
                            </p>
                        </div>
                        <!-- /.form-group -->
                    </form>
                </div>
                <!-- /.tab-pane -->
            </div>
        </aside>
        <!-- /.control-sidebar -->
        <!-- Add the sidebar's background. This div must be placed
           immediately after the control sidebar -->
        <div class="control-sidebar-bg" data-target="control_sidebar"></div>
    </div>
    <div id="tab-context-menu">
        <ul class="dropdown-menu" role="menu">
            <li><a menuindex="MENU_OPEN_IN_NEW_WINDOW" lang="lang_open_in_new_window">open in new window</a></li>
            <li><a menuindex="MENU_CLOSE" lang="lang_close">close</a></li>
            <li><a menuindex="MENU_CLOSE_OTHERS" lang="lang_close_others">close others</a></li>
            <li><a menuindex="MENU_CLOSE_ALL" lang="lang_close_all">close all</a></li>
            <li class="divider"></li>
            <li><a menuindex="MENU_REFRESH" lang="lang_refresh">refresh</a></li>
            <li class="divider"></li>
            <li><a menuindex="MENU_ADD_TO_FAVORITES" lang="lang_add_to_favorites">add to my favourite</a></li>
        </ul>
    </div>
    <div id="menu-context-menu">
        <ul class="dropdown-menu" role="menu">
            <li><a menuindex="MENU_EXPAND" lang="lang_expand">expand</a></li>
            <li><a menuindex="MENU_EXPAND_ALL" lang="lang_expand_all">expand all</a></li>
            <li><a menuindex="MENU_COLLAPSE" lang="lang_collapse">collapse</a></li>
            <li><a menuindex="MENU_COLLAPSE_ALL" lang="lang_collapse_all">collapse all</a></li>
            <li class="divider"></li>
            <li><a menuindex="MENU_REFRESH" lang="lang_refresh">collapse all</a></li>
        </ul>
    </div>
    <div id="bookmark-context-menu">
        <ul class="dropdown-menu" role="menu">
            <li><a menuindex="MENU_REMOVE_FROM_FAVORITES" lang="lang_remove">remove</a></li>
        </ul>
    </div>

    <div id="dlgChangePassword" class="ngf-modal">
        <div class="ngf-modal-header">
            <h1 class="modal-title" lang="lang_change_password"></h1>
        </div>
        <div class="ngf-modal-body">
            <div class="ngf-form-row-1">
                <label for="tbxOldPassword" class="ngf-label" lang="lang_old_password">Old Password</label>
                <input type="password" class="form-control" id="tbxOldPassword" value="" />
            </div>
            <div class="ngf-form-row-1">
                <label for="tbxNewPassword" class="ngf-label" lang="lang_new_password">New Password</label>
                <input type="password" class="form-control" id="tbxNewPassword" value="" />
            </div>
            <div class="ngf-form-row-1">
                <label for="tbxConfirmPassword" class="ngf-label" lang="lang_confirm_password">Confirm Password</label>
                <input type="password" class="form-control" id="tbxConfirmPassword" value="" />
            </div>
        </div>
        <div class="ngf-modal-footer">
            <button class="ngf-btn-save" onclick="return _portal.doChangePassword()"></button>
            <button class="ngf-btn-cancel" data-dismiss="modal"></button>
        </div>
    </div>

    <div class="ngf-loader-mask" id="loader-mask">
    </div>
    <div class="ngf-loader" id="loader">
        <div class="loader-inner">
            <div class="ngf-loading">
                Loading...
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="PageScriptContent" ContentPlaceHolderID="PageScriptContentHolder" runat="server">
    <script>       
        $(function () {
            _portal.init();
        });
    </script>
</asp:Content>

