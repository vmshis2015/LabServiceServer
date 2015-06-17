using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VietBaIT.CommonLibrary.Entities
{
    

    public class Schedule
    {
        private static string PageStatus = "Insert";
        private static string RegService = "Insert";
      
        private static decimal Payment = 0;
      
        public static string PageSchedule
        {
            get
            {
                return PageStatus;
            }
            set
            {
                PageStatus = value;
            }
        }
      
        public static string PageRegService
        {
            get
            {
                return RegService;
            }
            set
            {
                RegService = value;
            }
        }
        public static decimal PaymentExam
        {
            get
            {
                return Payment;
            }
            set
            {
                Payment = value;
            }
        }
    }
}
