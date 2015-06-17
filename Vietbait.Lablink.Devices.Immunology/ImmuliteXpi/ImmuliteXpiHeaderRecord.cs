using System;

namespace Vietbait.Lablink.Devices.Immunology.ImmuliteXpi
{
    [Serializable]
    public class ImmuliteXpiHeaderRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField AccessPassword = new AstmField(3);
        public AstmField AstmVersion = new AstmField(12, "1");
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
        public AstmField ProcessingId = new AstmField(11, "P");

        public AstmField ReceiverID = new AstmField(9, "");

        public AstmField SenderNameOrId = new AstmField(4, "");
        public AstmField SenderStreetAddress = new AstmField(5);
        public AstmField SendersTelephoneNumber = new AstmField(7);

        #endregion

        #region Constructor

        public ImmuliteXpiHeaderRecord() : this(string.Empty)
        {
        }

        public ImmuliteXpiHeaderRecord(string rawString)
        {
            RecordType = new AstmField(0, "H");
            RecordLength = 13;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}