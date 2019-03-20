<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WarningConfig.aspx.cs" Inherits="DTSServer.WarningConfig" %>

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
                            <a class="dropdown-item" href="#">报警配置</a>
                            <a class="dropdown-item" href="ConsumableConfig.aspx">易损件配置</a>
                            <a class="dropdown-item" href="CleanHistoryConfig.aspx">清理数据配置</a>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <button class="btn btn-primary" data-toggle="modal" data-target="#myModal" style="margin: 5px;">新增警告配置</button>
    <form id="form1" runat="server">
        <main role="main">
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel">新增警告配置</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <asp:Label runat="server" Text="名称:" style="margin-left:20px;margin-right:42px;"/>
                                <asp:TextBox ID="Model1_TextBox1" runat="server" Text="警告"/>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="等级:" style="margin-left:20px;margin-right:42px;"/>
                                <asp:TextBox ID="Model1_TextBox2" runat="server" text="1" onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="是否弹窗:" style="margin-left:20px;margin-right:10px;"/>
                                <asp:DropDownList ID="Model1_DropDownList1" runat="server" DataValueField="Popup" DataTextField="Popup">
                                    <asp:ListItem Value="1">是</asp:ListItem>
                                    <asp:ListItem Value="0">否</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" Text="处理方式:" style="margin-left:20px;margin-right:10px;"/>
                                <asp:TextBox ID="Model1_TextBox4" runat="server" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                            <asp:Button ID="AddWarning" runat="server" CssClass="btn btn-primary" Text="提交" OnClick="WarningGridView_AddingRow" />
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal -->
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="margin: 5px;">
                        <div id="Empty_Card" class="card" runat="server" visible="false">
                            <div class="card-body">
                                <h4 class="card-title">警告配置为空</h4>
                                <p class="card-text">请点击左上角按钮新增警告配置。</p>
                            </div>
                        </div>
                        <asp:GridView ID="WarningGridView" runat="server" CssClass="table table-hover table-bordered" BorderStyle="None" CellPadding="0" GridLines="None" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="WarningGridView_PageIndexChanging" OnRowCancelingEdit="WarningGridView_RowCancelingEdit" OnRowEditing="WarningGridView_RowEditing" AllowSorting="True" OnRowDeleting="WarningGridView_RowDeleting" OnRowUpdating="WarningGridView_RowUpdating">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="True" />
                                <asp:BoundField DataField="Name" HeaderText="名称" />
                                <asp:TemplateField HeaderText="等级">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Level")%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%#Eval("Level")%>' onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="是否弹窗">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Convert.ToBoolean(Eval("Popup"))?"是":"否"%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="DropDown1" runat="server" DataValueField="Popup" DataTextField="Popup">
                                            <asp:ListItem Value="1">是</asp:ListItem>
                                            <asp:ListItem Value="0">否</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Treatment" HeaderText="处理方式" />
                                <asp:CommandField ShowEditButton="True" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" OnClientClick="return confirm('请确认是否删除该警告信息？');">删除</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </main>
    </form>
</body>
</html>
