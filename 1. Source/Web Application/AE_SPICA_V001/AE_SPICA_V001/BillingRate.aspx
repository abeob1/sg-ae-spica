<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingRate.aspx.cs" Inherits="AE_SPICA_V001.BillingRate"
    EnableEventValidation="false" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="BillingRate" ContentPlaceHolderID="CPHSiteMaster" runat="server">
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

        function colon(e) {

            var x = document.getElementById('txtTimeDuration');

            var key = window.event ? e.keyCode : e.which;

            if (key != '8' && key != '46') {

                if (x.value.length == 2)

                    x.value += ":";
            }
        }

        function validateTime(x) {

            if (x.value.length == 5) {

                //var newreg = /^[0-2][0-3]:[0-5][0-9]$/;
                var newreg = /^(([0-1][0-9])|(2[0-3])):[0-5][0-9]$/;

                var first = x.value.split(":")[0];
                var second = x.value.split(":")[1];

                if (first > 24 || second > 59) {
                    alert("Invalid time format\n\n The valid format is hh:mm");
                    document.getElementById('txtTimeDuration').focus();
                }


                else if (!newreg.test(x.value)) {

                    alert("Invalid time format\n\n The valid format is hh:mm");
                    document.getElementById('txtTimeDuration').focus();
                }

            }
            else if (x.value != 0) {
                alert("Invalid time format\n\n The valid format is hh:mm");
                document.getElementById('txtTimeDuration').focus();
            }

            return false;
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
            <div class="col-lg-12">
                <table width="200" border="0" class="table borderless" cellpadding="2px;" cellspacing="2px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
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
                <div class="SpaceBox">
                </div>
            </div>
            <div class="col-lg-11 Maincontainer" style="overflow: auto;">
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
                        <asp:BoundField DataField="UserCode" HeaderText="User Code" SortExpression="UserCode"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Time" HeaderText="Time Duration (In Minutes)" SortExpression="Time"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="BillableUnit" HeaderText="Billable Unit" SortExpression="BillableUnit"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="HourlyRate" HeaderText="Hourly Rate" SortExpression="HourlyRate"
                            HeaderStyle-ForeColor="White" HeaderStyle-VerticalAlign="Middle" />
                        <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"
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
                                    <span>User Code</span>
                                </th>
                                <th>
                                    <span>User Name</span>
                                </th>
                                <th>
                                    <span>Time Duration (In Minutes)</span>
                                </th>
                                <th>
                                    <span>Billable Unit</span>
                                </th>
                                <th>
                                    <span>Hourly Rate</span>
                                </th>
                                <th>
                                    <span>Currency</span>
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
            <div class="col-lg-12">
                <div class="HeaderBox">
                    Billing Rate Configuration
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
                            <asp:Label ID="Label9" runat="server" Text="User Name"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control input-little">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Time Duration (In Minutes)"></asp:Label><span
                                class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtTimeDuration" runat="server" class="form-control input-little"
                                onkeypress="CheckNumeric(event);" MaxLength="5"></asp:TextBox>
                            <%--<cc1:MaskedEditExtender ID="meeInTime" runat="server" MaskType="Time" Mask="99:99" onblur="validatetime();
                                MessageValidatorTip="true" ErrorTooltipEnabled="true" TargetControlID="txtTimeDuration"
                                InputDirection="LeftToRight" AcceptNegative="Left">
                            </cc1:MaskedEditExtender>--%>
                            <%--<asp:TextBox ID="txtTimeDuration" runat="server" onkeyup="colon(event)" MaxLength="5"
                                onblur="validateTime(this)" class="form-control input-little"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Billing Unit"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillingUnit" runat="server" class="form-control input-little"
                                onkeypress="CheckNumeric(event);" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Hourly Rate"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtHourlyRate" runat="server" class="form-control input-little"
                                MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Currency"></asp:Label><span class="mandatory">*</span>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrency" runat="server" class="form-control input-little" MaxLength="10"
                                ReadOnly="true"></asp:TextBox>
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
