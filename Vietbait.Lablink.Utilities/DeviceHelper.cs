using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NLog;
using SubSonic;
using VietBaIT.CommonLibrary;
using Vietbait.Lablink.Model;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Utilities
{
    public class DeviceHelper : CommonBusiness
    {
        #region Fields

        private static readonly Logger Log;

        private static readonly BackgroundWorker _resultWorker;
        private static readonly List<TResultDetail> _resultList = new List<TResultDetail>();

        private static readonly string[] VietnameseSigns =
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        private static readonly string _barcodeType = string.Empty;

        #endregion

        #region Constructor

        static DeviceHelper()
        {
            new DeviceHelper();
            Log = LogManager.GetLogger(MainServiceLogger);
            object x =
                new Select(TblSystemParameter.Columns.SValue).From(TblSystemParameter.Schema.Name).Where(
                    TblSystemParameter.Columns.SName).IsEqualTo("BARCODETYPE").ExecuteScalar();
            _barcodeType = x == null ? "" : x.ToString();

            // Khởi tạo background worker:
            _resultWorker = new BackgroundWorker();
            _resultWorker.DoWork += _resultWorker_DoWork;
        }

        private static void _resultWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (_resultList.Count > 0)
            {
                try
                {
                    _resultList[0].Save();
                    _resultList.RemoveAt(0);
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion

        #region Constant

        public const char NULL = (char) 0;
        public const char STX = (char) 2;
        public const char ETX = (char) 3;
        public const char EOT = (char) 4;
        public const char ENQ = (char) 5;
        public const char ACK = (char) 6;
        public const char CR = (char) 13;
        public const char LF = (char) 10;
        public const char VT = (char) 11;
        public const char NAK = (char) 21;
        public const char ETB = (char) 23;
        public const char FS = (char) 28;
        public const char GS = (char) 29;
        public const char RS = (char) 30;
        public const char SOH = (char) 1;
        public const char SYN = (char) 22;
        public const char DC1 = (char) 17;
        public const char DC2 = (char) 18;
        public const char DC3 = (char) 19;
        public const char DC4 = (char) 20;
        public const string MainServiceLogger = "_LABLink Service";
        public static int REPORTTYPE;
        public static readonly string CRLF = String.Format("{0}{1}", CR, LF);

        #endregion

        #region Private Method

        private static void PrepareResult(ref Result result, DataTable tblPara, short testTypeId)
        {

            try
            {
                var deviceName = (from t in tblPara.AsEnumerable() select t[DDeviceList.Columns.DeviceName].ToString()).FirstOrDefault();

                // Lấy về barcode thực khi trong barcode có dấu "."
                result.Barcode = result.Barcode.Contains(".") && !deviceName.Equals("CompactX")
                    ? result.Barcode.Substring(0, result.Barcode.LastIndexOf('.'))
                    : result.Barcode;
            }
            catch (Exception)
            {
            }

            //Kiểm tra barcode
            //Nếu là rỗng hoặc chứa tất cả các ký tự 0 thì tự sinh barcode
            //Barcode tự sinh có dạng yyyyMMdd_HHmmss.NB
            if (string.IsNullOrEmpty(result.Barcode.Replace("0", "").Trim()))
                result.Barcode = string.Format("{0}.NB", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            //Nếu chuỗi barcode  <=4 tiến hành sinh barcode theo quy tắc.
            if (result.Barcode.Length < 5)
            {
                try
                {
                    result.Barcode = result.Barcode.PadLeft(4, '0');
                    result.Barcode = LablinkServiceConfig.GetTestTypeBarcode().Equals("False")
                        ? String.Format("{0}{1}{2}{3}", result.TestDate.Substring(8, 2),
                            result.TestDate.Substring(3, 2), result.TestDate.Substring(0, 2),
                            result.Barcode)
                        : String.Format("{0}{1}{2}{3}{4}", result.TestDate.Substring(8, 2),
                            result.TestDate.Substring(3, 2), result.TestDate.Substring(0, 2),
                            SpGetIntOrder(testTypeId),
                            result.Barcode);
                }
                catch (Exception ex)
                {
                    Log.Error("Error while parsing barcode\r\n{0}", ex);
                }
            }


            if (tblPara == null) return;

            if (!tblPara.Columns.Contains("TestData_ID")) tblPara.Columns.Add("TestData_ID");

            foreach (ResultItem resultItem in result.Items)
                for (int i = 0; i <= tblPara.Rows.Count - 1; i++)
                    if (tblPara.Rows[i]["Alias_name"].ToString().ToUpper().Trim() ==
                        resultItem.TestName.ToUpper().Trim())
                    {
                        string sTestName = Utility.sDbnull(tblPara.Rows[i]["Data_name"]);
                        if (!string.IsNullOrEmpty(sTestName)) resultItem.TestName = sTestName;
                        resultItem.TestValue = GetDataToView(resultItem.TestValue,
                            Utility.Int16Dbnull(tblPara.Rows[i]["Data_Point"], 2));
                        resultItem.MeasureUnit = Utility.sDbnull(tblPara.Rows[i]["Measure_Unit"]);
                        resultItem.NormalLevel = Utility.sDbnull(tblPara.Rows[i]["Normal_Level"]);
                        resultItem.NormalLevelW = Utility.sDbnull(tblPara.Rows[i]["Normal_LevelW"]);
                        resultItem.DataSequence = Utility.Int32Dbnull(tblPara.Rows[i]["Data_Sequence"], 100);
                        resultItem.TestTypeId = Utility.Int16Dbnull(tblPara.Rows[i]["TestType_ID"], -1);
                        if (resultItem.TestTypeId == -1) resultItem.TestTypeId = testTypeId;
                        string tempTestDataId = Utility.sDbnull(tblPara.Rows[i]["TestData_ID"]);
                        if (!string.IsNullOrEmpty(tempTestDataId)) resultItem.TestDataId = tempTestDataId;
                        break;
                    }
                    else
                    {
                        resultItem.TestTypeId = testTypeId;
                    }
        }

        /// <summary>
        ///     Lấy ra giá trị hiển thị của kết quả từ cấu hình
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="iDataPoint"></param>
        /// <returns></returns>
        public static string GetDataToView(string strInput, Int16 iDataPoint)
        {
            if (iDataPoint == -10) return strInput;
            try
            {
                strInput = strInput.Trim();

                for (int iCount = 0; iCount <= strInput.Length - 1; iCount++)
                    if ((strInput[iCount] != '.') && (!Char.IsDigit(strInput, iCount)))
                        return strInput;

                while ((strInput.Substring(0, 1) == "0") && (strInput.Length > 1))
                    if (strInput.Substring(1, 1) == "0")
                        strInput = strInput.Remove(0, 1);
                    else
                        break; // TODO: might not be correct. Was : Exit Do

                string dataToView =
                    Math.Round(Convert.ToDouble(strInput), iDataPoint, MidpointRounding.AwayFromZero)
                        .ToString(CultureInfo.InvariantCulture);
                if ((dataToView.IndexOf('.') < 0) && (iDataPoint > 0))
                {
                    dataToView = dataToView + ".";
                    for (int i = 0; i < iDataPoint; i++)
                        dataToView = dataToView + "0";
                }
                else if ((dataToView.IndexOf('.') > 0) && (iDataPoint > 0))
                {
                    //Lấy về số lượng sau dấu phẩy
                    int len =
                        dataToView.Substring(dataToView.IndexOf('.') + 1,
                            dataToView.Length - dataToView.IndexOf('.') - 1).Length;
                    if (len < iDataPoint)
                        for (int i = len; i < iDataPoint; i++)
                            dataToView = dataToView + "0";
                }
                return dataToView;
            }
            catch (Exception)
            {
                return strInput;
            }
        }

        private static string SpGetIntOrder(int testTypeId)
        {
            string result = String.Empty;
            SqlQuery query =
                new Select(TTestTypeList.Columns.IntOrder).Top("1").From(TTestTypeList.Schema.TableName).Where(
                    TTestTypeList.Columns.TestTypeId).IsEqualTo(testTypeId);
            IDataReader re = query.ExecuteReader();
            if (re.Read())
            {
                result = re.GetInt16(0).ToString(CultureInfo.InvariantCulture);
                if (result.Length == 1) result = String.Format("0{0}", result);
                return result;
            }
            return result;
        }

        private static TTestInfoCollection GetTestInfo(ref string barcode, ref long pPatientId,
            IEnumerable<short> arrTestTypeId, DateTime testDate, string testSequence)
        {
            try
            {
                //B1: Lấy về tất cả thông tin liên quan đến Barcode hiện tại:
                var dtBarcode =
                    new Select(string.Format("{0}.*", TTestInfo.Schema.Name)).From(TTestInfo.Schema.Name).
                        Where(TTestInfo.Columns.Barcode).IsEqualTo(barcode).
                        ExecuteAsCollection<TTestInfoCollection>();
                //B2: Lấy ra Patient_ID
                // 1.Nếu Barcode không tồn tại:
                if (dtBarcode.Count.Equals(0)) //Nếu không tìm thấy dòng nào
                {
                    barcode = barcode.EndsWith(".NB") ? barcode : string.Format("{0}.NB", barcode);

                    string autoGeneratePatientAndTestOrder =
                        LablinkServiceConfig.GetAutoGeneratePatientAndTestOrder().ToUpper();

                    // Nếu cho phép sinh bệnh nhân mới.

                    if ((autoGeneratePatientAndTestOrder == "1") || (autoGeneratePatientAndTestOrder == "TRUE"))
                    {
                        //Nếu chưa tồn tại bệnh nhân thì tạo BN mới
                        LPatientInfo tempPatient =
                            (new Select().From(LPatientInfo.Schema.Name).Where(LPatientInfo.Columns.PatientName).
                                IsEqualTo(barcode).ExecuteAsCollection<LPatientInfoCollection>()).FirstOrDefault();
                        if (tempPatient != null)
                        {
                            pPatientId = (long) tempPatient.PatientId;
                        }
                        else
                        {
                            var obj = new LPatientInfo
                            {
                                Pid = testDate.ToString("yyyyMMddHHmmssfff"),
                                PatientName = barcode,
                                Dateupdate = testDate,
                                //Address = testSequence,
                                IsNew = true
                            };
                            obj.Save();
                            pPatientId = (long) obj.PatientId;
                        }
                    }
                        //Nếu không cho phép thêm bệnh nhân mới. lấy MIN Patient r
                    else
                    {
                        //Lấy về Patient nhỏ nhất
                        pPatientId = Utility.Int64Dbnull(TTestInfo.CreateQuery().GetMin(TTestInfo.Columns.PatientId), 0);
                        pPatientId = pPatientId > -1 ? -1 : pPatientId - 1;
                    }
                    dtBarcode =
                        new Select("*").From(TTestInfo.Schema.Name).
                            Where(TTestInfo.Columns.Barcode).IsEqualTo(barcode).
                            ExecuteAsCollection<TTestInfoCollection>();
                } // Kết thúc trường hợp không có đăng ký
                    // Nếu đã có bệnh nhân thì lấy luôn ID cũ
                else
                {
                    // Lấy ngay ID đầu tiên lun
                    pPatientId = (long) dtBarcode.FirstOrDefault().PatientId;
                }

                //B3: Insert TestInfo
                foreach (short testTypeId in arrTestTypeId)
                {
                    short id = testTypeId;
                    TTestInfo tempTestInfo = (from t in dtBarcode
                        where t.TestTypeId == id
                        select t).FirstOrDefault();
                    if (tempTestInfo != null) continue;
                    tempTestInfo = new TTestInfo
                    {
                        TestTypeId = testTypeId,
                        PatientId = pPatientId,
                        Barcode = barcode,
                        TestDate = DateTime.Now,
                        RequireDate = testDate,
                        IsNew = true
                    };
                    tempTestInfo.Save();
                    dtBarcode.Add(tempTestInfo);
                }
                return dtBarcode;
            }
            catch (Exception ex)
            {
                Log.Error("Error while Get Test ID {0}", ex);
                throw;
            }
        }

        private static DataSet GetRegDataNew(int pDeviceId, string pBarcode)
        {
            try
            {
                return SPs.SpGetRegListForService(pBarcode, pDeviceId).GetDataSet();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        ///     Hàm trả về chuỗi Checksum của một Frame.
        ///     Check khi có một trong hai giá trị ETX, ETB
        /// </summary>
        /// <param name="frame">Chuỗi cần kiểm tra</param>
        /// <returns>Chuỗi trả về là 2 ký tự dùng để checksum</returns>
        public static string GetCheckSumValue(string frame)
        {
            string checksum = "00";

            int sumOfChars = 0;
            bool complete = false;

            //take each byte in the string and add the values
            for (int idx = 0; idx < frame.Length; idx++)
            {
                int byteVal = Convert.ToInt32(frame[idx]);

                switch (byteVal)
                {
                    case STX:
                        sumOfChars = 0;
                        break;
                    case ETX:
                    case ETB:
                        sumOfChars += byteVal;
                        complete = true;
                        break;
                    default:
                        sumOfChars += byteVal;
                        break;
                }

                if (complete)
                    break;
            }

            if (sumOfChars > 0)
            {
                //hex value mod 256 is checksum, return as hex value in upper case
                checksum = Convert.ToString(sumOfChars%256, 16).ToUpper();
            }

            //if checksum is only 1 char then prepend a 0
            return (checksum.Length == 1 ? "0" + checksum : checksum);
        }

        /// <summary>
        ///     Hàm trả về chuỗi Checksum của một Frame.
        ///     Check khi có một trong hai giá trị ETX, ETB
        /// </summary>
        /// <param name="frame">Chuỗi cần kiểm tra</param>
        /// <returns>Chuỗi trả về là 2 ký tự dùng để checksum</returns>
        public static string GetCheckSumValue(byte[] byteData)
        {
            string checksum = "00";

            int sumOfChars = 0;
            bool complete = false;

            //take each byte in the string and add the values
            for (int idx = 0; idx < byteData.Length; idx++)
            {
                int byteVal = byteData[idx];

                switch (byteVal)
                {
                    case (byte) STX:
                        sumOfChars = 0;
                        break;
                    case (byte) ETX:
                    case (byte) ETB:
                        sumOfChars += byteVal;
                        complete = true;
                        break;
                    default:
                        sumOfChars += byteVal;
                        break;
                }

                if (complete)
                    break;
            }

            if (sumOfChars > 0)
            {
                //hex value mod 256 is checksum, return as hex value in upper case
                checksum = Convert.ToString(sumOfChars%256, 16).ToUpper();
            }

            //if checksum is only 1 char then prepend a 0
            return (checksum.Length == 1 ? "0" + checksum : checksum);
        }

        /// <summary>
        ///     Lấy về chuỗi sau khi đã xử lý CheckSum
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static string GetStringAfterCheckSum(string frame)
        {
            string result;
            try
            {
                //Tính giá trị CheckSum của chuỗi truyền vào
                string csValue = GetCheckSumValue(frame);
                Console.WriteLine(csValue);

                //Gán biến CheckSum 
                string csValueFromString = string.Empty;

                //Tìm index của giá trị CRLF
                int lastIndexOfCrlf = frame.LastIndexOf(CRLF, StringComparison.Ordinal);
                if (frame.Length > lastIndexOfCrlf - 2)
                {
                    //Lấy giá trị CheckSum từ chuỗi truyền vào
                    csValueFromString = frame.Substring(lastIndexOfCrlf - 2, 2);
                    Console.WriteLine(csValueFromString);
                }

                //So sánh giá trị tính toán và giá trị chứa trong chuỗi
                bool valid = csValue.Equals(csValueFromString);

                //Nếu chuỗi tính toán bằng chuỗi CheckSum được gửi kèm => lấy ra dữ liệu
                if (valid)
                {
                    Console.WriteLine(@"CheckSum OK");
                    int idFirstStx = frame.IndexOf(STX);
                    result = frame.Substring(idFirstStx + 2);
                    int idLastCrlf = result.LastIndexOf(CRLF, StringComparison.Ordinal);
                    result = result.Substring(0, idLastCrlf - 3);
                }
                    //Nếu có lỗi xảy ra khi truyền dữ liệu trả về chuỗi lỗi mặc định
                else
                {
                    Console.WriteLine(@"CheckSum Error");
                    result = string.Empty;
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        ///     Lấy về chuỗi sau khi đã xử lý CheckSum
        /// </summary>
        /// <param name="byteData"> </param>
        /// <returns></returns>
        public static string GetStringAfterCheckSum(byte[] byteData)
        {
            string result;
            try
            {
                //Tính giá trị CheckSum của chuỗi truyền vào
                string csValue = GetCheckSumValue(byteData);
                Console.WriteLine(csValue);

                //Gán biến CheckSum 

                //Tìm index của giá trị CRLF
                int lastIndexOfCrlf = 0;
                for (int i = byteData.Length - 1; i >= 0; i--)
                    if (byteData[i] == LF)
                        if (byteData[i - 1] == CR)
                        {
                            lastIndexOfCrlf = i - 1;
                            break;
                        }

                if (lastIndexOfCrlf <= 0) return string.Empty;

                //Lấy giá trị CheckSum từ chuỗi truyền vào
                string csValueFromString = string.Format("{0}{1}", (char) byteData[lastIndexOfCrlf - 2],
                    (char) byteData[lastIndexOfCrlf - 1]);
                Console.WriteLine(csValueFromString);


                //So sánh giá trị tính toán và giá trị chứa trong chuỗi
                bool valid = csValue.Equals(csValueFromString);

                //Nếu chuỗi tính toán bằng chuỗi CheckSum được gửi kèm => lấy ra dữ liệu
                if (valid)
                {
                    Console.WriteLine(@"CheckSum OK");
                    int idFirstStx = 0;
                    for (int i = 0; i < byteData.Length - 1; i++)
                        if (byteData[i] == STX) idFirstStx = i;
                    int resultLength = lastIndexOfCrlf - idFirstStx - 5;
                    var tempResult = new byte[resultLength];

                    Array.Copy(byteData, idFirstStx + 2, tempResult, 0, resultLength);
                    result = GetTextAutoEncoding(tempResult);
                    //result = Encoding.UTF8.GetString(tempResult);
                }
                    //Nếu có lỗi xảy ra khi truyền dữ liệu trả về chuỗi lỗi mặc định
                else
                {
                    Console.WriteLine(@"CheckSum Error");
                    result = string.Empty;
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }

        public static string GetCheckSumValueCompactX(byte[] byteData)
        {
            string checksum = "00";

            int sumOfChars = 0;
            bool complete = false;

            //take each byte in the string and add the values
            for (int idx = Array.IndexOf(byteData, (byte)DeviceHelper.RS); idx <= Array.IndexOf(byteData, (byte)DeviceHelper.GS); idx++)
            {
                sumOfChars += byteData[idx];

                //switch (byteVal)
                //{
                //    case (byte)STX:
                //        sumOfChars = 0;
                //        break;
                //    case (byte)ETX:
                //    case (byte)ETB:
                //        sumOfChars += byteVal;
                //        complete = true;
                //        break;
                //    default:
                //        sumOfChars += byteVal;
                //        break;
                //}

                //if (complete)
                //    break;
            }

            if (sumOfChars > 0)
            {
                //hex value mod 256 is checksum, return as hex value in upper case
                checksum = Convert.ToString(sumOfChars % 256, 16).ToUpper();
            }

            //if checksum is only 1 char then prepend a 0
            return (checksum.Length == 1 ? "0" + checksum : checksum);
        }

        /// <summary>
        ///  Lấy về chuỗi sau khi đã xử lý CheckSum
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static string GetStringAfterCheckSumCompactX(byte[] byteData)
        {
            string result;
            try
            {
                //Tính giá trị CheckSum của chuỗi truyền vào
                string csValue = GetCheckSumValueCompactX(byteData);
                Console.WriteLine(csValue);

                //Gán biến CheckSum 

                int gsIdx = Array.IndexOf(byteData, (byte) DeviceHelper.GS);
                int rsIdx = Array.IndexOf(byteData, (byte) DeviceHelper.RS);
                string checkSum =
                    System.Text.Encoding.ASCII.GetString(new byte[] {byteData[gsIdx + 1], byteData[gsIdx + 2]});

                result = csValue == checkSum ? Encoding.ASCII.GetString(byteData, rsIdx, gsIdx - rsIdx) : string.Empty;
                
            }
            catch (Exception)
            {
                result = string.Empty;
            }
            return result;
        }


        public static void UpdateLogotoDatatable(ref DataTable dataTable)
        {
            //Nếu chưa tồn tại thì gán thêm trường
            if (!dataTable.Columns.Contains("LOGO"))
                dataTable.Columns.Add("LOGO", typeof (byte[]));

            //File Logo nằm trong thư mục logo
            string filename = string.Format(@"{0}\logo\logo.jpg", Application.StartupPath);
            if (!File.Exists(filename)) return;

            byte[] byteArray = ConvertImageToByteArray(Image.FromFile(filename), ImageFormat.Tiff);
            //Nếu có lỗi trong quá trình đọc file hoặc dữ liệu trả ra null thoát lun :(
            if (byteArray == null) return;
            foreach (DataRow dr in dataTable.Rows) dr["LOGO"] = byteArray;
            dataTable.AcceptChanges();
        }

        /// <summary>
        ///     Đọc ảnh vào mảng byte
        /// </summary>
        /// <param name="imageToConvert"></param>
        /// <param name="formatOfImage"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToByteArray(Image imageToConvert, ImageFormat formatOfImage)
        {
            byte[] ret;
            try
            {
                using (var ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    ret = ms.ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
            return ret;
        }

        /// <summary>
        ///     Lấy về thông tin cấu hình của thiết bị
        /// </summary>
        /// <param name="machineName">Tên thiết bị</param>
        /// <param name="testTypeId">Mã loại xét nghiệm</param>
        /// <param name="tblParaName">Bảng lưu các mã điều khiển thiết bị</param>
        /// <param name="deviceId">trả về mã của thiết bị XN</param>
        public static void GetDeviceConfig(string machineName, ref int testTypeId, ref DataTable tblParaName,
            ref decimal deviceId)
        {
            try
            {
                DataTable tempTable = new Select(DDeviceList.Columns.TestTypeId, DDeviceList.Columns.DeviceId).From(
                    DDeviceList.Schema.TableName)
                    .Where(string.Format("UPPER({0})", DDeviceList.Columns.DeviceName))
                    .IsEqualTo(
                        machineName.ToUpper()).ExecuteDataSet().Tables[0];

                if (tempTable.Rows.Count > 0)
                {
                    DataRow dr = tempTable.Rows[0];
                    //Lấy về TestTypeID
                    testTypeId = Convert.ToInt32(dr[DDeviceList.Columns.TestTypeId].ToString());
                    //Lấy về DeviceID
                    deviceId = Convert.ToInt32(dr[DDeviceList.Columns.DeviceId]);
                    //Lấy về bảng mã điều khiển thiết bị
                    try
                    {
                        tblParaName = GetParaNameFromDeviceId(deviceId);
                    }
                    catch (Exception)
                    {
                        tblParaName =
                            new Select(DDataControl.Columns.DataControlId, DDataControl.Columns.DeviceId,
                                DDataControl.Columns.DataTypeId, DDataControl.Columns.DataSequence,
                                DDataControl.Columns.ControlType, DDataControl.Columns.DataName,
                                DDataControl.Columns.AliasName, DDataControl.Columns.MeasureUnit,
                                DDataControl.Columns.DataPoint, DDataControl.Columns.NormalLevel,
                                DDataControl.Columns.NormalLevelW, DDataControl.Columns.DataPrint,
                                DDataControl.Columns.DataType, DDataControl.Columns.Description)
                                .From(DDataControl.Schema.TableName).Where(DDataControl.Columns.DeviceId).IsEqualTo(
                                    deviceId).ExecuteDataSet().Tables[0];
                    }
                    //Nếu bảng thiết bị không có trường TestType_ID thêm trường mới và gán giá trị
                    if (!tblParaName.Columns.Contains(DDeviceList.Columns.TestTypeId))
                        tblParaName.Columns.Add(DDeviceList.Columns.TestTypeId, typeof (int),
                            testTypeId.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    testTypeId = -1;
                    tblParaName = null;
                    deviceId = -1;
                }
            }
            catch (Exception ex)
            {
                testTypeId = -1;
                tblParaName = null;
                deviceId = -1;
                throw ex;
            }
        }

        public static DataTable GetParaNameFromDeviceId(decimal deviceId)
        {
            return SPs.SpGetDataControlByDeviceId((int) deviceId).GetDataSet().Tables[0];
        }

        /// <summary>
        ///     Cập nhật kết quả của bệnh nhân
        /// </summary>
        /// <param name="testTypeId">Mã loại xét nghiệm</param>
        /// <param name="result"></param>
        /// <param name="tblParaName">Bảng mã điều khiển thiết bị</param>
        /// <param name="deviceId"></param>
        public static bool ImportResultToDb(short testTypeId, Result result, DataTable tblParaName)
        {
            string useMultiInsertResultToDb = LablinkServiceConfig.GetUseMultiInsertResultToDB();
            return useMultiInsertResultToDb.Equals("True") || useMultiInsertResultToDb.Equals("1")
                ? ImportMultiResult(testTypeId, result, tblParaName)
                : ImportSingleResult(testTypeId, result, tblParaName);
        }

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tblParaName"></param>
        public static void PrepareCalculatedResults(ref Result result, DataTable tblParaName)
        {
            try
            {
                var newResult = new ResultItem();
                foreach (DataRow dr in tblParaName.Rows)
                {
                    string sConditionAll = Utility.sDbnull(dr[DDataControl.Columns.SCondition], "-1");
                    switch (sConditionAll)
                    {
                        case "-1":
                            break;
                        case "AUTO":
                            SetNewResult(dr, ref newResult);
                            newResult.TestValue = ApplyValue(Utility.sDbnull(dr[DDataControl.Columns.SFormula]), result);
                            break;
                        default:
                            string[] arrCondition = sConditionAll.Split(';');
                            string[] arrFormula = Utility.sDbnull(dr[DDataControl.Columns.SFormula]).Split(';');
                            bool IsNewResult = false;
                            for (int i = 0; i < arrCondition.Length; i++)
                            {
                                string sConditionWithValue = ApplyValue(arrCondition[i], result);
                                if (sConditionWithValue != "N/A" &&
                                    ConditionIsValid(sConditionWithValue) != "ConditionIsNotOK")
                                {
                                    SetNewResult(dr, ref newResult);
                                    newResult.TestValue = ApplyValue(arrFormula[i], result);
                                    result.Items.Add(newResult);
                                    IsNewResult = true;
                                    break;
                                }
                            }
                            if (!IsNewResult && arrCondition.Length < arrFormula.Length)
                            {
                                SetNewResult(dr, ref newResult);
                                newResult.TestValue = ApplyValue(arrFormula[arrFormula.Length - 1], result);
                                result.Items.Add(newResult);
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SetNewResult(DataRow dr, ref ResultItem newResult)
        {
            try
            {
                newResult = new ResultItem();
                newResult.TestName = Utility.sDbnull(dr[DDataControl.Columns.AliasName]);
                newResult.TestTypeId = Utility.Int16Dbnull(dr["TestType_ID"], -1);
                newResult.NormalLevel = Utility.sDbnull(dr[DDataControl.Columns.NormalLevel]);
                newResult.NormalLevelW = Utility.sDbnull(dr[DDataControl.Columns.NormalLevelW]);
                newResult.MeasureUnit = Utility.sDbnull(dr[DDataControl.Columns.MeasureUnit]);
                newResult.DataSequence = Utility.Int32Dbnull(dr[DDataControl.Columns.AliasName], -1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Lấy giá trị đã nhận được từ thiết bị thay vào công thức
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <param name="tblParaName"></param>
        /// <returns></returns>
        private static string ApplyValue(string str, Result result)
        {
            try
            {
                string vResult = str;
                while (vResult.IndexOf('{') >= 0)
                {
                    int startIndex = vResult.IndexOf('{');
                    int endIndex = vResult.IndexOf('}');
                    string vAliasName = vResult.Substring(startIndex + 1, endIndex - startIndex - 1);
                    string vTestValue = string.Empty;
                    foreach (ResultItem item in result.Items)
                        if (item.TestName.ToUpper() == vAliasName.ToUpper())
                        {
                            vTestValue = item.TestValue;
                            break;
                        }
                    if (vTestValue == string.Empty) return "N/A";
                    vResult = vResult.Remove(startIndex, endIndex - startIndex + 1);
                    vResult = vResult.Insert(startIndex, vTestValue);
                }
                return vResult;
            }
            catch (Exception)
            {
                return "N/A";
            }
        }

        /// <summary>
        ///     Kiểm tra nếu điều kiện thỏa mãn
        /// </summary>
        /// <param name="sCondition"></param>
        /// <returns></returns>
        private static string ConditionIsValid(string sCondition)
        {
            string vResultNotOK = "ConditionIsNotOK";
            try
            {
                string strOperator = "=,>,<";
                string myOperator = "";
                int idx = 0;
                string rightCondition;
                while (idx < sCondition.Length)
                {
                    if (strOperator.IndexOf(sCondition[idx]) < 0) idx += 1;
                    else
                    {
                        int tempIdx = idx;
                        while (tempIdx < sCondition.Length && strOperator.IndexOf(sCondition[tempIdx]) >= 0)
                        {
                            myOperator += sCondition[tempIdx];
                            tempIdx += 1;
                        }
                        break;
                    }
                }
                if (myOperator == "=")
                {
                    if (sCondition.Substring(0, idx).ToUpper() ==
                        sCondition.Substring(idx + 1, sCondition.Length - idx - 1).ToUpper())
                        return sCondition.Substring(idx + 1, sCondition.Length - idx - 1);
                    return vResultNotOK;
                }
                if (myOperator == ">=")
                {
                    rightCondition = ConditionIsValid(sCondition.Substring(idx + 2));
                    if (rightCondition == vResultNotOK) return vResultNotOK;
                    if (Utility.DecimaltoDbnull(sCondition.Substring(0, idx - 1), 1) >=
                        Utility.DecimaltoDbnull(rightCondition, 2))
                        return sCondition.Substring(0, idx);
                    return vResultNotOK;
                }
                if (myOperator == "<=")
                {
                    rightCondition = ConditionIsValid(sCondition.Substring(idx + 2));
                    if (rightCondition == vResultNotOK) return vResultNotOK;
                    if (Utility.DecimaltoDbnull(sCondition.Substring(0, idx - 1), 2) <=
                        Utility.DecimaltoDbnull(rightCondition, 1))
                        return sCondition.Substring(0, idx);
                    return vResultNotOK;
                }
                if (myOperator == ">")
                {
                    rightCondition = ConditionIsValid(sCondition.Substring(idx + 1));
                    if (rightCondition == vResultNotOK) return vResultNotOK;
                    if (Utility.DecimaltoDbnull(sCondition.Substring(0, idx - 1), 1) >
                        Utility.DecimaltoDbnull(rightCondition, 2))
                        return sCondition.Substring(0, idx);
                    return vResultNotOK;
                }
                if (myOperator == "<")
                {
                    rightCondition = ConditionIsValid(sCondition.Substring(idx + 1));
                    if (rightCondition == vResultNotOK) return vResultNotOK;
                    if (Utility.DecimaltoDbnull(sCondition.Substring(0, idx - 1), 2) <
                        Utility.DecimaltoDbnull(rightCondition, 1))
                        return sCondition.Substring(0, idx);
                    return vResultNotOK;
                }
                return sCondition;
            }
            catch (Exception)
            {
                return vResultNotOK;
            }
        }

        /// <summary>
        ///     Cập nhật kết quả của bệnh nhân
        /// </summary>
        /// <param name="testTypeId">Mã loại xét nghiệm</param>
        /// <param name="result"></param>
        /// <param name="tblParaName">Bảng mã điều khiển thiết bị</param>
        public static bool ImportSingleResult(short testTypeId, Result result, DataTable tblParaName)
        {
            try
            {
                if ((result == null) || (result.Items.Count <= 0)) return true;
                lock (result)
                {
                    long patientId = 0;

                    // Lấy về số lần test là số đằng sau dấu chấm "."
                    string testNo = "";
                    if (result.Barcode.Contains('.'))
                        testNo = result.Barcode.Substring(result.Barcode.LastIndexOf('.') + 1).Trim();
                    if (testNo == "1") testNo = "";

                    string[] tempTestDateString = result.TestDate.Split('/');

                    DateTime testDate;
                    try
                    {
                        testDate = new DateTime(Convert.ToInt32(tempTestDateString[2]),
                            Convert.ToInt32(tempTestDateString[1]),
                            Convert.ToInt32(tempTestDateString[0]));
                    }
                    catch (Exception)
                    {
                        testDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        result.TestDate = testDate.ToString("dd/MM/yyyy");
                    }

                    //Hiệu chỉnh kết quả trước khi insert
                    PrepareResult(ref result, tblParaName, testTypeId);

                    // Lấy về ngày làm xét nghiệm
                    string tempBarcode = result.Barcode;


                    // Lấy về tất cả các loại xét nghiệm của kết quả
                    IEnumerable<short> arrTestTypeId = (from r in result.Items.Cast<ResultItem>()
                        select r.TestTypeId).Distinct();

                    // Lấy về bảng đăng ký xét nghiệm
                    TTestInfoCollection dtTestInfo = GetTestInfo(ref tempBarcode, ref patientId, arrTestTypeId,
                        testDate,
                        result.TestSequence);

                    // lấy thêm Test Info từ T_regList
                    var dtTestInfoFromRegList =
                        new Select(TRegList.Columns.TestId, TRegList.Columns.Barcode, TRegList.Columns.TestDataId)
                            .From(TRegList.Schema.Name)
                            .Where(TRegList.Columns.Barcode)
                            .IsEqualTo(tempBarcode)
                            .ExecuteAsCollection<TRegListCollection>();

                    result.Barcode = tempBarcode;
                    result.PatientId = patientId;

                    // Lấy ra toàn bộ kết quả của Barcode này
                    var allResult =
                        new Select().From(TResultDetail.Schema.Name).Where(TResultDetail.Columns.Barcode).IsEqualTo(
                            result.Barcode).ExecuteAsCollection<TResultDetailCollection>();

                    //using (var scope = new TransactionScope())
                    //{
                    //    using (new SharedDbConnectionScope())
                    foreach (ResultItem item in result.Items)
                    {
                        // Nếu tên kết quả ra rỗng thì không truyền
                        if (item.TestName.Trim().Equals("")) continue;

                        // Nếu tham số này không cho in thì không truyền ra
                        bool printStatus;
                        try
                        {
                            printStatus = bool.Parse((from d in tblParaName.AsEnumerable()
                                where
                                    d["TestData_ID"].ToString()
                                        .Equals(item.TestDataId)
                                select d["Data_Print"]).FirstOrDefault().ToString());
                        }
                        catch (Exception)
                        {
                            printStatus = true;
                        }
                        if (item.TestValue.ToUpper().Contains(".PNG")) printStatus = true;
                        if (!printStatus) continue;

                        // Xử lý kết quả cho Nội Tiết
                        if (_barcodeType.ToUpper() == "NOITIET")
                        {
                            // xử lý kết quả TSH
                            if (item.TestName.ToUpper().Contains("TSH"))
                                if (IsNumber(item.TestValue))
                                    if (Convert.ToDouble(item.TestValue) < 0.03) item.TestValue = "0.03";

                            // Xử lý kết quả HIV
                            if (item.TestName.ToUpper().Contains("HBSAG"))
                            {
                                if (IsNumber(item.TestValue))
                                    item.TestValue =
                                        string.Format(
                                            Convert.ToDouble(item.TestValue) < 2
                                                ? "{0} - Âm Tính"
                                                : "{0} - Dương Tính", item.TestValue);
                            }

                            // Xử lý kết quả LDL-C
                            if (item.TestName.ToUpper().Contains("LDL"))
                            {
                                // Tìm kiếm kết quả Tri
                                string xxx = (from r in result.Items.Cast<ResultItem>()
                                    where r.TestName.ToUpper().Contains("TRI")
                                    select r.TestValue).FirstOrDefault() ?? "";
                                // Nếu tìm thấy kết quả TRI
                                if (!string.IsNullOrEmpty(xxx))
                                    if (IsNumber(xxx))
                                        if (Convert.ToDouble(xxx) > 4.5) item.TestValue = "HT Đục";
                            }

                            // Xử lý kết quả nghiệm pháp cho GLuco
                            if (item.TestDataId.ToUpper().Contains("GLU"))
                                if (!string.IsNullOrEmpty(testNo))
                                {
                                    if (testNo.Equals("2"))
                                    {
                                        item.TestDataId = "GLUC";
                                    }
                                    else if (testNo.Equals("3"))
                                    {
                                        item.TestDataId = "GLUCO";
                                    }
                                    DataRow dr = (from r in tblParaName.AsEnumerable()
                                        where r[LStandardTest.Columns.TestDataId].Equals(item.TestDataId)
                                        select r).FirstOrDefault();
                                    item.DataSequence = Utility.Int32Dbnull(dr[LStandardTest.Columns.DataSequence]);
                                    item.MeasureUnit = Utility.sDbnull(dr[LStandardTest.Columns.MeasureUnit]);
                                    item.NormalLevel = Utility.sDbnull(dr[LStandardTest.Columns.NormalLevel]);
                                    item.NormalLevelW = Utility.sDbnull(dr[LStandardTest.Columns.NormalLevelW]);
                                    item.TestName = Utility.sDbnull(dr[LStandardTest.Columns.DataName]);
                                    item.TestTypeId = Utility.Int16Dbnull(dr[LStandardTest.Columns.TestTypeId]);
                                }
                            // Xử lý kết quả nghiệm pháp cho CORT
                            if ((item.TestName.ToUpper().Contains("CORT")) ||
                                (item.TestDataId.ToUpper().Contains("CORT")))
                                if (!string.IsNullOrEmpty(testNo))
                                    item.TestName = string.Format("{0}.M{1}", item.TestName, testNo);
                            if (item.TestName.ToUpper().Contains("A-TPO"))
                            {
                                if (IsNumber(item.TestValue))
                                    if (Convert.ToDouble(item.TestValue) < 5) item.TestValue = "5.0";
                            }
                        }
                        TResultDetail re = (from r in allResult
                            where
                                (r.TestTypeId == item.TestTypeId) &&
                                (r.ParaName == item.TestName)
                            select r).FirstOrDefault() ?? new TResultDetail();
                        // Nếu tồn tại thì update lại kết quả

                        decimal tempTestId;

                        decimal tempTempTestId = (from t in dtTestInfoFromRegList
                            where
                                (t.Barcode == result.Barcode) && (t.TestDataId == item.TestDataId)
                            select t.TestId).FirstOrDefault();
                        if (tempTempTestId == 0)
                        {
                            tempTestId = (from t in dtTestInfo
                                where
                                    (t.Barcode == result.Barcode) && (t.TestTypeId == item.TestTypeId)
                                select t.TestId).FirstOrDefault();
                        }
                        else
                        {
                            tempTestId = tempTempTestId;
                        }


                        re.TestId = tempTestId;
                        re.PatientId = result.PatientId;
                        re.TestTypeId = item.TestTypeId;
                        re.TestDate = testDate;
                        re.DataSequence = item.DataSequence;
                        re.TestResult = item.TestValue;
                        re.NormalLevel = item.NormalLevel;
                        re.NormalLevelW = item.NormalLevelW;
                        re.MeasureUnit = item.MeasureUnit;
                        re.ParaName = item.TestName;
                        re.PrintData = true;
                        re.Barcode = result.Barcode;
                        re.TestDataId = item.TestDataId;
                        re.DeviceId = result.DeviceId;
                        re.UpdateNum = Utility.Int32Dbnull(re.UpdateNum, 0) + 1;
                        re.ParaStatus = 0;

                        _resultList.Add(re);
                        if (!_resultWorker.IsBusy) _resultWorker.RunWorkerAsync();

                        //re.Save();
                    }
                    //scope.Complete();
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error While Import Data to DB:\n {0}", ex));
                return false;
            }
        }

        /// <summary>
        ///     Cập nhật kết quả của bệnh nhân
        /// </summary>
        /// <param name="testTypeId">Mã loại xét nghiệm</param>
        /// <param name="result"></param>
        /// <param name="tblParaName">Bảng mã điều khiển thiết bị</param>
        /// <param name="deviceId"></param>
        public static bool ImportMultiResult(short testTypeId, Result result, DataTable tblParaName)
        {
            try
            {
                // Chuẩn hóa lại kết quả
                PrepareResult(ref result, tblParaName, testTypeId);

                string strResult = "";
                for (int i = 0; i < result.Items.Count; i++)
                {
                    ResultItem item = result.Items[i];
                    strResult = i != result.Items.Count - 1
                        ? strResult + string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}{1}{7}{1}{8}{1}{9}",
                            item.TestName, "|", item.TestValue, item.TestName,
                            item.DataSequence, item.NormalLevel, item.NormalLevelW,
                            item.MeasureUnit, 1, "¬")
                        : strResult + string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}{1}{7}{1}{8}{1}",
                            item.TestName, "|", item.TestValue, item.TestName,
                            item.DataSequence, item.NormalLevel, item.NormalLevelW,
                            item.MeasureUnit, 1);
                }
                SPs.SpInsertMultiResultDetail(testTypeId, result.Barcode, strResult, (int) result.DeviceId, 1).Execute();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     Hàm trả về giá trị tháng của chữ truyền vào
        ///     (JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC)
        /// </summary>
        /// <param name="pMonth"></param>
        /// <returns></returns>
        public static string GetMonth(string pMonth)
        {
            switch (pMonth.ToUpper().Trim())
            {
                case "JAN":
                    pMonth = "01";
                    break;
                case "FEB":
                    pMonth = "02";
                    break;
                case "MAR":
                    pMonth = "03";
                    break;
                case "APR":
                    pMonth = "04";
                    break;
                case "MAY":
                    pMonth = "05";
                    break;
                case "JUN":
                    pMonth = "06";
                    break;
                case "JUL":
                    pMonth = "07";
                    break;
                case "AUG":
                    pMonth = "08";
                    break;
                case "SEP":
                    pMonth = "09";
                    break;
                case "OCT":
                    pMonth = "10";
                    break;
                case "NOV":
                    pMonth = "11";
                    break;
                case "DEC":
                    pMonth = "12";
                    break;
            }
            return pMonth;
        }

        /// <summary>
        /// </summary>
        /// <param name="pDeviceId"></param>
        /// <param name="pBarcode"></param>
        /// <returns></returns>
        public static List<string> GetRegList(int pDeviceId, string pBarcode, ref string pPatientName)
        {
            var result = new List<string>();
            pPatientName = "";
            try
            {
                // Lấy về danh sách chỉ định
                DataSet tempds = GetRegDataNew(pDeviceId, pBarcode);
                if (tempds != null)
                {
                    DataTable temptable = tempds.Tables[0];
                    if (temptable != null)
                        result.AddRange(from DataRow row in temptable.Rows
                            select row[TRegList.Columns.AliasName].ToString());
                    try
                    {
                        DataTable temptable2 = tempds.Tables[1];
                        if (temptable2 != null)
                            pPatientName = (from DataRow row in temptable2.Rows
                                select row[LPatientInfo.Columns.PatientName].ToString()).FirstOrDefault() ?? "";
                        pPatientName = GetUnsignString(pPatientName);
                    }
                    catch (Exception)
                    {
                        pPatientName = "";
                    }
                }

                //Lấy ra tên bệnh nhân

                //Nếu có update lại status:
                try
                {
                    if (Boolean.Parse(LablinkServiceConfig.GetAutoUpdateOrderStatusAfterSend()))
                    {
                        DataTable temptable = tempds.Tables[0];
                        foreach (DataRow dr in temptable.Rows)
                        {
                            var item = new TRegList(dr[TRegList.Columns.TestRegDetailId]);
                            item.Status = 1;
                            item.Save();
                        }
                    }
                }
                catch (Exception)
                {
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Lấy về đường dẫn lưu ảnh trên server
        /// </summary>
        /// <returns></returns>
        public static string GetImageFolder()
        {
            try
            {
                object result =
                    new Select(TblSystemParameter.Columns.SValue).From(TblSystemParameter.Schema.Name).Where(
                        TblSystemParameter.Columns.SName).IsEqualTo("IMAGE_FOLDER").ExecuteScalar();
                if (result == null) return string.Empty;
                return result.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static bool IsNumber(string val)
        {
            try
            {
                double loz = Convert.ToDouble(val);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Hàm vẽ histogram
        /// </summary>
        /// <param name="histogramType">Loại biểu đồ</param>
        /// <param name="boxWidth">Chiều rộng của ảnh</param>
        /// <param name="boxHeight">Chiều cao của ảnh</param>
        /// <param name="input">Chuỗi ký tự nhập vào</param>
        /// <param name="xpoint">Tọa độ các điểm trên trục X</param>
        /// <returns></returns>
        public static Bitmap CreateHistogram(string histogramType, int boxWidth, int boxHeight, string input)
        {
            try
            {
                var bmp = new Bitmap(boxWidth, boxHeight);
                var picFont = new Font("Arial", 8, FontStyle.Bold);
                var textFont = new Font("Arial", 7);
                Graphics g = Graphics.FromImage(bmp);
                const int axisGapBox = 20;
                // Kích thước của toàn bộ hình
                int picWidth = boxWidth - 2*axisGapBox;
                int picHeight = boxHeight - 2*axisGapBox;
                var rect = new Rectangle(0, 0, boxWidth - 1, boxHeight - 1);
                Pen blackPen = Pens.Black;
                Pen redPen = Pens.Red;

                // draw black background
                g.Clear(Color.White);
                g.DrawRectangle(blackPen, rect);

                int index = histogramType == "PLT" ? 14 : 0;
                Point[] points = (from Match m in Regex.Matches(input, "..")
                    select new Point(index++, Convert.ToInt32(m.Value, 16))).ToArray();

                float drawingWidth = histogramType == "PLT" ? 150 : 350;

                float drawingHeight = points.Max(p => p.Y) - points.Min(p => p.Y);

                // Calculate the scale aspect we need to apply to points.
                float scaleX = picWidth/drawingWidth;
                float scaleY = picHeight/drawingHeight;

                var matrix = new Matrix();
                matrix.Scale(scaleX, scaleY);
                matrix.TransformPoints(points);

                // Vẽ các điểm 50,100,200,300
                Point[] xpoint = {};
                if (histogramType == "WBC")
                    xpoint = new[] {new Point(50, 0), new Point(100, 0), new Point(200, 0), new Point(300, 0)};
                else if (histogramType == "RBC")
                    xpoint = new[] {new Point(80, 0), new Point(110, 0), new Point(200, 0), new Point(300, 0)};
                else if (histogramType == "PLT")
                    xpoint = new[] {new Point(2, 0), new Point(10, 0), new Point(20, 0), new Point(30, 0)};

                string[] keyArray = (from p in xpoint
                    select p.X.ToString(CultureInfo.InvariantCulture)).ToArray();
                matrix.TransformPoints(xpoint);
                var xdic = new Dictionary<string, Point>();
                for (int i = 0; i < xpoint.Length; i++)
                    xdic.Add(keyArray[i], xpoint[i]);

                // Vẽ chữ tiêu đề
                string PicText = string.Format("{0} Histogram", histogramType);
                float TextPosition = (float) Math.Truncate(boxWidth - g.MeasureString(PicText, picFont).Width)/2 + 1;
                Brush blackBrush = Brushes.Black;
                g.DrawString(PicText, picFont, blackBrush, TextPosition, 1);
                g.DrawString("fL", picFont, blackBrush, boxWidth - 15, boxHeight - 15);

                GraphicsContainer containerState = g.BeginContainer();
                // Đảo ngược trục Y
                g.ScaleTransform(1.0F, -1.0F);

                // Dịch chuyển gốc tọa độ
                g.TranslateTransform(axisGapBox, -(float) boxHeight + axisGapBox);

                //Vẽ các trục tọa độ
                g.DrawLine(blackPen, 0, 0, 0, picHeight);
                g.DrawLine(blackPen, 0, 0, picWidth, 0);

                // Vẽ đường gióng

                //foreach (var point in xdic)
                //{
                //    g.DrawLine(blackPen, point.Value.X, -3, point.Value.X, 3);
                //    g.ScaleTransform(1.0F, -1.0F);
                //    g.DrawString(point.Key, textFont, blackBrush, point.Value.X-10, 5);
                //    g.ScaleTransform(1.0F, -1.0F);
                //}
                //if(histogramType=="RBC")
                //{
                //    g.DrawLine(blackPen, xdic["80"].X, 0, xdic["80"].X, picHeight);   
                //    g.DrawLine(blackPen, xdic["110"].X, 0, xdic["110"].X, picHeight);   
                //}

                // Vẽ hình
                //g.TranslateTransform(20, 0);
                g.DrawCurve(redPen, points);

                g.EndContainer(containerState);
                Invert(ref bmp);
                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void Invert(ref Bitmap bmp)
        {
            try
            {
                Bitmap temp = bmp;
                var bmap = (Bitmap) temp.Clone();
                for (int i = 0; i < bmap.Width; i++)
                {
                    for (int j = 0; j < bmap.Height; j++)
                    {
                        Color c = bmap.GetPixel(i, j);
                        bmap.SetPixel(i, j,
                            Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                    }
                }
                bmp = (Bitmap) bmap.Clone();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region String Utilities

        /// <summary>
        ///     Hàm ngắt chuỗi thành các mảng nhỏ với ký tự bắt đầu và ký tự kết thúc biết trước
        /// </summary>
        /// <param name="rawStringData">Chuỗi truyền vào</param>
        /// <param name="beginChar">Ký tự bắt đầu</param>
        /// <param name="endChar">Ký tự kết thúc</param>
        /// <returns>Mảng dữ liệu trả ra</returns>
        public static string[] SeperatorRawData(string rawStringData, char beginChar, char endChar)
        {
            var allResult = new string[] {};
            try
            {
                bool isNewString = false;
                int id = -1;
                foreach (char chr in rawStringData)
                {
                    //Nếu gặp ký tự beginChar thì khởi tạo chuỗi mới
                    if (chr == beginChar)
                    {
                        id++;
                        Array.Resize(ref allResult, id + 1);
                        isNewString = true;
                    }

                    //Nếu gặp ký tự endChar thì kết thúc nhận chuỗi
                    if (chr == endChar)
                    {
                        allResult[id] += chr;
                        isNewString = false;
                    }

                    //Nếu là ký tự bình thường thì ghép vào chuỗi đang xử lý trong điều kiện isNewString=true
                    if (isNewString) allResult[id] += chr;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return allResult;
        }

        public static string[] DeleteAllBlankLine(IEnumerable<string> lines)
        {
            return DeleteAllBlankLine(lines, true);
        }

        /// <summary>
        ///     Xóa các dòng trắng của dữ liệu
        /// </summary>
        /// <param name="lines">Mảng các chuỗi truyền vào</param>
        /// <param name="trim"> </param>
        /// <returns></returns>
        public static string[] DeleteAllBlankLine(IEnumerable<string> lines, bool trim)
        {
            string[] sReturn = null;
            int i = -1;
            foreach (string s in lines)
            {
                if (!String.IsNullOrEmpty(s.Trim()))
                {
                    i = i + 1;
                    Array.Resize(ref sReturn, i + 1);
                    sReturn[i] = trim ? s.Trim() : s;
                }
            }
            return sReturn;
        }

        /// <summary>
        ///     Xóa các dòng trắng của dữ liệu
        /// </summary>
        /// <param name="pString">Chuỗi dữ liệu truyền vào</param>
        /// <param name="pSeparateChar">Ký tự ngăn cách dòng</param>
        /// <returns> string[] Dữ liệu sau khi xóa dòng trắng</returns>
        public static string[] DeleteAllBlankLine(string pString, char[] pSeparateChar)
        {
            string[] pTempString = DeleteAllBlankLine(pString.Split(pSeparateChar));
            return pTempString;
        }

        public static string[] DeleteAllBlankLine(string pString, char pSeparateChar)
        {
            string[] pTempString = DeleteAllBlankLine(pString.Split(pSeparateChar));
            return pTempString;
        }

        public static string[] DeleteAllBlankLine(string pString, string pSeparateChar)
        {
            string[] pTempString = DeleteAllBlankLine(pString.Split(pSeparateChar.ToCharArray()));
            return pTempString;
        }

        public static string[] DeleteAllBlankLine(string pString, string pSeparateChar, bool pTrim)
        {
            string[] pTempString = DeleteAllBlankLine(pString.Split(pSeparateChar.ToCharArray()), pTrim);
            return pTempString;
        }

        /// <summary>
        ///     Hàm bỏ dấu tiếng Việt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetUnsignString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        //public Encoding detectTextEncoding(string filename, out String text, int taster = 1000)
        //{
        //    byte[] b = File.ReadAllBytes(filename);

        //    //////////////// First check the low hanging fruit by checking if a
        //    //////////////// BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
        //    if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF) { text = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4); return Encoding.GetEncoding("utf-32BE"); }  // UTF-32, big-endian 
        //    else if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00) { text = Encoding.UTF32.GetString(b, 4, b.Length - 4); return Encoding.UTF32; }    // UTF-32, little-endian
        //    else if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF) { text = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2); return Encoding.BigEndianUnicode; }     // UTF-16, big-endian
        //    else if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE) { text = Encoding.Unicode.GetString(b, 2, b.Length - 2); return Encoding.Unicode; }              // UTF-16, little-endian
        //    else if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF) { text = Encoding.UTF8.GetString(b, 3, b.Length - 3); return Encoding.UTF8; } // UTF-8
        //    else if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76) { text = Encoding.UTF7.GetString(b, 3, b.Length - 3); return Encoding.UTF7; } // UTF-7


        //    //////////// If the code reaches here, no BOM/signature was found, so now
        //    //////////// we need to 'taste' the file to see if can manually discover
        //    //////////// the encoding. A high taster value is desired for UTF-8
        //    if (taster == 0 || taster > b.Length) taster = b.Length;    // Taster size can't be bigger than the filesize obviously.


        //    // Some text files are encoded in UTF8, but have no BOM/signature. Hence
        //    // the below manually checks for a UTF8 pattern. This code is based off
        //    // the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
        //    // For our purposes, an unnecessarily strict (and terser/slower)
        //    // implementation is shown at: http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
        //    // For the below, false positives should be exceedingly rare (and would
        //    // be either slightly malformed UTF-8 (which would suit our purposes
        //    // anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
        //    int i = 0;
        //    bool utf8 = false;
        //    while (i < taster - 4)
        //    {
        //        if (b[i] <= 0x7F) { i += 1; continue; }     // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
        //        if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0) { i += 2; utf8 = true; continue; }
        //        if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0) { i += 3; utf8 = true; continue; }
        //        if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 && b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0) { i += 4; utf8 = true; continue; }
        //        utf8 = false; break;
        //    }
        //    if (utf8 == true)
        //    {
        //        text = Encoding.UTF8.GetString(b);
        //        return Encoding.UTF8;
        //    }


        //    // The next check is a heuristic attempt to detect UTF-16 without a BOM.
        //    // We simply look for zeroes in odd or even byte places, and if a certain
        //    // threshold is reached, the code is 'probably' UF-16.          
        //    double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
        //    int count = 0;
        //    for (int n = 0; n < taster; n += 2) if (b[n] == 0) count++;
        //    if (((double)count) / taster > threshold) { text = Encoding.BigEndianUnicode.GetString(b); return Encoding.BigEndianUnicode; }
        //    count = 0;
        //    for (int n = 1; n < taster; n += 2) if (b[n] == 0) count++;
        //    if (((double)count) / taster > threshold) { text = Encoding.Unicode.GetString(b); return Encoding.Unicode; } // (little-endian)


        //    // Finally, a long shot - let's see if we can find "charset=xyz" or
        //    // "encoding=xyz" to identify the encoding:
        //    for (int n = 0; n < taster - 9; n++)
        //    {
        //        if (
        //            ((b[n + 0] == 'c' || b[n + 0] == 'C') && (b[n + 1] == 'h' || b[n + 1] == 'H') && (b[n + 2] == 'a' || b[n + 2] == 'A') && (b[n + 3] == 'r' || b[n + 3] == 'R') && (b[n + 4] == 's' || b[n + 4] == 'S') && (b[n + 5] == 'e' || b[n + 5] == 'E') && (b[n + 6] == 't' || b[n + 6] == 'T') && (b[n + 7] == '=')) ||
        //            ((b[n + 0] == 'e' || b[n + 0] == 'E') && (b[n + 1] == 'n' || b[n + 1] == 'N') && (b[n + 2] == 'c' || b[n + 2] == 'C') && (b[n + 3] == 'o' || b[n + 3] == 'O') && (b[n + 4] == 'd' || b[n + 4] == 'D') && (b[n + 5] == 'i' || b[n + 5] == 'I') && (b[n + 6] == 'n' || b[n + 6] == 'N') && (b[n + 7] == 'g' || b[n + 7] == 'G') && (b[n + 8] == '='))
        //            )
        //        {
        //            if (b[n + 0] == 'c' || b[n + 0] == 'C') n += 8; else n += 9;
        //            if (b[n] == '"' || b[n] == '\'') n++;
        //            int oldn = n;
        //            while (n < taster && (b[n] == '_' || b[n] == '-' || (b[n] >= '0' && b[n] <= '9') || (b[n] >= 'a' && b[n] <= 'z') || (b[n] >= 'A' && b[n] <= 'Z')))
        //            { n++; }
        //            byte[] nb = new byte[n - oldn];
        //            Array.Copy(b, oldn, nb, 0, n - oldn);
        //            try
        //            {
        //                string internalEnc = Encoding.ASCII.GetString(nb);
        //                text = Encoding.GetEncoding(internalEnc).GetString(b);
        //                return Encoding.GetEncoding(internalEnc);
        //            }
        //            catch { break; }    // If C# doesn't recognize the name of the encoding, break.
        //        }
        //    }


        //    // If all else fails, the encoding is probably (though certainly not
        //    // definitely) the user's local codepage! One might present to the user a
        //    // list of alternative encodings as shown here: http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
        //    // A full list can be found using Encoding.GetEncodings();
        //    text = Encoding.Default.GetString(b);
        //    return Encoding.Default;
        //}


        public static string GetTextAutoEncoding(byte[] b, int taster = 1000)
        {
            //byte[] b = File.ReadAllBytes(filename);
            String text;
            //////////////// First check the low hanging fruit by checking if a
            //////////////// BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
            if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF)
            {
                text = Encoding.GetEncoding("utf-32BE").GetString(b, 4, b.Length - 4);
                return text;
                ;
            } // UTF-32, big-endian 
            if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00)
            {
                text = Encoding.UTF32.GetString(b, 4, b.Length - 4);
                return text;
            } // UTF-32, little-endian
            if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF)
            {
                text = Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2);
                return text;
            } // UTF-16, big-endian
            if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE)
            {
                text = Encoding.Unicode.GetString(b, 2, b.Length - 2);
                return text;
            } // UTF-16, little-endian
            if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF)
            {
                text = Encoding.UTF8.GetString(b, 3, b.Length - 3);
                return text;
            } // UTF-8
            if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76)
            {
                text = Encoding.UTF7.GetString(b, 3, b.Length - 3);
                return text;
            } // UTF-7


            //////////// If the code reaches here, no BOM/signature was found, so now
            //////////// we need to 'taste' the file to see if can manually discover
            //////////// the encoding. A high taster value is desired for UTF-8
            if (taster == 0 || taster > b.Length)
                taster = b.Length; // Taster size can't be bigger than the filesize obviously.


            // Some text files are encoded in UTF8, but have no BOM/signature. Hence
            // the below manually checks for a UTF8 pattern. This code is based off
            // the top answer at: http://stackoverflow.com/questions/6555015/check-for-invalid-utf8
            // For our purposes, an unnecessarily strict (and terser/slower)
            // implementation is shown at: http://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
            // For the below, false positives should be exceedingly rare (and would
            // be either slightly malformed UTF-8 (which would suit our purposes
            // anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
            int i = 0;
            bool utf8 = false;
            while (i < taster - 4)
            {
                if (b[i] <= 0x7F)
                {
                    i += 1;
                    continue;
                }
                    // If all characters are below 0x80, then it is valid UTF8, but UTF8 is not 'required' (and therefore the text is more desirable to be treated as the default codepage of the computer). Hence, there's no "utf8 = true;" code unlike the next three checks.
                if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0)
                {
                    i += 2;
                    utf8 = true;
                    continue;
                }
                if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 &&
                    b[i + 2] < 0xC0)
                {
                    i += 3;
                    utf8 = true;
                    continue;
                }
                if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 &&
                    b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0)
                {
                    i += 4;
                    utf8 = true;
                    continue;
                }
                utf8 = false;
                break;
            }
            if (utf8)
            {
                text = Encoding.UTF8.GetString(b);
                return text;
            }


            // The next check is a heuristic attempt to detect UTF-16 without a BOM.
            // We simply look for zeroes in odd or even byte places, and if a certain
            // threshold is reached, the code is 'probably' UF-16.          
            double threshold = 0.1;
                // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            int count = 0;
            for (int n = 0; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double) count)/taster > threshold)
            {
                text = Encoding.BigEndianUnicode.GetString(b);
                return text;
            }
            count = 0;
            for (int n = 1; n < taster; n += 2) if (b[n] == 0) count++;
            if (((double) count)/taster > threshold)
            {
                text = Encoding.Unicode.GetString(b);
                return text;
            } // (little-endian)


            // Finally, a long shot - let's see if we can find "charset=xyz" or
            // "encoding=xyz" to identify the encoding:
            for (int n = 0; n < taster - 9; n++)
            {
                if (
                    ((b[n + 0] == 'c' || b[n + 0] == 'C') && (b[n + 1] == 'h' || b[n + 1] == 'H') &&
                     (b[n + 2] == 'a' || b[n + 2] == 'A') && (b[n + 3] == 'r' || b[n + 3] == 'R') &&
                     (b[n + 4] == 's' || b[n + 4] == 'S') && (b[n + 5] == 'e' || b[n + 5] == 'E') &&
                     (b[n + 6] == 't' || b[n + 6] == 'T') && (b[n + 7] == '=')) ||
                    ((b[n + 0] == 'e' || b[n + 0] == 'E') && (b[n + 1] == 'n' || b[n + 1] == 'N') &&
                     (b[n + 2] == 'c' || b[n + 2] == 'C') && (b[n + 3] == 'o' || b[n + 3] == 'O') &&
                     (b[n + 4] == 'd' || b[n + 4] == 'D') && (b[n + 5] == 'i' || b[n + 5] == 'I') &&
                     (b[n + 6] == 'n' || b[n + 6] == 'N') && (b[n + 7] == 'g' || b[n + 7] == 'G') && (b[n + 8] == '='))
                    )
                {
                    if (b[n + 0] == 'c' || b[n + 0] == 'C') n += 8;
                    else n += 9;
                    if (b[n] == '"' || b[n] == '\'') n++;
                    int oldn = n;
                    while (n < taster &&
                           (b[n] == '_' || b[n] == '-' || (b[n] >= '0' && b[n] <= '9') || (b[n] >= 'a' && b[n] <= 'z') ||
                            (b[n] >= 'A' && b[n] <= 'Z')))
                    {
                        n++;
                    }
                    var nb = new byte[n - oldn];
                    Array.Copy(b, oldn, nb, 0, n - oldn);
                    try
                    {
                        string internalEnc = Encoding.ASCII.GetString(nb);
                        text = Encoding.GetEncoding(internalEnc).GetString(b);
                        return text;
                    }
                    catch
                    {
                        break;
                    } // If C# doesn't recognize the name of the encoding, break.
                }
            }


            // If all else fails, the encoding is probably (though certainly not
            // definitely) the user's local codepage! One might present to the user a
            // list of alternative encodings as shown here: http://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
            // A full list can be found using Encoding.GetEncodings();
            text = Encoding.Default.GetString(b);
            return text;
        }

        #endregion

        #endregion
    }
}