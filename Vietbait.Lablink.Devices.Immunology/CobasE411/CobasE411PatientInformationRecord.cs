using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CobasE411PatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField SequenceNumber = new AstmField(1);

        #endregion

        #region Constructor

        public CobasE411PatientInformationRecord() : this(string.Empty)
        {
        }

        public CobasE411PatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 1;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}