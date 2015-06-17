using System.IO.Ports;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Server.Rs232Server
{
    internal interface IRs232ServerHandler
    {
        void OnServerStartSuccess(string portName);
        void OnServerStartFalse(string portName);
        void OnIncommingData(DataReceiveRs232 obj);
        void OnEndReceiveData(SerialPort port);
    }
}