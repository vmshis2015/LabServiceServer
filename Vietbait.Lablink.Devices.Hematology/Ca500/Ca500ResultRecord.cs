using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Hematology
{
    internal class Ca500ResultRecord : AstmBaseRecord
    {
        #region Header Properties

        public AstmField DataOrMeasurementValue = new AstmField(3);
        public AstmField DateTimeTestCompleted = new AstmField(12);
        public AstmField ResultAbnormalFlags = new AstmField(6);
        public AstmField SequenceNo = new AstmField(1, "1");
        public AstmField Units = new AstmField(4);
        public AstmField UniversalTestId = new AstmField(2);

        #endregion

        #region Constructor

        public Ca500ResultRecord()
        {
            RecordType = new AstmField(0, "R");
            RecordLength = 12;
            DefaultValue = string.Empty;
        }

        public Ca500ResultRecord(string[] rawData)
        {
            RecordType = new AstmField(0, "R");
            RecordLength = 12;
            DefaultValue = string.Empty;
            Parse(rawData);
        }

        #endregion

        #region Public Method

        public ResultItem GetResult()
        {
            var item = new ResultItem();
            string[] tempStr = UniversalTestId.Data.Split(Rules.ComponentDelimiter);
            item.TestName = tempStr[3];
            item.TestValue = DataOrMeasurementValue.Data.Trim();
            item.MeasureUnit = Units.Data;
            return item;
        }

        #endregion
    }
}