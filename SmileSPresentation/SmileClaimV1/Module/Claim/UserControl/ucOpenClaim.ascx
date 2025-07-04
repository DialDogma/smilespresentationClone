<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOpenClaim.ascx.cs" Inherits="SmileClaimV1.Module.Claim.UserControl.ucOpenClaim" %>
<%@ Register Src="~/CommonUserControl/ucDatepicker.ascx" TagPrefix="uc1" TagName="ucDatepicker" %>


<div class="container">
    <div class="panel panel-default">
        <div class="panel-heading input-md">
            <h4>ข้อมูลเบื้องต้น</h4>
        </div>
        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%; text-align: right">เบอร์โทรผู้ป่วย :</td>
                    <td style="width: 12.5%">
                        <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%; text-align: right">วันที่เข้ารักษา :</td>
                    <td style="width: 12.5%">
                        <uc1:ucDatepicker runat="server" ID="ucDatepicker" />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">ประเภทการเคลม :</td>
                    <td style="width: 12.5%">
                        <asp:DropDownList ID="ddlClaimType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClaimType_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td style="width: 12.5%; text-align: right">ประเภทการเข้ารักษา :</td>
                    <td style="width: 12.5%">
                        <asp:DropDownList ID="ddlClaimAdmitType" runat="server"></asp:DropDownList>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">อาการ (Chief Complain) :</td>
                    <td colspan="2">
                        <select id="selector"  runat="server" class="selectpicker" data-show-subtext="false" data-live-search="true"></select>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; text-align: right">รายละเอียดเพิ่มเติม :</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtMoreDetail" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>

            </table>
        </div>
    </div>
</div>
<%--<script>

    $(function () {
        $.ajax({
            type: "GET",
            url: "http://www.phiangphu.com/api/Provinces",
            dataType: "json",
            success: function (res) {
                //for (var i = 0; i < res.length; i++) {
                //    $('#selector').append('<option data-subtext="' + res[i].provinceId + '">' + res[i].provinceDetail + '</option>');
                //}
      
                    $('#selector').append('<option data-subtext="' + res[0].provinceId + '">' + res[0].provinceDetail + '</option>');
             
                console.log(res);
            }
        });

    })
</script>--%>