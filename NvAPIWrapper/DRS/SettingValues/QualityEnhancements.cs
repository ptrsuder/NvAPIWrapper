using System;

namespace NvAPIWrapper.DRS.SettingValues
{
    public enum QualityEnhancements : UInt32
    {
        HighQuality = 0xFFFFFFF6,

        Quality = 0x0,

        Performance = 0xA,

        HighPerformance = 0x14,

        Default = 0x0
    }
}