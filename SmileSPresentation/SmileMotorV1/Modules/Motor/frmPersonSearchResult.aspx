<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPersonSearchResult.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmPersonSearchResult" EnableEventValidation="false" Theme="default" %>

<link href="../../Content/bootstrap.css" rel="stylesheet" />
<link href="../../Content/meStyleSheet.css" rel="stylesheet" />
<script type="text/javascript" src="../../Content/meJavaScript.js"></script>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<script type="text/javascript">

    function ReturnValue(result) {
        //Return value 
        window.opener.document.getElementById(getParameterByName('ReturnTo')).value = result;
        //force parent postback
        window.opener.__doPostBack();
        //Close browser
        window.close();
    }


</script>

<body>
    <form id="form1" runat="server">

        <div class="container">
            <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false " OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="ประเภทบัตร" DataField="PersonCardTypeDetail" />
                    <asp:BoundField HeaderText="ข้อมูลบัตร" DataField="PersonCardDetail" />
                    <asp:BoundField HeaderText="ชื่อ - นามสกุล" DataField="FullName" />
                    <asp:BoundField HeaderText="ประเภทอาชีพ" DataField="OccupationDetail" />
                    <asp:BoundField HeaderText="ระดับอาชีพ" DataField="OccupationLevelDetail" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
