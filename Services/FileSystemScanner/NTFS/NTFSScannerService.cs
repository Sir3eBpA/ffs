using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<ScanResult[]> Scan(DriveInfo[] drives, ScanOption scanOption = ScanOption.Sequential)
        {
            // Spotted illegal drive
            if (drives.Any(x => x == null))
            {
                throw new ArgumentException("Contains invalid drive name");
            }

            if (drives.Length == 0)
            {
                return null;
            }

            var scanner = CreateScanner(drives, scanOption);
            return await scanner.DoScan();
        }

        private ScannerBase CreateScanner(DriveInfo[] drives, ScanOption scanOption)
        {
            switch (scanOption)
            {
                case ScanOption.Sequential: return new SequentialScanner(drives);
                default:
                {
                    Log.Logger.Information($"Cannot find scanner of type {scanOption} using {nameof(SequentialScanner)} instead");
                    return new SequentialScanner(drives);
                }
            }
        }
    }
}
