using System;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using Vietbait.Lablink.Devices;
using Vietbait.Lablink.Utilities;
using Vietbait.Server.Rs232Server;

namespace Vietbait.Lablink.Service
{
    internal class LablinkRs232 : Rs232Server
    {
        #region Attributies

        private readonly Logger Log = LogManager.GetLogger(DeviceHelper.MainServiceLogger);
        private readonly Dictionary<string, object> _colDevices;
        private readonly Dictionary<string, string> _colDevicesName;

        #endregion

        #region Contructor

        /// <summary>
        ///     Hàm khởi tạo Server. Tham số truyền vào là danh sách các thiết bị.
        /// </summary>
        /// <param name="colDevices"></param>
        /// <param name="colDevicesName"></param>
        public LablinkRs232(Dictionary<string, object> colDevices, Dictionary<string, string> colDevicesName)
        {
            try
            {
                _colDevices = colDevices;
                _colDevicesName = colDevicesName;
            }
            catch (Exception ex)
            {
                Log.Error("Error while Init Lablink RS232 Server {0}", ex.ToString());
                throw;
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        ///     Trả lại giá trị dạng base device từ một connector
        /// </summary>
        /// <param name="deviceConnector">Tên cổng COM</param>
        /// <returns>Trả về BaseDevice object nếu tồn tại thiết bị Trả về NULL nếu thiết bị chưa được khởi tạo</returns>
        private BaseDevice GetDevice(string deviceConnector)
        {
            try
            {
                return (BaseDevice) _colDevices[_colDevicesName[deviceConnector]];
            }
            catch (Exception ex)
            {
                Log.Error("Error while GetDevice {0}", ex.ToString());
                return null;
            }
        }

        #endregion

        #region Overrides of Rs232Server

        public override void OnServerStartSuccess(string message)
        {
            //throw new NotImplementedException();
        }

        public override void OnServerStartFalse(string message)
        {
            //throw new NotImplementedException();
        }

        public override void OnIncommingData(DataReceiveRs232 obj)
        {
            try
            {
                string deviceConnector = obj.Port.PortName;
                BaseDevice device = GetDevice(deviceConnector);

                //Nếu không tồn tại thiết bị nào ghi lại log.
                if (device == null)
                    Log.Debug("Could not load any devcie on {0}", deviceConnector);
                else
                    //Nếu tồn tại thiết bị thì phát sinh sự kiện nhận được dữ liệu
                    device.RaiseEventIncommingData(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error in 'OnIncommingData' method. Error Message:{0}", ex);
                throw ex;
            }
        }

        public override void OnEndReceiveData(SerialPort port)
        {
            string deviceConnector = port.PortName;
            BaseDevice device = GetDevice(deviceConnector);

            if (device == null)
                Log.Debug("Could not load any devcie on {0}", deviceConnector);
            else
                //Nếu tồn tại thiết bị thì phát sinh sự kiện nhận được dữ liệu
                device.RaiseEventEndReceiveData(port);
        }

        #endregion
    }
}