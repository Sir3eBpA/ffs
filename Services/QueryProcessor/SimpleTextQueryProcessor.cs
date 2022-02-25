using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cysharp.Text;
using FFS.Services.QueryBuilder;
using FFS.Utils;

namespace FFS.Services.QueryProcessor
{
    public class SimpleTextQueryProcessor : IQueryProcessor
    {
        public QueryValidationResult IsValid(string query)
        {
            if (null == query)
                return new QueryValidationResult(false, "Query is NULL");

            if (query.Length == 0)
                return new QueryValidationResult(false, "Query is empty");

            return QueryValidationResult.Valid;
        }

        public ObservableCollection<INode> Process(string query, IList<INode> files)
        {
            ObservableCollection<INode> result = new ObservableCollection<INode>();

            string finalQuery = query.Trim();
            bool searchExtension = finalQuery[0] == '.';

            foreach (INode file in files)
            {
                if (searchExtension && file.Extension.Contains(query))
                {
                    result.Add(file);
                }
                else
                {
                    if(file.Name.Contains(query))
                        result.Add(file);
                }
            }

            return result;
        }
    }
}
