using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Vietbait.Lablink.Devices.Immunology.ImmuliteXpi
{
    [TestFixture]
    internal class _ImmuliteXpiTest
    {
        [Test]
        public void HeaderTest()
        {
            try
            {
                const string rawHeader =
                    @"H|\^&||Password|Siemens|Randolph^New^Jersey^07869||(201)927-2828|8N1|YourSystem||P|1|19940323082858";
                var oldHeader = new ImmuliteXpiHeaderRecord();
                oldHeader.Parse(rawHeader);
                //h.Parse(rawHeader);
                Debug.WriteLine(oldHeader.RecordType);
                string message = oldHeader.Dump();
                Debug.WriteLine(message);
                var newHeader = (ImmuliteXpiHeaderRecord) oldHeader.Clone();
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void OrderTest()
        {
            const string rawHeader =
                @"O|1|1550623||^^^LH|R|19931011091233|19931011091233||||||Post Menopausal";
            var oldHeader = new ImmuliteXpiTestOrderRecord(rawHeader);
            oldHeader.Parse(rawHeader);
            //h.Parse(rawHeader);
            string message = oldHeader.Dump();
            Debug.WriteLine(message);
            var immuliteXpiTestOrderRecord = ((ImmuliteXpiTestOrderRecord) oldHeader.Clone());
            string newitem = immuliteXpiTestOrderRecord.Create();
            Debug.WriteLine(immuliteXpiTestOrderRecord.Dump());
            Debug.WriteLine("");
        }

        //[Test]
        //public void TerminationTest()
        //{
        //    try
        //    {
        //        var newHeader = new Bs800TerminationRecord();
        //        Debug.WriteLine(newHeader.Create());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        [Test]
        public void PatientInforTest()
        {
            const string rawHeader =
                @"P|1|101|||Riker^Al||19611102|F|||||Bashere";
            var oldHeader = new ImmuliteXpiPatientInformationRecord(rawHeader);
            oldHeader.Parse(rawHeader);
            //h.Parse(rawHeader);
            string message = oldHeader.Dump();
            Debug.WriteLine(message);
            Debug.WriteLine("");
        }

        //[Test]
        //public void ResultTest()
        //{
        //    try
        //    {
        //        //var newHeader = new CobasE6000ResultRecord(@"R|1|^^^2/1/not|8.60|nmol/L||N||F||BMSERV|||E11");
        //        var newHeader = new Bs800ResultRecord(@"R|24|001^Calcium^^F|2.549481^^^^|mmol/L|^|N||F|2.549481^^^^|0|20130705123028||Mindry^");

        //        //Debug.WriteLine(tempP.Create());
        //        Debug.WriteLine(newHeader.Create());
        //        var resultItem = newHeader.GetResult();
        //        Debug.WriteLine(resultItem.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        //[Test]
        //public void QueryTest()
        //{
        //    try
        //    {
        //        //var newHeader = new CobasE6000RequestInformationRecord(@"Q|1|^^                      ^1^5032^1^^S1^SC||ALL||||||||O");
        //        var newHeader = new Bs800RequestInformationRecord(@"Q|1|^SAMPLE123||||||||||O");
        //        var orderBarcode = newHeader.GetOrderBarcode();
        //        Debug.WriteLine(orderBarcode);
        //        Debug.WriteLine(newHeader.Create());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}
    }
}