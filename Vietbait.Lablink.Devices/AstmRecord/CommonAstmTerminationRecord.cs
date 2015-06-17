using System;

namespace Vietbait.Lablink.Devices
{
    [Serializable]
    public class CommonAstmTerminationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField SequenceNumber = new AstmField(1, "1");
        public AstmField TerminationCode = new AstmField(2, "N");

        #endregion

        #region Constructor

        public CommonAstmTerminationRecord() : this(string.Empty)
        {
        }

        public CommonAstmTerminationRecord(string rawString)
        {
            RecordType = new AstmField(0, "L");
            RecordLength = 2;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}