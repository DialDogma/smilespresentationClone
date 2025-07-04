<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportClaimGivePower_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportClaimGivePower_PA" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <style>
        body {
            background: rgb(204,204,204);
        }

        .underlineTd {
            border-bottom-style: dotted;
            border-width: 1px;
        }

        .page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
            /*box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);*/
        }

        .underlineTd {
            border-bottom-style: dotted;
            border-width: 1px;
        }

        .page[size="A4"] {
            width: 21cm;
            height: 29.7cm;
        }

            .page[size="A4"][layout="portrait"] {
                width: 29.7cm;
                height: 21cm;
            }

        /*@media print {
            body, page {
                margin: 0;
                box-shadow: 0;
            }
        }*/

         @media print {
            footer{
                font: 8.5pt arial, tahoma, sans-serif;
                position: fixed;
                bottom: 0;
            }
        }

        Table, Tr, Td {
            font: 8.5pt arial, tahoma, sans-serif;
            /*font: 9pt, tahoma;*/
            border-bottom: 1px;
            word-wrap: break-word
        }

        p {
            line-height: 1.5;
        }

        img {
            max-width: 100px;
            height: auto;
        }

        .item {
            width: 500px;
            min-height: 500px;
            max-height: auto;
            float: left;
            margin: 3px;
            padding: 3px;
        }
    </style>
    <title></title>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div class="page">
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%; text-align: center; font-size: larger">
                        <asp:Label runat="server" ID="lblHeader">
                            หนังสือมอบอำนาจ
                        </asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">เขียนที่&nbsp;
                        <asp:Label runat="server" ID="lblHospital"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">วันที่ &nbsp;&nbsp; ...................................</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left; height: 10px" colspan="6">
                        <p>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;โดยหนังสือฉบับนี้ ข้าพเจ้า (นาย/นาง/นางสาว)
                      <asp:Label runat="server" ID="lblFullname"></asp:Label>
                            <%--      <asp:Label runat="server" ID="lblFirstName"></asp:Label>
                            &nbsp;
                            <asp:Label runat="server" ID="lblLastName"></asp:Label>--%>
                            &nbsp;ที่อยู่ เลขที่
                            <asp:Label runat="server" ID="lblAddressNo"></asp:Label>
                            แขวง/ตำบล
                            <asp:Label runat="server" ID="lblTambol"></asp:Label>
                            <br>
                            เขต/อำเภอ
                            <asp:Label runat="server" ID="lblAmphur"></asp:Label>

                            จังหวัด
                            <asp:Label runat="server" ID="lblProvince"></asp:Label>
                            บัตรประชาชนเลขที่
                            <asp:Label runat="server" ID="lblCustomerCardId"></asp:Label>
                            &nbsp;
                            &ensp;
                            <br />
                            ผู้เอาประกันภัย/ผู้รับผลประโยชน์/ผู้มีอำนาจกระทำการแทน (ซึ่งต่อไปนี้จะเรียกว่า "ผู้มอบอำนาจ") ตามสัญญากรมธรรม์ประกันภัย &nbsp;
                        เลขที่ &nbsp;
                        <asp:Label runat="server" ID="lblAppId"></asp:Label>
                            &nbsp; ของ
                        <asp:Label runat="server" ID="lblInsuranceCompany"></asp:Label>
                            &nbsp;
                        ผู้รับประกันภัย ขอมอบอำนาจให้ บริษัท สไมล์ ทีพีเอ จำกัด ทะเบียนเลขที่ 0575554000911 เป็นตัวแทนของข้าพเจ้าเพื่อกระทำการดังต่อไปนี้
                        </p>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">1. ติดต่อประสานงานกับบริษัทผู้รับประกันภัยดังกล่าวข้างต้น เกี่ยวกับการเรียกร้องค่าทดแทนตามสัญญาประกันภัยข้างต้น</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">2. รับค่าสินไหมทดแทนในสัญญากรมธรรม์ประกันภัยตามหมายเลขอ้างอิงดังกล่าวข้างต้น โดยให้บริษัทผู้รับประกันภัย</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">&nbsp;&nbsp;&nbsp; จ่ายในนามผู้รับมอบอำนาจได้</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">3. ผู้รับมอบอำนาจมีหน้าที่นำเงินค่าสินไหมทดแทน ส่งมอบให้แก่ผู้มอบอำนาจให้เสร็จสิ้น</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">4. ทั้งนี้ ให้ผู้รับมอบอำนาจ มีอำนาจถ้อยคำ ชี้แจง รับส่งเอกสาร ลงลายมือชื่อ ตลอดจนกระทำการใดๆ อันต่อเนื่อง</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">&nbsp;&nbsp;&nbsp; เพื่อให้บรรลุผลแห่งการมอบอำนาจนี้ทุกประการ</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;การใดที่ผู้รับมอบอำนาจได้กระทำภายในขอบแห่งการมอบอำนาจนี้ ข้าพเจ้ายินยอมรับผิดชอบทุกประการ โดยเสมือนหนึ่ง ข้าพเจ้าได้กระทำไปด้วยตนเองทุกประการ เพื่อเป็นหลักฐานจึงได้ลงลายมือชื่อไว้ต่อหน้าพยาน</p>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
            </table>
            <!-- footer -->
            <table style="width: 100%">
                <tr>
                    <td style="width: 33.3%"></td>
                    <td style="width: 60%">ลงชื่อ&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        ผู้มอบอำนาจ/ผู้ให้คำยินยอม/ผู้ปกครองโดยชอบธรรม
                    </td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%"></td>
                    <td style="width: 35%">&emsp;&emsp;&emsp;(.............................................)
                    </td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%"></td>
                    <td style="width: 40%">ลงชื่อ&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        ผู้รับมอบอำนาจ
                    </td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%"></td>
                    <td style="width: 35%">&emsp;&emsp;&emsp;(.............................................)
                    </td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 35%"></td>
                    <td style="width: 33.3%"></td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 12%"></td>
                    <td style="width: 33.3%">ลงชื่อ&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        พยาน
                    </td>
                    <td style="width: 16%"></td>
                    <td style="width: 33.3%">ลงชื่อ&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        พยาน
                    </td>
                </tr>
                <tr>
                    <td style="width: 12%"></td>
                    <td style="width: 33.3%">&emsp;&emsp;&emsp;(.............................................)</td>
                    <td style="width: 16%"></td>
                    <td style="width: 33.3%">&emsp;&emsp;&emsp;(.............................................)</td>
                </tr>
                <tr>
                    <td style="width: 12%"></td>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 16%"></td>
                    <td style="width: 33.3%">&emsp;</td>
                </tr>
                <tr>
                    <td style="width: 12%"></td>
                    <td style="width: 33.3%">&emsp;</td>
                    <td style="width: 16%"></td>
                    <td style="width: 33.3%">&emsp;</td>
                </tr>
                <tr>
                    <td style="width: 12%"></td>
                    <td style="width: 33.3%" colspan="2">*หมายเหตุ กรุณาแนบสำเนาบัตรประจำตัวประชาชนของผู้มอบอำนาจ</td>
                    <td style="width: 16%"></td>
                    <td style="width: 33.3%">&emsp;</td>
                </tr>
            </table>
        </div>
        <footer>
        <table>
            <tr>
                <td style="width: 10%">QF-CLM-COP-003</td>
                <td style="width: 12.5%;"></td>
                <td style="width: 12.5%; text-align: right;">Effective Date 1 Sep 2019</td>
            </tr>
        </table>
    </footer>
    </form>
</body>
</html>