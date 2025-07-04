<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportClaimGivePower.aspx.cs" Inherits="SmileSReport.Module.Claim.HTML.WebForm2" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
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
            line-height: 1.5;
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

        u {
            border-bottom: 1px dotted #000;
            text-decoration: none;
        }
        .auto-style1 {
            width: 100%;
        }
        .txtZCardId{
            width: 12px;
            height: 15px;
            border: 1px solid black;
            text-align: center;
        }
        .trSignature{
            text-align:center;
        }
        .auto-style3 {
            width: 62%;
        }
        .auto-style4 {
            width: 51%;
        }

        .underLineBorder{
            position: absolute;
            padding-left: 30px;
            line-height: 9px;
        }
        
        .underLineBorderH{
            position: absolute;
            padding-left: 10px;
            line-height: 9px;
            text-align: left;
        }
               
        </style>
    <title></title>
</head>
<body onload="window.print()">
    <form id="form1" runat="server">
        <div class="page">
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%; text-align: center; font-size: larger" colspan="2">
                        <asp:Label runat="server" ID="lblHeader">
                            <b>หนังสือมอบอำนาจ</b>
                        </asp:Label>
                    </td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="4">เขียนที่
                        <asp:Label runat="server" ID="lblHospitalHeader" CssClass="underLineBorderH"></asp:Label>.......................................................................
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%" colspan="4">วันที่ &nbsp;..........&nbsp; เดือน..............................&nbsp; พ.ศ.25...........</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                    <td style="width: 12.5%"></td>
                    <td style="width: 12.5%"></td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left; height: 10px" colspan="6">
                        <p>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;โดยหนังสือฉบับนี้ ข้าพเจ้า
                       <%-- <asp:Label runat="server" ID="lblFullname"></asp:Label>--%>
                            <asp:Label runat="server" ID="lblFullname" CssClass="underLineBorder"></asp:Label>
                            ............................................................
                            &nbsp;
                         <%--<asp:Label runat="server" ID="lblLastname"></asp:Label>--%>
                            &nbsp;ที่อยู่&nbsp;เลขที่&nbsp;
                            <%--<asp:Label runat="server" ID="lblFullAddressDetail"></asp:Label>--%>
                            <asp:Label runat="server" ID="lblAddressNo" CssClass="underLineBorder"></asp:Label>
                            .......................
                            &nbsp;หมู่ที่&nbsp;
                            <asp:Label runat="server" ID="lblAddressMoo" CssClass="underLineBorder"></asp:Label>
                            ...................
                        </p>
                        <p>
                            &nbsp;แขวง/ตำบล&nbsp;
                            <asp:Label runat="server" ID="lblAdressTumbol" CssClass="underLineBorder"></asp:Label>
                            .............................................
                            &nbsp;เขต/อำเภอ&nbsp;
                            <asp:Label runat="server" ID="lblAddressAmphoe" CssClass="underLineBorder"></asp:Label>
                            .............................................
                            &nbsp;จังหวัด&nbsp;
                            <asp:Label runat="server" ID="lblAddressProvince" CssClass="underLineBorder"></asp:Label>
                            ....................................
                        </p>
                        <p>
                             บัตรประชาชนเลขที่ &nbsp;
                        <%--<asp:Label runat="server" ID="lblZCardId"></asp:Label>--%>
                            <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum0" runat="server"></asp:TextBox>
                             - <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum1" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum2" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum3" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum4" runat="server"></asp:TextBox>
                             - <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum5" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum6" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum7" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum8" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum9" runat="server"></asp:TextBox>
                             - <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum10" runat="server"></asp:TextBox>
                             <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum11" runat="server"></asp:TextBox>
                             - <asp:TextBox Class="txtZCardId" ID="txtZCardIdNum12" runat="server"></asp:TextBox>
                            &nbsp;
                            ผู้เอาประกันภัย / ผู้รับผลประโยชน์ / 
                        </p>
                        <p>
                            ผู้มีอำนาจกระทำการแทน (ซึ่งต่อไปนี้จะเรียกว่า <b>"ผู้มอบอำนาจ"</b>) ตามสัญญากรมธรรม์ประกันภัย &nbsp;
                            เลขที่ 
                        <asp:Label runat="server" ID="lblAppId" CssClass="underLineBorder"></asp:Label>
                            .............................
                        </p>
                        <p>
                            ของ
                        <asp:Label runat="server" ID="lblInsuranceCompany" CssClass="underLineBorder"></asp:Label>
                            .....................................................................................
                            &nbsp;
                        ผู้รับประกันภัย ขอมอบอำนาจให้ บริษัท สไมล์ ทีพีเอ จำกัด 
                        </p>
                        <p>
                            ทะเบียนเลขที่ 0575554000911 เป็นตัวแทนของข้าพเจ้า เพื่อกระทำการ ดังต่อไปนี้
                        </p>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">1. ติดต่อประสานงานกับบริษัทผู้รับประกันภัยดังกล่าวข้างต้น เกี่ยวกับการเรียกร้องค่าสินไหมทดแทนตามสัญญาประกัน
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ภัยข้างต้น</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">2. รับค่าสินไหมทดแทนในสัญญากรมธรรม์ประกันภัยตามหมายเลขอ้างอิงดังกล่าวข้างต้น โดยให้บริษัทผู้รับประกันภัย</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">&nbsp;&nbsp;&nbsp; จ่ายในนามผู้รับมอบอำนาจได้</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">3. ผู้รับมอบอำนาจมีหน้าที่นำเงินค่าสินไหมทดแทน ส่งมอบให้แก่ผู้มอบอำนาจให้เสร็จสิ้น</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">4. ทั้งนี้ ให้ผู้รับมอบอำนาจ มีอำนาจถ้อยคำ ชี้แจง รับส่งเอกสาร ลงลายมือชื่อ ตลอดจนกระทำการใดๆ อันต่อเนื่อง</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%">&nbsp;</td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <div style="text-indent: 5%">&nbsp;&nbsp;&nbsp; เพื่อให้บรรลุผลแห่งการมอบอำนาจนี้ทุกประการ</div>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">
                        <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;การใดที่ผู้รับมอบอำนาจได้กระทำภายในขอบแห่งการมอบอำนาจนี้ ข้าพเจ้าในฐานะผู้มอบอำนาจยินยอมรับผิดชอบทุกประการ โดยเสมือนหนึ่ง ข้าพเจ้าได้กระทำไปด้วยตนเองทุกประการ เพื่อเป็นหลักฐานจึงได้ลงลายมือชื่อไว้ต่อหน้าพยาน</p>
                    </td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6">&nbsp;</td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td style="width: 12.5%; text-align: left" colspan="6"></td>
                    <td style="width: 12.5%">&nbsp;</td>
                </tr>
            </table>
            <!-- footer -->
            <table class="auto-style1">
                <tr class="trSignature">
                    <td class="auto-style4">ลงชื่อ ..........................................</td>
                    <td class="auto-style3">ลงชื่อ ..........................................</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">(.............................................)</td>
                    <td class="auto-style3">(.............................................)</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">(ผู้มอบอำนาจ)</td>
                    <td class="auto-style3">(ผู้รับมอบอำนาจ)</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr class="trSignature">
                    <td class="auto-style4">ลงชื่อ ..........................................</td>
                    <td class="auto-style3">ลงชื่อ ..........................................</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">(.............................................)</td>
                    <td class="auto-style3">(.............................................)</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">(พยาน)</td>
                    <td class="auto-style3">(พยาน)</td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4" colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>หมายเหตุ:</b> กรุณาแนบสำเนาบัตรประจำตัวประชาชนของผู้มอบอำนาจ <a style="text-decoration:underline;">ที่ลงลายมือชื่อรับรองแล้ว</a> มาพร้อมหนังสือมอบอำนาจฉบับนี้</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
                <tr class="trSignature">
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style3"></td>
                    <td style="width: 33.3%"></td>
                </tr>
            </table>
        </div>
    </form>
    <footer>
        <table>
            <tr>
                <td style="width: 10%">QF-CLM-COP-003</td>
                <td style="width: 12.5%;"></td>
                <td style="width: 12.5%; text-align: right;">Effective Date 1 Sep 2019</td>
            </tr>
        </table>
    </footer>
</body>
</html>