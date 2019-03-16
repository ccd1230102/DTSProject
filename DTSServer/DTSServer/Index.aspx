<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DTSServer.Index" %>

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
                    <li class="nav-item active">
                        <a class="nav-link" href="#">设备列表<span class="sr-only">(current)</span></a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown01" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">设备详情</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown01">
                            <a class="dropdown-item" href="DeviceDetails.aspx">班次信息</a>
                            <a class="dropdown-item" href="WarningDetails.aspx">警告信息</a>
                            <a class="dropdown-item" href="ConsumableDetails.aspx">易损件信息</a>
                        </div>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown02" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">系统设置</a>
                        <div class="dropdown-menu" aria-labelledby="dropdown02">
                            <a class="dropdown-item" href="WarningConfig.aspx">报警配置</a>
                            <a class="dropdown-item" href="DeviceConfig.aspx">易损件配置</a>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <form id="Form1" runat="server">
        <main role="main">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="margin: 5px;">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover table-bordered">
                                    <thead>
                                        <tr>
                                            <th>编号</th>
                                            <th>当前班次</th>
                                            <th>生产数量</th>
                                            <th>开机时间</th>
                                            <th>停机时间</th>
                                            <th>运行时间</th>
                                            <th>0故障运行时间</th>
                                            <th>运行状态</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("ID") %></td>
                                    <td><%#Eval("Shift") %></td>
                                    <td><%#Eval("Count") %></td>
                                    <td><%#Eval("StartTime") %></td>
                                    <td><%#Eval("StopTime") %></td>
                                    <td><%#Eval("RunningTime") %></td>
                                    <td><%#Eval("ZeroWarningTime") %></td>
                                    <td><%#Eval("Status") %></td>
                                    <td><a href="DeviceDetails.aspx?id=<%#Eval("ID") %>&from=<%=mNowTime %>&to=<%=mNowTime %>">详情</a></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                            </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div id="Empty_Card" class="card" runat="server" visible="false">
                            <div class="card-body">
                                <h4 class="card-title">设备列表为空</h4>
                                <p class="card-text">当前无法找到任何设备。</p>
                                <p id="Error_Text" class="card-text" runat="server" style="color:red;"/>
                            </div>
                        </div>
                    </div>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </main>
    </form>
</body>
</html>
