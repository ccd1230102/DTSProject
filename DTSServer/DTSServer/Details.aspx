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

        .tab-normal {
            BORDER-TOP-STYLE: none;
            BORDER-RIGHT-STYLE: none;
            BORDER-LEFT-STYLE: none;
            BACKGROUND-COLOR: #E0E0E0;
            BORDER-BOTTOM-STYLE: none;
            FONT-SIZE: 15px;
            cursor: pointer
        }

        .tab-selected {
            BORDER-TOP-STYLE: none;
            BORDER-RIGHT-STYLE: none;
            BORDER-LEFT-STYLE: none;
            BACKGROUND-COLOR: #000000;
            BORDER-BOTTOM-STYLE: none;
            COLOR: White;
            FONT-SIZE: 15px;
            cursor: pointer
        }
    </style>
    <div>
        <h1>设备详情</h1>
        <hr />
    </div>
    <div>
        <a href="Index.aspx">设备列表</a>
        <a href="Configration.aspx">设备配置</a>
    </div>
    <hr />
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table id="deviceTable">
                        <tr>
                            <th>编号</th>
                            <th>班次</th>
                            <th>生产数量</th>
                            <th>运行时间</th>
                            <th>0故障运行时间</th>
                            <th>停机时间</th>
                            <th>运行状态</th>
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
                            <td>
                                <asp:Label ID="Label6" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="TabButton1" Style="left: 16px; top: 24px;" OnClick="TabButton1_Click"
                        runat="server" Text="警告信息列表" CssClass="tab-selected"
                        Width="100px"></asp:Button>
                    <asp:Button ID="TabButton2" Style="left: 77px; top: 24px;" OnClick="TabButton2_Click"
                        runat="server" Text="易损件信息列表" CssClass="tab-normal"
                        Width="120px"></asp:Button>
                    <div>
                        <asp:Table ID="WarningList" runat="server" Width="1024">
                            <asp:TableRow>
                                <asp:TableHeaderCell>名称</asp:TableHeaderCell>
                                <asp:TableHeaderCell>报警时间</asp:TableHeaderCell>
                                <asp:TableHeaderCell>处理时间</asp:TableHeaderCell>
                                <asp:TableHeaderCell>处理方式</asp:TableHeaderCell>
                                <asp:TableHeaderCell>处理结果</asp:TableHeaderCell>
                                <asp:TableHeaderCell>处理时长(秒)</asp:TableHeaderCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:Table ID="ConsumableList" runat="server" Width="1024" Visible="false">
                            <asp:TableRow>
                                <asp:TableHeaderCell>名称</asp:TableHeaderCell>
                                <asp:TableHeaderCell>运行时间</asp:TableHeaderCell>
                                <asp:TableHeaderCell>寿命</asp:TableHeaderCell>
                                <asp:TableHeaderCell>更换时间</asp:TableHeaderCell>
                                <asp:TableHeaderCell>更换人员</asp:TableHeaderCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
