using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using SmileSDuration.Models;
using SmileSDuration.ComunicateService;
using SmileSDuration.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;

namespace SmileSDuration.Controllers
{
    [Helper.Authorization(Roles = "Developer")]
    public class UploadController : Controller
    {
        private readonly DurationV1Entities _context;

        #region Declare

        public UploadController()
        {
            _context = new DurationV1Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        #endregion Declare

        #region Action

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(FormCollection form, HttpPostedFileBase file)
        {
            var lst = new List<DetailImport>();

            if (file != null && file.ContentLength > 0)

                if (Path.GetExtension(file.FileName) != ".xlsx")
                {
                    //"กรุณาเลือก Excel file";
                    return RedirectToAction("FileFormatError", "Error", new { header = "กรุณาเลือก Excel file!", msg = "ไฟล์นำเข้าจะต้องเป็นไฟล์ Excel เท่านั้น." });
                }
                else
                {
                    //Import Excel To List
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var cs = package.Workbook.Worksheets;
                        var ws = cs[1];
                        var nr = ws.Dimension.End.Row;

                        try
                        {
                            for (int ri = 2; ri <= nr; ri++)
                            {
                                var ComunicateTypeId = Convert.ToInt32(ws.Cells[ri, 8].Value.ToString());
                                var item = new DetailImport()
                                {
                                    ComunicateCode = ws.Cells[ri, 1].Value.ToString(),
                                    ComTypeID = ComunicateTypeId,
                                    AppID = ws.Cells[ri, 3].Value.ToString(),
                                    CusName = ws.Cells[ri, 4].Value.ToString(),
                                    PayerName = ws.Cells[ri, 5].Value.ToString(),
                                    Amount = Convert.ToDecimal(ws.Cells[ri, 6].Value.ToString()),
                                    AmountTotal = Convert.ToDecimal(ws.Cells[ri, 7].Value.ToString()),
                                    MobileNo = ws.Cells[ri, 2].Value.ToString(),
                                    LetterDate = ws.Cells[ri, 9].Value.ToString(),//วันที่แจ้งเอกสาร
                                    PeriodOwe = ws.Cells[ri, 10].Value.ToString(),//จำนวนงวดค้างชำระ (2)
                                    DateRange = ws.Cells[ri, 11].Value.ToString(),//ช่วงวันที่ขาดชำระเบี้ย (2)
                                    DueDate = ws.Cells[ri, 12].Value.ToString(),//กำหนดชำระวันที่ (2)
                                    EndCover = ws.Cells[ri, 13].Value.ToString(),//วันสิ้นสุดความคุ้มครอง(2)
                                    DateOwe = ws.Cells[ri, 14].Value.ToString(), //ขาดชำระเบี้ยตั้งแต่วันที่ (3)
                                    RefNo = ws.Cells[ri, 15].Value.ToString()
                                };

                                switch (ComunicateTypeId)
                                {
                                    case 2: //แจ้งเตือนให้ชำระหนี้ PH /*update 7-2-2019 แจ้งว่าไม่ใช้แล้ว
                                        {
                                            item.Message = "ขณะนี้กรมธรรม์ประกันสุขภาพของ " + item.CusName +
                                                           " เลขที่อ้างอิง " + item.AppID + " ขาดการชำระเบี้ย " +
                                                           item.PeriodOwe + " เดือน (" + item.DateRange + ") จำนวนเงิน " +
                                                           item.AmountTotal +
                                                           " บาท ชำระผ่านธนาคารกรุงไทย เลขที่บัญชี 152-6-00748-7 หรือ Comp code 5233 กำหนดชำระภายใน " +
                                                           item.DueDate +
                                                           "หากมีการชำระเงินผ่านบัญชีธนาคารข้างต้นแล้ว โปรดแจ้งโอนที่ Call Center โทร. 087-7907999/02-5333999 (24ชม.)" +
                                                           "ขออภัยหากท่านได้ชำระเบี้ยประกันภัยเรียบร้อยแล้ว คลิกดูรายละเอียด ";
                                        };
                                        break;

                                    case 3://แจ้งสิ้นสุดกรมธรรม์ PH
                                        {
                                            //item.Message = "ขณะนี้กรมธรรม์ประกันสุขภาพของ " + item.CusName +
                                            //               " เลขที่อ้างอิง " + item.AppID +
                                            //               " ขาดการชำระเบี้ยตั้งแต่วันที่ " + item.DateOwe +
                                            //               " ส่งผลให้กรมธรรม์สิ้นสุดความคุ้มครอง สอบถามข้อมูล โทร. 087-7907999/02-5333999 (24 ชม.) ";

                                            //update 07-02-2019 ปรับแก้ไขเพิ่มเติมข้อความ
                                            //update 06-06-2019 ปรับแก้ไขเพิ่มเติมข้อความ
                                            //update 01-07-2019 ปรับแก้ไขเพิ่มเติมข้อความ
                                            //update 06-05-2020 ปรับแก้ไขเพิ่มเติมข้อความ
                                            item.Message = "เรียน สมาชิกกรมธรรม์ประกันสุขภาพ หมายเลขอ้างอิง " + item.AppID + " ขาดการชำระเบี้ย ส่งผลให้กรมธรรม์สิ้นสุดความคุ้มครอง ตั้งแต่วันที่ " + item.EndCover +
                                                           " หากมีความประสงค์ต่ออายุกรมธรรม์ หรือ หากท่านได้ชำระเบี้ยประกันแล้ว โปรดติดต่อ Call Center 1434 หรือ Line ID : @SiamSmile (24 ชม.) ";
                                        };
                                        break;

                                    case 4://แจ้งเตือนให้ชำระหนี้ Motor
                                        {
                                            item.Message = "ขณะนี้กรมธรรม์ประกันรถยนต์ของท่าน ทะเบียน " + item.RefNo +
                                                           " ขาดการชำระ " + item.PeriodOwe + " เดือน" +
                                                           " จำนวนเงิน " + item.AmountTotal + " บาท" +
                                                           " กำหนดชำระ " + item.DueDate + "  โอนเข้าบัญชี ธ.กรุงไทย เลขที่ 475-6-002978 ชื่อ บจ.สยามสไมล์ โบรกเกอร์" +
                                                           " สอบถามข้อมูล โทร.085-6958520 หรือ 087-7907999 (24ชม.)" +
                                                           " ขออภัยหากท่านได้ชำระยอดดังกล่าวแล้ว";
                                        };
                                        break;

                                    case 5://แจ้งสิ้นสุดกรมธรรม์ Motor
                                        {
                                            //TODO: Waiting message
                                        };
                                        break;
                                }

                                lst.Add(item);
                            }
                        }
                        catch (Exception e)
                        {
                            //"รูปแบบไฟล์ไม่ถูกต้อง";
                            return RedirectToAction("FileFormatError", "Error", new { header = "รูปแบบไฟล์ไม่ถูกต้อง!", msg = e.Message });
                        }
                    }
                }

            //Add New Header
            var header = new TempImportHeader()
            {
                IsActive = true,
                CreatedByID = 1,
                CreatedDate = DateTime.Now
            };

            _context.TempImportHeader.Add(header);
            _context.SaveChanges();

            //Add Detail
            foreach (var item in lst)
            {
                var detail = new TempImportDetail()
                {
                    TempImportHeaderID = header.TempImportHeaderID,
                    ComunicateCode = item.ComunicateCode,
                    ComunicateTypeID = item.ComTypeID,
                    AppID = item.AppID,
                    CustomerName = item.CusName,
                    PayerName = item.PayerName,
                    Amount = item.Amount,
                    AmountTotal = item.AmountTotal,
                    MobilePhoneNo = item.MobileNo,
                    IsActive = true,
                    CreatedByID = 1,
                    CreatedDate = DateTime.Now,
                    Message = item.Message,
                    LetterDate = item.LetterDate,
                    PeriodOwe = item.PeriodOwe,
                    DateRange = item.DateRange,
                    DueDate = item.DueDate,
                    EndCoverDate = item.EndCover,
                    DateOwe = item.DateOwe,
                    RefNo = item.RefNo
                };

                _context.TempImportDetail.Add(detail);
                _context.SaveChanges();
            }

            return RedirectToAction("DetailDataTable", new
            {
                id = header.TempImportHeaderID
            });
        }

        public ActionResult DetailDataTable(int id)
        {
            ViewBag.tempImportHeaderID = id;

            return View();
        }

        [HttpPost]
        public void PreviewSMS(FormCollection form)
        {
            var tempImportHeaderID = Convert.ToInt32(form["tempImportHeaderID_Preview"]);

            var list = _context.usp_Preview_Select(tempImportHeaderID, 0, 10000000, null, null).ToList();

            //Export To Excel
            ExcelManager.ExportToExcel(this.HttpContext, list, "PreviewSMS", "PreviewSMS");
        }

        [HttpPost]
        public ActionResult SubmitData_OldVersion(FormCollection form)
        {
            var tempImportHeaderID = Convert.ToInt32(form["tempImportHeaderID"]);

            //INSERT TEMP IMPORT HEADER
            var list = _context.usp_TempImportHeader_Insert(tempImportHeaderID);

            var listDuration = _context.Comunicate.Where(x => x.TempImportHeaderID == tempImportHeaderID).ToList();

            var smsService = new SmsServiceClient();

            foreach (var item in listDuration)
            {
                var msg = "";
                var type = 0;
                var sms = new SMS();

                switch (item.ComunicateTypeID)
                {
                    case 2: //`PH` แจ้งเตือนชำระเบี้ย
                        {
                            var urlConfig = Properties.Settings.Default["URLLetter1"].ToString();
                            var urlFull = urlConfig + "?id=" + item.ComunicateID;
                            var url = urlFull;
                            msg = item.Message + url;
                            type = 4; //ประเภทของข้อขวาม PH เตือนให้ชำระเบี้ย
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, url);
                            break;
                        }
                    case 3://`PH` แจ้งสิ้นสุดกรมธรรม์
                        {
                            var urlConfig = Properties.Settings.Default["URLLetter2"].ToString();
                            var urlFull = urlConfig + "?id=" + item.ComunicateID;
                            var url = urlFull;
                            msg = item.Message + url;
                            type = 5; //ประเภทของข้อขวาม PH เเจ้งสิ้นสุดกรมธรรม์
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, url);
                            break;
                        }
                    case 4://`Motor เตือนชำระเบี้ย
                        {
                            msg = item.Message;
                            type = 6; //ประเภทของข้อขวาม Motor เตือนให้ชำระเบี้ย
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, "-");
                            break;
                        }
                    case 5://`Motor แจ้งสิ้นสุดกรมธรรม์
                        {
                            msg = item.Message;
                            type = 7; //ประเภทของข้อขวาม Motor แจ้งสิ้นสุดกรมธรรม์
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, "-");
                            break;
                        }
                }

                sms.Message = msg;
                sms.SenderID = 3; // 3 คือ SiamSmile
                sms.SMSTypeID = type;
                sms.SendDate = DateTime.Now;
                sms.CreatedDate = DateTime.Now;
                sms.CreatedByID = 1;
                sms.SectionID = 1; //n/a

                //send sms
                var result = smsService.SendSmsV2(sms, item.MobilePhoneNo);

                //update duration
                var update = _context.Comunicate.FirstOrDefault(x => x.ComunicateID == item.ComunicateID);
                if (update != null)
                {
                    update.TransactionID = result.SMSTransectionID;
                }

                _context.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmitData(FormCollection form)
        {
            var tempImportHeaderID = Convert.ToInt32(form["tempImportHeaderID"]);

            //INSERT TEMP IMPORT HEADER
            var list = _context.usp_TempImportHeader_Insert(tempImportHeaderID);

            var listDuration = _context.Comunicate.Where(x => x.TempImportHeaderID == tempImportHeaderID).ToList();

            var type = 0;
            foreach (var item in listDuration)
            {
                var msg = "";
                var sms = new SMS();

                switch (item.ComunicateTypeID)
                {
                    case 2: //`PH` แจ้งเตือนชำระเบี้ย
                        {
                            var urlConfig = Properties.Settings.Default["URLLetter1"].ToString();
                            var urlFull = urlConfig + "?id=" + item.ComunicateID;
                            var url = urlFull;
                            msg = item.Message + url;
                            type = 4; //ประเภทของข้อขวาม PH เตือนให้ชำระเบี้ย
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, url);
                            break;
                        }
                    case 3://`PH` แจ้งสิ้นสุดกรมธรรม์
                        {
                            var urlConfig = Properties.Settings.Default["URLLetter2"].ToString();
                            var urlFull = urlConfig + "?id=" + item.ComunicateID;
                            var url = urlFull;
                            msg = item.Message + url;
                            type = 5; //ประเภทของข้อขวาม PH เเจ้งสิ้นสุดกรมธรรม์
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, url);
                            break;
                        }
                    case 4://`Motor เตือนชำระเบี้ย
                        {
                            msg = item.Message;
                            type = 6; //ประเภทของข้อขวาม Motor เตือนให้ชำระเบี้ย
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, "-");
                            break;
                        }
                    case 5://`Motor แจ้งสิ้นสุดกรมธรรม์
                        {
                            msg = item.Message;
                            type = 7; //ประเภทของข้อขวาม Motor แจ้งสิ้นสุดกรมธรรม์
                            //UPDATE URL IN DB_DURATION
                            _context.usp_ComunicateURL_Update(item.ComunicateID, "-");
                            break;
                        }
                }
            }

            //***ADD 4-6-2562***//
            var lstData = new List<SMSQueueHeaderDetail>();
            var lstComunicate = _context.usp_ComunicateByTempImportHeaderID_Select(tempImportHeaderID).ToList();

            if (lstComunicate.Count > 0)
            {
                foreach (var item in lstComunicate)
                {
                    lstData.Add(new SMSQueueHeaderDetail { PhoneNo = item.MobilePhoneNo, Message = item.Message + " " + item.URL });
                }
            }

            var smsDetail = new SMSQueueHeaderDetailViewModel
            {
                ProjectId = 3,
                SMSTypeId = type,
                Remark = "Send From SmileSDuration",
                Total = lstComunicate.Count,
                SendDate = "",
                Data = lstData.ToArray()
            };

            var apiUrl = new Uri(string.Format("http://operation.siamsmile.co.th:9215/api/sms/SendSMSList"));
            var authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwcm9qZWN0aWQiOjl9.XX7R_Ik8pydv2e04ZvrDew8tlszSDrjvTEYKdgO4t7A";
            var jsonData = JsonConvert.SerializeObject(smsDetail);
            var request = WebRequest.Create(apiUrl);
            var byteData = Encoding.UTF8.GetBytes(jsonData);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", authToken);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var result = JsonConvert.DeserializeObject<SMSResult>(responseString);

                if (listDuration.Count > 0)
                {
                    listDuration.ForEach(o =>
                    {
                        o.TransactionID = Convert.ToInt32(result.referenceHeaderID);
                    });
                }
                _context.SaveChanges();
                _context.Dispose();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (WebException)
            {
                return null;
            }
        }

        #endregion Action

        #region GetData

        public JsonResult GetDatatableDetail(int draw, int pageSize, int pageStart, string sortField, string orderType, string search, int tempImportHeaderID)
        {
            var list = _context.usp_TempImportDetail_Select(tempImportHeaderID, pageStart, pageSize, sortField, orderType, search).ToList();
            var dt = new Dictionary<string, object>
            {
                {"draw", draw },
                {"recordsTotal", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"recordsFiltered", list.Count() != 0 ? list.FirstOrDefault()?.TotalCount : list.Count()},
                {"data", list.ToList()}
            };

            return Json(dt, JsonRequestBehavior.AllowGet);
        }

        #endregion GetData
    }
}