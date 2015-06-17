using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CI4100RequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField IDNumber = new AstmField(2);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField StatusCode = new AstmField(12);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public CI4100RequestInformationRecord()
            : this(string.Empty)
        {
        }

        public CI4100RequestInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "Q");
            RecordLength = 12;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        public CI4100RequestInformationRecord(string[] rawString)
        {
            RecordType = new AstmField(0, "Q");
            RecordLength = 12;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion

        #region Public Method

        public string GetOrderBarcode()
        {
            string result = string.Empty;
            result = IDNumber.Data.Split(Rules.ComponentDelimiter)[1].Trim();
            return result;
        }

        #endregion
    }
}