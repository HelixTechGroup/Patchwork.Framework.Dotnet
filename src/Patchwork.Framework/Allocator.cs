#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Patchwork.Framework.Extensions;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;
#endregion

namespace Patchwork.Framework
{
    //public abstract class Allocator : Creatable, IAllocate
    //{
    //    #region Events
    //    /// <inheritdoc />
    //    public event EventHandler Allocated;

    //    /// <inheritdoc />
    //    public event EventHandler Allocating;

    //    /// <inheritdoc />
    //    public event EventHandler<object> Deallocated;

    //    /// <inheritdoc />
    //    public event EventHandler<object> Deallocating;
    //    #endregion

    //    #region Members
    //    protected static readonly object m_lock = new object();
    //    protected int m_allocatedCount;
    //    protected int m_capacity;
    //    protected int m_defaultExpansionAmount = 1;
    //    protected bool m_isExpandable;
    //    protected bool m_isLocked;
    //    protected LinkedList<WeakReference> m_stack;
    //    #endregion

    //    #region Properties
    //    /// <inheritdoc />
    //    public int Capacity
    //    {
    //        get { return m_capacity; }
    //    }

    //    /// <inheritdoc />
    //    public int FreeCount
    //    {
    //        get { return m_stack.Count - m_allocatedCount; }
    //    }

    //    /// <inheritdoc />
    //    public bool IsExpandable
    //    {
    //        get { return m_isExpandable; }
    //    }

    //    public bool IsLocked
    //    {
    //        get { return m_isLocked; }
    //    }

    //    /// <inheritdoc />
    //    public int UsedCount
    //    {
    //        get { return m_allocatedCount; }
    //    }
    //    #endregion

    //    #region Methods
    //    /// <inheritdoc />
    //    public object Allocate()
    //    {
    //        lock(m_lock)
    //        {
    //            if (m_isLocked)
    //                return null;

    //            CheckAllocation(1);
    //            var i = m_stack.FirstOrDefault(r => !r.IsAlive);
    //            if (i is null)
    //            {
    //                i = new WeakReference(AllocateResources());
    //                m_stack.AddLast(i);
    //            }
    //            else
    //                i.Target = AllocateResources();

    //            Allocating.Raise(this, EventArgs.Empty);
    //            m_allocatedCount++;
    //            Allocated.Raise(this, i?.Target);

    //            return i.Target;
    //        }
    //    }

    //    /// <inheritdoc />
    //    public object[] Allocate(int count)
    //    {
    //        var res = new ConcurrentList<object>(count);
    //        for (var i = 0; i < count; i++)
    //            res.Add(Allocate());

    //        return res.ToArray();
    //    }

    //    /// <inheritdoc />
    //    public void Deallocate(params object[] obj)
    //    {
    //        lock(m_lock)
    //        {
    //            if (m_isLocked)
    //                return;


    //            foreach (var o in obj)
    //            {
    //                Deallocating.Raise(this, o);
    //                DeallocateResources(o);
    //                var oRef = m_stack.Where(r => r.Target.Equals(o));
    //                foreach (var r in oRef)
    //                {
    //                    var g = GC.GetGeneration(r);
    //                    m_stack.Remove(r);
    //                }

    //                --m_allocatedCount;
    //                Deallocated.Raise(this, o);
    //            }
    //        }
    //    }

    //    /// <inheritdoc />
    //    public void DeallocateAll()
    //    {
    //        lock(m_lock)
    //        {
    //            if (m_isLocked)
    //                return;

    //            foreach (var i in m_stack)
    //                Deallocate(i);

    //            m_stack.Clear();
    //            m_allocatedCount = 0;
    //        }
    //    }

    //    /// <inheritdoc />
    //    public void Lock()
    //    {
    //        lock(m_lock)
    //        {
    //            m_isLocked = true;
    //        }
    //    }

    //    /// <inheritdoc />
    //    public void Unlock()
    //    {
    //        lock(m_lock)
    //        {
    //            m_isLocked = false;
    //        }
    //    }

    //    /// <inheritdoc />
    //    public bool Contains(object obj)
    //    {
    //        lock(m_lock)
    //        {
    //            return m_stack.Contains(obj);
    //        }
    //    }

    //    /// <inheritdoc />
    //    public bool Expand(int count)
    //    {
    //        lock(m_lock)
    //        {
    //            if (!m_isExpandable)
    //                return false;

    //            ExpandResources(count);
    //            Interlocked.Add(ref m_capacity, count);
    //            return true;
    //        }
    //    }

    //    protected void CheckAllocation(int count)
    //    {
    //        if (m_allocatedCount == m_capacity)
    //            Throw.If(!Expand(m_defaultExpansionAmount)).InvalidOperationException();

    //        if (m_stack.Count == 0)
    //            m_stack.AddFirst(new WeakReference(AllocateResources()));
    //    }

    //    protected abstract void ExpandResources(int count);

    //    protected abstract object AllocateResources();

    //    protected abstract void DeallocateResources(object obj);
    //    #endregion
    //}

    public abstract class Allocator<T> : Creatable, IAllocate<T> where T : class
    {
        #region Events
        /// <inheritdoc />
        public event EventHandler<T> Allocated;

        /// <inheritdoc />
        public event EventHandler<T> Allocating;

        /// <inheritdoc />
        public event EventHandler<T> Deallocated;

        /// <inheritdoc />
        public event EventHandler<T> Deallocating;
        #endregion

        #region Members
        //protected static readonly object m_lock = new object();
        protected int m_allocatedCount;
        protected int m_capacity;
        protected int m_defaultExpansionAmount = 1;

        protected bool m_isExpandable;

        //protected LinkedList<WeakReference> m_stack;
        protected bool m_isLocked;
        protected LinkedList<WeakReference<T>> m_stack;
        #endregion

        #region Properties
        /// <inheritdoc />
        public int Capacity
        {
            get { return m_capacity; }
        }

        /// <inheritdoc />
        public int FreeCount
        {
            get { return m_stack.Count - m_allocatedCount; }
        }

        /// <inheritdoc />
        public bool IsExpandable
        {
            get { return m_isExpandable; }
        }

        public bool IsLocked
        {
            get { return m_isLocked; }
        }

        /// <inheritdoc />
        public int UsedCount
        {
            get { return m_allocatedCount; }
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public T Allocate()
        {
            lock(m_lock)
            {
                if (m_isLocked)
                    return null;

                //CheckAllocation(1);
                var i = m_stack
                   .FirstOrDefault(wr => !wr.IsAlive());

                var result = AllocateResources();
                Allocating.Raise(this, result);
                if (i is null)
                {
                    i = new WeakReference<T>(result);
                    m_stack.AddLast(i);
                }
                else
                    i.SetTarget(result);

                Throw.If(!i.TryGetTarget(out var r)).InvalidOperationException();
                Interlocked.Increment(ref m_allocatedCount);
                Allocated.Raise(this, result);

                return r;
            }
        }

        /// <inheritdoc />
        public T[] Allocate(int count)
        {
            CheckAllocation(count);
            var res = new ConcurrentList<T>(count);
            for (var i = 0; i < count; i++)
                res.Add(Allocate());

            return res.ToArray();
        }

        /// <inheritdoc />
        public void Deallocate(params T[] obj)
        {
            lock(m_lock)
            {
                if (m_isLocked)
                    return;


                foreach (var o in obj)
                {
                    if (!Contains(o))
                        continue;

                    var remove = m_stack
                       .First(r => r
                                 .TargetEquals(o));
                    Deallocating.Raise(this, o);
                    DeallocateResources(o);
                    --m_allocatedCount;
                    Deallocated.Raise(this, o);
                    remove.SetTarget(null);
                }
            }
        }

        /// <inheritdoc />
        public void DeallocateAll()
        {
            lock(m_lock)
            {
                Throw.If(m_isLocked).InvalidOperationException();

                foreach (var r in m_stack)
                {
                    r.TryGetTarget(out var o);
                    Deallocating.Raise(this, o);
                    DeallocateResources(o);
                    --m_allocatedCount;
                    Deallocated.Raise(this, o);
                    r.SetTarget(null);
                }

                m_stack.Clear();
                m_allocatedCount = 0;
            }
        }

        /// <inheritdoc />
        public void Lock()
        {
            lock(m_lock)
            {
                m_isLocked = true;
            }
        }

        /// <inheritdoc />
        public void Unlock()
        {
            lock(m_lock)
            {
                m_isLocked = false;
            }
        }

        /// <inheritdoc />
        public bool Contains(T obj)
        {
            lock(m_lock)
            {
                return m_stack
                          .Count(r => r
                                             .TargetEquals(obj)) > 0;
            }
        }

        /// <inheritdoc />
        public bool Expand(int count)
        {
            lock(m_lock)
            {
                if (!m_isExpandable)
                    return false;

                ExpandResources(count);
                Interlocked.Add(ref m_capacity, count);
                return true;
            }
        }

        protected abstract T AllocateResources();

        protected abstract void DeallocateResources(T obj);

        protected abstract void ExpandResources(int count);

        protected virtual void CheckAllocation(int count)
        {
            if (m_allocatedCount == m_capacity)
                Throw.If(!Expand(m_defaultExpansionAmount)).InvalidOperationException();

            if (m_stack.Count == 0)
                m_stack.AddFirst(new WeakReference<T>(AllocateResources()));
        }

        /// <inheritdoc />
        protected override void InitializeResources()
        {
            base.InitializeResources();

            m_stack = new LinkedList<WeakReference<T>>();
        }
        #endregion
    }
}