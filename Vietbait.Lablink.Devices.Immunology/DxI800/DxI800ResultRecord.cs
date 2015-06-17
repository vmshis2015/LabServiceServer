using System;
using Vietbait.Lablink.TestResult;

namespace Vietbait.Lablink.Devices.Immunology
{
    [Serializable]
    public class DxI800ResultRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField SequenceNumber = new AstmField(1);
        public AstmField UniversalTestId = new AstmField(2);
        public AstmField DataOrMeasurementValue = new AstmField(3);
        public AstmField Units = new AstmField(4);
        public AstmField ReferenceRanges = new AstmField(5);
        //public AstmField ReferenceType = new AstmField(6);
        public AstmField ResultAbnormalFlags = new AstmField(6);
        public AstmField NatureOfAbnormalityTesting = new AstmField(7);
        public AstmField ResultStatus = new AstmField(8);
        //public AstmField OperatorIdentification = new AstmField(10);
        //public AstmField DateTimeTestStarted = new AstmField(11);
        public AstmField DateTimeTestCompleted = new AstmField(12);

        #endregion

        #region Constructor

        public DxI800ResultRecord() : this(string.Empty)
        {
        }

        public DxI800ResultRecord(string rawString)
        {
            RecordType = new AstmField(0, "R");
            RecordLength = 12;
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
            if(result.TestName.Equals("FreeT4"))
            {
                result.TestName = "FRT4";
            }
            else if(result.TestName.Equals("FRT4"))
            {
                result.TestName = "Free";
            }
            result.TestDataId = result.TestName;
            result.MeasureUnit = Units.Data.Trim();
            result.TestValue = DataOrMeasurementValue.Data;
            return result;
        }

        #endregion
    }
}