using System.IO.Ports;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Server.Rs232Server
{

    #region Delegate function to handler event

    public delegate void ServerStatusHandler(string message);

    public delegate void IncommingDataHandler(DataReceiveRs232 obj);

    public delegate void EndReceiveDataHanler(SerialPort port);

    #endregion

    internal interface IRs232Server
    {
        #region properties

        #endregion

        #region Public Method

        bool AddPort(string portName, int baudRate, int dataBits, StopBits stopbits, Parity parity, Handshake handshake,
            bool rtsEnable, bool dtrEnable, int timerInterval);

        void StartServer();
        void StopServer();

        #endregion

        #region Events

        event ServerStatusHandler ServerStartSuccess;
        event ServerStatusHandler ServerStartFalse;
        event IncommingDataHandler IncommingData;
        event EndReceiveDataHanler EndReceiveData;

        #endregion
    }
}