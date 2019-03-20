<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsumableDetails.aspx.cs" Inherits="DTSServer.ConsumableDetails" %>

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
                            <a class="dropdown-item" href="DeviceDetails.aspx">班次信息</a>
                            <a class="dropdown-item" href="WarningDetails.aspx">警告信息</a>
                            <a class="dropdown-item" href="#">易损件信息</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown02" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">系统设置</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown02">
                            <a class="dropdown-item" href="WarningConfig.aspx">报警配置</a>
                            <a class="dropdown-item" href="ConsumableConfig.aspx">易损件配置</a>
                            <a class="dropdown-item" href="CleanHistoryConfig.aspx">清理数据配置</a>
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
                    <asp:LinkButton ID="Nav_LinkButton11" CssClass="nav-link" Text="班次信息查询" runat="server" href="DeviceDetails.aspx"></asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton ID="Nav_LinkButton12" CssClass="nav-link" Text="警告信息查询" runat="server" href="WarningDetails.aspx"></asp:LinkButton>
                </li>
                <li class="nav-item">
                    <asp:LinkButton ID="Nav_LinkButton13" CssClass="nav-link active" Text="易损件信息查询" runat="server" href="ConsumableDetails.aspx"></asp:LinkButton>
                </li>
            </ul>
            <!-- Nav tabs -->
            <ul class="nav nav-tabs"  style="margin-left: 5px; margin-right: 5px; margin-top: 5px;">
                <li class="nav-item">
                    <a class="nav-link disabled" data-toggle="tab"></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link active" data-toggle="tab" href="#tab1">易损件状态</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" data-toggle="tab" href="#tab2">易损件更换历史</a>
                </li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content" style="margin-left: 5px; margin-right: 5px;">
                <div class="tab-pane active" id="tab1">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="Empty_Card1" class="card" runat="server" style="margin-left: 5px; margin-right: 5px;" visible="false">
                                <div class="card-body">
                                    <h4 class="card-title">易损件状态为空</h4>
                                    <p class="card-text">请尝试修改设备编号。</p>
                                </div>
                            </div>
                            <asp:GridView ID="ConsumableGridView" runat="server" CssClass="table table-hover table-bordered" BorderStyle="None" CellPadding="0" GridLines="None" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="ConsumableGridView_PageIndexChanging" AllowSorting="True">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="易损件编号" />
                                    <asp:BoundField DataField="Name" HeaderText="易损件名称" />
                                    <asp:TemplateField HeaderText="总寿命">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Convert.ToString(Eval("Limit")) + (Convert.ToInt32(Eval("Type")) == 0?"次":"小时")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="剩余寿命">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Convert.ToString(Eval("Residual")) + (Convert.ToInt32(Eval("Type")) == 0?"次":"小时")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="tab-pane" id="tab2">
                    <div id="Empty_Card2" class="card" runat="server" style="margin-left: 5px; margin-right: 5px;" visible="false">
                        <div class="card-body">
                            <h4 class="card-title">易损件更换历史为空</h4>
                            <p class="card-text">请尝试修改设备编号和查询日期。</p>
                        </div>
                    </div>
                    <asp:GridView ID="ConsumableReplaceGridView" runat="server" CssClass="table table-hover table-bordered" BorderStyle="None" CellPadding="0" GridLines="None" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="ConsumableReplaceGridView_PageIndexChanging" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="ConsumableID" HeaderText="易损件编号" />
                            <asp:BoundField DataField="Name" HeaderText="易损件名称" />
                            <asp:BoundField DataField="ReplacedTime" HeaderText="更换时间" />
                            <asp:BoundField DataField="ReplacedPeople" HeaderText="更换人员" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </main>
        <asp:Literal ID="ltScript" runat="server"></asp:Literal> 
    </form>
</body>
</html>
