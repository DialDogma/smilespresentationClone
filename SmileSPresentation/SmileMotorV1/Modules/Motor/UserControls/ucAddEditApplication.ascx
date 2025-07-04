<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditApplication.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditApplication" %>
<%@ Register Src="DropdownUserControls/ddlMotorProductGroup.ascx" TagName="ddlMotorProductGroup" TagPrefix="uc3" %>
<%@ Register Src="DropdownUserControls/ddlMotorProductType.ascx" TagName="ddlMotorProductType" TagPrefix="uc5" %>
<%@ Register Src="DropdownUserControls/ddlMotorProduct.ascx" TagName="ddlMotorProduct" TagPrefix="uc6" %>
<%@ Register Src="DropdownUserControls/ddlMotorCompanyInsurance.ascx" TagName="ddlMotorCompanyInsurance" TagPrefix="uc7" %>

<%@ Register Src="DropdownUserControls/ddlMotorClaimType.ascx" TagName="ddlMotorClaimType" TagPrefix="uc2" %>

<%@ Register Src="DropdownUserControls/ddlCompany.ascx" TagName="ddlCompany" TagPrefix="uc8" %>

<%@ Register Src="DropdownUserControls/ddlContractType.ascx" TagName="ddlContractType" TagPrefix="uc9" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMotorProduct.ascx" TagPrefix="uc1" TagName="ddlMotorProduct" %>
<%@ Register Src="~/CommonUserControls/ucDatepicker-DisablePastDates.ascx" TagPrefix="uc1" TagName="ucDatepickerDisablePastDates" %>
<!--comment 27-2-2562 ยังไม่ใช้งาน-->
<!--<%@ Register Src="~/CommonUserControls/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>-->
<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">ข้อมูลกรมธรรม์</h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">
            <asp:UpdatePanel ID="Up1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">เลขที่อ้างอิงกรมธรรม์ :</td>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:TextBox ID="txtMotorAppCode" runat="server" placeholder="เลขที่อ้างอิงกรมธรรม์"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                            <td style="width: 12.5%;">&nbsp;</td>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                            <td style="width: 12.5%; text-align: right;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">ประเภทสัญญา :</td>
                            <td style="width: 12.5%;">
                                <uc9:ddlContractType ID="ddlContractType1" runat="server" OneContractTypeSelectChanged="ddlContractType1_eContractTypeSelectChanged" AutoPostback="true" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ประเภทการซ่อม :</td>
                            <td style="width: 12.5%;">

                                <uc2:ddlMotorClaimType ID="ddlMotorClaimType1" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เริ่มคุ้มครอง :</td>
                            <td style="width: 12.5%;">
                                <uc1:ucDatepickerDisablePastDates ID="dtpStartCoverDate" runat="server" />
                                <%-- <uc1:ucDatepicker ID="dtpStartCoverDate" runat="server" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">บริษัทผู้รับประกัน :</td>
                            <td style="width: 12.5%; text-align: left;">
                                <uc7:ddlMotorCompanyInsurance ID="ddlMotorCompanyInsurance" runat="server" OnSelectChanged="ddlMotorCompanyInsurance_SelectChanged" AutoPostback="True" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ประเภทผลิตภัณฑ์ :</td>
                            <td style="width: 12.5%;">
                                <uc5:ddlMotorProductType ID="ddlMotorProductType" runat="server" OnSelectChanged="ddlMotorProductType_SelectChanged" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ผลิตภัณฑ์ :</td>
                            <td style="width: 12.5%;">
                                <uc1:ddlMotorProduct runat="server" ID="ddlMotorProduct" OnSelectChanged="ddlMotorProduct_SelectChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right">ผู้รับผลประโยชน์ :</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtHeir" runat="server"></asp:TextBox>
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>