using System;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;

namespace Vietbait.Lablink.Devices
{
    public abstract class TcpIpDevice : BaseDevice, ITcpipDevice
    {
        #region Attributies

        #endregion

        #region Overrides of BaseDevice

        /// <summary>
        ///     Hàm gửi dữ liệu đến Device
        /// </summary>
        /// <param name="bytesData">Dữ liệu cần gửi đi</param>
        /// <returns>True: Nếu gửi thành công, Ném lỗi ra ngoài</returns>
        public override bool SendBytesData(byte[] bytesData)
        {
            try
            {
                NetStream.Write(bytesData, 0, bytesData.Length);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool SendStringData(string stringData)
        {
            try
            {
                byte[] bytesData = Encoding.ASCII.GetBytes(stringData);
                return SendBytesData(bytesData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void OnEndReceiveData(object obj)
        {
            try
            {
                var client = (TcpClient) obj;
                NetStream = client.GetStream();
                ProcessRawData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void OnBeginReceiveData(object obj)
        {
            throw new NotImplementedException();
        }

        public override void OnInCommingData(object obj)
        {
            var data = (byte[]) obj;
            AddData(data);
        }

        /// <summary>
        ///     Hàm set stream cho thiết bị
        /// </summary>
        public override void SetNetStream(SerialPort port)
        {
        }

        public override void OnClientConnected(object obj)
        {
            try
            {
                var client = (TcpClient) obj;
                NetStream = client.GetStream();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Implementation of ITcpipDevice

        public NetworkStream NetStream { get; private set; }

        #endregion

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

        public override void ProcessRawData()
        {
            throw new NotImplementedException();
        }
    }
}