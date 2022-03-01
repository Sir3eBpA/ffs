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
using Serilog;

namespace FFS.Services.FileSystemScanner.Scanners
{
    public class SequentialScanner : ScannerBase
    {
        public SequentialScanner(IList<DriveInfo> drives) : base(drives) {}
        public SequentialScanner(IList<DriveInfo> drives, Func<INode, bool> filter) : base(drives, filter) {}

        public override async Task<IList<INode>> DoScan()
        {
            return await Task.Run(() =>
            {
                IList<INode> nodes = new List<INode>();
                Stopwatch sw = Stopwatch.StartNew();

                foreach (DriveInfo driveInfo in _drives)
                {
                    NtfsReader nftReader = new NtfsReader(driveInfo, RetrieveMode.Minimal);

                    Log.Logger.Information("Reading MFT took: " + sw.Elapsed);
                    foreach (INode data in nftReader.GetNodes(driveInfo.Name))
                    {
                        if ((data.Attributes & Attributes.Directory) != 0)
                            continue;

                        nodes.Add(data);
                    }
                    Log.Logger.Information("Adding file nodes took: " + sw.Elapsed);
                }
                sw.Stop();

                Console.WriteLine($"DoScan (total) took: {sw.Elapsed}");
                return nodes;
            });
        }
    }
}
