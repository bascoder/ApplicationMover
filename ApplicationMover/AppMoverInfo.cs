using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMover
{
    class AppMoverInfo
    {
        private string _sourceDirectory;
        private string _targetDirectory;

        /// <summary>
        /// Source directory
        /// </summary>
        public string SourceDirectory
        {
            get { return _sourceDirectory; }
            set { SetSourceDirectory(value); }
        }

        /// <summary>
        /// Directory to transfer app to
        /// </summary>
        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set { SetTargetDirectory(value); }
        }

        private void SetSourceDirectory(string value)
        {
            if (value?.Equals(TargetDirectory) == true)
            {
                throw new UserException("Expect source directory to be different from destination directory");
            }
            if (!DoesDirectoryExists(value))
            {
                throw new UserException("Expect directory to exist");
            }
            _sourceDirectory = value;
        }

        private void SetTargetDirectory(string value)
        {
            try
            {
                CreateDirectoryIfNotExists(value);
            }
            catch (Exception e)
            {
                throw new UserException("Could not create destination directory at " + value, e);
            }
            if (value?.Equals(SourceDirectory) == true)
            {
                throw new UserException("Expect destination directory to be different than source directory");
            }
            if (!IsTargetDirectoryValid(value))
            {
                throw new UserException("Expect destination directory to be empty");
            }
            _targetDirectory = value;
        }

        public bool IsValid()
        {
            return DoesDirectoryExists(_sourceDirectory)
                && DoesDirectoryExists(_targetDirectory)
                && IsTargetDirectoryValid(_targetDirectory);
        }

        private static bool IsTargetDirectoryValid(string directory)
        {
            return !string.IsNullOrWhiteSpace(directory)
                   && Directory.GetDirectories(directory).Length == 0
                   && Directory.GetFiles(directory).Length == 0;
        }

        private static void CreateDirectoryIfNotExists(string value)
        {
            if (!DoesDirectoryExists(value))
            {
                Directory.CreateDirectory(value);
            }
        }

        private static bool DoesDirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }
    }
}
