using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmManagePersonAddEdit : System.Web.UI.Page
    {
        #region Declare

        private Person _Person;

        /// <summary>
        /// Property Get/Set Person Class
        /// </summary>
        public Person Persons
        {
            get
            {
                _Person = new Person();
                if (hdfPerson_ID.Value != "")
                {
                    DataCenterDB db = new DataCenterDB();
                    _Person = db.GetPersonByPersonID(Convert.ToInt32(hdfPerson_ID.Value))[0];
                }

                return _Person;
            }
            set
            {
                _Person = value;
            }
        }

        #endregion Declare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set uc Text Header
            ucAddEditCustomer.SetTextHeader = "ข้อมูลบุคคล";
            ucAddEditCorporate.TextHeader.Text = "ข้อมูลนิติบุคคล";
            ucAddEditCustomer.setButtonEditPerson.Visible = false;

            if (HttpContext.Current.User.IsInRole("MotorUser"))
            {
                ucPersonSearch.Visible = false;
            }

            if (!IsPostBack)
            {
                // Load Dropdown List
                ucAddEditCustomer.DoLoadDropdownList();
                ucAddEditCorporate.DoLoadDropdownList();
                ucAddEditAddress.DoLoadDropdownList();
                ucAddEditAddress1.DoLoadDropdownList();
                ucAddEditAddress2.DoLoadDropdownList();
                this.ddlPersonType1.DoLoadDropdownList();

                // Check Query String
                if (Request.QueryString["personid"] != null && Request.QueryString["personid"] != string.Empty)
                {
                    DataCenterDB dbc = new DataCenterDB();

                    // Get Query String To HDF
                    hdfPerson_ID.Value = cFunction.Base64Decrypt(Request.QueryString["personid"]);

                    // Set HDF Vale From HDF
                    var _person_ID = Convert.ToInt32(hdfPerson_ID.Value);

                    //get card type
                    var cardtype_id = dbc.GetPersonCardByPersonID(_person_ID).Select(o => o.PersonCardType_ID).FirstOrDefault();

                    if (cardtype_id == 2 || cardtype_id == 3)
                    {
                        // Load uc Detail --บุคคลธรรมดา
                        DoloadCustomerDetailByPersonID(_person_ID);
                        this.ucAddEditCustomer.Visible = true;
                        this.ucAddEditCorporate.Visible = false;
                        this.ddlPersonType1.PersonTypeID = 2; //บุคคลธรรมดา
                    }
                    else
                    {
                        // Load uc Detail -- นิติบุคคล
                        DoloadCorporateDetailByPersonID(_person_ID);
                        this.ucAddEditCustomer.Visible = false;
                        this.ucAddEditCorporate.Visible = true;
                        this.ddlPersonType1.PersonTypeID = 3; //นิติบุคคล
                    }

                    ucAppDetailPersonRelatedApplication.DoLoad(_person_ID);
                    DoloadAddressByPersonID(_person_ID);
                }
                else
                {
                    // Hdf = ว่างนะจ๊ะ
                    hdfPerson_ID.Value = "";
                }
            }

            //เช็ค query string motorid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
            if (Request.QueryString["motorid"] != null && Request.QueryString["motorid"] != string.Empty)
            {
                //Decrypt querystring appid เก็บไว้ใน Hiddenfield
                hdfApp_ID.Value = cFunction.Base64Decrypt(Request.QueryString["motorid"]);
            }

            //appid เท่ากับ null && เท่ากับค่าว่าง
            else
            {
                hdfApp_ID.Value = string.Empty;
            }

            //Validate Edit Person
            IsValidateEditPerson(hdfPerson_ID.Value, hdfApp_ID.Value);

            // Set Button Style
            btnCopyAddress.Width = Unit.Pixel(150);
            btnCopyAddress1.Width = Unit.Pixel(150);
            btnSave.CssClass = "btn btn-success";
            ddlPersonType1.IsEnabled(false);
        }

        protected void ucPersonSearch_eSelectIndexPersonID(object sender, EventArgs e)
        {
            DataCenterDB dbc = new DataCenterDB();

            // เคลียร์ค่าก่อน
            DoClear();

            // Load uc Detail
            hdfPerson_ID.Value = ucPersonSearch.PersonID.ToString();
            ucAppDetailPersonRelatedApplication.DoLoad(ucPersonSearch.PersonID);

            DoloadAddressByPersonID(ucPersonSearch.PersonID);

            //validate person edit
            IsValidateEditPerson(hdfPerson_ID.Value, hdfApp_ID.Value);

            //get card type
            int cardtype_id = dbc.GetPersonCardByPersonID(ucPersonSearch.PersonID).Select(o => o.PersonCardType_ID).FirstOrDefault();

            if (cardtype_id == 2 || cardtype_id == 3)
            {
                DoloadCustomerDetailByPersonID(ucPersonSearch.PersonID);
                this.ucAddEditCorporate.Visible = false;
                this.ucAddEditCustomer.Visible = true;
                this.ddlPersonType1.PersonTypeID = 2; //บุคคลธรรมดา
            }
            else
            {
                DoloadCorporateDetailByPersonID(ucPersonSearch.PersonID);
                this.ucAddEditCorporate.Visible = true;
                this.ucAddEditCustomer.Visible = false;
                this.ddlPersonType1.PersonTypeID = 3; //นิติฯ
            }
        }

        protected void btnCopyAddress_Click(object sender, EventArgs e)
        {
            // Copy Address
            this.ucAddEditAddress1.Copyform(ucAddEditAddress);
        }

        protected void btnCopyAddress1_Click(object sender, EventArgs e)
        {
            // Copy Address
            this.ucAddEditAddress2.Copyform(ucAddEditAddress);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Validate Edit Person
            IsValidateEditPerson(hdfPerson_ID.Value, hdfApp_ID.Value);
            // Validate Card Detail
            if (IsValidate(ucAddEditCustomer.GetCardDetail(), ucAddEditCustomer.GetCardPassport(), ucAddEditCorporate.GetTaxpayerNo()) == true)
            {
                FunctionManager fm = new FunctionManager();
                string EncryptPersonID;

                // HDF ว่าง
                if (hdfPerson_ID.Value == string.Empty)
                {
                    EncryptPersonID = fm.Base64Encrypt(DoSave().ToString()); //ค่าจาก person_id ที่ return จาก Dosave
                    Response.Redirect("~/Modules/Motor/frmSuccess.aspx?" + "cid=" + EncryptPersonID);
                }
                // ไม่ว่าง
                else
                {
                    EncryptPersonID = fm.Base64Encrypt(DoUpdate().ToString());
                    Response.Redirect("~/Modules/Motor/frmSuccess.aspx?" + "cid=" + EncryptPersonID);
                }
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail By Person ID
        /// </summary>
        /// <param name="person_id"></param>
        private void DoloadCustomerDetailByPersonID(int person_id)
        {
            // Load Detail uc
            ucAddEditCustomer.DoLoadDetail(person_id);
            ucAddEditCustomer.DisableCardId(false);
            ucAddEditCustomer.DisablePassportId(false);
        }

        /// <summary>
        /// Do Load Detail By Person ID
        /// </summary>
        /// <param name="person_id"></param>
        private void DoloadCorporateDetailByPersonID(int person_id)
        {
            // Load Detail uc
            ucAddEditCorporate.DoLoad(person_id);
            ucAddEditCorporate.DisableTaxNumber(false);
        }

        /// <summary>
        /// Do Load Address By Person ID
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

            // Get Person Address By Type Person
            _addressType_1 = db.GetPersonAddressByPersonID(_person_ID, 2).FirstOrDefault().Address_ID;
            _addressType_2 = db.GetPersonAddressByPersonID(_person_ID, 3).FirstOrDefault().Address_ID;
            _addressType_3 = db.GetPersonAddressByPersonID(_person_ID, 4).FirstOrDefault().Address_ID;

            // Load uc Detail
            ucAddEditAddress.DoLoadByAddressID(_addressType_1);
            ucAddEditAddress1.DoLoadByAddressID(_addressType_2);
            ucAddEditAddress2.DoLoadByAddressID(_addressType_3);
        }

        /// <summary>
        /// Do Update Person
        /// </summary>
        /// <returns></returns>
        public int DoUpdate()
        {
            DataCenterDB db = new DataCenterDB();
            Person p = new Person();
            List<PersonCard> pCard = new List<PersonCard>();
            List<PersonContact> pContact = new List<PersonContact>();
            List<PersonAddress> lstPersonAddr = new List<PersonAddress>();
            PersonAddress perSonAddr1 = new PersonAddress();
            PersonAddress perSonAddr2 = new PersonAddress();
            PersonAddress perSonAddr3 = new PersonAddress();
            Address addr1 = new Address();
            Address addr2 = new Address();
            Address addr3 = new Address();
            FunctionManager fm = new FunctionManager();

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

            // Get Person Detail From uc
            //p = this.ucAddEditCustomer.Person;
            pCard = p.PersonCard;
            pContact = p.PersonContact;

            // Set Person ID
            p.Person_ID = Convert.ToInt32(hdfPerson_ID.Value);

            // Set Person Type ID
            p.PersonType_ID = personTypeID;

            // Get Person Adress
            addr1 = this.ucAddEditAddress.Address;
            addr2 = this.ucAddEditAddress1.Address;
            addr3 = this.ucAddEditAddress2.Address;

            // Add To List
            perSonAddr1.AddressType_ID = 2;
            perSonAddr1.Address = addr1;

            perSonAddr2.AddressType_ID = 3;
            perSonAddr2.Address = addr2;

            perSonAddr3.AddressType_ID = 4;
            perSonAddr3.Address = addr3;

            lstPersonAddr.Add(perSonAddr1);
            lstPersonAddr.Add(perSonAddr2);
            lstPersonAddr.Add(perSonAddr3);

            // Update Address
            int result = db.UpdateApplication_Person(p, pCard, pContact, lstPersonAddr, this.Page);

            return result;
        }

        /// <summary>
        /// Do Save Person
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

            // Get Person Adress
            addr1 = this.ucAddEditAddress.Address;
            addr2 = this.ucAddEditAddress1.Address;
            addr3 = this.ucAddEditAddress2.Address;

            // Add to List
            perSonAddr1.AddressType_ID = 2;
            perSonAddr1.Address = addr1;

            perSonAddr2.AddressType_ID = 3;
            perSonAddr2.Address = addr2;

            perSonAddr3.AddressType_ID = 4;
            perSonAddr3.Address = addr3;

            lstPersonAddr.Add(perSonAddr1);
            lstPersonAddr.Add(perSonAddr2);
            lstPersonAddr.Add(perSonAddr3);

            // Add Address
            int result = db.AddApplication_Person(p, p.PersonCard, p.PersonContact, lstPersonAddr, this.Page);

            return result;
        }

        /// <summary>
        /// Validate Person Detail Input
        /// </summary>
        /// <param name="personcardDetail"></param>
        /// <param name="passportcardDetail"></param>
        /// <returns></returns>
        private bool IsValidate(string personcardDetail, string passportcardDetail, string TaxpayerNoDetail)
        {
            DataCenterDB db = new DataCenterDB();
            DataTable dt = new DataTable();
            string address1;
            string address2;
            string address3;
            string customer;
            string corporate;

            if (personcardDetail != string.Empty || passportcardDetail != string.Empty || TaxpayerNoDetail != string.Empty)
            {
                if (personcardDetail != string.Empty)
                {
                    if (ucAddEditCustomer.IsValidateIdentityCard())
                    {
                        if (hdfPerson_ID.Value == string.Empty)
                        {
                            dt = db.GetPersonSearchByCardDetail(personcardDetail, 2, null);

                            if (dt.Rows.Count > 0)
                            {
                                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลขบัตรประจำตัวประชาชนมีอยู่ในระบบแล้ว!");
                                ucAddEditCustomer.SetAlertWarning(true);
                                ucAddEditCustomer.Focus();
                                return false;
                            }
                        }
                        ucAddEditCustomer.SetAlertWarning(false);
                    }
                    else
                    {
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบไม่ถูกต้อง กรุณาตรวจสอบหมายเลขบัตรประจำตัวประชาชน!");
                        ucAddEditCustomer.SetAlertWarning(true);
                        ucAddEditCustomer.Focus();
                        return false;
                    }
                }

                if (passportcardDetail != string.Empty)
                {
                    if (hdfPerson_ID.Value == string.Empty)
                    {
                        dt = db.GetPersonSearchByCardDetail(passportcardDetail, 3, null);

                        if (dt.Rows.Count > 0)
                        {
                            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลข Passport มีอยู่ในระบบแล้ว!");
                            ucAddEditCustomer.SetAlertWarning(true);
                            ucAddEditCustomer.Focus();
                            return false;
                        }
                    }
                    ucAddEditCustomer.SetAlertWarning(false);
                }

                if (TaxpayerNoDetail != string.Empty)
                {
                    if (hdfPerson_ID.Value == string.Empty)
                    {
                        dt = db.GetPersonSearchByCardDetail(TaxpayerNoDetail, 4, null);

                        if (dt.Rows.Count > 0)
                        {
                            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "เลขประจำตัวผู้เสียภาษี มีอยู่ในระบบแล้ว!");
                            ucAddEditCorporate.SetAlertWarning(true);
                            ucAddEditCorporate.Focus();
                            return false;
                        }
                    }
                    ucAddEditCorporate.SetAlertWarning(false);
                }

                //Validate Corporate
                //validate นิติบุคคล
                if (ddlPersonType1.PersonTypeID == 3) //นิติบุคคล
                {
                    //ValidateCustomer โดย class return ค่าเป็น string
                    corporate = ucAddEditCorporate.ValidateCorporate();

                    if (corporate != "")
                    {
                        //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, corporate);
                        return false;
                    }
                }

                //validate บุคคลธรรมดา
                if (ddlPersonType1.PersonTypeID == 2) //บุคคลธรรมดา
                {
                    customer = ucAddEditCustomer.ValidateCustomer();

                    if (customer != "")
                    {
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, customer);
                        return false;
                    }

                    if (ucAddEditCustomer.IsValidateBirthDate() == false)
                    {
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบวันเกิดไม่ถูกต้อง");
                        return false;
                    }
                }

                address1 = ucAddEditAddress.ValidateAddress();

                if (address1 != "")
                {
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address1);
                    return false;
                }

                address2 = ucAddEditAddress1.ValidateAddress();

                if (address2 != "")
                {
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address2);
                    return false;
                }

                address3 = ucAddEditAddress2.ValidateAddress();

                if (address3 != "")
                {
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, address3);
                    return false;
                }
            }
            else
            {
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาระบุหมายเลขบัตรประจำตัวประชาชน หรือ หมายเลข Passport");
                ucAddEditCustomer.SetAlertWarning(false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Do Clear
        /// </summary>
        private void DoClear()
        {
            // clear ค่าใน uc
            this.ucAddEditCustomer.DoClear();
            this.ucAddEditCorporate.DoClear();
            this.ucAddEditAddress.Doclear();
            this.ucAddEditAddress1.Doclear();
            this.ucAddEditAddress2.Doclear();

            // set default warning
            this.ucAddEditCustomer.SetAlertWarning(false);
            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
        }

        #endregion Method

        protected void ddlPersonType_eSelectChanged(object sender, EventArgs e)
        {
            int personTypeID = ddlPersonType1.PersonTypeID;

            if (personTypeID == 3) // 3 นิติบุคคล
            {
                this.ucAddEditCustomer.Visible = false;
                this.ucAddEditCorporate.Visible = true;

                //clear
                this.ucAddEditCustomer.DoClear();
                this.ucAddEditCorporate.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfPerson_ID.Value = "";
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditCorporate.DoLoadDropdownList(); //โหลด dropdownlist นิติ
            }
            else if (personTypeID == 2) // 2 บุคคลธรรมดา
            {
                this.ucAddEditCustomer.Visible = true;
                this.ucAddEditCorporate.Visible = false;

                //clear
                this.ucAddEditCorporate.DoClear();
                this.ucAddEditCustomer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfPerson_ID.Value = "";
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditCustomer.DoLoadDropdownList(); //โหลด dropdownlist บุคคลธรรมดา
            }
            else
            {
                this.ucAddEditCustomer.Visible = false;
                this.ucAddEditCorporate.Visible = false;
                //clear
                this.ucAddEditCustomer.DoClear();
                this.ucAddEditCorporate.DoClear();

                this.ucAddEditCustomer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                this.ucAddEditCustomer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
            }
        }

        private bool IsValidateEditPerson(string queryStringPersonid, string queryStringMotorAppId)
        {
            //เช็คข้อมูล person เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            if (queryStringPersonid != string.Empty)
            {
                int? motorappid = null;
                if (queryStringMotorAppId != string.Empty)
                {
                    motorappid = Convert.ToInt32(queryStringMotorAppId);
                }

                string resultMsg = ucAddEditCustomer.ValidateButtonEditPerson(Convert.ToInt32(queryStringPersonid), cFunction.GetCurrentLoginUser_ID(Page), motorappid);

                if (resultMsg != string.Empty)
                {
                    ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, resultMsg);
                    btnSave.Enabled = false;
                    ucAddEditCustomer.IsEnabled(false);
                    ucAddEditCorporate.IsEnabled(false);
                    ucAddEditAddress.IsEnabled(false);
                    ucAddEditAddress1.IsEnabled(false);
                    ucAddEditAddress2.IsEnabled(false);
                    return false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            return true;
        }
    }
}