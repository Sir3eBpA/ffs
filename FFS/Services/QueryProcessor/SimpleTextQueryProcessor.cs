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
            if (string.IsNullOrWhiteSpace(query))
                return new QueryValidationResult(false, "Query is NULL");

            if (query.Length == 0)
                return new QueryValidationResult(false, "Query is empty");

            return QueryValidationResult.Valid;
        }

        public ObservableCollection<INode> Process(string query, IList<INode> files, int limit)
        {
            if (!IsValid(query).IsValid)
                throw new ArgumentException("invalid query");

            if (null == files)
                throw new ArgumentNullException("files");

            ObservableCollection<INode> result = new ObservableCollection<INode>();

            string finalQuery = query.Trim();
            bool searchExtension = finalQuery[0] == '.';
            bool addAll = finalQuery[0] == '*';
            var finalQuerySpan = finalQuery.AsSpan();

            foreach (INode file in files)
            {
                if(limit != -1 && result.Count >= limit)
                    break;

                if (addAll)
                {
                    result.Add(file);
                    continue;
                }

                if (searchExtension)
                {
                    if (file.Extension.Contains(query))
                    {
                        result.Add(file);
                    }
                }
                else
                {
                    if(IsMatchingFilenameNoExtension(query, file, finalQuerySpan))
                        result.Add(file);
                }
            }

            return result;
        }

        /// <summary>
        /// Check filename matching the query excluding any extensions
        /// </summary>
        /// <param name="query">Query request, either extension or file name</param>
        /// <param name="file">File MFT reference node</param>
        /// <param name="finalQuerySpan">Span of the final 'normalized' query we received</param>
        /// <returns>TRUE if file matches filters</returns>
        private bool IsMatchingFilenameNoExtension(string query, INode file, ReadOnlySpan<char> finalQuerySpan)
        {
            bool add = false;
            ReadOnlySpan<char> fileName = file.Name.AsSpan();
            // check if there's any extension in this file name
            int extensionStart = fileName.IndexOf('.');
            if (extensionStart > -1)
            {
                ReadOnlySpan<char> fileNameNoExtension = fileName.Slice(0, extensionStart);
                string str = fileNameNoExtension.ToString();
                Console.WriteLine(str);
                if (fileNameNoExtension.Contains(finalQuerySpan, StringComparison.InvariantCulture))
                    add = true;
            }
            else // no extension
            {
                if (file.Name.Contains(query))
                    add = true;
            }

            return add;
        }
    }
}
