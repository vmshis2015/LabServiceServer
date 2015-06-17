namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500TestOrderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField ActionCode = new AstmField(11, "N");
        public AstmField InstrumentSpecimenId = new AstmField(3);
        public AstmField OrderDateAndTime = new AstmField(6);
        public AstmField Priority = new AstmField(5);
        public AstmField SequenceNo = new AstmField(1, "1");
        public AstmField SpecimenId = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public Ca500TestOrderRecord() //: this(string.Empty)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 11;
            DefaultValue = string.Empty;
        }

        public Ca500TestOrderRecord(string[] rawData) //: this(string.Empty)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 11;
            DefaultValue = string.Empty;
            Parse(rawData);
        }

        #endregion

        public string GetBarcode()
        {
            string[] tempStr = InstrumentSpecimenId.Data.Split(Rules.ComponentDelimiter);
            return tempStr.Length >= 3 ? tempStr[2].Trim() : string.Empty;
        }
    }
}