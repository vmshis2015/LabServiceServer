using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml.Serialization;
using Vietbait.Lablink.Notifier.Properties;

namespace Vietbait.Lablink.Notifier
{
    /// <summary>
    /// </summary>
    internal class ProcessIcon : IDisposable
    {
        /// <summary>
        ///     The NotifyIcon object.
        /// </summary>
        private readonly NotifyIcon _ni;

        private SerialPort _firstCom = new SerialPort();
        private SerialPort _secondCom = new SerialPort();

        private NotifierProperties _myProperties;
        public static string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     Nạp cấu hình của service
        /// </summary>
        /// <returns></returns>
        private NotifierProperties GetConfig()
        {
            try
            {
                var myProperties = new NotifierProperties();
                string filePath = string.Format("{0}{1}.xml", AppPath, myProperties.GetType().Name);
                var myFileStream = new FileStream(filePath, FileMode.Open);
                var mySerializer = new XmlSerializer(myProperties.GetType());
                myProperties = (NotifierProperties)mySerializer.Deserialize(myFileStream);
                myFileStream.Flush();
                myFileStream.Close();
                return myProperties;
            }
            catch (Exception ex)
            {
                return new NotifierProperties();
            }
        }

        

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessIcon" /> class.
        /// </summary>
        public ProcessIcon()
        {
            _myProperties = GetConfig();

            //var myWriter = new StreamWriter(string.Format("{0}{1}.xml", AppDomain.CurrentDomain.BaseDirectory, _myProperties.GetType().Name));
            //try
            //{
            //    var mySerializer = new XmlSerializer(_myProperties.GetType());
            //    mySerializer.Serialize(myWriter, _myProperties);
            //}
            //catch (Exception)
            //{
            //}
            //finally
            //{
            //    myWriter.Flush();
            //    myWriter.Close();
            //}
            // Instantiate the NotifyIcon object.
            _ni = new NotifyIcon();
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            _ni.Dispose();
        }

        /// <summary>
        ///     Displays the icon in the system tray.
        /// </summary>
        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.			
            _ni.MouseClick += ni_MouseClick;
            _ni.Icon = Resources.Disconnect;
            _ni.Text = @"LABLink Notifier - Disconnected";
            
            _ni.Visible = true;

            // Attach a context menu.
            _ni.ContextMenuStrip = new ContextMenus().Create();

            if (!InitSerialPort()) return;
            _ni.Icon = Resources.Connect;
            _ni.Text = @"LABLink Notifier - Connected";
        }

        private bool InitSerialPort()
        {
           // Set param for first com
            try
            {
                _firstCom.PortName = _myProperties.FirstPortName;
                _firstCom.BaudRate = _myProperties.FirstPortBaudrate;
                _firstCom.DataBits = _myProperties.FirstPortDataBits;
                _firstCom.DtrEnable = _myProperties.FirstPortDtr;
                _firstCom.Parity = _myProperties.FirstPortParity;
                _firstCom.RtsEnable = _myProperties.FirstPortRts;
                _firstCom.StopBits = _myProperties.FirstPortStopBits;
                _firstCom.Handshake = Handshake.None;

                _secondCom.PortName = _myProperties.SecondPortName;
                _secondCom.BaudRate = _myProperties.SecondPortBaudrate;
                _secondCom.DataBits = _myProperties.SecondPortDataBits;
                _secondCom.DtrEnable = _myProperties.SecondPortDtr;
                _secondCom.Parity = _myProperties.SecondPortParity;
                _secondCom.RtsEnable = _myProperties.SecondPortRts;
                _secondCom.StopBits = _myProperties.SecondPortStopBits;
                _secondCom.Handshake = Handshake.None;

                _firstCom.DataReceived += ComDataReceive;
                _secondCom.DataReceived += ComDataReceive;
                //_firstCom.Open();
                //_secondCom.Open();
            }
            catch (Exception ex)
            {
                return false;
            }

            Open1:
            try
            {
                _firstCom.Open();
            }
            catch (Exception)
            {
                goto Open1;
            }
            Open2:
            try
            {
                _secondCom.Open();
            }
            catch (Exception)
            {
                goto Open2;
            }
            return true;
        }

        private void ComDataReceive(object obj, SerialDataReceivedEventArgs e)
        {
            var sender = (SerialPort)obj;
            var anotherPort = sender.PortName.Equals(_firstCom.PortName) ? _secondCom : _firstCom;

            var bytesToRead = sender.BytesToRead;

            while (bytesToRead > 0)
            {
                var tempBuffer = new byte[bytesToRead];
                sender.Read(tempBuffer, 0, bytesToRead);
                anotherPort.Write(tempBuffer, 0, bytesToRead);
                bytesToRead = sender.BytesToRead;
            }
        }

        private SerialPort GetSerialPort(string comName)
        {
            var comPort = new SerialPort
            {
                ReadBufferSize = 131072,
                DataBits = 8,
                PortName = comName,
                BaudRate = 9600,
                StopBits = StopBits.One,
                Parity = Parity.None,
                Handshake = Handshake.None,
                RtsEnable = true,
                DtrEnable = true,
            };
            return comPort;
        }

        /// <summary>
        ///     Handles the MouseClick event of the ni control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void ni_MouseClick(object sender, MouseEventArgs e)
        {
            // Handle mouse button clicks.
            if (e.Button == MouseButtons.Left)
            {
                // Start Windows Explorer.
                Process.Start("explorer", null);
            }
        }
    }
}