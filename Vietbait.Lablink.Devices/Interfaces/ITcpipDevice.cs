using System.Net.Sockets;

namespace Vietbait.Lablink.Devices
{
    internal interface ITcpipDevice
    {
        #region Properties

        NetworkStream NetStream { get; }

        #endregion
    }
}