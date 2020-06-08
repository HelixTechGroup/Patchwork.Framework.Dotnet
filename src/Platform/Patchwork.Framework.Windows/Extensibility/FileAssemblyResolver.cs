using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Shield.Framework.Extensibility;

namespace Shield.Framework.Platform.Extensibility
{
    public sealed class FileAssemblyResolver : IAssemblyResolver
    {
        public event Action<IDispose> OnDispose;

        private bool m_disposed;
        private readonly ConcurrentList<AssemblyInfo> registeredAssemblies = new ConcurrentList<AssemblyInfo>();
        private bool handlesAssemblyResolve;

        public bool Disposed
        {
            get { return m_disposed; }
        }

        ~FileAssemblyResolver()
        {
            Dispose(false);
        }

        public void LoadAssemblyFrom(string assemblyPath)
        {
            if (!handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
                handlesAssemblyResolve = true;
            }

            var assemblyUri = GetFileUri(assemblyPath);

            if (assemblyUri == null)
                throw new ArgumentException(
                    "The argument must be a valid absolute Uri to an assembly file.", 
                    nameof(assemblyPath));

            if (!File.Exists(assemblyUri.LocalPath))
                throw new FileNotFoundException();

            var assemblyName = AssemblyName.GetAssemblyName(assemblyUri.LocalPath);
            var assemblyInfo = this.registeredAssemblies.FirstOrDefault(a => assemblyName == a.AssemblyName);

            if (assemblyInfo != null)
            {
                return;
            }

            assemblyInfo = new AssemblyInfo()
            {
                AssemblyName = assemblyName,
                AssemblyUri = assemblyUri
            };

            registeredAssemblies.Add(assemblyInfo);
        }

        private Uri GetFileUri(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            Uri uri;
            if (!Uri.TryCreate(filePath, UriKind.Absolute, out uri))
                return null;

            return !uri.IsFile ? null : uri;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var assemblyInfo = registeredAssemblies
                .FirstOrDefault(
                    a => 
                    AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));

            if (assemblyInfo == null)
                return null;

            return assemblyInfo.Assembly ?? (assemblyInfo.Assembly = Assembly.LoadFrom(assemblyInfo.AssemblyUri.LocalPath));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            if (handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                handlesAssemblyResolve = false;
            }

            OnDispose?.Invoke(this);
            m_disposed = true;
        }
    }
}
