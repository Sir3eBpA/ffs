using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Threading.Tasks;
using FFS.Models;
using FFS.Services;
using FFS.Services.FileSystemScanner;
using FFS.Services.FileSystemScanner.NTFS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Serilog;

namespace FFS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
            }
        }
        private ViewModelBase _currentPage;

        
        public MainWindowViewModel()
        {
            DiskScanViewModel discScanVM = App.DI.GetService<DiskScanViewModel>();
            if (null != discScanVM)
            {
                discScanVM.ScanCompleted += DiscScanVMOnScanCompleted;
                CurrentPage = discScanVM;
            }
        }

        private void DiscScanVMOnScanCompleted(ScanResult res)
        {
            CurrentPage = App.DI.GetService<QueryPanelViewModel>();
        }
    }
}
