using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SmileSMotorClassLibrary;
using System.Data;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmAddEditAppCustomer : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //โหลดข้อมูล dropdownlist
                this.ucAddEditCustomer.DoLoadDropdownList();
                this.ucAddEditCorporate.DoLoadDropdownList();
                this.ucAddEditAddress.DoLoadDropdownList();
                this.ucAddEditAddress1.DoLoadDropdownList();
                this.ucAddEditAddress2.DoLoadDropdownList();
                this.ddlPersonType1.DoLoadDropdownList();

                //Set default PersonType =2 and visible Corporate = false
                this.ucAddEditCorporate.Visible = false;
                this.ucAddEditCustomer.Visible = false;
                //this.ddlPersonType1.PersonTypeID = 2; //บุคคลธรรมดา

                //เช็ค query string cid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != string.Empty)
                {
                    DataCenterDB dbc = new DataCenterDB();
                    //Decrypt querystring cid เก็บไว้ใน Hiddenfield
                    hdfCustomer_ID.Value = cFunction.Base64Decrypt(Request.QueryString["cid"]);
                    int _personid = Convert.ToInt32(hdfCustomer_ID.Value);

                    //get card type
                    int cardtype_id = dbc.GetPersonCardByPersonID(_personid).Select(o => o.PersonCardType_ID).FirstOrDefault();

                    if (cardtype_id == 2 || cardtype_id == 3)
                    {
                        //โหลดข้อมูลจาก personid -- บุคคลธรรมดา
                        DoloadCustomerDetailByPersonID(_personid);
                        DoloadAddressByPersonID(_personid);
                        this.ucAddEditCustomer.Visible = true;
                        this.ucAddEditCorporate.Visible = false;
                        this.ddlPersonType1.PersonTypeID = 2; //บุคคลธรรมดา
                    }
                    else
                    {
                        //โหลดข้อมูลจาก personid -- นิติฯ
                        DoloadCorporateDetailByPersonID(_personid);
                        DoloadAddressByPersonID(_personid);
                        this.ucAddEditCorporate.Visible = true;
                        this.ucAddEditCustomer.Visible = false;
                        this.ddlPersonType1.PersonTypeID = 3; //นิติ
                    }
                    this.ucPersonSearchByIdentityCardNumber.SetPersonTypeIDTextbox(ddlPersonType1.PersonTypeID.ToString()); //set ค่า persontype
                    IsEnabled(false);
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
                    hdfPayer_ID.Value = cFunction.Base64Decrypt(Request.QueryString["pid"]);
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
                    hdfApp_ID.Value = cFunction.Base64Decrypt(Request.QueryString["appid"]);
                }

                //appid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    hdfApp_ID.Value = string.Empty;
                }
            }

            //เช็คข้อมูลผู้เอาประกัน เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            IsValidateEditPerson(hdfCustomer_ID.Value, hdfApp_ID.Value);

            //set ความกว้างให้กับ button
            btnCopyAddress.Width = Unit.Pixel(150);
            btnCopyAddress1.Width = Unit.Pixel(150);
        }

        /// <summary>
        /// Event Click ปุ่มถัดไป
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnForward_Click(object sender, EventArgs e)
        {
            string newIdentityCardID = "";
            string newPassportID = "";
            string newTaxpayerNo = "";

            int personType = ddlPersonType1.PersonTypeID;

            //Check value จาก ddl ประเภทบุคคล/นิติบุคคล
            if (personType == (int)MappingCode.ePersonType.Individual)
            {
                //ถ้าเป็น บุคคลธรรมดา
                //declear&set card detail & passport
                newIdentityCardID = ucAddEditCustomer.GetCardDetail();
                newPassportID = ucAddEditCustomer.GetCardPassport();
            }
            else if (personType == (int)MappingCode.ePersonType.Corporate)
            {
                newTaxpayerNo = ucAddEditCorporate.GetTaxpayerNo();
            }

            string encryptCustomerID;
            string encryptPayerID;
            string encryptAppID;

            //เช็ค hdfCUstomer_id ไม่เท่ากับ string.emtpy
            if (hdfCustomer_ID.Value == "")
            {
                //validate ข้อมูลลูกค้า
                if (IsValidate(newIdentityCardID, newPassportID, newTaxpayerNo) == true) //ส่ง parameter newเลขประจำตัวผู้เสียภาษี ไปอีก 1 ตัว
                {
                    //บันทึกข้อมูล return cus_id กลับมาเก็บไว้ใน hdfCustomer_id
                    hdfCustomer_ID.Value = DoSave().ToString();

                    //เข้ารหัส id
                    encryptCustomerID = cFunction.Base64Encrypt(hdfCustomer_ID.Value);
                    encryptPayerID = cFunction.Base64Encrypt(hdfPayer_ID.Value);
                    encryptAppID = cFunction.Base64Encrypt(hdfApp_ID.Value);

                    //redirect ไปหน้า frmAddEditAppPayer
                    Response.Redirect("~/Modules/Motor/frmAddEditAppPayer.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
                }
            }

            //ถ้าไม่
            else
            {
                //ถ้า App Id ไม่เท่ากับค่าว่าง
                if (hdfApp_ID.Value != "")
                {
                    var personRelatedTypeId = 2; //2 คือผู้เอาประกัน // 3 คือ Type ผู้ชำระเบี้ย
                    var personId = Convert.ToInt32(hdfCustomer_ID.Value);
                    var motorId = Convert.ToInt32(hdfApp_ID.Value);
                    var userId = cFunction.GetCurrentLoginUser_ID(Page);
                    DoUpdateCustomer(personRelatedTypeId, personId, motorId, userId);
                }
                //เข้ารหัส id
                encryptCustomerID = cFunction.Base64Encrypt(hdfCustomer_ID.Value);
                encryptPayerID = cFunction.Base64Encrypt(hdfPayer_ID.Value);
                encryptAppID = cFunction.Base64Encrypt(hdfApp_ID.Value);

                //redirect ไปหน้า Payer
                Response.Redirect("~/Modules/Motor/frmAddEditAppPayer.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
            }
        }

        protected void btnCopyAddress_Click(object sender, EventArgs e)
        {
            //ถ้า zipcode ไม่เท่ากับว่าง
            if (this.ucAddEditAddress.Address.ZipCode != "")
            {
                //copy ucaddress
                this.ucAddEditAddress1.Copyform(ucAddEditAddress);
            }
            else
            {
                //แจ้งเตืนข้อมูลไม่สมบูรณ์
                this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ที่อยู่ปัจจุบันยังไม่สมบูรณ์");
            }
        }

        protected void btnCopyAddress1_Click(object sender, EventArgs e)
        {
            //ถ้า zipcode ไม่เท่ากับว่าง
            if (this.ucAddEditAddress.Address.ZipCode != "")
            {
                //copy ucaddress
                this.ucAddEditAddress2.Copyform(ucAddEditAddress);
            }
            else
            {
                //แจ้งเตืนข้อมูลไม่สมบูรณ์
                this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ที่อยู่ปัจจุบันยังไม่สมบูรณ์");
            }
        }

        protected void ucPersonSearchByIdentityCardNumber_eClickClear(object sender, EventArgs e)
        {
            // Clear
            hdfCustomer_ID.Value = "";
            hdfPayer_ID.Value = "";
            this.ucAddEditCustomer.IsEnabled(true);
            this.ucAddEditCorporate.IsEnabled(true);
            this.ucAddEditAddress.IsEnabled(true);
            this.ucAddEditAddress1.IsEnabled(true);
            this.ucAddEditAddress2.IsEnabled(true);
            this.btnCopyAddress.Enabled = true;
            this.btnCopyAddress1.Enabled = true;

            this.ucAddEditCustomer.DoClear();
            this.ucAddEditCorporate.DoClear();
            this.ucAddEditAddress.Doclear();
            this.ucAddEditAddress1.Doclear();
            this.ucAddEditAddress2.Doclear();

            this.ddlPersonType1.PersonTypeID = -1;
            this.ucAddEditCustomer.Visible = false;
            this.ucAddEditCorporate.Visible = false;

            //set default warning
            this.ucAddEditCustomer.SetAlertWarning(false);
            this.ucAddEditCorporate.SetAlertWarning(false);
            this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
        }

        protected void ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged(object sender, EventArgs e)
        {
            DataCenterDB dbc = new DataCenterDB();
            // Get ค่า person_id ไว้ใน hdfPerson_ID
            hdfCustomer_ID.Value = ucPersonSearchByIdentityCardNumber.Person_ID.ToString();
            int customer_id = Convert.ToInt32(hdfCustomer_ID.Value);

            //เช็คข้อมูลผู้เอาประกัน เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            IsValidateEditPerson(hdfCustomer_ID.Value, hdfApp_ID.Value);

            //get card type
            int cardtype_id = dbc.GetPersonCardByPersonID(customer_id).Select(o => o.PersonCardType_ID).FirstOrDefault();

            if (cardtype_id == 2 || cardtype_id == 3)
            {
                //โหลด Detail  ucCustomer บุคคลธรรมดา
                ucAddEditCustomer.Visible = true;
                ucAddEditCorporate.Visible = false;
                DoloadCustomerDetailByPersonID(customer_id);
                DoloadAddressByPersonID(customer_id);
                ddlPersonType1.PersonTypeID = 2; //set dropdownlist เป็นบุคคลธรรมดา

                //set default warning
                this.ucAddEditCustomer.SetAlertWarning(false);
            }
            else
            {
                ucAddEditCorporate.Visible = true;
                ucAddEditCustomer.Visible = false;
                DoloadCorporateDetailByPersonID(customer_id);
                DoloadAddressByPersonID(customer_id);
                ddlPersonType1.PersonTypeID = 3; //set dropdownlist เป็นนิติบุคคล
                //set default warning
                this.ucAddEditCorporate.SetAlertWarning(false);
            }

            //disabe uc && btn
            this.ucAddEditCustomer.IsEnabled(false);
            this.ucAddEditCorporate.IsEnabled(false);
            this.ucAddEditAddress.IsEnabled(false);
            this.ucAddEditAddress1.IsEnabled(false);
            this.ucAddEditAddress2.IsEnabled(false);
            this.btnCopyAddress.Enabled = false;
            this.btnCopyAddress1.Enabled = false;

            this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
        }

        //click null
        protected void ucPersonSearchByIdentityCardNumber_eClickPersonIDChangednull(object sender, EventArgs e)
        {
            //alert
            ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.info, "กรอกคำค้น");
        }

        //Persontype selectchanged
        protected void ddlPersonType_eSelectChanged(object sender, EventArgs e)
        {
            //hdfPersonTypeID.Value = Convert.ToString(ddlPersonType1.PersonTypeID);
            int personTypeID = ddlPersonType1.PersonTypeID;

            if (personTypeID == 3) // 3 นิติบุคคล
            {
                this.ucAddEditCustomer.Visible = false;
                this.ucAddEditCorporate.Visible = true;

                //clear
                this.ucAddEditCustomer.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditCustomer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfCustomer_ID.Value = "";
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditAddress.IsEnabled(true);
                this.ucAddEditAddress1.IsEnabled(true);
                this.ucAddEditAddress2.IsEnabled(true);
                this.ucAddEditCorporate.DoLoadDropdownList(); //โหลด dropdownlist นิติ
                this.btnCopyAddress.Enabled = true;
                this.btnCopyAddress1.Enabled = true;
            }
            else if (personTypeID == 2) // 2 บุคคลธรรมดา
            {
                this.ucAddEditCustomer.Visible = true;
                this.ucAddEditCorporate.Visible = false;

                //clear
                this.ucAddEditCorporate.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditCustomer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfCustomer_ID.Value = "";
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditAddress.IsEnabled(true);
                this.ucAddEditAddress1.IsEnabled(true);
                this.ucAddEditAddress2.IsEnabled(true);
                this.ucAddEditCustomer.DoLoadDropdownList(); //โหลด dropdownlist บุคคลธรรมดา
                this.btnCopyAddress.Enabled = true;
                this.btnCopyAddress1.Enabled = true;
            }
            else
            {
                this.ucAddEditCustomer.Visible = false;
                this.ucAddEditCorporate.Visible = false;
                //clear
                this.ucAddEditCustomer.DoClear();
                this.ucAddEditCorporate.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditCustomer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfCustomer_ID.Value = "";
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditAddress.IsEnabled(true);
                this.ucAddEditAddress1.IsEnabled(true);
                this.ucAddEditAddress2.IsEnabled(true);
            }

            this.ucPersonSearchByIdentityCardNumber.SetPersonTypeIDTextbox(ddlPersonType1.PersonTypeID.ToString());
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Save Customer
        /// </summary>
        /// <returns></returns>
        public int DoSave()
        {
            DataCenterDB db = new DataCenterDB();
            Person p = new Person();
            List<PersonAddress> lstPersonAddr = new List<PersonAddress>();
            PersonAddress perSonAddr1 = new PersonAddress();
            PersonAddress perSonAddr2 = new PersonAddress();
            PersonAddress perSonAddr3 = new PersonAddress();
            Address addr1 = new Address();
            Address addr2 = new Address();
            Address addr3 = new Address();
            FunctionManager fm = new FunctionManager();
            int result;
            int personTypeID = ddlPersonType1.PersonTypeID;

            switch (personTypeID)
            {
                case (int)MappingCode.ePersonType.Individual: // 2 คือบุคคลธรรมดา
                    p = this.ucAddEditCustomer.Person;
                    break;

                case (int)MappingCode.ePersonType.Corporate: // 3 นิติบุคคล
                    p = this.ucAddEditCorporate.Person;
                    break;

                default:
                    break;
            }

            // 2 คือบุคคลธรรมดา 3 นิติ
            p.PersonType_ID = personTypeID;

            addr1 = this.ucAddEditAddress.Address; //ปัจจุบัน
            addr2 = this.ucAddEditAddress1.Address; //ที่ทำงาน
            addr3 = this.ucAddEditAddress2.Address; //ตามบัตร

            //Add to List
            perSonAddr1.AddressType_ID = 2; //ที่อยู่ปัจจุบัน
            perSonAddr1.Address = addr1;

            perSonAddr2.AddressType_ID = 3; //ที่อยู่ตามบัตรประชาชน
            perSonAddr2.Address = addr3;

            perSonAddr3.AddressType_ID = 4; //ที่อยู่ที่ทำงาน
            perSonAddr3.Address = addr2;

            lstPersonAddr.Add(perSonAddr1);
            lstPersonAddr.Add(perSonAddr2);
            lstPersonAddr.Add(perSonAddr3);

            result = db.AddApplication_Person(p, p.PersonCard, p.PersonContact, lstPersonAddr, this.Page);

            return result;
        }

        /// <summary>
        /// Doload Address By Person ID
        /// </summary>
        /// <param name="person_id">รหัส Person</param>
        private void DoloadAddressByPersonID(int person_id)
        {
            //  Setup Person ID
            int _person_ID = person_id;
            int _addressType_1;
            int _addressType_2;
            int _addressType_3;
            DataCenterDB db = new DataCenterDB();

            //get Address_ID
            _addressType_1 = db.GetPersonAddressByPersonID(_person_ID, 2).FirstOrDefault().Address_ID; //ปัจจุบัน
            _addressType_2 = db.GetPersonAddressByPersonID(_person_ID, 4).FirstOrDefault().Address_ID; //ที่ทำงาน
            _addressType_3 = db.GetPersonAddressByPersonID(_person_ID, 3).FirstOrDefault().Address_ID; //ตามบัตร

            //load ที่อยู่โดย address_ID
            ucAddEditAddress.DoLoadByAddressID(_addressType_1); //ปัจจุบัน
            ucAddEditAddress1.DoLoadByAddressID(_addressType_2); //ที่ทำงาน
            ucAddEditAddress2.DoLoadByAddressID(_addressType_3); // ตามบัตร
        }

        /// <summary>
        /// Load Customer Detail by Person_id
        /// </summary>
        /// <param name="person_id">ส่ง Person_id เข้ามา</param>
        private void DoloadCustomerDetailByPersonID(int person_id)
        {
            ucAddEditCustomer.DoLoadDetail(person_id);
        }

        /// <summary>
        ///  Load Corporate Detail by Person_id
        /// </summary>
        /// <param name="person_id"></param>
        private void DoloadCorporateDetailByPersonID(int person_id)
        {
            ucAddEditCorporate.DoLoad(person_id);
        }

        /// <summary>
        /// Enable uc & btn
        /// </summary>
        /// <param name="value">รับค่า true or false</param>
        private void IsEnabled(bool value)
        {
            this.ucAddEditCustomer.IsEnabled(value);
            this.ucAddEditCorporate.IsEnabled(value);
            this.ucAddEditAddress.IsEnabled(value);
            this.ucAddEditAddress1.IsEnabled(value);
            this.ucAddEditAddress2.IsEnabled(value);
            this.btnCopyAddress.Enabled = value;
            this.btnCopyAddress1.Enabled = value;
        }

        /// <summary>
        /// Validate Customer
        /// </summary>
        /// <param name="personcardDetail">เลขบัตรประจำตัวประชาชน</param>
        /// <param name="passportcardDetail">เลข passport</param>
        /// <returns></returns>
        private bool IsValidate(string personcardDetail, string passportcardDetail, string newTaxpayerNo)
        {
            DataCenterDB db = new DataCenterDB();
            DataTable dt = new DataTable();
            string address1;
            string address2;
            string address3;
            string customer;
            string corporate;

            if (ddlPersonType1.PersonTypeID == -1 || ddlPersonType1.PersonTypeID == 1)
            {
                //แจ้งเตือน
                this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาเลือกประเภทผู้เอาประกัน!");
                ucAddEditCustomer.SetAlertWarning(true);
                ucAddEditCustomer.Focus();
                return false;
            }
            //เช็คเลขบัตรประชาชน or เลข passport ไม่เท่ากับ string.empty
            if (personcardDetail != string.Empty || passportcardDetail != string.Empty || newTaxpayerNo != string.Empty) //เพิ่ม validate หมายเลขประจำตัวผู้เสียภาษี
            {
                //เช็คเลขบัตร ปชช ไม่เท่ากับ string.empty
                if (personcardDetail != string.Empty)
                {
                    //เช็ครูปแบบเลขบัตร ปชช ถ้าถูกต้อง
                    if (ucAddEditCustomer.IsValidateIdentityCard())
                    {
                        //ค้นหาเลขบัตรว่ามีอยู่ในระบบแล้วหรือยัง?
                        dt = db.GetPersonSearchByCardDetail(personcardDetail, 2, null); //2 คือ type บัตรประชาชน

                        //ถ้ามีแล้ว
                        if (dt.Rows.Count > 0)
                        {
                            //แจ้งเตือน
                            this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลขบัตรประจำตัวประชาชนมีอยู่ในระบบแล้ว!");
                            ucAddEditCustomer.SetAlertWarning(true);
                            ucAddEditCustomer.Focus();
                            return false;
                        }
                        //ไม่มี
                        ucAddEditCustomer.SetAlertWarning(false);
                    }

                    //รูปแบบไม่ถูกต้อง
                    else
                    {
                        //แจ้งเตือน
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบไม่ถูกต้อง กรุณาตรวจสอบหมายเลขบัตรประจำตัวประชาชน!");
                        ucAddEditCustomer.SetAlertWarning(true);
                        ucAddEditCustomer.Focus();
                        return false;
                    }
                }

                //เช็คหมายเลข passport ไม่เท่ากับ string.empty
                if (passportcardDetail != string.Empty)
                {
                    //ค้นหาว่ามีเลขPassportนี้แล้วหรือยัง?
                    dt = db.GetPersonSearchByCardDetail(passportcardDetail, 3, null); // 3 คือ type passport

                    //มีแล้ว
                    if (dt.Rows.Count > 0)
                    {
                        //แจ้งเตือนว่ามีแล้ว
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลข Passport มีอยู่ในระบบแล้ว!");
                        ucAddEditCustomer.SetAlertWarning(true);
                        ucAddEditCustomer.Focus();
                        return false;
                    }
                    //ไม่มี
                    ucAddEditCustomer.SetAlertWarning(false);
                }

                //เช็คหมายเลขประจำตัวผู้เสียภาษี ไม่เท่ากับ string.empty
                if (newTaxpayerNo != string.Empty)
                {
                    //ค้นหาว่ามีเลขPassportนี้แล้วหรือยัง?
                    dt = db.GetPersonSearchByCardDetail(newTaxpayerNo, 4, null); // 4 คือ เลขประจำตัวผู้เสียภาษี

                    //มีแล้ว
                    if (dt.Rows.Count > 0)
                    {
                        //แจ้งเตือนว่ามีแล้ว
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลขผู้เสียภาษี มีอยู่ในระบบแล้ว!");
                        ucAddEditCorporate.SetAlertWarning(true);
                        ucAddEditCorporate.Focus();
                        return false;
                    }
                    //ไม่มี
                    ucAddEditCorporate.SetAlertWarning(false);
                }

                //validate บุคคลธรรมดา
                if (ddlPersonType1.PersonTypeID == 2) //บุคคลธรรมดา
                {
                    //ValidateCustomer โดย class return ค่าเป็น string
                    customer = ucAddEditCustomer.ValidateCustomer();

                    //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                    if (customer != "")
                    {
                        //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, customer);
                        return false;
                    }

                    //บันทึกข้อมูลบุคคลใหม่ ต้องระบุวันเกิด ส่ง IsRequire = true
                    //validate รูปแบบ วันเกิด
                    if (ucAddEditCustomer.IsValidateBirthDate(true) == false)
                    {
                        //รูปแบบวันที่ไม่ถูกต้องให้แจ้งเตือน
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบวันเกิดไม่ถูกต้อง");
                        return false;
                    }
                }

                //validate นิติบุคคล
                if (ddlPersonType1.PersonTypeID == 3) //นิติบุคคล
                {
                    //ValidateCustomer โดย class return ค่าเป็น string
                    corporate = ucAddEditCorporate.ValidateCorporate();

                    if (corporate != "")
                    {
                        //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                        this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, corporate);
                        return false;
                    }
                }

                //Validate ที่อยู่ปัจจุบัน โดย class return ค่าเป็น string
                address1 = ucAddEditAddress.ValidateAddress();

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (address1 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address1);
                    return false;
                }

                //Validate ที่อยู่ตามบัตร ปชช โดย class return ค่าเป็น string
                address2 = ucAddEditAddress1.ValidateAddress();

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (address2 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address2);
                    return false;
                }

                //Validate ที่อยู่ที่ทำงาน โดย class return ค่าเป็น string
                address3 = ucAddEditAddress2.ValidateAddress();

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (address3 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address3);
                    return false;
                }
            }

            //เลขบัตร ปชช && passport เท่ากับว่าง
            else
            {
                //แจ้งเตือน
                this.ucAlert1.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาระบุหมายเลขบัตรประจำตัวประชาชน,หมายเลข Passport หรือ เลขประจำตัวผู้เสียภาษี");
                ucAddEditCustomer.SetAlertWarning(false);
                return false;
            }

            return true;
        }

        private void IsValidateEditPerson(string queryStringPersonid, string queryStringMotorAppId)
        {
            //เช็คข้อมูล person เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            if (queryStringPersonid != string.Empty)
            {
                int? motorappid = null;
                if (queryStringMotorAppId != string.Empty)
                {
                    motorappid = Convert.ToInt32(queryStringMotorAppId);
                }

                ucAddEditCustomer.ValidateButtonEditPerson(Convert.ToInt32(queryStringPersonid), cFunction.GetCurrentLoginUser_ID(Page), motorappid);
            }
        }

        #endregion Method

        private void DoUpdateCustomer(int personRelateAppTypeId, int personId, int motorId, int updateById)
        {
            //UPDATE PAYER
            using (var db = new MotorV1Entities())
            {
                var st = db.usp_MotorApplication_SelectStatusByID(motorId).First();

                if (st.MotorApplicationStatus_ID == 7) //Status บันทึกลูกค้าใหม่
                {
                    db.usp_MotorApplicationPersonRelatedApplication_Update(motorId, personId, personRelateAppTypeId, updateById);
                }
            }
        }
    }
}