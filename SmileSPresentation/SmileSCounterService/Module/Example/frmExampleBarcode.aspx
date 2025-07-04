<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmExampleBarcode.aspx.cs" Inherits="BarcodeForm.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Style/StyleForm1.css" rel="stylesheet" />
    <link href="../../Bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../../Bootstrap/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                    <div id="logo">
                        <asp:Image ID="ImageLogo" runat="server" ImageUrl="/Image/Logo.PNG" />
                    </div>
                    <div id="barcode">
                        <table class="tableDetail">
                            <tr>
                                <td id="cellBarcode">
                                    <asp:Image ID="ImageBarcode" runat="server" ImageUrl="~/Image/barcode.gif" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="detail">
                        <table class="tableDetail">
                            <tr>
                                <td id="cellDetail">
                                    <div id="textDetail">
                                        <asp:Label CssClass="lbLeft" ID="lbTotal1" runat="server" Text="ยอดรวม"></asp:Label>
                                        <asp:Label CssClass="lbRight" ID="lbTotal2" runat="server" Text="14,567 บาท"></asp:Label><br />

                                        <asp:Label CssClass="lbLeft" ID="lbUser1" runat="server" Text="ผู้ชำระเงิน"></asp:Label>
                                        <asp:Label CssClass="lbRight" ID="lbUser2" runat="server" Text="สมชาย มาเยอะ"></asp:Label><br />

                                        <asp:Label CssClass="lbLeft" ID="lbCompany1" runat="server" Text="ชำระให้"></asp:Label>
                                        <asp:Label CssClass="lbRight" ID="lbCompany2" runat="server" Text="SIAMSMILE"></asp:Label><br />

                                        <asp:Label CssClass="lbLeft" ID="lbDate1" runat="server" Text="วันที่ทำรายการ"></asp:Label>
                                        <asp:Label CssClass="lbRight" ID="lbDate2" runat="server" Text="dd/mm/yy"></asp:Label><br />
                                    </div>

                                    <div id="barcodeValue">
                                        <asp:Label CssClass="lbLeftVar" ID="txtId1" runat="server" Text="Tax Id No."></asp:Label>
                                        <asp:Label CssClass="lbRightVar" ID="txtId2" runat="server" Text="0135551004383"></asp:Label><br />

                                        <asp:Label CssClass="lbLeftVar" ID="serviceCode1" runat="server" Text="Service Code"></asp:Label>
                                        <asp:Label CssClass="lbRightVar" ID="serviceCode2" runat="server" Text="01"></asp:Label><br />

                                        <asp:Label CssClass="lbLeftVar" ID="referenceOne1" runat="server" Text="Reference No."></asp:Label>
                                        <asp:Label CssClass="lbRightVar" ID="referenceOne2" runat="server" Text="510120008698119"></asp:Label><br />

                                        <asp:Label CssClass="lbLeftVar" ID="referencetwo1" runat="server" Text="Reference No."></asp:Label>
                                        <asp:Label CssClass="lbRightVar" ID="referencetwo2" runat="server" Text=""></asp:Label><br />
                                    </div>

                                    <div id="textWarning">
                                        <asp:Label ID="lbWarning1" runat="server" Text="*สามารถนำโค้ดนี้ไปชำระได้ที่เคาเตอร์เซอร์วิสเท่านั้น"></asp:Label><br />
                                        <asp:Label ID="lbWarning2" runat="server" Text="*ไม่รวมค่าธรรมเนียมของเคาเตอร์เซอร์วิส"></asp:Label>
                                    </div>

                                    <asp:Image ID="ImageLogo2" runat="server" ImageUrl="~/Image/counter_service.jpg" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>
        </div>
    </form>
</body>
</html>
