using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CI4100HeaderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField DelimiterDefinition = new AstmField(1, @"\^&");
        public AstmField ProcessingId = new AstmField(11, "P");
        public AstmField VersionNo = new AstmField(12, "1");

        #endregion

        #region Constructor

        public CI4100HeaderRecord() //: this(string.Empty)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 12;
            DefaultValue = string.Empty;
        }

        //public CI4100HeaderRecord(string rawString)
        //{
        //    RecordType = new AstmField(0, "H");
        //    RecordLength = 12;
        //    DefaultValue = string.Empty;
        //    Parse(rawString);
        //}

        #endregion
    }
}