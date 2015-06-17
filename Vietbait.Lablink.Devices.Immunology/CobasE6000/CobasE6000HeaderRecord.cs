using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CobasE6000HeaderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField CommentOrSpecialInstructions = new AstmField(10);
        public AstmField DelimiterDefinition = new AstmField(1);
        public AstmField ProcessingId = new AstmField(11);
        public AstmField ReveiverId = new AstmField(9);
        public AstmField SenderNameOrId = new AstmField(4);
        public AstmField VersionNo = new AstmField(12, "1");

        #endregion

        #region Constructor

        public CobasE6000HeaderRecord() : this(string.Empty)
        {
        }

        public CobasE6000HeaderRecord(string rawString)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 12;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}