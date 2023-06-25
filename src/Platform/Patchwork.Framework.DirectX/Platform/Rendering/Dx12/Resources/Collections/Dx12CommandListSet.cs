using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Patchwork.Framework.Platform.Rendering.Resources;
using Shin.Framework;
using Shin.Framework.Collections.Concurrent;
using Shin.Framework.Extensions;

using VD3D12 = Vortice.Direct3D12;


namespace Patchwork.Framework.Platform.Rendering.Dx12.Resources.Collections
{
    internal interface IDx12CommandListSet : IDictionary<int, IList<INRenderCommandList>>, INRenderCommandQueue { }

    internal class Dx12CommandListSet<T> : 
        ConcurrentDictionary<int, Dx12CommandListSet<T>.CommandListSetItem>, 
        IDispose 
        where T : VD3D12.ID3D12CommandList
    {
        protected readonly Dx12CommandQueue m_queue;
        protected ConcurrentList<Dx12CommandList<T>> m_added;
        protected bool m_isDisposed;
        protected bool m_hasBegun;
        protected static readonly object m_lock = new object();

        public Dx12CommandListSet(INRenderCommandQueue queue, IEnumerable<T> items) : this(queue)
        {
            m_queue = queue as Dx12CommandQueue;
            var nItems = new ConcurrentList<Dx12CommandList<T>>();
            foreach (var i in items)
            {
                nItems.Add(new Dx12CommandList<T>(queue, i));
            }

            
            if (TryAdd(Count, new CommandListSetItem(nItems)))
                m_added.AddRange(nItems);
        }

        public Dx12CommandListSet(INRenderCommandQueue queue, IEnumerable<Dx12CommandList<T>> items) : this(queue)
        {
            //var l = new ConcurrentList<Dx12CommandList<T>(items);
            //var p = new KeyValuePair<int, IList<Dx12CommandList<T>>>(0, l);
            var dx12CommandLists = items as Dx12CommandList<T>[] ?? items.ToArray();
            if (TryAdd(Count, new CommandListSetItem(dx12CommandLists)))
                m_added.AddRange(dx12CommandLists);
        }

        //public Dx12CommandListSet()
        //{

        //}

        internal Dx12CommandListSet(INRenderCommandQueue queue)
        {
            m_queue = queue as Dx12CommandQueue;
            m_added = new ConcurrentList<Dx12CommandList<T>>();
            //CreateCommandList();
        }

        internal IEnumerable<INRenderCommandList> CommandLists
        {
            get
            {
                var sorted = this.ToImmutableSortedDictionary();
                var result = new ConcurrentList<INRenderCommandList>();
                foreach (var l in sorted.Values)
                {
                    result.AddRange(l);
                }

                return result;
            }
        }

        internal VD3D12.ID3D12CommandList[] Compile()
        {
            lock(m_lock)
            {
                var sorted = this.ToImmutableSortedDictionary();
                var result = new ConcurrentList<VD3D12.ID3D12CommandList>();

                foreach (var value in sorted.Values)
                {
                    result.AddRange(value.Compile());
                    //result.Add(value.Select(c => c.Resource));
                }


                return result.ToArray();   
            }
        }

        internal INRenderCommandList CreateCommandList()
        {
            return CreateCommandList(m_queue);
        }

        internal INRenderCommandList CreateCommandList(INRenderCommandQueue queue)
        {
            lock(m_lock)
            {
                var list = new Dx12GraphicsCommandList(queue);
                list.Create();
                if (TryGetValue(Count, out var lists))
                    lists?.Add(list);
                else
                    TryAdd(Count, new CommandListSetItem(new[] {list as INRenderCommandList<T>}));

                m_added.Add(list);
                return list;
            }
        }

        public void Begin()
        {
            if (m_hasBegun)
                return;

            lock(m_lock)
            {
                //m_queue?.Begin();
                foreach (var l in CommandLists)
                    l.Begin();   
                
                m_hasBegun = true;
            }
        }

        public void Execute()
        {
            if (m_hasBegun)
                return;
            
            lock(m_lock)
                m_queue?.Execute(CommandLists);
        }

        public void End()
        {
            if (!m_hasBegun)
                return;

            lock(m_lock)
            {
                //m_queue?.End();
                foreach (var l in CommandLists)
                    l.End();
                
                m_hasBegun = false;
            }
        }

        public void Reset()
        {
            if (m_hasBegun)
                return;
            
            //m_queue.Reset();
            foreach (var l in Values)
                l.Reset();

            var keys = new ConcurrentList<int>();
            foreach (var i in this.AsEnumerable())
            {
                foreach (var l in m_added)
                {
                    if (i.Value.Contains(l))
                        this[i.Key].Remove(l);   
                    
                    if (this[i.Key].Count == 0)
                        keys.Add(i.Key);
                }
            }

            foreach (var k in keys)
            {
                TryRemove(k, out var i);
                i?.Dispose();
            }
            
            m_added.Clear();
            //var eV = Values.AsEnumerable()
            //Clear();
            //CreateCommandList(m_queue);
        }

        public bool TryAdd(int index, params INRenderCommandList[] item)
        {
            lock(m_lock)
            {
                var t = item as CommandListSetItem;
                var a = new CommandListSetItem(item);
                if (t is not null &&
                    !TryAdd(index, t))
                    return false;
                else if (!TryAdd(index, a))
                    return false;

                m_added.AddRange(item.OfType<Dx12CommandList<T>>());
                return true;   
            }
        }

        internal class CommandListSetItem : ConcurrentList<INRenderCommandList>, IDispose
        {
            private bool m_isDisposed;
            protected bool m_hasBegun;
            
            protected CommandListSetItem() { }

            public CommandListSetItem(params INRenderCommandList[] items) : base(items)
            {
            }

            public CommandListSetItem(IEnumerable<INRenderCommandList> items) : base(items)
            {

            }

            public void Begin()
            {
                if (m_hasBegun)
                    return;
                
                lock(m_lock)
                {
                    foreach (var l in this)
                        l.Begin();   
                    
                    m_hasBegun = true;
                }
            }

            public IEnumerable<VD3D12.ID3D12CommandList> Compile()
            {
                lock(m_lock)
                {
                    var result = new ConcurrentList<VD3D12.ID3D12CommandList>();
                    foreach (var value in this)
                    {
                        result.Add(value.Resource);
                    }

                    return result;   
                }
            }
            
            public void End()
            {
                if (!m_hasBegun)
                    return;
                
                lock(m_lock)
                {
                    foreach (var l in this)
                        l.End();  
                    
                    m_hasBegun = false;
                }
            }

            public void Reset()
            {
                if (m_hasBegun)
                    return;
                
                lock(m_lock)
                {
                    foreach (var l in this)
                        l.Reset();
                }
            }

            ~CommandListSetItem()
            {
                Dispose(false);
            }

            #region Methods
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void DisposeManagedResources()
            {
                foreach (var tmp in this)
                    tmp.Dispose();
                
                Clear();
            }

            protected virtual void DisposeUnmanagedResources() { }

            protected virtual void OnDisposed(object sender, EventArgs e) { }

            protected virtual void OnDisposing(object sender, EventArgs e) { }

            protected void Dispose(bool disposing)
            {
                if (m_isDisposed)
                    return;

                //lock(m_lock)
                //{
                Disposing.Raise(this, EventArgs.Empty);
                if (disposing)
                    DisposeManagedResources();

                DisposeUnmanagedResources();
                Disposed.Raise(this, EventArgs.Empty);

                Disposing.Dispose();
                Disposed.Dispose();
                m_isDisposed = true;
                //}
            }

            private void WireUpDisposeEvents()
            {
                Disposing += OnDisposing;
                Disposed += OnDisposed;
            }
            #endregion

            /// <inheritdoc />
            public bool IsDisposed
            {
                get { return m_isDisposed; }
            }

            /// <inheritdoc />
            public event EventHandler Disposed;

            /// <inheritdoc />
            public event EventHandler Disposing;
        }

        ~Dx12CommandListSet()
        {
            Dispose(false);
        }

        #region Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeManagedResources()
        {
            foreach (var tmp in Values)
                tmp.Dispose();
            
            Clear();
        }

        protected virtual void DisposeUnmanagedResources() { }

        protected virtual void OnDisposed(object sender, EventArgs e) { }

        protected virtual void OnDisposing(object sender, EventArgs e) { }

        protected void Dispose(bool disposing)
        {
            if (m_isDisposed)
                return;

            //lock(m_lock)
            //{
                Disposing.Raise(this, EventArgs.Empty);
                if (disposing)
                    DisposeManagedResources();

                DisposeUnmanagedResources();
                Disposed.Raise(this, EventArgs.Empty);

                Disposing.Dispose();
                Disposed.Dispose();
                m_isDisposed = true;
            //}
        }

        private void WireUpDisposeEvents()
        {
            Disposing += OnDisposing;
            Disposed += OnDisposed;
        }
        #endregion

        /// <inheritdoc />
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public event EventHandler Disposing;
    }
}
