using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileMotorV1.Models;
using SmileSMotorClassLibrary;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmAddEditAppProduct : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                FunctionManager fm = new FunctionManager();

                //โหลดข้อมูล dropdownlist
                this.ucAddEditApplication.DoloadDropdownList();
                this.ucAddEditVehicle.DoloadDropDownList();
                this.ucAddEditPayMethod.DoLoadDropdownlist();
                this.ucApplicationOwner.DoloadDropdownlist();

                //set type เป็นเจ้าของผลงาน
                this.ucApplicationOwner.DoSetTypeOwner();

                //enable motorapcode = false
                this.ucAddEditApplication.IsEnabledMotorAppCode(false);

                //enable dropdownlist = false
                this.ucAddEditPayMethod.IsEnableDDL(false);

                //เช็ค query string cid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != string.Empty)
                {
                    //Decrypt querystring cid เก็บไว้ใน Hiddenfield
                    hdfCustomer_ID.Value = fm.Base64Decrypt(Request.QueryString["cid"]);
                    int _personid = Convert.ToInt32(hdfCustomer_ID.Value);
                    this.ucPersonDetail_customer.DoLoad(_personid);
                }

                //cid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    hdfCustomer_ID.Value = "";
                }

                //เช็ค query string pid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != string.Empty)
                {
                    //Decrypt querystring pid เก็บไว้ใน Hiddenfield
                    hdfPayer_ID.Value = fm.Base64Decrypt(Request.QueryString["pid"]);
                    int _payerid = Convert.ToInt32(hdfPayer_ID.Value);

                    //load
                    this.ucPersonDetail_payer.DoLoad(_payerid);
                    this.ucAddEditPayMethod.DoLoadDropdownlist(_payerid);
                    this.ucAddEditPayMethod.DoloadBankAccount(_payerid);
                    this.ucAddEditApplication.SetDateSelectedToday();
                }

                //pid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    hdfPayer_ID.Value = "";
                }

                //เช็ค query string appid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["appid"] != null && Request.QueryString["appid"] != string.Empty)
                {
                    //Decrypt querystring appid เก็บไว้ใน Hiddenfield
                    hdfApp_ID.Value = fm.Base64Decrypt(Request.QueryString["appid"]);
                    MotorDB db = new MotorDB();

                    //set id
                    int _appid = Convert.ToInt32(hdfApp_ID.Value);
                    var result = db.GetMotorApplicationByMotorApplicationID(_appid);
                    var customerId = result.Customer.Person_ID;
                    var payerId = result.Payer.Person_ID;
                    hdfCustomer_ID.Value = customerId.ToString();
                    hdfPayer_ID.Value = payerId.ToString();
                    //load
                    this.ucPersonDetail_customer.DoLoad(customerId);
                    this.ucPersonDetail_payer.DoLoad(payerId);
                    this.ucAddEditPayMethod.DoLoadDropdownlist(payerId);
                    this.ucAddEditPayMethod.DoloadBankAccount(payerId);

                    this.ucAddEditApplication.DoLoad(_appid);
                    this.ucAddEditVehicle.DoLoad(_appid);
                    //ตรวจสอบว่าเป็นแอปต่ออายุหรือไม่
                    var contracttypeId = result.MotorApplicationContractType_ID;
                    if (contracttypeId == 3 && result.MotorApplication_ID != result.MotorApplication_IDGroup)//แอปต่ออายุ
                    {
                        this.ucAddEditVehicle.IsEnabled(false);
                        divRenew.Visible = true;
                        ucAppDetailAppReNew.Doload(_appid);
                    }
                    this.ucAddEditPayMethod.DoLoad(_appid, payerId);
                    this.ucAddEditApplication.IsEnabledMotorAppCode(false);
                    this.ucApplicationOwner.DoLoad(_appid);

                    //Check ผลิตภัณฑ์
                    CheckProductPackage();
                }

                //appid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    hdfApp_ID.Value = "";

                    //เจ้าของผลงาน set ให้เป็น user ที่ login เข้ามา
                    this.ucApplicationOwner.LoadOwnerDefault();
                    //Remove ContractTypeID3 ต่ออายุ
                    ucAddEditApplication.RemoveContractTypeItemIndex(3);
                }

                if (HttpContext.Current.User.IsInRole("MotorDeveloper"))
                {
                    btnInputData.Visible = true;
                }
            }
        }

        /// <summary>
        /// ถัดไป
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnForward_Click(object sender, EventArgs e)
        {
            //app_id ไม่เท่ากับ string.empty
            if (hdfApp_ID.Value != string.Empty)
            {
                //validate
                if (IsValidate() == true)
                {
                    //Update and return motorAppID
                    int motorAppID = DoUpdate();

                    //เข้ารหัส base64 motoAppID
                    string EncryptMotorID = cFunction.Base64Encrypt(motorAppID.ToString());

                    //Redirect frmAddEditAppOther
                    Response.Redirect("~/Modules/Motor/frmAddEditAppOther.aspx?" + "appid=" + EncryptMotorID + "");
                }
            }

            //app_id ว่าง
            else
            {
                //validate
                if (IsValidate() == true)
                {
                    if (this.ucAddEditVehicle.ValidateDuplicate() == true)
                    {
                        //Save and return motoAppID
                        int motorAppID = DoSave();

                        //เข้ารหัส base64 motorAppID
                        string EncryptMotorID = cFunction.Base64Encrypt(motorAppID.ToString());

                        //Save วิธีการชำระเงิน
                        DoSavePayMethod(motorAppID);

                        //Save เจ้าของผลงาน
                        DoSaveOwner(motorAppID);

                        //Redirect frmAddEditAppOther
                        Response.Redirect("~/Modules/Motor/frmAddEditAppOther.aspx?" + "appid=" + EncryptMotorID + "");
                    }
                    else
                    {
                        //แจ้งเตือน
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "พบข้อมูลซ้ำ ! กรุณาตรวจสอบหมายเลขทะเบียน / เลขเครื่องยนต์ /เลขตัวถัง");
                    }
                }
            }
        }

        /// <summary>
        /// ย้อนกลับ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackward_Click(object sender, EventArgs e)
        {
            //app_id ไม่เท่ากับ ว่าง
            if (hdfApp_ID.Value != string.Empty)
            {
                FunctionManager fm = new FunctionManager();

                //set and encrypt id base64
                string encryptCustomerID = fm.Base64Encrypt(hdfCustomer_ID.Value);
                string encryptPayerID = fm.Base64Encrypt(hdfPayer_ID.Value);
                string encryptAppID = fm.Base64Encrypt(hdfApp_ID.Value);

                //redirect frmAddEditAppPayer
                Response.Redirect("~/Modules/Motor/frmAddEditAppPayer.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
            }

            //app_id ว่าง
            else
            {
                FunctionManager fm = new FunctionManager();

                //set and encrypt id base64
                string encryptCustomerID = fm.Base64Encrypt(hdfCustomer_ID.Value);
                string encryptPayerID = fm.Base64Encrypt(hdfPayer_ID.Value);

                //redirect frmAddEditAppPayer
                Response.Redirect("~/Modules/Motor/frmAddEditAppPayer.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=");
            }
        }

        /// <summary>
        /// Event Company Insurance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAddEditApplication_eMotorCompanyInsuranceSelectChanged(object sender, EventArgs e)
        {
            //clear วิธีการชำระเงิน
            this.ucAddEditPayMethod.DoClear();
        }

        protected void ucAddEditApplication_eProductTypeSelectChanged(object sender, EventArgs e)
        {
            //??
        }

        /// <summary>
        /// Event Select ผลิตภัณฑ์
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAddEditApplication_eProductSelectChanged(object sender, EventArgs e)
        {
            //ถ้า product ไม่เท่ากับ -1
            if (ucAddEditApplication.ProductID != -1) //-1 คือโปรดระบุ
            {
                hdfProduct_ID.Value = this.ucAddEditApplication.ProductID.ToString();

                //load เบี้ยผลิตภัณฑ์
                this.ucAddEditPayMethod.DoLoadPremiumByProduct(Convert.ToInt32(hdfProduct_ID.Value), this.ucAddEditPayMethod.PeriodType_ID);

                //check product package
                CheckProductPackage();
            }

            // ไม่ได้เลือกผลิตภัณฑ์
            else
            {
                //Clear วิธีการชำระเงิน
                this.ucAddEditPayMethod.DoClear();
            }
        }

        /// <summary>
        /// งวดชำระ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAddEditPayMethod_ePeriodTypeSelectChanged(object sender, EventArgs e)
        {
            //ProductID > 0
            if (this.ucAddEditApplication.ProductID > 0)
            {
                //Event ProductSelectChanged
                this.ucAddEditApplication_eProductSelectChanged(new object(), new EventArgs());

                //set PeriodType_ID to hidden field
                hdfPeriodType_ID.Value = this.ucAddEditPayMethod.PeriodType_ID.ToString();

                //load เบี้ยผลิตภัณฑ์
                this.ucAddEditPayMethod.DoLoadPremiumByProduct(Convert.ToInt32(hdfProduct_ID.Value), this.ucAddEditPayMethod.PeriodType_ID);
            }
        }

        #endregion Event

        #region Method

        private void DoInsertCustomerApplication(int appId, int personId, string custType)
        {
            var userID = cFunction.GetCurrentLoginUser_ID(this.Page);
            using (var db = new MotorV1Entities())
            {
                var rs = db.usp_CustomerApplication_Motor_Person_InsertOrUpdate(appId, personId, custType, userID);
            }
        }

        /// <summary>
        /// Save Product
        /// </summary>
        /// <returns></returns>
        private int DoSave()
        {
            MotorDB db = new MotorDB();
            MotorApplication motorApp = new MotorApplication();
            DateManager dm = new DateManager();
            int result = 0;

            int custPerson_id;
            int payerPerson_id;

            //set id
            custPerson_id = Convert.ToInt32(hdfCustomer_ID.Value);
            payerPerson_id = Convert.ToInt32(hdfPayer_ID.Value);

            //set to MotorApplication
            motorApp = this.ucAddEditApplication.MotorApplication;
            motorApp.VehicleforSave = this.ucAddEditVehicle.Vehicle;

            //save
            result = db.AddMotorApplicationAndVehicle(motorApp.MotorApplication_Code
                                                        , motorApp.MotorApplicationContractType_ID
                                                        , motorApp.Product.Product_ID
                                                        , motorApp.MotorApplicationClaimType_ID
                                                        , motorApp.StartCover
                                                        , motorApp.EndCover
                                                        , motorApp.Branch_ID
                                                        , motorApp.VehicleforSave.VehicleModel_ID
                                                        , motorApp.VehicleforSave.VehicleRegistrationDetail
                                                        , motorApp.VehicleforSave.VehicleRegistrationNumber
                                                        , motorApp.VehicleforSave.VehicleYear
                                                        , motorApp.VehicleforSave.VehicleChassisNumber.ToUpper()
                                                        , motorApp.VehicleforSave.VehicleEngineNumber.ToUpper()
                                                        , motorApp.VehicleforSave.VehiclePrice
                                                        , motorApp.VehicleforSave.VehicleSeat
                                                        , motorApp.VehicleforSave.VehicleWeight
                                                        , motorApp.VehicleforSave.VehicleCC
                                                        , motorApp.VehicleforSave.FuelType_ID
                                                        , custPerson_id
                                                        , payerPerson_id
                                                        , motorApp.VehicleforSave.VehicleRegistrationProvince_ID
                                                        , ucAddEditPayMethod.PeriodType_ID
                                                        , ucAddEditPayMethod.PayMethodType_ID
                                                        , ucAddEditPayMethod.PersonBankAccount_ID
                                                        , ucAddEditPayMethod.PremiumAmount
                                                        , ucAddEditPayMethod.PremiumTaxAmount
                                                        , ucAddEditPayMethod.PremiumDeliverAmount
                                                        , ucApplicationOwner.OwnerTypeID
                                                        , ucApplicationOwner.Employee.Branch_ID
                                                        , ucApplicationOwner.Employee.EmployeeTeam_ID
                                                        , ucApplicationOwner.Employee.Person_ID
                                                        , ucApplicationOwner.Employee.Employee_ID
                                                        , cFunction.GetCurrentLoginUser_ID(this.Page)
                                                        , motorApp.MotorApplicationHeirDetail
                                                        , ucApplicationOwner.ZebraID);

            DoInsertCustomerApplication(result, custPerson_id, "C");
            DoInsertCustomerApplication(result, payerPerson_id, "P");

            //return motorAppID
            return result;
        }

        /// <summary>
        /// Save วิธีการชำระเงิน
        /// </summary>
        /// <param name="motorApplicationID"></param>
        /// <returns></returns>
        private int DoSavePayMethod(int motorApplicationID)
        {
            int result;

            MotorDB db = new MotorDB();

            //save
            result = db.AddMotorApplicationPremium(motorApplicationID,
                                                   ucAddEditPayMethod.PeriodType_ID,
                                                   ucAddEditPayMethod.PayMethodType_ID,
                                                   ucAddEditPayMethod.PayMethodType_ID,
                                                   ucAddEditPayMethod.PremiumAmount,
                                                   ucAddEditPayMethod.PremiumTaxAmount,
                                                   ucAddEditPayMethod.PremiumDeliverAmount);

            return result;
        }

        /// <summary>
        /// Save เจ้าของผลงาน
        /// </summary>
        /// <param name="motorApplicationID"></param>
        /// <returns></returns>
        private int DoSaveOwner(int motorApplicationID)
        {
            MotorDB db = new MotorDB();
            int result;

            //save
            result = db.AddMotorApplicationOwner(ucApplicationOwner.OwnerTypeID,
                                                 motorApplicationID,
                                                 ucApplicationOwner.Employee.Branch_ID,
                                                 ucApplicationOwner.Employee.EmployeeTeam_ID,
                                                 ucApplicationOwner.Employee.Person_ID,
                                                 ucApplicationOwner.Employee.Employee_ID);

            return result;
        }

        /// <summary>
        /// Update motorApplication
        /// </summary>
        /// <returns></returns>
        private int DoUpdate()
        {
            MotorDB db = new MotorDB();
            MotorApplication motorApp = new MotorApplication();
            int result = 0;

            int custPerson_id;
            int payerPerson_id;
            int app_id;

            //set id
            custPerson_id = Convert.ToInt32(hdfCustomer_ID.Value);
            payerPerson_id = Convert.ToInt32(hdfPayer_ID.Value);
            app_id = Convert.ToInt32(hdfApp_ID.Value);

            //set to MotorApplication
            motorApp = this.ucAddEditApplication.MotorApplication;
            motorApp.VehicleforSave = this.ucAddEditVehicle.Vehicle;

            //set to MotorApplication
            result = db.UpdateMotorApplicationAndVehicle(app_id
                                                         , motorApp.MotorApplication_Code
                                                         , motorApp.MotorApplicationContractType_ID
                                                         , motorApp.Product.Product_ID
                                                         , motorApp.MotorApplicationClaimType_ID
                                                         , motorApp.StartCover
                                                         , motorApp.EndCover
                                                         , motorApp.Branch_ID
                                                         , motorApp.VehicleforSave.VehicleModel_ID
                                                         , motorApp.VehicleforSave.VehicleRegistrationDetail
                                                         , motorApp.VehicleforSave.VehicleRegistrationNumber
                                                         , motorApp.VehicleforSave.VehicleYear
                                                         , motorApp.VehicleforSave.VehicleChassisNumber
                                                         , motorApp.VehicleforSave.VehicleEngineNumber
                                                         , motorApp.VehicleforSave.VehiclePrice
                                                         , motorApp.VehicleforSave.VehicleSeat
                                                         , motorApp.VehicleforSave.VehicleWeight
                                                         , motorApp.VehicleforSave.VehicleCC
                                                         , motorApp.VehicleforSave.FuelType_ID
                                                         , motorApp.VehicleforSave.VehicleRegistrationProvince_ID
                                                         , custPerson_id
                                                         , payerPerson_id
                                                         , ucAddEditPayMethod.PeriodType_ID
                                                         , ucAddEditPayMethod.PayMethodType_ID
                                                         , ucAddEditPayMethod.PersonBankAccount_ID
                                                         , ucAddEditPayMethod.PremiumAmount
                                                         , ucAddEditPayMethod.PremiumTaxAmount
                                                         , ucAddEditPayMethod.PremiumDeliverAmount
                                                         , ucApplicationOwner.OwnerTypeID
                                                         , ucApplicationOwner.Employee.Branch_ID
                                                         , ucApplicationOwner.Employee.EmployeeTeam_ID
                                                         , ucApplicationOwner.Employee.Person_ID
                                                         , ucApplicationOwner.Employee.Employee_ID
                                                         , cFunction.GetCurrentLoginUser_ID(this.Page)
                                                         , motorApp.MotorApplicationHeirDetail
                                                         , ucApplicationOwner.ZebraID);

            DoInsertCustomerApplication(app_id, custPerson_id, "C");
            DoInsertCustomerApplication(app_id, payerPerson_id, "P");

            //Return id
            return result;
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <returns></returns>
        public bool IsValidate()
        {
            string application;
            string vehicle;
            string paymethod;
            string owner;
            DateManager dm = new DateManager();

            //ValidateApplication
            application = ucAddEditApplication.ValidateApplication();

            //ValidateVehicle
            vehicle = ucAddEditVehicle.ValidateVehicle();

            //ValidatePayMethod
            paymethod = ucAddEditPayMethod.ValidatePayMethod();

            //ValidateOwner
            owner = ucApplicationOwner.ValidateOwner();

            //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
            if (application != "")
            {
                //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, application);
                return false;
            }

            //เช็ครูปแบบวันที่
            if (ucAddEditApplication.IsValidateStartCover() == false)
            {
                //แจ้งเตือน
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบวันเริ่มคุ้มครองไม่ถูกต้อง");
                return false;
            }

            //if (cDate.IsValidate(ucApplicationDetail.MotorApplication.StartCover) == false)
            //{
            //    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบวันเริ่มคุ้มครองไม่ถูกต้อง");
            //    return false;
            //}

            //ยานพาหนะ
            // ถ้า return ค่าออกมาไม่ท่ากับ string.empty
            if (vehicle != "")
            {
                this.ucAddEditVehicle.ValidateFocus();
                //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, vehicle);
                return false;
            }

            //วิธีการชำระเงิน
            // ถ้า return ค่าออกมาไม่ท่ากับ string.empty
            if (paymethod != "")
            {
                //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, paymethod);
                return false;
            }

            //เจ้าของผลงาน
            // ถ้า return ค่าออกมาไม่ท่ากับ string.empty
            if (owner != "")
            {
                //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, owner);
                return false;
            }

            return true;
        }

        /// <summary>
        /// CheckProductPackage
        /// </summary>
        private void CheckProductPackage()
        {
            if (this.ucAddEditApplication.Product.IsPackage == true)
            {
                //ปิด textbox เบี้ย
                this.ucAddEditPayMethod.IsEnabledPremium(false);
            }
            else
            {
                //เปิด textbox เบี้ย
                this.ucAddEditPayMethod.IsEnabledPremium(true);
            }
        }

        #endregion Method

        protected void Button1_Click(object sender, EventArgs e)
        {
            ucAddEditApplication.SetDataTest();
            ucAddEditApplication_eProductSelectChanged(new Object(), new EventArgs());
            ucAddEditVehicle.SetDataTest();
            ucAddEditPayMethod.SetDataTest();
            //ucAddEditPayMethod_ePeriodTypeSelectChanged(new Object(), new EventArgs());
        }
    }
}