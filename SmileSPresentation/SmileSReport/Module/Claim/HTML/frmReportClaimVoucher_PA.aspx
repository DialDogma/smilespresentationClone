<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportClaimVoucher_PA.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.frmReportClaimVoucher_PA" %>

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

         @media print {
            footer{
                font: 8.5pt arial, tahoma, sans-serif;
                position: fixed;
                bottom: 0;
            }
        }

        Table, Tr, Td {
            font: 8pt arial, tahoma, sans-serif;
            /*font: 9pt, tahoma;*/
            border-bottom: 1px;
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
            <%-- Header --%>
            <table style="width: 100%">
                <tr>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%">
                        <asp:Label runat="server" ID="lblHeader" Font-Size="Large" Text="หนังสือรับรองการจ่าย"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
            <table style="width: 100%; margin-left: 40px">
                <tr>
                    <td style="width: 12.5%">
                        <br />
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">
                        <asp:Label runat="server" ID="lblCreateDate" Font-Size="XX-Small"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" rowspan="5">
                        <span>
                            <img src="../../../Image/logoSiamSmile.jpg" /></span>
                    </td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; margin-left: 50px" colspan="1">
                        <a>เรียน ผู้อำนวยการ</a>
                    </td>
                    <td style="width: 12.5%" colspan="2">
                        <asp:Label runat="server" ID="lblHospitalName"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; margin-left: 50px" colspan="5">
                        <a>เรื่อง ขอรับรองการพิจารณาสินไหมการรักษาพยาบาลของ</a>
                        &nbsp&nbsp&nbsp
                        <asp:Label runat="server" ID="lblCustomerName"></asp:Label>
                    </td>
                    <td style="width: 12.5%" colspan="2"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 25%; margin-left: 50px" colspan="3">
                        <a>อ้างถึง ใบแจ้งรายการค่ารักษา</a>
                    </td>
                    <td style="width: 27.5%" colspan="4">
                        <asp:Label runat="server" ID="lblReference"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 12.5%; margin-left: 50px" colspan="2">
                        <a>แผน</a>
                        &nbsp&nbsp&nbsp
                        <asp:Label runat="server" ID="lblProduct"></asp:Label>
                    </td>
                    <td style="width: 12.5%" colspan="5">
                        <a>เข้ารับการรักษาตั้งแต่วันที่</a>
                        &nbsp&nbsp
                        <asp:Label runat="server" ID="lblDateBetween"></asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
            </table>
            <%-- body detail --%>
            <table style="width: 90%; border-spacing: 0; margin-left: 35px">
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden"></td>
                    <th style="width: 50%; border: solid 1px black; border-right-style: hidden">
                        <a>รายการ</a>
                    </th>
                    <th style="width: 15%; border: solid 1px black; border-right-style: hidden">
                        <a>รายการเบิก</a>
                    </th>
                    <th style="width: 15%; border: solid 1px black; border-right-style: hidden">
                        <a>สิทธิ์เบิก</a>
                    </th>
                    <th style="width: 15%; border: solid 1px black">
                        <a>ส่วนเกินสิทธิ์</a>
                    </th>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">1
                    </td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblFirst"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListFirst" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimFirst" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessFirst" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">2
                    </td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblSecend"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListSecond" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimSecond" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessSecond" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">3
                    </td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblThird"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListThird" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimThird" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessThird" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">4
                    </td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblForth"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListForth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimForth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessForth" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">5
                    </td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblFifth"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListFifth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimFifth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessFifth" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">6</td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblSixth" Text=""></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListSixth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimSixth" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessSixth" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr id="TRSeventh" runat="server">
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; text-align: center">&nbsp;7</td>
                    <td style="width: 50%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblSeventh"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListSeventh" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimSeventh" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessSeventh" Text="-"></asp:Label>
                    </td>
                </tr>
                <%-- ส่วนลด/รวม --%>
                <tr>
                    <th style="width: 55%; text-align: center; border: solid 1px black; border-right-style: hidden; border-top-style: hidden" colspan="2">
                        <a>ส่วนลด</a>
                    </th>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden" colspan="2">
                        <asp:Label runat="server" ID="lblCanClaimDiscount" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessDiscount" Text="-"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th style="width: 55%; text-align: center; border: solid 1px black; border-right-style: hidden; border-top-style: hidden" colspan="2">
                        <a>รวม</a>
                    </th>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimListTotal" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblCanClaimTotal" Text="-"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblClaimExcessTotal" Text="-"></asp:Label>
                    </td>
                </tr>
                <%-- total --%>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <asp:Label runat="server" ID="lblTotalText" Text="สรุป**"></asp:Label>
                    </td>
                    <td style="width: 55%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; border-left-style: hidden" colspan="1">
                        <asp:Label runat="server" ID="lblTotalFirst" Text="ค่าใช้จ่ายทั้งสิ้น"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden; border-right-style: hidden">
                        <asp:Label runat="server" ID="lblTotalFirstResult" Text="-"></asp:Label>
                    </td>
                    <td style="border: solid 1px black; border-right-style: hidden; border-bottom-style: hidden; border-top-style: hidden" rowspan="5" colspan="2">
                        <asp:Label runat="server" ID="lblCustomerCompensate"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 55%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; border-left-style: hidden" colspan="1">
                        <asp:Label runat="server" ID="lblTotalSecond" Text="สิทธิ์ความคุ้มครอง"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden; border-right-style: hidden">
                        <asp:Label runat="server" ID="lblTotalSecondResult" Text="-"></asp:Label>
                    </td>
                    <td style="border: none"></td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 55%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; border-left-style: hidden" colspan="1">
                        <asp:Label runat="server" ID="lblTotalThird" Text="ส่วนเกินลูกค้าชำระเอง"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden; border-right-style: hidden">
                        <asp:Label runat="server" ID="lblTotalThidResult" Text="-"></asp:Label>
                    </td>
                    <td style="border: none"></td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 55%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; border-left-style: hidden" colspan="1">
                        <asp:Label runat="server" ID="lblTotalFourth"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden; border-right-style: hidden">
                        <asp:Label runat="server" ID="lblTotalFourthResult" Text="-"></asp:Label>
                    </td>
                    <td style="border: none"></td>
                </tr>
                <tr>
                    <td style="width: 5%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 55%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden; border-left-style: hidden" colspan="1">
                        <asp:Label runat="server" ID="lblTotalFifth"></asp:Label>
                    </td>
                    <td style="width: 15%; text-align: right; padding: 5px; border: solid 1px black; border-top-style: hidden; border-right-style: hidden">
                        <asp:Label runat="server" ID="lblTotalFifthResult" Text="-"></asp:Label>
                    </td>
                    <td style="border: none"></td>
                </tr>
            </table>
            <%-- footer --%>
            <table style="width: 100%; margin-left: 35px">
                <tr>
                    <td>
                        <br />
                    </td>
                    <td style="margin-left: 25px; padding-top: 5px">
                        <a>จึงเรียนมาเพื่อโปรดพิจารณา และขอขอบพระคุณในความร่วมมือด้วยดี</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <%-- signature field --%>
            <table style="width: 90%">
                <tr>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center">
                        <a>ขอแสดงความนับถือ</a>
                    </td>
                    <td style="width: 33.33%; text-align: center">
                        <a>รับทราบ</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center">
                        <br />
                    </td>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center"></td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center">
                        <br />
                    </td>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center"></td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center">
                        <br />
                    </td>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center"></td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center">
                        <a>Consult.........................................</a>
                    </td>
                    <td style="width: 33.33%; text-align: center">
                        <a>..........................................</a>
                    </td>
                    <td style="width: 33.33%; text-align: center">
                        <a>..........................................</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center">
                        <asp:Label runat="server" ID="lblApproveName" Text="Test Test"></asp:Label>
                    </td>
                    <td style="width: 33.33%; text-align: center">
                        <asp:Label runat="server" ID="lblCustomerName2" Text="Tesst Tesst"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center">
                        <a>ผู้อนุมัติ</a>
                    </td>
                    <td style="width: 33.33%; text-align: center">
                        <a>ผู้เอาประกัน</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33.33%; text-align: center">
                        <br />
                    </td>
                    <td style="width: 33.33%; text-align: center"></td>
                    <td style="width: 33.33%; text-align: center"></td>
                </tr>
            </table>
            <%-- Address Text --%>
            <table style="width: 90%; border-collapse: collapse; border: solid black; margin-left: 35px">
                <tr>
                    <th>
                        <asp:Label runat="server" ID="lblContactInfo" Text="Testttttttttt"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label runat="server" ID="lblContactAddress1" Text="Testtttttt2"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label runat="server" ID="lblContactAddress2" Text="tesssst3"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label runat="server" ID="lblContactAddress3" Text="tesstt4"></asp:Label>
                    </td>
                </tr>
            </table>
            <%-- notice --%>
            <table style="width: 90%; margin-left: 35px">
                <tr>
                    <td colspan="3" style="text-align: center">
                        <asp:Label ID="lblOverFromCompany" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; vertical-align: top; text-align: left"><a>หมายเหตุ</a></td>
                    <td style="width: 90%">
                        <asp:Label runat="server" ID="lblRemark" Text="ttttttttttttttttttttttttttt<br />tttttttttttttttttttttt<br />tttttttttttttttttttttttt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <%-- Time stamp info --%>
            <table style="width: 90%; border-spacing: 0; margin-left: 35px">
                <tr>
                    <th style="width: 25%; border: solid 1px black; border-right-style: hidden">
                        <a>เวลารับเอกสารจาก ร.พ.</a>
                    </th>
                    <th style="width: 25%; border: solid 1px black; border-right-style: hidden">
                        <a>ขอเอกสารเพิ่มจาก ร.พ.</a>
                    </th>
                    <th style="width: 25%; border: solid 1px black; border-right-style: hidden">
                        <a>เวลาที่ได้รับเอกสารครบ</a>
                    </th>
                    <th style="width: 25%; border: solid 1px black">
                        <a>แจ้งกลับ ร.พ.</a>
                    </th>
                </tr>
                <tr>
                    <td style="width: 25%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden">
                        <br />
                        <br />
                        <br />
                    </td>
                    <td style="width: 25%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 25%; border: solid 1px black; border-right-style: hidden; border-top-style: hidden"></td>
                    <td style="width: 25%; border: solid 1px black; border-top-style: hidden"></td>
                </tr>
            </table>
            <%-- Doc info --%>
            <table style="width: 90%; margin-left: 35px">
                <tr>
                    <td style="width: 12.5%; font-size: x-small; color: #808080">
                        <asp:Label runat="server" ID="lblComment"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <footer>
        <table>
            <tr>
                <td style="width: 10%">QF-CLM-COP-002</td>
                <td style="width: 12.5%;"></td>
                <td style="width: 12.5%; text-align: right;">Effective Date 1 Sep 2019</td>
            </tr>
        </table>
    </footer>
    </form>
</body>
</html>