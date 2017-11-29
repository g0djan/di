using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContiner.Interfaces
{
    interface IFileReader
    {
        IEnumerable<string> ReadFile(string filename);
    }
}
