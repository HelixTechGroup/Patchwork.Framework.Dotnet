using System;

namespace Patchwork.Framework.Environment {
    public interface IRuntimeInformation {
        RuntimeType Runtime { get; }
        Version RuntimeVersion { get; }

        void DetectRuntime();
    }
}