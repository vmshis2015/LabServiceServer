//using Vietbait.NetworkMonitor;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using Vietbait.Lablink.Utilities;
using Timer = System.Timers.Timer;

namespace Vietbait.Server.TCPIPServer
{
    public abstract class TcpipServer : ITcpipServer, ITcpipServerHandler
    {
        #region Private Attributies

        //private readonly Timer _networkMonitorTimer;
        private readonly Timer _timer = new Timer();
        //private readonly NetworkStatusMonitor _monitor;
        private bool _isRunning;
        private TcpListener _listener;
        //private NetworkStatusMonitor.StatusMonitorEventArgs _lastStatusMonitorEventArgs;

        #endregion

        #region Properties

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public int Port { get; set; }

        public int TimerInterval { get; set; }

        public int DelayTime { get; set; }

        #endregion

        #region Contructor

        protected TcpipServer()
        {
            try
            {
                //Init Timer for netmonitor
                //_networkMonitorTimer = new Timer(30000);
                //_networkMonitorTimer.Elapsed += OnTimerElapsed;

                //Add Monitor event
                //_monitor = new NetworkStatusMonitor(300);

                ////Event Type to catch
                //_monitor.MonitorNewConnections = true;
                //_monitor.MonitorDisconnections = true;
                //_monitor.MonitorNetworkAddressChanged = true;

                ////Add Handler
                //_monitor.NetworkInterfaceConnected += MonitorNetworkInterfaceChanged;
                //_monitor.NetworkInterfaceDisconnected += MonitorNetworkInterfaceChanged;
                //_monitor.NetworkInterfaceChanged += MonitorNetworkInterfaceChanged;
                //_monitor.NetworkAddressChanged += MonitorNetworkInterfaceChanged;

                //Add handlers for Clients
                BeginReciveData += OnBeginReciveData;
                ClientConnected += OnClientConnected;
                ClientDisconnected += OnClientDisconnected;
                EndReciveData += OnEndReciveData;
                IncommingData += OnIncommingData;
                //NetworkChanged += OnNetworkChanged;

                //Add handlers for Server
                StartServerFalse += OnStartServerFalse;
                StartServerSuccessfull += OnStartServerSuccessfull;
                StopServerSuccessfull += OnStopServerSuccessfull;

                //Add handler for Timer
                _timer.Elapsed += AcceptClients;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Method

        private void AcceptClients(object obj, ElapsedEventArgs e)
        {
            try
            {
                if (_listener == null) return;

                if (_listener.Pending())
                {
                    _timer.Enabled = false;

                    //Wait ultil Client connected
                    TcpClient client = _listener.AcceptTcpClient();
                    //Raise evete Client Connected
                    ClientConnected(client);

                    var doconnect = new Thread(DoConnect) {IsBackground = true};

                    bool setApartmentState = doconnect.TrySetApartmentState(ApartmentState.MTA);
                    if (!setApartmentState) setApartmentState = doconnect.TrySetApartmentState(ApartmentState.STA);
                    if (!setApartmentState) doconnect.SetApartmentState(ApartmentState.Unknown);

                    //Create new thread to hold this client
                    doconnect.Start(client);
                    _timer.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void DoConnect(object obj)
        {
            try
            {
                bool beginSend = false;
                //var clientConnected = true;

                //Grant Stream for client stream
                var client = (TcpClient) obj;
                NetworkStream clientStream = client.GetStream();

                //Handles Thread
                Thread currentThread = Thread.CurrentThread;

                string[] tempStr = client.Client.RemoteEndPoint.ToString().Split(':');
                string sourceIp = tempStr[0];
                int sourcePort = Convert.ToInt32(tempStr[1]);
                tempStr = client.Client.LocalEndPoint.ToString().Split(':');
                string destinationIP = tempStr[0];
                int destinationPort = Convert.ToInt32(tempStr[1]);

                var dataHeader = new DataHeader(sourceIp, sourcePort, destinationIP, destinationPort);

                while (_isRunning && (_listener != null)) //&& clientConnected)
                {
                    if (client.Available > 0)
                    {
                        //SetTimerStatus(_endTimer, false);
                        byte[] buffer;
                        lock (currentThread)
                        {
                            if (!beginSend)
                            {
                                beginSend = true;
                                BeginReciveData(client);
                            }
                            int bufferSize = client.Available;
                            buffer = new byte[bufferSize];
                            clientStream.Read(buffer, 0, bufferSize);
                        }
                        clientStream.Flush();

                        //Raise Event Incomming Data
                        IncommingData(dataHeader, buffer);
                        Thread.Sleep(DelayTime);
                        //SetTimerStatus(_endTimer, true);
                    }
                    else if (beginSend && !clientStream.DataAvailable)
                    {
                        beginSend = false;
                        EndReciveData(client);
                    }
                    else if (client.Client.Receive(new byte[1], SocketFlags.Peek) == 0)
                    {
                        //clientConnected = false;
                        ClientDisconnected(client);
                        client.Close();
                        return;
                    }
                }
                if (_isRunning) return;
                client.Close();
            }
            catch 
            {
            }
        }

        //private void MonitorNetworkInterfaceChanged(object sender, NetworkStatusMonitor.StatusMonitorEventArgs e)
        //{
        //    //Console.WriteLine("Network Interface: {1} {2}" , ConsoleColor.Red, e.Interface.Description, e.EventType.ToString());
        //    ResetMonitorTimer(e);
        //}

        //private void ResetMonitorTimer(NetworkStatusMonitor.StatusMonitorEventArgs e)
        //{
        //    _lastStatusMonitorEventArgs = e;
        //    _networkMonitorTimer.Stop();
        //    _networkMonitorTimer.Start();
        //}

        //private void OnTimerElapsed(object source, ElapsedEventArgs e)
        //{
        //    var timer = (Timer) source;
        //    timer.Enabled = false;
        //    NetworkChanged.Invoke(_lastStatusMonitorEventArgs);
        //}

        #endregion

        #region Implementation of ITcpipServer

        public void StartServer()
        {
            if (_isRunning) return;
            try
            {
                //Start Monitor Network
                //_monitor.StartMonitoring();

                //Start Listener
                if (_listener == null)
                    _listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));

                if (_listener != null)
                {
                    _listener.Server.ReceiveBufferSize = Int32.MaxValue;
                    _listener.Start();
                }

                //Start Timer
                _timer.Interval = TimerInterval == 0
                    ? 100
                    : (Math.Abs(TimerInterval - _timer.Interval) > 0 ? TimerInterval : _timer.Interval);
                _timer.Enabled = true;
                _timer.Start();
                _isRunning = true;
                StartServerSuccessfull(Port, "");
            }
            catch (Exception ex)
            {
                StartServerFalse(Port, ex.ToString());
            }
        }

        public void StopServer()
        {
            try
            {
                if (!IsRunning) return;

                try
                {
                    _timer.Stop();
                    _timer.Enabled = false;

                    //_networkMonitorTimer.Stop();
                    //_networkMonitorTimer.Enabled = false;
                    //_monitor.StopMonitoring();

                    if (_listener != null)
                    {
                        _listener.Stop();
                        _listener = null;
                    }
                    _isRunning = false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    StopServerSuccessfull(Port, "StopServerSuccessfull");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DisposeServer()
        {
            try
            {
                _isRunning = false;
                _timer.Enabled = false;
                _timer.Stop();
                if (_listener != null)
                {
                    _listener.Stop();
                    _listener = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public event ClientStatusHandler BeginReciveData;
        public event ClientStatusHandler ClientConnected;
        public event ClientStatusHandler ClientDisconnected;
        public event ClientStatusHandler EndReciveData;
        public event IncommingData IncommingData;
        //public event NetworkChanged NetworkChanged;
        public event ServerStatusHandler StopServerSuccessfull;
        public event ServerStatusHandler StartServerFalse;
        public event ServerStatusHandler StartServerSuccessfull;

        #endregion

        #region Implementation of ITcpipServerHandler

        /// <summary>
        /// Sự kiện phát sinh khi bắt đầu nhận dữ liệu
        /// </summary>
        /// <param name="client"></param>
        public abstract void OnBeginReciveData(TcpClient client);

        /// <summary>
        /// Sự kiện phát sinh khi có client kết nối đến server
        /// </summary>
        /// <param name="client">Client </param>
        public abstract void OnClientConnected(TcpClient client);

        /// <summary>
        /// Sự kiện phát sinh khi client ngắt kết nối đến server
        /// </summary>
        /// <param name="client"></param>
        public abstract void OnClientDisconnected(TcpClient client);

        /// <summary>
        /// Sự kiện phát sinh khi kết thúc nhận dữ liệu
        /// </summary>
        /// <param name="client"></param>
        public abstract void OnEndReciveData(TcpClient client);

        /// <summary>
        /// Sự kiện phát sinh khi khởi tạo server lỗi
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public abstract void OnStartServerFalse(int port, string exceptionString);

        /// <summary>
        /// Sự kiện phát sinh khi khởi tạo server thành công
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public abstract void OnStartServerSuccessfull(int port, string exceptionString);

        /// <summary>
        /// Sự kiện phát sinh khi ngắt server thành công
        /// </summary>
        /// <param name="port"></param>
        /// <param name="exceptionString"></param>
        public abstract void OnStopServerSuccessfull(int port, string exceptionString);

        /// <summary>
        /// Sự kiện phát sinh khi có dữ liệu được gửi đến từ client
        /// </summary>
        /// <param name="dataHeader">Thông tin kết nối</param>
        /// <param name="data">dữ liệu nhận được</param>
        public abstract void OnIncommingData(DataHeader dataHeader, byte[] data);
        
        #endregion
    }
}