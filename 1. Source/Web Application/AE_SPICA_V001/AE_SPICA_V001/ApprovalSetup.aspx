<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalSetup.aspx.cs"
    Inherits="AE_SPICA_V001.ApprovalSetup" EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ApprovalSetupMaster" ContentPlaceHolderID="CPHSiteMaster" runat="server">
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
                            <asp:Label ID="lblClub" runat="server" Text="Company Name"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanySearch" runat="server" class="form-control input-little"
                                MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="ScreenBtn">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="action ScreenICEButton"
                        OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="col-lg-7 container" style="overflow: auto;">
                <asp:GridView ID="grvSearch" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="5" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvSearch_PageIndexChanging" OnSorting="grvSearch_Sorting"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="CompanyCode" HeaderText="Company Code" SortExpression="CompanyCode"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="UserCode" HeaderText="Approver Code" SortExpression="UserCode"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="UserName" HeaderText="Approver" SortExpression="UserName"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="ApprovedMonth" HeaderText="Last Approved Month" SortExpression="ApprovedMonth"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                    </Columns>
                    <EmptyDataTemplate>
                        <table class="table noborder table-bordered">
                            <tr valign="middle">
                                <th>
                                    <span>ID</span>
                                </th>
                                <th>
                                    <span>Company Code</span>
                                </th>
                                <th>
                                    <span>Company Name</span>
                                </th>
                                <th>
                                    <span>Approver Code</span>
                                </th>
                                <th>
                                    <span>Approver</span>
                                </th>
                                <th>
                                    <span>Last Approved Month</span>
                                </th>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <span>No Data</span>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-12">
                <div class="HeaderBox">
                    Approval Setup
                </div>
            </div>
            <div class="col-lg-12">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Company Name"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control input-little"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanging">
                            </asp:DropDownList>
                            <asp:Label ID="lblId" runat="server" Text="Id" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Approver"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlApprover" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Last Approved Month"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtApprovedMonth" runat="server" class="form-control input-little"
                                MaxLength="10" ReadOnly="true"></asp:TextBox>
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
