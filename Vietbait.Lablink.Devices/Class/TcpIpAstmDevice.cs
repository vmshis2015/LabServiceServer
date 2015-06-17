using System;
using System.Globalization;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    public abstract class TcpIpAstmDevice : TcpIpDevice
    {
        #region Attributes

        /// <summary>
        ///     Biến ghi nhận trạng thái gửi nhận dữ liệu của thiết bị ASTM
        ///     0: Chưa có kết nối tới máy xét nghiệm;
        ///     1: Đã kết nối và đang truyền dữ liệu
        /// </summary>
        public int State;

        public new string StringData;

        #endregion

        #region Contructor

        protected TcpIpAstmDevice()
        {
            State = 0;
            StringData = string.Empty;
        }

        #endregion

        #region Public Method

        ////public override void ProcessRawData()
        ////{
        ////    try
        ////    {
        ////        if (State.Equals(0) && base.StringData.EndsWith(DeviceHelper.ENQ.ToString(CultureInfo.InvariantCulture)))
        ////        {
        ////            //Gửi lại tín hiệu ACK tới thiết bị
        ////            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));

        ////            //Gán trạng thái = 1
        ////            State = 1;
        ////            //System.Threading.Thread.Sleep(100);
        ////        } //Kết thúc kiểm tra ENQ

        ////            //Nếu trạng thái truyền dữ liệu đã được thiết lập
        ////        else if (State.Equals(1))
        ////        {
        ////            if (base.StringData.EndsWith(DeviceHelper.ENQ.ToString(CultureInfo.InvariantCulture)))
        ////            {
        ////                //Gửi lại tín hiệu ACK tới thiết bị
        ////                SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
        ////            }

        ////                //Nếu chuỗi ký tự kết thúc bằng CRLF
        ////            else if (base.StringData.EndsWith(DeviceHelper.CRLF))
        ////            {
        ////                //Kiểm tra CheckSum và lấy ra chuỗi dữ liệu
        ////                string dataAfterCheck = DeviceHelper.GetStringAfterCheckSum(base.StringData);

        ////                //Nếu chuỗi trả bị lỗi gửi thông báo gửi lại
        ////                //Ngược lại bổ sung thêm dữ liệu đã xử lý vào bộ đệm và gửi tín hiệu thông báo thành công
        ////                if (string.IsNullOrEmpty(dataAfterCheck))
        ////                {
        ////                    SendStringData(DeviceHelper.NAK.ToString(CultureInfo.InvariantCulture));
        ////                    Log.Debug("Error while checksum value");
        ////                }
        ////                else
        ////                {
        ////                    SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
        ////                    //AddData(Encoding.ASCII.GetBytes(dataAfterCheck));
        ////                    StringData += dataAfterCheck;
        ////                }

        ////                //System.Threading.Thread.Sleep(300);
        ////            }
        ////                //Nếu nhận chuỗi EOT thì phát sinh sự kiện kết thúc nhận dữ liệu
        ////            else if (
        ////                //(base.StringData.IndexOf(DeviceHelper.EOT.ToString()) >= 0)
        ////                (base.StringData.EndsWith(DeviceHelper.EOT.ToString(CultureInfo.InvariantCulture))) 
        ////                //||(base.StringData.EndsWith(DeviceHelper.ETX.ToString(CultureInfo.InvariantCulture)))
        ////                )
        ////            {
        ////                State = 0;
        ////                //SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
        ////                ProcessData();
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw;
        ////    }
        ////    finally
        ////    {
        ////        base.ClearData();
        ////    }
        ////}


        public override void ProcessRawData()
        {
            try
            {
                if (State.Equals(0) && (ByteData[ByteData.Length - 1] == DeviceHelper.ENQ))
                {
                    //Gửi lại tín hiệu ACK tới thiết bị
                    SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));

                    //Gán trạng thái = 1
                    State = 1;
                    //System.Threading.Thread.Sleep(100);
                } //Kết thúc kiểm tra ENQ

                    //Nếu trạng thái truyền dữ liệu đã được thiết lập
                else if (State.Equals(1))
                {
                    if (ByteData[ByteData.Length - 1] == DeviceHelper.ENQ)
                    {
                        //Gửi lại tín hiệu ACK tới thiết bị
                        SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                    }

                    else if (base.StringData.EndsWith(DeviceHelper.EOT.ToString(CultureInfo.InvariantCulture)))
                    {
                        State = 0;
                        //SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                        ProcessData();
                    }

                        //Nếu chuỗi ký tự kết thúc bằng CRLF
                    else if ((ByteData[ByteData.Length - 2] == DeviceHelper.CR) &&
                             (ByteData[ByteData.Length - 1] == DeviceHelper.LF))
                    {
                        //Kiểm tra CheckSum và lấy ra chuỗi dữ liệu
                        string dataAfterCheck = DeviceHelper.GetStringAfterCheckSum(base.StringData);

                        //Nếu chuỗi trả bị lỗi gửi thông báo gửi lại
                        //Ngược lại bổ sung thêm dữ liệu đã xử lý vào bộ đệm và gửi tín hiệu thông báo thành công
                        if (string.IsNullOrEmpty(dataAfterCheck))
                        {
                            SendStringData(DeviceHelper.NAK.ToString(CultureInfo.InvariantCulture));
                            Log.Debug("Error while checksum value");
                        }
                        else
                        {
                            SendStringData(DeviceHelper.ACK.ToString(CultureInfo.InvariantCulture));
                            //AddData(Encoding.ASCII.GetBytes(dataAfterCheck));
                            StringData += dataAfterCheck;
                        }

                        //System.Threading.Thread.Sleep(300);
                    }
                        //Nếu nhận chuỗi EOT thì phát sinh sự kiện kết thúc nhận dữ liệu
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                base.ClearData();
            }
        }

        /// <summary>
        ///     Hàm xử lý dữ liệu
        /// </summary>
        public abstract void ProcessData();

        /// <summary>
        ///     Xóa trắng dữ liệu
        /// </summary>
        public new void ClearData()
        {
            StringData = string.Empty;
        }

        #endregion
    }
}