<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AE_SPICA_V001.Login"
    MasterPageFile="~/LoginSite.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Registration" ContentPlaceHolderID="CPHLoginSite" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="Logo">
                <img src="Images/SPICA Logo1.png" />
            </div>
            <div class="LoginBox">
                <div class="LoginInputArea">
                    <table class="LoginTable">
                        <tr>
                            <td>
                                <asp:Label ID="lblUserCode" runat="server" Text="User Code" class="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserCode" runat="server" class="LoginTextbox" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPassword" runat="server" Text="Password" class="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="LoginTextbox"
                                    MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCompany" runat="server" Text="Company" class="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" class="LoginDropdown">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="LoginBtn">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" class="action ICEButton" OnClick="btnLogin_Click" />
                        <br />
                        <asp:Label ID="lblError" runat="server" class="Error"></asp:Label>
                    </div>
                </div>
            </div>
            <div align="center">
                <img src="Resources/imgBackground.jpg" class="bg" alt="Background" /></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
