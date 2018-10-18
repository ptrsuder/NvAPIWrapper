﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NvAPIWrapper.Native.Attributes;
using NvAPIWrapper.Native.General.Structures;
using NvAPIWrapper.Native.Helpers;
using NvAPIWrapper.Native.Interfaces;
using NvAPIWrapper.Native.Interfaces.GPU;

namespace NvAPIWrapper.Native.GPU.Structures
{
    /// <summary>
    ///     Holds clock frequencies associated with a physical GPU and an specified clock type
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    [StructureVersion(3)]
    public struct ClockFrequenciesV3 : IInitializable, IClockFrequencies
    {
        internal const int MaxClocksPerGpu = 32;

        internal StructureVersion _Version;
        internal readonly uint _ClockTypeAndReserve;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxClocksPerGpu)] internal
            ClockDomainInfo[] _Clocks;

        /// <summary>
        ///     Creates a new ClockFrequenciesV3
        /// </summary>
        /// <param name="clockType">The type of the clock frequency being requested</param>
        public ClockFrequenciesV3(ClockType clockType = ClockType.CurrentClock)
        {
            this = typeof(ClockFrequenciesV3).Instantiate<ClockFrequenciesV3>();
            _ClockTypeAndReserve = 0u.SetBits(0, 2, (uint) clockType);
        }

        /// <inheritdoc />
        public Dictionary<PublicClock, ClockDomainInfo> Clocks => _Clocks
            .Select((value, index) => new { index, value })
            .Where(arg => Enum.IsDefined(typeof(PublicClock), arg.index))
            .ToDictionary(arg => (PublicClock)arg.index, arg => arg.value);

        /// <summary>
        ///     Gets the type of clock frequencies provided with this object
        /// </summary>
        public ClockType ClockType => (ClockType) _ClockTypeAndReserve.GetBits(0, 2);

        /// <inheritdoc />
        public ClockDomainInfo GraphicsClock => _Clocks[(int)PublicClock.Graphics];

        /// <inheritdoc />
        public ClockDomainInfo MemoryClock => _Clocks[(int)PublicClock.Memory];

        /// <inheritdoc />
        public ClockDomainInfo VideoDecodingClock => _Clocks[(int)PublicClock.Video];

        /// <inheritdoc />
        public ClockDomainInfo ProcessorClock => _Clocks[(int)PublicClock.Processor];

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{ClockType}] 3D Graphics = {GraphicsClock} - Memory = {MemoryClock} - Video Decoding = {VideoDecodingClock} - Processor = {ProcessorClock}";
        }
    }
}
