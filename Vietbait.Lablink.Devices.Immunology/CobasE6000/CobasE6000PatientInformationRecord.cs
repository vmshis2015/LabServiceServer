using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CobasE6000PatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField DateOfBirth = new AstmField(7);
        public AstmField LaboratoryAssignedPatientId = new AstmField(3);
        public AstmField PatientSex = new AstmField(8);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField SpecialField1 = new AstmField(14);

        #endregion

        #region Constructor

        public CobasE6000PatientInformationRecord() : this(string.Empty)
        {
        }

        public CobasE6000PatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 34;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}