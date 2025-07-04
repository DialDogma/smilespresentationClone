<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWHT50.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmWHT50" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ใบรับรองทวิ 50</title>

    <%-- style --%>
    <style>
        body {
            margin: 0;
            padding: 0;
        }

        .page {
            background: white;
            display: block;
            margin: 0;
            padding: 0;
        }

            .page[size="A4"] {
                width: 21cm;
                height: 29.7cm;
            }

                .page[size="A4"][layout="portrait"] {
                    width: 29.7cm;
                    height: 21cm;
                }

        table {
            font: 8pt arial, tahoma, sans-serif;
            /*font: 9pt, tahoma;*/
            /*border-bottom: 1px;*/
        }

        tr, td {
            font: 8pt arial, tahoma, sans-serif;
            padding: 3px 3px 3px 3px;
        }

        .dotted {
            border-bottom-style: dotted;
            border-bottom-color: grey;
            padding-left: 2px;
            padding-right: 2px;
        }

        .rcorners {
            width: initial;
            height: 18px;
            padding: 1px;
            border-radius: 3px;
            background-color: lightgray;
        }

        .div-rcorners {
            width: initial;
            height: auto;
            margin: 2px;
            padding: 5px;
            border-style: solid;
            border-radius: 5px;
            border-width: 1px
        }

        .corners {
            width: initial;
            height: auto;
            padding: 5px;
            border-style: solid;
            border-width: 1px
        }
        .auto-style1 {
            border-bottom-style: dotted;
            border-bottom-color: grey;
            padding-left: 2px;
            padding-right: 2px;
            width: 75px;
        }
        .auto-style2 {
            border-bottom-style: dotted;
            border-bottom-color: grey;
            padding-left: 2px;
            padding-right: 2px;
            width: 75px;
        }

        .auto-style4 {
            border-bottom-style: dotted;
            border-bottom-color: grey;
            padding-left: 2px;
            padding-right: 2px;
            width: 100px;
        }
    </style>
    <%-- end style --%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page">
            <!-- ส่วน Header -->
            <table style="width: 100%; border-collapse: collapse; margin: 0 auto;">
                <tr>
                    <td>
                        <span><strong>ฉบับที่ 1</strong> (สำรหับผู้ถูกหักภาษี ณ ที่จ่าย ใช้แนบพร้อมกับแบบแสดงรายการภาษี)</span>

                    </td>
                </tr>
                <tr>
                    <td>
                        <span><strong>ฉบับที่ 2</strong> (สำหรับผู้ถูกหักภาษี ณ ที่จ่าย เก็บไว้เป็นหลักฐาน)</span>

                    </td>
                </tr>
            </table>

            <!-- ส่วน Body -->
            <div class="corners">
                <!-- ส่วนที่ 2 -->
                <table style="border-collapse: collapse; width: 100%;">
                    <tr>
                        <td style="width: 20%;"></td>
                        <td style="width: 60%; text-align: center;">
                            <h2 style="margin: 0px; padding: 0px;">หนังสือรับรองการหักภาษี ณ ที่จ่าย</h2>
                        </td>
                        <td style="width: 20%; text-align: right;">เล่มที่................
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;"></td>
                        <td style="width: 60%; text-align: center; font-size: small;">
                            <%--<h3 style="margin: 0px; padding: 0px;">ตามมาตรา 50 ทวิ แห่งประมวลรัษฎากร</h3>--%>
                            ตามมาตรา 50 ทวิ แห่งประมวลรัษฎากร
                        </td>
                        <td style="width: 20%; text-align: right;">เลขที่................
                        </td>
                    </tr>
                </table>

                <!-- ส่วนที่ 3 -->
                <div class="div-rcorners">
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td>
                                <strong>
                                    ผู้มีหน้าที่หักภาษี ณ ที่จ่าย :&nbsp;
                                </strong>
                                
                                <asp:Label ID="lblTaxOwnName" runat="server" Text="-"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    เลขประจำตัวผู้เสียภาษีอากร (13 หลัก)*&nbsp;
                                </strong>
                                <asp:TextBox ID="citizen1" Style="text-align: center" runat="server" MaxLength="13"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <strong>
                                    ชื่อ
                                </strong>
                                .............................................................................................

                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    เลขประจำตัวผู้เสียภาษีอากร&nbsp;
                                </strong>                       
                                <asp:TextBox ID="taxNo1" Style="text-align: center" runat="server" MaxLength="10"></asp:TextBox>

                                
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <strong>
                                    ที่อยู่
                                </strong>
                                ..............................................................................................................................................................................................................................
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- ส่วนที่ 4 -->
                <div class="div-rcorners">
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td>
                                <strong>
                                    ผู้ถูกหักภาษี ณ ที่จ่าย :&nbsp;
                                </strong>
                                
                            <asp:Label ID="lblPayTaxName" runat="server" Text="-"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    เลขประจำตัวผู้เสียภาษีอากร (13 หลัก)*&nbsp;
                                </strong>
                                
                                <asp:TextBox ID="citizen2" Style="text-align: center" runat="server" MaxLength="13"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <strong>
                                    ชื่อ
                                </strong>
                                .............................................................................................

                            </td>
                            <td style="text-align: right;">
                                <strong>
                                    เลขประจำตัวผู้เสียภาษีอากร
                                </strong>
                                
                                <asp:TextBox ID="taxNo2" Style="text-align: center" runat="server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <strong>
                                    ที่อยู่
                                </strong>  
                                ..............................................................................................................................................................................................................................
                            </td>
                        </tr>
                    </table>

                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td rowspan="2">
                                <strong>ลำดับที่</strong>&nbsp;
                                <asp:TextBox ID="txtNo" runat="server" Width="100px"></asp:TextBox>
                                &nbsp;ในแบบ
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="(1) ภ.ง.ด.1ก" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox2" runat="server" Text="(2) ภ.ง.ด.1ก พิเศษ" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox3" runat="server" Text="(3) ภ.ง.ด.2" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox4" runat="server" Text="(4) ภ.ง.ด.3" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="CheckBox5" runat="server" Text="(5) ภ.ง.ด.2ก" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox6" runat="server" Text="(6) ภ.ง.ด.3ก" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox7" runat="server" Text="(7) ภ.ง.ด.53" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>

                <!-- ส่วนที่ 5 -->
                <div class="div-rcorners">
                    <table style="border-collapse: collapse; width: 100%; display: inline-table;" border="1">
                        <tr>
                            <th style="width: 55%;">
                                ประเภทเงินได้พึงประเมินที่จ่าย
                            </th>
                            <th class="auto-style3">
                                วัน เดือน
                                <br />
                                หรือปีภาษี ที่จ่าย
                            </th>
                            <th colspan="2">
                                จำนวนเงินที่จ่าย
                            </th>
                            <th colspan="2">
                                ภาษีที่หัก
                                <br />
                                และนำส่งไว้
                            </th>
                        </tr>

                        <tr>
                            <td>1. เงินเดือน ค่าจ้าง เบี้ยเลี้ยง โบนัส ฯลฯ ตามมาตรา 40 (1)

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">2. ค่าธรรมเนียม ค่านายหน้า ฯลฯ ตามมาตรา 40 (2)

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">3. ค่าแห่งลิขสิทธิ์ ฯลฯ ตามมาตรา 40 (3)

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">4. (ก) ดอกเบี้ย ฯลฯ ตามมาตรา 40 (4) (ก)
                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;
                                (ข) เงินปันผล เงินส่วนแบ่งกำไร ฯลฯ ตามมาตรา 40 (4)
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (1) กรณีผู้ได้รับเงินปันผลได้รับเครดิตภาษี โดยจ่ายจาก
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                กำไรสุทธิของกิจการที่ต้องเสียภาษีเงินได้นิติบุคคลในอัตราดังนี้
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (1.1) อัตราร้อยละ 30 ของกำไรสุทธิ
                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (1.2) อัตราร้อยละ 25 ของกำไรสุทธิ

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (1.3) อัตราร้อยละ 20 ของกำไรสุทธิ

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (1.4) อัตราอื่น ๆ (ระบุ) ................... ของกำไรสุทธิ

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2) กรณีผู้ได้รับเงินปันผลไม่ได้รับเครดิตภาษี เนื่องจากจ่ายจาก
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2.1) กำไรสุทธิของกิจการที่ได้รับยกเว้นภาษีเงินได้นิติบุคคล
                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2.2) เงินปันผลหรือเงินส่วนแบ่งของกำไรที่ได้รับยกเว้นไม่ต้องนำมารวม
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                คำนวณเป็นรายได้เพื่อเสียภาษีเงินได้นิติบุคคล
                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2.3) กำไรสุทธิส่วนที่ได้หักผลขาดทุนสุทธิยกมาไม่เกิน 5 ปี
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                ก่อนรอยระยะเวลาบัญชีปีปัจจุบัน
                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2.4) กำไรที่รับรู้ทางบัญชีโดยวิธีส่วนได้ส่วนเสีย (equity method)

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                (2.5) อื่น ๆ (ระบุ) ...................

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">
                                
                                5. การจ่ายเงินได้ที่ต้องหักภาษี ณ ที่จ่าย ตามคำสั่งกรมสรรพากรที่ออกตามมาตรา
                                <br />
                              
                                &nbsp;&nbsp;&nbsp;
                                3 เตรส เช่น รางวัล ส่วนลดหรือประโยชน์ใด ๆ เนื่องจากการส่งเสริมการขาย รางวัล
                                <br />
                               
                                &nbsp;&nbsp;&nbsp;
                                ในการประกวด การแข่งขัน การชิงโชค ค่าแสดงของนักแสดงสาธารณะ ค่าจ้าง
                                <br />

                                &nbsp;&nbsp;&nbsp;
                                ทำของ ค่าโฆษณา ค่าเช่า ค่าขนส่ง ค่าบริการ ค่าเบี้ยประกันวินาศภัย ฯลฯ

                            </td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="border-top-style: hidden;">6. อื่น ๆ (ระบุ)......................................</td>
                            <td class="auto-style4"></td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="text-align: right;" colspan="2">
                                <h4 style="margin: 0px">
                                    รวมเงินที่จ่ายและภาษีที่หักนำส่ง

                                </h4>
                            </td>
                            <td class="auto-style2"></td>
                            <td class="dotted"></td>
                            <td class="auto-style1"></td>
                            <td class="dotted"></td>
                        </tr>

                        <tr>
                            <td style="margin: 0px; padding: 0px;" colspan="6">
                                <table style="border-collapse: collapse; width: 100%;">
                                    <tr>
                                        <td style="width: 40%; text-align: right;">
                                            <h4 style="margin: 0px 0px 0px 0px;">
                                                รวมเงินภาษีที่หักนำส่ง (ตัวอักษร)
                                            </h4>    
                                        </td>
                                        <td style="padding: 0px;">
                                            <div class="rcorners" style="margin: 0px 0px 0px 0px;">

                                            </div>
                                        </td>
                                    </tr>
                                </table>



                            </td>
                        </tr>
                    </table>
                </div>

                <!-- ส่วนที่ 6 -->
                <div class="div-rcorners">
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td>
                                <h3 style="margin: 0px; padding: 0px;">เงินที่จ่ายเข้า

                                </h3>
                            </td>
                            <td style="text-align: right;">กบข./กสจ./กองทุนสงเคราะห์ครูโรงเรียนเอกชน
                            </td>
                            <td>.........บาท
                            </td>
                            <td style="text-align: right;">กองทุนประกันสังคม
                            </td>
                            <td>.........บาท
                            </td>
                            <td style="text-align: right;">กองทุนสำรองเลี้ยงชีพ
                            </td>
                            <td>.........บาท
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- ส่วนที่ 7 -->
                <div class="div-rcorners">
                    <table style="border-collapse: collapse; width: 100%;">
                        <tr>
                            <td>
                                <h3 style="margin: 0px;">ผู้จ่ายเงิน

                                </h3>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox8" runat="server" Text="(1) หัก ณ ที่จ่าย" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox9" runat="server" Text="(2) ออกให้ตลอดไป" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox10" runat="server" Text="(3) ออกให้ครั้งเดียว" />
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox11" runat="server" Text="(4) อื่น ๆ (ระบุ)..................." />
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- ส่วนที่ 8 -->
                <table style="width: 100%; border-collapse: collapse;">
                    <tr>
                        <td style="padding: 0; margin: 0; width: 45%;">
                            <div class="div-rcorners">
                                <table style="width: 100%; border-collapse: collapse;">
                                    <tr>
                                        <td>
                                            <h3 style="margin: 0px;">คำเตือน
                                            </h3>

                                        </td>
                                        <td>ผู้มีหน้าที่ออกหนังสือรับรองการหักภาษี ณ ที่จ่าย
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>ฝ่าฝืนไม่ปฏิบัติตามมาตรา 50 ทวิ แห่งประมวล
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>รัษฎากร ต้องรับโทษทางอาญาจามมาตรา 35
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>แห่งประมวลรัษฎากร
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                        <td style="padding: 0; margin: 0; width: 55%;">
                            <div class="div-rcorners">
                                <table style="width: 100%; border-collapse: collapse; text-align: center;">
                                    <tr>
                                        <td>
                                            ขอรับรองว่าข้อความและตัวเลขดังกล่าวข้างต้นถูกต้องตรงกับความจริงทุกประการ
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>ลงชื่อ..........................................ผู้จ่ายเงิน
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>................./................./.................
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>(วัน เดือน ปี ที่ออกหนังสือรับรองฯ)
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                </table>

            </div>

            <!-- ส่วน Footer -->
            <table style="width: 100%; border-collapse: collapse;">
                <tr>
                    <td>
                        <span><strong>หมายเหตุ</strong> เลขประจำตัวผู้เสียภาษีอากร (13 หลัก)* หมายถึง</span>

                    </td>
                    <td>
                        1. กรณีบุคคลธรรมดาไทย ให้ใช้เลขบัตรประจำตัวประชาชนของกรมการปกครอง
                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                    <td>
                        2. กรณีนิติบุคคล ให้ใช้เลขทะเบียนนิติบุคคลของกรมพัฒนาธุระกิจการค้า
                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                    <td>
                        3. กรณีอื่น ๆ นอกเหนือจาก 1. และ 2. ให้ใช้เลขประจำตัวผู้เสียภาษีอากร (13 หลัก) ของกรมสรรพากร
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>