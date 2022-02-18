using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Castle.Components.DictionaryAdapter;
using FFS.Annotations;
using FFS.Services;
using FFS.Services.FileSystemScanner;
using FFS.Services.FileSystemScanner.NTFS;
using FFS.Utils;
using Microsoft.Toolkit.Mvvm.Input;
using Serilog;
using Swordfish.NET.Collections.Auxiliary;

namespace FFS.ViewModels
{
    public class SearchWindowViewModel : ViewModelBase
    {
        public AsyncRelayCommand QueryCommand { get; }

        public bool IsExecutingQuery
        {
            get => _isExecutingQuery;
            set 
            {
                SetProperty(ref _isExecutingQuery, value);
            }
        }
        private bool _isExecutingQuery;

        public ObservableCollection<INode> Files
        {
            get => _files;
            private set
            {
                SetProperty(ref _files, value);
            }
        }
        private ObservableCollection<INode> _files;

        private IFSScanner _fsScanner;
        private readonly object _collectionOfObjectsSync = new object();
        
        public SearchWindowViewModel(IFSScanner fsScanner)
        {
            _files = new ObservableCollection<INode>();

            IsExecutingQuery = false;
            _fsScanner = fsScanner;

            QueryCommand = new AsyncRelayCommand(ExecuteQuery);
            BindingOperations.EnableCollectionSynchronization(Files, _collectionOfObjectsSync);
        }

        private async Task ExecuteQuery()
        {
            if (null == _fsScanner)
            {
                Log.Error("Cannot execute query, IFSScanner is NULL");
                return;
            }

            IsExecutingQuery = true;

            DriveInfo[] drives = SystemDrivesRetriever.RetrieveFixed(new List<string>() { FileSystemDb.NTFS });

            Files.Clear();
            GC.Collect();

            List<INode> results = await _fsScanner.Scan(drives[2]) as List<INode> ?? new List<INode>();
            Files = new ObservableCollection<INode>(results);
            
            Log.Information($"Retrieved: {results.Count} from drive: {drives[0].Name}");
            results.Clear();
            
            IsExecutingQuery = false;
            GC.Collect();
        }
    }
}
