using System;

namespace CSVData {
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreSerializeCSVAttribute: Attribute{}
}