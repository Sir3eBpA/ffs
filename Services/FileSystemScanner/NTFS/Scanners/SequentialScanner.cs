using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services.FileSystemScanner.Scanners
{
    public class SequentialScanner : ScannerBase
    {
        public SequentialScanner(DriveInfo[] drives) : base(drives) {}

        public override async Task<ScanResult[]> DoScan()
        {
            return await Task.Run(() =>
            {
                ScanResult[] results = new ScanResult[_ntfsReaders.Length];

                for (int i = 0; i < _ntfsReaders.Length; ++i)
                {
                    ScanResult scanRes = new ScanResult(_drives[i]);

                    IEnumerable<INode> newNodes = _ntfsReaders[i].GetNodes(_drives[i].Name);
                    scanRes.AddFiles(newNodes);
                }

                return results;
            });
        }
    }
}
