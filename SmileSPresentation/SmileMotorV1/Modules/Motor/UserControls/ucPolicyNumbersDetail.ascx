<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPolicyNumbersDetail.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucPolicyNumbersDetail" %>
<asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField HeaderText="" HeaderStyle-Width="1px">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>.
            </ItemTemplate>
            <HeaderStyle Width="1px" />
        </asp:TemplateField>
        <%--        <asp:BoundField DataField="MotorApplicationPolicy_ID" HeaderText="MotorApplicationPolicy_ID" />
        <asp:BoundField DataField="MotorApplication_ID" HeaderText="MotorApplication_ID" />
        <asp:BoundField DataField="PolicyType_ID" HeaderText="PolicyType_ID" />--%>
        <asp:BoundField DataField="MotorApplicationPolicyDetail" HeaderText="รายละเอียดเลขที่กรมธรรม์" />
        <asp:BoundField DataField="CreateBy_ID" HeaderText="สร้างโดย" />
        <asp:BoundField DataField="CreateDate" HeaderText="วันที่สร้าง" />
        <%--        <asp:BoundField DataField="IsActive" HeaderText="IsActive" />--%>
    </Columns>
</asp:GridView>