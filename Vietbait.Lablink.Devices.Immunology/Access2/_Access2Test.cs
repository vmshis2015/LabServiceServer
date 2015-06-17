using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Vietbait.Lablink.Devices.Immunology
{
    [TestFixture]
    internal class _Access2Test
    {
        [Test]
        public void HeaderTest()
        {
            try
            {
                const string rawHeader = @"H|\^&|||BS800^01.03.07.03^123456|||||||PR|1394-97|20090910102501";
                var oldHeader = new Access2HeaderRecord();
                oldHeader.Parse(rawHeader);
                //h.Parse(rawHeader);
                Debug.WriteLine(oldHeader.RecordType);
                var newHeader = (Access2HeaderRecord) oldHeader.Clone();
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        //[Test]
        //public void OrderTest()
        //{
        //    try
        //    {
        //        const string rawHeader = @"O|1|1^1^1|SAMPLE123|1^Test1^2^1\2^Test2^2^1\3^Test3^2^1\4^Test4^2^1|R|20090910135300|20090910125300|||John|||||Urine|Dr.Who|Department1|1|Dr.Tom||||||O|||||";
        //        var order = new Bs800TestOrderRecord(rawHeader);
        //        Debug.WriteLine(order.Create());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

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

        //[Test]
        //public void PatientInforTest()
        //{
        //    try
        //    {
        //        var newHeader =
        //            new Bs800PatientInformationRecord(
        //                @"P|1||PATIENT111||Smith^Tom^J||19600315|M|||A||Dr.Bean|icteru|100012546||| Diagnosis information||0001|||||A1|002||||||||");
        //        Debug.WriteLine(newHeader.Create());
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

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