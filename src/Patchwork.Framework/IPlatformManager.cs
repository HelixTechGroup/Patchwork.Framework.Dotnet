using System;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;

namespace Patchwork.Framework
{
    public delegate void ProcessMessageHandler(IPlatformMessage message);

    public interface IPlatformManager<TAssembly> : IPlatformManager where TAssembly : AssemblyPlatformAttribute { }

    public interface IPlatformManager : IInitialize, IDispose
    {
        event ProcessMessageHandler ProcessMessage;
        event Action Startup;
        event Action Shutdown;
        bool IsRunning { get; }
        PlatformMessagePump MessagePump { get; }

        void Pump(CancellationToken token);

        void Wait();

        void Run(CancellationToken token);

        void RunAsync(CancellationToken token);

        void Create();
    }
}