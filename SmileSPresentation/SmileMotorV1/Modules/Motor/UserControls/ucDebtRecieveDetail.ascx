<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDebtRecieveDetail.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucDebtRecieveDetail" %>
<script src="../../../Scripts/sss.js"></script>
<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">รายละเอียดรายการ</h4>
    </div>
    <div class="panel-body">
        <asp:GridView ID="dgvMain" runat="server" 
            AutoGenerateColumns="false" 
            OnRowDataBound="dgvMain_RowDataBound" 
            OnRowCommand="dgvMain_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="ลำดับการรับหนี้" DataField="DebtReceive_ID" />
                <asp:BoundField HeaderText="ลำดับการตั้งหนี้" DataField="Debt_ID" />
                <asp:BoundField HeaderText="เบี้ยรับ" DataField="Premium_Receive" DataFormatString="{0:###,###.00}" />
                <asp:BoundField HeaderText="Math Header" DataField="MatchHeader_ID" />
                <asp:BoundField HeaderText="วิธีการชำระ" DataField="PayMethod_ID" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnLinkOpen" runat="server" Text="Cash Recieve" SkinID="warning" CommandName="Open" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:HiddenField ID="hdfDebt_ID" runat="server" />
</div>


