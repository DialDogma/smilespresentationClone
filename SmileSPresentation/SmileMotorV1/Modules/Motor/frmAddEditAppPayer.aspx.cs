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
    public partial class frmAddEditAppPayer : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FunctionManager fm = new FunctionManager();

                //โหลดข้อมูล dropdownlist & set autoPostback
                this.ddlRelation.DoLoadDropdownList();
                this.ddlRelation.AutoPostback = true;
                this.ucAddEditPayer.DoLoadDropdownList();
                this.ucAddEditAddress.DoLoadDropdownList();
                this.ucAddEditAddress1.DoLoadDropdownList();
                this.ucAddEditAddress2.DoLoadDropdownList();
                this.ucAddEditCorporate.DoLoadDropdownList();
                this.ucAddEditCorporate.TextHeader.Text = "ข้อมูลผู้ชำระเบี้ย/นิติบุคคล";
                this.ddlPersonType1.DoLoadDropdownList();

                //เช็ค query string cid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != string.Empty)
                {
                    //Decrypt querystring cid เก็บไว้ใน Hiddenfield
                    hdfCustomer_ID.Value = fm.Base64Decrypt(Request.QueryString["cid"]);

                    //load uc mini
                    this.ucPersonDetail_mini.DoLoad(Convert.ToInt32(hdfCustomer_ID.Value));
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

                    //ผู้เอาประกัน และ ผู้ชำระเบี้ย เป็นคนเดียวกัน
                    if (hdfCustomer_ID.Value == hdfPayer_ID.Value)
                    {
                        //set ความสัมพันธ์
                        this.ddlRelation.Relation_ID = 39; //39 คือ ความสัมพันธ์ "เป็นบุคคลเดียวกัน"

                        //เรียก Event
                        ddlRelation_eSelectedSamePerson(new object(), new EventArgs());
                    }
                    //ไม่ใช่คนเดียวกัน
                    else
                    {
                        //set id ผู้เอาประกัน & ผู้ชำระเบี้ย
                        int _payerid = Convert.ToInt32(hdfPayer_ID.Value);
                        int _personid = Convert.ToInt32(hdfCustomer_ID.Value);
                        DataCenterDB db = new DataCenterDB();

                        //set ข้อมูลความสัมพันธ์
                        this.ddlRelation.Relation_ID = db.GetPersonRelation(_payerid, _personid).PersonRelationType_ID;

                        int cardtype_id = db.GetPersonCardByPersonID(_payerid).Select(o => o.PersonCardType_ID).FirstOrDefault();

                        if (cardtype_id == 2 || cardtype_id == 3)
                        {
                            //โหลดข้อมูลจาก personid -- บุคคลธรรมดา
                            this.ucAddEditPayer.DoloadDetail(_payerid);
                            ucAddEditPayer.IsEnabled(false);
                            ucAddEditCorporate.Visible = false;
                            ucAddEditPayer.Visible = true;
                            ddlPersonType1.PersonTypeID = 2; //นิติ
                        }
                        else
                        {
                            //โหลดข้อมูลจาก personid -- นิติฯ
                            this.ucAddEditCorporate.DoLoad(_payerid);
                            ucAddEditCorporate.IsEnabled(false);
                            ucAddEditCorporate.Visible = true;
                            ucAddEditPayer.Visible = false;
                            ddlPersonType1.PersonTypeID = 3; //นิติ
                        }
                        //load
                        this.ucPersonDetail_mini.DoLoad(_personid);
                        DoloadAddressByPersonID(_payerid);

                        //set Enable = false
                        ucPersonSearchByIdentityCardNumber.IsEnabled(false);
                        ucAddEditAddress.IsEnabled(false);
                        ucAddEditAddress1.IsEnabled(false);
                        ucAddEditAddress2.IsEnabled(false);
                        IsEnabled(false);
                        this.ucPersonSearchByIdentityCardNumber.SetPersonTypeIDTextbox(ddlPersonType1.PersonTypeID.ToString()); //set ค่า persontype
                    }
                }
                //pid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    this.ddlRelation.Relation_ID = 39; //39 เป็นบุคคลเดียวกัน
                    ddlRelation_eSelectedSamePerson(new object(), new EventArgs());
                }

                //เช็ค query string appid ไม่เท่ากับ null และ ไม่เท่ากับค่าว่าง
                if (Request.QueryString["appid"] != null && Request.QueryString["appid"] != string.Empty)
                {
                    hdfApp_ID.Value = fm.Base64Decrypt(Request.QueryString["appid"]);
                }
                //appid เท่ากับ null && เท่ากับค่าว่าง
                else
                {
                    hdfApp_ID.Value = "";
                }
            }

            //เช็คข้อมูลผู้ชำระเบี้ย เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            IsValidateEditPerson(hdfPayer_ID.Value, hdfApp_ID.Value);

            //set ความกว้างให้กับ button
            btnCopyAddress.Width = Unit.Pixel(150);
            btnCopyAddress1.Width = Unit.Pixel(150);
        }

        protected void btnForward_Click(object sender, EventArgs e)
        {
            //declear&set card detail & passport
            string newIdentityCardID = "";
            string newPassportID = "";
            string newTaxpayerNo = "";

            int personType = ddlPersonType1.PersonTypeID; //ประเภท
                                                          //Check value จาก ddl ประเภทบุคคล/นิติบุคคล
            if (personType == (int)MappingCode.ePersonType.Individual)
            {
                //ถ้าเป็น บุคคลธรรมดา
                //declear&set card detail & passport
                newIdentityCardID = ucAddEditPayer.GetCardDetail();
                newPassportID = ucAddEditPayer.GetCardPassport();
            }
            else if (personType == (int)MappingCode.ePersonType.Corporate)
            {
                newTaxpayerNo = ucAddEditCorporate.GetTaxpayerNo();
            }

            string encryptCustomerID;
            string encryptPayerID;
            string encryptAppID;

            int payerID;

            //validate ให้ระบุความสัมพันธ์
            if (ddlRelation.ValidateRelation() == "")
            {
                //เช็ค hdfPayer_ID ไม่เท่ากับ string.emtpy
                if (hdfPayer_ID.Value == string.Empty)
                {
                    //validate ข้อมูลผู้ชำระเบี้ย
                    if (IsValidate(newIdentityCardID, newPassportID, newTaxpayerNo) == true) //ส่ง parameter newเลขประจำตัวผู้เสียภาษี ไปอีก 1 ตัว
                    {
                        //บันทึกข้อมูล return payerID กลับมาเก็ฐไว้ใน payerID
                        payerID = DoSave();

                        //SaveRelation
                        DoSaveRelation(payerID);

                        //set id to hdfPayer_ID
                        hdfPayer_ID.Value = payerID.ToString();

                        //เข้ารหัส id
                        encryptCustomerID = cFunction.Base64Encrypt(hdfCustomer_ID.Value);
                        encryptPayerID = cFunction.Base64Encrypt(hdfPayer_ID.Value);
                        encryptAppID = cFunction.Base64Encrypt(hdfApp_ID.Value);

                        //redirect ไปหน้า frmAddEditAppProduct
                        Response.Redirect("~/Modules/Motor/frmAddEditAppProduct.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
                    }
                }

                //ถ้าไม่
                else
                {
                    //SaveRelation
                    DoSaveRelation(Convert.ToInt32(hdfPayer_ID.Value));

                    //ถ้า App Id ไม่เท่ากับค่าว่าง
                    if (hdfApp_ID.Value != "")
                    {
                        var personRelatedTypeId = 3; // 3 คือ Type ผู้ชำระเบี้ย
                        var personId = Convert.ToInt32(hdfPayer_ID.Value);
                        var motorId = Convert.ToInt32(hdfApp_ID.Value);
                        var userId = cFunction.GetCurrentLoginUser_ID(Page);
                        DoUpdatePayer(personRelatedTypeId, personId, motorId, userId);
                    }

                    //เข้ารหัส id
                    encryptCustomerID = cFunction.Base64Encrypt(hdfCustomer_ID.Value);
                    encryptPayerID = cFunction.Base64Encrypt(hdfPayer_ID.Value);
                    encryptAppID = cFunction.Base64Encrypt(hdfApp_ID.Value);

                    //redirect ไปหน้า frmAddEditAppProduct
                    Response.Redirect("~/Modules/Motor/frmAddEditAppProduct.aspx?" + "cid=" + encryptCustomerID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
                }
            }
            //ไม่ได้เลือกความสันพันธ์
            else
            {
                //alert
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "โปรดระบุความสัมพันธ์");
            }
        }

        /// <summary>
        /// ย้อนกลับ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBackward_Click(object sender, EventArgs e)
        {
            FunctionManager fm = new FunctionManager();

            //Encrypt id
            string encryptPersonID = fm.Base64Encrypt(hdfCustomer_ID.Value);
            string encryptPayerID = fm.Base64Encrypt(hdfPayer_ID.Value);
            string encryptAppID = fm.Base64Encrypt(hdfApp_ID.Value);

            //Redirect
            Response.Redirect("~/Modules/Motor/frmAddEditAppCustomer.aspx?" + "cid=" + encryptPersonID + "&pid=" + encryptPayerID + "&appid=" + encryptAppID);
        }

        /// <summary>
        /// Event เลือกความสัมพันธ์ไม่ใช่คนเดียวกัน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRelation_eSelectedNotSamePerson(object sender, EventArgs e)
        {
            //set relation_id and payer_id
            hdfRelation_ID.Value = ddlRelation.Relation_ID.ToString();
            hdfPayer_ID.Value = "";

            // Clear uc
            ucAddEditPayer.DoClear();
            ucAddEditCorporate.DoClear();
            ucAddEditAddress.Doclear();
            ucAddEditAddress1.Doclear();
            ucAddEditAddress2.Doclear();

            //enable =true
            ucPersonSearchByIdentityCardNumber.IsEnabled(true);
            ucAddEditPayer.IsEnabled(true);
            ucAddEditCorporate.IsEnabled(true);
            ucAddEditAddress.IsEnabled(true);
            ucAddEditAddress1.IsEnabled(true);
            ucAddEditAddress2.IsEnabled(true);
            IsEnabled(true);

            //set default warning
            this.ucAddEditPayer.SetAlertWarning(false);
            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
            ddlPersonType1.IsEnabled(true);
            ucAddEditCorporate.SetAlertWarning(false);
            this.ucPersonSearchByIdentityCardNumber.SetPersonTypeIDTextbox(ddlPersonType1.PersonTypeID.ToString()); //set ค่า persontype
        }

        /// <summary>
        /// Event เลือกความสัมพันธ์เป็นบุคคลเดียกวัน
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRelation_eSelectedSamePerson(object sender, EventArgs e)
        {
            DataCenterDB dbc = new DataCenterDB();
            //set id
            int payerID;
            payerID = Convert.ToInt32(hdfCustomer_ID.Value);
            hdfPayer_ID.Value = hdfCustomer_ID.Value;

            hdfRelation_ID.Value = ddlRelation.Relation_ID.ToString();

            //เช็คข้อมูลผู้ชำระเบี้ย เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            IsValidateEditPerson(hdfPayer_ID.Value, hdfApp_ID.Value);

            //get card type
            int cardtype_id = dbc.GetPersonCardByPersonID(payerID).Select(o => o.PersonCardType_ID).FirstOrDefault();

            if (cardtype_id == 2 || cardtype_id == 3)
            {
                //โหลดข้อมูลจาก personid -- บุคคลธรรมดา
                //enable = false
                ucPersonSearchByIdentityCardNumber.IsEnabled(false);
                ucAddEditPayer.IsEnabled(false);
                ucAddEditAddress.IsEnabled(false);
                ucAddEditAddress1.IsEnabled(false);
                ucAddEditAddress2.IsEnabled(false);
                IsEnabled(false);

                //load detail
                ucAddEditPayer.DoloadDetail(payerID);
                DoloadAddressByPersonID(payerID);
                ucAddEditCorporate.Visible = false;
                ucAddEditPayer.Visible = true;
                ddlPersonType1.PersonTypeID = 2; //นิติ
            }
            else
            {
                //โหลดข้อมูลจาก personid -- นิติฯ
                ucPersonSearchByIdentityCardNumber.IsEnabled(false);
                ucAddEditCorporate.IsEnabled(false);
                ucAddEditAddress.IsEnabled(false);
                ucAddEditAddress1.IsEnabled(false);
                ucAddEditAddress2.IsEnabled(false);
                IsEnabled(false);

                //load detail
                ucAddEditCorporate.DoLoad(payerID);
                DoloadAddressByPersonID(payerID);
                ucAddEditCorporate.Visible = true;
                ucAddEditPayer.Visible = false;
                ddlPersonType1.PersonTypeID = 3; //นิติ
            }

            ddlPersonType1.IsEnabled(false);
            ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");
            ucAddEditPayer.SetAlertWarning(false);
            ucAddEditCorporate.SetAlertWarning(false);
        }

        /// <summary>
        /// Event เคลียร์
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucPersonSearchByIdentityCardNumber_eClickClear(object sender, EventArgs e)
        {
            // Clear uc
            hdfPayer_ID.Value = "";
            ucAddEditPayer.DoClear();
            ucAddEditCorporate.DoClear();
            ucAddEditAddress.Doclear();
            ucAddEditAddress1.Doclear();
            ucAddEditAddress2.Doclear();
            //set default warning
            this.ucAddEditPayer.SetAlertWarning(false);
            this.ucAddEditCorporate.SetAlertWarning(false);
            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.clear, "");

            this.ddlPersonType1.PersonTypeID = -1;
            this.ucAddEditPayer.Visible = false;
            this.ucAddEditCorporate.Visible = false;
        }

        /// <summary>
        /// Event Person Search Response Person ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucPersonSearchByIdentityCardNumber_eSelectedPersonIDChanged(object sender, EventArgs e)
        {
            DataCenterDB dbc = new DataCenterDB();
            hdfPayer_ID.Value = ucPersonSearchByIdentityCardNumber.Person_ID.ToString();
            int payerID = Convert.ToInt32(hdfPayer_ID.Value);

            //เช็คข้อมูลผู้ชำระเบี้ย เพื่อแก้ไขข้อมูลลูกค้า/ผู้ชำระเบี้ย
            IsValidateEditPerson(hdfPayer_ID.Value, hdfApp_ID.Value);

            //get card type
            int cardtype_id = dbc.GetPersonCardByPersonID(payerID).Select(o => o.PersonCardType_ID).FirstOrDefault();

            if (cardtype_id == 2 || cardtype_id == 3)
            {
                // Set Enabled uc And Doload Detail --บุคคลธรรมดา
                ucPersonSearchByIdentityCardNumber.IsEnabled(false);
                ucAddEditPayer.IsEnabled(false);
                ucAddEditPayer.DoloadDetail(payerID);

                //set default warning
                this.ucAddEditPayer.SetAlertWarning(false);
            }
            else
            {
                // Set Enabled uc And Doload Detail --นิติฯ
                ucPersonSearchByIdentityCardNumber.IsEnabled(false);
                ucAddEditCorporate.IsEnabled(false);
                ucAddEditCorporate.DoLoad(payerID);

                //set default warning
                this.ucAddEditCorporate.SetAlertWarning(false);
            }

            ucAddEditAddress.IsEnabled(false);
            ucAddEditAddress1.IsEnabled(false);
            ucAddEditAddress2.IsEnabled(false);
            DoloadAddressByPersonID(payerID);
        }

        /// <summary>
        /// คัดลอกที่อยู่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopyAddress_Click(object sender, EventArgs e)
        {
            if (this.ucAddEditAddress.Address.ZipCode != "")
            {
                //copy
                this.ucAddEditAddress1.Copyform(ucAddEditAddress);
            }
            else
            {
                //alert
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ที่อยู่ปัจจุบันยังไม่สมบูรณ์");
            }
        }

        /// <summary>
        /// คัดลอกที่อยู่
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCopyAddress1_Click(object sender, EventArgs e)
        {
            if (this.ucAddEditAddress.Address.ZipCode != "")
            {
                //copy
                ucAddEditAddress2.Copyform(ucAddEditAddress);
            }
            else
            {
                //alert
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "ที่อยู่ปัจจุบันยังไม่สมบูรณ์");
            }
        }

        protected void ddlPersonType_eSelectChanged(object sender, EventArgs e)
        {
            int personTypeID = ddlPersonType1.PersonTypeID;

            if (personTypeID == 3) // 3 นิติบุคคล
            {
                this.ucAddEditPayer.Visible = false;
                this.ucAddEditCorporate.Visible = true;

                //clear
                this.ucAddEditPayer.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditPayer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfPayer_ID.Value = "";
                this.ucAddEditPayer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditAddress.IsEnabled(true);
                this.ucAddEditAddress1.IsEnabled(true);
                this.ucAddEditAddress2.IsEnabled(true);
                this.ucAddEditCorporate.DoLoadDropdownList(); //โหลด dropdownlist นิติ
                btnCopyAddress.Enabled = true;
                btnCopyAddress1.Enabled = true;
            }
            else if (personTypeID == 2) // 2 บุคคลธรรมดา
            {
                this.ucAddEditPayer.Visible = true;
                this.ucAddEditCorporate.Visible = false;

                //clear
                this.ucAddEditCorporate.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditPayer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfPayer_ID.Value = "";
                this.ucAddEditPayer.IsEnabled(true);
                this.ucAddEditCorporate.IsEnabled(true);
                this.ucAddEditAddress.IsEnabled(true);
                this.ucAddEditAddress1.IsEnabled(true);
                this.ucAddEditAddress2.IsEnabled(true);
                this.ucAddEditPayer.DoLoadDropdownList(); //โหลด dropdownlist บุคคลธรรมดา
                btnCopyAddress.Enabled = true;
                btnCopyAddress1.Enabled = true;
            }
            else
            {
                this.ucAddEditPayer.Visible = false;
                this.ucAddEditCorporate.Visible = false;
                //clear
                this.ucAddEditPayer.DoClear();
                this.ucAddEditCorporate.DoClear();
                this.ucPersonSearchByIdentityCardNumber.DoClearTextBoxSearch();

                this.ucAddEditPayer.DoClear();
                this.ucAddEditAddress.Doclear();
                this.ucAddEditAddress1.Doclear();
                this.ucAddEditAddress2.Doclear();
                hdfPayer_ID.Value = "";
                this.ucAddEditPayer.IsEnabled(true);
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
        /// Do Save Payer Detail / Relation Payer
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
            //PersonRelation pr = new PersonRelation();
            TransactionManager tm = new TransactionManager();
            int payerID;
            int personTypeID = ddlPersonType1.PersonTypeID;

            switch (personTypeID)
            {
                case (int)MappingCode.ePersonType.Individual: // 2 คือบุคคลธรรมดา
                    p = this.ucAddEditPayer.Person;
                    break;

                case (int)MappingCode.ePersonType.Corporate: // 3 นิติบุคคล
                    p = this.ucAddEditCorporate.Person;
                    break;

                default:
                    break;
            }

            // 2 คือบุคคลธรรมดา 3 นิติ
            p.PersonType_ID = personTypeID;

            addr1 = this.ucAddEditAddress.Address;
            addr2 = this.ucAddEditAddress1.Address;
            addr3 = this.ucAddEditAddress2.Address;

            //Add to List
            perSonAddr1.AddressType_ID = 2; //ที่อยู่ปัจจุบัน
            perSonAddr1.Address = addr1;

            perSonAddr2.AddressType_ID = 3; //ที่อยู่ตามบัตรประชาชน
            perSonAddr2.Address = addr2;

            perSonAddr3.AddressType_ID = 4; //ที่อยู่ที่ทำงาน
            perSonAddr3.Address = addr3;

            lstPersonAddr.Add(perSonAddr1);
            lstPersonAddr.Add(perSonAddr2);
            lstPersonAddr.Add(perSonAddr3);

            //save and return id
            payerID = db.AddApplication_Person(p, p.PersonCard, p.PersonContact, lstPersonAddr, this.Page);

            return payerID;
        }

        /// <summary>
        /// Save Relation ความสัมพันธ์
        /// </summary>
        /// <param name="payerid">id ผู้ชำระเบี้ย</param>
        /// <returns></returns>
        private int DoSaveRelation(int payerid)
        {
            DataCenterDB db = new DataCenterDB();
            PersonRelation pr = new PersonRelation();
            int personRelationID;

            //set id
            pr.PersonRelation_ID = db.GetPersonRelation(payerid, Convert.ToInt32(hdfCustomer_ID.Value)).PersonRelation_ID;
            pr.PersonRelationType_ID = this.ddlRelation.Relation_ID;
            pr.Person_ID = payerid;
            pr.RelationWithPerson_ID = Convert.ToInt32(hdfCustomer_ID.Value);

            //Chk if not have Add else have Update
            if (pr.PersonRelation_ID == 0) //(tm.IsExist_AddRelation(pr) == true)
            {
                personRelationID = db.AddPersonRelation(pr, cFunction.GetCurrentLoginUser_ID(this.Page));
            }
            else
            {
                personRelationID = db.UpdatePersonRelation(pr, cFunction.GetCurrentLoginUser_ID(this.Page));
            }

            return personRelationID;
        }

        /// <summary>
        /// Do Load Addrees To UC Adrress By Person ID
        /// </summary>
        /// <param name="_person_ID">รหัส Person</param>
        private void DoloadAddressByPersonID(int _person_ID)
        {
            DataCenterDB db = new DataCenterDB();

            int _addressType_1;
            int _addressType_2;
            int _addressType_3;

            // Get Address By Person ID And Address Type
            _addressType_1 = db.GetPersonAddressByPersonID(_person_ID, 2).FirstOrDefault().Address_ID;
            _addressType_2 = db.GetPersonAddressByPersonID(_person_ID, 4).FirstOrDefault().Address_ID;
            _addressType_3 = db.GetPersonAddressByPersonID(_person_ID, 3).FirstOrDefault().Address_ID;

            // Load Address By UC
            ucAddEditAddress.DoLoadByAddressID(_addressType_1);
            ucAddEditAddress1.DoLoadByAddressID(_addressType_2);
            ucAddEditAddress2.DoLoadByAddressID(_addressType_3);
        }

        /// <summary>
        /// Do Search Relation Type หาความสัมพันธ์
        /// </summary>
        /// <returns></returns>
        private int DoSearchRelationType()
        {
            PersonRelation result;
            DataCenterDB db = new DataCenterDB();

            //set customer_ID and payer_ID
            int _customer_ID = Convert.ToInt32(hdfCustomer_ID.Value);
            int _payer_ID = Convert.ToInt32(hdfPayer_ID.Value);

            try
            {
                //get relation
                result = db.GetPersonRelation(_payer_ID, _customer_ID);
            }
            catch
            {
                result = new PersonRelation();
                result.PersonRelationType_ID = -1;
            }

            return result.PersonRelationType_ID;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="personRelateAppTypeId">ประเภท 2ผู้เอาประกัน/3ผู้ชำระเบี้ย</param>
        /// <param name="personId"></param>
        /// <param name="motorId"></param>
        /// <param name="updateById"></param>
        private void DoUpdatePayer(int personRelateAppTypeId, int personId, int motorId, int updateById)
        {
            //UPDATE PAYER
            using (var db = new MotorV1Entities())
            {
                var st = db.usp_MotorApplication_SelectStatusByID(motorId).First();

                if (st.MotorApplicationStatus_ID == 7 || st.MotorApplicationStatus_ID == 6) //Status บันทึกลูกค้าใหม่ และ 6 รอแก้ไข พี่แจ็คบอกนะ 7-9-2561
                {
                    db.usp_MotorApplicationPersonRelatedApplication_Update(motorId, personId, personRelateAppTypeId, updateById);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v">value true or false</param>
        private void IsEnabled(bool v)
        {
            btnCopyAddress.Enabled = v;
            btnCopyAddress1.Enabled = v;
        }

        /// <summary>
        /// Validate Payer
        /// </summary>
        /// <param name="personcardDetail">เลขบัตรประจำตัวประชาชน</param>
        /// <param name="passportcardDetail">passport id</param>
        /// <returns></returns>
        private bool IsValidate(string personcardDetail, string passportcardDetail, string newTaxpayerNo)
        {
            DataCenterDB db = new DataCenterDB();
            DataTable dt = new DataTable();
            string addr1;
            string addr2;
            string addr3;
            string payer;
            string Relation;
            string corporate;

            //เช็คเลขบัตรประชาชน or เลข passport ไม่เท่ากับ string.empty
            if (personcardDetail != string.Empty || passportcardDetail != string.Empty || newTaxpayerNo != string.Empty) //เพิ่ม validate หมายเลขประจำตัวผู้เสียภาษี
            {
                //เช็คเลขบตร ปชช ไม่เท่ากับ string.empty
                if (personcardDetail != string.Empty)
                {
                    //เช็ครูปแบบเลขบัตร ปชช ถ้าถูกต้อง
                    if (ucAddEditPayer.IsValidateIdentityCard())
                    {
                        //ค้นหาเลขบัตรว่ามีอยู่ในระบบแล้วหรือยัง?
                        dt = db.GetPersonSearchByCardDetail(personcardDetail, 2, null); //2 คือ type บัตรประชาชน

                        //ถ้ามีแล้ว
                        if (dt.Rows.Count > 0)
                        {
                            //แจ้งเตือน
                            this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลขบัตรประจำตัวประชาชนมีอยู่ในระบบแล้ว!");
                            ucAddEditPayer.SetAlertWarning(true);
                            ucAddEditPayer.Focus();
                            return false;
                        }
                        //ไม่มี
                        ucAddEditPayer.SetAlertWarning(false);
                    }
                    //รูปแบบไม่ถูกต้อง
                    else
                    {
                        //แจ้งเตือน
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบไม่ถูกต้อง กรุณาตรวจสอบหมายเลขบัตรประจำตัวประชาชน!");
                        ucAddEditPayer.SetAlertWarning(true);
                        ucAddEditPayer.Focus();
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
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลข Passport มีอยู่ในระบบแล้ว!");
                        ucAddEditPayer.SetAlertWarning(true);
                        ucAddEditPayer.Focus();
                        return false;
                    }

                    //ไม่มี
                    ucAddEditPayer.SetAlertWarning(false);
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
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "หมายเลขผู้เสียภาษี มีอยู่ในระบบแล้ว!");
                        ucAddEditCorporate.SetAlertWarning(true);
                        ucAddEditCorporate.Focus();
                        return false;
                    }
                    //ไม่มี
                    ucAddEditCorporate.SetAlertWarning(false);
                }

                //validate ความสัมพันธ์
                Relation = ddlRelation.ValidateRelation();

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
                    //ValidatePayer โดย class return ค่าเป็น string
                    payer = ucAddEditPayer.ValidatePayer();

                    //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                    if (payer != "")
                    {
                        //แจ้งเตือน
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, payer);
                        return false;
                    }

                    //บันทึกข้อมูลบุคคลใหม่ ต้องระบุวันเกิด ส่ง IsRequire = true
                    //validate รูปแบบ วันเกิด
                    if (ucAddEditPayer.IsValidateBirthDate(true) == false)
                    {
                        //รูปแบบวันที่ไม่ถูกต้องให้แจ้งเตือน
                        this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "รูปแบบวันเกิดไม่ถูกต้อง");
                        return false;
                    }
                }

                //ความสัมพันธ์ ไม่เท่ากับ string.empty
                if (Relation != "")
                {
                    //แจ้งเตือน
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, Relation);
                    return false;
                }

                //Validate ที่อยู่ โดย class return ค่าเป็น string
                addr1 = ucAddEditAddress.ValidateAddress();
                addr2 = ucAddEditAddress1.ValidateAddress();
                addr3 = ucAddEditAddress2.ValidateAddress();

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (addr1 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, addr1);
                    return false;
                }

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (addr2 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, addr2);
                    return false;
                }

                //ถ้า return ค่าออกมาไม่ท่ากับ string.empty
                if (addr3 != "")
                {
                    //ให้แสดงข้อวามแจ้งเตือนตามค่าที่ return
                    this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, addr3);
                    return false;
                }
            }

            //เลขบัตร ปชช && passport เท่ากับว่าง
            else
            {
                //แจ้งเตือน
                this.ucAlert.SetMessage(CommonUserControls.ucAlert.AlertStatus.danger, "กรุณาระบุหมายเลขบัตรประจำตัวประชาชน หรือ หมายเลข Passport");
                ucAddEditPayer.SetAlertWarning(false);
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

                ucAddEditPayer.ValidateButtonEditPerson(Convert.ToInt32(queryStringPersonid), cFunction.GetCurrentLoginUser_ID(Page), motorappid);
            }
        }

        #endregion Method
    }
}