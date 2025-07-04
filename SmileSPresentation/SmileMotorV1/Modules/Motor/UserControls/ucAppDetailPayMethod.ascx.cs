using SmileSMotorClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmileMotorV1.Modules.Motor.UserControls
{
    public partial class ucAppDetailPayMethod : System.Web.UI.UserControl
    {
        #region Deblare

        private int _bankId;
        private string _bankAccountDetail;

        public int BankID
        {
            get { return _bankId; }
            set { _bankId = value; }
        }

        public int PeriodTypeId { get; set; }

        public string BankAccountDetail
        {
            get { return _bankAccountDetail; }
            set { _bankAccountDetail = value; }
        }

        #endregion Deblare

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion Event

        #region Method

        /// <summary>
        /// Do Load Detail
        /// </summary>
        /// <param name="_motorApplicationID">Motor Application ID</param>
        public void DoLoad(int _motorApplicationID)
        {
            MotorDB db = new MotorDB();
            Premium pm = new Premium();

            // Get Motor Application Premium
            pm = db.GetMotorApplicationPremium(_motorApplicationID);

            // Set Bind Motor Premium Detail
            lblPaymentPeriodType.Text = pm.PayMethodTypeDetail;
            lblPeriod.Text = pm.PeriodTypeDetail;
            lblBank.Text = pm.BankDetail;
            lblAccountName.Text = pm.PersonBankAccountName;
            lblAccountNumber.Text = pm.PersonBankAccountNo;
            lblPremiumAmount.Text = pm.PremiumAmount.ToString("0,0.00");
            lblPremiumTaxAmount.Text = pm.PremiumTaxAmount.ToString("0,0.00");
            lblPremiumDeliverAmount.Text = pm.PremiumDeliverAmount.ToString("0,0.00");
            _bankId = pm.Bank_ID;
            _bankAccountDetail = pm.PersonBankAccountNo;
            PeriodTypeId = pm.PeriodType_ID;
        }

        /// <summary>
        ///GetMotorApplicationPeriodType_ID
        /// </summary>
        /// <returns></returns>
        public int GetMotorApplicationPeriodType_ID()
        {
            return PeriodTypeId;
        }

        #endregion Method
    }
}