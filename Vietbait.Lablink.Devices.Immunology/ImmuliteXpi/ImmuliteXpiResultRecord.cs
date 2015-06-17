using System;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class ImmuliteXpiResultRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField DateTimeTestCompleted = new AstmField(12);
        public AstmField DateTimeTestStarted = new AstmField(11);
        public AstmField InstrumentIdentification = new AstmField(13);
        public AstmField MeasurementValue = new AstmField(3);
        public AstmField NatureOfAbnormalityTesting = new AstmField(7);
        public AstmField OperatorId = new AstmField(10);
        public AstmField ReferenceRanges = new AstmField(5);
        public AstmField ResultAbnormalFlag = new AstmField(6);
        public AstmField ResultStatus = new AstmField(8);
        public AstmField SequenceNumber = new AstmField(1);
        public AstmField Units = new AstmField(4);
        public AstmField UniversalTestId = new AstmField(2);

        #endregion

        #region Constructor

        public ImmuliteXpiResultRecord() : this(string.Empty)
        {
        }

        public ImmuliteXpiResultRecord(string rawString)
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
            result.TestName = testnameArr[3].Trim();
            result.TestDataId = result.TestName;
            result.MeasureUnit = Units.Data.Trim();
            string[] tempResult = MeasurementValue.Data.Split(Rules.ComponentDelimiter);
            result.TestValue = tempResult[0];
            return result;
        }

        #endregion
    }
}