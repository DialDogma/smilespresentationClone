<%@ Page Title="บันทึกข้อมูลแอพพลิเคชั่นและเจ้าของผลงาน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAddEditAppProduct.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAddEditAppProduct" EnableEventValidation="false" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditApplication.ascx" TagPrefix="uc1" TagName="ucAddEditApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditVehicle.ascx" TagPrefix="uc1" TagName="ucAddEditVehicle" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPersonDetail_mini.ascx" TagPrefix="uc1" TagName="ucPersonDetail_mini" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucApplicationOwner.ascx" TagPrefix="uc1" TagName="ucApplicationOwner" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditPayMethod.ascx" TagPrefix="uc1" TagName="ucAddEditPayMethod" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailAppReNew.ascx" TagPrefix="uc1" TagName="ucAppDetailAppReNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 80%; text-align: right;">
                <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large" Text="บันทึก Application 3/4 : กรมธรรม์"></asp:Label>
            </td>
        </tr>
    </table>
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <div class="container">
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">ผู้เอาประกัน</h4>
                        </div>
                        <div class="panel-body">
                            <uc1:ucPersonDetail_mini runat="server" ID="ucPersonDetail_customer" />
                        </div>
                    </div>
                </td>
                <td style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">ผู้ชำระเบี้ย</h4>
                        </div>
                        <div class="panel-body">
                            <uc1:ucPersonDetail_mini runat="server" ID="ucPersonDetail_payer" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <uc1:ucApplicationOwner runat="server" ID="ucApplicationOwner" />
        <uc1:ucAddEditApplication runat="server"
            ID="ucAddEditApplication"
            OneMotorCompanyInsuranceSelectChanged="ucAddEditApplication_eMotorCompanyInsuranceSelectChanged"
            OneProductTypeSelectChanged="ucAddEditApplication_eProductTypeSelectChanged"
            OneProductSelectChanged="ucAddEditApplication_eProductSelectChanged" />
        <div class="panel panel-default" id="divRenew" runat="server" visible="False">
            <uc1:ucAppDetailAppReNew runat="server" ID="ucAppDetailAppReNew" />
        </div>
        <uc1:ucAddEditVehicle runat="server" ID="ucAddEditVehicle" />
        <uc1:ucAddEditPayMethod runat="server" ID="ucAddEditPayMethod" OnePeriodTypeSelectChanged="ucAddEditPayMethod_ePeriodTypeSelectChanged" />

        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnInputData" runat="server" OnClick="Button1_Click" Text="กรอกข้อมูล Test" Visible="false" />
                </td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center">&nbsp;</td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnBackward" runat="server" Text="ย้อนกลับ" SkinID="warning" Width="80%" OnClick="btnBackward_Click" /></td>
                <td style="width: 12.5%; text-align: center">
                    <asp:Button ID="btnForward" runat="server" Text="ถัดไป" SkinID="info" Width="100%" OnClick="btnForward_Click" /></td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdfCustomer_ID" runat="server" />
    <asp:HiddenField ID="hdfPayer_ID" runat="server" />
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
    <asp:HiddenField ID="hdfProduct_ID" runat="server" />
    <asp:HiddenField ID="hdfPeriodType_ID" runat="server" />
</asp:Content>