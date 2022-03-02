using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Utils;

namespace FFS.Services
{
    public class ScannableDrive
    {
        public event Action<bool> CheckedChanged; 

        public DriveInfo Drive => _drive;
        public int TakenStoragePct => 100 - _drive.GetUsagePercentage();
        public string DriveName => _drive.Name;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                CheckedChanged?.Invoke(value);
            }
        }
        private bool _isChecked;

        public ScannableDrive(DriveInfo drive)
        {
            _drive = drive;
        }

        private DriveInfo _drive;
    }
}
