namespace Vietbait.Lablink.Devices
{
    public interface ITerminationRecord
    {
        string TerminationRecord { get; set; }
        byte SequenceNumber { get; set; }
        string TerminationCode { get; set; }
    }
}