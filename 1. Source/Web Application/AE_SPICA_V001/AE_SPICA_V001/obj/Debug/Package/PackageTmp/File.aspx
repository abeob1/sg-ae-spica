<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="File.aspx.cs" Async="true"
    Inherits="AE_SPICA_V001.File" EnableEventValidation="false" MasterPageFile="~/Site.Master"
    EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 56%;
        }
        .SAPTotal
        {
            color: Green;
        }
        .style2
        {
            width: 162px;
        }
    </style>
</asp:Content>
<asp:Content ID="File" ContentPlaceHolderID="CPHSiteMaster" runat="server">
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
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-5">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblVessel" runat="server" Text="Vessel/Member"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblClub" runat="server" Text="Club"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClubSearch" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMember" runat="server" Text="Member"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtMemberSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFileRef" runat="server" Text="File Reference"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileRefSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblClaimHandler" runat="server" Text="Claim Handler"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimHandlerSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtYearSearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label22" runat="server" Text="File Status"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFileStatusSearch" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <div class="ScreenBtn">
                    <asp:Button ID="btnSeach" runat="server" Text="Search" class="action ScreenICEButton"
                        OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="col-lg-7 container" style="overflow: auto;">
                <asp:GridView ID="grvSearch" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="5" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvSearch_PageIndexChanging" OnSorting="grvSearch_Sorting"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="grvSearch_SelectedIndexChanged">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <%--  <asp:TemplateField HeaderText="Id" SortExpression="ClubCode" Visible="false">
                            <HeaderStyle VerticalAlign="Middle" ForeColor="White" />
                            <ItemTemplate>
                                <asp:Label ID="lblGridId" runat="server" Text='<%# Bind("Id") %>' BorderStyle="none">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="FileReferenceNo" HeaderText="File Reference" SortExpression="FileReferenceNo"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Vessel" HeaderText="Vessel/Member Name" SortExpression="Vessel"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="IncidentDate" HeaderText="Date" SortExpression="IncidentDate"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Status" HeaderText="Status (Interim/Final)" SortExpression="Status"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                    </Columns>
                    <EmptyDataTemplate>
                        <table class="table noborder table-bordered">
                            <tr valign="middle">
                                <th>
                                    <span>ID</span>
                                </th>
                                <th>
                                    <span>File Reference</span>
                                </th>
                                <th>
                                    <span>Vessel/Member Name</span>
                                </th>
                                <th>
                                    <span>Date</span>
                                </th>
                                <th>
                                    <span>Status (Interim / final)</span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <span>No Data</span>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <div class="col-lg-12">
                <div class="HeaderBox">
                    File Reference
                </div>
            </div>
            <div class="col-lg-6">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblIncidentDate" runat="server" Text="Incident Date"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtIncidentDate" runat="server" class="form-control input-little"
                                MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIncidentDate"
                                PopupButtonID="Image1" Format="<%$Appsettings:DateFormat%>" CssClass=" cal_Theme1">
                            </cc1:CalendarExtender>
                            <asp:Label ID="lblId" runat="server" Text="Id" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Vessel/Member"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" class="form-control input-little" MaxLength="150"
                                OnTextChanged="txtVessel_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Club"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClub" runat="server" AutoPostBack="true" CssClass="form-control input-little"
                                OnSelectedIndexChanged="ddlClub_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label24" runat="server" Text="BP"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <%-- <asp:DropDownList ID="ddlClubBP" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>--%>
                            <div class="col-lg-9" style="padding-left: 0px; padding-right: 0px;">
                                <asp:TextBox ID="txtClubBP" runat="server" class="form-control input-little" MaxLength="150"
                                    EnableViewState="true"></asp:TextBox></div>
                            <div class="col-lg-2" style="padding-left: 10px;">
                                <asp:Button ID="btnClubBP" runat="server" Text="..." Width="25px" OnClick="btnClubBP_Click" /></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label25" runat="server" Text="Club BP Contact"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <%--<asp:TextBox ID="txtClubBPContact" runat="server" class="form-control input-little"
                                MaxLength="150"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddlClubBPContact" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Member"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtMember" runat="server" class="form-control input-little" MaxLength="150"
                                TextMode="multiline" Columns="50" Height="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label17" runat="server" Text="Address L1"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressL1" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Address L2"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressL2" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label19" runat="server" Text="Address L3"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressL3" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label20" runat="server" Text="Address L4"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddressL4" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="BillTo"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBillTo" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Billing Address"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillingAddress" runat="server" class="form-control input-little"
                                TextMode="multiline" Columns="50" Height="80px" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="File Reference" Visible="false"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileReference" runat="server" class="form-control input-little"
                                MaxLength="150" Visible="false"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Year"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtYear" runat="server" class="form-control input-little" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Close On"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCloseOn" runat="server" class="form-control input-little" MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtCloseOn"
                                PopupButtonID="Image1" Format="<%$Appsettings:DateFormat%>" CssClass=" cal_Theme1">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-lg-6">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Claim Handler"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimHandler" runat="server" class="form-control input-little"
                                MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Club Reference"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtClubReference" runat="server" class="form-control input-little"
                                MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="C/O"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCO" runat="server" class="form-control input-little" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Address"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="Contact Name"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtContactName" runat="server" class="form-control input-little"
                                MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Email ID"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailId" runat="server" class="form-control input-little" MaxLength="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="VAT Number"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtVATNo" runat="server" class="form-control input-little" MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" class="form-control input-little"
                                MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Place of Incident"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlaceofIncident" runat="server" class="form-control input-little"
                                MaxLength="254"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="File Status"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFileStatus" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Status"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:Label ID="lblPeriodDateFrom" runat="server" Text="Period Date From"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodDateFrom" runat="server" AutoPostBack="false" class="form-control input-little"
                                MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtPeriodDateFrom"
                                PopupButtonID="Image1" Format="<%$Appsettings:DateFormat%>" CssClass=" cal_Theme1">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Period Date To"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodDateTo" runat="server" AutoPostBack="false" class="form-control input-little"
                                MaxLength="10"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtPeriodDateTo"
                                PopupButtonID="Image1" Format="<%$Appsettings:DateFormat%>" CssClass=" cal_Theme1">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label23" runat="server" Text="Voyage Number"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtVoyageNumber" runat="server" class="form-control input-little"
                                MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="Location Store"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationStore" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
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
            <div class="col-lg-12">
                <div class="HeaderBox">
                    Billing Information
                </div>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-11 Maincontainer" style="overflow: auto;">
                TimeEntry
                <asp:GridView ID="grvTimeEntry" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvTimeEntry_PageIndexChanging" OnSorting="grvTimeEntry_Sorting"
                    OnRowDataBound="OnRowDataBoundTime">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" HeaderStyle-ForeColor="White"
                            DataFormatString="<%$Appsettings:GridDateFormat%>" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="FileReference" HeaderText="File Reference" SortExpression="FileReference"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Task" HeaderText="Task" SortExpression="Task" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Duration" HeaderText="Duration(In Minutes)" SortExpression="Duration"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="User" HeaderText="User" SortExpression="User" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="BillableRate" HeaderText="Billable Rate" SortExpression="BillableRate"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="BillAmount" HeaderText="Billable Amount" SortExpression="BillAmount"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <%--<asp:TemplateField HeaderText="Date">
                            <HeaderStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>'
                                    BorderStyle="none">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Reference">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblFileReference" runat="server" Text='<%# Bind("FileReference") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Task">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblTask" runat="server" Text='<%# Bind("Task") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration(In Minutes)">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text='<%# Bind("Duration") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblUser" runat="server" Text='<%# Bind("User") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billable Rate">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblBillableRate" runat="server" Text='<%# Bind("BillableRate") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billable Amount">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblBillAmount" runat="server" Text='<%# Bind("BillAmount") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Currency") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <EmptyDataTemplate>
                        <table class="table noborder table-bordered">
                            <tr valign="middle">
                                <th>
                                    <span>Date</span>
                                </th>
                                <th>
                                    <span>File Reference</span>
                                </th>
                                <th>
                                    <span>Task</span>
                                </th>
                                <th>
                                    <span>Duration(In Minutes)</span>
                                </th>
                                <th>
                                    <span>Billable Unit</span>
                                </th>
                                <th>
                                    <span>User</span>
                                </th>
                                <th>
                                    <span>Billing Rate</span>
                                </th>
                                <th>
                                    <span>Billing Amount</span>
                                </th>
                                <th>
                                    <span>Currency</span>
                                </th>
                                <th>
                                    <span>Status</span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <span>No Data</span>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
                <div>
                    <table align="Right" class="MovingRight">
                        <tr>
                            <td>
                                Total Billable Amount :
                            </td>
                            <td>
                                <asp:Label ID="lblBillAmt" runat="server" CssClass="Total"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-11 Maincontainer" style="overflow: auto;">
                Expense Entry
                <asp:GridView ID="grvExpenseEntry" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvExpenseEntry_PageIndexChanging" OnSorting="grvExpenseEntry_Sorting"
                    OnRowDataBound="OnRowDataBoundExpense">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" HeaderStyle-ForeColor="White"
                            DataFormatString="<%$Appsettings:GridDateFormat%>" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="FileReference" HeaderText="File Reference" SortExpression="FileReference"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Expense" HeaderText="Expenses" SortExpression="Expense"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="ChargableAmt" HeaderText="Chargeable Amount" SortExpression="ChargableAmt"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="User" HeaderText="User" SortExpression="User" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                    </Columns>
                    <EmptyDataTemplate>
                        <table class="table noborder table-bordered">
                            <tr valign="middle">
                                <th>
                                    <span>Date</span>
                                </th>
                                <th>
                                    <span>File Reference</span>
                                </th>
                                <th>
                                    <span>Expense</span>
                                </th>
                                <th>
                                    <span>Chargeable Amount</span>
                                </th>
                                <th>
                                    <span>Currency</span>
                                </th>
                                <th>
                                    <span>Remarks</span>
                                </th>
                                <th>
                                    <span>User</span>
                                </th>
                                <th>
                                    <span>Status</span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <span>No Data</span>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
                <div>
                    <table align="Right" class="MovingRight">
                        <tr>
                            <td>
                                Total Chargeable Amount :
                            </td>
                            <td>
                                <asp:Label ID="lblChargeableAmt" runat="server" CssClass="Total"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-12">
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%">
                                Type of Bill :
                            </td>
                            <td style="width: 49%">
                                <asp:DropDownList ID="ddlTypeofBill" runat="server" CssClass="form-control input-little">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 10%">
                                Grand Total :
                            </td>
                            <td>
                                <asp:Label ID="lblGrandTotal" runat="server" CssClass="Total"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                Lump Sum :
                            </td>
                            <td style="width: 49%;">
                                <asp:CheckBox ID="chkLumpSum" runat="server" Style="margin-left: 28px;"></asp:CheckBox>
                            </td>
                            <td style="width: 10%">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                Billing Details :
                            </td>
                            <td style="width: 49%">
                                <asp:TextBox ID="txtBillingDetails" runat="server" class="form-control input-little"
                                    MaxLength="150" TextMode="multiline" Columns="50" Height="80px" Width="517px"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td width="36%">
                                Total amount billed at Holding level :
                                <asp:Label ID="lblTotalInvAmount" runat="server" CssClass="SAPTotal" Font-Bold="true"></asp:Label>
                            </td>
                            <td width="19%">
                                Balance :
                                <asp:Label ID="lblBalance" runat="server" CssClass="SAPTotal" Font-Bold="true"></asp:Label>
                            </td>
                            <td width="25%">
                                Last Payment Date :
                                <asp:Label ID="lblLastPaymentDate" runat="server" CssClass="SAPTotal" Font-Bold="true"></asp:Label>
                            </td>
                            <td width="20%">
                                Last Rec Num :
                                <asp:Label ID="lblLastRecNum" runat="server" CssClass="SAPTotal" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="col-lg-12">
                <div class="ScreenBtn1">
                    <asp:Button ID="btnPreview" runat="server" Text="Preview Billing" class="action ScreenICEButton"
                        OnClick="btnPreview_Click" OnClientClick="document.forms[0].target ='_blank';" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnTimeSheet" runat="server" Text="Time Sheet" class="action ScreenICEButton"
                        OnClick="btnTimeSheet_Click" OnClientClick="document.forms[0].target ='_blank';" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnApproval" runat="server" Text="Send to Approval" class="action ScreenICEButton"
                        OnClick="btnApproval_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnHoldingInv" runat="server" Text="Extract Holding Inv" class="action ScreenICEButton"
                        OnClick="btnHoldingInv_Click" />
                </div>
            </div>
            <%-- THis part of code is to display the Preview billing pop up--%>
            <asp:Button ID="btnHiddenOpen1" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpePrevClub" runat="server" TargetControlID="btnHiddenOpen1"
                Enabled="True" DropShadow="true" DynamicServicePath="" PopupControlID="PrevClubShow">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="PrevClubShow" runat="server" Style="width: 900px; height: 600px; background-color: #F8F8F8;
                border: 2px solid #C8C8C8; overflow: scroll;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="col-lg-12">
                            <div class="HeaderBox">
                                Search
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="SpaceBox">
                            </div>
                        </div>
                        <div class="col-lg-5">
                            <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Search"></asp:Label>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGridSearch" runat="server" class="form-control input-little"
                                            MaxLength="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div class="ScreenBtn">
                                <asp:Button ID="btnGridSearch" runat="server" Text="Search" class="action ScreenICEButton"
                                    OnClick="btnGridSearch_Click" />
                            </div>
                        </div>
                        <%--OnSorting="grvClub_Sorting" --%>
                        <asp:GridView ID="grvClub" runat="server" CssClass="table noborder table-bordered"
                            AllowPaging="true" PageSize="500" AllowSorting="True" AutoGenerateColumns="False"
                            OnRowDataBound="grvClub_RowDataBound" OnSelectedIndexChanged="grvClub_SelectedIndexChanged"
                            OnPageIndexChanging="grvClub_PageIndexChanging">
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country"
                                    HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table noborder table-bordered">
                                    <tr valign="middle">
                                        <th>
                                            <span>ID</span>
                                        </th>
                                        <th>
                                            <span>Country</span>
                                        </th>
                                        <th>
                                            <span>City</span>
                                        </th>
                                        <th>
                                            <span>Name</span>
                                        </th>
                                        <th>
                                            <span>Code</span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <span>No Data</span>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="col-lg-12">
                            <div class="ScreenBtn">
                                <asp:Button ID="btnClose" runat="server" Text="Close" class="action ScreenICEButton"
                                    OnClick="btnClose_Click" />
                            </div>
                        </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <%-- THis part of code is to display the Holding Invoice pop up--%>
            <asp:Button ID="btnHiddenOpen2" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeHoldingInv" runat="server" TargetControlID="btnHiddenOpen2"
                Enabled="True" DropShadow="true" DynamicServicePath="" PopupControlID="PrevHoldingInv">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="PrevHoldingInv" runat="server" Style="width: 900px; height: 600px;
                background-color: #F8F8F8; border: 2px solid #C8C8C8; overflow: scroll;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                    <ContentTemplate>
                        <%--OnSorting="grvClub_Sorting" --%>
                        <asp:GridView ID="grvHoldingInv" runat="server" CssClass="table noborder table-bordered"
                            AllowPaging="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                            OnRowDataBound="grvHoldingInv_RowDataBound" OnSelectedIndexChanged="grvHoldingInv_SelectedIndexChanged"
                            OnPageIndexChanging="grvHoldingInv_PageIndexChanging">
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="OurRef" HeaderText="OurRef" SortExpression="OurRef" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" SortExpression="DocEntry"
                                    HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="DocNum" HeaderText="DocNum" SortExpression="DocNum" HeaderStyle-ForeColor="White"
                                    HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="DocDate" HeaderText="DocDate" SortExpression="DocDate"
                                    HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                                <asp:BoundField DataField="Cancelled" HeaderText="Cancelled" SortExpression="Cancelled"
                                    HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table noborder table-bordered">
                                    <tr valign="middle">
                                        <th>
                                            <span>Type</span>
                                        </th>
                                        <th>
                                            <span>OurRef</span>
                                        </th>
                                        <th>
                                            <span>DocEntry</span>
                                        </th>
                                        <th>
                                            <span>DocNum</span>
                                        </th>
                                        <th>
                                            <span>DocDate</span>
                                        </th>
                                        <th>
                                            <span>Cancelled</span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <span>No Data</span>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="col-lg-12">
                            <div class="ScreenBtn">
                                <asp:Button ID="btnHoldingInvPreview" runat="server" Text="Preview" class="action ScreenICEButton"
                                    OnClick="btnHoldingInvPreview_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnHoldingInvClose" runat="server" Text="Close" class="action ScreenICEButton"
                                    OnClick="btnHoldingInvClose_Click" />
                            </div>
                        </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
