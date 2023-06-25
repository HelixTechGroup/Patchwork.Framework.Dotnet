using System;

namespace Patchwork.Framework.Platform
{
    public interface ICloneable<TNType> : ICloneable
    {
        //event EventHandler<TNType> Cloned;

        new TNType Clone();
    }
}