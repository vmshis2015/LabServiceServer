using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class LH780 : Rs232Base
    {
        public override void ProcessRawData()
        {
            try
            {
                Log.Trace("Begin Process Data");

                //Lưu lại Data
                Log.Trace(StringData);

                // Tách nhiều bệnh nhân:
                string[] allPatients = Seperator(ReformatData(StringData));

                //Khai báo mảng kết quả.
                var arrResultName = new[]
                {
                    "WBC", "RBC", "HGB", "HCT", "MCV", "MCH", "MCHC", "RDW", "PLT", "MPV",
                    "LY#", "MO#", "NE#", "EO#", "BA#", "NRBC#", "LY%", "MO%", "NE%", "EO%",
                    "BA%", "NRBC%", "PDW", "PCT"
                };
                var arrHistogram = new[]
                {
                    "RBCH", "PLTH", "WBCF"
                };
                string _destImagePath = DeviceHelper.GetImageFolder();
                if (!_destImagePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    _destImagePath = string.Format("{0}{1}", _destImagePath, Path.DirectorySeparatorChar);

                // Lọc theo từng bệnh nhân để import kết quả
                foreach (string patient in allPatients)
                {
                    string[] tempStringArr;

                    string[] strResult = DeviceHelper.DeleteAllBlankLine(patient, DeviceHelper.CRLF);

                    //Lấy ngày:
                    int rowIndex = GetRowIndex(strResult, "DATE");
                    if (rowIndex > -1)
                    {
                        tempStringArr = strResult[rowIndex].Split();
                        if (tempStringArr.Length < 2) TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                        else
                        {
                            if (tempStringArr[1].Trim() == "")
                                TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                            else
                            {
                                string[] s = tempStringArr[1].Trim().Split('/');

                                TestResult.TestDate = string.Format("{0}/{1}/{2}{3}", s[1], s[0],
                                    DateTime.Now.Year.ToString(
                                        CultureInfo.InvariantCulture).Substring(0, 2),
                                    s[2]);
                            }
                        }
                    }
                    else
                    {
                        TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    Log.Trace("TestDate - {0}", TestResult.TestDate);

                    //Lấy TestID:
                    rowIndex = GetRowIndex(strResult, "ID1");
                    if (rowIndex > -1)
                    {
                        tempStringArr = strResult[rowIndex].Split();
                        TestResult.Barcode = tempStringArr.Length < 2
                            ? "0000"
                            : (tempStringArr[1].Trim() == ""
                                ? "0000"
                                : tempStringArr[1].Trim());
                    }
                    else
                    {
                        TestResult.Barcode = "0000";
                    }
                    Log.Trace("Barcode - {0}", TestResult.Barcode);

                    //Lấy các kết quả.

                    //Lấy kết quả và lưu vào mảng.
                    foreach (string sResultName in arrResultName)
                    {
                        int indexResult = GetRowIndex(strResult, sResultName);
                        if (indexResult > -1)
                        {
                            tempStringArr = strResult[indexResult].Split();
                            string testValue = tempStringArr.Length < 2
                                ? ""
                                : (tempStringArr[1].Trim() == ""
                                    ? ""
                                    : tempStringArr[1].Trim());
                            AddResult(new ResultItem(sResultName, testValue));
                            Log.Trace("Add new Item Success:{0}-{1}", sResultName, testValue);
                        }
                    }

                    //try
                    //{
                    //    if (Directory.Exists(_destImagePath))
                    //    {
                    //        Log.Trace("Open folder: '{0}' Success", _destImagePath);
                    //        string fileName = string.Empty;
                    //        // Vẽ các biểu đồ
                    //        int boxWidth = 600;
                    //        int boxHeight = 300;

                    //        // Dựng Histogram

                    //        // WBC
                    //        rowIndex = GetRowIndex(strResult, "RBCH");
                    //        if (rowIndex > -1)
                    //        {
                    //            fileName = string.Format("{0}-{1}-RBCH.PNG", DateTime.Now.ToString("ddMMyyyy"),
                    //                TestResult.Barcode);
                    //            fileName = string.Format("{0}{1}", _destImagePath, fileName);
                    //            DeviceHelper.CreateHistogram("RBC", boxWidth, boxHeight,
                    //                strResult[rowIndex].Substring(4).Trim()).Save(fileName,
                    //                    ImageFormat.
                    //                        Png);
                    //            Log.Trace("Create RBC Histogram Success: File name - {0}", fileName);
                    //            TestResult.Add(new ResultItem("RBCH", fileName));
                    //        }

                    //        // PLT
                    //        rowIndex = GetRowIndex(strResult, "PLTH");
                    //        if (rowIndex > -1)
                    //        {
                    //            fileName = string.Format("{0}-{1}-PLTH.PNG", DateTime.Now.ToString("ddMMyyyy"),
                    //                TestResult.Barcode);
                    //            fileName = string.Format("{0}{1}", _destImagePath, fileName);
                    //            DeviceHelper.CreateHistogram("PLT", boxWidth, boxHeight,
                    //                strResult[rowIndex].Substring(4).Trim()).Save(fileName,
                    //                    ImageFormat.
                    //                        Png);
                    //            Log.Trace("Create PLT Histogram Success: File name - {0}", fileName);
                    //            TestResult.Add(new ResultItem("PLTH", fileName));
                    //        }
                    //        // WBC
                    //        rowIndex = GetRowIndex(strResult, "WBCF");
                    //        if (rowIndex > -1)
                    //        {
                    //            fileName = string.Format("{0}-{1}-WBCF.PNG", DateTime.Now.ToString("ddMMyyyy"),
                    //                TestResult.Barcode);
                    //            fileName = string.Format("{0}{1}", _destImagePath, fileName);
                    //            DeviceHelper.CreateHistogram("WBC", boxWidth, boxHeight,
                    //                strResult[rowIndex].Substring(4).Trim()).Save(fileName,
                    //                    ImageFormat.
                    //                        Png);
                    //            Log.Trace("Create WBC Histogram Success: File name - {0}", fileName);
                    //            TestResult.Add(new ResultItem("WBCF", fileName));
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Log.Trace("Open folder: '{0}' False", _destImagePath);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Log.Error(string.Format("Exception while build histogram:{0}", ex));
                    //}


                    Log.Debug("Import Result for barcode:" + TestResult.Barcode);
                    ImportResults();
                    Log.Debug("Import Result Success");
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Exception while processing data:{0}", ex));
            }
            finally
            {
                ClearData();
            }
        }

        private string[] Seperator(string s)
        {
            var l = new List<string>();
            string seperatorString = "--------------";
            int a = s.IndexOf(seperatorString);
            int b = s.IndexOf(seperatorString, a + seperatorString.Length);

            while (a > 0)
            {
                l.Add(s.Substring(a, b - a));
                a = s.IndexOf(seperatorString, b + seperatorString.Length);
                b = s.IndexOf(seperatorString, a + seperatorString.Length);
            }
            return l.ToArray();
        }

        /// <summary>
        ///     Chuẩn hóa lại dữ liệu. Xóa bỏ các dòng trắng, xóa bỏ các dòng bắt đầu bằng ký tự DC1
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string ReformatData(string inputString)
        {
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == DeviceHelper.STX)
                    inputString = inputString.Remove(i, 3);
            }
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == DeviceHelper.ETX)
                    inputString = inputString.Remove(i - 4, 5);
            }
            return inputString;
        }

        /// <summary>
        ///     Trả về index của dòng với chuỗ bắt đầu xác định
        /// </summary>
        /// <param name="arrString">Mảng cần tìm Index</param>
        /// <param name="beginString">Chuỗi bắt đầu</param>
        /// <returns></returns>
        private int GetRowIndex(string[] arrString, string beginString)
        {
            try
            {
                int result = -1;
                for (int i = 0; i < arrString.Length; i++)
                    if (arrString[i].StartsWith(beginString))
                    {
                        result = i;
                        break;
                    }
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}