using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shield.Framework;
using Shield.Framework.Collections;
using Shield.Framework.Extensibility;
using Shield.Framework.Extensions;

namespace Shield.Framework.Platform.Extensibility
{
    public sealed class FileModuleLoader : ModuleLoader
    {           
        private readonly IAssemblyResolver m_assemblyResolver;
        private readonly ConcurrentHashSet<Uri> m_downloadedUris;

        public FileModuleLoader(IAssemblyResolver assemblyResolver)
        {
            m_assemblyResolver = assemblyResolver;
            m_downloadedUris = new ConcurrentHashSet<Uri>();
        }

        public override bool CanLoadModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            return moduleInfo.Ref != null && moduleInfo.Ref.StartsWith(RefTypePrefix.File, StringComparison.Ordinal);
        }

        public override void LoadModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            try
            {
                var uri = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);

                // If this module has already been downloaded, I fire the completed event.
                if (m_downloadedUris.Contains(uri))
                    RaiseLoadModuleCompleted(moduleInfo, null);
                else
                {
                    var path = uri.LocalPath;

                    var fileSize = -1L;
                    if (File.Exists(path))
                    {
                        var fileInfo = new FileInfo(path);
                        fileSize = fileInfo.Length;
                    }

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    RaiseModuleDownloadProgressChanged(moduleInfo, 0, fileSize);

                    m_assemblyResolver.LoadAssemblyFrom(moduleInfo.Ref);

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    RaiseModuleDownloadProgressChanged(moduleInfo, fileSize, fileSize);

                    // I remember the downloaded URI.
                    m_downloadedUris.Add(uri);

                    RaiseLoadModuleCompleted(moduleInfo, null);
                }
            }
            catch (Exception ex)
            {
                RaiseLoadModuleCompleted(moduleInfo, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            m_assemblyResolver.Dispose();
            m_downloadedUris.Dispose();

            base.Dispose(disposing);
        }        
    }
}
