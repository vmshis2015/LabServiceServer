using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Immunology
{
    public abstract class ASTMDeviceRecord : IDeviceRecord
    {
        #region Implementation of IDeviceRecord

        public abstract char FieldDelimiter { get; set; }
        public abstract char RepeatDelimiter { get; set; }
        public abstract char ComponentDelimiter { get; set; }
        public abstract char EscDelimiter { get; set; }
        public abstract string RecordType { get; set; }
        public abstract char RecordDelimiter { get; set; }
        public abstract bool ParseData(string[] sInput);
        public abstract string CreateData();

        #endregion
    }
}

