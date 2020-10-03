#region Usings
using Patchwork.Framework.Platform.Windowing;
using Shin.Framework;
#endregion

namespace Patchwork.Framework.Manager
{
    public sealed class WindowsWindowManager : PlatformWindowManager
    {
        #region Methods
        /// <inheritdoc />
        protected override void CreateManager(params AssemblyWindowingAttribute[] managers)
        {
            base.CreateManager(managers);
        }

        /// <inheritdoc />
        protected override INWindow PlatformCreateWindow(NWindowDefinition definition)
        {
            Throw.If(!Core.Application.IsInitialized).InvalidOperationException();

            var win = new WinWindow(Core.Application, definition);
            win.Create();
            return win;
        }
        #endregion
    }
}