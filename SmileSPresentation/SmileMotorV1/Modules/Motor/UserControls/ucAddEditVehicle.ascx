<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditVehicle.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditVehicle" %>
<%@ Register Src="../../../CommonUserControls/ucUppercaseTextbox.ascx" TagName="ucUppercaseTextbox" TagPrefix="uc5" %>

<%@ Register Src="DropdownUserControls/ddlMotorVehicleBrand.ascx" TagName="ddlMotorVehicleBrand" TagPrefix="uc4" %>
<%@ Register Src="DropdownUserControls/ddlMotorVehicleType.ascx" TagName="ddlMotorVehicleType" TagPrefix="uc7" %>
<%@ Register Src="DropdownUserControls/ddlMotorVehicleSegment.ascx" TagName="ddlMotorVehicleSegment" TagPrefix="uc8" %>
<%@ Register Src="DropdownUserControls/ddlMotorVehicleModel.ascx" TagName="ddlMotorVehicleModel" TagPrefix="uc9" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlProvince.ascx" TagPrefix="uc1" TagName="ddlProvince" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlMotorVehicleSegment.ascx" TagPrefix="uc1" TagName="ddlMotorVehicleSegment" %>

<%@ Register Src="DropdownUserControls/ddlVehicleYear.ascx" TagName="ddlVehicleYear" TagPrefix="uc3" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlFuelType.ascx" TagPrefix="uc1" TagName="ddlFuelType" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">ข้อมูลรถ</h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">

            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">ยี่ห้อรถยนต์ :</td>
                            <td style="width: 12.5%">

                                <uc4:ddlMotorVehicleBrand ID="ddlMotorVehicleBrand" runat="server" OnSelectChanged="ddlMotorVehicleBrand_SelectChanged" AutoPostback="True" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ประเภทรถ :</td>
                            <td style="width: 12.5%;">

                                <uc7:ddlMotorVehicleType ID="ddlMotorVehicleType" runat="server" OnSelectChanged="ddlMotorVehicleType_SelectChanged" AutoPostback="True" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ประเภทย่อย :</td>
                            <td style="width: 12.5%;">
                                <uc1:ddlMotorVehicleSegment runat="server" ID="ddlMotorVehicleSegment" OnSelectChanged="ddlMotorVehicleSegment_SelectChanged" AutoPostback="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">รุ่นรถ :</td>
                            <td style="width: 12.5%">
                                <uc9:ddlMotorVehicleModel ID="ddlMotorVehicleModel" runat="server" AutoPostback="False" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ขนาดเครื่องยนต์ :</td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtVehicleCC" runat="server" placeHolder="ขนาดเครื่องยนต์ cc"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtVehicleCC_FilteredTextBoxExtender" runat="server" BehaviorID="txtVehicleCC_FilteredTextBoxExtender" TargetControlID="txtVehicleCC" FilterType="Numbers" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">น้ำหนักรถ(kg) :</td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtWeightVehicle" runat="server" placeHolder="น้ำหนักรถ"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtWeightVehicle_FilteredTextBoxExtender" runat="server" BehaviorID="txtWeightVehicle_FilteredTextBoxExtender" TargetControlID="txtWeightVehicle" FilterType="Numbers" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">ชนิดเชื้อเพลิง :</td>
                            <td style="width: 12.5%">
                                <uc1:ddlFuelType runat="server" ID="ddlFuelType" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">เลขทะเบียน :</td>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtRegistrationDetail" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="width: 70%">
                                            <asp:TextBox ID="txtRegistrationNumber" runat="server"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtRegistrationNumber_FilteredTextBoxExtender" runat="server" BehaviorID="txtRegistrationNumber_FilteredTextBoxExtender" FilterType="Numbers" TargetControlID="txtRegistrationNumber" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 12.5%; text-align: right;">จังหวัด :</td>
                            <td style="width: 12.5%;">
                                <uc1:ddlProvince ID="ddlMotorProvince" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">ปีที่จดทะเบียน :</td>
                            <td style="width: 12.5%">
                                <uc3:ddlVehicleYear ID="ddlVehicleYear" runat="server" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">จำนวนที่นั่ง :</td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtVehicleSeat" runat="server" placeHolder="จำนวนที่นั่ง"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtVehicleSeat_FilteredTextBoxExtender" runat="server" BehaviorID="txtVehicleSeat_FilteredTextBoxExtender" FilterType="Numbers" TargetControlID="txtVehicleSeat" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">ราคารถ :</td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtVehiclePrice" runat="server" placeHolder="ราคา" Text="0.00"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtVehiclePrice_FilteredTextBoxExtender" runat="server" BehaviorID="txtVehiclePrice_FilteredTextBoxExtender" FilterType="Numbers,Custom" TargetControlID="txtVehiclePrice" ValidChars="." />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">เลขเครื่องยนต์ :</td>
                            <td style="width: 12.5%;">
                                <asp:TextBox ID="txtEngineNo" runat="server" CssClass="form-control input-sm" Width="100%" placeHolder="เลขเครื่องยนต์" CharacterCasing="Upper"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtEngineNo_FilteredTextBoxExtender" runat="server" BehaviorID="txtEngineNo_FilteredTextBoxExtender" TargetControlID="txtEngineNo" FilterType="UppercaseLetters,LowercaseLetters,Numbers" />
                            </td>
                            <td style="width: 12.5%; text-align: right;">หมายเลขตัวถัง :</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtChassis" runat="server" CssClass="form-control input-sm" Width="100%" placeHolder="หมายเลขตัวถัง" CharacterCasing="Upper"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtChassis_FilteredTextBoxExtender" runat="server" BehaviorID="txtChassis_FilteredTextBoxExtender" TargetControlID="txtChassis" FilterType="UppercaseLetters,LowercaseLetters,Numbers" />
                            </td>
                            <td style="width: 12.5%;"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%;"></td>
                            <td style="width: 12.5%;">&nbsp;</td>
                            <td style="width: 12.5%;">&nbsp;</td>
                            <td style="width: 12.5%;">&nbsp;</td>
                            <td style="width: 12.5%;">&nbsp;</td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>