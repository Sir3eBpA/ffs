using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Models;
using FFS.Services.QueryBuilder;
using FFS.Services.QueryProcessor;
using Microsoft.Toolkit.Mvvm.Input;

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

        public ObservableCollection<FileNodeHighlight> Highlights
        {
            get => _highlights;
            set => SetProperty(ref _highlights, value);
        }
        private ObservableCollection<FileNodeHighlight> _highlights;

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

        public AsyncRelayCommand QueryCommand { get; }
        private ScanResult _model;

        public QueryPanelViewModel()
        {
            _files = new ObservableCollection<INode>();
            QueryCommand = new AsyncRelayCommand(DoQuery);
        }

        private async Task DoQuery()
        {
            IsExecutingQuery = true;

            SimpleTextQueryProcessor textQuery = new SimpleTextQueryProcessor();
            QueryValidationResult res = textQuery.IsValid(Query);
            if (res.IsValid)
            {
                Queries.SetActiveQuery(Query, textQuery);

                Files?.Clear();
                Files = await Task.Run(() => textQuery.Process(Query, _model.Files));

                Highlights = await Task.Run(() =>
                {
                    var data = new ObservableCollection<FileNodeHighlight>();
                    for (int i = 0; i < _files.Count; i++)
                    {
                        var r = new Range(0, 5);
                        data.Add(new FileNodeHighlight(r));
                    }
                    return data;
                });
            }

            IsExecutingQuery = false;
        }

        public void SetData(ScanResult mdl)
        {
            _model = mdl ?? throw new NullReferenceException("model");
            Files = new ObservableCollection<INode>(mdl.Files);
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
