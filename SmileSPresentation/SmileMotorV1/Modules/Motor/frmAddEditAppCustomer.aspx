<%@ Page Title="บันทึกข้อมูลผู้เอาประกัน" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAddEditAppCustomer.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAddEditAppCustomer" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditCustomer.ascx" TagPrefix="uc1" TagName="ucAddEditCustomer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditAddress.ascx" TagPrefix="uc1" TagName="ucAddEditAddress" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPersonSearchByIdentityCardNumber.ascx" TagPrefix="uc1" TagName="ucPersonSearchByIdentityCardNumber" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagName="ucAlert" TagPrefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPersonType.ascx" TagPrefix="uc1" TagName="ddlPersonType" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditCorporate.ascx" TagPrefix="uc1" TagName="ucAddEditCorporate" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc2:ucAlert ID="ucAlert1" runat="server"/>
    <table style="width: 100%;">
        <tr>
            <td style="width: 80%; text-align: right;">
                <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large" Text="บันทึก Application 1/4 : ข้อมูลผู้เอาประกัน"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="container">

        <table style="width: 100%;">
            <tr>
                <td colspan="3" style="width: 50%;">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4>
                                <asp:Label ID="lblTextHeader" runat="server" Text="ประเภทผู้เอาประกัน"></asp:Label>
                            </h4>
                        </div>
                        <div class="panel-body">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 12.5%; text-align: right;">ประเภท :</td>
                                    <td style="width: 12.5%">
                                        <uc1:ddlPersonType runat="server" ID="ddlPersonType1" OneSelectChanged="ddlPersonType_eSelectChanged" AutoPostback="true" />
                                    </td>
                                    <td style="width: 12.5%"></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td colspan="3" style="width: 50%;">
                    <uc1:ucPersonSearchByIdentityCardNumber runat="server"
                        ID="ucPersonSearchByIdentityCardNumber"
                        OneClickClear="ucPersonSearchByIdentityCardNumber_eClickClear"
                        OneSelectedPersonIDChanged="ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged"
                        OneClickPersonIDChangednull="ucPersonSearchByIdentityCardNumber_eClickPersonIDChangednull" />
                </td>
            </tr>
        </table>

        <div id="AddCustomerPanel">
            <uc1:ucAddEditCustomer runat="server" ID="ucAddEditCustomer" />
        </div>

        <div id="AddCorporatePanel">
            <uc1:ucAddEditCorporate runat="server" ID="ucAddEditCorporate" />
        </div>

        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#home">ที่อยู่ปัจจุบัน</a></li>
            <li><a data-toggle="tab" href="#menu1">ที่อยู่ที่ทำงาน</a></li>
            <li><a data-toggle="tab" href="#menu2">ที่อยู่ตามบัตรประชาชน</a></li>
        </ul>

        <div class="tab-content">
            <div id="home" class="tab-pane fade in active">
                <div class="panel panel-default">
                    <div class="panel-body">

                        <uc1:ucAddEditAddress runat="server" ID="ucAddEditAddress" />
                    </div>
                </div>

            </div>

            <div id="menu1" class="tab-pane fade">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnCopyAddress" runat="server" Text="คัดลอกจากที่อยู่ปัจจุบัน" OnClick="btnCopyAddress_Click" />

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <uc1:ucAddEditAddress runat="server" ID="ucAddEditAddress1" />
                    </div>
                </div>
            </div>
            <div id="menu2" class="tab-pane fade">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <asp:UpdatePanel ID="up2" runat="server">
                            <ContentTemplate>

                                <asp:Button ID="btnCopyAddress1" runat="server" Text="คัดลอกจากที่อยู่ปัจจุบัน" OnClick="btnCopyAddress1_Click" />

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <uc1:ucAddEditAddress runat="server" ID="ucAddEditAddress2" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center"></td>
            <td style="width: 12.5%; text-align: center">
                <asp:Button ID="btnForward" runat="server" Text="ถัดไป" SkinID="info" Width="100%" OnClick="btnForward_Click" />
            </td>
        </tr>
    </table>

    <asp:HiddenField ID="hdfCustomer_ID" runat="server" />
    <asp:HiddenField ID="hdfPayer_ID" runat="server" />
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
    <asp:HiddenField ID="hdfPersonTypeID" runat="server" />
</asp:Content>
