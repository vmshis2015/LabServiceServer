using System;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    [Serializable]
    public class Bs800ResultRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField DateTimeTestCompleted = new AstmField(12);
        public AstmField DateTimeTestStarted = new AstmField(11);
        public AstmField InstrumentIdentification = new AstmField(13);
        public AstmField MeasurementValue = new AstmField(3);
        public AstmField OperatorIdentification = new AstmField(10);
        public AstmField OriginalMeasurementValue = new AstmField(9);
        public AstmField ResultAbnormalFlag = new AstmField(6);
        public AstmField ResultStatus = new AstmField(8);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField Units = new AstmField(4);
        public AstmField UniversalTestId = new AstmField(2);

        #endregion

        #region Constructor

        public Bs800ResultRecord() : this(string.Empty)
        {
        }

        public Bs800ResultRecord(string rawString)
        {
            RecordType = new AstmField(0, "R");
            RecordLength = 13;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion

        #region Public Method

        public ResultItem GetResult()
        {
            var result = new ResultItem();

            //lấy về test name 
            string[] testnameArr = UniversalTestId.Data.Split(Rules.ComponentDelimiter);
            result.TestName = testnameArr[0].Trim();
            result.TestDataId = result.TestName;
            result.MeasureUnit = Units.Data.Trim();
            string[] tempResult = MeasurementValue.Data.Split(Rules.ComponentDelimiter);
            result.TestValue = tempResult[0];
            return result;
        }

        #endregion
    }
}