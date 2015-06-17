using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class CobasB121 : Rs232AstmDevice
        //internal class XS1000I : Rs232Base
    {
        /// <summary>
        ///     Xử lý sau kkhi nhận được dữ liệu
        /// </summary>
        public override void ProcessData()
        {
            try
            {
                // Kiểm tra kết thúc dữ liệu
                if (!StringData.EndsWith(string.Format("L|1|N{0}", DeviceHelper.CR)))
                {
                    Log.Trace("Du lieu nhan dc:\r\n{0}", StringData);
                    Log.Trace("Dek du data, thoat ra lam tiep");
                    return;
                }

                //Khai báo mảng kết quả.
                var arrResultName = new[]
                {
                    "pH", "PCO2", "PO2", "Na", "K", "Cl", "iCa", "tHb", "SO2", "Hct", "Temperature", "Baro", "cHCO3",
                    "SO2(c)", "BE", "BB", "PAO2", "AG", "pHt", "PCO2t", "PO2t", "PAO2t", "a/AO2t", "FIO2", "H+t"
                };

                Log.Trace("Begin Process Data");
                //Lưu lại Data
                Log.Trace(StringData);

                //Lấy về dữ liệu của các bệnh nhân
                IEnumerable<string> arrPatients = SeparatorData(StringData);

                //Duyệt qua mảng xử lý dữ liệu của từng bệnh nhân
                foreach (string patient in arrPatients)
                {
                    string[] records = patient.Split(DeviceHelper.CR);
                    foreach (string record in records)
                    {
                        string[] temp;
                        if (record.StartsWith("P"))
                        {
                            temp = record.Split('|');
                            if (temp.Length > 4)
                            {
                                TestResult.Barcode = temp[3].Trim();
                                Log.Debug("Barcode:{0}", TestResult.Barcode);
                            }
                        }
                        else if (record.StartsWith("R"))
                        {
                            try
                            {
                                temp = record.Split('|');
                                //Lấy về ngày tháng làm xét nghiệm
                                if (String.IsNullOrEmpty(TestResult.TestDate))
                                {
                                    string tempDate = temp[12];
                                    TestResult.TestDate = string.Format("{0}/{1}/{2}", tempDate.Substring(6, 2),
                                        tempDate.Substring(4, 2),
                                        tempDate.Substring(0, 4));
                                }

                                string strTestName = temp[2].Split('^')[3].Trim();

                                if (!arrResultName.Contains(strTestName)) continue;


                                string strTestValue = temp[3].Trim();
                                string strTestUnit = temp[4].Trim();
                                
                                AddResult(new ResultItem(strTestName, strTestValue, strTestUnit));
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error while processing result string:\r\n{1}\r\n Error Detail: {0}",ex,record );
                            }
                        }
                    }
                    
                    Log.Trace("Begin Import Result");
                    Log.Trace(ImportResults() ? "Import Result Success" : "Error While Import result");
                }
                ClearData();
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing data Error: {0}", ex);
                ClearData();
            }
        }

        private IEnumerable<string> SeparatorData(string stringData)
        {
            try
            {
                //Bỏ tất cả các ký tự cho đến khi gặp ký tự "P" đầu tiên
                stringData =
                    stringData.Substring(stringData.IndexOf(DeviceHelper.CR + "P|", StringComparison.Ordinal) + 1);

                //Ngắt chuỗi theo ký tự CR
                string[] arrStringData = stringData.Split(DeviceHelper.CR);

                //Biến để lưu tất cả các chuỗi kết quả.
                //Mỗi phần tử của chuỗi là một kết quả của bệnh nhân gồm các record P,O,R,C.....
                var allResult = new string[] {};

                //Biến lưu Index
                int id = 0;

                foreach (string line in arrStringData)
                {
                    if (line.StartsWith("P"))
                    {
                        id++;
                        Array.Resize(ref allResult, id);
                    }
                    allResult[id - 1] = allResult[id - 1] + line + DeviceHelper.CR;
                }

                return allResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}