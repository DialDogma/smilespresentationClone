<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucApplicationOwner.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucApplicationOwner" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlOwnerType.ascx" TagPrefix="uc1" TagName="ddlOwnerType" %>
<%@ Register Src="~/Modules/Motor/UserControls/ucEmployeeSearch.ascx" TagPrefix="uc1" TagName="ucEmployeeSearch" %>
<%@ Register Src="~/Modules/Motor/UserControls/DropdownUserControls/ddlZebra.ascx" TagPrefix="uc1" TagName="ddlZebra" %>

<div class="panel panel-default">
    <div class="panel-heading">
        <h4 class="panel-title">ข้อมูลเจ้าของผลงาน</h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right;">ประเภทเจ้าของผลงาน :</td>
                    <td style="width: 12.5%;">
                        <uc1:ddlOwnerType runat="server" ID="ddlOwnerType" />
                    </td>
                    <td style="width: 75%;" colspan="4">
                        <uc1:ucEmployeeSearch ID="ucEmployeeSearch1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right;">หมายเลขรถม้าลาย :</td>
                    <td style="width: 12.5%;">
                        <uc1:ddlZebra runat="server" ID="ddlZebra" />
                        <%--<asp:DropDownList runat="server" ID="ddlZebra" />--%>
                    </td>
                    <td style="width: 75%;" colspan="4"></td>
                </tr>
            </table>
        </div>
    </div>
</div>