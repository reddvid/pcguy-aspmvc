using System.ComponentModel;

namespace PCGuy.Common.Enums;

public enum SubCategories
{
    Audio,
    Monitors,
    Mouse,
    Keyboard,
    Cables,
    Controllers,
    Webcam,
    Storage,
    Wired,
    Wireless,
    Headphones,
    Headsets,
    Memory,
    [Description("RAM")] Ram,
    [Description("DDR4")] Ddr4,
    [Description("DDR5")] Ddr5,
    [Description("SSD")] Ssd,
    [Description("Hard Disks")] Hdd,
    [Description("Mouse Pads")] MousePads,
}