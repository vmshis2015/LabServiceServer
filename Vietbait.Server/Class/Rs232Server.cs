using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using NLog;
using Vietbait.Lablink.Utilities;
using Timer = System.Timers.Timer;

namespace Vietbait.Server.Rs232Server
{
    public abstract class Rs232Server : IRs232Server, IRs232ServerHandler
    {
        #region attributes

        #region Enum PortStatus

        private enum PortStatus
        {
            /// <summary>
            ///     Cổng ở trạng thái sẵn sàng nhận dữ liệu
            /// </summary>
            Idle = 0,

            /// <summary>
            ///     Dữ liệu tại cổng đang được thiết bị xử lý không sẵn sàng nhận dữ liệu
            /// </summary>
            Processing = 1
        }

        #endregion

        //private SerialPort[] _portList;
        private readonly Logger Log;
        private readonly Dictionary<Timer, SerialPort> _colPort = new Dictionary<Timer, SerialPort>();
        private readonly Dictionary<string, Timer> _colTimer = new Dictionary<string, Timer>();
        private readonly Dictionary<string, PortStatus> _portStatus = new Dictionary<string, PortStatus>();

        #endregion

        #region Private Method

        private void ResetTimer(string portName)
        {
            _colTimer[portName].Stop();
            _colTimer[portName].Start();
        }

        private void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            var timer = (Timer) source;
            timer.Enabled = false;
            timer.Stop();
            SerialPort serialPort = _colPort[timer];
            _portStatus[serialPort.PortName] = PortStatus.Processing;
            EndReceiveData.Invoke(serialPort);
            _portStatus[serialPort.PortName] = PortStatus.Idle;
        }

        public bool CheckPortAvailable(string portName)
        {
            try
            {
                if (_colTimer.Count == 0) return false;
                return _colTimer[portName] != null;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     ghép data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataToAdd"></param>
        private void Merge(ref byte[] data, byte[] dataToAdd)
        {
            try
            {
                try
                {
                    int id = data.Length;
                    Array.Resize(ref data, id + dataToAdd.Length);
                    dataToAdd.CopyTo(data, id);
                }
                catch (Exception)
                {
                    data = dataToAdd;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Hàm xử lý sự kiện khi một trong các Port nhận được dữ liệu
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void ComDataReceived(object obj, SerialDataReceivedEventArgs e)
        {
            var sender = (SerialPort) obj;
            //Kiểm tra trạng thái của cổng xem có sẵn sàng nhận dữ liệu không.
            if (_portStatus[sender.PortName] != PortStatus.Idle) return;

            var buffer = new byte[] {};

            int bytesToRead = sender.BytesToRead;

            while (bytesToRead > 0)
            {
                var tempBuffer = new byte[bytesToRead];
                sender.Read(tempBuffer, 0, bytesToRead);
                Merge(ref buffer, tempBuffer);
                Thread.Sleep(50);
                bytesToRead = sender.BytesToRead;
            }

            // Lưu lại file Log
            try
            {
                //Logger log = LogManager.GetLogger(sender.PortName);
                //log.Trace("Receive Data:{0}", Encoding.ASCII.GetString(buffer));

                string path = string.Format(@"D:\_COMLOG\{0}-{1}.log", DateTime.Today.ToString(@"yyyyMMdd"),
                    sender.PortName);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                string contents = string.Format("{0} RECV:{1}\r\n", DateTime.Now.ToString("HH:mm:ss.fff"),
                    Encoding.ASCII.GetString(buffer));
                File.AppendAllText(path, contents);
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\_LabService_Error.txt",
                    string.Format("{0} - {1}\n",
                        DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss"), ex));
            }

            IncommingData.Invoke(new DataReceiveRs232(sender, buffer));
            ResetTimer(sender.PortName);
        }

        #endregion

        #region Contructor

        protected Rs232Server()
        {
            ServerStartSuccess += OnServerStartSuccess;
            ServerStartFalse += OnServerStartFalse;
            IncommingData += OnIncommingData;
            EndReceiveData += OnEndReceiveData;
            Log = LogManager.GetLogger(DeviceHelper.MainServiceLogger);
        }

        #endregion

        #region Implementation of IRs232ServerHandler

        public abstract void OnServerStartSuccess(string message);

        public abstract void OnServerStartFalse(string message);

        public abstract void OnIncommingData(DataReceiveRs232 obj);

        public abstract void OnEndReceiveData(SerialPort port);

        #endregion

        #region Implementation of IRs232Server

        public bool AddPort(string portName, int baudRate, int dataBits, StopBits stopbits, Parity parity,
            Handshake handshake, bool rtsEnable, bool dtrEnable, int timerInterval)
        {
            try
            {
                if (!CheckPortAvailable(portName))
                {
                    var comPort = new SerialPort
                    {
                        ReadBufferSize = 131072,
                        DataBits = dataBits,
                        PortName = portName,
                        BaudRate = baudRate,
                        StopBits = stopbits,
                        Parity = parity,
                        Handshake = handshake,
                        RtsEnable = rtsEnable,
                        DtrEnable = dtrEnable,
                    };
                    _portStatus.Add(portName, PortStatus.Idle);

                    Timer timer = timerInterval > 0 ? new Timer(timerInterval) : new Timer(100);
                    _colTimer.Add(portName, timer);
                    if (timerInterval > 0) _colTimer[portName].Elapsed += OnTimerElapsed;

                    _colPort.Add(_colTimer[portName], comPort);
                    _colPort[_colTimer[portName]].DataReceived += ComDataReceived;
                    _colPort[_colTimer[portName]].PinChanged += Rs232PinChanged;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Rs232PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            //var serialPortName = ((SerialPort) sender).PortName;

            //throw new NotImplementedException();
            
            //var eventName = e.EventType.ToString();
            //if (e.EventType == SerialPinChange.Break)
            try
            {
                Log.Debug("Pinchanged - Detail:{0} On Port {1}", e.EventType.ToString(), ((SerialPort)sender).PortName);
            }
            catch (Exception ex)
            {
                Log.Error("Error while raise Pin changed - Detail:{0}",ex);
            }

        }

        public void StartServer()
        {
            //todo: Mở lại port khi lỗi
            List<SerialPort> errorPort = new List<SerialPort>();
            
            foreach (var port in _colTimer)
            {
                try
                {
                    port.Value.Enabled = false;
                    _colPort[port.Value].Open();
                }
                catch (Exception ex)
                {
                    Log.Error("Could not open {0}\r\nDetail:{1}", _colPort[port.Value].PortName,ex);
                    errorPort.Add(_colPort[port.Value]);
                    //errorPort.Add(_colPort[port.Value]);
                }
            }
            //while (errorPort.Count>0)
            //{
            //    try
            //    {
            //        errorPort[0].Open();
            //        errorPort.RemoveAt(0);
            //    }
            //    catch (Exception)
            //    {
                   
            //    }
            //}
        }

        public void StopServer()
        {
            try
            {
                foreach (var port in _colTimer)
                {
                    port.Value.Enabled = false;
                    _colPort[port.Value].Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public event ServerStatusHandler ServerStartSuccess;
        public event ServerStatusHandler ServerStartFalse;
        public event IncommingDataHandler IncommingData;
        public event EndReceiveDataHanler EndReceiveData;

        /// <summary>
        ///     Get Allport in services
        /// </summary>
        /// <returns></returns>
        public List<SerialPort> GetAllPorts()
        {
            return _colPort.Select(pair => pair.Value).ToList();
        }

        #endregion
    }
}