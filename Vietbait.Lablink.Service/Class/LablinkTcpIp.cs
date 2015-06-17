using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using NLog;
using Vietbait.Lablink.Devices;
using Vietbait.Lablink.Utilities;
using Vietbait.Server.TCPIPServer;

namespace Vietbait.Lablink.Service
{
    internal class LablinkTcpIp : TcpipServer
    {
        #region Attributies

        private readonly Logger Log = LogManager.GetLogger(DeviceHelper.MainServiceLogger);
        private readonly Dictionary<string, object> _colDevices;
        private readonly Dictionary<string, string> _colDevicesName;

        #endregion

        #region Contructor

        public LablinkTcpIp(Dictionary<string, object> colDevices, Dictionary<string, string> colDevicesName)
        {
            try
            {
                _colDevices = colDevices;
                _colDevicesName = colDevicesName;
            }
            catch (Exception ex)
            {
                Log.Error("Error while Init Lablink TCPIP Server {0}", ex.ToString());
                throw;
            }
        }

        #endregion

        #region Private Method

        private BaseDevice GetDevice(string deviceEndpoint)
        {
            try
            {
                BaseDevice device = null;
                try
                {
                    device = (BaseDevice) _colDevices[_colDevicesName[deviceEndpoint]];
                }
                catch (Exception)
                {
                }


                try
                {
                    device =
                        (BaseDevice)
                            _colDevices[
                                _colDevicesName[
                                    deviceEndpoint.Substring(0, deviceEndpoint.IndexOf(":", StringComparison.Ordinal))]];
                    
                }
                catch (Exception)
                {
                }
                return device;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private BaseDevice GetDevice(TcpClient client)
        {
            try
            {
                if (client == null) return null;
                //string temp = client.Client.RemoteEndPoint.ToString();
                //var tempDevice = GetDevice(temp);
                //return tempDevice ?? GetDevice(temp.Substring(0, temp.IndexOf(":", StringComparison.Ordinal)));
                //string deviceEndpoint = temp.Substring(0, temp.IndexOf(":", StringComparison.Ordinal));
                return GetDevice(client.Client.RemoteEndPoint.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Error while GetDevice {0}", ex.ToString());
                return null;
            }
        }

        #endregion

        #region Overrides of TcpipServer

        /// <summary>
        /// Sự kiện phát sinh khi bắt đầu nhận dữ liệu
        /// </summary>
        /// <param name="client"></param>
        public override void OnBeginReciveData(TcpClient client)
        {
            //Log.Trace("Begin recive data from {0}", client.Client.RemoteEndPoint.ToString());
        }

        /// <summary>
        /// Sự kiện phát sinh khi có client kết nối đến server
        /// </summary>
        /// <param name="client">Client </param>
        public override void OnClientConnected(TcpClient client)
        {
            BaseDevice baseDevice = GetDevice(client);
            if (baseDevice != null) baseDevice.RaiseEventClientConnected(client);
        }

        /// <summary>
        /// Sự kiện phát sinh khi client ngắt kết nối đến server
        /// </summary>
        /// <param name="client"></param>
        public override void OnClientDisconnected(TcpClient client)
        {
            Log.Trace("Client disconnected: {0}", client.Client.RemoteEndPoint.ToString());
        }

        /// <summary>
        /// Sự kiện phát sinh khi kết thúc nhận dữ liệu
        /// </summary>
        /// <param name="client"></param>
        public override void OnEndReciveData(TcpClient client)
        {
            Log.Trace("End recive data from {0}", client.Client.RemoteEndPoint.ToString());
            try
            {
                BaseDevice baseDevice = GetDevice(client);
                if (baseDevice != null) baseDevice.RaiseEventEndReceiveData(client);
                else Log.Debug("Device is null");
            }
            catch (Exception ex)
            {
                Log.Error("Error while raise 'OnEndReciveData' {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Sự kiện phát sinh khi khởi tạo server lỗi
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public override void OnStartServerFalse(int port, string exceptionString)
        {
            Log.Error("Start Server false on port {0} Error: {1}", port.ToString(CultureInfo.InvariantCulture),
                exceptionString);
        }

        /// <summary>
        /// Sự kiện phát sinh khi khởi tạo server thành công
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public override void OnStartServerSuccessfull(int port, string exceptionString)
        {
            Log.Debug("Start server success: Service monitoring port {0}", port);
        }

        /// <summary>
        /// Sự kiện phát sinh khi ngắt server thành công
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public override void OnStopServerSuccessfull(int port, string exceptionString)
        {
            Log.Debug("Server stoped: Service release port {0}", port);
        }

        /// <summary>
        /// Sự kiện phát sinh khi có dữ liệu được gửi đến từ client
        /// </summary>
        /// <param name="dataHeader">Thông tin kết nối</param>
        /// <param name="data">dữ liệu nhận được</param>
        public override void OnIncommingData(DataHeader dataHeader, byte[] data)
        {
            try
            {
                BaseDevice baseDevice = GetDevice(string.Format("{0}:{1}", dataHeader.ClientIP,dataHeader.ClientPort));
                if (baseDevice != null) baseDevice.RaiseEventIncommingData(data);
            }
            catch (Exception ex)
            {
                Log.Error("Error while raise 'OnIncommingData' {0}", ex.ToString());
            }
        }

        #endregion
    }
}