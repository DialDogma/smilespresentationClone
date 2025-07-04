<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPASchoolDetailSearch.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPASchoolDetailSearch" %>
<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ค้นหา โครงการโรงเรียนห่วงใย</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right">ปีการศึกษา :</td>
                    <td style="width: 12.5%">
                        <asp:DropDownList ID="ddlSchoolYear" runat="server">
                            <asp:ListItem Value="2560">2560</asp:ListItem>
                            <asp:ListItem Value="2559">2559</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">ชื่อโรงเรียน :</td>
                    <td style="width: 12.5%">
                        <asp:TextBox ID="txtSchoolNameSearch" runat="server" placeholder="ชื่อโรงเรียน"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearch" runat="server" SkinID="info" Text="ค้นหา" OnClick="btnSearch_Click" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="dgvSchool" runat="server" AutoGenerateColumns="False" AllowCustomPaging="true" AllowPaging="true"  OnPageIndexChanging="dgvSchool_PageIndexChanging" OnRowDataBound="dgvMain_RowDataBound"  >
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 + ((dgvSchool.PageIndex) * dgvSchool.PageSize) %>.
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="เลขที่ Application" DataField="ApplicationID" />
                                <asp:BoundField HeaderText="ชื่อโรงเรียน" DataField="School" />
                                <asp:BoundField HeaderText="จังหวัด" />
                                <asp:BoundField HeaderText="อำเภอ" />
                                <asp:BoundField HeaderText="ตำบล" />
                                <asp:BoundField HeaderText="สถานะ" DataField="ApplicationStatus" />
                                <asp:BoundField HeaderText="วันที่คุ้มครอง" DataField="StartCoverDate" DataFormatString="{0:d}"/>
                                <asp:BoundField HeaderText="วันที่สิ้นสุด" DataField="EndCoverDate" DataFormatString="{0:d}" />
                                <asp:HyperLinkField Text="เรียกดู" DataNavigateUrlFields="ApplicationID" DataNavigateUrlFormatString="~/Module/Claim/03_SearchPACustomer.aspx?appID={0}&pgroup=3" Target="_blank" /> <%-- ProductGroupID 3 = PA --%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>


