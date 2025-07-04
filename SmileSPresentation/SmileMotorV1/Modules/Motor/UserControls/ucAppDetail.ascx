<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAppDetail.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucAppDetail" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucBenefit.ascx" TagPrefix="uc1" TagName="ucBenefit" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailApplication.ascx" TagPrefix="uc1" TagName="ucAppDetailApplication" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailCustomer.ascx" TagPrefix="uc1" TagName="ucAppDetailCustomer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailAddress.ascx" TagPrefix="uc1" TagName="ucAppDetailAddress" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailVehicle.ascx" TagPrefix="uc1" TagName="ucAppDetailVehicle" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPayer.ascx" TagPrefix="uc1" TagName="ucAppDetailPayer" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailPayMethod.ascx" TagPrefix="uc1" TagName="ucAppDetailPayMethod" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailOwner.ascx" TagPrefix="uc1" TagName="ucAppDetailOwner" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucDocument.ascx" TagPrefix="uc1" TagName="ucDocument" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucApplicationMemo.ascx" TagPrefix="uc1" TagName="ucApplicationMemo" %>
<%@ Register Src="../../../CommonUserControls/ucProgressbar.ascx" TagName="ucProgressbar" TagPrefix="uc2" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailDept.ascx" TagPrefix="uc1" TagName="ucAppDetailDept" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailCorporate.ascx" TagPrefix="uc1" TagName="ucAppDetailCorporate" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucAppDetailAppReNew.ascx" TagPrefix="uc1" TagName="ucAppDetailAppReNew" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="divList" class="col-md-2 list-group divTop">
            <ul>
                <li><a class="list-group-item" href="#AppDetailOwner"><i class="glyphicon glyphicon-triangle-right"></i>เจ้าของผลงาน</a>

                    <a class="list-group-item" href="#AppDetailApplication"><i class="glyphicon glyphicon-triangle-right"></i>ข้อมูลกรมธรรม์</a>

                    <a class="list-group-item" href="#AppDetailAppReNew"><i class="glyphicon glyphicon-triangle-right"></i>ประวัติการต่ออายุ</a>

                    <a class="list-group-item" href="#AppDetailCustomer"><i class="glyphicon glyphicon-triangle-right"></i>ข้อมูลผู้เอาประกัน</a>

                    <a class="list-group-item" href="#Vehicle"><i class="glyphicon glyphicon-triangle-right"></i>ข้อมูลรถ</a>

                    <a class="list-group-item" href="#AppDetailPayer"><i class="glyphicon glyphicon-triangle-right"></i>ข้อมูลผู้ชำระเบี้ย</a>

                    <a class="list-group-item" href="#AppDetailPayMethod"><i class="glyphicon glyphicon-triangle-right"></i>วิธีชำระเงิน</a>

                    <a class="list-group-item" href="#AppDetailDept"><i class="glyphicon glyphicon-triangle-right"></i>ข้อมูลการชำระเงิน</a>

                    <a class="list-group-item" href="#Document"><i class="glyphicon glyphicon-triangle-right"></i>เอกสาร</a>

                    <a class="list-group-item" href="#ApplicationMemo"><i class="glyphicon glyphicon-triangle-right"></i>บันทึกข้อความ</a>
                </li>
            </ul>
        </div>
        <table style="width: 100%;">
            <tr>
                <td style="width: 80%; text-align: right;">Application :
            <asp:Label ID="lblAppCode" runat="server" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                    <asp:LinkButton ID="btnPrint" CssClass="btn btn-success" runat="server" Visible="False" Width="200px" OnClick="btnPrint_OnClick"><i class="glyphicon glyphicon-print"></i> พิมพ์หนังสือขอบคุณ</asp:LinkButton>
                    <asp:HiddenField runat="server" ID="hdfMotorAppID" />
                </td>
                <%--    <td style="width: 10%; text-align: right;">
                </td>--%>
            </tr>
            <tr id="TRCoverNoteManual" runat="server" visible="false">
                <td style="text-align: right;">
                    <asp:HyperLink ID="hplCoverNoteManual" runat="server" NavigateUrl="~/Manual/คู่มือการพิมพ์หนังสือขอบคุณ.pdf" Target="_blank">คู่มือการตั้งค่าการพิมพ์หนังสือขอบคุณ / </asp:HyperLink>
                    <asp:HyperLink ID="hplCoverNoteManualPart2" runat="server" NavigateUrl="~/Manual/เอกสารแนบท้ายประกันภัยรถยนต์MP2S.pdf" Target="_blank">เอกสารแนบท้ายประกันภัยรถยนต์MP2S</asp:HyperLink></td>
            </tr>
        </table>
        <%--<uc2:ucProgressbar ID="ucProgressbar1" runat="server" />--%>
        <div id="tab1" class="container">
            <div class="tab-content">
                <div id="Main" class="tab-pane fade in active">
                    <div class="panel-group">
                        <div class="panel-body">
                            <div class="panel panel-default" id="AppDetailOwner">
                                <uc1:ucAppDetailOwner runat="server" ID="ucAppDetailOwner" />
                            </div>
                            <div class="panel panel-default" id="AppDetailApplication">
                                <uc1:ucAppDetailApplication runat="server" ID="ucAppDetailApplication" />
                            </div>
                            <div class="panel panel-default" id="AppDetailAppReNew">
                                <uc1:ucAppDetailAppReNew runat="server" ID="ucAppDetailAppReNew" />
                            </div>
                            <div class="panel panel-default" id="AppDetailCustomer">
                                <uc1:ucAppDetailCustomer runat="server" ID="ucAppDetailCustomer" />
                            </div>
                            <div class="panel panel-default" id="AppDetailAddressCustome">
                                <uc1:ucAppDetailAddress runat="server" ID="ucAppDetailAddressCustomer" />
                            </div>
                            <div class="panel panel-default" id="Vehicle">
                                <uc1:ucAppDetailVehicle runat="server" ID="ucAppDetailVehicle" />
                            </div>
                            <div class="panel panel-default" id="AppDetailPayer">
                                <uc1:ucAppDetailPayer runat="server" ID="ucAppDetailPayer" />
                            </div>
                            <div class="panel panel-default" id="AppDetailAddressPayer">
                                <uc1:ucAppDetailAddress runat="server" ID="ucAppDetailAddressPayer" />
                            </div>
                            <div class="panel panel-default" id="AppDetailPayMethod">
                                <uc1:ucAppDetailPayMethod runat="server" ID="ucAppDetailPayMethod" />
                            </div>
                            <div class="panel panel-default" id="Document">
                                <uc1:ucDocument runat="server" ID="ucDocument" />
                            </div>
                            <div class="panel panel-default" id="ApplicationMemo">
                                <uc1:ucApplicationMemo runat="server" ID="ucApplicationMemo" />
                            </div>
                            <div class="panel panel-default" id="AppDetailDept">
                                <uc1:ucAppDetailDept runat="server" ID="ucAppDetailDept" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>