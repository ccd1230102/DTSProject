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
        <h1>设备列表</h1>
        <hr />
    </div>
    <div>
        <a href="Configration.aspx">设备配置</a>
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
                                    <th>编号</th>
                                    <th>班次</th>
                                    <th>生产数量</th>
                                    <th>运行时间</th>
                                    <th>0故障运行时间</th>
                                    <th>停机时间</th>
                                    <th>运行状态</th>
                                    <th></th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="normal">
                                <td><%#Eval("ID") %></td>
                                <td><%#Eval("Shift") %></td>
                                <td><%#Eval("Count") %></td>
                                <td><%#Eval("RunningTime") %></td>
                                <td><%#Eval("ZeroWarningTime") %></td>
                                <td><%#Eval("StopTime") %></td>
                                <td><%#Eval("Status") %></td>
                                <td><a href="Details.aspx?id=<%#Eval("ID") %>">详情</a></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alter">
                                <td><%#Eval("ID") %></td>
                                <td><%#Eval("Shift") %></td>
                                <td><%#Eval("Count") %></td>
                                <td><%#Eval("RunningTime") %></td>
                                <td><%#Eval("ZeroWarningTime") %></td>
                                <td><%#Eval("StopTime") %></td>
                                <td><%#Eval("Status") %></td>
                                <td><a href="Details.aspx?id=<%#Eval("ID") %>">详情</a></td>
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
