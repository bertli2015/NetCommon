using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetCommon.Console.Common
{
    public class DateUtil
    {
        public static int GetWorkDays(DateTime startDate, DateTime endDate)
        {
            TimeSpan ts = endDate.Subtract(startDate);
            int countday = ts.Days;//获取两个日期间的总天数  
            int weekday = 0;//工作日  
            //循环用来扣除总天数中的双休日  
            for (int i = 0; i < countday + 1; i++)
            {
                DateTime tempdt = startDate.Date.AddDays(i);
                if (tempdt.DayOfWeek != DayOfWeek.Saturday && tempdt.DayOfWeek != DayOfWeek.Sunday)
                {
                    weekday++;
                }
            }
            return weekday;
        }
    }
}