<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimEntryReport.aspx.cs"
    Inherits="AE_SPICA_V001.TimEntryReport" EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="TimeEntryReport" ContentPlaceHolderID="CPHSiteMaster" runat="server">
    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="col-lg-12">
                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
                <asp:Label ID="lblSuccess" runat="server" Visible="false" ForeColor="Green" Font-Bold="true"
                    Style="margin-left: 8px;"></asp:Label>
            </div>
            <div class="col-lg-12">
                <div class="col-lg-6">
                    <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="To Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="From Company"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFromCompany" runat="server" CssClass="form-control input-little">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="To Company"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlToCompany" runat="server" CssClass="form-control input-little">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-lg-12">
                    <div class="ScreenBtn">
                        <asp:Button ID="btnShow" runat="server" Text="Show Report" class="action ScreenICEButton"
                            OnClick="btnShow_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="action ScreenICEButton"
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="SpaceBox">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
