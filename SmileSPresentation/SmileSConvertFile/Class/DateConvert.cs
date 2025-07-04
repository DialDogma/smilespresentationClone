using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterTest
{
    public class DateConvert
    {
        private DateTime date;

        public DateTime Convert(string strDate)
        {
            double d = double.Parse(strDate);
            date = DateTime.FromOADate(d);

            return date;
        }

        public DateTime getDate
        {
            get { return date; }
            //set; 
        }
    }
}