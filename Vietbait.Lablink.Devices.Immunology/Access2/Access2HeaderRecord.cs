using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class Access2HeaderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField AstmVersion = new AstmField(12, "1394-97");
        public AstmField CurrentDate = new AstmField(13);
        public AstmField DelimiterDefinition = new AstmField(1, @"\^&");

        /// <summary>
        ///     PR (patient test result)
        ///     QR (QC test result)
        ///     CR (calibration result)
        ///     RQ (request query)
        ///     QA (query response)
        ///     SA (sample request information)
        /// </summary>
        public AstmField ProcessingId = new AstmField(11);

        public AstmField SenderNameOrId = new AstmField(4);

        #endregion

        #region Constructor

        public Access2HeaderRecord() : this(string.Empty)
        {
        }

        public Access2HeaderRecord(string rawString)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 13;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}