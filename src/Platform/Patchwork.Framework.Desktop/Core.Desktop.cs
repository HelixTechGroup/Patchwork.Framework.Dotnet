using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Patchwork.Framework
{
    public static partial class Core
    {
        public static bool CreateConsole()
        {
            return Application.OpenConsole();
        }

        public static void CloseConsole()
        {
            Application.CloseConsole();
        }
    }
}
