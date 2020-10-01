using System;
using Shin.Framework;

namespace Patchwork.Framework
{
    public interface ICreate : IInitialize, IDispose
    {
        event EventHandler Created;
        event EventHandler Creating;
        bool IsCreated { get; }

        void Create();
    }
}