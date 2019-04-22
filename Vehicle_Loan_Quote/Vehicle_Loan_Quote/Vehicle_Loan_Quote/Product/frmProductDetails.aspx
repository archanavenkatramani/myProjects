<%@ Page Title="Quotation" Language="C#" MasterPageFile="~/MasterPage/LoanDetails.Master" AutoEventWireup="true" CodeBehind="frmProductDetails.aspx.cs" Inherits="Vehicle_Loan_Quote.Product.frmProductDetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            $("[id*=txtVehiclePrice]").on('keyup', function () {
                if ($(this).val() != "") {
                    var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                    $(this).val(n.toLocaleString());
                }
            });
            $("[id*=txtDepositAmount]").on('keyup', function () {
                if ($(this).val() != "") {
                    var n = parseInt($(this).val().replace(/\D/g, ''), 10);
                    $(this).val(n.toLocaleString());
                }
            });

            $("[id$=txtDeliveryDate]").mask("99/99/9999");

            $("[id*=txtDeliveryDate]").datepicker({ dateFormat: 'dd/mm/yy' });

            $(function () {
                $("[id*=txtDeliveryDate]").datepicker();
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function ValidateCalculate() {
            var vehiclePrice = document.getElementById("<%=txtVehiclePrice.ClientID %>").value;
            var depositAmount = document.getElementById("<%=txtDepositAmount.ClientID %>").value;
            var deliveryDate = document.getElementById("<%=txtDeliveryDate.ClientID %>").value;
            var term = document.getElementById("<%=ddlTerm.ClientID %>").value;

            if (vehiclePrice != "" && vehiclePrice.indexOf(',') != -1)
                vehiclePrice = vehiclePrice.replace(',', '');

            if (depositAmount != "" && depositAmount.indexOf(',') != -1)
                depositAmount = depositAmount.replace(',', '');

            var percentAmount = (Math.round((vehiclePrice * 15) / 100));

            if (vehiclePrice == "" || vehiclePrice == "0" || vehiclePrice == "0.00") {
                alert("Vehicle price should not be empty or 0 pound. Please enter a valid amount.");
                return false;
            }
            else if (depositAmount == "" || depositAmount == "0" || depositAmount == "0.00") {
                alert("Deposit amount should not be empty or 0 pound. Please enter a valid amount.");
                return false;
            }
            else if (parseFloat(depositAmount) > parseFloat(vehiclePrice)) {
                alert("Deposit amount should not be more than vehicle price. Please enter a correct amount.");
                return false;
            }
            else if (parseFloat(depositAmount) < parseFloat(percentAmount)) {
                alert("There should be a minimum of 15% deposit amount. Please enter a correct amount");
                return false;
            }
            else if (deliveryDate == "") {
                alert("Please select delivery date");
                return false;
            }
            else if (term == "0" || term == "Select") {
                alert("Please select loan term");
                return false;
            }

            return true;
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tbl" cellpadding="0" cellspacing="0" width="100%">
        <tr class="trHeader">
            <td colspan="2" class="tdHeader">Loan Details
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 15%" class="tdFieldsHeader">Vehicle price (in GBP)
            </td>
            <td style="width: 85%">
                <asp:TextBox ID="txtVehiclePrice" runat="server" CssClass="textBox" onkeypress="return isNumberKey(event)" />
            </td>
        </tr>
        <tr>
            <td class="tdFieldsHeader">Deposit amount (in GBP)
            </td>
            <td>
                <asp:TextBox ID="txtDepositAmount" runat="server" CssClass="textBox" onkeypress="return isNumberKey(event)" />
            </td>
        </tr>
        <tr>
            <td class="tdFieldsHeader">Delivery date
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryDate" CssClass="textBox" Width="80px" runat="server"></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td class="tdFieldsHeader">Term (in Years)
            </td>
            <td>
                <asp:DropDownList ID="ddlTerm" runat="server" CssClass="dropDown">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;

            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Calculate" CssClass="btn" OnClientClick="if (!ValidateCalculate()) { return false;};" OnClick="btnSubmit_Click" />&nbsp;
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn" OnClick="btnReset_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="divQuote" runat="server">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr class="trHeader">
                            <td colspan="2" class="tdHeader">Loan Summary
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="tdFieldsHeader">Total loan amount
                            </td>
                            <td style="width: 85%">
                                <asp:Label ID="lblTotalLoanAmount" runat="server" Text="" CssClass="label" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdFieldsHeader">Monthly payable
                            </td>
                            <td>
                                <asp:Label ID="lblMontlyPayable" runat="server" Text="" CssClass="label" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tdFieldsHeader">
                                <asp:GridView ID="gridLoanDetails" runat="server" Width="40%" AllowPaging="false" AutoGenerateColumns="false"
                                    EmptyDataText="No Records Found" CssClass="Grid">
                                    <Columns>
                                        <asp:BoundField HeaderText="Date" DataField="loanDate" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="10%" />
                                        <asp:BoundField HeaderText="Loan Amount" DataField="loanAmount" HeaderStyle-Width="15%" DataFormatString="{0:C}" />
                                        <asp:BoundField HeaderText="Amount Due" DataField="amountDue" HeaderStyle-Width="15%" DataFormatString="{0:C}" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
