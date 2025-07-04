<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBenefit.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucBenefit" %>

<asp:GridView ID="dgvMain" runat="server" Width="100%" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="BenefitDetail" HeaderText="สิทธิประโยชน์" ItemStyle-HorizontalAlign="Left">
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Coverage" HeaderText="ความคุ้มครอง" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}">
            <HeaderStyle Width="20%"></HeaderStyle>
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
    </Columns>
</asp:GridView>
