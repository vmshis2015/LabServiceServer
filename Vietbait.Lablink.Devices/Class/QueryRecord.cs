namespace Vietbait.Lablink.Devices
{
    public abstract class QueryRecord : IDeviceRecord
    {
        #region attributries

        public abstract FieldOfRecord SeqNumber { get; set; }
        public abstract string QueryBarcode { get; set; }
        public abstract FieldOfRecord StartingRangeId { get; set; }
        public abstract FieldOfRecord EndingRangeId { get; set; }
        public abstract FieldOfRecord TestIdString { get; set; }
        public abstract FieldOfRecord RequestInforStatusCode { get; set; }

        public abstract char FieldDelimiter { get; set; }
        public abstract char RepeatDelimiter { get; set; }
        public abstract char ComponentDelimiter { get; set; }
        public abstract char EscDelimiter { get; set; }
        public abstract string RecordType { get; set; }
        public abstract char RecordDelimiter { get; set; }

        #endregion

        public abstract bool ParseData(string[] sInput);

        public abstract string CreateData();
    }
}