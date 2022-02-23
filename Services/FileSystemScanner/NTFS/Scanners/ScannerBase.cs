using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FFS.Utils;

namespace FFS.Services.FileSystemScanner.Scanners
{
    public abstract class ScannerBase
    {
        protected IList<DriveInfo> _drives;
        protected Func<INode, bool> _filter;

        protected ScannerBase(IList<DriveInfo> drives)
        {
            _drives = drives;
        }

        protected ScannerBase(IList<DriveInfo> drives, Func<INode, bool> filter)
        {
            _drives = drives;
            _filter = filter;
        }

        public abstract Task<IList<INode>> DoScan();
    }
}
