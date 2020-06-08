﻿using System.Runtime.InteropServices;

namespace Patchwork.Framework.Platform.Interop
{
    internal static class Properties
    {
#if !ANSI
        public const CharSet BuildCharSet = CharSet.Unicode;
#else
        public const CharSet BuildCharSet = CharSet.Ansi;
#endif
    }
}