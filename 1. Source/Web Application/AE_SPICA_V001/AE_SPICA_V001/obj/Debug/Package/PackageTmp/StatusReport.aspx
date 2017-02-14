<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatusReport.aspx.cs" Inherits="AE_SPICA_V001.StatusReport"
    EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="StatusReport" ContentPlaceHolderID="CPHSiteMaster" runat="server">
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
                                <asp:Label ID="lblIncidentFromDate" runat="server" Text="File Creation From Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtIncidentFromDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIncidentFromDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="File Creation To Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtIncidentToDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtIncidentToDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Period From Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPeriodFromDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtPeriodFromDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Period To Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPeriodToDate" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtPeriodToDate"
                                    PopupButtonID="Image1" Format="yyyy-MM-dd" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="From CH"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFromCH" runat="server" CssClass="form-control input-little">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="To CH"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlToCH" runat="server" CssClass="form-control input-little">
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Status"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-little">
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
