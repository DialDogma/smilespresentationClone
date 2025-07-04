<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmOwnerSearchResult.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmOwnerSearchResult" EnableEventValidation="false" Theme="default" %>

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
        //window.close();
    }
</script>


<body>
    <form id="form1" runat="server">
        <table style="width:100%">
            <tr>
                <td style="width:25%; text-align:right;">
                    <asp:Label ID="lblTitle" runat="server" Text="คำค้นหา :" CssClass="h4"></asp:Label>
                </td>
                <td style="width:25%;">
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                </td>
                <td style="width:25%;">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"/>
                </td>
                <td style="width:25%;">

                </td>
            </tr>
        </table>                              
        <div class="container">
            <asp:GridView ID="dgvMain" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="dgvMain_SelectedIndexChanged" OnRowDataBound="OnRowDataBound">
                <Columns>
                    <asp:BoundField DataField="EmployeeCode" HeaderText="รหัสพนักงาน" />
                    <asp:BoundField DataField="FirstName" HeaderText="ชื่อ" />
                    <asp:BoundField DataField="LastName" HeaderText="สกุล" />
                    <asp:BoundField DataField="NickName" HeaderText="ชื่อเล่น" />
                    <asp:BoundField DataField="BranchDetail" HeaderText="สาขา" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
