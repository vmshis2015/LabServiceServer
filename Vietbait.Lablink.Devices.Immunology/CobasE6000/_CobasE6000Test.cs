using System;
using System.Diagnostics;
using NUnit.Framework;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Immunology
{
    [TestFixture]
    internal class _CobasE6000Test
    {
        [Test]
        public void HeaderTest()
        {
            try
            {
                const string rawHeader = @"H|\^&|||cobas6000^1|||||host|RSUPL^BATCH|P|1";
                var oldHeader = new CobasE6000HeaderRecord();
                oldHeader.Parse(rawHeader);
                //h.Parse(rawHeader);
                Debug.WriteLine(oldHeader.RecordType);
                var newHeader = (CobasE6000HeaderRecord) oldHeader.Clone();
                newHeader.ReveiverId.Data = oldHeader.SenderNameOrId.Data;
                newHeader.SenderNameOrId.Data = oldHeader.ReveiverId.Data;
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
            try
            {
                const string rawHeader =
                    @"O|1|000003                |3^5238^3^^S1^SC|^^^2^1|R||20000529125556||||N||||1|||||||20000529125645|||F";
                var order = new CobasE6000TestOrderRecord(rawHeader);
                var neworder =
                    new CobasE6000TestOrderRecord(
                        @"O|1|                  0003|0^5020^5^^S1^SC|^^^10^1|R||20121105163859||||N||||1|||||||20121105163860|||F");
                Debug.WriteLine(order.Create());
                Debug.WriteLine(neworder.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void PatientInforTest()
        {
            try
            {
                var newHeader = new CobasE6000PatientInformationRecord(@"P|1|||||||M||||||35^Y");
                var tempP = new CobasE6000PatientInformationRecord(@"P|1|||||||M||||||35^Y||||||||||||||||||||");
                Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void QueryTest()
        {
            try
            {
                //var newHeader = new CobasE6000RequestInformationRecord(@"Q|1|^^                      ^1^5032^1^^S1^SC||ALL||||||||O");
                var newHeader =
                    new CobasE6000RequestInformationRecord(
                        @"Q|1|^^121108010001          ^1^5032^1^^S1^SC^R1||ALL||||||||O");
                //var tempP = new CobasE6000PatientInformationRecord(@"P|1|||||||M||||||35^Y||||||||||||||||||||");
                //Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.GetOrderBarcode());
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void ResultTest()
        {
            try
            {
                //var newHeader = new CobasE6000ResultRecord(@"R|1|^^^2/1/not|8.60|nmol/L||N||F||BMSERV|||E11");
                var newHeader =
                    new CobasE6000ResultRecord(@"R|1|^^^64/1/not|46.22|IU/L||H||F||BMSERV|20050915150011||E11");

                //Debug.WriteLine(tempP.Create());
                Debug.WriteLine(newHeader.Create());
                ResultItem resultItem = newHeader.GetResult();
                Debug.WriteLine(resultItem.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        [Test]
        public void TerminationTest()
        {
            try
            {
                var newHeader = new CobasE6000TerminationRecord();
                Debug.WriteLine(newHeader.Create());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}