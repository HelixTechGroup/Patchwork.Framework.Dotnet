namespace Patchwork.Framework.Platform
{
    public partial class NApplication
    {
        #region Methods
        /// <inheritdoc />
        public abstract bool OpenConsole();

        public abstract void CloseConsole();
        #endregion
    }
}