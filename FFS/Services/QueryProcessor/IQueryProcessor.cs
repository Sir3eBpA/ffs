using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services.QueryBuilder
{
    public struct QueryValidationResult
    {
        public static QueryValidationResult Valid = new QueryValidationResult(true, string.Empty);

        public QueryValidationResult(bool isValid, string issue)
        {
            IsValid = isValid;
            Issue = issue;
        }

        public bool IsValid { get; set; }
        public string Issue { get; set; }
    }

    public interface IQueryProcessor
    {
        QueryValidationResult IsValid(string query);

        ObservableCollection<INode> Process(string query, IList<INode> files, int limit);
    }
}
