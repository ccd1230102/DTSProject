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
        <h1>Device Configration</h1>
        <hr />
    </div>
    <div>
        <a href="Index.aspx">Device List</a>
    </div>
    <hr />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <h2>Device Setting</h2>
                    <h3>To be added</h3>
                </div>
                <br />
                <div>
                    <h2>Warning Setting</h2>
                    <asp:Button ID="AddWarning" runat="server" Text="Add" />
                    <asp:GridView ID="WarningGridView" runat="server" CssClass="mGrid" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="WarningGridView_PageIndexChanging" OnRowCancelingEdit="WarningGridView_RowCancelingEdit" OnRowEditing="WarningGridView_RowEditing" AllowSorting="True" OnRowDeleting="WarningGridView_RowDeleting" OnRowUpdating="WarningGridView_RowUpdating" Width="1024">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="Level" HeaderText="Level" />
                            <asp:BoundField DataField="Treatment" HeaderText="Treatment" />
                            <asp:CommandField ShowEditButton="True" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div>
                    <h2>Consumable Setting</h2>
                    <asp:Button ID="Button1" runat="server" Text="Add" />
                    <asp:GridView ID="ConsumableGridView" runat="server" AllowPaging="True" AutoGenerateColumns="False" OnPageIndexChanging="ConsumableGridView_PageIndexChanging" OnRowCancelingEdit="ConsumableGridView_RowCancelingEdit" OnRowEditing="ConsumableGridView_RowEditing" AllowSorting="True" OnRowDeleting="ConsumableGridView_RowDeleting" OnRowUpdating="ConsumableGridView_RowUpdating" Width="1024">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="Limit" HeaderText="Limit" />
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
