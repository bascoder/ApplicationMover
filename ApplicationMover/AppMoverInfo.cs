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
            set
            {
                if (DoesDirectoryExists(value))
                {
                    _sourceDirectory = value;
                }
                else
                {
                    throw new UserException("Expect directory to exist");
                }
            }
        }

        /// <summary>
        /// Directory to transfer app to
        /// </summary>
        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                SetTargetDirectory(value);
            }
        }

        private void SetTargetDirectory(string value)
        {
            if (IsTargetDirectoryValid(value))
            {
                CreateDirectoryIfNotExists(value);
                _targetDirectory = value;
            }
            else
            {
                throw new UserException("Expect target directory to be empty");
            }
        }

        public bool IsValid()
        {
            return DoesDirectoryExists(_sourceDirectory)
                && DoesDirectoryExists(_targetDirectory)
                && IsTargetDirectoryValid(_targetDirectory);
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

        private static bool IsTargetDirectoryValid(string directory)
        {
            return !string.IsNullOrWhiteSpace(directory)
                && Directory.GetDirectories(directory).Length == 0
                && Directory.GetFiles(directory).Length == 0;
        }
    }
}
