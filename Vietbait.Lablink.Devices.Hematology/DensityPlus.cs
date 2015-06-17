using System;
using System.Globalization;
using System.Timers;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class DensityPlus : Rs232Base
    {
        /// <summary>
        ///     Khai báo timmer để gửi ACK
        /// </summary>
        private readonly Timer _timer = new Timer(200);

        public override void ProcessRawData()
        {
            try
            {
                _timer.Elapsed -= TimerOnElapsed;
            }
            catch (Exception)
            {
            }
            finally
            {
                _timer.Elapsed += TimerOnElapsed;
            }
            try
            {
                //Kiểm tra nếu chưa kết thúc nhận dữ liệu
                if (!StringData.Contains(DeviceHelper.EOT.ToString(CultureInfo.InvariantCulture)))
                {
                    _timer.Enabled = true;
                    _timer.Start();
                    return;
                }
                _timer.Stop();
                string[] allResult = StringData.Split(DeviceHelper.CR);

                foreach (string record in allResult)
                {
                    TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                    bool startsWithD = record.StartsWith(string.Format("{0}D", DeviceHelper.STX));
                    bool startsWithRcs = record.StartsWith(string.Format("{0}RCS", DeviceHelper.STX));
                    if (startsWithD || startsWithRcs)
                    {
                        string[] strResult = DeviceHelper.DeleteAllBlankLine(record, " ");
                        try
                        {
                            TestResult.Barcode = strResult[0].Substring(startsWithD ? 2 : 7);
                        }
                        catch (Exception)
                        {
                            TestResult.Barcode = "";
                        }
                        try
                        {
                            TestResult.Add(new ResultItem(strResult[1], strResult[2], strResult[3]));
                        }
                        catch (Exception)
                        {
                        }
                        ImportResults();
                    }
                }
                ClearData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
        }
    }
}