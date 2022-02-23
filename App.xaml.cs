using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdonisUI.Extensions;
using FFS.Services.FileSystemScanner;
using FFS.Services.FileSystemScanner.NTFS;
using FFS.ViewModels;
using FFS.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace FFS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider DI { get; private set; }

        public App()
        {
            InitializeComponent();
            SetupDI();
            SetupLogger();

            MainWindow window = new MainWindow() {DataContext = DI.GetService<MainWindowViewModel>()};
            window.Show();
        }

        private void SetupDI()
        {
            ServiceCollection services = new ServiceCollection();

            // Singletons
            services.AddSingleton<IFSScanner, NTFSScannerService>();
            services.AddSingleton<MainWindowViewModel>();

            // Transient services
            services.AddTransient<DiskScanViewModel>();

            DI = services.BuildServiceProvider();
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
