using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services.FileSystemScanner.NTFS
{
    interface IFSScanner
    {
        IEnumerable<string> GetSupportedFileSystems();

        bool IsSupported(DriveInfo drive);

        Task<ScanResult[]> Scan(DriveInfo[] drives, ScanOption scanOption = ScanOption.Sequential);
    }
}
