<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClaimHCIDetail.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucClaimHCIDetail" %>
<%@ Register Src="~/CommonUserControl/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>

<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>
                <asp:Label ID="lblTextHeader" runat="server" Text="รายละเอียดการเคลม"></asp:Label>
            </h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">
                        วันที่ - ถึงวันที่ :
                    </td>
                    <td style="width: 12.5%">
                        <uc1:ucDatepicker runat="server" ID="ucDateFrom" />
                    </td>
                    <td style="width: 12.5%">
                        <uc1:ucDatepicker runat="server" ID="ucDateTo" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">
                        ประเภทการเข้ารักษา :
                    </td>
                    <td style="width: 12.5%" colspan="2">
                        <asp:DropDownList ID="ddlCureType" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">
                        คำค้น :
                    </td>
                    <td style="width: 12.5%" colspan="2">
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    </td>
                    
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" SkinID="primary" OnClick="btnSearch_Click" />
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td colspan="2">
                        <asp:CheckBox ID="chkNotClaim" runat="server" Text="แสดงเฉพาะรายการที่ยังไม่ทำเคลม" SkinID="result" />
                    </td>
                    <td style="width: 12.5%; text-align: right;"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" OnRowDataBound="dgvMain_RowDataBound" AllowPaging="True" OnPageIndexChanging="dgvMain_PageIndexChanging"  >
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex +1 %>.
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ประเภท" DataField="ProductGroupDetail" />
                                <asp:HyperLinkField HeaderText="เลขที่การแจ้ง" DataTextField="HospitalClaimInformCode" DataNavigateUrlFields="HospitalClaimInformID" DataNavigateUrlFormatString="~/Module/Claim/09_DetailConfirmClaim.aspx?hospitalClaimInformID={0}" />
                                <asp:TemplateField HeaderText="เลขที่เคลม">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkNav" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="ชื่อ - สกุล ผู้เอาประกัน" DataField="FullName" />
                                <asp:BoundField HeaderText="วันที่แจ้งใช้สิทธิ์" DataField="CreatedDate"/>
                                <asp:BoundField HeaderText="สถานะ" DataField="HCIStatusDetail" />
                                <asp:BoundField HeaderText="โรงพยาบาล" DataField="OrganizeDetail" />
                                <asp:BoundField HeaderText="จังหวัด" DataField="ProvinceDetail" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>