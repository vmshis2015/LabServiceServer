using System.Net.Sockets;
using Vietbait.Lablink.Utilities;

//using Vietbait.NetworkMonitor;

namespace Vietbait.Server.TCPIPServer
{
    internal interface ITcpipServerHandler
    {
        void OnBeginReciveData(TcpClient client);
        void OnClientConnected(TcpClient client);
        void OnClientDisconnected(TcpClient client);
        void OnEndReciveData(TcpClient client);
        void OnStartServerFalse(int port, string exceptionString);
        void OnStartServerSuccessfull(int port, string exceptionString);
        void OnStopServerSuccessfull(int port, string exceptionString);
        void OnIncommingData(DataHeader dataHeader, byte[] data);
        //void OnNetworkChanged(NetworkStatusMonitor.StatusMonitorEventArgs lastStatusMonitorEventArgs);
    }
}