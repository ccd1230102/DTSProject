<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="DTSServer.Details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <style>
        table,table tr th, table tr td { border:1px solid #000000; }
        table { width: 1024px; min-height: 25px; line-height: 25px; text-align: center; border-collapse: collapse; padding:2px;}   
        table tr:hover { background-color: #0180FE; color: #fff; }
    </style>
    <div>
        <h1>Device Details</h1>
        <hr />
    </div>
    <div>
        <a href="Index.aspx">Device List</a>
        <a href="Configration.aspx">Configration</a>
    </div>
    <hr />
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <th>ID</th>
                            <th>IP adress</th>
                            <th>Status</th>
                            <th>Latest warning</th>
                            <th>Running Time</th>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <div>
                            <asp:Table ID="WarningList" runat="server" Width="1024">
                                <asp:TableRow>
                                    <asp:TableHeaderCell ColumnSpan="6">Warning List</asp:TableHeaderCell> 
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell>ID</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Name</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Level</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Treatment</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Result</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Time</asp:TableHeaderCell> 
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                        <br />
                        <div>
                            <asp:Table ID="ConsumableList" runat="server" Width="1024">
                                <asp:TableRow>
                                    <asp:TableHeaderCell ColumnSpan="5">Consumable List</asp:TableHeaderCell> 
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableHeaderCell>Name</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Information</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Left Time</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Replace Time</asp:TableHeaderCell> 
                                    <asp:TableHeaderCell>Replace Person</asp:TableHeaderCell> 
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
