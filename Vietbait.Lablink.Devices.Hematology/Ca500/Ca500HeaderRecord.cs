namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500HeaderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField AstmVersionNo = new AstmField(12, "1");
        public AstmField DelimiterDefinition = new AstmField(1, @"\^&");
        public AstmField ReceiverId = new AstmField(9);
        public AstmField SenderNameOrId = new AstmField(4);

        #endregion

        #region Constructor

        public Ca500HeaderRecord() //: this(string.Empty)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 12;
            DefaultValue = string.Empty;
        }

        #endregion
    }
}