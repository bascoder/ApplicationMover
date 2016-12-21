using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMover
{
    /// <summary>
    /// Straight forward implementation of <see cref="IAppMover"/>
    /// </summary>
    class GenericAppMover : IAppMover
    {
        private string _sourceDirectory;
        private string _destinationDirectory;

        public GenericAppMover()
        {
        }

        public void MoveApplication(AppMoverInfo appMoverInfo)
        {
            _sourceDirectory = appMoverInfo.SourceDirectory;
            _destinationDirectory = appMoverInfo.TargetDirectory;
            if (!appMoverInfo.IsValid())
            {
                throw new ArgumentException("Expect source and target directory to exist");
            }

            CopyDirectory(_sourceDirectory, _destinationDirectory);
            DeleteSources();
            JunctionHelper.CreateJunction(_sourceDirectory, _destinationDirectory);
        }

        private void DeleteSources()
        {
            Directory.Delete(path: _sourceDirectory, recursive: true);
        }

        /// <summary>
        /// Copy directory with subdirectories
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destinationDirectory"></param>
        private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDirectory);

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            CopyFiles(destinationDirectory, source);
            CopySubDirectories(destinationDirectory, source);
        }

        private static void CopySubDirectories(string destinationDirectory, DirectoryInfo source)
        {
            DirectoryInfo[] subDirectories = source.GetDirectories();
            foreach (var subDirectory in subDirectories)
            {
                string newPath = Path.Combine(destinationDirectory, subDirectory.Name);
                CopyDirectory(subDirectory.FullName, newPath);
            }
        }

        private static void CopyFiles(string destinationDirectory, DirectoryInfo source)
        {
            FileInfo[] srcFiles = source.GetFiles();
            foreach (var file in srcFiles)
            {
                var newPath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(newPath);
            }
        }
    }
}
