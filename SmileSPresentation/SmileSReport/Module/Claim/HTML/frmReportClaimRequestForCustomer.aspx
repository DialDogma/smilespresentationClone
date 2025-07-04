<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportClaimRequestForCustomer.aspx.cs" Inherits="Web.frmReportClaimRequestForCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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
            font: 8pt arial, tahoma, sans-serif;
            /*padding:4px;*/
            /*font: 9pt, tahoma;*/
            border-color:black;

        }

        .padding {
            padding:10px;
        }
        
    </style>
</head>
<body onload="window.print()" >


    <form id="form1" runat="server">
        <div class="page" style="margin-top:60px; margin-left:20px; margin-right:20px">

            <table style="width: 100%; border-collapse: collapse;" border="1">
                <tr>
                    <td>
                        <table style="width:100%">
                           
                            <tr>
                                <td style="width:20%" rowspan="4">
                                    <img src="../../../Image/23192995_1742808349062749_1900743945_n.jpg" width="120px" height="80px" />
                                </td>
                                <td></td>
                                
                            </tr>
                             <tr>
                                 <td></td>
                            </tr>
                             <tr>
                                 <td >
                                    
                                 </td>
                            </tr>
                            <tr>
                                <td style="font-size:smaller; text-align:right" >
                                    <asp:Label ID="lblClaimID" runat="server" Text="CL5903010994"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </td>
                    <td style="width: 45%">
                        <table style="width:100%; text-align:center">
                            <tr>
                                <td style="font-weight:bold">แบบเรียกร้องค่าสินไหมทดแทน</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblProvince" runat="server" Text="นนทบุรี"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPeriod" runat="server" Text="01/10/2559"></asp:Label> 
                                    /
                                    <asp:Label ID="lblAgentName" runat="server" Text="คุณสสัสดิการ ประกันสุขภาพครอบครัว"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblProduct" runat="server" Text="502-Gold"></asp:Label>
                                    <asp:Label ID="lblCompany" runat="server" Text="บริษัท ชับสามัคคีประกันภัย จำกัด (มหาชน)"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden;"  border="1">
                <tr style="">
                    <td style="border-right-style:hidden;" class="padding">
                        <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" /> &nbsp;บัตรประกันอุบัติเหตุและสุขภาพ

                    </td>
                    <td style="border-right-style:hidden;" class="padding">
                       <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" /> &nbsp; ประกันภัยชดเชยรายได้

                    </td>
                    <td style="" class="padding">
                        <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; ประกันภัยโรคมะเร็ง

                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="border-right-style:hidden;" class="padding">

                        ชื่อสมาชิกผู้เอาประกัน : <asp:Label ID="lblCustName" runat="server" Text="นายณัฐวัตน์  เกตุวิชิต"></asp:Label>

                    </td>
                    <td style="width:15%">
                        อายุ : <asp:Label ID="lblAge" runat="server" Text="31"></asp:Label> ปี

                    </td>
                    <td style="width:35%" class="padding">
                        เลขที่บัตรสมาชิก : <asp:Label ID="lblAppID" runat="server" Text="9229992"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td class="padding">
                        ที่อยู่ : <%--เลขที่
                        <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>
                        ซอย <asp:Label ID="lblSoi" runat="server" Text="ราชพฤกษ์ 25"></asp:Label>
                        อาคาร  <asp:Label ID="lblBuilldingName" runat="server" Text="-"></asp:Label>
                        หมู่บ้าน  <asp:Label ID="lblVillingName" runat="server" Text="กฤษดานคร"></asp:Label>
                        หมู่  <asp:Label ID="lblMoo" runat="server" Text="5"></asp:Label>
                        ถนน  <asp:Label ID="lblRoad" runat="server" Text="แจ้งวัฒนะ"></asp:Label>
                        ตำบล <asp:Label ID="lblSubDistrict" runat="server" Text="บางตลาด"></asp:Label>
                        อำเภอ <asp:Label ID="lblDistrict" runat="server" Text="ปากเกร็ด"></asp:Label>
                        จังหวัด <asp:Label ID="lblProvince" runat="server" Text="นนทบุรี"></asp:Label>
                        <asp:Label ID="lblZipcode" runat="server" Text="11120"></asp:Label>--%>
                        <asp:Label ID="lblAddressDetail" runat="server" Text="เลขที่ 53/837 ซอย รายพฤกษ์ 25 อาคาร - หมู่บ้าน กฤษดานครหมู่ 5 ถนนแจ้งวัฒนะ ตำบล บางตลาด อำเภอ ปากเกร็ด จังหวัดนนทบุรี 11120"></asp:Label>

                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td class="padding">
                        อาชีพ : <asp:Label ID="lblOccapation" runat="server" Text="พนักงานบริษัท"></asp:Label>
                    </td>
                    <td style="width:35%" class="padding">
                        โทรศัพท์ : <asp:Label ID="lblPhone" runat="server" Text="0814142623"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td class="padding">
                        ชื่อและที่อยู่นายจ้าง : <asp:Label ID="lblworkname" runat="server" Text="บริษัท มาตรฐาน ไอ.เอส.ซี จำกัด"></asp:Label>
                    </td>
                    <td style="width:35%" class="padding">
                        โทรศัพท์ : <asp:Label ID="lblworkPhone" runat="server" Text="-"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="width:30%" class="padding">
                        วันที่เกิดอุบัติเหตุ : <asp:Label ID="lblDateHappen" runat="server" Text="26/3/2559 0:00:00"></asp:Label>
                    </td>
                    <td class="padding">
                        วันที่เข้ารับการรักษา : <asp:Label ID="lbldateIn" runat="server" Text="26/3/2559 0:00:00"></asp:Label> - 
                        <asp:Label ID="lblDateOut" runat="server" Text="26/3/2559 0:00:00"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td class="padding">
                        สถานพยาบาล : 
                        <asp:Label ID="lblHospital" runat="server" Text="โรงพยาบาลโอเวอร์บรุ๊ค"></asp:Label>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="width:30%" class="padding">
                        ชื่อบัญชี : <asp:Label ID="lblAccountName" runat="server" Text="นายณัฐวัฒน์ เกตุวิชิต"></asp:Label>
                    </td>
                    <td class="padding">
                        ธนาคาร : <asp:Label ID="lblBank" runat="server" Text="กรุงไทย"></asp:Label>
                    </td>
                    <td style="width:35%" >

                        <table style="width:100%">
                            <tr>
                                <td style="width:70%" >
                                     สาขา : <asp:Label ID="lblBranch" runat="server" Text="สำนักงานใหญ่"></asp:Label>
                                </td>
                                <td style="text-align:right">สำรองจ่าย</td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="width:30%" class="padding">

                        เลขที่บัญชี : <asp:Label ID="lblAccountNo" runat="server" Text="5040825498"></asp:Label>
                    </td>
                    <td >

                        <table style="width:100%">
                            <tr>
                                <td style="width:33.3%; padding:4px" >
                                    บัญชีประเภท :</td>
                                <td style="width:33.3%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; ออมทรัพย์

                                </td>
                                <td style="width:33.4%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; กระแสรายวัน

                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="width:30%; align-items: flex-start;" class="padding">เอกสารประกอบ : <br /><br /><br /><br /></td>
                    <td style="padding-left:10px">
                        <table style="width:100%">
                            <tr>
                                <td style="width:30%">
                                    ใบเสร็จรับเงิน : 
                                    <asp:Label ID="lblnet" runat="server" Text="0"></asp:Label> บาท
                                </td>
                                <td style="width:20%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; ต้นฉบับ

                                </td>
                                <td style="width:20%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; สำเนา

                                </td>
                                <td>
                                    (เบิก  <asp:Label ID="lblPay" runat="server" Text="0"></asp:Label> บาท)
                                </td>
                            </tr>
                            <tr>
                                <td style="width:30%">
                                    ใบรับรองแพทย์ : 
                                   
                                </td>
                                <td style="width:20%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; ต้นฉบับ

                                </td>
                                <td style="width:20%">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; สำเนา

                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width:25%">
                                    อื่น ๆ : 
                                   
                                </td>
                                <td style="width:20%"></td>
                                <td style="width:20%"></td>
                                <td></td>
                            </tr>
                        </table>

                    </td>
                </tr>

            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1" >
                <tr>
                    <td style="border-bottom-style:hidden" class="padding"> 
                        ระบุการเจ็บป่วย / บาดเจ็บที่ได้รับ (ในกรณีที่บาดเจ็บให้บรรยายการเกิดอุบัติเหตุโดยละเอียด)
                    </td>
                    <td style="width:35%" class="padding">
                        บส : <asp:Label ID="lblBSID" runat="server" Text="BH-601000000"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="border-bottom-style:hidden; border-right-style:hidden; padding:8px">
                        ประเภทเคลม : <asp:Label ID="lblClaimType" runat="server" Text="OPD ทั่วไป(H)"></asp:Label>

                    </td>
                    <td style="border-bottom-style:hidden; padding:8px" >
                        <asp:Label ID="lblOPDNo" runat="server" Text="2559/1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="border-bottom-style:hidden; border-right-style:hidden; padding:8px" >
                        อาการสำคัญ :  <asp:Label ID="lblChiefComplain" runat="server" Text="ไข้ + ไอ"></asp:Label>

                    </td>
                    <td style="border-bottom-style:hidden;" ></td>
                </tr>

                <tr>
                    <td style="border-bottom-style:hidden; border-right-style:hidden; padding:8px" >
                        Diagnosis :  <asp:Label ID="lblICD10_Eng" runat="server" Text="Acute pharyngitis"></asp:Label>

                    </td>
                    <td style="border-bottom-style:hidden;" ></td>
                </tr>

                <tr>
                    <td style="border-right-style:hidden; padding:8px" >
                        Diagnosis :  <asp:Label ID="lblICD10_Thai" runat="server" Text="คออักเสบเฉียบพลัน"></asp:Label>

                    </td>
                    <td style=" text-align:right" >
                        <asp:Label ID="lblTransfer" runat="server" Text="*โอนแยก*"></asp:Label>
                    </td>
                </tr>

            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden" border="1">
                <tr>
                    <td style="padding:1px">
                        <table style="width:100%">
                            <tr>
                                <td colspan="4" class="padding">ท่านกำลังเรียกร้องค่าทดแทนอื่นใดเนื่องจากการบาดเจ็บคราวนี้ด้วยหรือไม่</td>
                            </tr>
                            <tr>
                                <td class="padding"></td>
                                <td style="text-align:center" class="padding">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; เรียกร้อง

                                </td>
                                <td style="text-align:center" class="padding">
                                    <img src="../../../Image/23146157_10204155314440111_1876582272_n.jpg" width="12px" />&nbsp; ไม่เรียกร้อง

                                </td>
                                <td class="padding"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:35%; padding:1px;">
                        <table style="width:100%; padding:0px;" border="0">
                            <tr>
                                <td colspan="4" class="padding">ถ้าเรียกร้อง ชื่อบริษัท</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align:center" class="padding"><br /></td>
                                <td style="text-align:center" class="padding"></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table style="width: 100%; border-collapse: collapse; border-top-style: hidden; border-bottom-style:hidden"  border="1">
                <tr>
                    <td style="text-align:center; border-bottom-style:hidden; font-weight:bold" class="padding">ใบมอบฉันทะ</td>
                </tr>
                 <tr>
                    <td style="border-bottom-style:hidden;" class="padding">
                         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<div class="ql-block" data-block-id="block-d7a0a767-da73-49ce-b3a3-767a547bef0b">
                             &nbsp;&nbsp;&nbsp;&nbsp;
                             ข้าพเจ้าขอมอบฉันทะให้โรงพยาบาล แพทย์ หรือผู้อื่นใด ซึ่งได้กระทำการตรวจและรักษาข้าพเจ้า มีอำนาจแจ้งหรือให้ข้อมูลของข้าพเจ้า ต่อบริษัทหรือผู้แทนอื่นใดที่ได้รับมอบอำนาจถึงการเจ็บป่วย การได้รับบาดเจ็บ ประวัติทางการแพทย์ การปรึกษา ใบสั่งยา หรือการรักษา และสำเนาบันทึกของโรงพยาบาลหรือการแพทย์ของข้าพเจ้าได้ ตลอดจนเอกสารอื่นใดอันเกี่ยวข้องกับการเรียกร้องสินไหมทดแทนครั้งนี้ อนึ่งสำเนาของใบมอบฉันทะนี้ ให้ถือว่ามีผลใช้บังคับได้และสมบูรณ์เท่ากับต้นฉบับ</div>
&nbsp;</td>
                </tr>
                <tr>
                    <td style="border-bottom-style:hidden;" class="padding">
                        <br /><br />
                    </td>
                </tr>
            </table>
            

            <table border="1" style="border-collapse:collapse; border-top-style:hidden;  width:100%">
                <tr>
                    <td style="width:35.3%; text-align:center; border-right-style:hidden" class="padding">
                        ลงนาม................................................ผู้เรียกร้อง
                    </td>
                    <td style="width:31.3%; border-right-style:hidden" class="padding"></td>
                    <td style="width:33.4%; text-align:center" class="padding">
                        วันที่ ........./........./.....................
                    </td>
                </tr>
            </table>


            <table border="1" style="border-collapse:collapse; border-top-style:hidden;  width:100%">
                <tr>
                    <td class="padding">
                        สำคัญ :  แบบฟอร์มนี้ต้องกรอกและส่งบริษัทประกันโดยเร็วที่สุด อย่างช้าไม่เกิน 30 วัน นับจากวันที่ออกจาก
                        โรงพยาบาลเพื่อการจ่ายค่าสินไหมทดแทนได้รวดเร็ว โปรดแนบรายงานแพทย์ รายละเอียดการรักษาพยาบาล
                        และใบเสร็จค่ารักษาพยาบาลมาพร้อมแบบเรียกร้องนี้
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
