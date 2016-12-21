using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMover
{
    static class JunctionHelper
    {
        public static void CreateJunction(string source, string target)
        {
            //TODO use Win32 to create junction
            // see: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365006(v=vs.85).aspx
            // and http://www.flexhex.com/docs/articles/hard-links.phtml
            throw new NotImplementedException("Cannot create a Junction from C# code yet");
        }
    }
}
