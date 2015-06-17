using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Vietbait.Lablink.Devices;
using Vietbait.Lablink.Utilities;
using Vietbait .Server .FileWatcherServer ;

namespace Vietbait.Lablink.Service
{
    internal class LablinkFileWatcherServer:FileWatcherServer
    {

        #region Attributies

        private readonly Dictionary<string, object> _colDevices;
        private readonly Dictionary<string, string> _colDevicesName;

        #endregion

        #region Contructor

        public LablinkFileWatcherServer(Dictionary<string, object> colDevices, Dictionary<string, string> colDevicesName)
        {
            _colDevices = colDevices;
            _colDevicesName = colDevicesName;
        }

        #endregion

        #region Private Method

        private BaseDevice GetDevice(string deviceConnector)
        {
            return (BaseDevice) _colDevices[_colDevicesName[deviceConnector]];
        }

        #endregion

        public override void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                var deviceConnector = ((FileSystemWatcher) source).Path;
                var device = GetDevice(deviceConnector);
                if (e.ChangeType == WatcherChangeTypes.Deleted) return;
                device.AddStringData(File.ReadAllText(e.FullPath));
                device.RaiseEventEndReceiveData(new DataReceiveFile(deviceConnector ,e.FullPath,""));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void OnRenamed(object source, RenamedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
