using System;
using System.IO.Ports;

namespace Vietbait.Lablink.Utilities
{
    public class Device
    {
        public Device()
        {
            DeviceGuid = Guid.NewGuid();
            DeviceType = ConnectorType.RS232;
            Status = true;
            DeviceName = string.Empty;
            DeviceAsembly = string.Empty;
            DeviceNameSpace = string.Empty;
            Connector = string.Empty;
            BaudRate = 9600;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            Handshake = Handshake.None;
            RtsEnable = false;
            DtrEnable = false;
            TimeInterval = 100;
        }

        /// <summary>
        ///     Loại kết nối (RS232 / TCPIP)
        /// </summary>
        public ConnectorType DeviceType { get; set; }

        public Guid DeviceGuid { get; set; }

        /// <summary>
        ///     Trạng Thái thiết bị
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///     Tên Thiết bị
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        ///     Thư viện chứa thiết bị
        /// </summary>
        public string DeviceAsembly { get; set; }

        /// <summary>
        ///     Class điều khiển thiết bị
        /// </summary>
        public string DeviceNameSpace { get; set; }

        /// <summary>
        ///     Tên cổng COM hoặc IP của máy được kết nối
        /// </summary>
        public string Connector { get; set; }

        /// <summary>
        ///     BaudRate của cổng COM
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        ///     Databits của cổng COM
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        ///     StopBit của cổng Com
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        ///     Parity của cổng COM
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        ///     Handshake của cổng Com
        /// </summary>
        public Handshake Handshake { get; set; }

        /// <summary>
        ///     RTS Enable
        /// </summary>
        public bool RtsEnable { get; set; }

        /// <summary>
        ///     DTR Enable
        /// </summary>
        public bool DtrEnable { get; set; }

        /// <summary>
        ///     Timmer Interval
        /// </summary>
        public int TimeInterval { get; set; }
    }

    /// <summary>
    ///     Loại kết nối của thiết bị
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        ///     RS232 = 1
        /// </summary>
        RS232 = 1,

        /// <summary>
        ///     TCPIP = 2
        /// </summary>
        TCPIP = 2
    }
}