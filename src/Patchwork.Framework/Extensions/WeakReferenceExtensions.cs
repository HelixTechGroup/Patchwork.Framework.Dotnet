#region Usings
using System;
#endregion

namespace Patchwork.Framework.Extensions
{
    public static class WeakReferenceExtensions
    {
        #region Methods
        public static bool IsAlive<T>(this WeakReference<T> value)
            where T : class
        {
            return value.TryGetTarget(out _);
        }

        public static bool TargetEquals<T>(this WeakReference<T> value, T obj)
            where T : class
        {
            return value.TryGetTarget(out var t)
                && t.Equals(obj);
        }
        #endregion
    }
}