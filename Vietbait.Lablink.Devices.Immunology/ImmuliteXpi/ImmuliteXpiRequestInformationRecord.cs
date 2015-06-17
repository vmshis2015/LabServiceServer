using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class ImmuliteXpiRequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties

        /// <summary>
        ///     Ｏ: Request sample inquire
        ///     Ａ : Cancel current inquire
        /// </summary>
        public AstmField RequestInformationStatusCodes = new AstmField(12, "O");

        public AstmField SequenceNumber = new AstmField(1);
        public AstmField StartingRangeIdNumber = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public ImmuliteXpiRequestInformationRecord()
            : this(string.Empty)
        {
        }

        public ImmuliteXpiRequestInformationRecord(string rawString)
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
            result = StartingRangeIdNumber.Data.Split(Rules.ComponentDelimiter)[1].Trim();
            return result;
        }

        #endregion
    }
}