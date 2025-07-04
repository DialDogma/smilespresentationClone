<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSuccess.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmSuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">

            function closeBrowser() {
                window.open('', '_self', '');
                window.close();
            }
        </script>
        <div id="divImg" style="text-align: center" runat="server" visible="False">
            <img src="../../Image/emoji_cry.png" />
        </div>
        <div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%"></td>
                </tr>

                <tr>
                    <td style="width: 100%; text-align: center"></td>
                </tr>

                <tr>
                    <td style="width: 100%; text-align: center">
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Size="Large"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td style="width: 100%"></td>
                </tr>

                <tr>
                    <td style="width: 100%; text-align: center" class=""><a href="javascript:closeBrowser();">ปิดหน้านี้</a></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>