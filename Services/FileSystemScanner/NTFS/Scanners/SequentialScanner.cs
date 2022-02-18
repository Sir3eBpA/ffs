using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Utils;

namespace FFS.Services.FileSystemScanner.Scanners
{
    public class SequentialScanner : ScannerBase
    {
        public SequentialScanner(DriveInfo drive) : base(drive) {}
        public SequentialScanner(DriveInfo drive, Func<INode, bool> filter) : base(drive, filter) {}

        public override async Task<IList<INode>> DoScan()
        {
            return await Task.Run(() =>
            {
                IList<INode> nodes = new List<INode>();
                DriveInfo drive = _drive.GetDriveInfoWithDiskNameAsLetter();
                var nftReader = new NtfsReader(drive, RetrieveMode.Minimal);

                Stopwatch sw = Stopwatch.StartNew();
                foreach (INode data in nftReader.GetNodes(_drive.Name))
                {
                    if ((data.Attributes & Attributes.Directory) != 0)
                        continue;

                    nodes.Add(data);
                }
                sw.Stop();

                Console.WriteLine($"DoScan took: {sw.Elapsed}");
                return nodes;
            });
        }
    }
}
