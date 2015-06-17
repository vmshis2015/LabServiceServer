using System;
using System.Collections.Generic;
using System.Linq;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    [Serializable]
    public class Bs800TestOrderRecord : AstmBaseRecord
    {
        #region Termination Properties

        public AstmField Collected = new AstmField(10);
        public AstmField CollectionVolume = new AstmField(9);
        public AstmField DateTimeSpecimenReceivedInTheLab = new AstmField(14);
        public AstmField InstrumentSpecimenId = new AstmField(3);
        public AstmField OfflineDilutionFactor = new AstmField(18, "1");
        public AstmField OrderingPhysician = new AstmField(16);
        public AstmField PhysicianPhoneNumber = new AstmField(17);
        public AstmField Priority = new AstmField(5, "R");

        /// <summary>
        ///     O（request from）
        ///     Q（query response）
        ///     F（final result）
        /// </summary>
        public AstmField ReportTypes = new AstmField(25, "O");

        public AstmField RequestedDateAndTime = new AstmField(6);
        public AstmField SampleId = new AstmField(2);
        public AstmField SequenceNumber = new AstmField(1, "1");
        public AstmField SpecimenCollectionDateAndTime = new AstmField(7);

        /// <summary>
        ///     serum
        ///     urine
        ///     CSF
        ///     plasma
        ///     timed
        ///     other
        ///     blood
        ///     amniotic
        ///     urethral
        ///     saliva
        ///     cervical
        ///     synovia
        /// </summary>
        public AstmField SpecimenType = new AstmField(15, "serum");

        public AstmField UniversalTestId = new AstmField(4);

        public AstmField UserField = new AstmField(19);

        #endregion

        #region Constructor

        public Bs800TestOrderRecord() : this(string.Empty)
        {
        }

        public Bs800TestOrderRecord(string rawString)
        {
            RecordType = new AstmField(0, "O");
            RecordLength = 30;
            DefaultValue = string.Empty;
            Parse(rawString);
        }

        #endregion

        #region Public Method

        public string CreateUniversalTestid(List<string> pOrderList)
        {
            List<string> resultList =
                pOrderList.Select(s => string.Format("{1}{0}Test1{0}2{0}1", Rules.ComponentDelimiter, s)).ToList();
            return string.Join(Rules.RepeatDelimiter.ToString(), resultList.ToArray());
        }

        #endregion
    }
}