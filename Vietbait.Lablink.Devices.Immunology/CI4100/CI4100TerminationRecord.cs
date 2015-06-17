using System;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class CI4100TerminationRecord : CommonAstmTerminationRecord
    {
        public CI4100TerminationRecord()
        {
            RecordType = new AstmField(0, "L");
            RecordLength = 1;
            DefaultValue = string.Empty;
        }
    }
}