<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportRequestHistory.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportRequestHistory" %>

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

        u {
            border-bottom: 1px dotted #000;
            text-decoration: none;
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
            line-height: 0.5;
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
                    <%--<td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%; text-align: right" colspan="3">
                        <h2>งานสินไหม แผนกสุขภาพ</h2>
                    </td>
                    <%--<td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%" rowspan="4">
                        <span>
                            <img src="../../../Image/logoSiamSmile.jpg" />
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right">
                        <asp:Label runat="server" ID="lblDatePrint"></asp:Label>
                    </td>
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
                    <td style="width: 12.5%" colspan="2">
                        <p>เรื่อง  ขอรายละเอียดเพิ่มเติม</p>
                    </td>
                    <%--<td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="6">
                        <p>
                            เรียน  หัวหน้าฝ่ายการเงินผู้ป่วยใน/เวชระเบียน &emsp;
                            <asp:Label runat="server" ID="lblHospitalName"></asp:Label>
                        </p>
                    </td>
                    <%--<td style="width: 12.5%"></td>--%>                    <%--<td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <%--<tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="3">
                        <p>
                        </p>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>--%>
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
                    <td style="width: 12.5%; line-height: 1.6;" colspan="6" <%--rowspan="3"--%>>&emsp;&emsp; ตามที่ท่านได้ส่งเอกสารเพื่อแจ้งใช้สิทธิแฟกซ์เคลมเพื่อพิจารณาสินไหมทดแทนเบื้องต้นของสมาชิกผู้เอาประกัน

                            <asp:Label runat="server" ID="lblCustomerFullName"></asp:Label>
                        &nbsp;
                           <asp:Label runat="server" ID="lblANHN"></asp:Label>
                        &nbsp;
                           กับโครงการบัตรประกันสุขภาพกลุ่มข้าราชการ  &nbsp;

                            ตามเอกสารลงวันที่
                        <asp:Label runat="server" ID="lblDateIn"></asp:Label>
                        &nbsp;-&nbsp;
                           <asp:Label runat="server" ID="lblDateCut"></asp:Label>
                        &nbsp;
                           นั้นเนื่องจากบริษัทฯ ใคร่ขอเสนอเอกสารเพิ่มเติมจากทางโรงพยาบาล

                            ดังนี้
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <%--td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
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
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>O.P.D. Card ทั้งหมด
                        </span>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%" colspan="3"></td>
                    <%--<td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ใบสั่งการแพทย์(Doctor Order)
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>บันทึกการให้ยาของพยาบาล
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>Progress Note
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ฟอร์มปรอท
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ใบรายงานการผ่าตัด(Operative Note)
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>Nurse Note
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ผลการตรวจ
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ทางห้องปฎิบัติการ และพยาธิวิทยา
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ทางรังสีวิทยา
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>การตรวจโดยวิธีพิเศษอื่นๆ
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>รายละเอียดรายการและค่าใช้จ่าย
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ค่ายา ค่าเวชภัณฑ์ และอุปกรณ์การแพทย์
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ค่าตรวจ LAB พยาธิ รังสี และตรวจพิเศษอื่นๆ
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>ค่าแพทย์แต่ละท่าน/วัน+ระบุสาขาเฉพาะทาง
                        </span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">
                        <input type="radio" value="" style="height: 25px; width: 25px; vertical-align: middle;" />
                        <span>อื่นๆ
                        </span>
                    </td>
                    <td style="width: 12.5%" colspan="3"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%;" colspan="6">
                        <p>________________________________________________________________________________</p>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="6">
                        <p>________________________________________________________________________________</p>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="6">
                        <p>&emsp;&emsp;จึงเรียนมาเพื่อทราบ และโปรดดำเนินการ บริษัทฯขอขอบพระคุณในความร่วมมือมา ณ โอกาสนี้</p>
                        <p>และโปรดส่งเอกสาร กลับมาที่ หมายเลขโทรสาร 0-2533-3258-9 หากท่านมีข้อสงสัยหรือขัดข้องประการใด</p>
                        <p>กรุณาติดต่อหมายเลขโทรศัพท์ 1434, 0-2533-3999 ต่อ 322,301,383 หรือ 384</p>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="2">ขอแสดงความนับถือ
                    </td>
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
                    <td style="width: 12.5%; text-align: center" colspan="2">
                        <asp:Label runat="server" ID="lblEmpName"></asp:Label>
                    </td>

                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="2">
                        <asp:Label runat="server" ID="lblDept"></asp:Label>
                    </td>

                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; font-size: 9px" colspan="4">บริษัท สไมล์ ทีพีเอ จำกัด
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; font-size: 9px" colspan="4">89/6-10 ถนนเฉลิมพงษ์ แขวงสายไหม เขตสายไหม กรุงเทพฯ 10220
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; font-size: 9px" colspan="4">โทรสาร: 0-2533-3258-9 โทรศัพท์: 1434, 0-2533-3999
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
        <footer>
        <table>
            <tr>
                <td style="width: 10%">QF-CLM-COP-005</td>
                <td style="width: 12.5%;"></td>
                <td style="width: 12.5%; text-align: right;">Effective Date 1 Sep 2019</td>
            </tr>
        </table>
    </footer>
    </form>
</body>
</html>