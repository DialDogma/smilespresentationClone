using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using SmileSMotorClassLibrary;
using SmileMotorV1.Models;

namespace SmileMotorV1.Modules.Motor
{
    public partial class frmMotorApplicationMonitor : System.Web.UI.Page
    {
        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTitle();
                //check role = MotorDeveloper OR MotorUnderwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //check querystring st (st= notice Status) not null
                    if (Request.QueryString["st"] != null) //status
                    {
                        //get value from query string
                        int statusId = Convert.ToInt32(Request.QueryString["st"]);
                        //load Gridview from statusId
                        LoadGridview(null, statusId); //สถานะแก้ไขรอการอนุมัติ
                    }
                    //check querystring n (n= NoticeEndCover) not null
                    else if (Request.QueryString["n"] != null)//notice
                    {
                        //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                        LoadGridviewNoticeEndCover();
                    }
                    //check querystring np (np= Notice Policy) not null
                    else if (Request.QueryString["np"] != null)//np=notice policy
                    {
                        //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                        LoadGridviewDocumentPolicy();
                    }
                    //check querystring nc (nc= Notice Cashreceive) not null
                    else if (Request.QueryString["nc"] != null)//nc=notice cashreceive
                    {
                        var value = Convert.ToInt32(Request.QueryString["nc"]);
                        //check value querystring = 0
                        switch (value)
                        {
                            case 0:
                                //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                                LoadGridviewNewAppCashReceive();
                                break;

                            case 1:
                                //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                                LoadGridviewCashReceiveSuccess();
                                break;
                        }
                    }
                    //check querystring rv (rv= Notice Reverse) not null
                    else if (Request.QueryString["rv"] != null)//nc=notice Reverse
                    {
                        //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                        LoadGridviewAppWaitReverse();
                    }
                }

                //check role = MotorUser
                if (HttpContext.Current.User.IsInRole("MotorUser"))
                {
                    //check querystring st (st= status) not null
                    if (Request.QueryString["st"] != null)//status
                    {
                        //Load Gridview ตามสาขา และโหลดเฉพาะสถานะ 6 (รอแก้ไข)
                        LoadGridview(cFunction.GetCurrentBranchID(Page), 6); //สถานะรอแก้ไข
                    }
                    //check querystring n (n= NoticeEndCover) not null
                    else if (Request.QueryString["n"] != null)//notice
                    {
                        //load gridview ตามสาขา
                        LoadGridviewNoticeEndCover(cFunction.GetCurrentBranchID(Page));
                    }
                    //check querystring nย (nย= Notice Policy) not null
                    else if (Request.QueryString["np"] != null)//np=notice policy
                    {
                        //load gridview ตามสาขา
                        LoadGridviewDocumentPolicy(cFunction.GetCurrentBranchID(Page));
                    }
                    //check querystring nc (nc= Notice Cashreceive) not null
                    else if (Request.QueryString["nc"] != null)//nc=notice cashreceive
                    {
                        var value = Convert.ToInt32(Request.QueryString["nc"]);
                        //check value querystring = 0
                        switch (value)
                        {
                            case 0:
                                //load gridview ตามสาขา
                                LoadGridviewNewAppCashReceive(cFunction.GetCurrentBranchID(Page));
                                break;

                            case 1:
                                //load gridview ตามสาขา
                                LoadGridviewCashReceiveSuccess(cFunction.GetCurrentBranchID(Page));
                                break;
                        }
                    }
                }
            }
        }

        protected void dgvMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvMain.PageIndex = e.NewPageIndex;

            //check role = Developer OR Underwrite
            if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
            {
                //check querystring st (st= Notice Status) not null
                if (Request.QueryString["st"] != null)//st= status
                {
                    //Get value from query string
                    int statusId = Convert.ToInt32(Request.QueryString["st"]);
                    //load from status
                    LoadGridview(null, statusId); //สถานะแก้ไขรอการอนุมัติ
                }
                //check querystring n (nc= Notice EndCover) not null
                else if (Request.QueryString["n"] != null)//n= notice
                {
                    //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                    LoadGridviewNoticeEndCover();
                }
                //check querystring np (np= Notice Policy) not null
                else if (Request.QueryString["np"] != null)//np=notice policy
                {
                    //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                    LoadGridviewDocumentPolicy();
                }
                //check querystring nc (nc= Notice Cashreceive) not null
                else if (Request.QueryString["nc"] != null)//nc=notice cashreceive
                {
                    var value = Convert.ToInt32(Request.QueryString["nc"]);
                    //check value querystring = 0
                    switch (value)
                    {
                        case 0:
                            //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                            LoadGridviewNewAppCashReceive();
                            break;

                        case 1:
                            //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                            LoadGridviewCashReceiveSuccess();
                            break;
                    }
                }
                //check querystring rv (rv= Notice Reverse) not null
                else if (Request.QueryString["rv"] != null)//nc=notice Reverse
                {
                    //DEV AND MOTOR UNDERWRITE NOT SEND ARGUMENT BRANCH_ID
                    LoadGridviewAppWaitReverse();
                }
            }
            //check role = User
            if (HttpContext.Current.User.IsInRole("MotorUser"))
            {
                //check querystring st (st= Notice Status) not null
                if (Request.QueryString["st"] != null) //st= status
                {
                    //load ตามสาขา สถานะ 6 รอแก้ไข
                    LoadGridview(cFunction.GetCurrentBranchID(Page), 6); //สถานะรอแก้ไข
                }
                //check querystring n (n= Notice EndCover) not null
                else if (Request.QueryString["n"] != null) //n= notice
                {
                    //load ตามสาขา
                    LoadGridviewNoticeEndCover(cFunction.GetCurrentBranchID(Page));
                }
                //check querystring np (np= Notice Policy) not null
                else if (Request.QueryString["np"] != null)//np=notice policy
                {
                    //load ตามสาขา
                    LoadGridviewDocumentPolicy(cFunction.GetCurrentBranchID(Page));
                }
                //check querystring nc (nc= Notice Cashreceive) not null
                else if (Request.QueryString["nc"] != null)//nc=notice cashreceive
                {
                    var value = Convert.ToInt32(Request.QueryString["nc"]);
                    //check value querystring = 0
                    switch (value)
                    {
                        case 0:
                            //load gridview ตามสาขา
                            LoadGridviewNewAppCashReceive(cFunction.GetCurrentBranchID(Page));
                            break;

                        case 1:
                            //load gridview ตามสาขา
                            LoadGridviewCashReceiveSuccess(cFunction.GetCurrentBranchID(Page));
                            break;
                    }
                }
            }
        }

        protected void dgvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = dgvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string EncryptID = cFunction.Base64Encrypt(id);

                //check role Developer OR Underwrite
                if (HttpContext.Current.User.IsInRole("MotorDeveloper") || HttpContext.Current.User.IsInRole("MotorUnderwrite"))
                {
                    //check query string st (notice status) not null
                    if (Request.QueryString["st"] != null)//status
                    {
                        //add attributes  open frmApproveNewApp
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmApproveNewApp.aspx?" + "motorid=" + EncryptID + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                    //check query string n OR nc not null
                    else if (Request.QueryString["n"] != null || Request.QueryString["nc"] != null) //n=notice endcover ,nc=notice cashreceive
                    {
                        //add attributes open frmAppdetail
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + EncryptID + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                    //check query string np not null
                    else if (Request.QueryString["np"] != null) //n=notice ,np=notice policy
                    {
                        //add attributes open frmAppdetail and add query string [readed]
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + EncryptID + "&readed=1" + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                    //check querystring rv (rv= Notice Reverse) not null
                    else if (Request.QueryString["rv"] != null)//nc=notice Reverse
                    {
                        //add attributes open frmApproveNewApp
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmApproveNewApp.aspx?" + "motorid=" + EncryptID + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                }
                //check role = User
                if (HttpContext.Current.User.IsInRole("MotorUser"))
                {
                    //check query string np(np=notice policy) not null
                    if (Request.QueryString["np"] != null)
                    {
                        //add attributes open frmAppdetail and add query string [readed]
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + EncryptID + "&readed=1" + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                    else
                    {
                        //add attributes open frmAppdetail
                        e.Row.Attributes.Add("onclick", "window.open('" + "frmAppDetail.aspx?" + "code=" + EncryptID + "')");
                        e.Row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                    }
                }
            }
        }

        protected void dgvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in dgvMain.Rows)
            {
                if (row.RowIndex == dgvMain.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "คลิกเพื่อเปิดดูรายละเอียด";
                }
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
        }

        #endregion Event

        #region Method

        public void LoadTitle()
        {
            if (Request.QueryString["n"] == "0")
            {
                lbTitle.Text = "Appใกล้หมดอายุ";
            }
            if (Request.QueryString["np"] == "0")
            {
                lbTitle.Text = "เอกสารกรรมธรรม์";
            }
            if (Request.QueryString["rv"] == "0")
            {
                lbTitle.Text = "รอคืนสภาพ";
            }
            switch (Request.QueryString["nc"])
            {
                case "0":
                    lbTitle.Text = "รอชำระเบี้ย";
                    break;

                case "1":
                    lbTitle.Text = "ชำระเบี้ย";
                    break;
            }
            switch (Request.QueryString["st"])
            {
                case "4":
                    lbTitle.Text = "รอการอนุมัติ";
                    break;

                case "8":
                    lbTitle.Text = "แก้ไขรอการอนุมัติ";
                    break;
            }
        }

        public void LoadGridview(int? branchID, int statusId)
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Monitor_Select(branchID, statusId).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        public void LoadGridviewNoticeEndCover(int? branchID = null)
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Report_EndCoverNotice_Select(2, null, 7, branchID, 60).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        public void LoadGridviewDocumentPolicy(int? branchID = null)
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Report_DocumentNotice_Select(branchID).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        public void LoadGridviewNewAppCashReceive(int? branchID = null)
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Report_NewAppNotice_Select(branchID).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        public void LoadGridviewCashReceiveSuccess(int? branchID = null)
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Report_NewAppMatchedNotice_Select(branchID).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        public void LoadGridviewAppWaitReverse()
        {
            var db = new MotorV1Entities();
            var fm = new SmileSMotorClassLibrary.FunctionManager();

            var lst = db.usp_MotorApplication_Report_StatusNotice_Select(null, null, 10).ToList();

            fm.LoadGridview(dgvMain, fm.ToDataTable(lst), "ApplicationID");
        }

        #endregion Method
    }
}