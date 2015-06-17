using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using NLog.Targets;
using Vietbait.Lablink.TestResult;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    public abstract class BaseDevice : IBaseDevice, IBaseDeviceHandlers
    {
        protected Result TestResult;
        private string _baseDeviceName;
        private byte[] _data;
        private decimal _deviceId;
        private DataTable _paraNameTable;
        private int _testTypeId;

        #region Implementation of IBaseDevice

        #region Contructor

        protected BaseDevice()
        {
            //Trả về tên của class. Tên này dùng làm tên thiết bị.
            _baseDeviceName = GetType().ToString();
            //Gán hàm xử lý sự kiện cho 2 sụ kiện của Base Device
            BeginReceiveData += OnBeginReceiveData;
            EndReceiveData += OnEndReceiveData;
            IncommingData += OnInCommingData;
            ClientConnected += OnClientConnected;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Trả về dữ liệu của thiết bị dạng string
        /// </summary>
        public string StringData
        {
            get { return _data != null ? Encoding.UTF8.GetString(_data) : ""; }
        }

        /// <summary>
        ///     Trả về dữ liệu của thiết bị là một mảng byte
        /// </summary>
        public byte[] ByteData
        {
            get { return _data; }
        }

        /// <summary>
        ///     Tên của thiết bị (mặc định được lấy theo tên class chứa các hàm xử lý của nó)
        /// </summary>
        public string DeviceName
        {
            get
            {
                int index = _baseDeviceName.LastIndexOf(".", StringComparison.Ordinal) + 1;
                return _baseDeviceName.Substring(index).ToUpper();
            }
            set
            {
                int index = _baseDeviceName.LastIndexOf(".", StringComparison.Ordinal) + 1;
                _baseDeviceName = string.Format("{0}.{1}", _baseDeviceName.Substring(index).ToUpper(), value);
            }
        }

        /// <summary>
        ///     ID của loại xét nghiệm
        /// </summary>
        public int TestTypeId
        {
            get { return _testTypeId; }
        }

        public Logger Log { get; set; }

        #endregion

        #region Events

        /// <summary>
        ///     Sự kiện phát sinh khi bắt đầu nhận dữ liệu từ thiết bị
        /// </summary>
        public event MyEventHandler BeginReceiveData;

        /// <summary>
        ///     Sự kiện phát sinh khi kết thúc nhận dữ liệu từ thiết bị
        /// </summary>
        public event MyEventHandler EndReceiveData;

        /// <summary>
        ///     Sự kiên phát sinh khi có dữ liệu được gửi tới
        /// </summary>
        public event MyEventHandler IncommingData;

        public event MyEventHandler ClientConnected;

        #endregion

        #region Abstract Method

        public abstract bool SendByte(byte byteData);

        /// <summary>
        ///     Hàm gửi dữ liệu dạng byte[] đến thiết bị
        /// </summary>
        /// <param name="bytesData"> Mảng byte cần gửi đi</param>
        /// <returns>true: khi không xảy ra lỗi</returns>
        public abstract bool SendBytesData(byte[] bytesData);

        /// <summary>
        ///     Hàm gửi dữ liệu dạng string đến thiết bị
        /// </summary>
        /// <param name="stringData">String cần gửi</param>
        /// <returns>true: khi không xảy ra lỗi</returns>
        public abstract bool SendStringData(string stringData);

        /// <summary>
        ///     Hàm xử lý dữ liệu nhận được từ thiết bị khi kết thúc nhận dữ liệu
        /// </summary>
        public abstract void ProcessRawData();

        /// <summary>
        ///     Hàm set stream cho thiết bị
        /// </summary>
        public abstract void SetNetStream(SerialPort port);

        /// <summary>
        ///     Hàm khởi tạo thiết bị.
        /// </summary>
        /// <param name="deviceName">Tên thiết bị truyền vào</param>
        /// <returns></returns>
        public bool InitialDevice(string deviceName)
        {
            try
            {
                Log = LogManager.GetLogger(deviceName);
                DeviceName = deviceName;
                DeviceHelper.GetDeviceConfig(DeviceName, ref _testTypeId, ref _paraNameTable, ref _deviceId);
                TestResult = new Result(_deviceId);
                return true;
            }
            catch (Exception)
            {
                Log.Error("Could not get device configuration from data base");
                Log.Debug("Device has default config: TestTypeID = -1,Parara Name Table is null, DeviceID = -1");
                return false;
            }
        }

        #endregion

        /// <summary>
        ///     Thêm dữ liệu nhận được từ thiết bị vào mảng lưu trữ
        /// </summary>
        /// <param name="byteData">Dữ liệu nhận dược dạng Byte</param>
        public void AddData(byte[] byteData)
        {
            try
            {
                try
                {
                    int id = _data.Length;
                    Array.Resize(ref _data, id + byteData.Length);
                    byteData.CopyTo(_data, id);
                }
                catch (Exception)
                {
                    _data = byteData;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///     Hàm dùng để gọi sự kiện Bắt đầu gửi dữ liệu
        /// </summary>
        /// <param name="obj"></param>
        public void RaiseEventBeginReceiveData(object obj)
        {
            BeginReceiveData.Invoke(obj);
        }

        /// <summary>
        ///     Hàm dùng để gọi sự kiện kết thúc nhận dữ liệu.
        /// </summary>
        /// <param name="obj"></param>
        public void RaiseEventEndReceiveData(object obj)
        {
            EndReceiveData.Invoke(obj);
        }

        public void RaiseEventClientConnected(object obj)
        {
            ClientConnected.Invoke(obj);
        }

        /// <summary>
        ///     Xóa sạch dữ liệu trong mảng lưu trữ
        ///     Thường dùng sau khi xử lý dữ liệu
        /// </summary>
        public void ClearData()
        {
            try
            {
                _data = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///     Thêm một kết quả vào mảng lưu trữ
        /// </summary>
        /// <param name="resultItem">Kết quả mới</param>
        public void AddResult(ResultItem resultItem)
        {
            try
            {
                TestResult.Add(resultItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Hàm lưu trữ dữ liệu vào DataBase
        /// </summary>
        /// <returns></returns>
        public bool ImportResults()
        {
            try
            {
                // Hiệu chỉnh lại định dạng ngày tháng năm
                try
                {
                    DateTime tempDate = Convert.ToDateTime(TestResult.TestDate,
                        new DateTimeFormatInfo {ShortDatePattern = "dd/MM/yyyy"});
                }
                catch (Exception)
                {
                    TestResult.TestDate = DateTime.Now.ToString("dd/MM/yyyy");
                    Log.Error("Error while parse DateTime, Set Test_Date = {0}", TestResult.TestDate);
                }

                // Import dữ liệu vào DB
                bool result = DeviceHelper.ImportResultToDb((short) _testTypeId, TestResult, _paraNameTable);

                FileTarget logFile = (from t in LogManager.Configuration.AllTargets
                    where t is FileTarget
                    select (FileTarget) t).FirstOrDefault();

                if (logFile != null)
                {
                    string fileName = logFile.FileName.Render(new LogEventInfo {TimeStamp = DateTime.Now});
                    fileName = fileName.Replace("/", @"\");
                    fileName = fileName.Replace(@"\\", @"\");


                    //Lấy về tên thư mục chứa file đang ghi Log
                    fileName = Path.GetDirectoryName(fileName);

                    //Tạo thư mục Log với tên của thiết bị.
                    fileName = string.Format("{0}{1}{2}", fileName, Path.DirectorySeparatorChar, Log.Name);
                    if (!Directory.Exists(fileName)) Directory.CreateDirectory(fileName);
                    ////Tạo thư mcụ để lưu kết quả.
                    //fileName = string.Format("{0}{1}{2}", fileName, Path.DirectorySeparatorChar, "Result");
                    Thread.Sleep(2);
                    //Tạo file với tên file bắt đầu bằng Barcode-Ngày tháng năm - giờ phút giây
                    fileName = string.Format("{0}{1}{2}-{3}.log", fileName, Path.DirectorySeparatorChar,
                        TestResult.Barcode.Replace("*", ""), DateTime.Now.ToString("ddMMyyyyHHmmssffff"));
                    fileName = fileName.Replace("/", @"\").Replace(@"\\", @"\");
                    fileName = fileName.Replace("\"", @"");

                    string tempfileName = Path.GetFileName(fileName);
                    if (tempfileName != null) tempfileName = tempfileName.Replace(@":", @"");
                    fileName = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar + tempfileName;

                    if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    try
                    {
                        File.WriteAllLines(fileName, TestResult.ToStringArray());
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Could not write result to file '{0}' \n Error: {1} ", fileName, ex));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error While Import result to DB ER:\n {0}", ex.ToString());
                return false;
            }
            finally
            {
                if (TestResult != null) TestResult.Clear();
            }
        }

        public void AddResult(List<ResultItem> resultItems)
        {
            foreach (ResultItem resultItem in resultItems)
            {
                TestResult.Add(resultItem);
            }
        }

        /// <summary>
        ///     Thêm dữ liệu nhận được từ thiết bị vào mảng lưu trữ
        /// </summary>
        /// <param name="stringData">Dữ liệu nhận được ở dạng chuỗi</param>
        public void AddStringData(string stringData)
        {
            AddData(new ASCIIEncoding().GetBytes(stringData));
        }

        /// <summary>
        ///     Hàm dùng để gọi sự kiện khi có dữ liệu gửi đến
        /// </summary>
        /// <param name="obj"></param>
        public virtual void RaiseEventIncommingData(object obj)
        {
            IncommingData.Invoke(obj);
        }

        #endregion

        #region Implementation of IBaseDeviceHandlers

        public abstract void OnEndReceiveData(object obj);
        public abstract void OnBeginReceiveData(object obj);
        public abstract void OnInCommingData(object obj);
        public abstract void OnClientConnected(object obj);

        #endregion

        #region Public Method

        /// <summary>
        ///     Hàm khởi tạo thiết bị.
        /// </summary>
        /// <returns>True:Khởi tạo thiết bị thành công - False:Khởi tạo thiết bị không thành công</returns>
        public bool InitialDevice()
        {
            return InitialDevice(DeviceName);
        }

        public List<string> GetRegList(string pBarcode)
        {
            string patientName = string.Empty;
            return DeviceHelper.GetRegList((int) _deviceId, pBarcode, ref patientName);
        }

        public List<string> GetRegList(string pBarcode, ref string pPatientName)
        {
            return DeviceHelper.GetRegList((int) _deviceId, pBarcode, ref pPatientName);
        }

        public int GetTestTypeId()
        {
            return _testTypeId;
        }

        public DataTable GetParaNameTable()
        {
            return _paraNameTable;
        }

        public decimal GetDeviceId()
        {
            return _deviceId;
        }

        #endregion
    }
}