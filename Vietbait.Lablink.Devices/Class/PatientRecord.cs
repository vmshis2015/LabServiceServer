namespace Vietbait.Lablink.Devices
{
    public abstract class PatientRecord : IDeviceRecord
    {
        public abstract FieldOfRecord SegNumber { get; set; }
        public abstract FieldOfRecord PraticePid { get; set; }
        public abstract FieldOfRecord PatientName { get; set; }
        public abstract char FieldDelimiter { get; set; }
        public abstract char RepeatDelimiter { get; set; }
        public abstract char ComponentDelimiter { get; set; }
        public abstract char EscDelimiter { get; set; }
        public abstract string RecordType { get; set; }
        public abstract char RecordDelimiter { get; set; }
        public abstract bool ParseData(string[] sInput);
        public abstract string CreateData();
    }
}