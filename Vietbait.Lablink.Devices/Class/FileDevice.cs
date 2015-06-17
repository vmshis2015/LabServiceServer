using System;
using System.IO.Ports;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    public abstract class FileDevice : BaseDevice
    {
        public string SourceFolder;
        public string FullFilePath;
        public string LogPath;
        public string FileName;
        public string UserName;
        public string Password;

        #region Contructor

        #endregion

        #region Overrides of BaseDevice

        public override void OnEndReceiveData(object obj)
        {
            try
            {
                var dataReceiveFile = (DataReceiveFile) obj;
                SourceFolder = dataReceiveFile.SourceFolder;
                FullFilePath = dataReceiveFile.FullFilePath;
                LogPath = dataReceiveFile.LogPath;
                FileName = dataReceiveFile.FileName;
                ProcessRawData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public override bool SendBytesData(byte[] bytesData)
        {
            throw new NotImplementedException();
        }

        public override bool SendStringData(string stringData)
        {
            throw new NotImplementedException();
        }

        public override void OnBeginReceiveData(object obj)
        {
            throw new NotImplementedException();
        }

        public override void OnInCommingData(object obj)
        {
            throw new NotImplementedException();
        }

        public override bool SendByte(byte byteData)
        {
            throw new NotImplementedException();
        }

        public override void SetNetStream(SerialPort port)
        {
            throw new NotImplementedException();
        }

        public override void OnClientConnected(object obj)
        {
            throw new NotImplementedException();
        }
    }
}