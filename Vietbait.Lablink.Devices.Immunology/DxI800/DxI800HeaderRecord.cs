using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class DxI800HeaderRecord : AstmBaseRecord
    {
        #region Header Properties
        
        public AstmField DelimiterDefinition = new AstmField(1);
        public AstmField SenderNameOrId = new AstmField(4);
        public AstmField ReceiverId = new AstmField(9);
        //public AstmField CommentOrSpecialInstructions = new AstmField(10);
        public AstmField ProcessingId = new AstmField(11);
        public AstmField VersionNo = new AstmField(12, "1");
        public AstmField DateTime = new AstmField(13);

        #endregion

        #region Constructor

        public DxI800HeaderRecord() : this(string.Empty)
        {
        }

        public DxI800HeaderRecord(string rawString)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 13;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}