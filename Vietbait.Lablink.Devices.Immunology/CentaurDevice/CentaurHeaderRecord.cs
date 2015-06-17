using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology.CentaurDevice
{
    internal class CentaurHeaderRecord : IHeaderRecord
    {
        #region Implementation of IHeaderRecord

        public string sHeaderRecord { get; set; }

        #endregion

        public CentaurHeaderRecord()
        {
            sHeaderRecord = string.Concat("H|\\^&", DeviceHelper.CR);
        }
    }
}