using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Services.Exporters;

namespace FFS.Services.Reporting
{
    public interface IExporter
    {
        void Export(IList<INode> nodes, TextWriter serializer);
    }
}
