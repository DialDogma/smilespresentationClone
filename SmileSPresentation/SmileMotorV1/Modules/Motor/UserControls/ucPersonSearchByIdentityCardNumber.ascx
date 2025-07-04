<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPersonSearchByIdentityCardNumber.ascx.cs" Inherits="SmileMotorV1.Modules.Motor.UserControls.ucPersonSearchByIdentityNumber" %>
<script src="../../../Scripts/sss.js"></script>
<script type="text/javascript">

    //BOOM :
    function OpenPopup(pagename, resultClientID) {
        //  ใส่ Script window.open เพื่อให้ Popup.asp เปิดขึ้นมา
        // ต้องใช้ ClientID เนื่องจากเป็น User Control (ถ้าไม่ใช่ User Control ใช้ ID ตรง ๆ ก็ได้)

        // query string ที่จะส่งไป
        var txtValue = '<%= txtCustomerIDNumber.ClientID%>';

         var txtPersonTypeID = '<%= txtPersonTypeID.ClientID%>';

        // Control ที่จะรับ result
<%--        var resultClientID = '<%= txtResultPostback1.ClientID %>';--%>

        // รวม querystring 
        var qString = '?txtsearch=';
        qString += document.getElementById(txtValue).value;
        qString += '&PersonTypeID=' + document.getElementById(txtPersonTypeID).value;
        qString += '&ReturnTo=' + resultClientID;

        // Open Popup
        var newWindow = PopupCenter((pagename + qString), 'popup', '800', '400');

        return false;
    }

    // Validation BY BOOM
    function IsValidate() {
        var val;
        var val2;
        var txtValue = '<%= txtCustomerIDNumber.ClientID%>';
        var txtPersonTypeID = '<%= txtPersonTypeID.ClientID%>';
        val = document.getElementById(txtValue).value;
        val2 = document.getElementById(txtPersonTypeID).value;
        if (val == "") {
            alert("กรุณาระบุคำค้น!")
            //public event EventHandler eClickPersonIDChangednull;
            //handler = eClickPersonIDChangednull;
            //EventHandler;
            //eClickPersonIDChangednull;
            document.getElementById(txtValue).focus();
            return false
        }
        else if (val2 == "-1" || val2 == "1" || val2 == "") {
            alert("กรุณาเลือกประเภทผู้เอาประกัน!")
            return false
        }
        else {
            OpenPopup('frmPersonSearchResult.aspx', '<%= txtResultPostback1.ClientID%>');
            return false;
        }
    }
</script>

<div class="panel panel-default">
    <div class="panel-heading input-md">
        <h4>ค้นหาข้อมูล</h4>
    </div>
    <div class="panel-body">
        <table style="width: 100%;">
            <tr>
                <td style="width: 9%; text-align: right">คำค้น :</td>
                <td style="width: 17%;"> 
                    <asp:TextBox ID="txtCustomerIDNumber" runat="server" MaxLength="13" ToolTip="เลขบัตรประชาชน,Passport,เลขประจำตัวผู้เสียภาษี" placeholder="เลขบัตรประชาชน,Passport,เลขผู้เสียภาษี"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="filterTxtCustomerIDNumber" runat="server" FilterType="Numbers" TargetControlID="txtCustomerIDNumber" />
                </td>
                <td style="width: 12.5%;">
                    <input id="btnSearch" runat="server" type="button" OnClick="IsValidate();" value="ค้นหา" class="btn btn-info btn-sm" style="width: 100%;" />
                    <%--<input id="btnSearch" runat="server" type="button" value="ค้นหา" class="btn btn-default btn-sm" style="width: 100%;"  />--%>
                    <%--<asp:Button ID="btnSearch1" runat="server" SkinID="info" Text="ค้นหาxxx" OnClick="btnSearch1_Click" Visible="false" />--%>
                </td>
                <td style="width: 12.5%;">
                    <asp:Button ID="btnClear" runat="server" Text="Clear" SkinID="warning" OnClick="OnClickClear" />
                </td>
                <td id="td1" style="width: 8%;" runat="server">
                    <asp:TextBox runat="server" ID="txtResultPostback1"
                        Text="" Style="display: none;"
                        AutoPostBack="true" OnTextChanged="txtResultPostback1_TextChanged">
                    </asp:TextBox>
                </td>
                <td id="td2" style="width: 8%;" runat="server">
                     <asp:TextBox runat="server" ID="txtPersonTypeID"
                        Text="" Style="display: none;">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
    </div>

</div>
<asp:HiddenField ID="hdfPersonID" runat="server" />
