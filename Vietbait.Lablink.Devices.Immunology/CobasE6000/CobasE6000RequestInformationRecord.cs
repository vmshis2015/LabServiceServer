using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CobasE6000RequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField RequestInformationStatusCodes = new AstmField(12);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField StartingRangeIdNumber = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public CobasE6000RequestInformationRecord()
            : this(string.Empty)
        {
        }

        public CobasE6000RequestInformationRecord(string rawString)
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
            result = StartingRangeIdNumber.Data.Split(Rules.ComponentDelimiter)[2].Trim();
            return result;
        }

        #endregion
    }
}