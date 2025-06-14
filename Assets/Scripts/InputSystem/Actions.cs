using System;

namespace InputSystem {
    
    [Flags]
    public enum Actions {
        None        = 0x000000,
        Front       = 0x000001,
        Back        = 0x000010,
        Vertical    = 0x000011,
        Left        = 0x000100,
        Right       = 0x001000,
        Horizontal  = 0x001100,
        Charge      = 0x010000,
        UseItem     = 0x100000,
    }
}