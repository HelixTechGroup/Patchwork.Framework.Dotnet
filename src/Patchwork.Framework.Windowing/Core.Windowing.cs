namespace Patchwork.Framework
{
    public static partial class Core
    {
        #region Properties
        public static IPlatformWindowManager Window
        {
            get { return IoCContainer.Resolve<IPlatformWindowManager>(); }
        } //m_container.ResolveAll<IPlatformManager>().Where(m => m.GetType().ContainsInterface<IWindowManager>()).First() as IWindowManager; } }
        #endregion
    }
}