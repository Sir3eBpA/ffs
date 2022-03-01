using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Filesystem.Ntfs;

namespace FFS.Services.QueryProcessor.Tests;

class MockNode : INode
{
    public MockNode(string name)
    {
        Name = name;
        FullName = "";
        Streams = null;
        Extension = Path.GetExtension(name);
    }

    public Attributes Attributes { get; }
    public uint NodeIndex { get; }
    public uint ParentNodeIndex { get; }
    public string Name { get; }
    public ulong Size { get; }
    public string FullName { get; }
    public string Extension { get; }
    public IList<IStream>? Streams { get; }
    public DateTime CreationTime { get; }
    public DateTime LastChangeTime { get; }
    public DateTime LastAccessTime { get; }
}