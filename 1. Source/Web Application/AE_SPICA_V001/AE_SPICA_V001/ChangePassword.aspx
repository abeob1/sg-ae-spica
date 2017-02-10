<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="AE_SPICA_V001.ChangePassword" EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Approval" ContentPlaceHolderID="CPHSiteMaster" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-lg-12">
                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
                <asp:Label ID="lblSuccess" runat="server" Visible="false" ForeColor="Green" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
            </div>
            <div class="col-lg-12" style="z-index: 20px;">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblOldPassword" runat="server" Text="Old Password" TextMode="Password"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtOldPassword" runat="server" class="form-control input-little"
                                MaxLength="20" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblNewPassword" runat="server" Text="New Password"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassword" runat="server" class="form-control input-little"
                                MaxLength="20" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" class="form-control input-little"
                                MaxLength="20" TextMode="Password"></asp:TextBox>
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
