using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Vietbait.Lablink.Notifier
{
    public class NotifierProperties
    {
        public class OrderedDisplayNameAttribute : DisplayNameAttribute
        {
            public OrderedDisplayNameAttribute(int position, int total, string displayName)
            {
                var sb = new StringBuilder(displayName);

                for (int index = position; index < total; index++)
                    sb.Insert(0, '\t');
                base.DisplayNameValue = sb.ToString();
            }
        }

        private const string StrfirstPort = "First Port";
        private const string StrsecondPort = "Second Port";

        public NotifierProperties()
        {
            FirstPortName = "";
            FirstPortBaudrate = 9600;
            FirstPortDataBits = 8;
            FirstPortDtr = true;
            FirstPortParity = Parity.None;
            FirstPortRts = true;
            FirstPortStopBits = StopBits.One;

            SecondPortName = "";
            SecondPortBaudrate = 9600;
            SecondPortDataBits = 8;
            SecondPortDtr = true;
            SecondPortParity = Parity.None;
            SecondPortRts = true;
            SecondPortStopBits = StopBits.One;
        }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("Port Name"),
         OrderedDisplayName(1,7,"Port Name")]
        public string FirstPortName { get; set; }
        
        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("Baudrate"),
         OrderedDisplayName(2, 7, "Baudrate")]
        public int FirstPortBaudrate { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("Stopbits"),
         OrderedDisplayName(3, 7, "Stopbits")]
        public StopBits FirstPortStopBits { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("Data bits"),
         OrderedDisplayName(4, 7, "Data bits")]
        public int FirstPortDataBits { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("Parity"),
         OrderedDisplayName(5, 7, "Parity")]
        public Parity FirstPortParity { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("RTS"),
         OrderedDisplayName(6, 7, "RTS")]
        public bool FirstPortRts { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrfirstPort),
         Description("DTR"),
         OrderedDisplayName(7, 7, "DTR")]
        public bool FirstPortDtr { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("Port Name"),
         OrderedDisplayName(1, 7, "Port Name")]
        public string SecondPortName { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("Baudrate"),
         OrderedDisplayName(2, 7, "Baudrate")]
        public int SecondPortBaudrate { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("Stopbits"),
         OrderedDisplayName(3, 7, "Stopbits")]
        public StopBits SecondPortStopBits { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("Data bits"),
         OrderedDisplayName(4, 7, "Data bits")]
        public int SecondPortDataBits { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("Parity"),
         OrderedDisplayName(5, 7, "Parity")]
        public Parity SecondPortParity { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("RTS"),
         OrderedDisplayName(6, 7, "RTS")]
        public bool SecondPortRts { get; set; }

        [Browsable(true), ReadOnly(false), Category(StrsecondPort),
         Description("DTR"),
         OrderedDisplayName(7, 7, "DTR")]
        public bool SecondPortDtr { get; set; }

    }
}
