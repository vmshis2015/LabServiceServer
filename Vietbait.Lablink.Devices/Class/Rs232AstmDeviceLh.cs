using System;
using System.Globalization;
using System.Text;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    /// <summary>
    ///     Class giành riêng cho máy LH
    /// </summary>
    public abstract class Rs232AstmDeviceLh : Rs232Base
    {
        #region Attributes

        /// <summary>
        ///     Chuỗi lỗi mặc định khi không kiểm tra được frame hay frame có lỗi
        /// </summary>
        private const string ErrorString = "-vietbait-hientd-error";

        /// <summary>
        ///     Biến ghi nhận trạng thái gửi nhận dữ liệu của thiết bị ASTM
        ///     0: Chưa có kết nối tới máy xét nghiệm;
        ///     1: Đã kết nối và đang truyền dữ liệu
        /// </summary>
        private int _state;

        #endregion

        #region Contructor

        protected Rs232AstmDeviceLh()
        {
            _state = 0;
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     Hàm trả về chuỗi Checksum của một Frame.
        ///     Check khi có một trong hai giá trị ETX, ETB
        /// </summary>
        /// <param name="frame">Chuỗi cần kiểm tra</param>
        /// <returns>Chuỗi trả về là 2 ký tự dùng để checksum</returns>
        private string GetCheckSumValue(string frame)
        {
            string checksum = "00";

            //int sumOfChars = 0;
            //bool complete = false;

            ////take each byte in the string and add the values
            //for (int idx = 0; idx < frame.Length; idx++)
            //{
            //    int byteVal = Convert.ToInt32(frame[idx]);

            //    switch (byteVal)
            //    {
            //        case DeviceHelper.STX:
            //            sumOfChars = 0;
            //            break;
            //        case DeviceHelper.ETX:
            //        case DeviceHelper.ETB:
            //            sumOfChars += byteVal;
            //            complete = true;
            //            break;
            //        default:
            //            sumOfChars += byteVal;
            //            break;
            //    }

            //    if (complete)
            //        break;
            //}

            //if (sumOfChars > 0)
            //{
            //    //hex value mod 256 is checksum, return as hex value in upper case
            //    checksum = Convert.ToString(sumOfChars % 256, 16).ToUpper();
            //}

            ////if checksum is only 1 char then prepend a 0
            //return (checksum.Length == 1 ? "0" + checksum : checksum);

            return checksum;
        }

        /// <summary>
        ///     Lấy về chuỗi sau khi đã xử lý CheckSum
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        private string GetStringAfterCheckSum(string frame)
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
                int lastIndexOfCrlf = frame.LastIndexOf(DeviceHelper.ETX.ToString(CultureInfo.InvariantCulture),
                    StringComparison.Ordinal);
                if (frame.Length > lastIndexOfCrlf - 2)
                {
                    //Lấy giá trị CheckSum từ chuỗi truyền vào
                    //csValueFromString = frame.Substring(lastIndexOfCrlf - 2, 2);
                    csValueFromString = "00";
                    Console.WriteLine(csValueFromString);
                }

                //So sánh giá trị tính toán và giá trị chứa trong chuỗi
                bool valid = csValue.Equals(csValueFromString);

                //Nếu chuỗi tính toán bằng chuỗi CheckSum được gửi kèm => lấy ra dữ liệu
                if (valid)
                {
                    Console.WriteLine(@"CheckSum OK");
                    int idFirstStx = frame.IndexOf(DeviceHelper.STX);
                    result = frame.Substring(idFirstStx + 3);
                    int idLastCrlf = result.LastIndexOf(DeviceHelper.ETX.ToString(), StringComparison.Ordinal);
                    result = result.Substring(0, idLastCrlf - 4);
                }
                    //Nếu có lỗi xảy ra khi truyền dữ liệu trả về chuỗi lỗi mặc định
                else
                {
                    Console.WriteLine(@"CheckSum Error");
                    result = ErrorString;
                }
            }
            catch (Exception)
            {
                result = ErrorString;
            }
            return result;
        }

        #endregion

        #region Public Method

        /// <summary>
        ///     Hàm xử lý dữ liệu khi các máy giao tiếp theo ASTM gửi đến
        /// </summary>
        /// <param name="obj"></param>
        public override void OnInCommingData(object obj)
        {
            try
            {
                // Chuyển kiểu dữ liệu về dạng DataReceiveRs232 do lớp Base Devices chỉ gửi Object
                var rawData = (DataReceiveRs232) obj;

                //Kiểm tra đối tượng
                if (rawData != null)
                {
                    // Nếu chưa gán Port thì gán
                    SetNetStream(rawData.Port);

                    //Nếu trạng thái =0 và có ký tự SYN kiểm tra ký tự SYN
                    byte[] bytesdata = rawData.Data;
                    string stringdata = Encoding.ASCII.GetString(bytesdata);
                    if (_state.Equals(0) && stringdata.EndsWith(DeviceHelper.SYN.ToString(CultureInfo.InvariantCulture)))
                    {
                        //Gửi lại tín hiệu ACK tới thiết bị
                        SendStringData(DeviceHelper.SYN.ToString(CultureInfo.InvariantCulture));

                        //Gán trạng thái = 1
                        _state = 1;
                        //System.Threading.Thread.Sleep(100);
                    } //Kết thúc kiểm tra SYN

                        // Nếu trạng thái truyền được thiết lập chờ để nhận tiếp 2 bytes
                    else if (_state.Equals(1))
                    {
                        if (bytesdata.Length == 2)
                        {
                            //Gửi lại ACK cho thiết bị
                            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                            //Thiết lập trạng thái bắt đầu nhận dữ liệu
                            _state = 2;
                        }
                    }

                        //Nếu trạng thái nhận dữ liệu đã được thiết lập
                    else if (_state.Equals(2))
                    {
                        //if (stringdata.EndsWith(DeviceHelper.EOT.ToString()+ DeviceHelper.ENQ.ToString()))
                        if (stringdata.EndsWith(DeviceHelper.SYN.ToString(CultureInfo.InvariantCulture)))
                        {
                            _state = 0;
                            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                            RaiseEventEndReceiveData(rawData.Port);
                        }
                            //Nếu chuỗi ký tự kết thúc bằng CRLF
                        else if (stringdata.EndsWith(DeviceHelper.ETX.ToString(CultureInfo.InvariantCulture)))
                        {
                            //Kiểm tra CheckSum và lấy ra chuỗi dữ liệu
                            string dataAfterCheck = GetStringAfterCheckSum(stringdata);

                            //Nếu chuỗi trả bị lỗi gửi thông báo gửi lại
                            //Ngược lại bổ sung thêm dữ liệu đã xử lý vào bộ đệm và gửi tín hiệu thông báo thành công
                            if (dataAfterCheck.Equals(ErrorString))
                            {
                                SendStringData(DeviceHelper.NAK.ToString(CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                                AddData(Encoding.ASCII.GetBytes(dataAfterCheck));
                                Console.WriteLine(stringdata);
                            }
                            //System.Threading.Thread.Sleep(300);
                        }
                            //Nếu nhận chuỗi EOT thì phát sinh sự kiện kết thúc nhận dữ liệu
                        else if (
                            (stringdata.IndexOf(DeviceHelper.SYN.ToString(CultureInfo.InvariantCulture),
                                StringComparison.Ordinal) >= 0))
                        {
                            _state = 0;
                            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                            RaiseEventEndReceiveData(rawData.Port);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}