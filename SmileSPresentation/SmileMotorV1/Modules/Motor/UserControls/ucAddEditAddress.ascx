<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAddEditAddress.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAddEditAddress" %>

<%@ Register Src="DropdownUserControls/ddlProvince.ascx" TagName="ddlProvince" TagPrefix="uc1" %>

<%@ Register Src="DropdownUserControls/ddlDistrict.ascx" TagName="ddlDistrict" TagPrefix="uc2" %>
<%@ Register Src="DropdownUserControls/ddlSubDistrict.ascx" TagName="ddlSubDistrict" TagPrefix="uc4" %>

<style type="text/css">
    .auto-style1 {
        width: 241px;
    }

    .auto-style2 {
        width: 161px;
    }
    .form-control {}
</style>

<div class="table-responsive">

    <asp:UpdatePanel ID ="updatepanel00" runat ="server">
        <ContentTemplate>
            <table style="width :100%">
                <tr>
                    <td style="width:15.5%">
                        <asp:HiddenField ID="hdfAddressID" runat="server" />
                    </td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:6.5%"></td>
                </tr>
                <tr>
                    <td style="width:15.5% ; text-align:right">ชื่อหน่วยงาน :</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtBuildingName" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลอาคาร" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:8.5% ; text-align:right">หมู่บ้าน :</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtVillageName" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลชื่อหมู่บ้าน" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:6.5%"></td>
                </tr>
                <tr>
                    <td style="width:15.5% ; text-align:right">เลขที่ :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtNo" runat="server" MaxLength="20" ToolTip="กรอกข้อมูลบ้านเลขที่" Width="100%" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td style="width:8.5% ; text-align:right">หมู่ที่ :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtMoo" runat="server" MaxLength="20" ToolTip="กรอกข้อมูลหมู่ที่" Width="100%" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td style="width:8.5% ; text-align:right">ชั้น :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtFloor" runat="server" MaxLength="255" ToolTip="กรอกข้อมูลชั้น" Width="100%" CssClass="form-control"></asp:TextBox>
                    </td>
                    <td style="width:8.5% ; text-align:right">ห้อง :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtRoom" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลห้อง" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:6.5%"></td>
                </tr>
                <tr>
                    <td style="width:15.5%; text-align:right">ซอย :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtSoi" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลซอย" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:8.5% ; text-align:right">ถนน :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtRoad" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลถนน" Width="100%"></asp:TextBox>
                    </td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:6.5%"></td>
                </tr>
                <tr>
                    <td style="width:15.5% ; text-align:right">จังหวัด :</td>
                    <td style="width:12.5%">
                        <uc1:ddlProvince ID="ddlProvince1" runat="server" OnSelectChanged="ddlProvince1_SelectChanged1" />
                    </td>
                    <td style="width:8.5% ;text-align :right">อำเภอ :</td>
                    <td style="width:12.5%">
                        <uc2:ddlDistrict ID="ddlDistrict" runat="server" OnSelectChanged="ddlDistrict_SelectChanged" />
                    </td>
                    <td style="width:8.5% ; text-align:right">ตำบล :</td>
                    <td style="width:12.5%">
                        <uc4:ddlSubDistrict ID="ddlSubDistrict1" runat="server" OnSelectChanged="ddlSubDistrict1_SelectChanged" />
                    </td>
                    <td style="width:8.5% ; text-align:right">รหัสไปรษณีย์ :</td>
                    <td style="width:12.5%">
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" MaxLength="255" ToolTip="กรอกข้อมูลรหัสไปรษณีย์" Width="100%" Enabled="False"></asp:TextBox>
                    </td>
                    <td style="width:6.5%"></td>
                </tr>
                <tr>
                    <td style="width:15.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:8.5%"></td>
                    <td style="width:12.5%"></td>
                    <td style="width:6.5%"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>



</div>
