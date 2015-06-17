using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using Microsoft.VisualBasic;
using Vietbait.Lablink.Utilities;

namespace Vietbait.Lablink.Devices
{
    /// <summary>
    ///     Class giành riêng cho máy Advia 2120
    /// </summary>
    public abstract class Advia2120Device : Rs232Base
    {
        #region Class Message

        internal class Message
        {
            /// <summary>
            ///     Parse new message
            /// </summary>
            /// <param name="rawData"></param>
            public Message(string rawData)
            {
                try
                {
                    Mt = (byte) Strings.Asc(rawData.Substring(1, 1));
                    Id = rawData.Substring(2, 1);
                    Data = rawData.Substring(3, rawData.Length - 7);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public byte Mt { get; set; }
            public string Id { get; set; }
            public string Data { get; set; }
        }

        #endregion

        #region Attributes

        private const int TimerInterval = 10000;

        // Declare MT Value
        private const byte MinMtValue = 0x30;
        private const byte MaxMtValue = 0x5A;
        private readonly BackgroundWorker _adviaWorker;

        /// <summary>
        ///     Timer to control flow
        /// </summary>
        private readonly Timer _myTimer;

        private readonly List<string> _resultList = new List<string>();

        public new string StringData;

        private byte _errorCount;

        private bool _initAdvia;
        private string _lastStringData = "";

        #endregion

        #region Contructor

        protected Advia2120Device()
        {
            _myTimer = new Timer(TimerInterval) {AutoReset = true};
            _myTimer.Elapsed += _myTimer_Elapsed;
            _myTimer.Start();

            _adviaWorker = new BackgroundWorker();
            _adviaWorker.DoWork += _adviaWorker_DoWork;
            _adviaWorker.RunWorkerCompleted += _adviaWorker_RunWorkerCompleted;
        }

        private void _adviaWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.Trace("Finish All result");
        }

        private void _adviaWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (_resultList.Count > 0)
            {
                StringData = _resultList[0];
                ProcessResultData();
                _resultList.RemoveAt(0);
            }
        }

        #endregion

        #region Private Method

        private void _myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _lastStringData = CreateIMessage();
            SendStringData(_lastStringData);
            if (_initAdvia) Log.Trace("Send I Message: {0}", _lastStringData);
            base.ClearData();
            StringData = string.Empty;
            _initAdvia = false;
        }

        /// <summary>
        ///     Hàm trả về chuỗi Checksum của một Frame.
        ///     Check khi có một trong hai giá trị ETX, ETB
        /// </summary>
        /// <param name="frame">Chuỗi cần kiểm tra</param>
        /// <returns>Chuỗi trả về là 2 ký tự dùng để checksum</returns>
        private byte GetCheckSumValue(string frame)
        {
            byte checksum = 0;
            for (int i = 0; i < frame.Length; i++)
            {
                var asc = (byte) Strings.Asc(frame[i]);
                checksum = i == 0 ? asc : (byte) (checksum ^ asc);
            }
            if (checksum == 0x03) checksum = 0x7F;
            //if ((checksum == 0x05) && (frame.Substring(1, 1)).ToUpper() == "S") checksum = 0x0D;
            return checksum;
        }

        /// <summary>
        ///     Create Message to send
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        private string CreateMessage(string inputMessage)
        {
            if (!inputMessage.EndsWith(DeviceHelper.CRLF))
                inputMessage = string.Format("{0}{1}", inputMessage, DeviceHelper.CRLF);
            char checksumValue = Strings.Chr(GetCheckSumValue(inputMessage));
            return string.Format("{0}{1}{2}{3}", DeviceHelper.STX, inputMessage, checksumValue, DeviceHelper.ETX);
        }

        private string CreateIMessage()
        {
            string raw = string.Format("0I ");
            return CreateMessage(raw);
        }

        private string CreateZMessage(byte mt)
        {
            string raw = string.Format("{0}Z{1} 0", Strings.Chr(mt), Strings.Space(17));
            return CreateMessage(raw);
        }

        private string CreateSMessage(byte mt)
        {
            string raw = string.Format("{0}S{1}", Strings.Chr(mt), Strings.Space(10));
            return CreateMessage(raw);
        }

        #endregion

        #region Public Method

        /// <summary>
        ///     Hàm xử lý dữ liệu khi các máy giao tiếp theo ASTM gửi đến
        /// </summary>
        public override void ProcessRawData()
        {
            try
            {
                // Stop Timer
                _myTimer.Stop();

                string stringdata = base.StringData;

                Log.Trace("Recv string data:{0}", stringdata);

                if (stringdata.Length == 1)
                {
                    var b = (byte) Strings.Asc(stringdata[0]);
                    // Nếu nhận được NAK
                    if (stringdata.StartsWith(string.Format("{0}", DeviceHelper.NAK)))
                    {
                        if (_errorCount >= 6) return;
                        base.ClearData();
                        SendStringData(_lastStringData);
                        _errorCount++;
                        _initAdvia = false;
                        return;
                    }
                    else if (b != 0)
                    {
                        if (!_initAdvia)
                        {
                            b = (byte) (b + 1);
                            _lastStringData = CreateSMessage(b);
                            SendStringData(_lastStringData);
                            _initAdvia = true;
                            Log.Trace("Send S Message in Init pharse:{0}", _lastStringData);
                            base.ClearData();
                            _myTimer.Start();
                            Log.Trace("Started Timer after Init Advia");
                        }
                    }
                    else
                    {
                        _myTimer.Start();
                    }
                    base.ClearData();
                    return;
                }
                if (!stringdata.EndsWith(DeviceHelper.ETX.ToString())) return;
                // Lấy dữ liệu từ STX đến ETX
                int idStx = stringdata.IndexOf(DeviceHelper.STX);
                int idetx = stringdata.IndexOf(DeviceHelper.ETX);
                //stringdata = DeviceHelper.SeperatorRawData(stringdata, DeviceHelper.STX, DeviceHelper.ETX)[0];
                stringdata = stringdata.Substring(idStx, idetx - idStx + 1);

                // Kiểm tra checksum của đoạn dữ liệu
                byte checksumValue = GetCheckSumValue(stringdata.Substring(1, stringdata.Length - 3));

                // Lấy chuỗi checksum từ string
                var checksumFromString = (byte) Strings.Asc(stringdata.Substring(stringdata.Length - 2, 1));

                // Kiểm tra checksum
                if (checksumValue != checksumFromString)
                {
                    // Khi dữ liệu lỗi gửi lại NAK
                    Log.Trace("Checksum False:{0}<>{1}", Strings.Chr(checksumValue), Strings.Chr(checksumFromString));
                    base.ClearData();
                    SendStringData(DeviceHelper.NAK.ToString());
                    _myTimer.Start();
                    Log.Trace("Started Timer after check sum false");
                    return;
                }

                Log.Trace("Checksum success");
                // Lấy ra message vừa nhận được
                var message = new Message(stringdata);

                // Gửi lại MT:
                SendByte(message.Mt);
                //Log.Trace("Send MT Byte to Advia:{0}", Strings.Chr(message.Mt));

                // Lấy MT Tiếp theo
                var nextMt = (byte) (message.Mt + 1);
                if (nextMt > MaxMtValue) nextMt = MinMtValue;

                // Kiểm tra message
                switch (message.Id.ToUpper())
                {
                    case "S":
                    {
                        _lastStringData = CreateSMessage(nextMt);
                        SendStringData(_lastStringData);
                        //Log.Trace("Send S Message in transfer pharse:{0}", _lastStringData);
                        base.ClearData();
                        _myTimer.Start();
                        Log.Trace("Started Timer after send S Message");
                    }
                        break;
                    case "R":
                    {
                        _lastStringData = CreateZMessage(nextMt);
                        SendStringData(_lastStringData);
                        Log.Trace("Send Z Message after Recv R message:{0}", _lastStringData);
                        //StringData = message.Data;
                        Log.Debug("Add result to result list");

                        _resultList.Add(message.Data);
                        if (!_adviaWorker.IsBusy)
                        {
                            Log.Debug("Call Advia worker");
                            _adviaWorker.RunWorkerAsync();
                        }

                        //ProcessResultData();
                        base.ClearData();
                        //_myTimer.Start();
                        //Log.Trace("Started Timer after process result");
                    }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while processing raw Data:\r\nError:{0}", ex);
                throw;
            }
            finally
            {
            }
        }

        public new void ClearData()
        {
            StringData = string.Empty;
        }

        /// <summary>
        ///     Hàm xử lý kết quả của Advia 2120 giành cho từng thiết bị
        /// </summary>
        public abstract void ProcessResultData();

        #endregion
    }
}