using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class SwelabAlfaA : Rs232Base
    {
        #region Overrides of Rs232Devcie

        public override void ProcessRawData()
        {
            try
            {
                string mystringData = StringData.Trim();
                if (!mystringData.EndsWith("-->")) return;

                mystringData = string.Format(@"<VBIT>{0}</VBIT>", mystringData);

                XElement xElement = XElement.Parse(mystringData);

                // Get Id List
                List<string> barcodeList = (from p in xElement.Descendants("p")
                    let element = p.Element("n")
                    where element != null && element.Value.Equals("ID")
                    let element1 = p.Element("v")
                    select element1 != null ? element1.Value :"").ToList();

                List<string> testDateList = (from p in xElement.Descendants("p")
                    let element = p.Element("n")
                    where element != null && element.Value.Equals("DATE")
                    let element1 = p.Element("v")
                    where element1 != null
                    select element1.Value.Substring(0, 10)).ToList();
                
                var allResult = xElement.Descendants("smpresults");
                
                // Duyệt từng bệnh nhân để import dữ liệu
                var xElements = allResult as IList<XElement> ?? allResult.ToList();
                for (int i = 0; i < xElements.Count(); i++)
                {
                    TestResult.Barcode = barcodeList[i];
                    var tempTestDate = testDateList[i].Split('-');
                    TestResult.TestDate = string.Format("{0}/{1}/{2}", tempTestDate[2],
                    tempTestDate[1], tempTestDate[0]);
                    foreach (XElement element in xElements[i].Descendants("p"))
                    {
                        string testName = "";
                        try
                        {
                            testName = element.Element("n").Value;
                        }
                        catch (Exception)
                        {
                        }
                        string testValue = "";
                        try
                        {
                            testValue = element.Element("v").Value;
                        }
                        catch (Exception)
                        {
                        }
                        TestResult.Add(new ResultItem(testName, testValue));
                    }
                    Log.Debug("Begin import result: Barrcode {0}",TestResult.Barcode);
                    Log.Debug(ImportResults()?"Import Result success":"Error while import result");
                }
                ClearData();
            }
            catch (Exception ex)
            {
                Log.Debug("Error while process data:\r\n{0}",ex);
                ClearData();
                
            }
        }

        #endregion
    }
}