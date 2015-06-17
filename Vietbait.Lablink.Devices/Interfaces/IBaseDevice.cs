using System.Workflow.Activities;
using NLog;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices
{
    public delegate void MyEventHandler(object obj);

    [ExternalDataExchange]
    public interface IBaseDevice
    {
        #region Properties

        // Readonly Properties
        string StringData { get; }

        byte[] ByteData { get; }

        string DeviceName { get; }

        int TestTypeId { get; }

        Logger Log { get; set; }

        #endregion

        #region Events

        event MyEventHandler BeginReceiveData;
        event MyEventHandler EndReceiveData;
        event MyEventHandler IncommingData;
        event MyEventHandler ClientConnected;

        #endregion

        #region Method

        //Must override Methods
        bool SendByte(byte byteData);
        bool SendBytesData(byte[] bytesData);
        bool SendStringData(string stringData);

        //Public Method
        bool InitialDevice();
        void AddData(byte[] byteData);
        void ProcessRawData();
        void RaiseEventBeginReceiveData(object obj);
        void RaiseEventEndReceiveData(object obj);
        void RaiseEventClientConnected(object obj);
        void ClearData();
        void AddResult(ResultItem resultItem);
        bool ImportResults();

        #endregion
    }
}