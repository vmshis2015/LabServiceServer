namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500TerminationRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField SequenceNo = new AstmField(1, "1");
        public AstmField TerminationCord = new AstmField(2, "N");

        #endregion

        #region Constructor

        public Ca500TerminationRecord() //: this(string.Empty)
        {
            RecordType = new AstmField(0, "L");
            RecordLength = 2;
            DefaultValue = string.Empty;
        }

        #endregion
    }
}