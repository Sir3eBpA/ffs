using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Services.Reporting;

namespace FFS.Services.Exporters
{
    public class ExportSystem
    {
        private IExporter[] _availableExporters;

        public ExportSystem(IEnumerable<IExporter> exporters)
        {
            _availableExporters = exporters.ToArray();
        }

        public async Task Export<T>(IList<INode> data, string path) where T : IExporter
        {
            IExporter exporter = _availableExporters.First(x => x.GetType() == typeof(T));
            if (null == exporter)
            {
                throw new ArgumentException($"No exporter of type: {typeof(T)} registered");
            }

            using (TextWriter tw = new StreamWriter(path))
            {
                await Task.Run(() => exporter.Export(data, tw));
            }
        }
    }
}
