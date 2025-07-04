<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportApplicationCoverNote.aspx.cs" Inherits="SmileMotorV1.Modules.Motor.frmReportApplicationCoverNote" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>หนังสือรับรองการทำประกันภัยรถยนต์</title>

    <link href="https://fonts.googleapis.com/css?family=Sarabun" rel="stylesheet">

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" integrity="sha384-HSMxcRTRxnN+Bdg0JdbxYKrThecOKuH5zCYotlSAcp1+c8xmyTe9GYg1l9a69psu"
        crossorigin="anonymous">

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap-theme.min.css"
        integrity="sha384-6pzBo3FDv/PJ8r2KRkGHifhEocL+1X2rVCTTkUfGk7/0pbek5mMa1upzvWbrUbOZ" crossorigin="anonymous">

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js" integrity="sha384-aJ21OjlMXNL5UyIl/XNwTMqvzeRMZH2w8c5cRVpzpU8Y5bApTppSuUkhZXN0VxHd"
        crossorigin="anonymous"></script>
    <!--Jquery-->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <style>
        table,
        th,
        td {
            border: 1px solid black;
            border-collapse: collapse;
        }

            table#t01,
            table#t01 > tbody > tr > th,
            table#t01 > tbody > tr > td {
                border: 1px solid black !important;
                border-collapse: collapse;
            }

        th {
            padding: 1px;
            border: 1px solid black;
        }

        td {
            padding: 5px;
            border: 1px solid black;
        }

        body {
            font-family: 'Sarabun', sans-serif;
        }

        .header-table {
            /* border: 1px solid rgb(55, 110, 230); */
            border-collapse: collapse;
            background-color: rgb(7, 136, 196) !important;
            color: white !important;
        }

        .detail-table {
            /* border: 1px solid rgb(55, 110, 230); */
            border-collapse: collapse;
            text-align: left;
        }

        .footer1 {
            position: absolute;
            left: 0px;
            bottom: 0px;
            width: 100%;
            height: 20px;
            background-color: rgb(44, 53, 91) !important;
        }

        .footer2 {
            position: absolute;
            left: 0px;
            bottom: 20px;
            width: 100%;
            height: 20px;
            background-color: #0788C4 !important;
        }
    </style>
</head>

<body>

    <form style="width: 100%">
        <!-- Page1 -->
        <div style="page-break-after: always;">
            <div style="text-align: center;">
                <img src="../../Image/ssm_logo.png" alt="Siam Smile" style="width: 100px; height: 100px;">
            </div>
            <br>
            <p style="text-align: center; font-size: 14px;"><b>หนังสือขอบคุณการทำประกันภัยรถยนต์</b></p>
            <div style="margin-left: 10px;">
                <span id="txt_customerName" runat="server" style="position: absolute; margin: -7px 0 0 10%">-</span>
                <p style="font-size: 14px; margin-top: 20px;"><b>เรียน........................................................................................................</b></p>
                <%--                <div style="word-wrap: break-word; letter-spacing: 0.8px; font-size: 12px">
                    <span style="text-indent: 30px; display: inline-block;">บริษัท สยามสไมล์ โบรกเกอร์ (ประเทศไทย) จำกัด "บริษัท"
                        ขอขอบพระคุณท่านที่ได้มอบความไว้วางใจให้บริษัทเป็นนายหน้าประกันภัยเพื่อเสนอความคุ้มครองกรมธรรม์ประกันภัยรถยนต์ให้กับท่าน
                        ซึ่งบริษัทได้รับคำขอเอาประกันภัยของท่านและได้รับชำระค่าเบี้ยประกันภัยรายเดือนจากท่านแล้ว จำนวน
                        588 บาท เพื่อให้เป็นไปตามความประสงค์ของท่าน
                        บริษัทได้พิจารณาคัดเลือกบริษัทประกันวินาศภัยที่มีความมั่นคง
                        เหมาะสมและมีผลิตภัณฑ์ประกันภัยที่ตรงกับความประสงค์ของท่าน บริษัทจึงได้เลือกให้ บริษัท
                        อาคเนย์ประกันภัย จำกัด (มหาชน) "อาคเนย์ประกันภัย"
                        เป็นผู้รับประกันภัยรถยนต์ของท่านเป็นที่เรียบร้อยแล้ว
                        เพื่อให้ท่านทราบถึงรายละเอียดความคุ้มครองตามเอกสารเสนอขายกรมธรรม์ประกันภัย
                        บริษัทจึงขอแจ้งรายละเอียดตามที่ปรากฎในตารางข้างล่างนี้
                    </span>
                </div>--%>
                <div style="word-wrap: break-word; font-size: 14px; margin-top: 15px">
                    <p style="margin-left: 0.8cm;">
                        บริษัท สยามสไมล์ โบรกเกอร์ (ประเทศไทย) จำกัด "บริษัท"
                        ขอขอบพระคุณท่านที่ได้มอบความไว้วางใจให้บริษัทเป็นนายหน้าประกันภัยเพื่อ
                    </p>
                    <p>
                        เสนอความคุ้มครองกรมธรรม์ประกันภัยรถยนต์ให้กับท่าน
                        ซึ่งบริษัทได้รับคำขอเอาประกันภัยของท่านและได้รับชำระค่าเบี้ยประกันภัยรายเดือน
                    </p>
                    <p>
                        จากท่านแล้วจำนวน 588 บาท เพื่อให้เป็นไปตามความประสงค์ของท่าน
                        บริษัทได้พิจารณาคัดเลือกบริษัทประกันวินาศภัยที่มีความมั่นคงเหมาะสม
                    </p>
                    <p>
                        และมีผลิตภัณฑ์ประกันภัยที่ตรงกับความประสงค์ของท่าน
                        บริษัทจึงได้เลือกให้ บริษัท อาคเนย์ประกันภัย จำกัด (มหาชน) "อาคเนย์ประกันภัย"
                    </p>
                    <p>
                        เป็นผู้รับประกันภัยรถยนต์ของท่านเป็นที่เรียบร้อยแล้ว
                        เพื่อให้ท่านทราบถึงรายละเอียดความคุ้มครองตามเอกสารเสนอขายกรมธรรม์ประกันภัย
                    </p>
                    <p>บริษัทจึงขอแจ้งรายละเอียดตามที่ปรากฎในตารางข้างล่างนี้</p>
                </div>
            </div>
            <table style="width: 100%">
                <tr>
                    <th colspan="7" style="font-size: 16px; text-align: center;">รายละเอียดการรับประกันภัย</th>
                </tr>
                <tr>
                    <td style="padding: 5px;" colspan="7">
                        <div class="row">
                            <span id="txt_applicationCode" runat="server" style="position: absolute; margin: -4px 0 0 13%">-</span>
                            <span style="margin-left: 0.4cm;">เอกสารเลขที่:<span style="margin-left: 0.2cm;">...........................................................................................................................................................................................................</span>
                            </span>
                        </div>
                        <div class="row">
                            <span id="txt_customerFullname" runat="server" style="position: absolute; margin: -4px 0 0 15%">-</span>
                            <span style="margin-left: 0.4cm;">ผู้เอาประกันภัย:<span style="margin-left: 0.2cm;">.......................................................................................................................................................................................................</span>
                            </span>
                        </div>
                        <div class="row">
                            <span id="txt_address" runat="server" style="position: absolute; margin: -4px 0 0 8%">-</span>
                            <span style="margin-left: 0.4cm;">ที่อยู่:<span style="margin-left: 0.2cm;">........................................................................................................................................................................................................................</span>
                            </span>
                        </div>
                        <div class="row">
                            <span style="margin-left: 0.4cm;">...................................................................................................................................................................................................................................</span>
                        </div>
                        <!-- <div class="row">
                            <span id="txt_carddetail" runat="server" style="position: absolute; margin: -4px 0 0 18%">-</span>
                            <span style="margin-left: 0.3cm;">เลขที่บัตรประชาชน: <span style="margin-left: 0.2cm;">.............................................................................................................</span></span>
                            <span id="txt_contactdetail" runat="server" style="position: absolute; margin: -4px 0 0 20%">-</span>
                            <span style="margin-left: 0.3cm;">โทรศัพท์: <span>......................................................................
                            </span></span>
                        </div> -->
                    </td>
                </tr>
                <tr>
                    <td style="padding: 5px;" colspan="7">
                        <div class="row">
                            <span id="txt_startCoverDate" runat="server" style="position: absolute; margin: -4px 0 0 30%">-</span>
                            <span style="margin-left: 0.4cm;">ระยะเวลาเอาประกันภัย : เริ่มต้นวันที่ <span style="margin-left: 0.2cm;">.................................................................</span>
                                <span style="color: red !important">**กรมธรรม์มีผลสิ้นสุดกรณีท่านไม่ชำระเบี้ยตามรอบเรียกเก็บ**</span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">รายการรถยนต์ที่เอาประกันภัย</td>
                </tr>
                <tr>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">ลำดับ
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">รหัส
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">ชื่อรถยนต์/รุ่น
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">เลขทะเบียน
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">เลขตัวถัง
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">ปี/รุ่น
                    </td>
                    <td style="text-align: center; font-size: 12px; font-weight: bold">จำนวนที่นั่งขนาด/น้ำหนัก
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-bottom: 5px; text-align: center; width: 5%">1</td>
                    <td style="padding-top: 5px; padding-bottom: 5px; text-align: center; width: 10%">
                        <span id="txt_vehicleSegmentCode" runat="server"></span>
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px; width: 25%; text-align: center;">
                        <span id="txt_vehicleBrandModel" runat="server"></span>
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px; width: 20%; text-align: center">
                        <span id="txt_vehicleLicenePlate" runat="server"></span>
                        <br />
                        <span id="txt_vehicleLicenePlate2" runat="server">4545454</span>
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px; width: 25%; text-align: center;">
                        <span id="txt_vehicleChassisNumber" runat="server"></span>
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px; text-align: center; width: 5%">
                        <span id="txt_vehicleYear" runat="server"></span>
                    </td>
                    <td style="padding-top: 5px; padding-bottom: 5px; text-align: center; width: 10%">
                        <span id="txt_vehicleSeatWeight" runat="server"></span>
                    </td>
                </tr>
            </table>
            <br>

            <table id="t01" style="width: 100%">
                <tr class="header-table" style="height: 40px">
                    <th class="header-table" style="font-size: 16px; text-align: center;">รายละเอียดความคุ้มครอง</th>
                    <th class="header-table" style="font-size: 16px; text-align: center;">MP2s</th>
                </tr>
                <tr>
                    <th class="detail-table"><span style="font-size: 14px;">1. ความรับผิดต่อบุคคลภายนอก</span></th>
                    <th style="padding-top: 5px; padding-bottom: 5px;"></th>
                </tr>

                <tr>
                    <td style="padding: 0.5px">
                        <ol>1.1 ชีวิต ร่างกายและอนามัย ต่อคน</ol>
                        <ol>1.2 ชีวิต ร่างกายและอนามัย ต่อครั้ง</ol>
                        <ol>1.3 ทรัพย์สินของบุคคลภายนอก</ol>
                        <td style="padding-top: 0.5px; text-align: right;">
                            <ol>750,000</ol>
                            <ol>10,000,000</ol>
                            <ol>600,000</ol>
                        </td>
                </tr>

                <tr>
                    <th class="detail-table">2. ความเสียหายต่อรถยนต์ที่ทำประกัน</th>
                    <th style="padding-top: 5px; padding-bottom: 5px;"></th>
                </tr>

                <tr>
                    <td style="padding: 0.5px;">
                        <ol>2.1 ภัยจากการชนกับยานพาหนะทางบกและสามารถระบุคู่กรณีได้</ol>
                        <ol>2.2 กรณีเสียหายที่เกิดจากภัย รถหาย ไฟไหม้ น้ำท่วม</ol>
                        <td style="padding-top: 5px; text-align: right;">
                            <ol>100,000</ol>
                            <ol>100,000</ol>
                        </td>
                </tr>
                <tr>
                    <th class="detail-table">3. การประกันตัวผู้ขับขี่ ต่อครั้ง</th>
                    <td style="padding-top: 5px; text-align: right;">
                        <ol>1,000,000</ol>
                    </td>
                </tr>
                <tr>
                    <th class="detail-table">4. อุบัติเหตุส่วนบุคคล ต่อคน</th>
                    <th style="padding-top: 5px; padding-bottom: 5px;"></th>
                </tr>
                <tr>
                    <td style="padding: 0.5px;">
                        <ol>4.1 เสียชีวิต สูญเสียอวัยวะหรือทุพพลภาพถาวร (รถเก๋ง 5 คน,รถกระบะ 3 คน)</ol>
                        <ol>4.2 ค่ารักษาพยาบาล (รถเก๋ง 5 คน,รถกระบะ 3 คน)</ol>
                        <td style="padding-top: 5px; text-align: right;">
                            <ol>100,000</ol>
                            <ol>100,000</ol>
                        </td>
                </tr>
            </table>

            <div class="row col-xs-12" style="text-align: center; margin: 18px 10px 30px 10px;">
                <div class="col-xs-4" style="text-align: right;">
                    <img src="../../Image/carleft.JPG" alt="left" style="width: 100px">
                </div>
                <div class="col-xs-4">
                    <p style="text-align: center; font-size: 11px; margin: 0 0 0!important"><b>ฝ่ายงานรับประกันภัย</b></p>
                    <p style="text-align: center; font-size: 11px; margin: 0 0 0!important"><b>บริษัท สยามสไมล์โบรกเกอร์( ประเทศไทย) จำกัด</b></p>
                    <p style="text-align: center; font-size: 14px; margin: 0 0 0!important"><b>Call Center : 0-2533-3999</b></p>
                </div>
                <div class="col-xs-4" style="text-align: left;">
                    <img src="../../Image/carright.JPG" alt="right" style="width: 100px">
                </div>
            </div>

            <div class="footer2">
                <span style="position: absolute; left: 0; font-size: 12px; color: white !important; margin: 2px 0px 0px 5px">ผู้จัดการโครงการ
                    : บริษัท สยามสไมล์โบรกเกอร์ (ประเทศไทย) จำกัด</span>
                <span style="position: absolute; right: 0; font-size: 12px; color: white !important; margin: 2px 5px 0px 0px">ใบอนุญาตเลขที่
                    ว00017/2553</span>
            </div>
            <div class="footer1">
                <span style="position: absolute; left: 0; font-size: 12px; color: white !important; margin: 2px 0px 0px 5px">บริษัทผู้รับประกันภัย
                    : บริษัท อาคเนย์ประกันภัย จำกัด (มหาชน)</span>
            </div>
        </div>

        <%--        <br>
        <br>
        <br>--%>
        <!-- Page2 -->
        <%--<div style="page-break-after: always; margin-left: 40px;">
            <h4 style="text-align: center">สรุปสาระสำคัญ</h4>
            <h4 style="text-align: center">กรมธรรม์ประกันภัยรถยนต์ คุ้มค่า อุ่นใจ (ประเภท MP2s)</h4>
            <br>
            <p><ins><b>คำนิยามศัพท์ที่สำคัญ</b></ins></p>

            <div>
                <span style="margin-left: 1.5cm;">"ผู้เอาประกันภัย"</span>
                <span style="margin-left: 1.6cm;">หมายถึง</span>
                <span style="margin-left: 0.7cm;">บุคคลที่ระบุชื่อเป็นผู้เอาประกันภัยในตาราง</span>
            </div>

            <div>
                <span style="margin-left: 1.5cm;">"รถยนต์"</span>
                <span style="margin-left: 2.6cm;">หมายถึง</span>
                <span style="margin-left: 0.7cm;">รถยนต์ที่เอาประกันภัย ซึ่งมีรายการที่ระบุไว้ในตาราง</span>
            </div>

            <div>
                <span style="margin-left: 1.5cm;">"อุบัติเหตุแต่ละครั้ง"</span>
                <span style="margin-left: 1.2cm;">หมายถึง</span>
                <span style="margin-left: 0.7cm;">เหตุการณ์หนึ่ง หรือหลายเหตุการณ์สืบเนื่องกัน
                    ซึ่งเกิดจากสาเหตุเดียวกัน</span>
            </div>

            <div>
                <span style="margin-left: 1.5cm;">"ความเสียหายส่วนแรก"</span>
                <span style="margin-left: 0.7cm;">หมายถึง</span>
                <span style="margin-left: 0.7cm;">ส่วนแรกของความรับผิด หรือความเสียหายอันมีการคุ้มครอง</span>
            </div>

            <p style="margin-left: 7.1cm;">ตามข้อสัญญา หรือเอกสารแนบท้ายแห่งกรมธรรม์ประกันภัยนี้ที่</p>

            <p style="margin-left: 7.1cm;">ผู้เอาประกันภัยต้องรับผิดชอบเองความคุ้มครองกรมธรรม์ประกันภัยนี้</p>
            <br>

            <div>
                <span><ins><b>ความคุ้มครอง</b></ins></span> <span style="margin-left: 1.5cm;">ให้ความคุ้มครองเป็นจำนวนเงินตามที่ปรากฏในหน้าตารางกรมธรรม์ประกันภัยสำหรับความ</span>
            </div>

            <p style="margin-left: 3.4cm;">เสียหายดังนี้</p>
            <p style="margin-left: 3.4cm;">
                1. <span style="margin-left: 0.2cm;">ความรับผิดชอบต่อบุคคลภายนอก
                    (แต่ไม่รวมถึงผู้ขับขี่ที่ต้องรับผิดตามกฎหมาย ตลอดจนลูกจ้าง คู่</span>
            </p>
            <p style="margin-left: 3.9cm;">สมรส บิดา มารดา บุตรของผู้ขับขี่นั้น)</p>
            <p style="margin-left: 3.9cm;">
                1.1. <span style="margin-left: 0.2cm;">ความเสียหายต่อชีวิต ร่างกาย
                    หรืออนามัย</span>
            </p>
            <p style="margin-left: 4.7cm;">1) <span style="margin-left: 0.2cm;">ค่ารักษาพยาบาลกรณีบาดเจ็บ</span></p>
            <p style="margin-left: 4.7cm;">2) <span style="margin-left: 0.2cm;">ทุพพลภาพถาวรหรือเสียชีวิต</span></p>
            <p style="margin-left: 3.9cm;">1.2. <span style="margin-left: 0.2cm;">ความเสียหายต่อทรัพย์สิน/รถยนต์บุคคลภายนอก</span></p>
            <p style="margin-left: 3.4cm;">
                2. <span style="margin-left: 0.2cm;">ความเสียหายต่อรถยนต์เนื่องจากการชนกับยานพาหนะทางบก
                    และผู้เอาประกันภัยสามารถ</span>
            </p>
            <p style="margin-left: 3.9cm;">แจ้งให้บริษัททราบรายละเอียดของคู่กรณีตามกฎหมายได้</p>
            <p style="margin-left: 4.7cm;">
                ทั้งนี้ยานพาหนะทางบกให้หมายถึง
                เฉพาะรถที่เดินด้วยกำลังเครื่องยนต์โดยใช้พลังงาน
            </p>
            <p style="margin-left: 3.9cm;">เชื้อเพลิง เช่น น้ำมัน ก๊าช หรือกำลังไฟฟ้า และรวมถึงรถพ่วง รถไฟ รถราง</p>
            <p style="margin-left: 3.4cm;">3. <span style="margin-left: 0.2cm;">ความคุ้มครองตามเอกสารแนบท้ายความคุ้มครองเพิ่มเติม</span></p>
            <p style="margin-left: 3.9cm;">3.1 <span style="margin-left: 0.2cm;">การประกันภัยอุบัติเหตุส่วนบุคคล</span></p>
            <p style="margin-left: 3.9cm;">3.2 <span style="margin-left: 0.2cm;">การประกันภัยค่ารักษาพยาบาล</span></p>
            <p style="margin-left: 3.9cm;">3.3 <span style="margin-left: 0.2cm;">การประกันภัยตัวผู้ขับขี่</span></p>
            <p style="margin-left: 3.9cm;">3.4 <span style="margin-left: 0.2cm;">ข้อยกเว้นภัยก่อการร้าย</span></p>
            <p style="margin-left: 3.9cm;">
                3.5 <span style="margin-left: 0.2cm;">ความคุ้มครองการสูญหาย
                    ไฟไหม้โดยสิ้นเชิง</span>
            </p>
            <p style="margin-left: 3.9cm;">
                3.6 <span style="margin-left: 0.2cm;">ความคุ้มครองความเสียหายต่อรถยนต์โดยสิ้นเชิง
                    เนื่องจากการชนกับยานพาหนะทางบก</span>
            </p>
            <p style="margin-left: 3.9cm;">
                3.7 <span style="margin-left: 0.2cm;">ความคุ้มครองความเสียหายต่อรถยนต์โดยสิ้นเชิง
                    เนื่องจากอุบัติเหตุอื่นๆ</span>
            </p>

            <div>
                <span><b>ความเสียหายส่วนแรก</b></span> <span style="margin-left: 0.7cm;">ผู้เอาประกันภัยจะต้องรับผิดชอบตามจำนวนที่ระบุไว้ในกรมธรรม์ประกันภัย</span>
            </div>
        </div>

        <br>
        <br>
        <br>
        <br>
        <!-- Page3 -->
        <div style="page-break-after: always; margin-left: 40px;">
            <p><ins><b>ข้อยกเว้นที่สำคัญ</b></ins></p>

            <div>
                <span style="margin-left: 1.5cm;"><ins><b>หมวดเงื่อนไขทั่วไป</b></ins></span> <span><b>กรมธรรม์ประกันภัยนี้ไม่คุ้มครองความเสียหายหรือความรับผิดอันเกิดขึ้นเป็นผลโดยตรงหรือ</b></span>
                <p><b>โดยอ้อมจาก</b></p>
            </div>

            <p style="margin-left: 1.5cm;">
                <b>1. <span style="margin-left: 0.2cm;">สงคราม การรุกราน การกระทำของชาติศัตรู
                        การสู้รบ หรือการปฏิบัติการที่มีลักษณะเป็นการทำสงคราม (จะได้</span></b>
            </p>
            <p style="margin-left: 2.1cm;"><b>ประกาศสงครามหรือไม่ก็ตาม)</b></p>
            <p style="margin-left: 1.5cm;">
                <b>2. <span style="margin-left: 0.2cm;">สงครามกลางเมือง การแข็งข้อของทหาร
                        การกบฏ การปฏิวัติ การต่อต้าน รัฐบาล การยึดอำนาจการปกครองโดย</span></b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>กำลังทหาร หรือโดยประการอื่น
                    ประชาชนก่อความวุ่นวายถึงขนาดหรือเท่ากับการลุกฮือต่อต้านรัฐบาล</b>
            </p>
            <p style="margin-left: 1.5cm;"><b>3. <span style="margin-left: 0.2cm;">วัตถุอาวุธปรมาณู</span></b></p>
            <p style="margin-left: 1.5cm;">
                <b>4. <span style="margin-left: 0.2cm;">การแตกตัวของประจุ การแผ่รังสี
                        การกระทบกับกัมมันตภาพรังสีจากเชื้อเพลิงปรมาณู หรือจากกากปรมาณู อัน</span></b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>เกิดจากการเผาไหม้เชื้อเพลิงปรมาณู และสำหรับจุดประสงค์ของข้อสัญญานี้
                    การเผานั้นรวมถึงกรรมวิธีใดๆแห่ง</b>
            </p>
            <p style="margin-left: 2.1cm;"><b>การแตกแยกตัวปรมาณู ซึ่งดำเนินติดต่อกันไปด้วยตัวของมันเอง</b></p>

            <p style="margin-left: 1.5cm;"><ins><b>หมวดการคุ้มครองความรับผิดต่อบุคคลภายนอก</b></ins></p>
            <p style="margin-left: 1.5cm;">
                <b>1. <span style="margin-left: 0.2cm;">ความเสียหายต่อทรัพย์สินดังต่อไปนี้
                        จะไม่ได้รับความคุ้มครอง</span></b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>1.1 <span style="margin-left: 0.2cm;">ทรัพย์สินที่ผู้เอาประกันภัย
                        ผู้ขับขี่เป็นฝ่ายต้องรับผิดตามกฎหมาย คู่สมรส บิดา มารดา บุตรของผู้เอา</span></b>
            </p>
            <p style="margin-left: 2.9cm;">
                <b>ประกันภัยหรือผู้ขับขี่นั้นเป็นเจ้าของ หรือเป็นผู้เก็บรักษา ควบคุม
                    หรือครอบครอง</b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>1.2 <span style="margin-left: 0.2cm;">เครื่องชั่ง สะพานรถ สะพานรถไฟ ถนน
                        ทางวิ่ง ทางเดิน สนาม หรือสิ่งหนึ่งสิ่งใดที่อยู่ใต้สิ่งดังกล่าว อันเกิด</span></b>
            </p>
            <p style="margin-left: 2.9cm;"><b>จากการสั่นสะเทือน หรือจากน้ำหนักรถยนต์ หรือน้ำหนักบรรทุกของรถยนต์</b></p>
            <p style="margin-left: 2.1cm;">
                <b>1.3 <span style="margin-left: 0.2cm;">สัมภาระหรือทรัพย์สินอื่นใดที่นำติดตัวขึ้นบนรถยนต์
                        หรือทรัพย์สินที่บรรทุกอยู่ในรถยนต์ หรือกำลังยกขึ้น</span></b>
            </p>
            <p style="margin-left: 2.9cm;">
                <b>หรือกำลังยกลง จากรถยนต์
                    หรือทรัพย์สินที่รถยนต์กำลังยกจากที่หนึ่งไปยังอีกที่หนึ่ง</b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>1.4 <span style="margin-left: 0.2cm;">ทรัพย์สินที่ได้รับความเสียหายจากการรั่วไหลของสารเคมีหรือวัตถุอันตรายที่บรรทุกอยู่ในรถยนต์
                        เว้นแต่</span></b>
            </p>
            <p style="margin-left: 2.9cm;">
                <b>การรั่วไหลนั้นเกิดจากอุบัติเหตุจากรถยนต์
                    หรือการรั่วไหลของแก๊สหรือเชื้อเพลิงเพื่อการเดินเครื่องของ</b>
            </p>
            <p style="margin-left: 2.9cm;"><b>รถยนต์</b></p>

            <p style="margin-left: 1.5cm;">
                <b>2. <span style="margin-left: 0.2cm;">การประกันภัยตามหมวดนี้
                        ไม่คุ้มครองความรับผิดอันเกิดจาก</span></b>
            </p>
            <p style="margin-left: 2.1cm;"><b>2.1 <span style="margin-left: 0.2cm;">การใช้รถยนต์นอกอาณาเขตคุ้มครอง</span></b></p>
            <p style="margin-left: 2.1cm;">
                <b>2.2 <span style="margin-left: 0.2cm;">การใช้รถยนต์ในทางผิดกฎหมาย เช่น
                        ใช้รถยนต์ไปปล้นทรัพย์ ชิงทรัพย์ หรือใช้ขนยาเสพติด เป็นต้น</span></b>
            </p>
            <p style="margin-left: 2.1cm;"><b>2.3 <span style="margin-left: 0.2cm;">การใช้ในการแข่งขันความเร็ว</span></b></p>
            <p style="margin-left: 2.1cm;">
                <b>2.4 <span style="margin-left: 0.2cm;">การใช้ลากจูงหรือผลักดัน
                        เว้นแต่รถที่ถูกลากจูงหรือถูกผลักดันได้ประกันภัยไว้กับบริษัทด้วย หรือเป็นรถลาก</span></b>
            </p>
            <p style="margin-left: 2.9cm;"><b>จูงโดยสภาพ หรือรถที่มีระบบห้ามล้อเชื่อมโยงถึงกัน</b></p>
            <p style="margin-left: 2.1cm;">
                <b>2.5 <span style="margin-left: 0.2cm;">ความรับผิดซึ่งเกิดจากสัญญาที่ผู้ขับขี่ทำขึ้น
                        ซึ่งถ้าไม่มีสัญญานั้นแล้ว ความรับผิดของผู้ขับขี่จะไม่เกิดขึ้น</span></b>
            </p>
            <p style="margin-left: 2.1cm;">
                <b>2.6 <span style="margin-left: 0.2cm;">การขับขี่โดยบุคคลซึ่งในขณะขับขี่มีปริมาณแอลกอฮอล์ในเลือดเกินกว่า
                        50 มิลลิกรัมเปอร์เซ็นต์</span></b>
            </p>
        </div>

        <br>
        <br>
        <br>
        <!-- Page4 -->
        <div style="page-break-after: always; margin-left: 40px;">
            <p style="margin-left: 1.5cm;"><ins><b>หมวดการคุ้มครองความเสียหายต่อรถยนต์โดยสิ้นเชิงเนื่องจากการชนกับยานพาหนะทางบก</b></ins></p>
            <p><ins><b>ข้อ 1 ความคุ้มครอง</b></ins></p>
            <br>

            <p>
                บริษัทจะชดใช้ค่าสินไหมทดแทนความเสียหายที่เกิดขึ้นระหว่างระยะเวลาประกันภัย
                ต่อรถยนต์ที่เสียหายโดยสิ้นเชิง
                อันมี
            </p>
            <p>
                <span>สาเหตุจากการชนกับยานพาหนะทางบก ไม่ว่าจะเกิดจากความประมาทของรถยนต์คันที่เอาประกันภัยหรือคู่กรณี
                    และผู้เอา</span>
            </p>
            <p>ประกันภัยสามารถแจ้งให้บริษัททราบรายละเอียดคู่กรณีอีกฝ่ายได้</p>
            <p>คุ้มครองความเสียหายสิ้นเชิงต่อรถยนต์โดยสิ้นเชิงเนื่องจากการชนกับยานพาหนะทางบก 100,000 บาท</p>
            <p>
                ยานพาหนะทางบกตามกรมธรรม์ประกันภัยนี้ ให้หมายถึงเฉพาะรถที่เดินด้วยกำลังเครื่องยนต์
                โดยใช้พลังงานเชื้อเพลิง เช่น
            </p>
            <p>น้ำมัน ก๊าซ หรือกำลังไฟฟ้า และรวมถึงรถพ่วง รถไฟ รถราง</p>
            <p>
                รถยนต์เสียหายสิ้นเชิง หมายถึง
                รถยนต์ได้รับความเสียหายจนไม่อาจซ่อมให้อยู่ในสภาพเดิมได้หรือเสียหายไม่น้อยกว่าร้อยละ
            </p>
            <p>70 ของมูลค่ารถยนต์ในขณะเกิดความเสียหาย</p>
            <p>
                ในกรณีที่เอาประกันภัยไว้กับบริษัทไม่ต่ำกว่าร้อยละ 80 ของมูลค่ารถยนต์ในขณะที่เอาประกันภัย
                หรือผู้รับประโยชน์แล้วแต่
            </p>
            <p>
                กรณี ต้องโอนกรรมสิทธิ์รถยนต์ให้แก่บริษัททันที
                โดยค่าใช้จ่ายของบริษัทและให้ถือว่าการคุ้มครองนั้นเป็นอันสิ้นสุด
            </p>
            <p><ins><b>ข้อ 2 การสละสิทธิ</b></ins></p>
            <p>
                ในกรณีที่มีความเสียหายต่อรถยนต์ เมื่อบุคคลอื่นเป็นผู้ใช้รถยนต์โดยได้รับความยินยอมจากผู้เอาประกันภัย
                บริษัทสละสิทธิใน
            </p>
            <p>
                การไล่เบี้ยจากผู้ใช้รถยนต์นั้น เว้นแต่การใช้โดยบุคคลของสถานให้บริการเกี่ยวกับการซ่อมแซมรถ
                การทำความสะอาด
                การ
            </p>
            <p>บำรุงรักษารถ หรือการติดตั้งอุปกรณ์เพิ่มเติม เมื่อรถยนต์ได้ส่งมอบให้เพื่อรับบริการนั้น</p>

            <span><ins><b>ข้อ 3 การยกเว้นความเสียหายต่อรถยนต์</b></ins></span> <span>การประกันภัยนี้ไม่คุ้มครองความเสียหายของรถยนต์ที่เอาประกันภัยเกิดจาก</span>
            <p>3.1 อุบัติเหตุจากการพลิกคว่ำ หรือหักหลบสิ่งกีดขวางของรถยนต์คันที่เอาประกันภัย</p>
            <p>3.2 อุบัติเหตุที่เกิดขึ้นเพราะการใช้รับจ้างสาธารณะ</p>
            <p>
                3.3 กรณีเฉี่ยวชนกับวัตถุใดๆ ที่ไม่ใช่ยานพาหนะทางบก เช่น เสาไฟฟ้า หรือสิ่งมีชีวิตอื่นๆเช่น คน สุนัข
                เป็นต้น
            </p>
            <p>
                3.4 อุบัติเหตุที่เกิดขึ้นและ/หรือเป็นผลมาจากรถยนต์ที่เอาประกันภัยถูกลักทรัพย์ ชิงทรัพย์ ปล้นทรัพย์
                ยักยอกทรัพย์ ฉ้อโกง
            </p>
            <p>กรรโชกทรัพย์ หรือรีดทรัพย์</p>
            <p>3.5 การเสื่อมราคา หรือการสึกหรอของรถยนต์</p>
            <p>
                3.6 การแตกหักของเครื่องจักรกลไกของรถยนต์ หรือการหยุดเดินของเครื่องจักรกลไล
                หรือเครื่องไฟฟ้ารถยนต์อันมิได้เกิด
            </p>
            <p>จากอุบัติเหตุ</p>
            <p>
                3.7 ความเสียหายโดยตรงต่อรถยนต์ อันเกิดจากการบรรทุกน้ำหนัก หรือจำนวนผู้โดยสารเกินกว่าที่ได้รับอนุญาต
                อันมิได้เกิด
            </p>
            <p>จากอุบัติเหตุ</p>
            <p>
                3.8 ความเสียหายอันเกิดจากการขาดการใช้รถยนต์ เว้นแต่การขาดการใช้รถยนต์นั้นเกิดจากบริษัทประวิงการซ่อม
                หรือซ่อม
            </p>
            <p>ล่าช้าเกินกว่าที่ควรจะเป็นโดยไม่มีเหตุผลอันสมควร</p>
            <span><ins><b>ข้อ 4 การยกเว้นการใช้</b></ins></span> <span>:
                การประกันภัยนี้ไม่คุ้มครอง</span>
            <br>
            <br>
            <p style="margin-left: 1.5cm;"><b>1. </b><span style="margin-left: 0.2cm;">การใช้รถยนต์นอกอาณาเขตที่คุ้มครอง</span></p>
            <p style="margin-left: 1.5cm;">
                <b>2. </b><span style="margin-left: 0.2cm;">การใช้รถยนต์ไปในทางที่ผิดกฎหมาย
                    เช่น ใช้รถยนต์ไปปล้นทรัพย์ ชิงทรัพย์ หรือ ใช้ขนยาเสพย์ติด เป็นต้น</span>
            </p>
            <p style="margin-left: 1.5cm;"><b>3. </b><span style="margin-left: 0.2cm;">การใช้ในการแข่งขันความเร็ว</span></p>
        </div>

        <br>
        <br>
        <br>
        <!-- Page5 -->
        <div style="page-break-after: always; margin-left: 40px;">
            <span><ins><b>ข้อ 5 การยกเว้นการใช้อื่นๆ</b></ins></span> <span>: การประกันภัยนี้ไม่คุ้มครอง</span>
            <br>
            <br>
            <p style="margin-left: 1.5cm;">
                <b>1. </b><span style="margin-left: 0.2cm;">การลากจูง หรือผลักดัน
                    เว้นแต่รถที่ถูกลากจูง หรือถูกผลักดันได้ประกันภัยไว้กับบริษัทด้วย หรือเป็นรถลากจูง</span>
            </p>
            <p style="margin-left: 2.1cm;">โดยสภาพ หรือรถที่มีระบบห้ามล้อเชื่อมโยงถึงกัน</p>
            <p style="margin-left: 1.5cm;"><b>2. </b><span style="margin-left: 0.2cm;">การใช้รถยนต์นอกเหนือจากที่ระบุไว้ในตารางในขณะเกิดอุบัติเหตุ</span></p>
            <p style="margin-left: 1.5cm;">
                <b>3. </b><span style="margin-left: 0.2cm;">การขับขี่โดยบุคคลซึ่งในขณะขับขี่มีปริมาณแอลกอฮอล์ในเลือดเกินกว่า
                    50 มิลลิกรัมเปอร์เซ็นต์ ซึ่งเป็นไปตาม</span>
            </p>
            <p style="margin-left: 2.1cm;">
                กฎกระทรวงฉบับที่ 16 (พ.ศ.2537) ออกตามความในพระราชบัญญัติ จราจรทางบก พ.ศ. 2522
                กำหนดให้ถือว่าเมา
            </p>
            <p style="margin-left: 2.1cm;">สุรา</p>
            <p style="margin-left: 1.5cm;">
                <b>4. </b><span style="margin-left: 0.2cm;">การขับขี่โดยบุคคลที่ไม่เคยได้รับอนุญาติขับขี่ใดๆ
                    หรือเคยได้รับแต่ถูกตัดสิทธิตามกฎหมาย หรือใช้ใบขับขี่</span>
            </p>
            <p style="margin-left: 2.1cm;">รถจักรยานยนต์ไปขับขี่รถยนต์</p>
            <p style="margin-left: 2.1cm;"><b><ins>หมวดความคุ้มครองเสียหายต่อรถยนต์โดยสิ้นเชิง เนื่องจากอุบัติเหตุอื่นๆ</ins></b></p>

            <span><ins><b>ข้อ ความคุ้มครอง</b></ins></span>
            <br>
            <br>
            <p>
                บริษัทจะชดใช้ค่าสินไหมทดแทนความเสียหายที่เกิดนขึ้นระหว่างระยะเวลาประกันภัย
                ต่อรถยนต์ที่เสียหายโดยสิ้นเชิง อันมี
            </p>
            <p>สาเหตุจากอุบัติเหตุ แต่ไม่รวมถึงความเสียหายที่เกิดจากไฟไหม้และการชนกับยานพาหนะทางบก</p>
            <p>ความคุ้มครองความเสียหายสิ้นเชิงต่อรถยนต์โดยสิ้นเชิงเนื่องจากอุบัติเหตุอื่นๆ 100,000 บาท</p>
            <p>
                ไฟไหม้ในที่นี้หมายถึงความเสียหายต่อรถยนต์ที่เป็นผลมาจากไฟไหม้ไม่ว่าจะเป็นการไหม้โดยตัวเอง
                หรือการไหม้ที่เป็นผลสืบ
            </p>
            <p>เนื่องมาจากสาเหตุอื่นใด</p>
            <p>
                ยานพาหนะทางบกตามกรมธรรม์ประกันภัยนี้ ให้หมายถึงเฉพาะรถที่เดินด้วยกำลังเครื่องยนต์
                โดยใช้พลังงานเชื้อเพลิง เช่น
            </p>
            <p>น้ำมัน ก๊าซ หรือกำลังไฟฟ้า และรวมถึงรถพ่วง รถไฟ รถราง</p>
            <p>
                รถยนต์เสียหายสิ้นเชิง หมายถึง
                รถยนต์ได้รับความเสียหายจนไม่อาจซ่อมให้อยู่ในสภาพเดิมได้หรือเสียหายไม่น้อยกว่าร้อยละ
            </p>
            <p>70 ของมูลค่ารถยนต์ในขณะเกิดความเสียหาย</p>
            <p>
                ในกรณีที่เอาประกันภัยไว้กับบริษัทไม่ต่ำกว่าร้อยละ 80 ของมูลค่ารถยนต์ในขณะที่เอาประกันภัย
                หรือผู้รับประโยชน์แล้วแต่
            </p>
            <p>
                กรณี
                ต้องโอนกรรมสิทธิ์รถยนต์ให้แก่บริษัททันที
                โดยค่าใช้จ่ายของบริษัทและให้ถือว่าการคุ้มครองนั้นเป็นอันสิ้นสุด
            </p>
            <p style="margin-left: 2.1cm;">
                <b><ins>หมวดความคุ้มครองเสียหายต่อรถยนต์โดยสิ้นเชิง
                        เนื่องจากการสูญหายไฟไหม้โดยสิ้นเชิง</ins></b>
            </p>
            <p><ins><b>ข้อ ความคุ้มครอง</b></ins></p>
            <span>1.1</span> <span style="margin-left: 0.2cm;">รถยนต์สูญหาย
                บริษัทจะชดใช้ค่าสินไหมทดแทน เมื่อรถยนต์สูญหายไป อันเกิดจากการกระทำความผิดฐานลักทรัพย์ ชิง</span>
            <p style="margin-left: 0.7cm;">
                ทรัพย์ ปล้นทรัพย์ ยักยอกทรัพย์
                หรือเกิดความเสียหายต่อรถยนต์โดยสิ้นเชิงอันเกิดจากการกระทำผิด หรือการพยายาม
            </p>
            <p style="margin-left: 0.7cm;">กระทำผิดเช่นว่านั้น</p>
            <span>1.2</span> <span style="margin-left: 0.2cm;">รถยนต์ไฟไหม้โดยสิ้นเชิง บริษัทจะชดใช้สินไหมทดแทน
                เมื่อรถยนต์เกิดความเสียหายจากไฟไหม้โดยสิ้นเชิง ไม่ว่าจะเป็น</span>
            <p style="margin-left: 0.7cm;">การไหม้โดยตัวของมันเองหรือการไหม้ที่เป็นผลสืบเนื่องจากสาเหตุใดๆ ก็ตาม</p>
            <p style="margin-left: 0.7cm;">
                คุ้มครองความเสียหายสิ้นเชิงต่อรถยนต์โดยสิ้นเชิงเนื่องจากการสูญหาย
                ไฟไหม้โดยสิ้นเชิง 100,000 บาท
            </p>
        </div>

        <br>
        <br>
        <br>
        <!-- Page6 -->
        <div style="page-break-after: always; margin-left: 40px;">
            <p><ins><b>เงื่อนไขที่สำคัญ</b></ins></p>
            <span>ข้อ 1.</span> <span style="margin-left: 0.4cm;"><span style="color: red"><ins>กรมธรรม์ประกันภัยนี้มีผลใช้บังคับทันทีเมื่อผู้เอาประกันภัยชำระเบี้ยประกันภัยแล้วตามเงื่อนไขและมีผลสิ้นสุดเมื่อ</ins></span></span>
            <p style="color: red"><ins>ไม่มีการชำระเบี้ยตามวันเวลาที่กำหนด</ins></p>
            <p style="margin-left: 1.2cm;">
                การชำระเบี้ยประกันภัยให้แก่ตัวแทนประกันภัย
                พนักงานและนายหน้าประกันภัยผู้ได้รับมอบอำนาจให้รับชำระเบี้ย
            </p>
            <p>
                ประกันภัย
                ตลอดจนบุคคลหรือนิติบุคคลที่บริษัทยอมรับการกระทำของบุคคลหรือนิติบุคคลดังกล่าวเสมือนตัวแทนของบริษัท ให้
            </p>
            <p>ถือว่าเป็นการชำระเบี้ยประกันภัยแก่บริษัทโดยถูกต้อง</p>
            <span>ข้อ 2.</span> <span style="margin-left: 0.4cm;"><ins>การจัดการเรียกร้องเมื่อเกิดความเสียหาย</ins></span>
            <br>
            <br>
            <p style="margin-left: 1.2cm;">
                เมื่อมีความเสียหาย หรือความรับผิดตามกรมธรรม์ประกันภัยเกิดขึ้น ผู้เอาประกันภัย
                หรือผู้ขับขี่จะต้องแจ้งให้บริษัท
            </p>
            <p>ทราบโดยไม่ชักช้า และดำเนินการอันจำเป็นเพื่อรักษาสิทธิตามกฎหมาย</p>
            <p style="margin-left: 1.2cm;">
                บริษัทมีสิทธิเข้าดำเนินการในนามของผู้เอาประกันภัยเกี่ยวกับอุบัติเหตุที่เกิดขึ้นได้
                หากความเสียหายที่เกิดขึ้นนั้นอยู่
            </p>
            <p>ภายใต้ความคุ้มครองในกรมธรรม์ประกันภัย</p>
            <p style="margin-left: 1.2cm;">ความคุ้มครองของบริษัทจะเกิดขึ้นเมื่อผู้เอาประกันภัยหรือผู้ขับขี่ดำเนินการโดยสุจริต</p>
            <span>ข้อ 3.</span> <span style="margin-left: 0.4cm;"><ins>การแก้ไข</ins></span>
            <br>
            <br>
            <p style="margin-left: 1.2cm;">
                สัญญาคุ้มครองและเงื่อนไขแห่งกรมธรรม์ประกันภัยนี้
                จะเปลี่ยนแปลงแก้ไขได้โดยเอกสารแนบท้ายของบริษัทเท่านั้น
            </p>
            <br>
            <br>

            <p>
                <b>กรุณาตรวจสอบเงื่อนไขความคุ้มครอง และข้อยกเว้น จากกรมธรรม์ประกันภัยโดยละเอียด
                    หากมีข้อความใดในเอกสารนี้ ขัด</b>
            </p>
            <p><b>หรือแย้งกับข้อความที่ปรากฏในกรมธรรม์ประกันภัยให้ใช้ข้อความตามที่ปรากฏในกรมธรรม์ประกันภัยบังคับแทน</b></p>
        </div>--%>
    </form>
</body>
</html>

<script>
    $(function () {
        window.print();
    });
</script>