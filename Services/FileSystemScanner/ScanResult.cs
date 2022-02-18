using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;

namespace FFS.Services.FileSystemScanner
{
    public class ScanResult
    {
        public DriveInfo Drive { get; }
        public List<INode> ScannedFiles { get; }
        public int FilesCount => ScannedFiles?.Count ?? 0;

        public ScanResult(DriveInfo drive)
        {
            Drive = drive;
            ScannedFiles = new List<INode>();
        }

        public void AddFiles(IEnumerable<INode> files)
        {
            ScannedFiles.AddRange(files);
        }

        public void AddFile(INode file)
        {
            ScannedFiles.Add(file);
        }
    }
}