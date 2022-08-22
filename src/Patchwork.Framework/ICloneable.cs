using System;

namespace Patchwork.Framework.Platform
{
    public interface ICloneable<TNType> : ICloneable
    {
        new TNType Clone();
    }
}