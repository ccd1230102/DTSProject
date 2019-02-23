<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceDetails.aspx.cs" Inherits="DTSServer.DeviceDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>设备监控系统</title>
    <link rel="stylesheet" href="/Content/bootstrap.min.css" />
    <link rel="stylesheet" href="/Content/bootstrap-datepicker3.min.css" />
    <link rel="stylesheet" href="/Css/style.css" />
    <script src="/Scripts/jquery-3.3.1.min.js"></script>
    <script src="/Scripts/bootstrap.min.js"></script>
    <script src="/Scripts/bootstrap-datepicker.min.js"></script>
    <script src="/Scripts/bootstrap-datepicker.zh-CN.min.js"></script>
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
                    <li class="nav-item dropdown active">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown01" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">设备详情</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown01">
                            <a class="dropdown-item" href="#">班次信息</a>
                            <a class="dropdown-item" href="WarningDetails.aspx">警告信息</a>
                            <a class="dropdown-item" href="ConsumableDetails.aspx">易损件信息</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown02" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">系统设置</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown02">
                            <a class="dropdown-item" href="WarningConfig.aspx">报警配置</a>
                            <a class="dropdown-item" href="ConsumableConfig.aspx">易损件配置</a>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <form id="form1" runat="server">
        <main role="main">
            <div style="display: inline-flex;">
                <div class="input-group mb-3" style="margin: 5px; width:225px;">
                    <div class="input-group-prepend">
                        <span class="input-group-text">设备编号</span>
                    </div>
                    <asp:TextBox ID="TextBox_DeviceID" runat="server" type="text" CssClass="form-control" placeholder="请输入设备编号" autocomplete="off" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                </div>
                <div id="date-picker" style="margin: 5px;">
                    <div class="input-daterange input-group" id="datepicker" style="width: 450px;">
                        <div class="input-group-prepend">
                        <span class="input-group-text">日期从</span>
                    </div>
                        <asp:TextBox ID="TextBox_DateFrom" runat="server" type="text" CssClass="input-sm form-control" name="start" autocomplete="off" />
                        <span class="input-group-text">到</span>
                        <asp:TextBox ID="TextBox_DateTo" runat="server" type="text" CssClass="input-sm form-control" name="end" autocomplete="off" />
                    </div>
                </div>
                <div>
                    <asp:Button ID="SearchButton" runat="server" CssClass="btn btn-primary" Text="查询" Style="margin:5px;" OnClick="Search_Click" />
                </div>
            </div>
            <ul class="nav nav-tabs" style="margin-left:5px;margin-right:5px;">
                <li class="nav-item">
                    <asp:LinkButton ID="Nav_LinkButton11" CssClass="nav-link active" Text="班次信息查询" runat="server" href="#"></asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton ID="Nav_LinkButton12" CssClass="nav-link" Text="警告信息查询" runat="server" href="WarningDetails.aspx"></asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton ID="Nav_LinkButton13" CssClass="nav-link" Text="易损件信息查询" runat="server" href="ConsumableDetails.aspx"></asp:LinkButton>
                </li>
            </ul>
            <div style="margin-left: 5px; margin-right: 5px;">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="Empty_Card" class="card" runat="server" visible="false">
                            <div class="card-body">
                                <h4 class="card-title">班次信息为空</h4>
                                <p class="card-text">请尝试修改设备编号和查询日期。</p>
                            </div>
                        </div>
                        <asp:GridView ID="ShiftGridView" runat="server" CssClass="table table-hover table-bordered" BorderStyle="None" CellPadding="0" GridLines="None" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="ShiftGridView_PageIndexChanging" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="Shift" HeaderText="班次" />
                                <asp:BoundField DataField="Count" HeaderText="生产数量" />
                                <asp:BoundField DataField="StartTime" HeaderText="上班时间" />
                                <asp:BoundField DataField="StopTime" HeaderText="下班时间" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </main>
        <asp:Literal ID="ltScript" runat="server"></asp:Literal> 
    </form>
</body>
</html>
