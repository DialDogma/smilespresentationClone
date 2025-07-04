<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchConfirmClaimPrivilege.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucSearchConfirmClaimPrivilege" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/CommonUserControl/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>
<%@ Register Src="~/CommonUserControl/ucAlert.ascx" TagPrefix="uc1" TagName="ucAlert" %>

        <uc1:ucAlert runat="server" id="ucAlert" />
        <div class="container">
            <div class="panel panel-default">
                <div class="panel-heading input-md">
                    <h4>
                        <asp:Label ID="lblTextHeader" runat="server" Text="ค้นหาการแจ้งยืนยันใช้สิทธิ์"></asp:Label>
                    </h4>
                </div>
                <div class="panel-body">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 12.5%; text-align: right;">วันที่แจ้ง - ถึงวันที่ :
                            </td>
                            <td style="width: 12.5%">
                                <uc1:ucDatepicker runat="server" ID="ucDateFrom" />
                            </td>
                            <td style="width: 12.5%">
                                <uc1:ucDatepicker runat="server" ID="ucDateTo" />
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">คำค้น :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                            </td>

                            <td style="width: 12.5%">
                                <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="primary" OnClick="btnSearch_Click" />
                            </td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%"></td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:GridView ID="dgvMain" runat="server"
                                    AutoGenerateColumns="False"
                                    OnSelectedIndexChanged="dgvMain_SelectedIndexChanged"
                                    DataKeyNames="HospitalClaimInformID"
                                    OnPageIndexChanging="dgvMain_PageIndexChanging" 
                                    OnRowDataBound="dgvMain_RowDataBound"   
                                    AllowPaging="True" AllowCustomPaging="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex +1 + ((dgvMain.PageIndex) * dgvMain.PageSize) %>.
                                            </ItemTemplate>

                                            <HeaderStyle Width="1px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowSelectButton="True" />
                                        <asp:BoundField HeaderText="เลขที่การแจ้ง" DataField="HospitalClaimInformCode" />
                                        <asp:BoundField HeaderText="เลขที่อ้างอิง" DataField="ClaimHeaderID" />
                                        <asp:BoundField HeaderText="ชื่อ - สกุล" DataField="FullName" />
                                        <asp:BoundField HeaderText="ประเภทการเข้ารักษา" DataField="ClaimAdmitTypeDetail" />
                                        <asp:BoundField HeaderText="ประเภทเคลม" DataField="ClaimTypeDetail" />
                                        <%--<asp:BoundField HeaderText="อาการ" DataField="ChiefComplainDetail" />--%>
                                        <asp:BoundField HeaderText="วันที่เกิดเหตุ" DataField="DateHappen" DataFormatString="{0:d}" />
                                        <asp:BoundField HeaderText="วันที่แจ้งใช้สิทธิ์" DataField="CreatedDate" DataFormatString="{0:d}" />
                                        <asp:BoundField HeaderText="สถานะ" DataField="HCIStatusDetail" />
                                        <asp:HyperLinkField Text="เรียกดู" DataNavigateUrlFields="HospitalClaimInformID" DataNavigateUrlFormatString="~/Module/Claim/09_DetailConfirmClaim.aspx?hospitalClaimInformID={0}" Target="_blank" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 12.5%; text-align: right;">
                                <asp:HiddenField ID="hdfHospitalClaimInformID" runat="server" />
                            </td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%"></td>
                            <td style="width: 12.5%; text-align: right;"></td>
                            <td style="width: 12.5%">
                                <asp:Button ID="btnCancelPrivilege" runat="server" Text="ยกเลิกการใช้สิทธิ์" SkinID="danger" OnClick="btnCancelPrivilege_Click" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnCancelPrivilege" BackgroundCssClass="modalBackground" PopupControlID="ModalPanelConfirmCancelPrivileg"></ajaxToolkit:ModalPopupExtender>
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


<asp:Panel ID="ModalPanelConfirmCancelPrivileg" Width="800" runat="server" CssClass="modalPopup" align="center" Style="display: contents">

    <table style="width: 100%">
        <tr>
            <td style="width: 12.5%; text-align: right;">สาเหตุการยกเลิก :
            </td>
            <td style="width: 12.5%;" colspan="2">
                <asp:DropDownList ID="ddlRemarkCancel" runat="server"></asp:DropDownList>
            </td>
            <td style="width: 12.5%;"></td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align: right">
                <asp:Button ID="btnSumitCancel" runat="server" Text="ยืนยันการยกเลิก" SkinID="success" OnClick="btnSumitCancel_Click" />
            </td>
            <td style="text-align: right">
                <asp:Button ID="btnCloseModal" runat="server" Text="กลับสู่หน้าหลัก" SkinID="danger" />
            </td>
            <td></td>
        </tr>
    </table>

</asp:Panel>



