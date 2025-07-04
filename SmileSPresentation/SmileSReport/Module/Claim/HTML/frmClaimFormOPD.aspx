<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClaimFormOPD.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmClaimFormOPD" %>

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

        /*font: 8pt arial, tahoma, sans-serif;*/

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

        #form {
            width: 100%;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        table, td {
            border: 1px solid;
            padding: 0;
            margin: 0;
        }

        th {
            font: large arial, tahoma, sans-serif;
            font-weight: bold;
            padding: 10px;
        }

        #table-main {
            width: 30px;
            margin: auto;
        }

        #row2 {
            font: 8pt arial, tahoma, sans-serif;
            height: 60px;
            font-weight: bold;
        }

        td > div {
            display: table-cell;
        }

        .hosname {
            height: 24px;
            width: 392px;
            padding-top: 10px;
            padding-left: 5px;
        }

        #row4-table, #row6, #row7 {
            font: 8pt arial, tahoma, sans-serif;
            font-weight: bold;
        }

        .container {
            display: table-cell;
            display: inline;
            position: relative;
            padding-left: 26px;
            cursor: pointer;
            font: 8pt arial, tahoma, sans-serif;
            font-weight: bold;
            padding-top: 3px;
        }

            .container input {
                position: absolute;
                opacity: 0;
            }

        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 12px;
            width: 12px;
            background-color: #eee;
            border: 2px solid black;
            border-radius: 50%;
        }

        .container input:checked ~ .checkmark {
            background-color: #2196F3;
        }

        br {
            line-height: 0.55cm;
        }

        .lbl-input {
            font-weight: normal;
        }

        .row2-div {
            border-right: 1px solid;
            text-align: center;
        }

        .row-head {
            height: 29px;
            font: 11pt arial, tahoma, sans-serif;
            font-weight: bold;
            vertical-align: middle;
            padding-left: 5px;
        }

        .row4-table-td {
            border: none;
            padding-top: 7px;
        }

        .row7-table-td {
            border: none;
            padding-top: 10px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="page">
            <div id="form">
                <table id="table-main">
                    <tr id="row1">
                        <th>แบบฟอร์มคนไข้นอก <u>กรณีเจ็บป่วยและอุบัติเหตุ</u> (OPD CLAIM FORM)</th>
                    </tr>
                    <tr id="row2">
                        <td>
                            <div class="row2-div" style="width: 95px; vertical-align: middle; font-size: 17px;">
                                ถึง
                            </div>
                            <div class="row2-div" style="width: 250px;">
                                <div>
                                    <div style="height: 22px;">
                                        &nbsp
                                    </div>
                                    <div style="height: 60px; border-top: 1px solid; display: table-cell; vertical-align: bottom; width: 250px;">
                                        เลขที่กรมธรรม์&nbsp
                                    <label id="lbl0_PolicyNumber" class="lbl-input" runat="server">......................................................</label>

                                    </div>

                                </div>

                            </div>
                            <div class="row2-div" style="width: 95px; vertical-align: middle; font-size: 17px;">
                                จาก
                            </div>
                            <div>
                                <div>
                                    <div class="hosname" style="border-bottom: 1px solid;">
                                        <div style="display: table-cell; width: 287px;">
                                            โรงพยาบาล &nbsp
                                        <label id="lbl0_Hospital" class="lbl-input" runat="server">.........................................................................</label>&nbsp
                                        </div>
                                        <div style="display: table-cell; width: 100px;">
                                            HN&nbsp
                                        <label id="lbl0_HN" class="lbl-input" runat="server">.........................</label>
                                        </div>
                                    </div>
                                    <div class="hosname" style="border-bottom: 1px solid;">
                                        <div style="display: table-cell; width: 190px;">
                                            โทรศัพท์&nbsp
                                        <label id="lbl0_Phone" class="lbl-input" runat="server">..............................................</label>&nbsp
                                        </div>
                                        <div style="display: table-cell; width: 195px;">
                                            โทรสาร&nbsp
                                        <label id="lbl0_Fax" class="lbl-input" runat="server">...................................................</label>
                                        </div>
                                    </div>
                                    <div class="hosname">

                                        <div style="display: table-cell; width: 307px;">
                                            วันที่เข้ารักษา&nbsp
                                        <label id="lbl0_DateTreatment" class="lbl-input" runat="server">.......................</label>&nbsp
                                        </div>
                                        <div style="display: table-cell; width: 150px;">
                                            เวลา&nbsp
                                        <label id="lbl0_TimeTreatment" class="lbl-input" runat="server">....................</label>&nbsp
                                        </div>
                                        <div style="display: table-cell; width: 230px;">
                                            ผู้ส่ง&nbsp
                                        <label id="lbl0_Sender" class="lbl-input" runat="server">..........................................</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr id="row3">
                        <td style="background-color: yellow">
                            <div class="row-head">
                                ส่วนที่ 1 สำหรับผู้เอาประกันกรอก / Section 1 (To be completed by the Insured Person)
                            </div>
                        </td>
                    </tr>
                    <tr id="row4">
                        <td>
                            <div style="padding: 4px;">
                                <table id="row4-table" style="border: none;">
                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 408px;">
                                                ชื่อ-สกุล</br>
                                            Name&nbsp
                                            <label id="lbl1_Name" class="lbl-input" runat="server">.......................................................................................................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 281px;">
                                                วัน เดือน ปี เกิด</br>
                                            Date of birth&nbsp
                                            <label id="lbl1_BirthDate" class="lbl-input" runat="server">..........................................................</label>

                                            </div>
                                            <div style="padding-left: 13px; width: 157px;">
                                                อายุ</br>
                                            Age&nbsp
                                            <label id="lbl1_Age" class="lbl-input" runat="server">.......................................</label>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr style="margin-top: 2px;">
                                        <td class="row4-table-td">
                                            <div style="width: 262px;">
                                                อาชีพ</br>
                                            Ocupation&nbsp
                                            <label id="lbl1_Ocpation" class="lbl-input" runat="server">....................................................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 325px;">
                                                บัตรประชาชน / อื่นๆ ระบุ</br>
                                            ID Card / Other , Please specify&nbsp
                                            <label id="lbl1_Card" class="lbl-input" runat="server">..................................................</label>

                                            </div>
                                            <div style="padding-left: 13px; width: 196px;">
                                                เลขที่บัตร</br>
                                            No&nbsp
                                            <label id="lbl1_CardNo" class="lbl-input" runat="server">...........................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 584px;">
                                                ที่อยู่ปัจจุบัน</br>
                                            Address&nbsp
                                            <label id="lbl1_Address" class="lbl-input" runat="server">.............................................................................................................................................................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 260px;">
                                                โทรศัพท์</br>
                                            Telephone&nbsp
                                            <label id="lbl1_Phone" class="lbl-input" runat="server">...........................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 470px;">
                                                บริษัทประกันภัยอื่นๆ (ถ้ามีโปรดระบุ ชื่อบริษัท)</br>
                                            Other Insures&nbsp
                                            <label id="lbl1_Insure" class="lbl-input" runat="server">..............................................................................................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 227px;">
                                                หมายเลขกรมธรรม์</br>
                                            Policy No&nbsp
                                            <label id="lbl1_Policy_No" class="lbl-input" runat="server">...........................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 279px;">
                                                ประเภทของประกันภัย</br>
                                            Type of insurance&nbsp
                                            <label id="lbl1_TypeInsurance" class="lbl-input" runat="server">..................................</label>
                                            </div>
                                        </rd>
                                    </tr>
                                    <tr>
                                    <td class="row4-table-td">
                                        <div style="width:100%; height:74px;">
                                            อาการเจ็บป่วยหรืออุบัติเหตุ ลักษณะของอาการหรือการเกิดเหตุโดยสังเขป</br>
                                            Symptoms of illness or accident & Brief detail of how the accident occurred

                                            <label id="lbl1_AccidentDetail" class="lbl-input" runat="server">
                                                ..............................................................................................................................................</br>
                                                .................................................................................................................................................................................................................................................................................</br>
                                                .................................................................................................................................................................................................................................................................................
                                            </label>

                                        </div>
                                    </td>
                                </tr>

                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 473px;">
                                                วันที่เกิดเหตุ</br>
                                            Date of accident&nbsp
                                            <label id="lbl1_DateAccident" class="lbl-input" runat="server">............................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 430px;">
                                                เวลาที่เกิดเหตุ</br>
                                            Time of accident&nbsp
                                            <label id="lbl1_TimeAccident" class="lbl-input" runat="server">.........................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 647px;">
                                                สถานที่เกิดเหตุ</br>
                                            Place of accident&nbsp
                                            <label id="lbl1_PlaceAccident" class="lbl-input" runat="server">................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div>
                                                การเจ็บป่วย / การเกิดเหตุครั้งนี้ท่าน
                                            <label class="container" style="padding-left: 300px;">
                                                ไม่เคยรักษาที่ใดมาก่อน
                                                <input id="rdb1_NoTreatment" type="radio" name="radio" value="">
                                                <span class="checkmark" style="margin-left: 270px;"></span>
                                            </label>
                                                <label class="container" style="padding-left: 70px;">
                                                    เคยรักษามาก่อนที่โรงพยาบาล
                                                <input id="rdb1_YesTreatment" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 40px;"></span>
                                                </label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: none; padding-top: 12px;">
                                            <div style="width: 100%;">
                                                As a result of this illness or accident, treatment&nbsp
                                            <label id="lbl1_ResultTreatment" class="lbl-input" runat="server">............................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 412px;">
                                                ได้ใช้สิทธิ์ไปแล้วจำนวนเงินทั้งสิ้น
                                            <label style="padding-left: 230px;">บาท</label>
                                                </br>
                                            The cost of treatment that has already been given&nbsp
                                            <label id="lbl1_CostTreatment" class="lbl-input" runat="server">.......................................... </label>
                                                &nbspBath
                                            </div>
                                            <div style="padding-left: 13px; width: 369px;">
                                                ยังเหลือเงินตามสิทธิ์สำหรับค่ารักษาพยาบาลอีก
                                            <label style="padding-left: 120px;">บาท</label>
                                                </br>
                                            The balance of medicine expends&nbsp
                                            <label id="lbl1_Balance" class="lbl-input" runat="server" style="padding-left: 73px;">.............................. </label>
                                                &nbspBath
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div>
                                                &nbsp&nbsp ข้าพเจ้าขอมอบฉันทะให้โรงพยาบาล แพทย์ หรือบุคคลอื่นใดที่ได้ทำการตรวจรักษาข้าพเจ้า หรือบุคคลในครอบครัวของข้าพเจ้า
                                            มีอำนาจแจ้งความใดๆ ที่เกี่ยวกับการบาด</br>
                                            เจ็บ ประวัติทางการแพทย์ การปรึกษาโรค ใบสั่งยา หรือการรักษา และสำเนาเอกสารประวัติทางการแพทย์ของโรงพยาบาลทั้งหมดต่อ บริษัท ประกันภัยตามชื่อที่ปรากฏด้านบน</br>
                                            หรือที่ได้รับมอบหมายจากบริษัท ฯ อนึง สำเนาใบมอบฉันทะนี้ให้ถือว่ามีผลบังคับใช้เช่นเดียวกับต้นฉบับ
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row4-table-td">
                                            <div style="width: 336px;">
                                                ลงชื่อ</br>
                                            Signed&nbsp
                                            <label id="lbl1_Signed" class="lbl-input" runat="server">...................................................................................................</label>
                                            </div>
                                            <div style="padding-left: 13px;">
                                                &emsp; &ensp; ผู้เอาประกัน</br>
                                            (The insured person)
                                            </div>
                                            <div style="padding-left: 13px; width: 250px;">
                                                วันที่</br>
                                            Date&nbsp
                                            <label id="Label1" class="lbl-input" runat="server">..........................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr id="row5">
                        <td style="background-color: yellow">
                            <div class="row-head">
                                ส่วนที่ 2 สำหรับแพทย์ผู้ทำการรักษากรอก / Section 2 Medical information (To be completed bt heading doctor)
                            </div>
                        </td>
                    </tr>
                    <tr id="row6">
                        <td>
                            <div style="border-right: 1px solid; width: 407px; height: 25px; vertical-align: middle;">
                                &nbsp;The Patient's Name
                            <label for=""></label>
                            </div>
                            <div style="vertical-align: middle;">
                                &nbsp;Is the illness/injury related to an accident?
                            <label class="container" style="padding-left: 50px;">
                                Yes
                                <input type="radio" name="radio">
                                <span class="checkmark" style="margin-left: 20px;"></span>
                            </label>
                                <label class="container" style="padding-left: 70px;">
                                    No
                                <input type="radio" name="radio">
                                    <span class="checkmark" style="margin-left: 40px;"></span>
                                </label>
                            </div>
                        </td>
                    </tr>
                    <tr id="row7">
                        <td>
                            <div style="padding: 4px;">
                                <table style="border: none;">
                                    <tr>
                                        <td style="border: none; padding-top: 4px;">
                                            <div style="width: 100%;">
                                                Cheif Complaint&nbsp
                                            <label id="lbl2_CheifComplaint" class="lbl-input" runat="server">...................................................................................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">

                                            <div style="width: 324px;">
                                                1.Symptom / Accident date&nbsp
                                            <label id="lbl2_No1_SympAccDate" class="lbl-input" runat="server">...........................................................</label>
                                            </div>
                                            <div style="padding-left: 13px; width: 465px;">
                                                2.Date you first saw the patient for this illness / injury&nbsp
                                            <label id="lbl2_No2_IllInjDate" class="lbl-input" runat="server">............................................................</label>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 100%;">
                                                3.(a)Present illness / Details of injury&nbsp
                                            <label id="lbl2_No3a_PresentIll" class="lbl-input" runat="server">..........................................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 100%;">
                                                (b)Pertinent clinical finding (Symptom & signs)&nbsp
                                            <label id="lbl2_No3b_Pertinent" class="lbl-input" runat="server">..........................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 100%;">
                                                Was the injury/illness influenced by the use of alcohol or drugs?
                                            <label class="container" style="padding-left: 50px;">
                                                Yes
                                                <input id="rdb2_AlcYes" type="radio" name="radio" value="">
                                                <span class="checkmark" style="margin-left: 20px;"></span>
                                            </label>
                                                <label class="container" style="padding-left: 70px;">
                                                    No
                                                <input id="rdb2_AlcNo" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 40px;"></span>
                                                </label>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 100%;">
                                                <label id="lbl2_Alc" class="lbl-input" runat="server">.................................................................................................................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div>
                                                <div style="display: table-cell; width: 281px;">
                                                    4.Diagnosis 1&nbsp
                                                <label id="lbl2_No4_Diagnosis1" class="lbl-input" runat="server">....................................................................</label>&nbsp
                                                </div>
                                                <div style="display: table-cell; width: 123px;">
                                                    ICD-10&nbsp
                                                <label id="lbl2_No4_Icd_1" class="lbl-input" runat="server">............................</label>
                                                </div>
                                            </div>
                                            <div style="padding-left: 13px;">
                                                <div style="display: table-cell; width: 270px;">
                                                    Diagnosis 2&nbsp
                                                <label id="lbl2_No4_Diagnosis2" class="lbl-input" runat="server">...................................................................</label>&nbsp
                                                </div>
                                                <div style="display: table-cell; width: 120px;">
                                                    ICD-10&nbsp
                                                <label id="lbl2_No4_Icd_2" class="lbl-input" runat="server">...........................</label>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div>
                                                5.Treatment &nbsp;
                                            <label class="container" style="padding-left: 20px;">
                                                <input id="rdb2_No5_Suture" type="radio" name="radio" value="">
                                                <span class="checkmark" style="margin-left: 0px;"></span>
                                                Suture&nbsp
                                                <label id="lbl2_No5_Suture" class="lbl-input" runat="server">..........</label>&nbsp Site
                                            </label>
                                                <label class="container" style="padding-left: 30px;">
                                                    Wound dressing
                                                <input id="rdb2_No5_Wound1" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 10px;"></span>
                                                </label>
                                                <label class="container" style="padding-left: 30px;">
                                                    Wound dressing
                                                <input id="rdb2_No5_Wound2" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 10px;"></span>
                                                </label>
                                                <label class="container" style="padding-left: 27px;">
                                                    Wound dressing
                                                <input id="rdb2_No5_Wound3" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 7px;"></span>
                                                </label>

                                            </div>
                                            <div style="width: 277px;">
                                                <label class="container" style="padding-left: 30px;">
                                                    Other&nbsp
                                                <input id="rdb2_No5_Other" type="radio" name="radio" value="">
                                                    <span class="checkmark" style="margin-left: 10px;"></span>
                                                    <label id="lbl2_No5_TreatmentOther" class="lbl-input" runat="server">.......................................................................</label>
                                                </label>
                                                </br>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td class="row7-table-td">
                                        <div style="width:100%; height:53px;">
                                            <label class="container">
                                                <input id="rdb2_Medi_Inves_Other" type="radio" name="radio" value="">
                                                <span class="checkmark"></span>
                                                Medication / Investigation / Other (Please specify)

                                                <label id="lbl2_Medi_Inves_Other" class="lbl-input" runat="server">
                                                    .................................................................................................................................................................................<br>
                                                        .................................................................................................................................................................................................................................................................................</br>
                                                        .................................................................................................................................................................................................................................................................................
                                                    </label>
                                            </label>
                                        </div>
                                    </td>
                                </tr>
                                    <tr>
                                        <td style="border: none; padding-top: 5px;">
                                            <div>
                                                6.Was the patient pragnant at the time of treatment &nbsp;
                                            <label class="container" style="padding-left: 50px;">
                                                No
                                                <input id="rdb2_No6_NoTime" type="radio" name="radio" value="">
                                                <span class="checkmark" style="margin-left: 20px;"></span>
                                            </label>

                                            </div>
                                            <div style="width: 180px;">
                                                <label class="container" style="padding-left: 70px;">
                                                    Yes&nbsp
                                                <label id="lbl2_No6_YesTime" class="lbl-input" runat="server">.................</label>&nbsp Week
                                                <input id="rdb2_No6_YesTime" type="radio" name="radio">
                                                    <span class="checkmark" style="margin-left: 40px;"></span>
                                                </label>
                                            </div>
                                            <div style="padding-left: 50px; padding-top: 8px; width: 243px;">
                                                (LMP&nbsp
                                            <label id="lbl2_No6_Lmp" class="lbl-input" runat="server">....................................................................</label>&nbsp
                                            )
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 100%;">
                                                7.Other comments&nbsp
                                            <label id="lbl2_No7_Comment" class="lbl-input" runat="server">...............................................................................................................................................................................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: none; padding-top: 10px;">
                                            <div>
                                                I hereby certify that I have personaly examined and readted the insured in connection to the above disability and that the
                                            facts are in my opinion as given above.
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">

                                            <div style="width: 557px">
                                                Hospital Name&nbsp
                                            <label id="lbl2_Last_Hospital" class="lbl-input" runat="server">................................................................................................................................................................</label>&nbsp
                                            </div>
                                            <div style="width: 251px">
                                                Telephone No&nbsp
                                            <label id="lbl2_Last_Phone" class="lbl-input" runat="server">...........................................................</label>

                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 397px">
                                                Name of physician&nbsp
                                            <label id="lbl2_Last_Physician" class="lbl-input" runat="server">...................................................................................................</label>&nbsp
                                            </div>
                                            <div style="width: 226px">
                                                License No&nbsp
                                            <label id="lbl2_Last_LicNo" class="lbl-input" runat="server">......................................................</label>&nbsp
                                            </div>
                                            <div style="width: 186px">
                                                Speciality&nbsp
                                            <label id="lbl2_Lasr_Special" class="lbl-input" runat="server">............................................</label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="row7-table-td">
                                            <div style="width: 542px">
                                                Signature&nbsp
                                            <label id="lbl2_Last_Sign" class="lbl-input" runat="server">.................................................................................................................................................................</label>&nbsp
                                            </div>
                                            <div style="width: 266px">
                                                Date&nbsp
                                            <label id="lbl2_Last_Date" class="lbl-input" runat="server">................................................................................</label>
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
