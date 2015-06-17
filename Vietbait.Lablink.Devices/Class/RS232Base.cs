using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    public abstract class Rs232Base : BaseDevice
    {
        #region Attributies

        private SerialPort _netStream = new SerialPort();

        #endregion

        #region Contructor

        #endregion

        #region Overrides of BaseDevice

        /// <summary>
        ///     hàm gửi dữ liệu đến thiết bị
        /// </summary>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        public override bool SendBytesData(byte[] bytesData)
        {
            try
            {
                _netStream.Write(bytesData, 0, bytesData.Length);
                _netStream.BaseStream.Flush();

                // Lưu lại Log của thiết bị
                // Lưu lại file Log
                try
                {
                    //var log = LogManager.GetLogger(_netStream.PortName);
                    //log.Debug("Send Data:{0}", Encoding.ASCII.GetString(bytesData));
                    string path = string.Format(@"D:\_COMLOG\{0}-{1}.log", DateTime.Today.ToString(@"yyyyMMdd"),
                        _netStream.PortName);
                    //string contents = string.Format("{0} {1}: {2}\r\n", DateTime.Now.ToLongTimeString(),
                    //    _netStream.PortName, Encoding.ASCII.GetString(bytesData));

                    string contents = string.Format("{0} SEND:{1}\r\n", DateTime.Now.ToString("HH:mm:ss.fff"),
                        Encoding.UTF8.GetString(bytesData));

                    File.AppendAllText(path, contents);
                }
                catch (Exception ex)
                {
                    File.WriteAllText(@"C:\Program Files\_LabService_Error.txt",
                        string.Format("{0} - {1}",
                            DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), ex));
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gủi dữ liệu dạng chuỗi cho thiết bị
        /// </summary>
        /// <param name="stringData"></param>
        /// <returns></returns>
        public override bool SendStringData(string stringData)
        {
            return SendBytesData(Encoding.UTF8.GetBytes(stringData));
        }

        /// <summary>
        ///     Gửi dữ liệu cho thiết bị dạng Byte
        /// </summary>
        /// <param name="byteData">Mảng Byte cần gửi đi</param>
        /// <returns>True: Nếu gửi thành công, False: nếu gửi lỗi</returns>
        public override bool SendByte(byte byteData)
        {
            try
            {
                var bytesData = new byte[1];
                bytesData[0] = byteData;
                return SendBytesData(bytesData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Gán cổng Com cho thiết bị
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public override void SetNetStream(SerialPort port)
        {
            try
            {
                if (_netStream.Equals(null) || !_netStream.Equals(port)) _netStream = port;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Hàm xử lý sự kiện khi kết thúc nhận một Frame từ thiết bị
        /// </summary>
        /// <param name="obj"></param>
        public override void OnEndReceiveData(object obj)
        {
            try
            {
                SetNetStream(obj as SerialPort);
                ProcessRawData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void OnBeginReceiveData(object obj)
        {
        }

        public override void OnInCommingData(object obj)
        {
            try
            {
                var dr = obj as DataReceiveRs232;
                if (dr == null) return;
                SetNetStream(dr.Port);
                AddData(dr.Data);
                //try
                //{
                //    Logger log = LogManager.GetLogger(dr.Port.PortName);
                //    log.Trace("Receive Data:{0}", Encoding.ASCII.GetString(dr.Data));
                //}
                //catch (Exception ex)
                //{
                //    File.AppendAllText(@"D:\_LabService_Error.txt",
                //                                    string.Format("{0} - {1}\n",
                //                                                  DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), ex));
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void OnClientConnected(object obj)
        {
        }

        #endregion
    }
}