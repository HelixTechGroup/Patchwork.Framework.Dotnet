#region Usings
using System;
using System.Threading;
using Patchwork.Framework.Messaging;
using Patchwork.Framework.Platform;
using Shin.Framework.Messaging;
#endregion

namespace Patchwork.Framework
{
    public delegate void ProcessMessageHandler<TMessage>(TMessage message) where TMessage : IPlatformMessage;

    public interface IPlatformManager<TAssembly, TMessage> : IPlatformManager 
        where TAssembly : PlatformAttribute 
        where TMessage : IPlatformMessage
    {
        #region Events
        
        #endregion

        #region Properties
        static IPlatformManager<TAssembly, TMessage> Instance { get; }
        #endregion
    }

    public interface IPlatformManager : ICreate
    {
        #region Events
        event Action Shutdown;
        event ProcessMessageHandler ProcessMessage;
        event Action Startup;
        #endregion

        #region Properties
        bool IsRunning { get; }
        IPlatformMessagePump MessagePump { get; }

        MessageIds[] SupportedMessages { get; }
        #endregion

        #region Methods
        void Pump(CancellationToken token);

        void Wait();

        void Run(CancellationToken token);

        void RunAsync(CancellationToken token);

        void RunOnce(CancellationToken token);
        #endregion
    }
}