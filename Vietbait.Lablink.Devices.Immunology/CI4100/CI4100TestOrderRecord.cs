using System;
using System.Collections.Generic;
using System.Linq;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CI4100TestOrderRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField ActionCode = new AstmField(11);
        public AstmField InstrumentSpecimenId = new AstmField(3);
        public AstmField ReportTypes = new AstmField(25);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField SpecimenId = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);
        //public AstmField Priority = new AstmField(5);
        //public AstmField SpecimenCollectionDateAndTime  = new AstmField(7);

        #endregion

        #region Constructor

        public CI4100TestOrderRecord() : this(string.Empty)
        {
        }

        public CI4100TestOrderRecord(string rawString)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 26;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        public CI4100TestOrderRecord(string[] rawString)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 26;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion

        #region Public Method

        public string CreateUniversalTestid(List<string> pOrderList)
        {
            List<string> resultList =
                pOrderList.Select(s => string.Format("{0}{0}{0}{1}", Rules.ComponentDelimiter, s)).ToList();
            return string.Join(Rules.RepeatDelimiter.ToString(), resultList.ToArray());
        }

        #endregion
    }
}