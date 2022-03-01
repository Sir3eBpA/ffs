using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Models
{
    public class ScanResult
    {
        public IList<INode> Files { get; set; }
        public IList<DriveInfo> ScannedDrives { get; set; }
    }
}
