<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CleanHistoryConfig.aspx.cs" Inherits="DTSServer.CleanHistoryConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备监控系统</title>
    <link rel="stylesheet" href="/Content/bootstrap.min.css" />
    <link rel="stylesheet" href="/Css/style.css" />
    <script src="/Scripts/jquery-3.3.1.min.js"></script>
    <script src="/Scripts/bootstrap.min.js"></script>
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-md fixed-top navbar-dark bg-dark">
            <div class="navbar-header">
                <a class="navbar-brand" href="#">
                    <img alt="Brand" style="max-width: 100px; margin-top: -7px;" src="Images/logo.jpg" />
                </a>
            </div>
            <a class="navbar-brand" href="#">设备监控系统</a>
            <button class="navbar-toggler p-0 border-0" type="button" data-toggle="offcanvas">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="navbar-collapse offcanvas-collapse" id="navbarsExampleDefault">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="Index.aspx">设备列表<span class="sr-only">(current)</span></a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown01" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">设备详情</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown01">
                            <a class="dropdown-item" href="DeviceDetails.aspx">班次信息</a>
                            <a class="dropdown-item" href="WarningDetails.aspx">警告信息</a>
                            <a class="dropdown-item" href="ConsumableDetails.aspx">易损件信息</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown active">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown02" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">系统设置</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown02">
                            <a class="dropdown-item" href="WarningConfig.aspx">报警配置</a>
                            <a class="dropdown-item" href="ConsumableConfig.aspx">易损件配置</a>
                            <a class="dropdown-item" href="#">清理数据配置</a>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <form id="form1" runat="server">
        <main role="main" class="container">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="InforDiv" runat="server" class="alert alert-success"/>
                    <div class="my-3 p-3 bg-white rounded shadow-sm">
                        <h6 class="border-bottom border-gray pb-2 mb-0">自动清理数据配置(数据将在0点自动清理)</h6>
                        <div class="form-group" style="margin-top:5px;">
                            <label for="name">保存天数：</label>
                            <asp:TextBox ID="TextBoxDays" runat="server" Text="" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                        </div>
                        <div class="checkbox">
                            <label>
                                <asp:CheckBox ID="AutomaticCleanSwitch" Text="开启自动清理" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </label>
                        </div>
                        <asp:button CssClass="btn btn-primary" runat="server" text="修改" OnClick="OnChangeButtonClicked" OnClientClick="return confirm('请确认是否修改？');"/>
                    </div>
                    <div class="my-3 p-3 bg-white rounded shadow-sm">
                        <h6 class="border-bottom border-gray pb-2 mb-0">手动清理数据</h6>
                        <div class="form-group" style="margin-top:5px;">
                            <label for="name">保存天数：</label>
                            <asp:TextBox ID="TextBoxDays_2" runat="server" Text="" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                        </div>
                        <asp:button CssClass="btn btn-primary" runat="server" text="清理" OnClick="OnChangeButton2Clicked" OnClientClick="return confirm('请确认是否清理？');"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </main>
    </form>
</body>
</html>
