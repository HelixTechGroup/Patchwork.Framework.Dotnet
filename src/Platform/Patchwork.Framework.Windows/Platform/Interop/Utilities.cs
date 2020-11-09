using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static Patchwork.Framework.Platform.Interop.User32.Methods;
using static Patchwork.Framework.Platform.Interop.Kernel32.Methods;

namespace Patchwork.Framework.Platform.Interop
{
    public static class Utilities
    {
        public static bool CheckOperation(IntPtr result)
        {
            if (result != IntPtr.Zero)
                return true;

            CheckLastError();
            return true;
        }

        public static bool CheckOperation(int result)
        {
            if (result != 0)
                throw new Win32Exception(result);

            CheckLastError();
            return true;
        }

        public static bool CheckOperation(Func<bool> result)
        {
            if (!result())
                return false;

            CheckLastError();
            return true;
        }

        public static bool CheckOperation(bool result)
        {
            if (result)
                return true;

            CheckLastError();
            return true;
        }

        public static void CheckLastError()
        {
            var e = (int)GetLastError();
            if (e != 0)
                throw new Win32Exception(e);
        }
    }
}
