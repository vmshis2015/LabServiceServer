using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class Access2PatientInformationRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField AttendingPhysicianName = new AstmField(13);
        public AstmField BodySurfaceArea = new AstmField(15);
        public AstmField DateOfBirth = new AstmField(7);
        public AstmField Location = new AstmField(25);
        public AstmField NatureOfAltDiagCodeAndClass = new AstmField(26);
        public AstmField PatientAddress = new AstmField(10);
        public AstmField PatientDiagnosis = new AstmField(18);
        public AstmField PatientDiet = new AstmField(20);
        public AstmField PatientId = new AstmField(3);
        public AstmField PatientMedications = new AstmField(19);
        public AstmField PatientName = new AstmField(5);
        public AstmField PatientRace = new AstmField(9);
        public AstmField PatientSex = new AstmField(8);
        public AstmField PatientTelephone = new AstmField(12);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField SpecialField1 = new AstmField(14);

        #endregion

        #region Constructor

        public Access2PatientInformationRecord() : this(string.Empty)
        {
        }

        public Access2PatientInformationRecord(string rawString)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 34;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion
    }
}