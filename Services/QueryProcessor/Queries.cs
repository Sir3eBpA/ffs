using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Services.QueryBuilder;

namespace FFS.Services.QueryProcessor
{
    public static class Queries
    {
        public static string ActiveQuery { get; private set; }
        public static IQueryProcessor ActiveQueryProcessor { get; private set; }

        public static void SetActiveQuery(string query, IQueryProcessor queryProcessor)
        {
            if(string.IsNullOrEmpty(query) || null == queryProcessor)
                return;

            ActiveQuery = query;
            ActiveQueryProcessor = queryProcessor;
        }
    }
}
