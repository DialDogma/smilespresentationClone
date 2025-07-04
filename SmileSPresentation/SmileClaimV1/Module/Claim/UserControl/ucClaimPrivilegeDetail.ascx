<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimPrivilegeDetail.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucClaimPrivilegeDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="../../../CommonUserControl/ucDatepicker.ascx" TagName="ucDatepicker" TagPrefix="uc1" %>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="ข้อมูลการยืนยันใช้สิทธิ์"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">เลขที่การยืนยันใช้สิทธิ์ :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblPrivilegeNo" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;">สถานะ :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblStatus" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;">เลขที่เคลม :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblClaimNo" runat="server" SkinID="result"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">ชื่อสถานพยาบาล :
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <asp:Label ID="lblHospital" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">วันที่ทำรายการ :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblCreateDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;">ชื่อเจ้าหน้าที่ :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblCreateBy" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;">
                        <asp:Button ID="btnHistory" runat="server" Text="ประวัติการทำรายการ" SkinID="link" OnClick="btnHistory_Click" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalHistory" runat="server" BackgroundCssClass="modalBackground" PopupControlID="ModalPanelHistory" TargetControlID="btnHistory"></ajaxToolkit:ModalPopupExtender>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="Label1" runat="server" Text="รายละเอียด"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">ประเภทการเข้ารักษา :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblClaimAdmitType" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;">วันที่เข้ารักษา :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblClaimAdmitDate" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">ประเถทการเคลม :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblClaimType" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">อาการ (Chief Complain) :
                    </td>
                    <td style="width: 12.5%" colspan="4">
                        <asp:Label ID="lblChiefComplain" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">รายละเอียดเพิ่มเติม :
                    </td>
                    <td style="width: 12.5%" colspan="4">
                        <asp:Label ID="lblMoreDetail" runat="server" SkinID="result"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </div>
</div>

<asp:Panel ID="ModalPanelHistory" runat="server" Width="800" CssClass="modalPopup" align="center" style="display:none">
        <%-- TODO: Gridview --%>
    <h1>ทดสอบ ModalPopup</h1>
     <table style="width: 100%">
        <tr>
            <td style="text-align: right">
                <asp:Button ID="btnCloseModalHistory" runat="server" Text="ปิด" SkinID="danger" />
            </td>
        </tr>
    </table>
</asp:Panel>