namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500PatientRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField SequenceNo = new AstmField(1, "1");

        #endregion

        #region Constructor

        public Ca500PatientRecord()
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 1;
            DefaultValue = string.Empty;
        }

        public Ca500PatientRecord(string[] rawData)
        {
            RecordType = new AstmField(0, "P");
            RecordLength = 1;
            DefaultValue = string.Empty;
            Parse(rawData);
        }

        #endregion
    }
}