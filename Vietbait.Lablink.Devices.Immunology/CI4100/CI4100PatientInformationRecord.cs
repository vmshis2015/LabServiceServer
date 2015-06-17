using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CI4100PatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        //public AstmField LaboratoryAssignedPatientId = new AstmField(3);
        public AstmField PatientName = new AstmField(5);
        public AstmField SequenceNumber = new AstmField(1, "1");
        //public AstmField DateOfBirth  = new AstmField(7);
        //public AstmField PatientSex  = new AstmField(8);

        #endregion

        #region Constructor

        public CI4100PatientInformationRecord() : this(string.Empty)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 26;
            DefaultValue = string.Empty;
        }

        public CI4100PatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 34;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        public CI4100PatientInformationRecord(string[] rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 34;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}