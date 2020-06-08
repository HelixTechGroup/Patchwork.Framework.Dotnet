#region Usings
using Shield.Framework.Platform.IO.Managers;
#endregion

namespace Shield.Framework.Platform.IO
{
    public class WindowsStorageProvider : PlatformStorageProvider
    {
        public WindowsStorageProvider(ILocalApplicationStorageManager localStorage, IPrivateApplicationStorageManager privateStorage) :
            base(localStorage, privateStorage) { }
    }
}