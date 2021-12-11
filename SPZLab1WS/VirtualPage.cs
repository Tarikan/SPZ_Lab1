﻿using System;

namespace SPZLab1WS;

public class VirtualPage
{
    public int PageAddress { get; set; }
    
    public bool P { get; set; } // presence bit
    
    public bool U { get; set; } // Usage bit
    
    public bool M { get; set; } // Modification bit
    
    public DateTime TimeOfLastUsage { get; set; }
    
    public int ProcessId { get; init; }
}