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
        protected DriveInfo _drive;
        protected Func<INode, bool> _filter;

        protected ScannerBase(DriveInfo drive)
        {
            _drive = drive;
        }

        protected ScannerBase(DriveInfo drive, Func<INode, bool> filter)
        {
            _drive = drive;
            _filter = filter;
        }

        public abstract Task<IList<INode>> DoScan();
    }
}
