using BarcodeLib;
using QRCoder;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;
using SmileSBillPayment.Models;
using SmileSBillPayment.Helper;

namespace SmileSBillPayment.Controllers
{
    public class GenBillController : Controller
    {
        #region Method

        /// <summary>
        /// Generate Bill
        /// </summary>
        /// <param name="GridHtml"></param>
        /// <returns></returns>
        [Route("doc/{ecode}")]
        public ActionResult Bill(string ecode)
        {
            //declare
            var redirectPathFile = "";
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    var base64EncodedBytes = Convert.FromBase64String(ecode);
                    var base64Decode = Encoding.UTF8.GetString(base64EncodedBytes);

                    //path folder
                    string path = AppDomain.CurrentDomain.BaseDirectory + "BillPaymentFile";
                    //fileName
                    string fileName = $"{base64Decode}.pdf";
                    //check path
                    if (!Directory.Exists(path))
                    {
                        //create
                        Directory.CreateDirectory(path);
                    }

                    //full path & file name
                    string fullPath = path + '\\' + fileName;

                    //check file
                    if (!System.IO.File.Exists(fullPath))
                    {
                        //get header and detail
                        var header = GetBillPaymentForGenerateFile(base64Decode);
                        var detail = GetBillPaymentDetailForGenerateFile(base64Decode);
                        //log generate file
                        var result_log = BillPaymentGenerateFileLog(header.BillPaymentId);
                        // read parameters from the webpage
                        //string htmlString = GridHtml;
                        string htmlString = SetDataHtml(base64Decode, header, detail);
                        string baseUrl = "";

                        string pdf_page_size = "A4";
                        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

                        string pdf_orientation = "Portrait";
                        PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                            pdf_orientation, true);

                        int webPageWidth = 1024;
                        try
                        {
                            webPageWidth = Convert.ToInt32("");
                        }
                        catch { }

                        int webPageHeight = 0;
                        try
                        {
                            webPageHeight = Convert.ToInt32("");
                        }
                        catch { }

                        // instantiate a html to pdf converter object
                        HtmlToPdf converter = new HtmlToPdf();

                        // set converter options
                        converter.Options.PdfPageSize = pageSize;
                        converter.Options.PdfPageOrientation = pdfOrientation;
                        converter.Options.WebPageWidth = webPageWidth;
                        converter.Options.WebPageHeight = webPageHeight;

                        // create a new pdf document converting an url
                        PdfDocument doc = converter.ConvertHtmlString(htmlString, baseUrl);

                        // save pdf document to full path
                        doc.Save(fullPath);
                        doc.Close();

                        //get file path
                        redirectPathFile = Server.MapPath("~/BillPaymentFile/" + fileName);
                    }
                    else
                    {
                        //get file path
                        redirectPathFile = Server.MapPath("~/BillPaymentFile/" + fileName);
                    }
                }

                return File(redirectPathFile, "application/pdf");
            }
            catch (Exception e)
            {
                var txtErr = "Message:" + e.Message + " / InnerException:" + e.InnerException;

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, txtErr);
            }
        }

        /// <summary>
        /// Generate QRCode
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string GenerateQRCode(string taxId, string suffix, string ref1, string ref2, string amount)
        {
            var std = new StandardPBDetail
            {
                TaxIDNo = taxId,
                Suffix = suffix,
                ReferenceNo1 = ref1,
                ReferenceNo2 = ref2,
                Amount = amount
            };

            string payload = StandardPB.GetPaymentBarcode(std);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);

            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);

            return qrCodeImageAsBase64;
        }

        /// <summary>
        /// Generate Barcode
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string GenerateBarCode(string taxId, string suffix, string ref1, string ref2, string amount)
        {
            Barcode brcode = new Barcode();
            var std = new StandardPBDetail
            {
                TaxIDNo = taxId,
                Suffix = suffix,
                ReferenceNo1 = ref1,
                ReferenceNo2 = ref2,
                Amount = amount
            };

            string BarcodeText = StandardPB.GetPaymentBarcode(std);
            brcode.IncludeLabel = true;
            brcode.Alignment = AlignmentPositions.CENTER;
            brcode.LabelPosition = LabelPositions.BOTTOMCENTER;
            brcode.Encode(TYPE.CODE128, BarcodeText, Color.Black, Color.White, 450, 50);
            MemoryStream ms = new MemoryStream();
            brcode.SaveImage(ms, SaveTypes.PNG);
            var barcodeByte = ms.ToArray();

            string barCodeImageAsBase64 = Convert.ToBase64String(barcodeByte);

            return barCodeImageAsBase64;
        }

        /// <summary>
        /// --header
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private usp_GetBillPaymentForGenerateFile_Result GetBillPaymentForGenerateFile(string code)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                var result = db.usp_GetBillPaymentForGenerateFile(code).FirstOrDefault();

                return result;
            }
        }

        /// <summary>
        /// --log
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool BillPaymentGenerateFileLog(int billPaymentId)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                var result = db.usp_BillPaymentGenerateFileLog_Insert(billPaymentId, GlobalFunction.GetIPAddress()).FirstOrDefault();

                return result.IsResult.Value;
            }
        }

        /// <summary>
        /// --detail
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<usp_GetBillPaymentDetailForGenerateFile_Result> GetBillPaymentDetailForGenerateFile(string code)
        {
            using (var db = new SSSCashReceiveEntities())
            {
                var result = db.usp_GetBillPaymentDetailForGenerateFile(code).ToList();

                return result;
            }
        }

        //set data
        private string SetDataHtml(string code, usp_GetBillPaymentForGenerateFile_Result header, List<usp_GetBillPaymentDetailForGenerateFile_Result> detail)
        {
            var qr = GenerateQRCode("0135551004383", "01", header.BillPaymentCode.ToString(), "0", header.Amount.ToString());
            var barCode = GenerateBarCode("0135551004383", "01", header.BillPaymentCode.ToString(), "0", header.Amount.ToString());
            var amountThaiBahtText = GlobalFunction.ThaiBahtText(header.Amount.ToString());

            int rowItemCount = detail.Count;
            var divheight = "700px";
            var pageBreak = "";
            if (rowItemCount > 13)
            {
                divheight = "1550px";
                pageBreak = "<div style='page-break-after: always'></div>";
            }

            var loopDetail = "";

            for (int i = 0; i < detail.Count; i++)
            {
                loopDetail += @"<tr>
<td style='font-size:17pt;' align='left'>" + detail[i].Description + @"</ td >
<td style='white-space:nowrap;font-size:17pt;' align='right'>" + detail[i].Amount.Value.ToString("N2") + @"</ td >
</tr>";
            }

            //Part1
            var part1 = @"
<style>
page{
background:white;
display:block;
margin:0 auto;
margin-bottom:0.5cm;
box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);
}
page[size='A4']{
width:100%;
}
</style>
<page size='A4'>
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:100%;background-image:url(https://customer.siamsmile.co.th/service/content/img/BPbackground.jpg);background-repeat: repeat;font-family:JasmineUPC;font-size:15pt;'>
<tr>
<td align='center' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td colspan='6'></td>
</tr>
<tr>
<td style='width:1%;background-color:#a8a9ad;'></td>
<td style='width:1%;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' valign='top' style='width:60%;'>
<img src='https://customer.siamsmile.co.th/service/content/img/BPlogo.png' />
</td>
<td align='right' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='height:50px;'></td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>บ.สยามสไมล์โบรกเกอร์ (ประเทศไทย) จำกัด</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>เลขที่ 89/6-10 ชั้น 4,5 ถนนเฉลิมพงษ์</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>แขวงสายไหม เขตสายไหม กรุงเทพ 10220</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>Call Center 1434 หรือติดต่อ 02-533-3999</td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:55%;' align='left' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' style='font-size:15pt;'>เรียน " + header.PayerName + @"</td>
</tr>
<tr>
<td align='left' style='font-size:15pt;'>Reference : " + header.BillPaymentCode + @"</td>
</tr>
<tr>
<td align='left' valign='top'>
<img src=""data:image/png;base64," + qr + @" width=""100"" height=""100"">
</td>
</tr>
</table>
</td>
<td align='center' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:95%;background-color:Gainsboro;'>
<tr>
<td style='height:12px;'></td>
</tr>
<tr>
<td align='center' valign='bottom' style='font-size:18pt;'>ใบแจ้งการชำระเงิน</td>
</tr>
<tr>
<td align='center' valign='middle' style='font-size:18pt;'>(Bill Payment Pay-In Slip)</td>
</tr>
<tr>
<td style='height:12px;'></td>
</tr>
</table>
<div style='height:20px;'></div>
<table cellpadding='0' cellspacing='0' style='width:95%;'>
<tr>
<td align='center'><img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto""></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:20px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:50%;border-right:solid;border-right-width:1px;font-size:15pt;' align='left' valign='middle'>ยอดที่ต้องชำระ : " + header.Amount.Value.ToString("N2") + @" บาท</td>
<td align='left' valign='middle' style='font-size:17pt;'>งวดความคุ้มครอง " + header.Period.Value.ToString("dd/MM/yyyy") + @"</td>
</tr>
</table>
<div style='height:10px;'></div>
</td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
</table>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;height:" + divheight + @";border-style:solid;border-width:1px;'>
<tr>
<td align='left' valign='top'>
<table cellpadding='6' cellspacing='0' style='width:100%;border-style:none;border-width:0px;'>
<tr>
<td style='background-color:Gainsboro;font-size:15pt;' align='center'>รายการ<br />(Description)</td>
<td style='width:1px;background-color:Gainsboro;white-space:nowrap;font-size:15pt;' align='center'>จำนวนเงิน (บาท)<br />Amount (Baht)</td>
</tr>" + loopDetail +
            @"</table>
</td>
</tr>
</table>
<div style='height:20px;'></div>
</td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
</table>";
            //End Part1

            ////<td align='left' valign='middle' style='font-size:15pt;'>กำหนดชำระเงินภายในวันที่ : " + header.PaymentDuedate.Value.ToString("dd MMMM yyyy").Replace("0", "") + @"</td>

            //Part2
            var part2 = @"<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td align='left' valign='bottom' style='background-image:url(https://customer.siamsmile.co.th/service/content/img/scissorsup.png);background-repeat:no-repeat;width:30px;height:10px;'></td>
<td style='width:1px;white-space:nowrap;font-size:8pt;'>(สำหรับลูกค้า)</td>
<td></td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
<tr>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td align='left' valign='top' style='border-top-style:dashed;border-top-width:1px;border-top-color:black;background-image:url(https://customer.siamsmile.co.th/service/content/img/scissorsdown.png);background-repeat:no-repeat;width:30px;height:10px;'></td>
<td style='width:1px;white-space:nowrap;font-size:8pt;border-top-style:dashed;border-top-width:1px;border-top-color:black;'>(สำหรับเจ้าหน้าที่)</td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#a8a9ad;'></td>
</tr>
</table>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td colspan='6'></td>
</tr>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<div style='height:10px;'></div>
<table cellpadding='4' cellspacing='0' style='width:100%;'>
<tr>
<td style='font-size:15pt;'>ช่องทางการชำระเงินที่เคาท์เตอร์สาขาธนาคาร (ค่าธรรมเนียมไม่เกิน 20 บาท)</td>
</tr>
<tr>
<td>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:20px;'></td>
<td style='width:20px;' valign='middle'>
<table cellpadding='0' cellspacing='0' style='width:20px;height:20px;border-style:solid;border-width:1px;'>
<tr>
<td></td>
</tr>
</table>
</td>
<td style='width:40%;' align='left' valign='middle'><span style='margin-left:10px;font-size:15pt;'>ธนาคารที่ให้บริการรับชำระบิล</span></td>
<td style='width:10px;'></td>
<td align='left' style='white-space:nowrap;'><img src='https://customer.siamsmile.co.th/service/content/img/K.png' width='25' /><img src='https://customer.siamsmile.co.th/service/content/img/NBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/mizuho.png' height='25' style='margin-left:5px;' /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td style='font-size:15pt;'>ช่องทางอิเล็กทรอนิกส์ ATM, Internet, Mobile Banking (ค่าธรรมเนียมเป็นไปตามที่ธนาคารกำหนด)</td>
</tr>
<tr>
<td align='left'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:20px;'></td>
<td style='width:20px;' valign='middle'>
<table cellpadding='0' cellspacing='0' style='width:20px;height:20px;border-style:solid;border-width:1px;'>
<tr>
<td></td>
</tr>
</table>
</td>
<td style='width:40%;' align='left'><span style='margin-left:10px;font-size:15pt;'>ธนาคารที่ให้บริการรับชำระบิล</span></td>
<td style='width:10px;'></td>
<td align='left' style='white-space:nowrap;'><img src='https://customer.siamsmile.co.th/service/content/img/SCB.jpg' width='25' /><img src='https://customer.siamsmile.co.th/service/content/img/BBL.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/KTB.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/KBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/K.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/NBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/GSB.jpg' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/TMB.jpg' width='25' style='margin-left:5px;' /></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='border-right:solid;border-right-width:1px;width:20%;font-size:15pt;' align='left'>รับเฉพาะเงินสดเท่านั้น</td>
<td style='border-right:solid;border-right-width:1px;width:30%;font-size:15pt;' align='left'>จำนวนเงิน(บาท) / Amount(Bath)</td>
<td align='right' style='border-right:solid;border-right-width:1px;width:20%;'>" + header.Amount.Value.ToString("N2") + @"</td>
<td style='width:30%;font-size:15pt;' align='left'>สำหรับเจ้าหน้าที่</td>
</tr>
<tr>
<td align='center' valign='middle' style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;height:70px;font-size:15pt;'>จำนวนเงินเป็นตัวอักษร<br />Amount in words</td>
<td align='center' valign='middle' colspan='2' style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;font-size:15pt;'>" + amountThaiBahtText + @"</td>
<td align='center' valign='middle' style='border-top:solid;border-top-width:1px;'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:10px;'>
</td>
<td style='white-space:nowrap;width:1px;font-size:15pt;'>
ผู้รับเงิน
</td>
<td style='border-bottom:dotted;border-bottom-width:1px'>
</td>
<td style='width:10px;'>
</td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:50%;height:40px;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>ชื่อผู้นำฝาก / Deposit by</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
<td style='width:50%;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>โทรศัพท์ / Telephone</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' style='width:50%;' valign='top'>
<table cellpadding='0' cellspacing='0'>
<tr>
<td><img src=""data:image/png;base64," + qr + @" width=""100"" height=""100""></td>
<td style='width:20px;'></td>
<td align='left' valign='middle' style='font-size:15pt;'>ชำระได้ทุกธนาคารที่ระบุ</td>
</tr>
</table>
</td>
<td align='center' valign='top'>
<div style='height:20px;'></div>
<img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto""></td>
</tr>
</table>
</td>
<td style='width:1%;'></td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#a8a9ad;'></td>
</tr>
</table>
</td>
</tr>
</table>";
            //End Part2

            var stringOutput = part1 + pageBreak + part2;
            return stringOutput;
        }

        private string SetDataHtmlBackupV1(string code, usp_GetBillPaymentForGenerateFile_Result header, List<usp_GetBillPaymentDetailForGenerateFile_Result> detail)
        {
            var qr = GenerateQRCode("0135551004383", "01", header.BillPaymentCode.ToString(), "0", header.Amount.ToString());
            var barCode = GenerateBarCode("0135551004383", "01", header.BillPaymentCode.ToString(), "0", header.Amount.ToString());
            var amountThaiBahtText = GlobalFunction.ThaiBahtText(header.Amount.ToString());

            int rowItemCount = detail.Count;
            var divheight = "700px";
            var pageBreak = "";
            if (rowItemCount > 13)
            {
                divheight = "1550px";
                pageBreak = "<div style='page-break-after: always'></div>";
            }

            var loopDetail = "";

            for (int i = 0; i < detail.Count; i++)
            {
                loopDetail += @"<tr>
<td style='font-size:17pt;' align='left'>" + detail[i].Description + @"</ td >
<td style='white-space:nowrap;font-size:17pt;' align='right'>" + detail[i].Amount.Value.ToString("N2") + @"</ td >
</tr>";
            }

            //Part1
            var part1 = @"
<style>
page{
background:white;
display:block;
margin:0 auto;
margin-bottom:0.5cm;
box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);
}
page[size='A4']{
width:100%;
}
</style>
<page size='A4'>
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:100%;background-image:url(https://customer.siamsmile.co.th/service/content/img/BPbackground.jpg);background-repeat: repeat;font-family:JasmineUPC;font-size:15pt;'>
<tr>
<td align='center' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td colspan='6'></td>
</tr>
<tr>
<td style='width:1%;background-color:#a8a9ad;'></td>
<td style='width:1%;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' valign='top' style='width:60%;'>
<img src='https://customer.siamsmile.co.th/service/content/img/BPlogo.png' />
</td>
<td align='right' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='height:50px;'></td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>บ.สยามสไมล์โบรกเกอร์ (ประเทศไทย) จำกัด</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>เลขที่ 89/6-10 ชั้น 4,5 ถนนเฉลิมพงษ์</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>แขวงสายไหม เขตสายไหม กรุงเทพ 10220</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>Call Center 1434 หรือติดต่อ 02-533-3999</td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:55%;' align='left' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' style='font-size:15pt;'>เรียน " + header.PayerName + @"</td>
</tr>
<tr>
<td align='left' style='font-size:15pt;'>Reference : " + header.BillPaymentCode + @"</td>
</tr>
<tr>
<td align='left' valign='top'>
<img src=""data:image/png;base64," + qr + @" width=""100"" height=""100"">
</td>
</tr>
</table>
</td>
<td align='center' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:95%;background-color:Gainsboro;'>
<tr>
<td style='height:12px;'></td>
</tr>
<tr>
<td align='center' valign='bottom' style='font-size:18pt;'>ใบแจ้งการชำระเงิน</td>
</tr>
<tr>
<td align='center' valign='middle' style='font-size:18pt;'>(Bill Payment Pay-In Slip)</td>
</tr>
<tr>
<td style='height:12px;'></td>
</tr>
</table>
<div style='height:20px;'></div>
<table cellpadding='0' cellspacing='0' style='width:95%;'>
<tr>
<td align='center'><img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto""></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:20px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:50%;border-right:solid;border-right-width:1px;font-size:15pt;' align='left' valign='middle'>ยอดที่ต้องชำระ : " + header.Amount.Value.ToString("N2") + @" บาท</td>
<td align='left' valign='middle' style='font-size:17pt;'>งวดความคุ้มครอง " + header.Period.Value.ToString("dd/MM/yyyy").Replace("0", "") + @"</td>
</tr>
</table>
<div style='height:10px;'></div>
</td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
</table>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;height:" + divheight + @";border-style:solid;border-width:1px;'>
<tr>
<td align='left' valign='top'>
<table cellpadding='6' cellspacing='0' style='width:100%;border-style:none;border-width:0px;'>
<tr>
<td style='background-color:Gainsboro;font-size:15pt;' align='center'>รายการ<br />(Description)</td>
<td style='width:1px;background-color:Gainsboro;white-space:nowrap;font-size:15pt;' align='center'>จำนวนเงิน (บาท)<br />Amount (Baht)</td>
</tr>" + loopDetail +
            @"</table>
</td>
</tr>
</table>
<div style='height:20px;'></div>
</td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
</table>";
            //End Part1

            ////<td align='left' valign='middle' style='font-size:15pt;'>กำหนดชำระเงินภายในวันที่ : " + header.PaymentDuedate.Value.ToString("dd MMMM yyyy").Replace("0", "") + @"</td>

            //Part2
            var part2 = @"<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td align='left' valign='bottom' style='background-image:url(https://customer.siamsmile.co.th/service/content/img/scissorsup.png);background-repeat:no-repeat;width:30px;height:10px;'></td>
<td style='width:1px;white-space:nowrap;font-size:8pt;'>(สำหรับลูกค้า)</td>
<td></td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
<tr>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td align='left' valign='top' style='border-top-style:dashed;border-top-width:1px;border-top-color:black;background-image:url(https://customer.siamsmile.co.th/service/content/img/scissorsdown.png);background-repeat:no-repeat;width:30px;height:10px;'></td>
<td style='width:1px;white-space:nowrap;font-size:8pt;border-top-style:dashed;border-top-width:1px;border-top-color:black;'>(สำหรับเจ้าหน้าที่)</td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#a8a9ad;'></td>
</tr>
</table>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td colspan='6'></td>
</tr>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td valign='top'>
<div style='height:10px;'></div>
<table cellpadding='4' cellspacing='0' style='width:100%;'>
<tr>
<td style='font-size:15pt;'>ช่องทางการชำระเงินที่เคาท์เตอร์สาขาธนาคาร (ค่าธรรมเนียมไม่เกิน 20 บาท)</td>
</tr>
<tr>
<td>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:20px;'></td>
<td style='width:20px;' valign='middle'>
<table cellpadding='0' cellspacing='0' style='width:20px;height:20px;border-style:solid;border-width:1px;'>
<tr>
<td></td>
</tr>
</table>
</td>
<td style='width:40%;' align='left' valign='middle'><span style='margin-left:10px;font-size:15pt;'>ธนาคารที่ให้บริการรับชำระบิล</span></td>
<td style='width:10px;'></td>
<td align='left' style='white-space:nowrap;'><img src='https://customer.siamsmile.co.th/service/content/img/K.png' width='25' /><img src='https://customer.siamsmile.co.th/service/content/img/NBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/mizuho.png' height='25' style='margin-left:5px;' /></td>
</tr>
</table>
</td>
</tr>
<tr>
<td style='font-size:15pt;'>ช่องทางอิเล็กทรอนิกส์ ATM, Internet, Mobile Banking (ค่าธรรมเนียมเป็นไปตามที่ธนาคารกำหนด)</td>
</tr>
<tr>
<td align='left'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:20px;'></td>
<td style='width:20px;' valign='middle'>
<table cellpadding='0' cellspacing='0' style='width:20px;height:20px;border-style:solid;border-width:1px;'>
<tr>
<td></td>
</tr>
</table>
</td>
<td style='width:40%;' align='left'><span style='margin-left:10px;font-size:15pt;'>ธนาคารที่ให้บริการรับชำระบิล</span></td>
<td style='width:10px;'></td>
<td align='left' style='white-space:nowrap;'><img src='https://customer.siamsmile.co.th/service/content/img/SCB.jpg' width='25' /><img src='https://customer.siamsmile.co.th/service/content/img/BBL.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/KTB.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/KBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/K.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/NBank.png' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/GSB.jpg' width='25' style='margin-left:5px;' /><img src='https://customer.siamsmile.co.th/service/content/img/TMB.jpg' width='25' style='margin-left:5px;' /></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='border-right:solid;border-right-width:1px;width:20%;font-size:15pt;' align='left'>รับเฉพาะเงินสดเท่านั้น</td>
<td style='border-right:solid;border-right-width:1px;width:30%;font-size:15pt;' align='left'>จำนวนเงิน(บาท) / Amount(Bath)</td>
<td align='right' style='border-right:solid;border-right-width:1px;width:20%;'>" + header.Amount.Value.ToString("N2") + @"</td>
<td style='width:30%;font-size:15pt;' align='left'>สำหรับเจ้าหน้าที่</td>
</tr>
<tr>
<td align='center' valign='middle' style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;height:70px;font-size:15pt;'>จำนวนเงินเป็นตัวอักษร<br />Amount in words</td>
<td align='center' valign='middle' colspan='2' style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;font-size:15pt;'>" + amountThaiBahtText + @"</td>
<td align='center' valign='middle' style='border-top:solid;border-top-width:1px;'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:10px;'>
</td>
<td style='white-space:nowrap;width:1px;font-size:15pt;'>
ผู้รับเงิน
</td>
<td style='border-bottom:dotted;border-bottom-width:1px'>
</td>
<td style='width:10px;'>
</td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:50%;height:40px;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>ชื่อผู้นำฝาก / Deposit by</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
<td style='width:50%;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>โทรศัพท์ / Telephone</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' style='width:50%;' valign='top'>
<table cellpadding='0' cellspacing='0'>
<tr>
<td><img src=""data:image/png;base64," + qr + @" width=""100"" height=""100""></td>
<td style='width:20px;'></td>
<td align='left' valign='middle' style='font-size:15pt;'>ชำระได้ทุกธนาคารที่ระบุ</td>
</tr>
</table>
</td>
<td align='center' valign='top'>
<div style='height:20px;'></div>
<img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto""></td>
</tr>
</table>
</td>
<td style='width:1%;'></td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#a8a9ad;'></td>
</tr>
</table>
</td>
</tr>
</table>";
            //End Part2

            var stringOutput = part1 + pageBreak + part2;
            return stringOutput;
        }

        #endregion Method
    }
}