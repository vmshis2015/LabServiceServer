using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class ImmuliteXpiPatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField Birthdate = new AstmField(7);
        public AstmField PatientName = new AstmField(5);
        public AstmField PatientSex = new AstmField(8);
        public AstmField PracticeAssignedPatientID = new AstmField(2);
        public AstmField SequenceNumber = new AstmField(1, "1");
        //public AstmField PatientRace = new AstmField(9);
        //public AstmField PatientAddress = new AstmField(10);
        //public AstmField PatientTelephone = new AstmField(12);
        //public AstmField AttendingPhysicianName = new AstmField(13);
        //public AstmField SpecialField1 = new AstmField(14);
        //public AstmField BodySurfaceArea = new AstmField(15);
        //public AstmField PatientDiagnosis = new AstmField(18);
        //public AstmField PatientMedications = new AstmField(19);
        //public AstmField PatientDiet = new AstmField(20);
        //public AstmField Location = new AstmField(25);
        //public AstmField NatureOfAltDiagCodeAndClass = new AstmField(26);

        #endregion

        #region Constructor

        public ImmuliteXpiPatientInformationRecord() : this(string.Empty)
        {
        }

        public ImmuliteXpiPatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 13;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}