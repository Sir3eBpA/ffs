using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Models;
using FFS.Services.QueryBuilder;
using FFS.Services.QueryProcessor;
using Microsoft.Toolkit.Mvvm.Input;
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
                OpenCommand.NotifyCanExecuteChanged();
                ShowInExplorerCommand.NotifyCanExecuteChanged();
            }
        }

        private INode _selectedItem;

        public AsyncRelayCommand QueryCommand { get; }
        public RelayCommand ShowInExplorerCommand { get; }
        public RelayCommand OpenCommand { get; }
        private ScanResult _model;

        public QueryPanelViewModel()
        {
            _files = new ObservableCollection<INode>();
            QueryCommand = new AsyncRelayCommand(DoQuery);

            ShowInExplorerCommand = new RelayCommand(ShowInExplorer, () => null != SelectedItem);
            OpenCommand = new RelayCommand(Open, () => null != SelectedItem);

            ShowInExplorerCommand.NotifyCanExecuteChanged();
            OpenCommand.NotifyCanExecuteChanged();

            ScanDurationMS = 0;
        }

        private void Open()
        {
            var process = new Process()
            {
                StartInfo = { UseShellExecute = true, FileName = SelectedItem.FullName }
            };
            process.Start();
        }

        private void ShowInExplorer()
        {
            string arg = $"/select, \"{SelectedItem.FullName}\"";
            Process.Start("explorer.exe", arg);
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
