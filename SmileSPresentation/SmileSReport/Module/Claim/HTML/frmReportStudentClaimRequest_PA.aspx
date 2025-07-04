<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportStudentClaimRequest_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportClaimRequest_PA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        /*.page {
            width: 21cm;
            min-height: 29.7cm;
            padding: 2cm;
            margin: 1cm auto;
            border: 1px #D3D3D3 solid;
            border-radius: 5px;
            background: white;
            box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
        }*/
        .underlineTd {
            border-bottom-style: dotted;
            border-width: 1px;
        }

        body {
            background: rgb(204,204,204);
        }

        .page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
            box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);
        }

            .page[size="A4"] {
                width: 21cm;
                height: 29.7cm;
            }

                .page[size="A4"][layout="portrait"] {
                    width: 29.7cm;
                    height: 21cm;
                }

            .page[size="A3"] {
                width: 29.7cm;
                height: 42cm;
            }

                .page[size="A3"][layout="portrait"] {
                    width: 42cm;
                    height: 29.7cm;
                }

            .page[size="A5"] {
                width: 14.8cm;
                height: 21cm;
            }

                .page[size="A5"][layout="portrait"] {
                    width: 21cm;
                    height: 14.8cm;
                }

        @media print {
            body, page {
                margin: 0;
                box-shadow: 0;
            }
        }

        Table, Tr, Td {
            font: 7.5pt arial, tahoma, sans-serif;
            /*padding:4px;*/
            /*font: 9pt, tahoma;*/
            border-color: black;
            border-spacing: 0;
            padding-left: 5px;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        h4 {
            margin-top: 0;
            margin-bottom: 0;
        }

        .padding {
            padding: 10px;
        }

        u {
            border-bottom: 1px dotted #000;
            text-decoration: none;
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
                    <td style="width: 12.5%; text-align: center; display: table-cell" colspan="4">
                        <h4>แบบเรียกร้องค่าสินไหมทดแทนประกันอุบัติเหตุนักเรียน นักศึกษา</h4>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>--%>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center" colspan="4">
                        <h4>Student Accident Claim Form</h4>
                    </td>
                    <%--<td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>--%>
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
                    <td style="width: 12.5%; font-size: 8px">
                        <asp:Label runat="server" ID="lblClaimHeaderId" Text=""></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; font-size: 8px" colspan="2">
                        <asp:Label runat="server" ID="lblinsuranceCompanyHeader" Text=""></asp:Label>
                    </td>
                    <%--      <td style="width: 12.5%"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-left: solid 0.5px; border-top: solid 0.5px; padding-left: 4px" colspan="3">กรมธรรม์เลขที่:  <u>&emsp;&emsp;
                        <asp:Label runat="server" ID="lblAppId" Text=""></asp:Label>&emsp;&emsp;</u>
                    </td>
                    <%--<td style="width: 12.5%; border-top: solid 0.5px"></td>
                    <td style="width: 12.5%; border-top: solid 0.5px"></td>--%>
                    <td style="width: 12.5%; border-top: solid 0.5px"></td>
                    <%--<td style="width: 12.5%; border-top: solid 0.5px">&nbsp;</td>--%>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-top: solid 0.5px" colspan="2">วันที่รับแจ้ง<u> &emsp;
                        <asp:Label runat="server" ID="lblDateActive" Text="4 มกราคม 2559"></asp:Label>
                        &emsp; </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-left: solid 0.5px; border-bottom: solid 0.5px; padding-left: 4px" colspan="3">ชื่อสถาบันการศึกษา: <u>&emsp;&emsp;<asp:Label runat="server" ID="lblSchoolName" Text=""></asp:Label>&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%; border-bottom: solid 0.5px; border-right: solid 0.5px" colspan="3"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-left: solid 0.5px; border-right: solid 0.5px; border-bottom: solid 0.5px; text-align: center" colspan="6">
                        <h4>ข้อมูลผู้เอาประกันภัย</h4>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-left: solid 0.5px; border-right: solid 0.5px; border-top: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;<asp:Label runat="server" ID="lblCusName" Text=""></asp:Label>&emsp;&emsp;</u>
                        <asp:Label runat="server" ID="lblCusAge" Text=""></asp:Label>&emsp;
                    </u>ปี
                        เลขบัตรประชาชน <u>&emsp;
                            <asp:Label runat="server" ID="lblCusCardId" Text=""></asp:Label>&emsp;
                        </u>
                        เลขบัตรประจำตัวนักศึกษา <u>&emsp;
                            <asp:Label runat="server" ID="lblCusSchoolCardId" Text=""></asp:Label>&emsp;
                        </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ระดับ <u>&emsp;
                        <asp:Label runat="server" ID="lblSchoolClass" Text=""></asp:Label>
                        &emsp; &emsp; &emsp; </u>
                        ชั้นปี <u>&emsp;
                            <asp:Label runat="server" ID="lblSchoolClassDetail" Text=""></asp:Label>
                            &emsp; </u>
                        โทรศัพท์ <u>&emsp;&emsp;
                            <asp:Label runat="server" ID="lblPhoneNum" Text=""></asp:Label>
                            &emsp;&emsp; </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ที่อยู่ที่สามารถติดต่อได้ <u>
                        <asp:Label runat="server" ID="lblFullAddressDetail" Text=""></asp:Label></u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">จำนวนเงินที่เบิกเป็นเงิน <u>&emsp;
                        <asp:Label runat="server" ID="lblClaimAmount" Text="9,999.99"></asp:Label>
                        &emsp;</u>บาท
                        ตัวอักษร(<u><asp:Label runat="server" ID="lblClaimAmountText" Text=""></asp:Label></u> )
                        ใบเสร็จรับเงิน <u>&emsp;<asp:Label runat="server" ID="lblBillAmount"></asp:Label>
                            &emsp;</u> ฉบับ
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">วันที่เกิดอุบัติเหตุ <u>&emsp;
                        <asp:Label runat="server" ID="lblAccidentDate" Text="4 มกราคม 2559"></asp:Label>
                        &emsp;</u>
                        เวลาที่เกิดอุบัติเหตุ <u>&emsp;
                            <asp:Label runat="server" ID="lblAccidentTime" Text="19.04"></asp:Label>
                            &emsp;</u> น.
                        สถานที่เกิดเหตุ <u>&emsp;
                            <asp:Label runat="server" ID="lblAccidentPlace" Text="บ้าน"></asp:Label>&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">รายละเอียดการเกิดอุบัติเหตุ(โดยละเอียด)<u>&emsp;<asp:Label runat="server" ID="lblICD10" Text="อบ.หกล้มบาดเจ็บที่แขน 2 ข้าง"></asp:Label>&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                    </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px; border-bottom: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;
                        </u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">การเจ็บป่วย/การเกิดอุบัติเหตุครั้งนี้ท่าน<input type="checkbox" />ไม่เคยรักษาที่ใดมาก่อน
                        <input type="checkbox" />เคยรักษามาก่อนที่โรงพยาบาล<u>&emsp;&emsp;&emsp;&emsp;</u>
                        เมื่อวันที่ <u>&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">โดยได้ใช้สิทธิไปแล้วเป็นจำนวนทั้งสิ้น<u>&emsp;&emsp;&emsp;&emsp;&emsp;</u>บาท ยังคงเหลือเงินตามสิทธิสำหรับค่ารักษาพยาบาลอีก
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;</u>บาท
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">&emsp;&emsp;ข้าพเจ้าขอมอบฉันทะให้โรงพยาบาล แพทย์ หรือบุคคลอื่นใดทำการตรวจและรักษาข้าพเจ้า หรือบุคคลในครอบครัวของข้าพเจ้า
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">มีอำนาจแจ้งข้อความใดๆที่เกี่ยวกับการบาดเจ็บ ประวัติทางการแพทย์ การปรึกษาโรค ใบสั่งยา หรือการรักษา และสำเนาเอกสารประวัติ
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ทางการแพทย์ของโรงพยาบาลทั้งหมดต่อ บริษัทประกันภัยตามชื่อที่ปรากฎด้านบน หรือผู้ที่ได้รับมอบหมายจากบริษัทฯ
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">อนึ่งสำเนาใบมอบฉันทะนี้ให้ถือว่ามีผลบังคับใช้เช่นเดียวกับต้นฉบับ
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6" rowspan="3">(ประทับตราสถานศึกษา)
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <%--<td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <%--<td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6"></td>--%>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ลงชื่อ<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>ผู้เอาประกัน &emsp;&emsp;
                        วันที่<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>&emsp;&emsp;
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px; border-bottom: solid 0.5px" colspan="6"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-left: solid 0.5px; border-right: solid 0.5px; border-bottom: solid 0.5px; text-align: center" colspan="6">
                        <h4>สำหรับแพทย์ผู้ทำการตรวจรักษากรอก</h4>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ชื่อสกุลผู้บาดเจ็บ <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u> HN<u>&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                        วันที่ทำการตรวจรักษา<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u> เวลา<u>&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ชื่อสถานพยาบาลที่ตรวจรักษา <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">สาเหตุการเกิดอุบัติเหตุ(Cause of accident)
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">อาการบาดเจ็บที่ได้รับ(Illness/Injury)<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">การวินิจฉัยโรคมีความเห็นว่า(Diagnosis)
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">
                        <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: center; border-left: solid 0.5px" colspan="2" rowspan="3">(ประทับตราสถานพยาบาล)
                    </td>
                    <td style="width: 12.5%; border-right: solid 0.5px" colspan="4">
                        <br />
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px" colspan="4">ลงชื่อ:<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>แพทย์ผู้ตรวจรักษา
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px" colspan="4">&emsp;&emsp;&emsp;(<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>)
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; text-align: right; border-right: solid 0.5px; border-left: solid 0.5px" colspan="6">ใบประกอบวิชาชีพเวชกรรมเลขที่<u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u> วันที่ลงความเห็น <u>&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;</u>&emsp;&emsp;&emsp;&emsp;
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%; border-right: solid 0.5px; border-left: solid 0.5px; border-bottom: solid 0.5px" colspan="6"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>