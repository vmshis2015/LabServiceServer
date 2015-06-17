using System;
using System.Linq;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class DxI800RequestInformationRecord : AstmBaseRecord
    {
        #region Header Properties
        
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField StartingRangeIdNumber = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);
        public AstmField RequestInformationStatusCodes = new AstmField(12);

        #endregion

        #region Constructor

        public DxI800RequestInformationRecord()
            : this(string.Empty)
        {
        }

        public DxI800RequestInformationRecord(string rawString)
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
            string barcode = string.Empty;
            barcode = StartingRangeIdNumber.Data.Split(Rules.ComponentDelimiter)[1].Trim();
            return barcode;
        }

        #endregion
    }
}