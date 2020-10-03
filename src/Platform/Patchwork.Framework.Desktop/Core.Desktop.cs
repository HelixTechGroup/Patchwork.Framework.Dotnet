namespace Patchwork.Framework
{
    public static partial class Core
    {
        #region Methods
        public static bool CreateConsole()
        {
            return Application.OpenConsole();
        }

        public static void CloseConsole()
        {
            Application.CloseConsole();
        }
        #endregion
    }
}