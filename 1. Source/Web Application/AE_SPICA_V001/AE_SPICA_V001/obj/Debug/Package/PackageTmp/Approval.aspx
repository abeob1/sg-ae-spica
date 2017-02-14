<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approval.aspx.cs" Inherits="AE_SPICA_V001.Approval"
    EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Approval" ContentPlaceHolderID="CPHSiteMaster" runat="server">
    <script type="text/javascript">
        function CheckAll(oCheckbox) {
            var GridView2 = document.getElementById("<%=grvFile.ClientID %>");
            for (i = 1; i < GridView2.rows.length; i++) {
                GridView2.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    </script>
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
                            <asp:Label ID="lblMonth" runat="server" Text="Month"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtMonth" runat="server" class="form-control input-little" MaxLength="8"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtMonth"
                                PopupButtonID="Image1" Format="MMM yyyy" CssClass=" cal_Theme1">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl1" runat="server" Text="Approved Amount Base"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label ID="lblApprovedAmountBase" runat="server" Style="margin-left: 27px;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-lg-12">
                <div class="HeaderBox">
                    <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-11 Maincontainer" style="overflow: auto;">
                Tick to select
                <asp:GridView ID="grvFile" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvFile_PageIndexChanging">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <input id="Checkbox2" type="checkbox" onclick="CheckAll(this)" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID">
                            <HeaderStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' BorderStyle="none">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vessel">
                            <HeaderStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:Label ID="lblVessel" runat="server" Text='<%# Bind("Vessel") %>' BorderStyle="none">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Spica Ref No">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblSpicaRefNo" runat="server" Text='<%# Bind("SpicaRefNo") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Member's Name">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblMemberName" runat="server" Text='<%# Bind("MemberName") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Claim Handler">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblClaimHandler" runat="server" Text='<%# Bind("ClaimHandler") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Year">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblYear" runat="server" Text='<%# Bind("Year") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type of Bill">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lbltypeofBill" runat="server" Text='<%# Bind("TypeofBill") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Billable Amount">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lbltotalBillAmt" runat="server" Text='<%# Bind("TotalBillAmt") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Chargable Amount">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lbltotalChrgAmt" runat="server" Text='<%# Bind("TotalChrgAmt") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grand Total">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblgrandTotal" runat="server" Text='<%# Bind("GrandTotal") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkPreviewBilling" OnClick="lnkPeviewBilling_Click">Preview Billing</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkBillingDetails" OnClick="lnkBillingDetails_Click">Billing Details</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table class="table noborder table-bordered">
                            <tr valign="middle">
                                <th>
                                    <span>#</span>
                                </th>
                                <th>
                                    <span>ID</span>
                                </th>
                                <th>
                                    <span>Vessel</span>
                                </th>
                                <th>
                                    <span>Name</span>
                                </th>
                                <th>
                                    <span>Company</span>
                                </th>
                                <th>
                                    <span>Spica Ref No</span>
                                </th>
                                <th>
                                    <span>Member's Name</span>
                                </th>
                                <th>
                                    <span>Claim Handler</span>
                                </th>
                                <th>
                                    <span>Year</span>
                                </th>
                                <th>
                                    <span>Type of Bill</span>
                                </th>
                                <th>
                                    <span>Total Billable Amount</span>
                                </th>
                                <th>
                                    <span>Total Chargeable Amount</span>
                                </th>
                                <th>
                                    <span>Grand Total</span>
                                </th>
                                <th>
                                    <span>#</span>
                                </th>
                                <th>
                                    <span>#</span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="15">
                                    <span>No Data</span>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <div class="col-lg-12">
                <div class="ScreenBtn">
                    <asp:Button ID="btnApprove" runat="server" Text="Approve" class="action ScreenICEButton"
                        OnClick="btnApprove_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnReject" runat="server" Text="Reject" class="action ScreenICEButton"
                        OnClick="btnReject_Click" />
                </div>
            </div>
            <%-- THis part of code is to display the Billing Details pop up--%>
            <asp:Button ID="btnHiddenOpen" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpePopup" runat="server" TargetControlID="btnHiddenOpen"
                PopupControlID="panShow">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panShow" runat="server" Style="width: 900px; height: 600px; background-color: #F8F8F8; overflow :scroll;
                border: 2px solid #C8C8C8">
                <asp:UpdatePanel runat="server" ID="updatepnl" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="col-lg-12">
                            <div class="SpaceBox">
                            </div>
                        </div>
                        <div class="col-lg-11 Maincontainer" style="overflow: scroll;">
                            TimeEntry
                            <asp:GridView ID="grvTimeEntry" runat="server" CssClass="table noborder table-bordered"
                                AllowPaging="true" PageSize="100" AllowSorting="True" AutoGenerateColumns="False"
                                OnPageIndexChanging="grvTimeEntry_PageIndexChanging">
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <HeaderStyle VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>'
                                                BorderStyle="none">
                                            </asp:Label>
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
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table noborder table-bordered">
                                        <tr valign="middle">
                                            <th>
                                                <span>Date</span>
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
                                            <td colspan="9">
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
                        <div class="col-lg-11 Maincontainer" style="overflow:scroll;">
                            Expense Entry
                            <asp:GridView ID="grvExpenseEntry" runat="server" CssClass="table noborder table-bordered"
                                AllowPaging="true" PageSize="100" AllowSorting="True" AutoGenerateColumns="False"
                                OnPageIndexChanging="grvExpenseEntry_PageIndexChanging">
                                <PagerSettings Mode="NumericFirstLast" />
                                <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <HeaderStyle VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpenseDate" runat="server" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>'
                                                BorderStyle="none">
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Expenses">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpense" runat="server" Text='<%# Bind("Expense") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chargeable Amount">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblChrgAmt" runat="server" Text='<%# Bind("ChargableAmt") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Currency">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server" Text='<%# Bind("Currency") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblUser" runat="server" Text='<%# Bind("User") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' BorderStyle="none" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table class="table noborder table-bordered">
                                        <tr valign="middle">
                                            <th>
                                                <span>Date</span>
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
                                            <td colspan="7">
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
                            <div class="ScreenBtn">
                                <asp:Button ID="btnClose" runat="server" Text="Close" class="action ScreenICEButton"
                                    OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- <asp:UpdateProgress ID="UpdateProg2" DisplayAfter="0" runat="server">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel2" runat="server" CssClass="updateProgress">
                            <div style="text-align: center; padding-top: 350px; padding-left: 50px; width: 800px;
                                height: 700px;">
                                <asp:Image ID="Image1" runat="server" Height="50px" ImageUrl="~/Images/loading.gif"
                                    Width="50px" />
                            </div>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>
            </asp:Panel>

            <%-- THis part of code is to display the Preview billing pop up--%>
            <%--<asp:Button ID="btnHiddenOpen1" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpePrevBiling" runat="server" TargetControlID="btnHiddenOpen1"
                PopupControlID="PrevBillingShow">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="PrevBillingShow" runat="server" Style="width: 900px; height: 600px;
                background-color: #F8F8F8; border: 2px solid #C8C8C8; overflow: scroll;">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                            ToolPanelWidth="250px" Width="350px" ToolPanelView="None" />
                        <div class="col-lg-12">
                            <div class="ScreenBtn">
                                <asp:Button ID="btnClose1" runat="server" Text="Close" class="action ScreenICEButton"
                                    OnClick="btnClose1_Click" />
                            </div>
                        </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
