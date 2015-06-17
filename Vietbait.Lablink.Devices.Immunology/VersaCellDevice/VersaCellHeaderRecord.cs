using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology
{
    internal class VersaCellHeaderRecord : IHeaderRecord
    {
        #region Implementation of IHeaderRecord

        public string sHeaderRecord { get; set; }

        #endregion

        public VersaCellHeaderRecord()
        {
            sHeaderRecord = string.Concat(@"H|\^&|||||||||", DeviceHelper.CR);
        }
    }
}