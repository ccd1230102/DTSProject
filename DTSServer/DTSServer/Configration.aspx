<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configration.aspx.cs" Inherits="DTSServer.Configration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <style type="text/css">
    </style>
    <div>
        <h1>设备配置</h1>
        <hr />
    </div>
    <div>
        <a href="Index.aspx">设备列表</a>
    </div>
    <hr />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="display:none;" >
                    <h2>设备配置</h2>
                    <h3>开发中。。。</h3>
                </div>
                <div>
                    <h2>报警信息配置</h2>
                    <asp:Button ID="AddWarning" runat="server" Text="添加" OnClick="WarningGridView_AddingRow"/>
                    <asp:GridView ID="WarningGridView" runat="server" CssClass="mGrid" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="WarningGridView_PageIndexChanging" OnRowCancelingEdit="WarningGridView_RowCancelingEdit" OnRowEditing="WarningGridView_RowEditing" AllowSorting="True" OnRowDeleting="WarningGridView_RowDeleting" OnRowUpdating="WarningGridView_RowUpdating" Width="1024">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="True" />
                            <asp:BoundField DataField="Name" HeaderText="名称" />
                            <asp:TemplateField HeaderText="等级">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Level")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%#Eval("Level")%>' onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Treatment" HeaderText="处理方式" />
                            <asp:CommandField ShowEditButton="True" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div>
                    <h2>易损件信息配置</h2>
                    <asp:Button ID="Button1" runat="server" Text="添加" OnClick="ConsumableGridView_AddingRow"/>
                    <asp:GridView ID="ConsumableGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="ConsumableGridView_PageIndexChanging" OnRowCancelingEdit="ConsumableGridView_RowCancelingEdit" OnRowEditing="ConsumableGridView_RowEditing" AllowSorting="True" OnRowDeleting="ConsumableGridView_RowDeleting" OnRowUpdating="ConsumableGridView_RowUpdating" Width="1024">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="编号" ReadOnly="True" />
                            <asp:BoundField DataField="Name" HeaderText="名称" />
                            <asp:BoundField DataField="Information" HeaderText="信息" />
                            <asp:TemplateField HeaderText="寿命(小时)">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Limit")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%#Eval("Limit")%>' onkeypress="if (event.keyCode < 48 || event.keyCode >57) event.returnValue = false;"/>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
