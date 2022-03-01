using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services
{
    public class SystemDrivesRetriever
    {
        public static DriveInfo[] RetrieveFixed(List<string> allowedFileSystems)
        {
            List<DriveInfo> allDrives = DriveInfo.GetDrives().ToList();
            for (int i = 0; i < allDrives.Count; ++i)
            {
                if (allDrives[i].DriveType != DriveType.Fixed)
                {
                    allDrives.RemoveAt(i);
                    continue;
                }

                if (!allowedFileSystems.Contains(allDrives[i].DriveFormat))
                {
                    allDrives.RemoveAt(i);
                    continue;
                }
            }

            return allDrives.ToArray();
        }
    }
}
