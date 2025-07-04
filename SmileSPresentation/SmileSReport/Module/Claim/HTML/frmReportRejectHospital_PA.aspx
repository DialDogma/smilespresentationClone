<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportRejectHospital_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportHospitalReject_PA" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic" />
    <link href="https://fonts.googleapis.com/css2?family=Sarabun:ital,wght@0,300;0,400;0,500;0,600;0,700;1,300;1,400;1,600&display=swap" rel="stylesheet" />

    <style>
        .content {
            padding-left: 20px;
            overflow: auto;
            background-color: #fff;
        }


        .page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
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
            thead {
                display: table-header-group;
            }
            tfoot {
                display: table-footer-group;
            }
        }

        .header, .footer {
            width: 100%;
            /*position: fixed;*/
            left: 0;
        }

        .header img, .footer img {
            width: 100%;
            height: 100%;
            object-fit: cover; /* เพื่อให้ภาพเต็มพื้นที่ */
        }

        tr, td {
            font-family: "Sarabun", sans-serif;
            font-size: 10pt;
            font-weight: 300;
            word-wrap: break-word;
            vertical-align: middle;
            padding-bottom: 5px;
            text-align: justify
        }

        .td-line-height {
            line-height: 1.7;
        }

        .tr-nomal {
            width: 12.5%;
        }

        .tr-right{
            text-align: right;
        }

        .td-input-pd-r {
            padding-right: 5px;
        }

        .custom-checkbox {
            position: relative;
            display: inline-block;
            width: 20px;
            height: 20px;
        }

        .custom-checkbox input {
            /*position: absolute;*/
            opacity: 0;
            width: 0;
            height: 0;
        }
        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 20px;
            width: 20px;
            background-color: #fff;
            border: 1px solid #000;
            border-radius: unset;
        }
        
    </style>
    <title></title>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div class="page">
            <!-- ส่วน Header -->
            <%--<div class="header">
                <img src="../../../Image/headpdfform.jpg" alt="Header Image" />
            </div>--%>

            <!-- ส่วน Body -->
            <div class="content">
                <table width="100%" border="0" style="border-collapse: collapse">
                    <thead>
                        <tr>
                            <td colspan="8" class="tr-nomal">
                                <img src="../../../Image/headpdfform.jpg" style="width:100%;object-fit: cover;top: 0;"/>
                            </td>
                        </tr>
                    </thead>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <%--row 1--%>
                    <tr>  
                        <td colspan="5" class="tr-nomal">&nbsp;</td>
                        <td class="tr-nomal tr-right">วันที่&nbsp;</td>
                        <td colspan="2" class="tr-nomal">
                            <asp:Label runat="server" ID="lblPrintFromDate"></asp:Label>
                        </td>
                    </tr>
                    <%--row 2--%>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <%--row 3--%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td class="tr-nomal" style="font-weight: 500">เรื่อง </td>
                        <td colspan="6" class="tr-nomal"><asp:Label runat="server" ID="lblTitle" Text="การชำระคืนค่ารักษาพยาบาล"></asp:Label></td>
                    </tr>
                    <%--row 4--%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td class="tr-nomal" style="font-weight: 500">เรียน </td>
                        <td colspan="6" class="tr-nomal">ฝ่ายบัญชี&nbsp;<asp:Label runat="server" ID="lblHospitalName"></asp:Label></td>
                    </tr>
                    <%--row 5--%>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <%--row 6--%>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <%--<tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>--%>
                    <%--title 1--%>
                    <tr>
                        
                        <td colspan="8" class="tr-nomal td-line-height">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            ตามที่ท่านได้ส่งเอกสารเพื่อวางบิลเรียกเก็บค่ารักษาพยาบาล ของ <asp:Label runat="server" ID="lblCustomerName"></asp:Label> ซึ่งเข้ารับ
                            การรักษาที่ โรงพยาบาลของท่าน เมื่อวันที่  <asp:Label runat="server" ID="lblAdmitDate" Text=" 01-01-2599"></asp:Label>  ค่ารักษาเรียกเก็บรวม
                            <asp:Label runat="server" ID="lblAmount"></asp:Label>
                            โดยบริษัท ได้รับเอกสารเรียกเก็บ เมื่อวันที่ <asp:Label runat="server" ID="lblReceiveDocDate"></asp:Label> นั้น
                        </td>
                    </tr>

                    <%--title 2--%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td colspan="7" class="tr-nomal">
                            บริษัทฯ ขอเรียนให้ท่านทราบว่า บริษัทฯ ไม่สามารถชำระเงินตามจำนวน ที่โรงพยาบาลเรียกเก็บได้เนื่องจาก
                        </td>
                    </tr>

                    <%--title 3--%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td colspan="7" class="tr-nomal">
                            1. ตามเงื่อนไขกรมธรรม์ ไม่คุ้มครองค่าใช้จ่ายจากการรักษาพยาบาล หรือความเสียหาย ที่เกิดจากการบาดเจ็บ 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                             (รวมทั้งภาวะแทรกซ้อน) อาการหรือภาวะความผิดปกติ ที่เกิดจาก
                        </td>
                    </tr>

                    <%--title 4--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การตรวจวินิจฉัย การบาดเจ็บหรือการเจ็บป่วยเพื่อหาสาเหตุใดๆที่ไม่เกี่ยวข้องโดยตรงกับการรักษาในครั้งนี้ 
                        </td>
                    </tr>

                    <%--title 5--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การฆ่าตัวตาย หรือพยายามฆ่าตัวตาย หรือทำร้ายร่างกายตนเอง หรือเจตนาก่อให้เกิดความบาดเจ็บแก่ตนเอง  
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                           หรือยินยอมให้ผู้อื่นกระทำไม่ว่าจะอยู่ในระหว่างวิกลจริตหรือไม่ก็ตาม ทั้งนี้รวมถึงอุบัติเหตุจากการที่ผู้เอาประกันภัย กิน ดื่ม หรือ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                           ฉีดยา หรือสารมีพิษเข้าร่างกาย การใช้ยาเกินกว่าที่แพทย์สั่ง หรือเกิดขึ้นจากการทะเลาะวิวาท 
                        </td>
                    </tr>

                    <%--title 6--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การตรวจรักษาโรคที่เกี่ยวเนื่องกับภาวะทางจิตใจ โรคประสาทหรือวิกลจริต หรือทางพฤติกรรมหรือความผิด
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ปกติทางบุคลิกภาพ รวมถึงสภาวะสมาธิสั้น ออธิสซึม เครียด ความผิดปกติของการกิน หรือความวิตกกังวล หรือการติดสุรา การ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                             เสพ หรือติดยาเสพติด กามโรค เอดส์ โรคทางเพศสัมพันธ์ โรคทางพันธุกรรม หรือความบกพร่องของร่างกาย ที่เป็นมาแต่กำเนิด 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                           หรือการเจ็บป่วย อันเนื่องมาจากโรคดังกล่าว หรืออุบัติเหตุจากการใช้ยา
                        </td>
                    </tr>

                    <%--title 7--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การบริการระหว่างการรักษาตัวแบบพักฟื้น การพักผ่อน หรืออนามัย หรือการรักษาโดยวิธีให้พักอยู่เฉยๆ เช่น 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            อาการอ่อนเพลีย ภาวะกังวลใจ ปลายประสาทอักเสบ นอนไม่หลับ การตรวจร่างกายหรือสุขภาพทั่วไป หรือการพักรักษา เพื่อทำ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            กายภาพบำบัด
                        </td>
                    </tr>

                    <%--title 8--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การบริการเกี่ยวกับการตั้งครรภ์ รวมทั้งการคลอดบุตร การแท้งบุตร หรืออาการแทรกซ้อนอันเนื่องจากสาเหตุ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ดังกล่าว การทำหรือการรักษาเกี่ยวกับการคุมกำเนิด การรักษาเกี่ยวกับความสามารถในการให้กำเนิด หรือการบริการ หรือการ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            รักษาเด็ก แรกเกิด การทำหมัน หรือการวิจัยสาเหตุของหมัน
                        </td>
                    </tr>

                    <%--title 9--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การรักษาความผิดปกติของการเจริญเติบโตและพัฒนาการ เช่น การเจริญเติบโตช้า น้ำหนักน้อย ภาวะการ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            พัฒนาทางสมองช้า รวมถึงภาวะฮอร์โมนผิดปกติที่เกี่ยวกับการเจริญเติบโต และพัฒนาการทางด้านสมอง รวมถึงอาการ โดย
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ธรรมชาติ เช่น รอยเหี่ยวย่น ภาวะหมดฮอร์โมนเพศ (วัยทอง) หรือภาวะเข้าสู่วัยเจริญพันธุ์ก่อนวัยอันควร(Prepuberty)
                        </td>
                    </tr>

                    <%--title 10--%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การตรวจรักษาเหงือกอักเสบ การตรวจรักษาหรือการทำศัลยกรรมเกี่ยวกับฟัน หรือเหงือก หรือการตรวจรักษา
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ด้านทันตกรรมไม่ว่าลักษณะใดๆ 
                        </td>
                    </tr>

                     <%-- title 11 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td  colspan="7" class="tr-nomal">
                            การตรวจรักษาสายตา การทำเลสิค การตรวจสอบการสะท้อนภาพของตา หรือการประกอบแว่นตา หรือ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            คอนแทคเลนส์ เครื่องช่วยให้ได้ยิน เครื่องกระตุ้นหัวใจ และการผ่าตัดเพื่อช่วยให้สวยงาม ศัลยกรรมตกแต่ง 
                        </td>
                    </tr>
                    
                    <%--Next Page--%>
                    <%--<tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>--%>

                    <%-- title 12 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            เอดส์ (AIDS) โรคอื่นๆที่มีสัมพันธ์กับเอดส์ และโรคเพศสัมพันธ์อื่นๆ
                        </td>
                    </tr>

                    <%-- title 13 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การเสียชีวิต หรือทุพพลภาพถาวร เนื่องจากอาหารเป็นพิษ การปวดหลัง อันมีสาเหตุจากหมอนรองกระดูกทับ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            เส้นประสาท กระดูกสันหลังเลื่อน (Spondylolithesis) กระดูกสันหลังเสื่อม (Dcgeneratation หรือ Spodylosis) กระดูกสันหลัง  
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            อักเสบ(Spondylolysis)
                        </td>
                    </tr>

                    <%-- title 14 --%><tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            โรคผิวหนังประเภทสิว ฝ้า การรักษาเพื่อความสวยงาม รวมถึงการเจ็บป่วยอันมีผลจากการกระทำเพื่อความ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            สวยงาม การบริการการผ่าตัดอันมีลักษณะเลือกได้ ค่าเวชภัณฑ์ และค่าใช้จ่ายอื่นๆ เพื่อความสวยงาม การลดความอ้วน หรือ 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ลดน้ำหนัก
                        </td>
                    </tr>

                    <%-- title 15 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การตรวจรักษา หรือการป้องกัน การใช้ยา หรือสารต่างๆ เพื่อชะลอการเสื่อมของวัย หรือการให้ฮอร์โมน ทดแทน  
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ในวัยใกล้หมดหรือหมดระดู การเสื่อมสมรรถภาพทางเพศ การรักษาความผิดปกติทางเพศ และการแปลงเพศ
                        </td>
                    </tr>

                    <%-- title 16 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การรักษาโดยใช้น้ำแร่ วารีบำบัด ธรรมชาติบำบัด แผนโบราณ หรือวิธีการฝังเข็ม รวมถึงแพทย์ทางเลือก
                        </td>
                    </tr>

                    <%-- title 17 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การเข้ารับการตรวจสุขภาพไม่ว่าจะอยู่ในโรงพยาบาลหรือไม่ก็ตาม
                        </td>
                    </tr>

                    <%-- title 18 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การตรวจรักษาที่ยังอยู่ในระหว่างทดลอง การตรวจหรือรักษาโรค หรืออาการหยุดหายใจขณะหลับ การตรวจ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            หรือการรักษาความผิดปกติของการนอนหลับ การนอนกรน
                        </td>
                    </tr>

                    <%-- title 19 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การบริการอื่นๆ อันมิใช่การรักษาโรค และอื่นๆ ที่คล้ายคลึงกัน การจัดซื้อหรือการใช้เครื่องเหนี่ยวรั้งหรือค้ำจุน 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            เครื่องมือหรืออุปกรณ์
                        </td>
                    </tr>

                    <%-- title 20 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การรักษาตัวทางการแพทย์ของผู้เอาประกันภัยที่มีอยู่แล้ว หรือสืบเนื่องจากการเจ็บป่วยของผู้เอาประกันภัยก่อน
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            วันเริ่มต้นการประกันภัย หรือก่อนวันที่มีสิทธิได้รับความคุ้มครอง (มีอาการ หรือได้รับการวินิจฉัย หรือเคยได้รับการตรวจรักษาอยู่
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            แล้วก่อนทำประกัน)
                        </td>
                    </tr>

                    <%-- title 21 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การกระทำของผู้เอาประกันภัยขณะอยู่ภายใต้ฤทธิ์สุรา สารเสพติด หรือยาเสพติดให้โทษ 
                        </td>
                    </tr>

                    <%-- title 22 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            การล่าสัตว์ การเข้าแข่งขันรถ หรือเรือทุกชนิด การเข้าแข่งม้า การแข่งขันที่ใช้ความเร็ว การเล่นสกีน้ำ การเล่น
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            สเก็ตทุกประเภท การกระโดดร่ม การเล่นโปโล การชกมวยอาชีพ การเล่นกีฬาอาชีพ รวมถึงการเล่นกีฬาหรือการกระทำเสี่ยงตาย
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            อื่นๆ เช่น บันจี้จัมพ์ เป็นต้น
                        </td>
                    </tr>

                    <%-- title 23 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            อื่นๆ (ระบุ) 
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ..........................................................................................................................................................................................................
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ..........................................................................................................................................................................................................
                        </td>
                    </tr>

                    <%--title 24--%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td colspan="7" class="tr-nomal">
                            2. ตามเงื่อนไขกรมธรรม์ อื่นๆ ดังนี้
                        </td>
                    </tr>

                    <%-- title 25 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            อยู่ในเงื่อนไขข้อยกเว้น <a style="color:black;font-weight:300;text-decoration:underline;">ไม่คุ้มครองการรักษาอาการ หรือ โรคที่อยู่ในระยะเวลารอคอย 30 วัน</a>
                        </td>
                    </tr>

                    <%-- title 26 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            อยู่ในเงื่อนไขข้อยกเว้น <a style="color:black;font-weight:300;text-decoration:underline;">ไม่คุ้มครองการรักษาอาการ หรือ โรคที่อยู่ในระยะเวลารอคอย 120 วัน</a>
                        </td>
                    </tr>

                    <%-- title 27 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            เกินสิทธิ์ความคุ้มครอง
                        </td>
                    </tr>

                    <%-- title 28 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            เกินวงเงินความคุ้มครอง 
                        </td>
                    </tr>

                    <%-- title 29 --%>
                    <tr>
                        <td class="tr-nomal tr-right td-input-pd-r">
                            <label class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </label>
                        </td>
                        <td colspan="7" class="tr-nomal">
                            อื่น ๆ (ระบุ) .............................................................................................................................................................
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">
                            ............................................................................................................................................................................................................
                        </td>
                    </tr>

                    <%--Next Page--%>
                    <%--<tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                --%>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <%-- title 30 --%>
                    <tr>
                        <td class="tr-nomal">&nbsp;</td>
                        <td colspan="7" class="tr-nomal">
                            3. จำนวนเงินที่ไม่สามารถให้ความคุ้มครองได้ <asp:Label runat="server" ID="lblUnCoverAmount"></asp:Label> 
                        </td>
                    </tr>

                    <%--Signature From--%>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr> 
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr> 
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr> 
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr> 

                    <tr>
                        <td colspan="8" class="tr-nomal">
                            จึงเรียนมาเพื่อทราบ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal" style="text-align: center">
                            ขอแสดงความนับถือ
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal" style="text-align: center">
                             ( นายวีรภัทร เอมอิ่ม )
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal" style="text-align: center">
                            ผู้ช่วยผู้จัดการแผนกสินไหมโรงพยาบาล
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal" style="text-align: center">
                             โทร.085 695 8527 , 1434   Email :  teamclaim4.isc@gmail.com
                        </td>
                    </tr>
                    <tr>
                       <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                       <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="8" class="tr-nomal">&nbsp;</td>
                    </tr>
                    <tfoot>
                        <tr>
                            <td colspan="8" class="tr-nomal">
                                <img src="../../../Image/endpdfform.jpg" style="width:100%;object-fit: cover;bottom: 0;"/>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>

            <!-- ส่วน Footer -->
            <%--<div class="footer">
                <img src="../../../Image/endpdfform.jpg" alt="Footer Image" />
            </div>--%>
        </div>
    </form>
</body>
</html>