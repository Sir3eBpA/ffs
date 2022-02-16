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
        public static DriveInfo GetDriveInfoWithDiskNameAsLetter(this DriveInfo drive)
        {
            if (null == drive)
                return null;

            string letter = drive.Name.StripToCharactersOnly();
            if (string.IsNullOrWhiteSpace(letter))
                return null;

            return new DriveInfo(letter);
        }
    }
}
