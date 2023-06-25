using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Patchwork.Framework.Threading.Runtime
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=true)]
    internal class RunsOnMainThreadAttribute : Attribute
    {
    }
}
