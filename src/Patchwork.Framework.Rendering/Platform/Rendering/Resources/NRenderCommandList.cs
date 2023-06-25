using System.Drawing;

namespace Patchwork.Framework.Platform.Rendering.Resources
{

    public abstract class NRenderCommandList : NRenderResource, INRenderCommandList
    {
        protected NRenderCommandType m_type;
        protected INRenderCommandQueue m_queue;

        /// <inheritdoc />
        public NRenderCommandList(INRenderDevice device, NRenderCommandType type) : base(device)
        {
            m_type = type;
        }

        public NRenderCommandList(INRenderCommandQueue queue) : this(queue.Device, queue.Type)
        {
            m_queue = queue;
        }

        /// <inheritdoc />
        public INRenderCommandQueue Queue
        {
            get { return m_queue; }
        }

        public NRenderCommandType Type
        {
            get { return m_type; }
        }

        /// <inheritdoc />
        public void Begin()
        {
            lock(m_lock)
            {
                PlatformBegin();
            }
        }

        protected abstract void PlatformBegin();

        /// <inheritdoc />
        public void Execute(INRenderCommandQueue queue)
        {
            m_queue ??= queue;
            Execute();
        }

        public void Execute()
        {
            lock(m_lock)
            {
                PlatformExecute();
                m_queue?.Execute(this);
            }
        }

        protected abstract void PlatformExecute();

        /// <inheritdoc />
        public void End()
        {
            lock(m_lock)
            {
                PlatformEnd();
            }
        }

        protected abstract void PlatformEnd();

        /// <inheritdoc />
        public void Reset()
        {
            lock(m_lock)
            {
                PlatformReset();
            }
        }

        protected abstract void PlatformReset();

        /// <inheritdoc />
        public void Clear(Color color)
        {
            lock(m_lock)
            {
                PlatformClear(color);
            }
        }

        protected abstract void PlatformClear(Color color);
    }

    public abstract class NRenderCommandList<TNative> : NRenderResource<TNative>, INRenderCommandList
    {
        protected NRenderCommandType m_type;
        protected INRenderCommandQueue m_queue;

        /// <inheritdoc />
        public NRenderCommandList(INRenderDevice device, NRenderCommandType type) : base(device)
        {
            m_type = type;
        }

        public NRenderCommandList(INRenderCommandQueue queue) : this(queue.Device, queue.Type)
        {
            m_queue = queue;
        }

        protected NRenderCommandList(INRenderCommandQueue queue, TNative resource) : base(queue.Device, resource)
        {
            m_queue = queue;
        }

        /// <inheritdoc />
        public INRenderCommandQueue Queue
        {
            get { return m_queue; }
        }

        public NRenderCommandType Type
        {
            get { return m_type; }
        }

        /// <inheritdoc />
        public void Begin()
        {
            lock(m_lock)
            {
                PlatformBegin();
            }
        }

        protected abstract void PlatformBegin();

        /// <inheritdoc />
        public void Execute(INRenderCommandQueue queue)
        {
            m_queue ??= queue;
            Execute();
        }

        public void Execute()
        {
            lock(m_lock)
            {
                PlatformExecute();
                m_queue?.Execute(this);
            }
        }

        protected abstract void PlatformExecute();

        /// <inheritdoc />
        public void End()
        {
            lock(m_lock)
            {
                PlatformEnd();
            }
        }

        protected abstract void PlatformEnd();

        /// <inheritdoc />
        public void Reset()
        {
            lock(m_lock)
            {
                PlatformReset();
            }
        }

        protected abstract void PlatformReset();

        /// <inheritdoc />
        public void Clear(Color color)
        {
            lock(m_lock)
            {
                PlatformClear(color);
            }
        }

        protected abstract void PlatformClear(Color color);
    }
}