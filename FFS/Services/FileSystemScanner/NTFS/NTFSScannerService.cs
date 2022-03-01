using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FFS.Services.FileSystemScanner.NTFS;
using FFS.Services.FileSystemScanner.Scanners;
using FFS.Utils;
using Serilog;

namespace FFS.Services.FileSystemScanner
{
    public class NTFSScannerService : IFSScanner
    {
        private List<string> _supportedFileSystems = new List<string>
        {
            FileSystemDb.NTFS
        };

        public IEnumerable<string> GetSupportedFileSystems()
        {
            return _supportedFileSystems;
        }

        public bool IsSupported(DriveInfo drive)
        {
            if (null == drive)
                return false;
            return _supportedFileSystems.Contains(drive.DriveFormat);
        }

        public async Task<IList<INode>> Scan(IList<DriveInfo> drives, Func<INode, bool> filter = null)
        {
            // Spotted illegal drive
            if (null == drives)
            {
                throw new ArgumentNullException("drives");
            }

            if (drives.Count == 0)
                return new List<INode>();

            var scanner = new SequentialScanner(drives, filter);
            return await scanner.DoScan();
        }
    }
}
