using System;
using System.Collections.Generic;
using System.Linq;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class ImmuliteXpiTestOrderRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField InstrumentSectionId = new AstmField(24);
        public AstmField OrderingPhysician = new AstmField(16);
        public AstmField Priority = new AstmField(5, "R");
        public AstmField RelevantClinicalInformation = new AstmField(13);
        public AstmField RequestedDateAndTime = new AstmField(6);
        public AstmField SequenceNumber = new AstmField(1, "1");
        public AstmField SpecimenCollectionDateAndTime = new AstmField(7);
        public AstmField SpecimenId = new AstmField(2);
        public AstmField UniversalTestId = new AstmField(4);

        #endregion

        #region Constructor

        public ImmuliteXpiTestOrderRecord() : this(string.Empty)
        {
        }

        public ImmuliteXpiTestOrderRecord(string rawString)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 30;
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