using System;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework;

namespace Patchwork.Framework
{
    public delegate void ProcessMessageHandler<TMessage>(TMessage message) where TMessage : IPlatformMessage;

    public interface IPlatformManager<TAssembly, TMessage> : IPlatformManager where TAssembly : PlatformAttribute where TMessage : IPlatformMessage
    {
        event ProcessMessageHandler<TMessage> ProcessMessage;

        static IPlatformManager<TAssembly, TMessage> Instance { get; }
    }

    public interface IPlatformManager : IInitialize, IDispose
    {
        event Action Startup;
        event Action Shutdown;
        bool IsRunning { get; }
        IPlatformMessagePump MessagePump { get; }

        void Pump(CancellationToken token);

        void Wait();

        void Run(CancellationToken token);

        void RunAsync(CancellationToken token);

        void Create();
    }
}