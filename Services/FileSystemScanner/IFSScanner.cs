using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services.FileSystemScanner.NTFS
{
    public interface IFSScanner
    {
        IEnumerable<string> GetSupportedFileSystems();

        bool IsSupported(DriveInfo drive);

        Task<IList<INode>> Scan(DriveInfo drives, Func<INode, bool> filter = null);
    }
}
