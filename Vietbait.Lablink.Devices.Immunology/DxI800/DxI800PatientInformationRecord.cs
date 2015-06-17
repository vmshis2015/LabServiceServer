using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class DxI800PatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField SequenceNumber = new AstmField(1);
        public AstmField PracticeAssignedPatientID = new AstmField(2);

        #endregion

        #region Constructor

        public DxI800PatientInformationRecord() : this(string.Empty)
        {
        }

        public DxI800PatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 2;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}