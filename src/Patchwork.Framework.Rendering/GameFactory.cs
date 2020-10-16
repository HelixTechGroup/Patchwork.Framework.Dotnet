#region Usings
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using UniverseSol.Framework.Service;
using UniverseSol.Framework.System;
using UniverseSol.Framework.System.Threading.Coroutine;

#endregion

namespace UniverseSol.Framework.Factory
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>A game factory.</summary>
    ///
    /// <seealso cref="UniverseSol.Framework.Factory.IGameFactory"/>
    ///-------------------------------------------------------------------------------------------------
    public abstract class GameFactory<T> : IGameFactory<T> where T : IUniverseSolObject
    {
        #region Events
        public event EventHandler<UniverseSolPropertyChangeEventArgs<string>> TemplateFileNameChangedEventHandler;

        public event EventHandler<UniverseSolPropertyChangeEventArgs<string>> NameChangedEventHandler;

        public event Action<Task> InitializeCompleteCallback;

        public event Action<Task> PostInitializeCompleteCallback;

        public event Action<Task> UpdateCompleteCallback;

        public event EventHandler<UniverseSolPropertyChangeEventArgs<bool>> EnabledChangedEventHandler;

        public event Action<IUniverseSolObject, GameTime> OnUpdate;

        public event Action<IUniverseSolObject> OnDispose;

        public event Action<IUniverseSolObject> OnInitialize;

        public event Action<IUniverseSolObject> OnPostInitialize;

        public event Action<T> OnCreate;

        public event Action<T> OnDestroy;
        #endregion

        #region Members
        protected readonly ICoroutine m_coroutine;
        protected readonly object m_factoryLock;
        protected readonly Guid m_id;
        protected readonly ILoggingService m_loggingService;
        protected readonly IStorageService m_storageService;
        protected bool m_disposed;
        protected bool m_enabled;
        protected string m_fullXMLPath;
        protected bool m_initialized;
        protected string m_name;
        protected bool m_threaded;
        protected XDocument m_xmlDoc;
        protected string m_xmlFile;
        #endregion

        #region Properties
        public bool Disposed
        {
            get { return m_disposed; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets a value indicating whether this object is enabled.</summary>
        ///
        /// <value>true if enabled, false if not.</value>
        ///-------------------------------------------------------------------------------------------------
        public bool Enabled
        {
            get { return m_enabled; }
            set
            {
                OnEnabledChange(new UniverseSolPropertyChangeEventArgs<bool>(this, m_enabled, value));
                m_enabled = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the identifier.</summary>
        ///
        /// <value>The identifier.</value>
        ///-------------------------------------------------------------------------------------------------
        public Guid Id
        {
            get { return m_id; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets a value indicating whether the initialized.</summary>
        ///
        /// <value>true if initialized, false if not.</value>
        ///-------------------------------------------------------------------------------------------------
        public bool Initialized
        {
            get { return m_initialized; }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                OnNameChange(new UniverseSolPropertyChangeEventArgs<string>(this, m_name, value));
                m_name = value;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets or sets the XML file.</summary>
        ///
        /// <value>The XML file.</value>
        ///-------------------------------------------------------------------------------------------------
        public string TemplateFileName
        {
            get { return m_xmlFile; }
            set
            {
                OnTemplateFileNameChange(new UniverseSolPropertyChangeEventArgs<string>(this, m_xmlFile, value));
                m_xmlFile = value;
            }
        }

        public bool Threaded
        {
            get { return m_threaded; }
        }
        #endregion

        protected GameFactory(IStorageService storageService, ILoggingService loggingService, ICoroutine coroutine)
        {
            m_id = Guid.NewGuid();
            m_factoryLock = new object();
            m_storageService = storageService;
            m_loggingService = loggingService;
            m_coroutine = coroutine;

            TemplateFileNameChangedEventHandler += (sender, args) => { LoadTemplate(args.NewValue); };
        }

        ~GameFactory()
        {
            Dispose(false);
        }

        #region Methods
        public static bool operator ==(GameFactory<T> left, GameFactory<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GameFactory<T> left, GameFactory<T> right)
        {
            return !Equals(left, right);
        }

        public abstract T Create(string templateName);

        public abstract void Destroy(T instance);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Updates the given gameTime.</summary>
        ///
        /// <param name="gameTime">The game time.</param>
        ///-------------------------------------------------------------------------------------------------
        public virtual void Update(GameTime gameTime)
        {
            if (!m_enabled || !m_initialized)
                return;

            if (OnUpdate != null)
                OnUpdate(this, gameTime);
        }

        /// <summary>Initializes this object.</summary>
        public virtual void Initialize()
        {
            if (m_initialized)
                return;

            LoadTemplate(m_xmlFile);

            if (OnInitialize != null)
                OnInitialize(this);
            m_initialized = m_enabled = true;
        }

        /// <summary>Posts the initialize.</summary>
        public virtual void PostInitialize()
        {
            if (!m_initialized)
                return;

            if (OnPostInitialize != null)
                OnPostInitialize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                    NameChangedEventHandler.Dispose();

                if (OnDispose != null)
                    OnDispose(this);
                m_disposed = true;
            }
        }

        protected virtual void LoadTemplate(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (!m_storageService.FileExists(fileName))
                throw new FileNotFoundException();

            lock(m_factoryLock)
            {
                var stream = m_storageService.OpenFile(fileName);
                m_xmlDoc = XDocument.Load(stream);
                m_storageService.CloseFile(stream);
            }
        }

        public Task UpdateAsync(GameTime gameTime)
        {
            if (!m_threaded)
            {
                m_loggingService.LogWarn("Please enable threading on service: " + m_name + " to use threaded functions.");
                return
                    Task.FromResult(false)
                        .FromException(
                            new NotSupportedException("Please enable threading on factory: " + m_name + " to use threaded functions."));
            }

            var t = new Task(() => Update(gameTime));
            if (UpdateCompleteCallback != null)
                t.ContinueWith(UpdateCompleteCallback);

            return t;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = m_id.GetHashCode();
                hashCode = (hashCode * 397) ^ m_factoryLock.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(GameFactory<T> other)
        {
            return other != null;
        }

        public bool Equals(IGameFactory other)
        {
            return other != null;
        }

        public override bool Equals(object obj)
        {
            var svc = obj as GameFactory<T>;
            return svc != null && Equals(svc);
        }

        public Task InitializeAsync()
        {
            if (!m_threaded)
            {
                m_loggingService.LogWarn("Please enable threading on service: " + m_name + " to use threaded functions.");
                return
                    Task.FromResult(false)
                        .FromException(
                            new NotSupportedException("Please enable threading on factory: " + m_name + " to use threaded functions."));
            }

            var t = new Task(Initialize);
            if (InitializeCompleteCallback != null)
                t.ContinueWith(InitializeCompleteCallback);

            return t;
        }

        public Task PostInitializeAsync()
        {
            if (!m_threaded)
            {
                m_loggingService.LogWarn("Please enable threading on factory: " + m_name + " to use threaded functions.");
                return
                    Task.FromResult(false)
                        .FromException(
                            new NotSupportedException("Please enable threading on service: " + m_name + " to use threaded functions."));
            }

            var t = new Task(PostInitialize);
            if (PostInitializeCompleteCallback != null)
                t.ContinueWith(PostInitializeCompleteCallback);

            return t;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnNameChange(UniverseSolPropertyChangeEventArgs<string> e)
        {
            NameChangedEventHandler.Raise(this, e);
        }

        private void OnEnabledChange(UniverseSolPropertyChangeEventArgs<bool> e)
        {
            EnabledChangedEventHandler.Raise(this, e);
        }

        private void OnTemplateFileNameChange(UniverseSolPropertyChangeEventArgs<string> e)
        {
            TemplateFileNameChangedEventHandler.Raise(this, e);
        }
        #endregion
    }
}