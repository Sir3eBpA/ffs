using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FFS.Models;
using FFS.Services.Exporters;
using FFS.Services.QueryBuilder;
using FFS.Services.QueryProcessor;
using FFS.Utils;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using MvvmDialogs;
using MvvmDialogs.DialogFactories;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs.FrameworkDialogs;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Serilog;
using Serilog.Core;

namespace FFS.ViewModels
{
    public class QueryPanelViewModel : ViewModelBase
    {
        public ObservableCollection<INode> Files
        {
            get => _files;
            set => SetProperty(ref _files, value);
        }
        private ObservableCollection<INode> _files;

        public long ScanDurationMS
        {
            get => _scanDurationMS;
            set => SetProperty(ref _scanDurationMS, value);
        }
        private long _scanDurationMS;

        public bool IsExecutingQuery
        {
            get => _isExecutingQuery;
            set
            {
                SetProperty(ref _isExecutingQuery, value);
            }
        }
        private bool _isExecutingQuery;

        public string Query
        {
            get => _query;
            set => SetProperty(ref _query, value);
        }
        private string _query;

        public INode SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateCommandsCanExecute();
            }
        }

        private INode _selectedItem;

        public AsyncRelayCommand QueryCommand { get; }
        public AsyncRelayCommand ExportCSVCommand { get; }

        public RelayCommand ShowInExplorerCommand { get; }
        public RelayCommand OpenCommand { get; }

        private ScanResult _model;

        private readonly IDialogService _dialogService;
        private readonly List<IRelayCommand> _commands;
        private readonly ExportSystem _exportSystem;

        public QueryPanelViewModel(IDialogService dialogService, ExportSystem exportSystem)
        {
            _dialogService = dialogService;
            _exportSystem = exportSystem;

            _files = new ObservableCollection<INode>();

            QueryCommand = new AsyncRelayCommand(DoQuery);
            ExportCSVCommand = new AsyncRelayCommand(ExportResultsToCSV);

            ShowInExplorerCommand = new RelayCommand(ShowInExplorer, () => null != SelectedItem);
            OpenCommand = new RelayCommand(Open, () => null != SelectedItem);

            _commands = new List<IRelayCommand>() { ShowInExplorerCommand, OpenCommand };
            UpdateCommandsCanExecute();

            ScanDurationMS = 0;
        }

        private void UpdateCommandsCanExecute()
        {
            if (null == _commands)
            {
                Log.Warning("Trying to update _commands can execute but collection is null");
                return;
            }

            foreach (IRelayCommand command in _commands)
            {
                command.NotifyCanExecuteChanged();
            }
        }

        [SupportedOSPlatform("windows7.0")]
        private async Task ExportResultsToCSV()
        {
            SaveFileDialogSettings settings = new SaveFileDialogSettings()
            {
                AddExtension = true,
                DefaultExt = ".csv",
                CheckPathExists = true,
                Title = "Select folder to export search results",
                OverwritePrompt = true,
                Filter = "CSV files (.csv)|*.csv|Text files (.txt)|*.txt",
            };

            bool? success = _dialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                Log.Logger.Information($"Exporting {Files.Count} results to CSV at path: {settings.FileName}");
                try
                {
                    IsExecutingQuery = true;
                    await _exportSystem.Export<CSVExporter>(Files, settings.FileName);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex.Message);
                    MessageBox.Show("Error exporting to CSV, reason: " + ex.Message, "Error exporting!");
                }
                finally
                {
                    IsExecutingQuery = false;
                }
            }
        }

        private void Open()
        {
            try
            {
                WindowsUtils.OpenFileWithAssociatedSoftware(SelectedItem.FullName);
            }
            catch (Exception ex)
            {
                Log.Error($"Open file error: {ex.Message}");
                MessageBox.Show($"Cannot open file at path: {SelectedItem.FullName} ; reason: {ex.Message}");
            }
        }

        private void ShowInExplorer()
        {
            try
            {
                WindowsUtils.ShowFileInExplorer(SelectedItem.FullName);
            }
            catch (Exception ex)
            {
                Log.Error($"Show in explorer error: {ex.Message}");
                MessageBox.Show($"Cannot show file in explorer at path: {SelectedItem.FullName} ; reason: {ex.Message}");
            }
        }

        private async Task DoQuery()
        {
            IsExecutingQuery = true;

            SimpleTextQueryProcessor textQuery = new SimpleTextQueryProcessor();
            QueryValidationResult res = textQuery.IsValid(Query);

            Stopwatch sw = Stopwatch.StartNew();
            if (res.IsValid)
            {
                Queries.SetActiveQuery(Query, textQuery);

                Files?.Clear();
                Files = await Task.Run(() => textQuery.Process(Query, _model.Files));
            }
            ScanDurationMS = sw.ElapsedMilliseconds;

            IsExecutingQuery = false;
        }

        public void SetData(ScanResult mdl)
        {
            _model = mdl ?? throw new NullReferenceException("model");
            Files = new ObservableCollection<INode>(mdl.Files);
        }

        public override void OnDispose()
        {
            base.OnDispose();

            if (null != _model)
            {
                _model.Files?.Clear();
                _model.ScannedDrives?.Clear();
            }
            _model = null;

            Files?.Clear();

            IsExecutingQuery = false;
            GC.Collect();
        }

        public struct FileNodeHighlight
        {
            public Range Range { get; }

            public FileNodeHighlight(Range r)
            {
                Range = r;
            }
        }
    }
}
