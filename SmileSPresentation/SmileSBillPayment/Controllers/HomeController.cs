using BarcodeLib;
using QRCoder;
using SelectPdf;
using SmileSBillPayment.Helper;
using System;
using System.Drawing;
using System.Net;
using Authorization = SmileSBillPayment.Helper.Authorization;
using System.IO;
using System.Web.Mvc;
using System.Text;

namespace SmileSBillPayment.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IndexExample()
        {
            // lineNotify("wdrk2KSdxKDFLy2ueAHYJEyWbul1RLP2rcOcwDXcgPP", "เเจ้งเตือนผ่านการเรียก api code c# ", 1, 102, "");
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(string GridHtml)
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        // read parameters from the webpage
                        //string htmlString = GridHtml;
                        string htmlString = SetDataHtml("PHONGPHAN0" + i, "12346578955522wwwww0000THAOYOT" + i);
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

                        //path folder
                        string path = AppDomain.CurrentDomain.BaseDirectory + "FileTest";
                        //fileName
                        string fileName = $"PDF-TEST-{DateTime.Now.ToString("yyyyMMddHHmmss")}-{i}.pdf";
                        //check path
                        if (!Directory.Exists(path))
                        {
                            //create
                            Directory.CreateDirectory(path);
                        }
                        //full path & file name
                        string fullPath = path + '\\' + fileName;
                        // save pdf document to full path
                        doc.Save(fullPath);
                        var ssss = Path.GetFullPath(fullPath);
                        // save pdf document
                        //var pdf = doc.Save();

                        //assign to session
                        //Session["DownloadPDF_FileManager"] = pdf;

                        // close pdf document
                        doc.Close();
                    }
                }

                object objResult = new { success = true, responseText = "Export Done!" };

                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                object objResult = new { success = false, responseText = "Message:" + e.Message + " / InnerException:" + e.InnerException };
                return Json(objResult, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadPDF()
        {
            if (Session["DownloadPDF_FileManager"] != null)
            {
                byte[] data = Session["DownloadPDF_FileManager"] as byte[];

                string excelName = $"PDF-TEST-{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";

                return File(data, "application/pdf", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export2(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // read parameters from the webpage
                string htmlString = GridHtml;
                string baseUrl = "";

                string pdf_page_size = "A4";
                PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                    pdf_page_size, true);

                string pdf_orientation = "Portrait";
                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    pdf_orientation, true);

                int webPageWidth = 1024;
                try
                {
                    webPageWidth = Convert.ToInt32(1024);
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

                // save pdf document
                var pdf = doc.Save();

                Session["DownloadPDF_FileManager"] = pdf;

                // close pdf document
                doc.Close();

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadPDF2()
        {
            if (Session["DownloadPDF_FileManager"] != null)
            {
                byte[] data = Session["DownloadPDF_FileManager"] as byte[];

                string excelName = $"PDF-TEST-{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";

                return File(data, "application/pdf", excelName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        /// <summary>
        /// Generate QRCode
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string GenerateQRCode(string txt)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(txt, QRCodeGenerator.ECCLevel.H);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);

            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);

            return qrCodeImageAsBase64;
        }

        /// <summary>
        /// Generate Barcode
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string GenerateBarCode(string txt)
        {
            Barcode brcode = new Barcode();
            string BarcodeText = txt;
            brcode.IncludeLabel = true;
            brcode.Alignment = AlignmentPositions.LEFT;
            brcode.LabelPosition = LabelPositions.BOTTOMCENTER;
            brcode.Encode(TYPE.CODE128, BarcodeText, Color.Black, Color.White, 320, 80);
            MemoryStream ms = new MemoryStream();
            brcode.SaveImage(ms, SaveTypes.PNG);
            var barcodeByte = ms.ToArray();

            string barCodeImageAsBase64 = Convert.ToBase64String(barcodeByte);

            return barCodeImageAsBase64;
        }

        private string SetDataHtml(string v1, string v2)
        {
            var qr = GenerateQRCode(v2);
            var barCode = GenerateBarCode(v1);
            //var stringOutput = @"<div style=""font-family:'Angsana New';""class=""container"">
            //                    <br>
            //                    <table style= ""border:solid 1px;width:100%"">
            //                        <tbody>
            //                            <tr style=""border-bottom: solid 1px;"">
            //                                <td colspan=""10"" style=""font-size:30px;font-weight:bold;text-indent: 15px;""> ใบรับฝากเงิน(Pay Slip) </td>
            //                            </tr>
            //                            <tr>
            //                                <td rowspan=""4"" style=""width:10%;text-align:center""><img style=""width:90px;"" class=""img-logo"" src=""http://customer.siamsmile.co.th/service/content/img/logo.png""></td>
            //                                <td colspan=""4""> บริษัท private สยามสไมล์ โบรกเกอร์(ประเทศไทย) จำกัด</td>
            //                                <td style=""width:10%""></td>
            //                                <td colspan=""4"">วันที่รับชำระ/Date.......................................................</td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4"">เลขที่ 89/6-10 ถนนเฉลิมพงษ์</td>
            //                                <td style=""width:10%""></td>
            //                                <td rowspan=""3"" colspan=""4"">
            //                                    <table style=""border:solid 1px;width:90%"">
            //                                        <tbody>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;"">ชื่อผู้ชำระเบี้ย:" + v1 + @"</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;""> เลขที่บัตรประชาชน / Ref.1: 1570500111XXXX</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;"">เบอร์โทรศัพท์ / Ref.2:" + v2 + @"</td>
            //                                            </tr>
            //                                         </tbody>
            //                                    </table>
            //                                </td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4""> แขวงสายไหม เขตสายไหม กรุงเทพฯ 10220</td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4"" style=""font-size: 20px;font-weight: bold;""> Call Center 02-5333-999</td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td style=""text-indent: 15px;"" colspan=""5""> วิธีการชำระเงิน : หักผ่านธนาคาร</td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""10"">
            //                                    <table id=""sub-table"" style=""border-collapse: collapse; border: 1px solid;width:95%;margin-left: 15px;margin-bottom: 20px;"">
            //                                        <tbody>
            //                                            <tr>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> รวมทั้งสิ้น </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> จำนวนเงิน(บาท) / Amount(Baht) </td>
            //                                                <td colspan=""4"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                                <td rowspan=""2"" colspan=""6"" style=""width:15%;text-align:center; border: 1px solid;""> ผู้รับเงิน....................</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;"" > จำนวนเงินเป็นตัวอักษร / Amount in Words</td>
            //                                                <td style=""text-align:center;border: 1px solid;"" colspan=""6""><span style=""font-weight:bold;""> เจ็ดร้อยห้าสิบบาทถ้วน </span></td>
            //                                            </tr>
            //                                            <tr style=""background-color:#4ea6f2 !important;font-weight:bold"">
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> ลำดับที่ </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> เลขอ้างอิง </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> ชื่อ - สกุล </td>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> แผน </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> เบี้ย </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> งวด </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> ส่วนลด </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;border-right:solid 1px""> รวม </td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> 1 </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> 00000X</td>
            //                                                <td style=""width:20%;text-align:left; border: 1px solid;""> PHONGPHAN THAOYOT</td>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> XXXX - 501 </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> XXX / X </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> XXX </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                            </tr>
            //                                       </tbody>
            //                                    </table>
            //                                </td>
            //                           </tr>
            //                      </tbody>
            //                </table>
            //                <br>
            //                <div>
            //                    <div style=""display:inline-block"">
            //                        <div title=""http://www.siamsmile.co.th/"">
            //                            <img src=""data:image/png;base64," + qr + @" width=""100"" height=""100"">
            //                         </div>
            //                    </div>
            //                    <div style=""display:inline-block"">
            //                            <img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto"">
            //                    </div>
            //                </div>
            //          </div>";

            //BillPayment Form Layout

            int rowItemCount = 1;
            var divheight = "<div style='height:550px;'>";
            var pageBreak = "";
            if (rowItemCount > 13)
            {
                divheight = "<div style='height:1000px;'>";
                pageBreak = "<div style='page-break-after: always'></div>";
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
<table cellpadding='0' cellspacing='0' style='width:100%;background-image:url(http://customer.siamsmile.co.th/service/content/img/BPbackground.jpg);background-repeat: repeat;font-family:JasmineUPC;font-size:15pt;'>
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
<img src='http://customer.siamsmile.co.th/service/content/img/BPlogo.png' />
</td>
<td align='right' valign='top'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='height:50px;'></td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>บ.สยามสไมล์โบรกเกอร์(ประเทศไทย) จำกัด</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>เลขที่ 89/6-10 ชั้น 4,5 ถนนเฉลิมพงษ์</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>แขวงสายไหม เขตสายไหม กรุงเทพ 10220</td>
</tr>
<tr>
<td align='right' style='font-size:15pt;'>Call Center 02-533-3999</td>
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
<td align='left' style='font-size:15pt;'>เรียน <!--TODO-->คุณจำปาทอง มณีทองคำ</td>
</tr>
<tr>
<td align='left' style='font-size:15pt;'>เลขที่บัตรประชาชน/Ref: <!--TODO-->01234567890123</td>
</tr>
<tr>
<td align='left' style='font-size:15pt;'>เบอร์โทรศัพท์: <!--TODO-->0123456789</td>
</tr>
</table>
</td>
<td align='right' valign='top'>
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
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:95%;'>
<tr>
<td align='center'><img src='http://customer.siamsmile.co.th/service/content/img/BPBarcodeDemo.jpg' /></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:50%;border-right:solid;border-right-width:1px;font-size:15pt;' align='left' valign='middle'>ยอดที่ต้องชำระ : <!--TODO-->350.00 บาท</td>
<td align='left' valign='middle' style='font-size:15pt;'>โปรดชำระภายในวันที่ : <!--TODO-->20 มกราคม 2563</td>
</tr>
</table>
<div style='height:10px;'></div>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td align='left' valign='middle' style='font-size:15pt;'>รายละเอียด กรมธรรม์</td>
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
<td colspan='6'></td>
</tr>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td valign='top'>
" + divheight + @"
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;border-bottom:none;'><tr>
<td style='width:1px;background-color:Gainsboro;white-space:nowrap;font-size:15pt;' align='center'>ลำดับที่</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>เลขที่อ้างอิง</td>
<td style='background-color:Gainsboro;font-size:15pt;' align='center'>ชื่อ - สกุล</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>แผน</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>เบี้ย</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>งวด</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>ส่วนลด</td>
<td style='width:1px;background-color:Gainsboro;font-size:15pt;' align='center'>รวม</td>
</tr>
<!--TODO Start Loop-->
<tr>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='center'><!--TODO-->1.</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='left'><!--TODO-->59201231</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='left'><!--TODO-->นายกติกรณ์ บุญญภรณ์</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='center'><!--TODO-->502 - Silver</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='right'><!--TODO-->350.00</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='center'><!--TODO-->01/01/2563</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='right'><!--TODO-->0.00</td>
<td style='border-bottom:solid;border-bottom-width:1px;white-space:nowrap;font-size:15pt;' align='right'><!--TODO-->350.00</td>
</tr>
<!--End Loop-->
</table>
</div>
<table cellpadding='7' cellspacing='0' style='width:100%;border-style:solid;border-width:1px;'>
<tr>
<td style='width:33%;height:40px;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>จำนวนเงิน</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>บาท</td>
</tr>
</table>
</td>
<td style='width:33%;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>ลงชื่อ</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
<td style='width:33%;' align='left' valign='bottom'>
<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1px;white-space:nowrap;font-size:15pt;'>วันที่</td>
<td style='border-bottom:dotted;border-bottom-width:1px'></td>
</tr>
</table>
</td>
</tr>
</table>
<div style='height:35px;'></div>
</td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
</table>";
            //End Part1

            //Part2
            var part2 = @"<table cellpadding='0' cellspacing='0' style='width:100%;'>
<tr>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;'></td>
<td align='left' valign='bottom' style='background-image:url(http://customer.siamsmile.co.th/service/content/img/scissorsup.png);background-repeat:no-repeat;height:10px;'></td>
<td style='width:1%;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
<td style='width:1%;background-color:#25aae2;'></td>
</tr>
<tr>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;background-color:#25aae2;'></td>
<td style='border-top-style:dashed;border-top-width:1px;border-top-color:black;width:1%;'></td>
<td align='left' valign='top' style='border-top-style:dashed;border-top-width:1px;border-top-color:black;background-image:url(http://customer.siamsmile.co.th/service/content/img/scissorsdown.png);background-repeat:no-repeat;height:10px;'></td>
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
<td style='font-size:15pt;'>ช่องทางการชำระเงินที่เคาท์เตอร์สาขาธนาคาร(ไม่เกิน 20 บาท)</td>
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
<td align='left' valign='middle'><span style='margin-left:10px;font-size:15pt;'>ธนาคารให้บริการรับชำระบิล</span></td>
</tr>
</table>
</td>
</tr>
<tr>
<td style='font-size:15pt;'>ช่องทางอิเล็กทรอนิกส์ ATM, Internet, Mobile Banking(ไม่เกิน 5 บาท)</td>
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
<td style='width:40%;' align='left'><span style='margin-left:10px;font-size:15pt;'>ธนาคารให้บริการรับชำระบิล</span></td>
<td style='width:10px;'></td>
<td align='left' style='white-space:nowrap;'><img src='http://customer.siamsmile.co.th/service/content/img/SCB.jpg' width='25' /><img src='http://customer.siamsmile.co.th/service/content/img/BBL.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/KTB.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/BAY.jpg' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/KBank.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/K.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/CIMB.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/TMB.jpg' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/NBank.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/UOB.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/ICBC.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/M.png' width='25' style='margin-left:5px;' /><img src='http://customer.siamsmile.co.th/service/content/img/GSB.jpg' width='25' style='margin-left:5px;' /></td>
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
<td style='border-right:solid;border-right-width:1px;width:20%;'></td>
<td style='width:30%;font-size:15pt;' align='left'>สำหรับเจ้าหน้าที่</td>
</tr>
<tr>
<td style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;height:70px;'></td>
<td style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;'></td>
<td style='border-top:solid;border-top-width:1px;border-right:solid;border-right-width:1px;'></td>
<td style='border-top:solid;border-top-width:1px;'></td>
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
<td><img src='http://customer.siamsmile.co.th/service/content/img/BPQRCodeDemo.png' /></td>
<td style='width:20px;'></td>
<td align='left' valign='middle' style='font-size:15pt;'>ชำระได้ทุกธนาคาร</td>
</tr>
</table>
</td>
<td align='center' valign='top'><img src='http://customer.siamsmile.co.th/service/content/img/BPBarcodeDemo.jpg' /></td>
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
            //var stringOutput = @"<div style=""font-family:'Angsana New';""class=""container"">
            //                    <br>
            //                    <table style = 'border:solid 1px;width:100%;color:red'>
            //                        <tbody>
            //                            <tr style=""border-bottom: solid 1px;"">
            //                                <td colspan=""10"" style=""font-size:150px;font-weight:bold;text-indent: 15px;""> ใบรับฝากเงิน(Pay Slip) </td>
            //                            </tr>
            //                            <tr>
            //                                <td rowspan=""4"" style=""width:10%;text-align:center""><img style =""width: 90px;"" class=""img-logo"" src=""http://customer.siamsmile.co.th/service/content/img/logo.png""></td>
            //                                <td colspan=""4""> บริษัท private สยามสไมล์ โบรกเกอร์(ประเทศไทย) จำกัด</td>
            //                                <td style=""width:10%""></td>
            //                                <td colspan=""4"">วันที่รับชำระ/Date.......................................................</td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4"">เลขที่ 89/6-10 ถนนเฉลิมพงษ์</td>
            //                                <td style=""width:10%""></td>
            //                                <td rowspan=""3"" colspan=""4"">
            //                                    <table style=""border:solid 1px;width:90%"">
            //                                        <tbody>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;"">ชื่อผู้ชำระเบี้ย:" + v1 + @"</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;""> เลขที่บัตรประชาชน / Ref.1: 1570500111XXXX</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""padding: 2px 0px 0px 4px;"">เบอร์โทรศัพท์ / Ref.2:" + v2 + @"</td>
            //                                            </tr>
            //                                         </tbody>
            //                                    </table>
            //                                </td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4""> แขวงสายไหม เขตสายไหม กรุงเทพฯ 10220</td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""4"" style=""font-size: 20px;font-weight: bold;""> Call Center 02-5333-999</td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td style=""text-indent: 15px;"" colspan=""5""> วิธีการชำระเงิน : หักผ่านธนาคาร</td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                                <td style=""width:10%""></td>
            //                            </tr>
            //                            <tr>
            //                                <td colspan=""10"">
            //                                    <table id=""sub-table"" style=""border-collapse: collapse; border: 1px solid;width:95%;margin-left: 15px;margin-bottom: 20px;"">
            //                                        <tbody>
            //                                            <tr>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> รวมทั้งสิ้น </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> จำนวนเงิน(บาท) / Amount(Baht) </td>
            //                                                <td colspan=""4"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                                <td rowspan=""2"" colspan=""6"" style=""width:15%;text-align:center; border: 1px solid;""> ผู้รับเงิน....................</td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;"" > จำนวนเงินเป็นตัวอักษร / Amount in Words</td>
            //                                                <td style=""text-align:center;border: 1px solid;"" colspan=""6""><span style=""font-weight:bold;""> เจ็ดร้อยห้าสิบบาทถ้วน </span></td>
            //                                            </tr>
            //                                            <tr style=""background-color:#4ea6f2 !important;font-weight:bold"">
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> ลำดับที่ </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> เลขอ้างอิง </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> ชื่อ - สกุล </td>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> แผน </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> เบี้ย </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> งวด </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> ส่วนลด </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;border-right:solid 1px""> รวม </td>
            //                                            </tr>
            //                                            <tr>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> 1 </td>
            //                                                <td style=""width:20%;text-align:center; border: 1px solid;""> 00000X</td>
            //                                                <td style=""width:20%;text-align:left; border: 1px solid;""> PHONGPHAN THAOYOT</td>
            //                                                <td style=""width:10%;text-align:center; border: 1px solid;""> XXXX - 501 </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> XXX / X </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> XXX </td>
            //                                                <td colspan=""2"" style=""text-align:center; border: 1px solid;""> 750 </td>
            //                                            </tr>
            //                                       </tbody>
            //                                    </table>
            //                                </td>
            //                           </tr>
            //                      </tbody>
            //                </table>
            //                <br>
            //                <div>
            //                    <div style=""display:inline-block"">
            //                        <div title=""http://www.siamsmile.co.th/"">
            //                            <img src=""data:image/png;base64," + qr + @" width=""100"" height=""100"">
            //                         </div>
            //                    </div>
            //                    <div style=""display:inline-block"">
            //                            <img src=""data:image/png;base64," + barCode + @" width=""auto"" height=""auto"">
            //                    </div>
            //                </div>
            //          </div>";

            return stringOutput;
        }

        #region LineNotify

        private void notifyPicture(string token, string url)
        {
            lineNotify(token, " ", 0, 0, url);
        }

        private void notifySticker(string token, int stickerID, int stickerPackageID)
        {
            lineNotify(token, " ", stickerPackageID, stickerID, "");
        }

        private void lineNotify(string token, string msg)
        {
            lineNotify(token, msg, 0, 0, "");
        }

        private void lineNotify(string token, string msg, int stickerPackageID, int stickerID, string pictureUrl)
        {
            try
            {
                var postData = string.Format("message={0}", msg);
                if (stickerPackageID > 0 && stickerID > 0)
                {
                    var stickerPackageId = string.Format("stickerPackageId={0}", stickerPackageID);
                    var stickerId = string.Format("stickerId={0}", stickerID);
                    postData += "&" + stickerPackageId.ToString() + "&" + stickerId.ToString();
                }
                if (pictureUrl != "")
                {
                    var imageThumbnail = string.Format("imageThumbnail={0}", pictureUrl);
                    var imageFullsize = string.Format("imageFullsize={0}", pictureUrl);
                    postData += "&" + imageThumbnail.ToString() + "&" + imageFullsize.ToString();
                }
                var data = Encoding.UTF8.GetBytes(postData);

                //web Request
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);
                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion LineNotify
    }
}