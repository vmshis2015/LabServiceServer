namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500RequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField BeginningRequestDateAndTime = new AstmField(6);
        public AstmField NatureOfRequestDayLimit = new AstmField(5, "0");
        public AstmField SequenceNo = new AstmField(1, "1");
        public AstmField StartingRangeIdNo = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public Ca500RequestInformationRecord() //: this(string.Empty)
        {
            RecordType = new AstmField(0, "Q");
            RecordLength = 6;
            DefaultValue = string.Empty;
        }

        public Ca500RequestInformationRecord(string[] rawData) //: this(string.Empty)
        {
            RecordType = new AstmField(0, "Q");
            RecordLength = 6;
            DefaultValue = string.Empty;
            Parse(rawData);
        }

        #endregion
    }
}