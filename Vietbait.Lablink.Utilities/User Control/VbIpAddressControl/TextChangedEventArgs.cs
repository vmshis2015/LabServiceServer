using System;

namespace Vietbait.Lablink.Utilities
{
    internal class TextChangedEventArgs : EventArgs
    {
        public int FieldIndex { get; set; }

        public String Text { get; set; }
    }
}