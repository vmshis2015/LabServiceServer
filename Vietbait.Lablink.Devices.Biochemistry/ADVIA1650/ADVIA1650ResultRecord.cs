using System;
using System.Collections;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class ADVIA1650ResultRecord
    {
        public string Barcode;
        public string DateTestCompleted;
        public char RecordDelimiter = DeviceHelper.CR;

        public ADVIA1650ResultRecord()
        {
        }

        public ADVIA1650ResultRecord(string s) : this()
        {
            ResultDataParse(s);
        }

        public Hashtable htResult { get; set; }

        public void ResultDataParse(string sInput)
        {
            htResult = new Hashtable();
            sInput = sInput.Remove(0, 6);
            Int16 noOfTest = Convert.ToInt16(sInput.Substring(0, 3));
            DateTestCompleted = sInput.Substring(3, 8);
            Barcode = sInput.Substring(13, 13).Trim();

            for (int i = 0; i < noOfTest; ++i)
            {
                string testCode = sInput.Substring(83 + i*15, 3);
                string dataValue = sInput.Substring(83 + i*15 + 4, 8).Trim();
                htResult.Add(testCode, dataValue);
            }
        }
    }
}