#region Usings
using System;
#endregion

namespace Patchwork.Framework
{
    public interface IAllocate : ICreate
    {
        #region Properties
        int UsedCount { get; }
        int Capacity { get; }
        bool IsExpandable { get; }
        bool IsLocked { get; }
        int FreeCount { get; }
        #endregion

        #region Methods
        void DeallocateAll();

        void Lock();

        void Unlock();

        bool Expand(int count);
        #endregion
    }

    public interface IAllocate<T> : IAllocate
    {
        #region Events
        event EventHandler<T> Allocated;
        event EventHandler<T> Allocating;
        event EventHandler<T> Deallocated;
        event EventHandler<T> Deallocating;
        #endregion

        #region Methods
        new T Allocate();

        new T[] Allocate(int count);

        new void Deallocate(params T[] obj);

        new bool Contains(T obj);
        #endregion
    }
}