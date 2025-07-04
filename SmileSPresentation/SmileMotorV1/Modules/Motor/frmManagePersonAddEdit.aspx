<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmManagePersonAddEdit.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmManagePersonAddEdit" EnableEventValidation="false" Theme="default" %>

<%@ Register Src="~/Modules/Motor/UserControls/ucPersonSearch.ascx" TagPrefix="uc1" TagName="ucPersonSearch" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPersonRelatedApplication.ascx" TagPrefix="uc1" TagName="ucAppDetailPersonRelatedApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditCustomer.ascx" TagPrefix="uc1" TagName="ucAddEditCustomer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditAddress.ascx" TagPrefix="uc1" TagName="ucAddEditAddress" %>
<%@ Register Src="~/CommonUserControls/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAddEditCorporate.ascx" TagPrefix="uc1" TagName="ucAddEditCorporate" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlPersonType.ascx" TagPrefix="uc1" TagName="ddlPersonType" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <uc1:ucAlert runat="server" ID="ucAlert" />
        <uc1:ucPersonSearch runat="server" ID="ucPersonSearch" OneSelectIndexPersonID="ucPersonSearch_eSelectIndexPersonID" />
        <uc1:ucAppDetailPersonRelatedApplication runat="server" ID="ucAppDetailPersonRelatedApplication" />

        <div class="panel panel-default">
            <div class="panel-heading input-md">
                <h4>
                    <asp:Label ID="lblTextHeader" runat="server" Text="ประเภทผู้เอาประกัน"></asp:Label>
                </h4>
            </div>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 8%"></td>
                        <td style="width: 8%; text-align: right">ประเภท :</td>
                        <td style="width: 17%">
                            <uc1:ddlPersonType runat="server" ID="ddlPersonType1" OneSelectChanged="ddlPersonType_eSelectChanged" AutoPostback="true" />
                        </td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                        <td style="width: 12.5%"></td>
                    </tr>
                </table>

            </div>
        </div>

        <uc1:ucAddEditCustomer runat="server" ID="ucAddEditCustomer" />
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
    <table style="width: 100%;">
        <tr>
            <td style="width: 12.5%;"></td>
            <td style="width: 12.5%;"></td>
            <td style="width: 12.5%;"></td>
            <td style="width: 12.5%;"></td>
            <td style="width: 12.5%;"></td>
            <td style="width: 12.5%;">
                <asp:Button ID="btnSave" runat="server" Text="บันทึกข้อมูล" OnClick="btnSave_Click" />
                <ajaxToolkit:ConfirmButtonExtender ID="btnSave_ConfirmButtonExtender" runat="server" BehaviorID="btnSave_ConfirmButtonExtender" ConfirmText="ยืนยันการบันทึกข้อมูล!" TargetControlID="btnSave" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdfPerson_ID" runat="server" />
    <asp:HiddenField ID="hdfApp_ID" runat="server" />
</asp:Content>
