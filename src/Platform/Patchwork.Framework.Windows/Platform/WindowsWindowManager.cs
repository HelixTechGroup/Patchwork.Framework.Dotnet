using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Patchwork.Framework.Platform.Window;
using Shin.Framework;

namespace Patchwork.Framework.Platform
{
    public sealed class WindowsWindowManager : WindowManager
    {
        /// <inheritdoc />
        protected override INativeWindow PlatformCreateWindow(NativeWindowDefinition definition)
        {
            Throw.If(!Core.Application.IsInitialized).InvalidOperationException();

            var win = new WindowsWindow(Core.Application, definition);
            win.Create();
            return win;
        }

        /// <inheritdoc />
        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            base.CreateManager(managers);
            //Core.AddManager<IWindowManager>(this);
            //Core.AddManager<RenderManager>();
        }
    }
}
