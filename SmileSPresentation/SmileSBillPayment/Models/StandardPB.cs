using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSBillPayment.Models
{
    /// <summary>
    /// Thai/Standard Payment Barcode
    /// </summary>
    public class StandardPB
    {
        public static string GetPaymentBarcode(StandardPBDetail stdDetail)
        {
            //กรณีอนุญาตให้ผู้ชำระเงิน ชำระค่าใช้จ่ายบางส่วนได้ ให้ใช้เลขศูนย์ "0"  1 หลัก แทนการใส่จำนวนเต็ม
            var newAmount = "0";
            //ไม่ใช่ 0
            if (stdDetail.Amount != "0")
            {
                //มีทศนิยมหรือไม่
                if (stdDetail.Amount.Contains("."))
                {
                    //ตัดทศนิยมออก
                    var a = stdDetail.Amount.Replace(".", "");
                    //ต้องไม่เกิน 10 หลัก
                    if (a.Length <= 10)
                    {
                        newAmount = a;
                    }
                    else
                    {
                        //ตัดตัวเลขที่เกิน 10 หลัก ออก
                        newAmount = a.Remove(10);
                    };
                }
                else
                {
                    if (stdDetail.Amount.Length <= 10)
                    {
                        newAmount = stdDetail.Amount + "00";

                        if (newAmount.Length > 10) newAmount.Remove(10);
                    }
                }
            }
            var strPaymentBarcode = "";
            // ref2 == "-" คือ ไม่มี ref2
            if (stdDetail.ReferenceNo2 == "-")
            {
                strPaymentBarcode = String.Format("{0}{1}{2}\r{3}\r\r{4}", stdDetail.Prefix, stdDetail.TaxIDNo, stdDetail.Suffix, stdDetail.ReferenceNo1, newAmount);
            }
            else
            {
                strPaymentBarcode = String.Format("{0}{1}{2}\r{3}\r{4}\r{5}", stdDetail.Prefix, stdDetail.TaxIDNo, stdDetail.Suffix, stdDetail.ReferenceNo1, stdDetail.ReferenceNo2, newAmount);
            }

            return strPaymentBarcode;
        }
    }

    public class StandardPBDetail
    {
        public string Prefix { get; } = "|";
        public string TaxIDNo { get; set; }
        public string Suffix { get; set; }
        public string ReferenceNo1 { get; set; }
        public string ReferenceNo2 { get; set; }
        public string Amount { get; set; }
    }
}