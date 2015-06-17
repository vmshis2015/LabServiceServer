using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Utilities
{
    [TestFixture]
    internal class UtilitiesTest
    {
        [Test]
        public void GetNumber()
        {
            string x = File.ReadAllText(@"C:\_ADVIA.txt");
            var re = new Regex(@"[-+]?([0-9]*\.[0-9]+|[0-9]+)");
            //Match m = re.Match(" *KET -13.999998mmol/L       ");
            //if (m.Success)
            //{
            //    Console.WriteLine(string.Format("RegEx found " + m.Value + " at position " + m.Index));

            //}
        }

        [Test]
        public void TestByteArrayCheckSum()
        {
            byte[] peer0_25 =
            {
                0x02, 0x35, 0x52, 0x7c, 0x32, 0x7c, 0x5e, 0x5e,
                0x5e, 0x37, 0x30, 0x30, 0x33, 0x30, 0x5e, 0x5e,
                0x5e, 0x5e, 0x7c, 0x44, 0xc6, 0xb0, 0xc6, 0xa1,
                0x6e, 0x67, 0x20, 0x74, 0xc3, 0xad, 0x6e, 0x68,
                0x7c, 0x7c, 0x7c, 0x7c, 0x7c, 0x46, 0x7c, 0x7c,
                0x7c, 0x7c, 0x32, 0x30, 0x31, 0x33, 0x30, 0x37,
                0x31, 0x39, 0x31, 0x30, 0x31, 0x36, 0x31, 0x38,
                0x7c, 0x7c, 0x0d, 0x03, 0x46, 0x42, 0x0d, 0x0a
            };

            string s;
            //s = string.Format("{0}2R|15|^^^90080^^^^|Âm Tính|||||F||||20130715165316||{1}{2}D0{3}{4}",
            //                  DeviceHelper.STX, DeviceHelper.CR, DeviceHelper.ETX, DeviceHelper.CR,
            //                  DeviceHelper.LF);
            //s = string.Format("{0}2R|15|^^^90080^^^^|Am Tinh|||||F||||20130715165316||{1}{2}D0{3}{4}",
            //                  DeviceHelper.STX, DeviceHelper.CR, DeviceHelper.ETX, DeviceHelper.CR,
            //                  DeviceHelper.LF);
            //s =
            //    System.IO.File.ReadAllText(
            //        @"D:\_PROJECT\Vietbait.CobasAdapter\Vietbait.CobasAdapter\bin\TestCheckSum.log");

            s = Encoding.UTF8.GetString(peer0_25);
            byte[] p = Encoding.UTF8.GetBytes(s);
            string xxx = DeviceHelper.GetStringAfterCheckSum(s);
            string xxxx = DeviceHelper.GetStringAfterCheckSum(peer0_25);
            //var xxx = DeviceHelper.GetStringAfterCheckSum(System.Text.Encoding.UTF8.GetBytes(s));
            Console.WriteLine(xxx);
        }

        [Test]
        public void TestCheckSumString()
        {
            byte[] bytes = File.ReadAllBytes(@"D:\_ErrorReport\YHHQ\20130923\rawdata_2.txt");
            string checksum = DeviceHelper.GetStringAfterCheckSum(bytes);
            Console.WriteLine(checksum);
        }

        [Test]
        public void TestCreateHistogram()
        {
            int boxWidth = 1024;
            int boxHeight = 300;
            string wbcinput =
                "000000000000000000000000000000000000070604060607070B111B273C54697C8FA5B8C8CCCCC2B8A895816F5F4F41342B241E1B191614131111131314131414171719171613131313131313131313141416161717171616191B1E1E1F21262727292B2C2C2C2E333434363739393939393937373A3F42423F3E4146494949494A474647494A49464646464644413F414242413F3E3C3A393A3939393A3A3A37342F2E2B2B2C2C2C2B2B2927262624231F1E1C1C1B1917161616141413110E0C0C0B0B090707070706060607070604040303030303030101010101010303010100000000000000000000000000000000000000000000000000000000000000";
            string rbcinput =
                "020200000000000000000000000000000000000000000000000001020406070B0E141920293341505F6F8192A0ACB5BFC7CCC9C5C0BBB1A395877A6C5F52463D342D26201C181613110F0D0C0C0C0B0B0B0B0A090A0B0C0C0B0A0A0C0C0D0C0C0B0B0B0A0A0A090907070708090A0909080807080707060606070606060605050505050505040505050403020303030202010101020202010100000000010101010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            string pltinput =
                "00000000000515324F64798FA4B4BEC6C9CCC9C4BEB6AC9E978F84796F645C544F4A443F3C37322D27221F1A17120F0D0F0F0F0D0D0D0A0A070707070707050205020502070002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

            DeviceHelper.CreateHistogram("WBC", boxWidth, boxHeight, wbcinput).Save(@"C:\WBC.PNG", ImageFormat.Png);
            DeviceHelper.CreateHistogram("RBC", boxWidth, boxHeight, rbcinput).Save(@"C:\RBC.PNG", ImageFormat.Png);
            DeviceHelper.CreateHistogram("PLT", boxWidth, boxHeight, pltinput).Save(@"C:\PLT.PNG", ImageFormat.Png);
        }

        [Test]
        public void TestGetDataToView()
        {
            string dataToView = DeviceHelper.GetDataToView("4.65", 1);
            Console.WriteLine(dataToView);
        }

        [Test]
        public void TestGetParaNameFromDeviceId()
        {
            DataTable dataToView = DeviceHelper.GetParaNameFromDeviceId(1);
            Console.WriteLine(dataToView);
        }

        [Test]
        public void TestGetRegList()
        {
            string patientName = "";
            List<string> ds = DeviceHelper.GetRegList(28, "1303218017", ref patientName);
            //ds = DeviceHelper.GetRegData(9, "1204260002");
            Console.WriteLine();
        }

        [Test]
        public void TestImportResult()
        {
            int testTypeId = 0;
            DataTable tblParaName = null;
            decimal deviceId = 0;
            DeviceHelper.GetDeviceConfig("AU640",ref testTypeId,ref tblParaName,ref deviceId);

            var lo = new Result(1);
            lo.Barcode = "1407080001.2";
            lo.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
            lo.Add(new ResultItem("GLU", "4.4"));
            DeviceHelper.ImportSingleResult(-1, lo, tblParaName);
        }

        [Test]
        public void TestPrepareCalculatedResults()
        {
            DataTable tblParaName = DeviceHelper.GetParaNameFromDeviceId(3);
            var result = new Result(-1);
            result.Barcode = DateTime.Now.ToString("yyMMdd") + 0001;
            result.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
            result.Add(new ResultItem("7", "3"));
            DeviceHelper.PrepareCalculatedResults(ref result, tblParaName);
        }

        [Test]
        public void TestString()
        {
            string s = "1305130001.3.";
            Console.ReadKey(true);
        }

        [Test]
        public void sssss()
        {
            //var x = System.Text.Encoding.ASCII.GetString(new byte[] {230});
            //File.WriteAllText(@"C:\x.txt",x);
            byte[] hexstring = File.ReadAllBytes(@"D:\MyDocument\_Desktop\GABIN-2.bin");
            //byte[] dBytes = Enumerable.Range(0, hexstring.Length)
            //    .Where(x => x % 2 == 0)
            //    .Select(x => Convert.ToByte(hexstring.Substring(x, 2), 16))
            //    .ToArray();
            string returnstr = DeviceHelper.GetStringAfterCheckSum(hexstring);
            Debug.WriteLine(returnstr);
        }
    }
}