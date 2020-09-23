using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;
using Shin.Framework.Extensions;

namespace Patchwork.Framework.Platform
{
    public sealed class WindowsWindowManager : PlatformWindowManager
    {
        /// <inheritdoc />
        protected override INWindow PlatformCreateWindow(NWindowDefinition definition)
        {
            Throw.If(!Core.Application.IsInitialized).InvalidOperationException();

            var win = new WinWindow(Core.Application, definition);
            win.Create();
            return win;
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            base.CreateManager(managers);
        }
    }
}
