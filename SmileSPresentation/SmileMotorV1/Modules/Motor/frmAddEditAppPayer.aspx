<%@ Page Title="บันทึกข้อมูลผู้ชำระเบี้ย" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmAddEditAppPayer.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmAddEditAppPayer" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditPayer.ascx" TagPrefix="uc1" TagName="ucAddEditPayer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditAddress.ascx" TagPrefix="uc1" TagName="ucAddEditAddress" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPersonSearchByIdentityCardNumber.ascx" TagPrefix="uc1" TagName="ucPersonSearchByIdentityCardNumber" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlRelation.ascx" TagPrefix="uc1" TagName="ddlRelation" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucPersonDetail_mini.ascx" TagPrefix="uc1" TagName="ucPersonDetail_mini" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditCorporate.ascx" TagPrefix="uc1" TagName="ucAddEditCorporate" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPersonType.ascx" TagPrefix="uc1" TagName="ddlPersonType" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 80%; text-align: right;">
                <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large" Text="บันทึก Application 2/4 : ข้อมูลผู้ชำระเบี้ย"></asp:Label>
            </td>
        </tr>
    </table>
    <uc1:ucAlert runat="server" ID="ucAlert" />
    <div class="container">
        <table style="width: 100%">
            <tr>
                <td colspan="4" style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4 class="panel-title">ผู้เอาประกัน</h4>
                        </div>
                        <div class="panel-body">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="4" style="width: 100%">
                                        <uc1:ucPersonDetail_mini runat="server" ID="ucPersonDetail_mini" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td colspan="4" style="width: 50%">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4 class="panel-title">บันทึกข้อมูลผู้ชำระเบี้ย</h4>
                        </div>
                        <div class="panel-body">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%; text-align: right;" colspan="2">ความสัมพันธ์กับผู้เอาประกัน :</td>
                                    <td style="width: 50%">
                                        <uc1:ddlRelation runat="server" ID="ddlRelation" OneSelectedNotSamePerson="ddlRelation_eSelectedNotSamePerson" OneSelectedSamePerson="ddlRelation_eSelectedSamePerson" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>

        <table style="width: 100%;">
            <tr>
                <td colspan="3" style="width: 50%;">
                    <div class="panel panel-default">
                        <div class="panel-heading input-md">
                            <h4>
                                <asp:Label ID="lblTextHeader" runat="server" Text="ประเภทผู้ชำระเบี้ย"></asp:Label>
                            </h4>
                        </div>
                        <div class="panel-body">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 12.5%; text-align: right">ประเภท :</td>
                                    <td style="width: 12.5%">
                                        <uc1:ddlPersonType runat="server" ID="ddlPersonType1" AutoPostback="true" OneSelectChanged="ddlPersonType_eSelectChanged" />
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
                        OneSelectedPersonIDChanged="ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged"
                        OneClickClear="ucPersonSearchByIdentityCardNumber_eClickClear" />
                </td>
            </tr>
        </table>

        <uc1:ucAddEditPayer runat="server" ID="ucAddEditPayer" />
        <uc1:ucAddEditCorporate runat="server" ID="ucAddEditCorporate" />

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

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnCopyAddress1" runat="server" Text="คัดลอกจากที่อยู่ปัจจุบัน" OnClick="btnCopyAddress1_Click" />

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <uc1:ucAddEditAddress runat="server" ID="ucAddEditAddress2" />
                    </div>
                </div>
            </div>
        </div>

        <table style="width: 100%;">
            <tr>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center"></td>
                <td style="width: 12.5%; text-align: center">&nbsp;</td>
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
    <asp:HiddenField ID="hdfRelation_ID" runat="server" />
</asp:Content>
