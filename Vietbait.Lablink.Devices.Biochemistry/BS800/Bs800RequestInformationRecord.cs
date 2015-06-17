using System;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    [Serializable]
    public class Bs800RequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField PatientIdAndSpecimenId = new AstmField(2);

        /// <summary>
        ///     Ｏ: Request sample inquire
        ///     Ａ : Cancel current inquire
        /// </summary>
        public AstmField RequestInformationStatusCodes = new AstmField(12);

        public AstmField SequenceNumber = new AstmField(1);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public Bs800RequestInformationRecord()
            : this(string.Empty)
        {
        }

        public Bs800RequestInformationRecord(string rawString)
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
            result = PatientIdAndSpecimenId.Data.Split(Rules.ComponentDelimiter)[1].Trim();
            return result;
        }

        #endregion
    }
}