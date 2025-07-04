<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SmileClaimV1.Module.Claim.WebForm1" Theme="default" %>

<%@ Register Src="~/Module/Claim/UserControl/ucOpenClaim.ascx" TagPrefix="uc1" TagName="ucOpenClaim" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/bootstrap-select.min.css" rel="stylesheet" />
<%--    <script src="../../js/jquery-3.1.1.min.js"></script>--%>
    <%--<script src="../../js/bootstrap.min.js"></script>--%>
    <%--    <script src="../../js/bootstrap-select.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucOpenClaim runat="server" ID="ucOpenClaim" />

    <div>
        <select id="selector" runat="server" class="selectpicker" data-show-subtext="true" data-live-search="true">
            <%--<option data-subtext="Rep California">Tom</option>
                <option data-subtext="Rep California">Jade</option>
                <option data-subtext="Rep California">Kae</option>
                <option data-subtext="Rep California">Mark</option>--%>
        </select>
        <asp:Button ID="bbbb" runat="server" OnClick="bbbb_Click" />
        <input type="button" id="btnSubmit" />
    </div>
    <script src="../../js/jquery-1.9.1.min.js"></script>

    <script>
       
        function GetProvinces() {
            $.ajax({
                type: "GET",
                url: "http://www.phiangphu.com/api/Provinces",
                dataType: "json",
                success: function (res) {
                     selectorid = document.getElementById('<%=selector.ClientID%>');
                    for (var i = 0; i < res.length; i++) {
                        $(selectorid).append('<option data-subtext="' + res[i].provinceId + '">' + res[i].provinceDetail + '</option>');
                    }

                    console.log(res);
                }
            });
        }

        GetProvinces();

        selectid=document.getElementById('<%=selector.ClientID%>')
        $(selectid).change(function (e) {

            e.preventDefault();
            console.log($(selectorid).val());

        })

    </script>
</asp:Content>


