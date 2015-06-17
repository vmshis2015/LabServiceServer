using System;
using NUnit.Framework;
using Vietbait.Lablink.Utilities;

namespace Vietbait.DemoProject
{
    [TestFixture]
    internal class UtilitiesTest
    {
        [Test]
        public void TestGetRegList()
        {
            string s;
            s = string.Format("{0}2R|15|^^^90080^^^^|Âm Tính|||||F||||20130715165316||{1}{2}D0{3}{4}",
                DeviceHelper.STX, DeviceHelper.CR, DeviceHelper.ETX, DeviceHelper.CR,
                DeviceHelper.LF);
            //s = string.Format("{0}2R|15|^^^90080^^^^|Am Tinh|||||F||||20130715165316||{1}{2}D0{3}{4}",
            //                  DeviceHelper.STX, DeviceHelper.CR, DeviceHelper.ETX, DeviceHelper.CR,
            //                  DeviceHelper.LF);
            //s =
            //    System.IO.File.ReadAllText(
            //        @"D:\_PROJECT\Vietbait.CobasAdapter\Vietbait.CobasAdapter\bin\TestCheckSum.log");
            string x = TestDataForm.GetCheckSumValue2(s);
            Console.WriteLine(s);
        }
    }
}