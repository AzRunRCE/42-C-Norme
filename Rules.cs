using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appRegex
{
    interface IRules
    {
        void Verify(string filename, string[] content);
    }
}
