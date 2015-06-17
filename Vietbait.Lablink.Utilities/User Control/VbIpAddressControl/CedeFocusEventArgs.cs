using System;

namespace Vietbait.Lablink.Utilities
{
    internal class CedeFocusEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }

        public Action Action { get; set; }

        public Direction Direction { get; set; }

        public Selection Selection { get; set; }
    }
}