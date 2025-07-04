using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSConvertFileV2
{
    public class TypeConvert
    {
        private DateTime date;
        public DateTime DateConvert(string strDate)
        {
            double d = double.Parse(strDate);
            date = DateTime.FromOADate(d);

            return date;
        }

        //public DateTime getDate
        //{
        //    get { return date; }
        //    //set; 
        //}
       
    }
}