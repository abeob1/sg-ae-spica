<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeEntry.aspx.cs" Inherits="AE_SPICA_V001.TimeEntry"
    EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="TimeEntry" ContentPlaceHolderID="CPHSiteMaster" runat="server">
    <script type="text/javascript">
        function CheckNumeric(e) {

            if (window.event) // IE 
            {
                if ((e.keyCode < 48 || e.keyCode > 57) & e.keyCode != 8) {
                    event.returnValue = false;
                    return false;

                }
            }
            else { // Fire Fox
                if ((e.which < 48 || e.which > 57) & e.which != 8) {
                    e.preventDefault();
                    return false;

                }
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
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
                            <asp:Label ID="lblVessel" runat="server" Text="Vessel"></asp:Label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtVessel" runat="server" class="form-control input-little" MaxLength="20"></asp:TextBox>
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
                            <asp:TextBox ID="txtMember" runat="server" class="form-control input-little" MaxLength="20"></asp:TextBox>
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
                                MaxLength="4" onkeypress="CheckNumeric(event);"></asp:TextBox>
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
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnSelectedIndexChanged">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" HeaderStyle-ForeColor="White"
                            HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="FileReferenceNo" HeaderText="File Reference" SortExpression="FileReferenceNo"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Vessel" HeaderText="Vessel Name" SortExpression="Vessel"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="IncidentDate" HeaderText="Date" SortExpression="IncidentDate"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
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
                                    <span>Vessel Name</span>
                                </th>
                                <th>
                                    <span>Date</span>
                                </th>
                                <th>
                                    <span>Description</span>
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
                    Time Entry
                </div>
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-11 Maincontainer" style="overflow: auto;">
                <div class="col-lg-6">
                    <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimeEntryDate" runat="server" AutoPostBack="True" class="form-control input-little"
                                    MaxLength="10"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTimeEntryDate"
                                    PopupButtonID="Image1" Format="<%$Appsettings:DateFormat%>" CssClass=" cal_Theme1">
                                </cc1:CalendarExtender>
                                <asp:Label ID="lblId" runat="server" Text="Id" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="File Reference"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileReference" runat="server" class="form-control input-little"
                                    MaxLength="150" ReadOnly ="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Task"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTask" runat="server" AutoPostBack="true" CssClass="form-control input-little"
                                    OnSelectedIndexChanged="ddlTask_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Task Description"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" TextMode="multiline" class="form-control input-little"
                                    MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-lg-6">
                    <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Duration"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtDuration" runat="server" class="form-control input-little" MaxLength="20" AutoPostBack = "true"
                                    onkeypress="CheckNumeric(event);" OnTextChanged="txtDuration_onTextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Billable Unit"></asp:Label><span class="mandatory">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillableUnit" runat="server" class="form-control input-little" AutoPostBack = "true"
                                    MaxLength="20" onkeypress="CheckNumeric(event);" OnTextChanged="txtBillableUnit_onTextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Billable Amount" Visible="false"></asp:Label>
                            </td>
                            <%--<td>
                                
                            </td>--%>
                            <td>
                                <asp:TextBox ID="txtBillAmount" runat="server" class="form-control input-little"
                                    MaxLength="20" onkeypress="return isNumberKey(event)" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Private Remarks"></asp:Label>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrivateRemarks" runat="server" TextMode="MultiLine" class="form-control input-little"
                                    MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="col-lg-12">
                    <div class="ScreenBtn">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="action ScreenICEButton"
                            OnClick="btnSubmit_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="action ScreenICEButton"
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="SpaceBox">
                    </div>
                </div>
                <asp:GridView ID="grvTimeEntry" runat="server" CssClass="table noborder table-bordered"
                    AllowPaging="true" PageSize="10" AllowSorting="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="grvTimeEntry_PageIndexChanging">
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="pager-row myTableClass" />
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <HeaderStyle VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEdit" OnClick="lnkEdit_Click">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkDelete" OnClick="lnkDelete_Click" OnClientClick="return confirm('Are you sure you want delete');">Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Id" Visible="false">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" Width="100" />
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:yyyy-MM-dd}") %>'
                                    BorderStyle="none" />
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
                        <asp:TemplateField HeaderText="Description of Task">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text='<%# Bind("Duration") %>' BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billable Unit">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblBillableUnit" runat="server" Text='<%# Bind("BillableUnit") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Billable Amount">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblBillableAmount" runat="server" Text='<%# Bind("BillAmount") %>'
                                    BorderStyle="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText="Private Remarks">
                            <HeaderStyle VerticalAlign="Middle" Font-Bold="true" />
                            <ItemTemplate>
                                <asp:Label ID="lblPrivateRemarks" runat="server" Text='<%# Bind("PrivateRemarks") %>'
                                    BorderStyle="none" />
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
                                    <span>#</span>
                                </th>
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
                                    <span>Description of Task</span>
                                </th>
                                <th>
                                    <span>Duration</span>
                                </th>
                                <th>
                                    <span>Billable Unit</span>
                                </th>
                                <th>
                                    <span>Billable Amount</span>
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
            </div>
            <div class="col-lg-12">
                <div class="SpaceBox">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
