using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Utils
{
    public static class WindowsUtils
    {
        public static void ShowFileInExplorer(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            string arg = $"/select, \"{path}\"";
            Process.Start("explorer.exe", arg);
        }

        public static Process OpenFileWithAssociatedSoftware(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            var process = new Process()
            {
                StartInfo = { UseShellExecute = true, FileName = path }
            };
            process.Start();
            return process;
        }
    }
}
