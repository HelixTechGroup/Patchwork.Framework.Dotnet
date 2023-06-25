namespace Patchwork.Framework
{
    public static partial class Core
    {
        #region Properties
        public static IPlatformRenderingManager Renderer
        {
            get { return IoCContainer.Resolve<IPlatformRenderingManager>(); }
        } //m_container.ResolveAll<IPlatformManager>().Where(m => m.GetType().ContainsInterface<IWindowManager>()).First() as IWindowManager; } }
        #endregion
    }
}