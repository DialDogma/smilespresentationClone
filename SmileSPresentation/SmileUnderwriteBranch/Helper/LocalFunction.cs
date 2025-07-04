using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileUnderwriteBranch.Helper
{
    public class LocalFunction
    {
        public LocalFunction()
        {
        }

        /// <summary>
        /// Get Period List
        /// </summary>
        /// <returns></returns>
        public static List<Period> GetPeriodList()
        {
            var result = new List<Period>();

            // วันที่เปลี่ยนงวด
            var changePeriodDay = Properties.Settings.Default.ChangePeriodDay;
            var numberOfMonthToShow = Properties.Settings.Default.NumberOfPeriodToShow;

            var today = DateTime.Today.Day;

            var defaultDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime addDay;

            //ถ้าวันที่มากกว่า  แสดงเดือนถัดไป
            if (today >= changePeriodDay)
            {
                var addMonths = defaultDay.AddMonths(1);

                addDay = addMonths;

                //Add result
                result.Add(new Period
                {
                    Value = addMonths,
                    Display = $"{addMonths:dd/MM/}" + (addMonths.Year + 543)
                });
            }
            else
            {
                //Add result
                result.Add(new Period
                {
                    Value = defaultDay,
                    Display = $"{defaultDay:dd/MM/}" + (defaultDay.Year + 543)
                });

                addDay = defaultDay;
            }

            //Get Period List ย้อนหลัง
            for (var i = 1; i < numberOfMonthToShow; i++)
            {
                addDay = addDay.AddMonths(-1);
                result.Add(new Period
                {
                    Value = addDay,
                    Display = $"{addDay:dd/MM/}" + (addDay.Year + 543)
                });
            }

            return result;
        }
    }

    public struct Period
    {
        public DateTime Value { get; set; }
        public string Display { get; set; }
    }
}