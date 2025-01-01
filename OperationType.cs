using System.ComponentModel;

namespace PS5CodeReader
{
    internal enum OperationType
    {
        [Description("Read Error Codes")]
        ReadErrorCodes,
        [Description("Read Error Codes (full log)")]
        ReadAllErrorCodes, 
        [Description("Clear Error Codes")]
        ClearErrorCodes,
        [Description("Interpret Error Code")]
        InterpretError,
        [Description("Monitor Mode")]
        MonitorMode,
        [Description("Run Command Lists")]
        RunCommandList,
        [Description("Run Raw Command")]
        RunRawCommand
    }
}
