<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportGetMoneyClaim_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportGetMoneyClaim_PA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        Table, Tr, Td {
            font: 8.6pt arial, tahoma, sans-serif;
            /*font: 9pt, tahoma;*/
            border-bottom: 1px;
        }

        p {
            line-height: 1.5;
            word-wrap: break-word;
        }

        u {
            border-bottom: 1px dotted #000;
            text-decoration: none;
        }

        .dotted {
        }

        img {
            max-width: 50%;
            max-height: 50%;
        }
    </style>
    <title></title>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div class="page">
            <table style="width: 100%">
                <%-- <tr>
                    <td style="width: 15%"></td>
                    <td style="width: 15%; text-align: center;" colspan="6">
                        <div style="height: 60px; width: 500px; overflow: hidden">
                            <asp:Image runat="server" ID="imgLogo" BorderStyle="None" BorderWidth="0" />
                            <img src="../../../Image/ViriyaLogo.png" style="max-width: 50%; max-height: 50%" />
                        </div>
                    </td>
                    <td style="width: 15%"></td>
                </tr>--%>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%; text-align: center" colspan="4">
                        <h2 style="font-weight: bold">บันทึกรับเงินสินไหมทดแทน</h2>
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <%--<td style="width: 15%"></td>--%>
                    <td style="width: 15%; font-weight: bold" colspan="3">ฝ่ายสินไหมทดแทน
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <%--<td style="width: 15%"></td>--%>
                    <td style="width: 15%; font-weight: bold" colspan="3">
                        <p>
                            เขียนที่ <u>
                                <asp:Label runat="server" ID="lblInsuranceCompany3"></asp:Label></u>
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <%--<td style="width: 15%"></td>--%>
                    <td style="width: 15%; font-weight: bold" colspan="3">วันที่&emsp;
                        <u>&emsp;&emsp;<asp:Label runat="server" ID="lblPrintDate"></asp:Label>&emsp;&emsp;</u>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%;" colspan="6">
                        <p>
                            &emsp;&emsp;&emsp;ข้าพเจ้า

                                <asp:Label runat="server" ID="lblCusName" Text=""></asp:Label>

                            ผู้เอาประกันภัย/ผู้รับผลประโยชน์/ผู้มีอำนาจกระทำการแทน
                        (ซึ่งต่อไปนี้จะเรียกว่า"ผู้รับเงิน")ตามสัญญากรมธรรม์เลขที่
                            <u>
                                <asp:Label runat="server" ID="lblAppId" Text=""></asp:Label>
                            </u>
                            "ผู้รับเงิน" ได้รับเงินค่าสินไหมทดแทนทั้งสิ้นจำนวน
                            <u>
                                <asp:Label runat="server" ID="lblSumClaimMoney" Text=""></asp:Label>
                            </u>
                            บาท (<u><asp:Label runat="server" ID="lblSumClaimMoneyWrite" Text=""></asp:Label></u>) จากบริษัท
                        <u>
                            <asp:Label runat="server" ID="lblInsuranceCompany" Text=""></asp:Label></u>
                            "ผู้รับประกันภัย"โดยถูกต้องและครบถ้วนแล้ว ตามวันเดือนปีที่ระบุข้างต้น
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; margin-right: 0px;" colspan="6">เงินจำนวนนี้เป็นการชำระค่าสินไหมทดแทนในการเรียกร้องสินไหมของผู้เอาประกันภัยดังรายละเอียดต่อไปนี้
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" class="underlineTd" colspan="6">
                        <asp:Label runat="server" ID="lblICD10"></asp:Label>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" class="underlineTd" colspan="6">เข้ารับการรักษาที่
                            <asp:Label runat="server" ID="lblHospitalDetail"></asp:Label></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" class="underlineTd" colspan="6">&emsp;</td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" class="underlineTd" colspan="6">&emsp;</td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" class="underlineTd" colspan="6">&emsp;</td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" colspan="6">
                        <p>
                            &emsp;&emsp;&emsp;ข้าพเจ้าได้ตกลงยินยอมให้ถือว่าการชำระเงินสำหรับความเสียหายรายนี้เป็นที่พอใจแก่ข้าพเจ้า
                            เป็นการระงับข้อพิพาทที่มีอยู่ หรือจะมีขึ้นในภายหน้า อีกทั้งเป็นการเลิกร้างปลดเปลื้องสิทธิเรียกร้องทั้งทางแพ่ง และอาญาต่อผู้รับประกันภัย
                            ข้าพเจ้าจะไม่เรียกร้องค่าสินไหมทดแทนใดๆ จากผู้รับประกันภัยอีกต่อไป หากต่อไปในภายหน้ามีทายาท หรือบุคคลอื่นใดมาโต้แย้งสิทธิ
                            จากการรับเงินค่าสินไหมทดแทนดังกล่าวข้างต้น ข้าพเจ้าตกลงยินยอมเป็นผู้รับผิดชอบชดใช้ให้ทั้งสิ้น
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%" colspan="6">
                        <p>
                            &emsp;&emsp;&emsp;อนึ่ง เมื่อ
                            <asp:Label runat="server" ID="lblInsuranceCompany2"></asp:Label>
                            ได้จ่ายเงินค่าสินไหมทดแทนให้กับข้าพเจ้าจนครบถ้วนแล้ว ในฐานะผู้รับประกันภัย ย่อมมีสิทธิตามกฎหมาย
                            ที่จะไปไล่เบี้ยเอาค่าสินไหม/ค่าเสียหายจำนวนดังกล่าวคืนจากผู้กระทำผิดที่ละเมิดข้าพเจ้าต่อไป เพื่อเป็นหลักฐานข้าพเจ้าจึงได้ลงลายมือชื่อไว้เป็นสำคัญ
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%">&nbsp;</td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; border: dotted; text-align: center" rowspan="3" colspan="2">
                        <a>ประทับตราสถานศึกษา</a>
                    </td>
                    <td style="width: 15%; border: solid" colspan="2" rowspan="6">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <input type="checkbox" />เลขที่รับ<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="checkbox" />วันที่รับ<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="checkbox" checked="checked" />ตรวจรายชื่อ<u>&emsp;&emsp;&emsp;&emsp;</u>มี<u>&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="checkbox" checked="checked" />แผน<u>&emsp;<asp:Label runat="server" ID="lblProductType"></asp:Label>&emsp;</u>/<u>&emsp;<asp:Label runat="server" ID="lblMoneyProduct"></asp:Label>&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="checkbox" />PAY IN<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="checkbox" />เงินสด<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>อนุมัติ 1. <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;&emsp;</u>
                                </td>
                            </tr>
                            <tr>
                                <td>&emsp;&emsp;&nbsp;&nbsp;&nbsp;2. <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;&nbsp;&nbsp;&emsp;</u>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 15%; text-align: center" colspan="2">
                        <p>
                            ลงชื่อ...........................................ผู้รับเงิน
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; text-align: center" colspan="2">(...........................................)
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <%--<td style="width: 15%; font-size: 6px" colspan="2">
                        <a>วันเริ่มคุ้มครอง:<asp:Label runat="server" ID="lblStartCoverDate" Text="16 พฤษภาคม 2558"></asp:Label>
                        </a>
                    </td>--%>

                    <td style="width: 15%; text-align: center" colspan="2">
                        <p>
                            ลงชื่อ...........................................พยาน
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; font-size: 9px" colspan="2">
                        <a>วันเริ่มคุ้มครอง:<asp:Label runat="server" ID="lblStartCoverDate" Text="16 พฤษภาคม 2558"></asp:Label>
                        </a>
                    </td>

                    <td style="width: 15%; text-align: center" colspan="2">(...........................................)
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; font-size: 9px" colspan="2">
                        <a>วันที่มีผล:<asp:Label runat="server" ID="lblActionDate" Text="16 พฤษภาคม 2558"></asp:Label>
                        </a>
                    </td>

                    <td style="width: 15%; text-align: center" colspan="2">
                        <p>
                            ลงชื่อ...........................................พยาน
                        </p>
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%; font-size: 9px" colspan="2">
                        <a>วันสิ้นสุดความคุ้มครอง:<asp:Label runat="server" ID="lblEndCoverDate" Text="16 พฤษภาคม 2559"></asp:Label>
                        </a>
                    </td>

                    <td style="width: 15%; text-align: center" colspan="2">(...........................................)
                    </td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%">&nbsp;</td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 15%">&nbsp;</td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>