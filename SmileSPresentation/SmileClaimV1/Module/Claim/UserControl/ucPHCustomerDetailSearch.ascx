<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPHCustomerDetailSearch.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucPHCustomerDetailSearch" %>
<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ค้นหา ประกันส่วนบุคคล</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right">คำค้นหา :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="ระบุคำค้นหา : ชื่อ,นามสกุล,เลขที่อ้างอิง,สถานะ ฯลฯ"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%">
                        <asp:Button ID="btnSearch" runat="server" SkinID="info" Text="ค้นหา" OnClick="btnSearch_Click"/>
                       
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" AllowCustomPaging="true" AllowPaging="true" OnPageIndexChanging="dgvMain_PageIndexChanging" OnRowDataBound="dgvMain_RowDataBound"   >
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 + ((dgvMain.PageIndex) * dgvMain.PageSize) %>.
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ApplicationID" HeaderText="เลขที่อ้างอิง" />
                                 <asp:BoundField DataField="FullName" HeaderText="ชื่อ สกุล" />
                                 <asp:BoundField DataField="Product" HeaderText="แผน" />
                                 <asp:BoundField DataField="Birthdate" HeaderText="วันเกิด" DataFormatString="{0:d}"  />
                                 <asp:BoundField DataField="ApplicationStatus" HeaderText="สถานะ" />
                                 <asp:BoundField DataField="StartCoverDate" HeaderText="วันที่เริ่มคุ้มครอง" DataFormatString="{0:d}" />
                                 <asp:BoundField DataField="CancelDate" HeaderText="วันที่ยกเลิก"  DataFormatString="{0:d}"  />
                                 <asp:HyperLinkField HeaderText="เรียกดู" Text="เรียกดู" DataNavigateUrlFields="ApplicationID" DataNavigateUrlFormatString="~/Module/Claim/04_Detail.aspx?appID={0}&pgroup=2" Target="_blank" /> <%-- ProductGroupID 2 = PH--%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
