using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace SmileSCriticalIllness.Helper
{
    public class cTNIWebService
    {
        public TNIWebServiceResult IssuePolicy(
            int application_id,
            string app_no,
            DateTime app_date,
            string cust_prefix,
            string cust_name,
            string cust_surname,
            string cust_IDCard,
            string cust_sex,
            DateTime cust_birthdate,
            string cust_occupation,
            string cust_email,
            string addr_poladdr,
            string addr_polbld,
            string addr_polmoo,
            string addr_polsoi,
            string addr_polroad,
            string addr_district,
            string addr_amphur,
            string addr_province,
            string addr_postcode,
            string addr_tel,
            string send_prefix,
            string send_name,
            string send_surname,
            string send_addr1,
            string send_addr2,
            string send_district,
            string send_amphur,
            string send_province,
            string send_postcode,
            string send_tel,
            double prem_total,
            DateTime paid_date,
            string payer_fullname,
            DateTime receipt_date,
            string payer_IDCard,
            string product_code,
            string package_code,
            DateTime effect_date,
            DateTime expire_date,
            float sumins,
            float net,
            float stamp,
            float vat,
            float total,
            string inscus_bene01_prefix,
            string inscus_bene01_name,
            string inscus_bene01_surname,
            string inscus_bene01_relation,
            string inscus_bene01_rate,
            string inscus_bene02_prefix,
            string inscus_bene02_name,
            string inscus_bene02_surname,
            string inscus_bene02_relation,
            string inscus_bene02_rate
            )
        {
            var tniJsonData = new TNIJsonData();
            var data = new DataNon();
            var item = new DataNonBenefit();
            var benefit = new Beneficial();

            //bind data
            data.APP_NO = app_no;
            data.APP_DATE = DateToString(app_date);
            data.APP_INFORM_NAME = Properties.Settings.Default.TNI_APP_INFORM_NAME;
            data.CUS_TYPE = Properties.Settings.Default.TNI_CUS_TYPE;
            data.CUS_FOREIGNER = Properties.Settings.Default.TNI_CUS_FOREIGNER;
            data.CUS_PREFIX = cust_prefix;
            data.CUS_NAME = cust_name;
            data.CUS_SURNAME = cust_surname;
            data.CUS_IDTYPE = Properties.Settings.Default.TNI_CUS_IDTYPE;
            data.CUS_IDCARD = cust_IDCard;
            data.CUS_SEX = cust_sex;
            data.CUS_BIRTHDATE = DateToString(cust_birthdate);
            data.CUS_OCCUPATION = cust_occupation;
            data.CUS_NATION = Properties.Settings.Default.TNI_CUS_NATION;
            data.CUS_EMAIL = cust_email;
            data.CUS_TAX_LOCATION = Properties.Settings.Default.TNI_CUS_TAX_LOCATION;
            data.CUS_TAX_BRN = Properties.Settings.Default.TNI_CUS_TAX_BRN;
            data.CUS_TAX_BRN_NM = Properties.Settings.Default.TNI_CUS_TAX_BRN_NM;
            data.ADDR_POLADDR = addr_poladdr;
            data.ADDR_POLBLD = addr_polbld;
            data.ADDR_POLMOO = "หมู่ที่ " + addr_polmoo;
            data.ADDR_POLSOI = "ซ." + addr_polsoi;
            data.ADDR_POLROAD = "ถ." + addr_polroad;
            data.ADDR_DISTRICT = (addr_province == "กรุงเทพมหานคร")? addr_district : "ต." + addr_district;
            data.ADDR_AMPHUR = addr_amphur;
            data.ADDR_PROVINCE = addr_province;
            data.ADDR_POSTCODE = addr_postcode;
            data.ADDR_TEL = this.PhoneNumberToShow(addr_tel);
            data.SEND_PREFIX = send_prefix;
            data.SEND_NAME = send_name;
            data.SEND_SURNAME = send_surname;
            data.SEND_ADDR1 = send_addr1;
            data.SEND_ADDR2 = send_addr2;
            data.SEND_DISTRICT = (send_province == "กรุงเทพมหานคร") ?  send_district : "ต." + send_district;
            data.SEND_AMPHUR = send_amphur;
            data.SEND_PROVINCE = send_province;
            data.SEND_POSTCODE = send_postcode;
            data.SEND_TEL = this.PhoneNumberToShow(send_tel);
            data.SEND_FAX = "";
            data.PREM_TOTAL = prem_total.ToString();
            data.PAID_DATE = DateToString(paid_date);
            data.PAID_TYPE = Properties.Settings.Default.TNI_PAID_TYPE;
            data.PAID_DOC_DESC = app_no; //PAID_TYPE = 0 ให้ส่งเลขที่ APPNO
            data.PAID_CRCARD = Properties.Settings.Default.TNI_PAID_CRCARD;
            data.INF_RCP_NAME = payer_fullname;
            data.INF_RCP_DATE = DateToString(receipt_date);
            data.INF_RCP_NAMETYPE = Properties.Settings.Default.TNI_INF_RCP_NAMETYPE;
            data.INF_RCP_CUSTYPE = Properties.Settings.Default.TNI_INF_RCP_CUSTYPE;
            data.INF_RCP_IDCARD = payer_IDCard;
            data.INF_RCP_TAXID = payer_IDCard;
            data.INF_RCP_TAX_LOCATION = Properties.Settings.Default.TNI_INF_RCP_TAX_LOCATION;
            data.INF_RCP_TAX_BRN = Properties.Settings.Default.TNI_INF_RCP_TAX_BRN;
            data.INF_RCP_TAX_BRN_NM = Properties.Settings.Default.TNI_INF_RCP_TAX_BRN_NM;
            data.INI_COMPANY_CODE = Properties.Settings.Default.TNI_INI_COMPANY_CODE;
            data.SERVICETYPE = Properties.Settings.Default.TNI_SERVICETYPE;
            data.PRODUCT_CODE = product_code;
            data.PACKAGE_CODE = package_code;
            data.EFFECT_DATE = DateToString(effect_date);
            data.EXPIRE_DATE = DateToString(expire_date);
            data.PRINT_EPOLICY = Properties.Settings.Default.TNI_PRINT_EPOLICY;
            data.SUMINS = sumins.ToString();
            data.NET = net.ToString();
            data.STAMP = stamp.ToString();
            data.VAT = vat.ToString();
            data.TOTAL = total.ToString();
            data.TA_DESTINATION = Properties.Settings.Default.TNI_TA_DESTINATION;

            //bind item
            item.REF_ID = Properties.Settings.Default.TNI_REF_ID;
            item.INSUS_SEQ = "1";
            item.INSCUS_PREFIX = cust_prefix;
            item.INSCUS_NAME = cust_name;
            item.INSCUS_SURNAME = cust_surname;
            item.INSCUS_IDTYPE = Properties.Settings.Default.TNI_CUS_IDTYPE;
            item.INSCUS_IDCARD = cust_IDCard;
            item.INSCUS_OCCUPATION = cust_occupation;
            item.INSCUS_BIRTHDATE = DateToString(cust_birthdate);
            item.INSCUS_SEX = cust_sex;
            item.INSCUS_BENE01_PREFIX = inscus_bene01_prefix;
            item.INSCUS_BENE01_NAME = inscus_bene01_name;
            item.INSCUS_BENE01_SURNAME = inscus_bene01_surname;

            //แก้ไขตามใบคำร้องที่ 0182
            if (!inscus_bene02_name.IsNullOrWhiteSpace())
            {
                item.INSCUS_BENE01_SURNAME += '/' + inscus_bene02_prefix + inscus_bene02_name + ' ' + inscus_bene02_surname;
            }

            item.INSCUS_BENE01_RELATION = inscus_bene01_relation;
            item.INSCUS_BENE01_RATE = inscus_bene01_rate;
            item.INSCUS_BENE02_PREFIX = inscus_bene02_prefix;
            item.INSCUS_BENE02_NAME = inscus_bene02_name;
            item.INSCUS_BENE02_SURNAME = inscus_bene02_surname;
            item.INSCUS_BENE02_RELATION = inscus_bene02_relation;
            item.INSCUS_BENE02_RATE = inscus_bene02_rate;
            item.INSCUS_ADDR_POLADDR = addr_poladdr;
            item.INSCUS_ADDR_POLBLD = addr_polbld;
            item.INSCUS_ADDR_POLMOO = "หมู่ที่" + addr_polmoo;
            item.INSCUS_ADDR_POLSOI = "ซ." + addr_polsoi;
            item.INSCUS_ADDR_POLROAD = "ถ." + addr_polroad;
            item.INSCUS_ADDR_DISTRICT = (addr_province == "กรุงเทพมหานคร")? addr_district : "ต." + addr_district;
            item.INSCUS_ADDR_AMPHUR = addr_amphur;
            item.INSCUS_ADDR_PROVINCE = addr_province;
            item.INSCUS_ADDR_POSTCODE = addr_postcode;
            item.INSCUS_ADDR_TEL = this.PhoneNumberToShow(addr_tel);

            List<DataNonBenefit> lstBeneficial = new List<DataNonBenefit>();

            lstBeneficial.Add(item);
            benefit.DataNonItem = lstBeneficial;
            data.NonItemList = benefit;
            tniJsonData.DataNon = data;

            //แปลง Data เป็น Json
            var dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(tniJsonData);

            TNIWebServiceResult result = new TNIWebServiceResult();

            //init application_Id
            result.ApplicationId = application_id;
            result.AppNo = app_no;
            result.JsonOut = dataJson;

            try
            {
                var wsResult = "";
                if(Properties.Settings.Default.TNI_RandomWebserviceResult)
                {
                    // Random result สำหรับ Test
                    wsResult = TNIWebServiceMockupResult();
                }
                else
                {
                    // Inject TNI Webservice
                    wsResult = new TNIService.ServiceNonMotorForOtherBrokersGWSoapClient().ServiceNonMotor_JSON(dataJson);
                }

                //Map result and return
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<TNIWebServiceResult>(wsResult);
                //Reset application_Id (in case wsResult return null)
                result.ApplicationId = application_id;
                result.AppNo = app_no;
                result.IsCallSuccess = true;
                result.FullResultReturn = wsResult;
                result.JsonOut = dataJson;
            }
            catch(Exception ex)
            {
                result.IsCallSuccess = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Date to string function
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private string DateToString(DateTime d)
        {
            return string.Format("{0}/{1}/{2}",d.Day.ToString("0#"),d.Month.ToString("0#"),d.Year.ToString());
        }

        /// <summary>
        /// Show /hide phone number
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        private string PhoneNumberToShow(string tel)
        {
            string result;

            result = (Properties.Settings.Default.TNI_IsHideCustPhoneNo) ? Properties.Settings.Default.TNI_CustPhoneNoWhenHide : tel;

            return result;
        }

        /// <summary>
        /// Mockup Random result โอกาสผ่าน = 80%
        /// </summary>
        /// <returns></returns>
        private string TNIWebServiceMockupResult()
        {
            string result = "";
            var randomSuccess = new Random().Next(100);
            var randomResult = 0;
            var PolicyNo = "";

            //โอกาสสำเร็จ 80%
            if(randomSuccess >= 50)
            {
                randomResult = 1;
                PolicyNo = new Random().Next(0,999999).ToString();
            }
            else
            {
                randomResult = new Random().Next(0,9);
            }

            switch(randomResult)
            {
                case 0:
                    result = @"{""MESSAGE_CODE"":""0"",""MESSAGE_DESC"":""ไม่สามารถสร้างเลข policyNo ได้"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 1:
                    result = @"{""MESSAGE_CODE"":""1"",""MESSAGE_DESC"":"""",""POLICYNO"":" + "\"" + PolicyNo + "\"" + @",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 2:
                    result = @"{""MESSAGE_CODE"":""2"",""MESSAGE_DESC"":""ไม่สามารถรับแจ้งประกันภัย รายการนี้ได้"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 3:
                    result = @"{""MESSAGE_CODE"":""3"",""MESSAGE_DESC"":""ไม่สามารถตรวจสอบข้อมูลผู้ถูกกำหนดได้"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 4:
                    result = @"{""MESSAGE_CODE"":""4"",""MESSAGE_DESC"":""Service operation error, please contract system admin."",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 5:
                    result = @"{""MESSAGE_CODE"":""5"",""MESSAGE_DESC"":""ข้อมูลลำดับที่ 1 กรุณาระบุ [Field_1],[Field_2],[Field...]"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 6:
                    result = @"{""MESSAGE_CODE"":""6"",""MESSAGE_DESC"":""Service operation error, please contract system admin."",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 7:
                    result = @"{""MESSAGE_CODE"":""7"",""MESSAGE_DESC"":""ไม่พบรายการ product code ในระบบ"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 8:
                    result = @"{""MESSAGE_CODE"":""8"",""MESSAGE_DESC"":""Service operation error, please contract system admin."",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                case 9:
                    result = @"{""MESSAGE_CODE"":""9"",""MESSAGE_DESC"":""ไม่สามารถ run เลขที่กรมธรรม์ได้"",""POLICYNO"":"",""APPNO"":"""",""RETURNDATE"":""04 / 11 / 2019"",""RETURNTIME"":""11:07:00""}";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }
    }

    public class TNIJsonData
    {
        public DataNon DataNon { get; set; }
    }

    public class DataNon
    {
        public string APP_NO { get; set; }
        public string APP_DATE { get; set; }
        public string APP_INFORM_NAME { get; set; }
        public string CUS_TYPE { get; set; }
        public string CUS_FOREIGNER { get; set; }
        public string CUS_PREFIX { get; set; }
        public string CUS_NAME { get; set; }
        public string CUS_SURNAME { get; set; }
        public string CUS_IDTYPE { get; set; }
        public string CUS_IDCARD { get; set; }
        public string CUS_SEX { get; set; }
        public string CUS_BIRTHDATE { get; set; }
        public string CUS_OCCUPATION { get; set; }
        public string CUS_NATION { get; set; }
        public string CUS_EMAIL { get; set; }
        public string CUS_TAX_LOCATION { get; set; }
        public string CUS_TAX_BRN { get; set; }
        public string CUS_TAX_BRN_NM { get; set; }
        public string ADDR_POLADDR { get; set; }
        public string ADDR_POLBLD { get; set; }
        public string ADDR_POLMOO { get; set; }
        public string ADDR_POLSOI { get; set; }
        public string ADDR_POLROAD { get; set; }
        public string ADDR_DISTRICT { get; set; }
        public string ADDR_AMPHUR { get; set; }
        public string ADDR_PROVINCE { get; set; }
        public string ADDR_POSTCODE { get; set; }
        public string ADDR_TEL { get; set; }
        public string SEND_PREFIX { get; set; }
        public string SEND_NAME { get; set; }
        public string SEND_SURNAME { get; set; }
        public string SEND_ADDR1 { get; set; }
        public string SEND_ADDR2 { get; set; }
        public string SEND_DISTRICT { get; set; }
        public string SEND_AMPHUR { get; set; }
        public string SEND_PROVINCE { get; set; }
        public string SEND_POSTCODE { get; set; }
        public string SEND_TEL { get; set; }
        public string SEND_FAX { get; set; }
        public string PREM_TOTAL { get; set; }
        public string PAID_DATE { get; set; }
        public string PAID_TYPE { get; set; }
        public string PAID_DOC_DESC { get; set; }
        public string PAID_CRCARD { get; set; }
        public string INF_RCP_NAME { get; set; }
        public string INF_RCP_DATE { get; set; }
        public string INF_RCP_NAMETYPE { get; set; }
        public string INF_RCP_CUSTYPE { get; set; }
        public string INF_RCP_IDCARD { get; set; }
        public string INF_RCP_TAXID { get; set; }
        public string INF_RCP_TAX_LOCATION { get; set; }
        public string INF_RCP_TAX_BRN { get; set; }
        public string INF_RCP_TAX_BRN_NM { get; set; }
        public string INI_COMPANY_CODE { get; set; }
        public string SERVICETYPE { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string PACKAGE_CODE { get; set; }
        public string EFFECT_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string PRINT_EPOLICY { get; set; }
        public string SUMINS { get; set; }
        public string NET { get; set; }
        public string STAMP { get; set; }
        public string VAT { get; set; }
        public string TOTAL { get; set; }
        public string TA_DESTINATION { get; set; }
        public Beneficial NonItemList { get; set; }
    }

    public class Beneficial
    {
        public List<DataNonBenefit> DataNonItem { get; set; }
    }

    public class DataNonBenefit
    {
        public string REF_ID { get; set; }
        public string INSUS_SEQ { get; set; }
        public string INSCUS_PREFIX { get; set; }
        public string INSCUS_NAME { get; set; }
        public string INSCUS_SURNAME { get; set; }
        public string INSCUS_IDTYPE { get; set; }
        public string INSCUS_IDCARD { get; set; }
        public string INSCUS_OCCUPATION { get; set; }
        public string INSCUS_BIRTHDATE { get; set; }
        public string INSCUS_SEX { get; set; }
        public string INSCUS_BENE01_PREFIX { get; set; }
        public string INSCUS_BENE01_NAME { get; set; }
        public string INSCUS_BENE01_SURNAME { get; set; }
        public string INSCUS_BENE01_RELATION { get; set; }
        public string INSCUS_BENE01_RATE { get; set; }
        public string INSCUS_BENE02_PREFIX { get; set; }
        public string INSCUS_BENE02_NAME { get; set; }
        public string INSCUS_BENE02_SURNAME { get; set; }
        public string INSCUS_BENE02_RELATION { get; set; }
        public string INSCUS_BENE02_RATE { get; set; }
        public string INSCUS_ADDR_POLADDR { get; set; }
        public string INSCUS_ADDR_POLBLD { get; set; }
        public string INSCUS_ADDR_POLMOO { get; set; }
        public string INSCUS_ADDR_POLSOI { get; set; }
        public string INSCUS_ADDR_POLROAD { get; set; }
        public string INSCUS_ADDR_DISTRICT { get; set; }
        public string INSCUS_ADDR_AMPHUR { get; set; }
        public string INSCUS_ADDR_PROVINCE { get; set; }
        public string INSCUS_ADDR_POSTCODE { get; set; }
        public string INSCUS_ADDR_TEL { get; set; }
    }

    public class TNIWebServiceResult
    {
        public int ApplicationId { get; set; }
        public bool IsCallSuccess { get; set; }
        public string ErrorText { get; set; }
        public DateTime InvokeDateTime { get; set; }
        public string Message_Code { get; set; }
        public string Message_Desc { get; set; }
        public string PolicyNo { get; set; }
        public string AppNo { get; set; }
        public string FullResultReturn { get; set; }
        public string JsonOut { get; set; }

        public TNIWebServiceResult()
        {
            this.InvokeDateTime = DateTime.Now;
        }
    }
}