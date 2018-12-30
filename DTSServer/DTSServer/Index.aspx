<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DTSServer.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <style type="text/css">
        table,table tr th, table tr td { border:1px solid #000000; }
        table { width: 1024px; min-height: 25px; line-height: 25px; text-align: center; border-collapse: collapse; padding:2px;}  

        table tr.alter { background-color: aliceblue; }
        table tr.normal:hover { background-color: #0180FE; color: #fff; }
        table tr.alter:hover { background-color: #0180FE; color: #fff; }
    </style>
    <div>
        <h1>Device List</h1>
        <hr />
    </div>
    <div>
        <a href="Configration.aspx">Configration</a>
    </div>
    <hr />
    <form id="Form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <table>
                                <tr>
                                    <th>ID</th>
                                    <th>IP adress</th>
                                    <th>Status</th>
                                    <th>Latest warning</th>
                                    <th>Running Time</th>
                                    <th>Details</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="normal">
                                <td><%#Eval("ID") %></td>
                                <td><%#Eval("IP") %></td>
                                <td><%#Eval("Status") %></td>
                                <td><%#Eval("Warning") %></td>
                                <td><%#Eval("Time") %></td>
                                <td><a href="Details.aspx?id=<%#Eval("ID") %>">Details</a></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alter">
                                <td><%#Eval("ID") %></td>
                                <td><%#Eval("IP") %></td>
                                <td><%#Eval("Status") %></td>
                                <td><%#Eval("Warning") %></td>
                                <td><%#Eval("Time") %></td>
                                <td><a href="Details.aspx?id=<%#Eval("ID") %>">Details</a></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
