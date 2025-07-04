<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditCorporate.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditCorporate" %>

<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlCustomerTitle.ascx" TagPrefix="uc1" TagName="ddlCustomerTitle" %>

<div class="panel panel-default">
    <div class="panel-heading input-md">
        <h4>
            <asp:Label ID="lblTextHeader" runat="server" Text="ข้อมูลผู้เอาประกัน / นิติบุคคล"></asp:Label>
        </h4>
    </div>
    <div class="panel-body">
        <table style="width: 100%;">
            <tr>
                <td style="width: 16%; text-align: right;">ชื่อ :</td>
                <td style="width: 17%;">
                    <uc1:ddlCustomerTitle runat="server" id="ddlCustomerTitle" />
                </td>
                <td style="width: 37.5%;" colspan="3">
                    <asp:TextBox ID="txtCorporateName" runat="server"></asp:TextBox>
                </td>
                <td style="width: 12.5%; "></td>
            </tr>
            <tr>
                <td style="width: 12.5%; text-align: right;">เลขประจำตัวผู้เสียภาษี :</td>
                <td style="width: 12.5%;">
                    <asp:TextBox ID="txtTaxpayerNo" runat="server"></asp:TextBox>
                </td>
                <td style="width: 12.5%; text-align: right;">เบอร์โทรศัพท์ :</td>
                <td style="width: 12.5%; ">
                    <asp:TextBox ID="txtTaxpayerMobileNumber" runat="server"></asp:TextBox>
                </td>
                <td style="width: 12.5%; "></td>
                <td style="width: 12.5%; "></td>
            </tr>
        </table>
    </div>
</div>
