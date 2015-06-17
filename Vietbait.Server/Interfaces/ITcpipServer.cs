using System.Net.Sockets;
using Vietbait.Lablink.Utilities;

//using Vietbait.NetworkMonitor;

namespace Vietbait.Server.TCPIPServer
{

    #region Delegate Function to Handles Events

    public delegate void ServerStatusHandler(int port, string exceptionString);

    public delegate void ClientStatusHandler(TcpClient tcpClient);

    public delegate void IncommingData(DataHeader dataHeader, byte[] data);

    //public delegate void NetworkChanged(NetworkStatusMonitor.StatusMonitorEventArgs lastStatusMonitorEventArgs);

    #endregion

    public interface ITcpipServer
    {
        #region Properties

        int DelayTime { get; set; }
        int Port { get; set; }
        int TimerInterval { get; set; }

        #endregion

        #region Method

        void StartServer();
        void StopServer();
        void DisposeServer();

        #endregion

        #region Declare Events for Server

        event ClientStatusHandler BeginReciveData;
        event ClientStatusHandler ClientConnected;
        event ClientStatusHandler ClientDisconnected;
        event ClientStatusHandler EndReciveData;
        event ServerStatusHandler StopServerSuccessfull;
        event ServerStatusHandler StartServerFalse;
        event ServerStatusHandler StartServerSuccessfull;
        event IncommingData IncommingData;
        //event NetworkChanged NetworkChanged;

        #endregion
    }
}