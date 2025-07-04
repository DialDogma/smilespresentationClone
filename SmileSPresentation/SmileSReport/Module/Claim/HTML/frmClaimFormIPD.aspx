<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClaimFormIPD.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.WebForm1" %>

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
            border-color: black;
        }

        .padding {
            padding: 3px;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
        }

        .border-right-hidden {
            border-right-style: hidden;
        }

        .border-bottom-hidden {
            border-bottom-style: hidden;
        }

        .border-top-hidden {
            border-top-style: hidden;
        }

        .border-bottom-dotted {
            border-bottom-style: dotted;
        }

        .border-bottom-width {
            border-bottom-width: 1px;
        }
    </style>

</head>
<body>


    <form id="form1" runat="server">


        <div class="page" style="vertical-align: top; page-break-after: always;">

            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: center; font-size: large; color: #437fd3; font-weight: bold">แบบฟอร์มคนไข้ใน และแฟ็กซ์เคลม ( IPD&FAX CLAIM FORM )
                        </td>
                    </tr>
                </table>

                <table style="border-collapse: collapse; width: 100%" border="1">
                    <tr>
                        <td style="width: 7%; text-align:center">ถึง </td>
                        <td style="width: 40%; " class="padding">บริษัท
                            <asp:Label ID="lbl0_company" runat="server" Text=""></asp:Label></td>
                        <td style="width: 7%; text-align:center" class="border-bottom-hidden">จาก</td>
                        <td class="padding">โรงพยาบาล
                            <asp:Label ID="lbl0_Hospital" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-top-style: hidden; text-align:center" class="padding">To </td>
                        <td class="padding">กรมธรรม์เลขที่
                            <asp:Label ID="lbl0_Policy" runat="server" Text=""></asp:Label></td>
                        <td style="text-align:center">From </td>
                        <td class="padding">
                            <table style="width: 100%; padding:-2px">
                                <tr>
                                    <td style="width: 15%">ห้อง 
                                        <asp:Label ID="lbl0_Room" runat="server" Text=""></asp:Label>

                                    </td>
                                    <td style="width: 42%">โทรศัพท์ 
                                        <asp:Label ID="lbl0_Tel" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 43%">โทรสาร 
                                        <asp:Label ID="lbl0_Fax" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-top-style: hidden"></td>
                        <td class="padding">เลขสมาชิก / ลำดับที่ 
                            <asp:Label ID="lbl0_MemberNo" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="border-top-style: hidden"></td>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 60%" class="padding">กรณีแฟ็กซ์เคลมชื่อผู้ส่ง 
                                        <asp:Label ID="lbl0_NameSendFax" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 40%">เวลาส่ง 
                                        <asp:Label ID="lbl0_TimeSend" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>

            <div style="margin-top: 5px">


                <table border="1" class="table">
                    <%-- ส่วนที่ 1 --%>
                    <tr>
                        <td>

                            <%-- ชื่อผู้เอาประกัน --%>
                            <table border="0">

                                <tr>
                                    <td style="font-weight: bold" colspan="7" class="padding">
                                        <u>ส่วนที่ 1 สำหรับผู้เอาประกัน</u>&nbsp;
                                            (Patient Details)
                                    </td>
                                </tr>
                                <tr style="border-top-style: hidden">
                                    <td style="border-right-style: hidden; border-bottom-style: hidden; width: 15%" class="padding">ชื่อ-นามสกุล/Name 
                                    </td>
                                    <td style="border-bottom-style: dotted; border-right-style: hidden; width: 35%; border-bottom-width: 1px">
                                        <asp:Label ID="lbl1_Name" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-right-style: hidden; border-bottom-style: hidden; width: 23%" class="padding">วัน / เดือน / ปีเกิด/Date of Birth</td>
                                    <td style="border-bottom-style: dotted; border-right-style: hidden; width: 13%; border-bottom-width: 1px">
                                        <asp:Label ID="lbl1_BrithDate" runat="server" Text=""></asp:Label>

                                    </td>
                                    <td style="border-bottom-style: hidden; border-right-style: hidden; width: 8%" class="padding">อายุ / Age </td>
                                    <td style="border-bottom-style: dotted; border-right-style: hidden; width: 8%; border-bottom-width: 1px">
                                        <asp:Label ID="lbl1_Age" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="border-bottom-style: hidden">ปี</td>
                                </tr>
                            </table>

                            <%-- บัตรประชาชน เลขที่ --%>
                            <table border="0" style="width: 100%">
                                <tr class="border-top-hidden">
                                    <td class="border-bottom-hidden border-right-hidden padding" style="width: 32%" >บัตรประชาชน / อื่น ๆ /ID Card - Please specify</td>
                                    <td class="border-right-hidden border-bottom-dotted border-bottom-width" style="width: 35%">
                                        <asp:Label ID="lbl1_IDCard" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="border-bottom-hidden border-right-hidden" style="width: 10%">เลขที่ (Card #)</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl1_Card" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table border="0" style="width: 100%">
                                <tr class="border-top-hidden">
                                    <td class="border-right-hidden border-bottom-hidden padding" style="width: 21%">ที่อยู่ปัจจุบัน / Current Address </td>
                                    <td class="border-right-hidden border-bottom-dotted border-bottom-width" style="width: 47%">
                                        <asp:Label ID="lbl1_AddressDetail" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="border-bottom-hidden border-right-hidden" style="width: 13%">โทรศัพท์ (Tel No.)</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl1_TelNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr class="border-top-hidden">
                                    <td style="width: 34%" class="padding">ท่านมีประกันภัยกับบริษัทอื่นหรือไม่(Other Insurers)</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 5%">
                                        <asp:Label ID="lbl1_OtherInsurers" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 26%">ถ้ามีโปรดระบุชื่อบริษัท (Please specify)</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 8%">
                                        <asp:Label ID="lbl1_specify" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 15%">กรมธรรม์ (Policy No.)</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl1_Policy" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 71%" class="padding">จากการเกิดเหตุครั้งนี้ท่าน / As a result of this incident, เคยรักษามาก่อนที่โรงพยาบาล / I have been treated at
                                    </td>
                                    <td class="border-bottom-width border-bottom-dotted">
                                        <asp:Label ID="lbl1_Treated" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%" class="padding">เมื่อวันที่ (Date of treatment)</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 15%">
                                        <asp:Label ID="lbl1_Dateoftreatment" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 40%">โดยได้ใช้สิทธิไปแล้วเป็นจำนวนเงิน /Cost of treatment given</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 15%">
                                        <asp:Label ID="lbl1_CostTreatmentGiven" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>บาท (Baht)</td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="font-weight: bold" class="padding"><u>หนังสือให้ความยินยอม</u></td>
                                </tr>
                                <tr>
                                    <td class="padding">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        ข้าพเจ้ายินยอมและตกลงให้บริษัทจ่ายค่ารักษาพยาบาลที่ข้าพเจ้าได้เข้ารับการรักษาครั้งนี้ โดยถือเสมือนหนึ่งบริษัทได้จ่ายชดเชยค่ารักษาพยาบาล
                                        ให้แก่ข้าพเจ้าโดยชอบแล้ว ตามข้อกำหนดและเงื่อนไขแห่งกรมธรรม์ประกันภัย ส่วนค่ารักษาพยาบาลใดๆ ที่อยู่นอกเหนือความคุ้มครองของกรมธรรม์ 
                                        ข้าพเจ้าจะเป็นผู้ชำระให้แก่สถานพยาบาลโดยตรงเอง ข้าพเจ้าขอมอบฉันทะให้โรงพยาบาล แพทย์ หรือบุคคลอื่นใดที่ได้ทำการตรวจและรักษาข้าพเจ้า หรือบุคคลในครอบครัว
                                        ของข้าพเจ้า มีอำนาจแจ้งข้อความใดๆ ที่เกี่ยวกับการบาดเจ็บ ประวัติทางการแพทย์ การปรึกษาโรค ใบสั่งยา หรือการรักษา และสำเนาเอกสารประวัติทางการแพทย์ของโรงพยาบาลทั้งหมดต่อ <b>บริษัทประกันภัยตามชื่อที่ปรากฎด้านบน</b>
                                        หรือผู้ที่ได้รับมอบหมายจากบริษัทฯ อนึ่ง สำเนาใบมอบฉันทะนี้ให้ถือว่ามีผลใช้บังคับได้เช่นเดียวกับต้นฉบับ
                                    </td>
                                </tr>
                                <tr>
                                    <td class="padding">I agree and authorize the Company to pay medical expenses to the hospital where I have been treated. This payment will be
                                        considered as reimbursement of medical expenses under the terms and conditions of the policy. I shall be responsible for and pay
                                         myself any medical expenses in excess of the company's liability to the hospital directly. I hereby authorize any hospital, doctor
                                         or other person who has attended to me or any member of my family to furnish the <b>Insurance Company</b> or its representatives
                                         with all information including medical history, consultations, prescriptions, treatment, and copies of all hospital and medical
                                         records that are related to this claim. I agree that a photocopy of this Authorization shall be considered as effective and valid as the original.
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 55%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%">ลงนาม</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 48%"></td>
                                                <td>ผู้เอาประกันภัย / ผู้ยินยอม/Insured</td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                    <td style="width: 45%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 13%">ลงนาม</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 48%"></td>
                                                <td>พยาน/Witness</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 55%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 10%; text-align: right">(</td>
                                                <td style="width: 48%; text-align:center">
                                                    <asp:Label ID="lbl1_NameInsured" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>)</td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td></td>
                                    <td style="width: 45%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 13%; text-align: right">(</td>
                                                <td style="width: 48%; text-align:center">
                                                    <asp:Label ID="lbl1_NameWitness" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>)</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 55%">

                                        <table style="width: 80%">
                                            <tr>
                                                <td style="width: 15%">วันที่/date</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl1_date_1" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 12%">เดือน/month</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 13%">
                                                    <asp:Label ID="lbl1_month_1" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 15%">พ.ศ./year</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl1_year_1" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td></td>

                                    <td style="width: 45%">

                                        <table style="width: 80%">
                                            <tr>
                                                <td style="width: 20%">วันที่/date</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl1_date_2" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 12%">เดือน/month</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 13%">
                                                    <asp:Label ID="lbl1_month_2" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 15%">พ.ศ./year</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl1_year_2" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>


                    <%-- ส่วนที่ 2 --%>
                    <tr>
                        <td>

                            <table style="width: 100%">
                                <tr>
                                    <td style="font-weight: bold; width: 80%" class="padding"><u>ส่วนที่ 2 สำหรับแพทย์ผู้ทำการตรวจรักษา</u>
                                        (To be completed by the attending doctor)
                                    </td>
                                    <td style="width: 20%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 3%; font-weight: bold">HN.</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                                    <asp:Label ID="lbl2_HN" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 3%; font-weight: bold">AN.</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                                    <asp:Label ID="lbl2_AN" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 60%" class="padding">
                                        <table style="width: 100%; margin-left:-2px">
                                            <tr>
                                                <td style="width: 12%">Admission date:</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                                    <asp:Label ID="lbl2_AdmissionDate" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 3%">Time:</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl2_Time" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 2%">BP=</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl2_BP" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 40%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 3%">T.=</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl2_T" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 3%">P.=</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl2_P" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 2%">R.=</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 10%">
                                                    <asp:Label ID="lbl2_R" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 16.5%" class="padding">Chief Complaint/duration :</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 72%">
                                        <asp:Label ID="lbl2_ChiefComplaint" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%" class="padding">Is the injury/illness directly related to an ACCIDENT?</td>
                                    <td style="width: 50%">
                                        <table style="width: 70%">
                                            <tr>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">Yes</td>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">No</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%" class="padding">Was the injury/illness influenced by the use of alcohol or drugs? </td>
                                    <td style="width: 50%">
                                        <table style="width: 70%">
                                            <tr>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">Yes</td>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">No</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 12%" class="padding">Initial diagnosis:</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 35%">
                                        <asp:Label ID="lbl2_InitialDiagnosis" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 23%">Underlying conditions / disease:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl2_UnderlyingConditionsdisease" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 16.5%" class="padding">Reason for admission :</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl2_ReasonForadmission" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 16%" class="padding">Provisional diagnosis:</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 40%">
                                        <asp:Label ID="lbl2_Provisionaldiagnosis" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 7%">ICD 10 :</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 40%">
                                        <asp:Label ID="lbl2_ICD10" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 12%" class="padding">Treatment plan:</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 44%">
                                        <asp:Label ID="lbl2_TreatmentPlan" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 6%">ICD 9:</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 41%">
                                        <asp:Label ID="lbl2_ICD9" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 17%" class="padding">Expected length of stay</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 15%">
                                        <asp:Label ID="lbl2_ExpectedLength" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 31%">day (s)&nbsp;&nbsp; Estimated cost of treatment (THB.)</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl2_EstimatedCostofTreatment" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 30%" class="padding">Previous treatment for similar illness</td>
                                    <td style="width: 70%">
                                        <table style="width: 60%">
                                            <tr>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">Yes</td>
                                                <td style="width: 2%; vertical-align: middle; text-align: right">
                                                    <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                                </td>
                                                <td style="width: 15%">No</td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 15%" class="padding">Date of consultation:</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 35%">
                                        <asp:Label ID="lbl2_DateofConsultation" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 6%">Hospital:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl2_Hospitalconsultation" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 14%" class="padding">Other comments:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl2_OtherComments" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">

                                        <table>
                                            <tr>
                                                <td style="width: 8%; margin-left:-2px;" >Doctor's Signature :</td>
                                                <td class="border-bottom-dotted border-bottom-width " style="width: 23%">
                                                    <asp:Label ID="lbl2_DoctorSignature" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 45%">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 28%">License Number:</td>
                                                <td class="border-bottom-dotted border-bottom-width">
                                                    <asp:Label ID="lbl2_LicenseNumber" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 45%">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: right; width: 19%">(</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 30%; text-align:center">
                                                    <asp:Label ID="lbl2_DoctorName" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 10%">)</td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td></td>
                                    <td style="width: 45%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 18%">Speciality:</td>
                                                <td class="border-bottom-dotted border-bottom-width">
                                                    <asp:Label ID="lbl2_Speciality" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                            </table>

                        </td>
                    </tr>



                    <%-- ส่วนที่ 3 --%>
                    <tr>
                        <td>

                            <table style="width: 100%">
                                <tr>
                                    <td style="font-weight: bold">
                                        <u>ส่วนที่ 3 สำหรับเจ้าหน้าที่บริษัท</u>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 2%; vertical-align: middle; text-align: right">
                                        <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                    </td>
                                    <td style="width: 55%">กรมธรรม์มีผลบังคับและมีค่ารักษาพยาบาล (รอพิจารณารับรองค่ารักษาพร้อมส่วนที่4)</td>
                                    <td></td>
                                    <td style="width: 12%"><img src="../../../Image/square.png" style="vertical-align: middle;" /> ถ้า Admin เกิน</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 6%">
                                        <asp:Label ID="lbl3_OverAdmit" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 15%">วัน&nbsp;&nbsp; กรุณาแจ้งอีกครั้ง</td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 2%; vertical-align: middle; text-align: right">
                                        <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                    </td>
                                    <td style="width: 51%">กรุณาส่งเคลมโดยวิธีปกติ (ส่งเอกสารเรียกร้องค่ารักษาโดยตรงกับบริษัท) เนื่องจาก</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl3_SendNormalClaim" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 2%; vertical-align: middle; text-align: right">
                                        <img src="../../../Image/square.png" style="vertical-align: middle;" />
                                    </td>
                                    <td style="width: 24%">บริษัทไม่สามารถให้บริการได้ เนื่องจาก</td>
                                    <td class="border-bottom-dotted border-bottom-width ">
                                        <asp:Label ID="lbl3_OutofService" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 3%">ลงชื่อ</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 60%">
                                                    <asp:Label ID="lbl3_EmployeeName" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>เจ้าหน้าที่สินไหม</td>
                                            </tr>
                                        </table>

                                    </td>

                                    <td>

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 8%">วันที่</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 60%">
                                                    <asp:Label ID="lbl3_Date" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 3%">เวลา</td>
                                                <td class="border-bottom-dotted border-bottom-width">
                                                    <asp:Label ID="lbl3_Time" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>

                </table>



            </div>

        </div>



        <div class="page" style="vertical-align: top; page-break-after: always;">

            <div style="font-weight: bold; font-size: 9pt">ส่วนที่ 4 ใบรายงานแพทย์ (Physician's Discharge Summary)</div>
            <div>
                <table class="table" border="1">
                    <tr>
                        <td style="width: 5%; text-align:center" class="padding">ถึง </td>
                        <td style="width: 35%" class="padding">บริษัท 
                            <asp:Label ID="lbl4_Company" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 5%; text-align:center" class="padding">จาก </td>
                        <td colspan="2">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 18%" class="padding">โรงพยาบาล</td>
                                    <td style="width: 30%">
                                        <asp:Label ID="lbl4_Hospital" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 8%">โทรสาร</td>
                                    <td style="width: 17%">
                                        <asp:Label ID="lbl4_Fax" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 6%">ผู้ส่ง</td>
                                    <td>
                                        <asp:Label ID="lbl4_SendFaxBy" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" class="padding">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 13%" >Patient's Name</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 42%">
                                        <asp:Label ID="lbl4_PatientName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 5%"></td>
                                    <td style="width: 3%">HN</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_HN" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 3%">AN</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_AN" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" class="padding">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 12%">Admission Date</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                        <asp:Label ID="lbl4_AdmissionDate" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 4%">Time</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 15%">
                                        <asp:Label ID="lbl4_TimeAdmission" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 11%">Discharge Date</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                        <asp:Label ID="lbl4_DischargeDate" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 4%">Time</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 15%">
                                        <asp:Label ID="lbl4_TimeDischarge" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" style="font-weight: bold; font-size: 10pt" class="padding">Please give full details relating to the treatment

                        </td>
                    </tr>

                    <tr>
                        <td colspan="5">

                            <table style="width: 100%">
                                <tr>
                                    <td style="font-weight: bold" class="padding">For Illness:</td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 35%" class="padding">1. The date you first saw the patient for this illness:
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width padding">
                                        <asp:Label ID="lbl4_DateFirstSawPatient" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 32%" class="padding">2. Chief complaint and duration of symptoms:
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_ChiefComplaint" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 47%" class="padding">3. In your opinion,how long should symptoms persist for this illness:
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_opinion_Symptoms" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="font-weight: bold" class="padding">For Accident:</td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%" class="padding">1. Date & time of accident
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 25%">
                                        <asp:Label ID="lbl4_DatetimeofAccident" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 25%">Date & time you first saw this patient
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_DatetimeFirstsawPatient" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 39%" class="padding">2. Cause of accident, nature of wound, injured organs:
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Causeofaccident" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 74%" class="padding">3. Was the patient under the influence of alcohol or drugs at the time of arrival to the hospital? (&nbsp; ) No (&nbsp; ) Yes
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_patientundertheinfluence" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>

                    <tr>
                        <td colspan="5">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 33%" class="padding">Pertinent clinical findings (Symptoms & Signs):
                                    </td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Pertinentclinical" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td class="border-bottom-dotted border-bottom-width padding" >
                                        <br />
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 15%" class="padding">Underlying diseases:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_UnderlyingDiseases" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%" class="padding">Investigations/ pathological studies:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Investigations" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <br />
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <br />
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 9%" class="padding">Diagnosis 1 </td>
                                    <td class="border-bottom-dotted border-bottom-width padding" style="width: 58%">
                                        <asp:Label ID="lbl4_Diagnosis_1" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 4%">ICD10</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_ICD10_1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 9%" class="padding">Diagnosis 2 </td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 58%">
                                        <asp:Label ID="lbl4_Diagnosis_2" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 4%">ICD10</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_ICD10_2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 9%" class="padding">Diagnosis 3 </td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 58%">
                                        <asp:Label ID="lbl4_Diagnosis_3" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 4%">ICD10</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_ICD10_3" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="font-size: smaller">(Please state the diagnosis leading to treatment on this admission (not including underlying diseases or conditions not treated). Please rank in order from the most important.)
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 8%" class="padding">Treatment:</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Treatment" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <br />
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 14%" class="padding">Surgery/ Operation</td>
                                    <td class="border-bottom-dotted border-bottom-width" style="width: 45%">
                                        <asp:Label ID="lbl4_Surgery" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 5%">ICD9/CPT</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_ICD9" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 16%" class="padding">Result/ Complications</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Complications" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>


                    <tr>
                        <td colspan="5">

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">Is the illness related to alcohol, drug abuse or addiction?
                                    </td>
                                    <td style="width: 5%" >(&nbsp; ) No</td>
                                    <td style="width: 6%" >(&nbsp; ) Yes</td>
                                    <td class="border-bottom-dotted border-bottom-width padding">
                                        <asp:Label ID="lbl4_illnessrelatedtoalcohol" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">Is the patient pregnant?
                                    </td>
                                    <td style="width: 5%">(&nbsp; ) No</td>
                                    <td style="width: 6%">(&nbsp; ) Yes</td>
                                    <td>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 25%">Gestation age</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 20%">
                                                    <asp:Label ID="lbl4_GestaionAge" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td>Wks</td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">Was the treatment related to infertility?
                                    </td>
                                    <td style="width: 5%">(&nbsp; ) No</td>
                                    <td style="width: 6%">(&nbsp; ) Yes</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_treatment_related_Fertility" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">HIV test?
                                    </td>
                                    <td style="width: 5%">(&nbsp; ) No</td>
                                    <td style="width: 12%">(&nbsp; ) Done&nbsp;&nbsp; Result</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_HIV" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>


                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%" class="padding">Has the patient been treated by other doctors?&nbsp; (&nbsp; ) No (&nbsp; ) Yes,
                                    </td>
                                    <td style="width: 23%">(Please give name and address)</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Patient_treated" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>

                    <tr>
                        <td colspan="5" style="font-weight: bold; font-size: medium" class="padding">The Patient's Medical History</td>
                    </tr>

                    <%--<tr>
                        <td >Date</td>
                        <td >Signs & Symptoms</td>
                        <td >Diagnosis</td>
                        <td >Treatment</td>
                        <td >Physician/ Hospital Name</td>
                    </tr>--%>
                    <tr>
                        <td colspan="5">

                            <table border="1" class="table" style="margin: -1px; border-left-style: hidden; border-right-style: hidden">
                                <tr class="border-top-hidden">
                                    <td style="padding-left: -3px">Date</td>
                                    <td>Signs & Symptoms</td>
                                    <td>Diagnosis</td>
                                    <td>Treatment</td>
                                    <td>Physician/ Hospital Name</td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%" class="padding">Estimated time for recovery</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Estimated_Time" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 13%" class="padding">Other Comments</td>
                                    <td class="border-bottom-dotted border-bottom-width">
                                        <asp:Label ID="lbl4_Other_Comments" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%;" class="padding">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 25%; text-align: right; padding:6px" >Signed</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 53%"></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                    <td style="width: 50%">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 31%; padding:6px">Medical License No</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 50%">
                                                    <asp:Label ID="lbl4_Medical_Lecense" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 45%;" class="padding">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 25%; text-align: right">(</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 50%; text-align:center">
                                                    <asp:Label ID="lbl4_Signed" runat="server"></asp:Label>
                                                </td>
                                                <td>)</td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                    <td style="width: 50%" class="padding">

                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 7%" class="padding">Speciality</td>
                                                <td class="border-bottom-dotted border-bottom-width" style="width: 45%">
                                                    <asp:Label ID="lbl4_Speciality" runat="server"></asp:Label>
                                                </td>
                                                <td style="width: 5%">Date:</td>
                                                <td class="border-bottom-dotted border-bottom-width">
                                                    <asp:Label ID="lbl4_Speciality_Date" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>

        </div>


    </form>
</body>
</html>
