using System;
using System.Collections.Generic;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    public class ADVIA1650QueryRecord
    {
        public List<string> QueryBarcode = new List<string>();

        public ADVIA1650QueryRecord()
        {
        }

        public ADVIA1650QueryRecord(string sInput)
            : this()
        {
            ParseData(sInput);
        }

        public bool ParseData(string sInput)
        {
            try
            {
                int noOfTest = Convert.ToInt16(sInput.Substring(6, 2));
                for (int i = 0; i < noOfTest; i++)
                {
                    QueryBarcode.Add(sInput.Substring(9 + i*13, 13));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

            //QueryBarcode = sInput.Substring(9, 13).Trim( );
            //return true;
        }
    }
}