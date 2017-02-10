<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AE_SPICA_V001.Registration"
    MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Registration" ContentPlaceHolderID="CPHSiteMaster" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-lg-12">
                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
                <asp:Label ID="lblSuccess" runat="server" Visible="false" ForeColor="Green" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
            </div>
            <div class="col-lg-12">
                <div class="HeaderBox">
                    Search
                </div>
            </div>
            <div class="col-lg-12">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblUser" runat="server" Text="User Code"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="ScreenBtn">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="action ScreenICEButton"
                        OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="col-lg-12">
                <div class="HeaderBox">
                    User Information
                </div>
            </div>
            <div class="col-lg-12">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" Text="" />&nbsp;&nbsp;&nbsp;Active
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="User Code"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserCode" runat="server" class="form-control input-little" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="User Name"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" class="form-control input-little" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Password"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control input-little"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Confirm Password"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password" class="form-control input-little"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="User Role"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Email"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control input-little" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Company Name"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" class="form-control input-little"
                                MaxLength="100" ReadOnly="true"></asp:TextBox>
                            <asp:TextBox ID="txtCompanyCode" runat="server" class="form-control input-little"
                                MaxLength="30" Visible="false"></asp:TextBox>
                            <asp:DropDownList ID="ddlCompany" runat="server" class="form-control input-little" Visible="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-lg-12">
                <div class="ScreenBtn">
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="action ScreenICEButton"
                        OnClick="btnSave_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="action ScreenICEButton"
                        OnClick="btnCancel_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
