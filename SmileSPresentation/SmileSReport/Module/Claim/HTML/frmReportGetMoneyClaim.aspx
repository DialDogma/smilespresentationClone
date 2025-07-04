<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportGetMoneyClaim.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportGetMoneyClaim" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        body {
            background: rgb(204,204,204);
        }

        .page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
            /*box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);*/
        }

            .page[size="A4"] {
                width: 21cm;
                height: 29.7cm;
            }

                .page[size="A4"][layout="portrait"] {
                    width: 29.7cm;
                    height: 21cm;
                }

        @media print {
            footer {
                font: 8.5pt arial, tahoma, sans-serif;
                position: fixed;
                bottom: 0;
            }
        }

        Table, Tr, Td {
            font: 8.6pt arial, tahoma, sans-serif;
            /*font: 9pt, tahoma;*/
            border-bottom: 1px;
        }

        p {
            line-height: 1.1;
            word-wrap: break-word;
        }

        u {
            border-bottom: 1px dotted #000;
            text-decoration: none;
        }

        .dotted {
        }

        img {
            max-width: 100%;
            max-height: 100%;
        }

        .auto-style1 {
            width: 12.5%;
            height: 46px;
        }

        .underLineBorder {
            position: absolute;
            padding-left: 20px;
            line-height: 7px;
        }

        .underLineBorderMoney {
            position: absolute;
            padding-left: 35px;
            line-height: 9px;
        }

        .underLineBorderH {
            position: absolute;
            padding-left: 35px;
            line-height: 7px;
            left: 150px;
        }

        .underLineBorderD {
            position: absolute;
            padding-left: 20px;
            line-height: 7px;
        }
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

    <title></title>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div class="page">
            <table style="width: 100%">
                <%-- <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center;" colspan="6">
                        <div style="height: 60px; width: 500px; overflow: hidden">
                            <asp:Image runat="server" ID="imgLogo" BorderStyle="None" BorderWidth="0" />
                            <img src="../../../Image/ViriyaLogo.png" style="max-width: 100%; max-height: 100%" />
                        </div>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>--%>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; padding-left: 60px" colspan="4">
                        <h2>บันทึกรับเงินสินไหมทดแทน</h2>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <%--<tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; padding-bottom: 5px;" colspan="2">ฝ่ายสินไหมทดแทน
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>--%>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td colspan="3">
                        <table style="width: 100%;">
                            <tr>
                                <td>เขียนที่
                                    <asp:Label runat="server" ID="lblWriteAt" CssClass="underLineBorder"></asp:Label>...........................................................................
                                    <%--<asp:Label runat="server" ID="lblPoint">......................</asp:Label>--%>
                                    <%--<hr style="border: 0.5px dotted #000; text-decoration: none; width: 60px; margin-left: 0px; margin-top: 15px;" id="hrpoint" />--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td colspan="3">
                        <table style="width: 100%">
                            <tr>
                                <td>วันที่
                                    <asp:Label runat="server" ID="lblPrintDate" CssClass="underLineBorder"></asp:Label>
                                    ...............................................................................
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td colspan="7" class="auto-style1">
                        <p>
                            &emsp;&emsp;&emsp;ข้าพเจ้า
                            <asp:Label runat="server" ID="lblCusName" CssClass="underLineBorder"></asp:Label>
                            ..............................................................................................
                            ผู้เอาประกันภัย / ผู้รับประโยชน์ / ผู้มีอำนาจกระทำการแทน
                        </p>
                        <p>
                            (ซึ่งต่อไปนี้จะเรียกว่า<b>"ผู้รับเงิน"</b>) ตามสัญญากรมธรรม์ประกันภัย เลขที่
                            <asp:Label runat="server" ID="lblAppId" CssClass="underLineBorder"></asp:Label>
                            ....................................................................................................
                           
                        </p>
                        <p>
                            "ผู้รับเงิน" ได้รับเงินค่าสินไหมทดแทนทั้งสิ้นจำนวน
                                <asp:Label runat="server" ID="lblSumClaimMoney" CssClass="underLineBorderMoney"></asp:Label>
                            .............................................
                            บาท (
                            <asp:Label runat="server" ID="lblSumClaimMoneyWrite" CssClass="underLineBorderMoney"></asp:Label>
                            .....................................................................
                            ) 
                        </p>
                        <p>
                            จาก
                            <asp:Label runat="server" ID="lblInsuranceCompany" CssClass="underLineBorder"></asp:Label>
                            ........................................................................................................................................
                            (ซึ่งต่อไปนี้จะเรียกว่า<b>"ผู้รับประกันภัย"</b>)
                        </p>
                        <p>
                            โดยถูกต้องและครบถ้วนแล้ว ตามวันเดือนปีที่ระบุข้างต้น
                        </p>
                        <p>
                            &emsp;&emsp;&emsp;เงินจำนวนนี้ เป็นการชำระค่าสินไหมทดแทน ในการเรียกร้องสินไหมของผู้เอาประกันภัย ดังรายการต่อไปนี้
                        </p>
                        <p>
                            <asp:Label runat="server" ID="lblICD10" CssClass="underLineBorder"></asp:Label>
                            ...........................................................................................................................................................................................................
                        </p>
                        <p>
                            <asp:Label runat="server" ID="Label1" CssClass="underLineBorderD" Text="เข้ารับการรักษาที่"></asp:Label>
                            <asp:Label runat="server" ID="lblHospitalDetail" CssClass="underLineBorderH"></asp:Label>
                            ...........................................................................................................................................................................................................</p>
                        <p>...........................................................................................................................................................................................................</p>
                        <p>...........................................................................................................................................................................................................</p>
                        <p>...........................................................................................................................................................................................................</p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="7">
                        <p>
                            &emsp;&emsp;&emsp;ข้าพเจ้าได้ตกลงยินยอมให้ถือว่าการชำระเงินสำหรับความเสียหายรายนี้เป็นที่พอใจแก่ข้าพเจ้า เป็นการระงับข้อพิพาทที่มีอยู่ หรือจะมี
                        </p>
                        <p>
                            ขึ้นในภายภาคหน้า อีกทั้งเป็นการเลิกร้างปลดเปลื้องสิทธิเรียกร้องทั้งทางแพ่ง และทางอาญาต่อผู้รับประกันภัย ข้าพเจ้าจะไม่เรียกร้องค่า
                        </p>
                        <p>
                            สินไหมทดแทนใดๆ จากผู้รับประกันภัยอีกต่อไป หากต่อไปในภายภาคหน้ามีทายาท หรือบุคคลอื่นใดมาโต้แย้งสิทธิจากการรับเงินค่าสินไหม
                        </p>
                        <p>
                            ทดแทนดังกล่าวข้างต้น ข้าพเจ้าตกลงยินยอมเป็นผู้รับผิดชอบใช้ให้ทั้งสิ้น
                        </p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="7">
                        <p>
                            &emsp;&emsp;&emsp;อนึ่ง เมื่อผู้รับประกันภัยได้จ่ายค่าสินไหมทดแทนให้กับข้าพเจ้าจนครบถ้วนแล้ว ดังนั้น บริษัทฯ ในฐานะผู้รับประกันภัย ย่อมมีสิทธิตาม
                        </p>
                        <p>
                            กฎหมายที่จะไปไล่เบี้ยเอาค่าสินไหมทดแทน หรือค่าเสียหายจำนวนดังกล่าวคืนจากผู้กระทำที่ละเมิดข้าพเจ้าต่อไป เพื่อเป็นหลักฐาน
                        </p>
                        <p>
                            ข้าพเจ้าจึงได้ลงลายมือชื่อไว้เป็นสำคัญ
                        </p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; padding-left: 40px" colspan="4">
                        <p>
                            ลงชื่อ................................................
                        </p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; padding-left: 40px" colspan="4">(............................................................)
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; padding-left: 40px" colspan="4">(ผู้รับเงิน)
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center;" colspan="3">
                        <p>
                            ลงชื่อ................................................
                        </p>
                    </td>
                    <td style="width: 12.5%; text-align: center;" colspan="4">
                        <p>
                            ลงชื่อ................................................
                        </p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center;" colspan="3">(............................................................)
                    </td>
                    <td style="width: 12.5%; text-align: center;" colspan="4">(............................................................)
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center;" colspan="3">(พยาน)
                    </td>
                    <td style="width: 12.5%; text-align: center;" colspan="4">(พยาน)
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="7">
                        <b>หมายเหตุ : </b>กรุณาแนบสำเนาบัตรประจำตัวประชาชนของผู้รับเงิน<a style="text-decoration: underline">ที่ลงลายมือชื่อรับรองแล้ว</a> มาพร้อมบันทึกรับเงินสินไหมฉบับนี้
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </form>
    <footer>
        <table>
            <tr>
                <td style="width: 10%">QF-CLM-COP-004</td>
                <td style="width: 12.5%;"></td>
                <td style="width: 12.5%; text-align: right;">Effective Date 1 Sep 2019</td>
            </tr>
        </table>
    </footer>
</body>
</html>

<script>

    //$(function () {
    //    debugger;
    //    var sss = document.getElementById("lblWriteAt");
    //    if ($("#lblWriteAt").text() == "") {
    //        $("#tdWriteAt").css("border-bottom-style", "none");
    //        //$("#lblPoint").prop("visible", true);
    //        //<h1>This is a Heading1111111111111</h1>

    //        $("#lblWriteAt").hide();
    //        $("#hrpoint").show();
    //    } else {
    //        $("#tdWriteAt").css("border-bottom-style", "none");
    //        //$("#lblPoint").prop("visible", false);
    //        //<h1>This is a Heading5555555555555555</h1>
    //        //alert("22222");
    //        $("#lblWriteAt").show();
    //        $("#hrpoint").hide();
    //    }

    //});
</script>
