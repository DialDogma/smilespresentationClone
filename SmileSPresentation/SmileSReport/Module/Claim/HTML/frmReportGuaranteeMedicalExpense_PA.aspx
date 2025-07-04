<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportGuaranteeMedicalExpense_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportGuaranteeMedicalExpense_PA" %>

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

        /*@media print {
            body, page {
                margin: 0;
                box-shadow: 0;
            }
        }*/

        Table, Tr, Td {
            font: 9pt arial, tahoma, sans-serif;
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
            max-width: 100%;
            max-height: 100%;
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
                    <td style="width: 12.5%" colspan="4">
                        <h3>หนังสือรับรองการใช้สิทธิ์เบิกค่ารักษาพยาบาล</h3>
                    </td>
                    <%-- <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <%--<td style="width: 12.5%"></td>--%>
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
                    <td style="width: 12.5%" colspan="3">
                        <asp:Label runat="server" ID="lblSchoolNameHeader" Text="โรงเรียนนวมินทราชูทิศ กรุงเทพมหานคร"></asp:Label>
                    </td>
                    <%--      <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">ตำบล<asp:Label runat="server" ID="lblSubDistrict" Text="นวลจันทร์"></asp:Label>
                        อำเภอ<asp:Label runat="server" ID="lblDistrict" Text="บึงกุ่ม"></asp:Label>
                    </td>
                    <%--<td style="width: 12.5%"></td>--%>
                    <%--<td style="width: 12.5%"></td>--%>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">จังหวัด
                        <asp:Label runat="server" ID="lblProvince" Text="กรุงเทพมหานคร"></asp:Label>
                        รหัสไปรษณีย์
                        <asp:Label runat="server" ID="lblPostCode" Text="10230"></asp:Label>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">โทรศัพท์
                        <asp:Label runat="server" ID="lblPhone" Text="085-0439223"></asp:Label>
                        โทรสาร
                        <asp:Label runat="server" ID="lblPhoneCode" Text="-"></asp:Label>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">วันที่
                        <asp:Label runat="server" ID="lblDatePrint" Text="4 มกราคม 2559"></asp:Label>
                    </td>
                    <%-- <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
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
                    <td style="width: 12.5%" colspan="3">เรื่อง &emsp; รับรองสิทธิ์นักเรียน/นักศึกษา
                    </td>
                    <%--td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="3">เรียน &emsp; ผู้อำนวยการ
                        <asp:Label runat="server" ID="lblHospital" Text="โรงพยาบาลสินแพทย์"></asp:Label>
                    </td>
                    <%-- <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
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
                    <td style="width: 12.5%" colspan="6">&emsp;&emsp;&emsp;&emsp;&emsp; ข้าพเจ้า
                        <asp:Label runat="server" ID="lblEmpName" Text="นายจักรกริช อิ่นคำ"></asp:Label>
                        ตำแหน่ง
                        <asp:Label runat="server" ID="lblEmpDepartment" Text="Callcenter"></asp:Label>
                        จังหวัด
                        <asp:Label runat="server" ID="lblEmpLocal" Text="เชียงราย"></asp:Label>
                        ขอรับรองว่า
                        <asp:Label runat="server" ID="lblStudentName" Text="ด.ช.วุฒิชัย ชัยฝาง"></asp:Label>
                        ประจำปีการศึกษา
                        <asp:Label runat="server" ID="lblYear" Text="2558"></asp:Label>
                        เป็นนักเรียน/นักศึกษา/บุคลากร ของโรงเรียน/วิทยาลัย
                        <asp:Label runat="server" ID="lblSchoolNameBody" Text="โรงเรียนนวมินทราชูทิศ กรุงเทพมหานคร"></asp:Label>
                        จริง
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
                    <td style="width: 12.5%" colspan="6">&emsp;&emsp;&emsp;&emsp;&emsp; ทั้งนี้โรงเรียน/สถาบัน ได้ทำประกันอุบัติเหตุ กับ บริษัท สยามสไมล์โบรกเกอร์(ประเทศไทย) จำกัด
                        โดยมี
                        <asp:Label runat="server" ID="lblInsuranceCompany" Text="บริษัท ชับบ์สามัคคีประกันภัย จำกัด(มหาชน)"></asp:Label>
                        เป็นผู้รับประกันภัย สามารถใช้สิทธิ์ในการเบิกค่ารักษาพยาบาลได้ตามปกติ
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
                        <h4>มีสิทธิประโยชน์ในการเข้ารับการรักษาดังนี้</h4>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="2">1.
                        <asp:Label runat="server" ID="lblAdvantage1" Text="ค่ารักษาพยาบาลต่อครั้ง"></asp:Label>
                    </td>
                    <%--  <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">
                        <asp:Label runat="server" ID="lblAdvantage1Cost" Text="10,000"></asp:Label>
                    </td>
                    <td style="width: 12.5%">บาท/อุบัติเหตุ
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
                    <td style="width: 12.5%" colspan="4">ทั้งนี้การพิจารณาเป็นไปตามเงื่อนไขของกรมธรรม์
                        <br />
                        ให้ไว้ ณ วันที่
                        <asp:Label runat="server" ID="lblAppDate" Text="4 มกราคม 2559"></asp:Label>
                    </td>
                    <%-- <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>--%>
                    <%--td style="width: 12.5%">&nbsp;</td>--%>
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
                    <td style="width: 12.5%; text-align: center" colspan="3">ลงชื่อ ..............................................
                    </td>
                    <%--<td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="3">(.................................................)
                    </td>
                    <%--<td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="3">เจ้าหน้าที่ Call Center
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: center" colspan="3">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: center" colspan="3">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: center" colspan="3">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: center" colspan="3">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="3">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; font-weight: bold" colspan="6">บริษัท สไมล์ทีพีเอ จำกัด
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; font-weight: bold" colspan="6">89/6-10 ถนนเฉลิมพงษ์ แขวงสายไหม เขตสายไหม กรุงเทพฯ 10220 โทรสาร: 0-2533-3258-9 โทรศัพท์: 1434, 0-2533-3999
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>