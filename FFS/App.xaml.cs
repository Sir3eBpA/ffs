using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdonisUI.Extensions;
using FFS.Services.Exporters;
using FFS.Services.FileSystemScanner;
using FFS.Services.FileSystemScanner.NTFS;
using FFS.Services.Reporting;
using FFS.ViewModels;
using FFS.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using MvvmDialogs.DialogFactories;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs.FrameworkDialogs;
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

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
                window.Title = $"FFS v{version.Major}.{version.Minor}.{version.Build}";
            else
                window.Title = "FFS, no version?";

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
            services.AddTransient<QueryPanelViewModel>();

            // Register mvvm-dialogues dependencies
#pragma warning disable CA1416 // Validate platform compatibility
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IDialogFactory, ReflectionDialogFactory>();
            services.AddTransient<IDialogTypeLocator, NamingConventionDialogTypeLocator>();
            services.AddTransient<IFrameworkDialogFactory, DefaultFrameworkDialogFactory>();
#pragma warning restore CA1416 // Validate platform compatibility

            // register exporters
            services.AddTransient<ExportSystem>();
            services.AddTransient<IExporter, CSVExporter>();

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
