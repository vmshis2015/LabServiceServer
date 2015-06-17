namespace Vietbait.Lablink.Devices
{
    public interface IDeviceRecord
    {
        char FieldDelimiter { get; set; }

        char RepeatDelimiter { get; set; }

        char ComponentDelimiter { get; set; }

        char EscDelimiter { get; set; }

        string RecordType { get; set; }

        char RecordDelimiter { get; set; }

        //public DeviceRecord()
        //{
        //    FieldDelimiter = '|';
        //    RepeatDelimiter = '\\';
        //    ComponentDelimiter = '^';
        //    ESCDelimiter = '&';
        //}

        bool ParseData(string[] sInput);
        string CreateData();
    }
}