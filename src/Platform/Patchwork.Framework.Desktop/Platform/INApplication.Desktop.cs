namespace Patchwork.Framework.Platform
{
    public partial interface INApplication
    {
        #region Methods
        bool OpenConsole();

        void CloseConsole();
        #endregion
    }
}