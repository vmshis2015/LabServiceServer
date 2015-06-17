using System;
using System.Collections.Generic;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices.Biochemistry
{
    public class ADVIA1650OrderRecord : OrderRecord
    {
        #region Overrides of OrderRecord

        public override FieldOfRecord SegNumber
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override FieldOfRecord SpecId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override FieldOfRecord TestIdString
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string[] TestId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override char FieldDelimiter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override char RepeatDelimiter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override char ComponentDelimiter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override char EscDelimiter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string RecordType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override char RecordDelimiter
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool ParseData(string[] sInput)
        {
            throw new NotImplementedException();
        }

        public override string CreateData()
        {
            throw new NotImplementedException();
        }

        public string CreateData(int frameNo, string barcode, List<string> listInput)
        {
            try
            {
                var sTemp = new string[14 + listInput.Count];
                frameNo = (frameNo + 1)%8;
                sTemp[0] = frameNo.ToString("D1");
                sTemp[1] = "O";
                sTemp[2] = " ";
                sTemp[3] = "01";
                sTemp[4] = "01";
                sTemp[5] = listInput.Count.ToString("D3");
                sTemp[6] = "N";
                sTemp[7] = "0";
                sTemp[8] = barcode.PadRight(13);
                sTemp[9] = string.Concat(new string(' ', 39));
                sTemp[10] = "M";
                sTemp[11] = String.Concat(new string(' ', 11), " 1.0");
                sTemp[12] = "1";
                sTemp[13] = "1";
                for (byte i = 0; i < listInput.Count; i++)
                {
                    sTemp[14 + i] = string.Concat(listInput[i], "M");
                }
                string orderRecord = string.Concat(DeviceHelper.STX.ToString(), string.Concat(sTemp), " ",
                    DeviceHelper.ETX.ToString());
                string checksum = DeviceHelper.GetCheckSumValue(orderRecord);
                return string.Concat(orderRecord, checksum, DeviceHelper.CRLF);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}