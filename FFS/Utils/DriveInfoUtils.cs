using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Utils
{
    public static class DriveInfoUtils
    {
        public static int GetUsagePercentage(this DriveInfo drive)
        {
            return (int)(100 * (double)drive.TotalFreeSpace / drive.TotalSize);
        }
    }
}
