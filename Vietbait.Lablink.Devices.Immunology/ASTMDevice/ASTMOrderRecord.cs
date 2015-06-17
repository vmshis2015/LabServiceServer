using System;

namespace Vietbait.Lablink.Devices.Immunology 
{
    public abstract class ASTMOrderRecord : ASTMDeviceRecord
    {

        public abstract FieldOfRecord SegNumber{get;set;}
        public abstract FieldOfRecord SpecId { get; set; }
        public abstract FieldOfRecord TestIdString { get; set; }
        public abstract string[] TestId { get; set; }
        
        

    }
}