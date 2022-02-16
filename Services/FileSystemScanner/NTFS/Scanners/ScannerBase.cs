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
        protected DriveInfo[] _drives;
        protected NtfsReader[] _ntfsReaders;

        protected ScannerBase(DriveInfo[] drives)
        {
            _drives = drives;
            BuildNTFSReaders(drives);
        }

        protected void BuildNTFSReaders(DriveInfo[] drives)
        {
            _ntfsReaders = new NtfsReader[drives.Length];
            for (int i = 0; i < drives.Length; ++i)
            {
                DriveInfo drive = drives[i].GetDriveInfoWithDiskNameAsLetter();
                _ntfsReaders[i] = new NtfsReader(drive, RetrieveMode.All);
            }
        }

        public abstract Task<ScanResult[]> DoScan();
    }
}
