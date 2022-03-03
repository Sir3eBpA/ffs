using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using FFS.Services.Reporting;

namespace FFS.Services.Exporters
{
    public class CSVExporter : IExporter
    {
        struct ExportData
        {
            public string FileName { get; set; }
            public string Extension { get; set; }
            public ulong Size { get; set; }
        }
        
        public void Export(IList<INode> nodes, TextWriter stream)
        {
            if (null == nodes)
                throw new ArgumentNullException("nodes");

            if (null == stream)
                throw new ArgumentNullException("stream");

            using (var csv = new CsvWriter(stream, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader(typeof(ExportData));
                csv.NextRecord();

                for (int i = 0; i < nodes.Count; i++)
                {
                    ExportData d = new ExportData()
                    {
                        FileName = nodes[i].FullName,
                        Extension = nodes[i].Extension,
                        Size = nodes[i].Size,
                    };
                    csv.WriteRecord(d);
                    csv.NextRecord();
                }
            }
        }
    }
}
