namespace Patchwork.Framework
{
    public static partial class Core
    {
        #region Properties
        public static IPlatformRenderManager Renderer
        {
            get { return IoCContainer.Resolve<IPlatformRenderManager>(); }
        } //m_container.ResolveAll<IPlatformManager>().Where(m => m.GetType().ContainsInterface<IWindowManager>()).First() as IWindowManager; } }
        #endregion
    }
}