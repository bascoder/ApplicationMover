using System;
using System.Diagnostics;
using System.IO;

namespace ApplicationMover
{
    /// <summary>
    /// Implementation of <see cref="IAppMover"/> making use of batch script
    /// </summary>
    class MkLinkAppMover : IAppMover
    {
        private const string ScriptPath = "junction.cmd";

        public void MoveApplication(AppMoverInfo appMoverInfo)
        {
            if (!appMoverInfo.IsValid())
            {
                throw new ArgumentException("Expect AppMoverInfo object to be valid");
            }

            var arguments = $"{appMoverInfo.SourceDirectory} {appMoverInfo.TargetDirectory}";

            var process = new Process();
            var startInfo = new ProcessStartInfo()
            {
                FileName = ScriptPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = arguments,
                WorkingDirectory = Directory.GetCurrentDirectory()
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
