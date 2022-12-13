using System;
using System.Instant;
using System.Runtime.InteropServices;
using System.Uniques;

namespace UltimatR
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Dto : Identifiable, IDto
    {
    }
}   