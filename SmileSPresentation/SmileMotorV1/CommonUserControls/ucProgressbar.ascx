<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProgressbar.ascx.cs" Inherits="SmileMotorV1.CommonUserControls.ucProgressbar_ascx" %>
<div class="container">
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="progress">
                <div class="progress-bar progress-bar-striped active" role="progressbar"
                    aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">
                    กำลังโหลดข้อมูลกรุณารอซักครู่...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="float: right;">
        <asp:Label ID="lblProgressTime" runat="server" SkinID="mini"></asp:Label>
    </div>
</div>
