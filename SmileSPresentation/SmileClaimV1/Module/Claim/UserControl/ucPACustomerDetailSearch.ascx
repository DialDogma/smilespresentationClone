<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPACustomerDetailSearch.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPACustomerDetailSearch" %>
<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ค้นหา โครงการโรงเรียนห่วงใย</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align:right;">
                        ข้อมูลโรงเรียน :
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblApplicationDetail" runat="server"  SkinID="result" ></asp:Label>
                    </td>
                    <td style="width: 12.5%; text-align:right;">
                        แผน :
                    </td>
                    <td style="width: 12.5%">
                        <asp:Label ID="lblProduct" runat="server"  SkinID="result" ></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">คำค้นหา :</td>
                    <td style="width: 12.5%">
                        <asp:TextBox ID="txtStudentName" runat="server" placeholder="ค้นโดย ชื่อ หรือ นามสกุล"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearchStudent" runat="server" Text="ค้นหา" SkinID="info" OnClick="btnSearchStudent_Click" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="dgvMain_PageIndexChanging" AllowCustomPaging="true" AllowPaging="true">
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 + ((dgvMain.PageIndex) * dgvMain.PageSize) %>.
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CustomerDetailID" HeaderText="เลขที่อ้างอิง" />
                                <asp:BoundField DataField="FullName" HeaderText="ชื่อ สกุล" />
                                <asp:BoundField DataField="CustomerType" HeaderText="ประเภท	" />
                                <asp:BoundField DataField="CustomerStatus" HeaderText="สถานะ" />
                                <asp:BoundField DataField="StartCoverDate" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="EndCoverDate" HeaderText="วันที่สิ้นสุด" DataFormatString="{0:d}" />
                                <asp:HyperLinkField Text="เรียกดู" DataNavigateUrlFields="ApplicationID,CustomerDetailID" DataNavigateUrlFormatString="~/Module/Claim/04_Detail.aspx?appID={0}&cusID={1}&pgroup=3" Target="_blank" />  <%-- ProductGroupID 3 = PA --%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdfApplicationID" runat="server" />
        </div>
    </div>
</div>
