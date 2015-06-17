using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    internal class A25 : FileDevice
    {
        public override void ProcessRawData()
        {
            try
            {
                string localFilePath = string.Format("MyLog{0}{1}{0}{2}{0}{3}{0}{4}{0}Raw{0}{5}_{6}{7}",
                    Path.DirectorySeparatorChar,
                    DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'),
                    DateTime.Now.Day.ToString().PadLeft(2, '0'), DeviceName,
                    Path.GetFileNameWithoutExtension(FileName), 0,
                    Path.GetExtension(FileName));
                if (!Directory.Exists(Path.GetDirectoryName(localFilePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));
                int errorcount = 0;
                label2:
                if (errorcount <= 20)
                {
                    try
                    {
                        if (errorcount == 0) Log.Debug("Try to coppy file: {0}", FullFilePath);
                        //Lấy về tên file bị thay đổi
                        if (FileName != null)
                        {
                            int fileNameCount = 0;
                            while (File.Exists(localFilePath))
                            {
                                fileNameCount++;
                                localFilePath = string.Format("MyLog{0}{1}{0}{2}{0}{3}{0}{4}{0}Raw{0}{5}_{6}{7}",
                                    Path.DirectorySeparatorChar,
                                    DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'),
                                    DateTime.Now.Day.ToString().PadLeft(2, '0'), DeviceName,
                                    Path.GetFileNameWithoutExtension(FileName), fileNameCount,
                                    Path.GetExtension(FileName));
                            }
                            File.Copy(FullFilePath, localFilePath, true);
                            Log.Debug("Coppy file success:{0}", localFilePath);
                        }
                    }
                    catch (Exception)
                    {
                        errorcount++;
                        Thread.Sleep(2000);
                        goto label2;
                    }

                    Log.Trace(
                        "Begin Process Data-----------------------------------------------------------------------");
                    try
                    {
                        // Đoạn xử lý dữ liệu chính
                        // Đọc toàn bộ dữ liệu
                        var allrawdata = File.ReadAllLines(localFilePath);

                        // Lấy ra danh sách barcode
                        var allBarcode = (from raw in allrawdata
                                          where !string.IsNullOrEmpty(raw.Trim())
                                          select raw.Split('\t')[0].Trim()).Distinct().ToList();
                        
                        // Duyệt theo mảng barcode lấy dữ liệu của từng bệnh nhân
                        var allPatients = new List<string>();

                        foreach (string barcode in allBarcode)
                        {
                            var tempPatient = string.Join(DeviceHelper.CR.ToString(), (from raw in allrawdata
                                where !string.IsNullOrEmpty(raw.Trim()) && raw.Split('\t')[0].Trim().Equals(barcode)
                                select raw).ToArray());
                            allPatients.Add(tempPatient);
                        }

                        // Xử lý dữ liệu từng bệnh  nhân

                        foreach (string allPatient in allPatients)
                        {
                            TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                            var allData = allPatient.Split(DeviceHelper.CR);
                            foreach (string strResult in allData)
                            {
                                var tempResult = strResult.Split('\t');
                                if (string.IsNullOrEmpty(TestResult.Barcode)) TestResult.Barcode = tempResult[0];
                                AddResult(new ResultItem(tempResult[1].Trim(), tempResult[3].Trim(), tempResult[4].Trim()));
                            }
                            Log.Trace("Begin import result for barcode:{0}",TestResult.Barcode);
                            Log.Debug(ImportResults()?"Import Result Success" : "Error while import result");
                            TestResult.Clear();
                        }
                        Log.Debug("Finish !!!!");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Error While Process file:{0}", ex));
                    }
                }
                else
                {
                    Log.Error("Could not Copy File");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                File.WriteAllText(@"C:\A25-Exception.txt", ex.ToString());
            }
            finally
            {
                ClearData();
                TestResult.Clear();
            }
        }
    }
}