using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FFS.Services.FileSystemScanner;
using FFS.Services.FileSystemScanner.NTFS;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FFS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            SetupDI();
            SetupLogger();
        }

        private void SetupDI()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IFSScanner, NTFSScannerService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        private void SetupLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
