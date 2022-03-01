using Microsoft.VisualStudio.TestTools.UnitTesting;
using FFS.Services.QueryProcessor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFS.Services.QueryBuilder;

using INode = System.IO.Filesystem.Ntfs.INode;

namespace FFS.Services.QueryProcessor.Tests
{
    [TestClass()]
    public class SimpleTextQueryProcessorTests
    {
        private IList<INode>? _filledMockNodesList;

        [TestInitialize()]
        public void Init()
        {
            _filledMockNodesList = new List<INode>() {
                new MockNode("File.pdf"),
                new MockNode("pdfFile.aa")
            };
        }

        //
        // Is Valid checks
        //

        [TestMethod()]
        public void EmptyQueryReturns_InvalidResult()
        {
            var q = new SimpleTextQueryProcessor();
            QueryValidationResult res = q.IsValid("");
            Assert.AreNotEqual(res, QueryValidationResult.Valid);
        }

        [TestMethod()]
        public void NullQueryReturns_InvalidResult()
        {
            var q = new SimpleTextQueryProcessor();
            QueryValidationResult res = q.IsValid(null);
            Assert.AreNotEqual(res, QueryValidationResult.Valid);
        }

        [TestMethod()]
        public void NonEmptyQueryReturns_ValidResult()
        {
            var q = new SimpleTextQueryProcessor();
            QueryValidationResult res = q.IsValid("123456789");
            Assert.AreEqual(res, QueryValidationResult.Valid);
        }

        //
        // Process checks
        //

        [TestMethod]
        public void InvalidQueryProcess_ThrowsArgumentException()
        {
            var q = new SimpleTextQueryProcessor();
            Assert.ThrowsException<ArgumentException>(() => q.Process(null, null));
        }

        [TestMethod]
        public void EmptyFilesQueryProcess_ThrowsArgumentNullException()
        {
            var q = new SimpleTextQueryProcessor();
            Assert.ThrowsException<ArgumentNullException>(() => q.Process("123", null));
        }

        [TestMethod]
        public void ExtensionQueryReturns_ExtensionMatchOnly()
        {
            var q = new SimpleTextQueryProcessor();
            ObservableCollection<INode> res = q.Process(".pdf", _filledMockNodesList);

            Assert.IsNotNull(res);
            // make sure we only get 1 file and that file has pdf extension
            Assert.AreEqual(res[0], _filledMockNodesList?[0]);
            // make sure there's only 1 file as a result
            Assert.IsTrue(res.Count == 1);
        }

        [TestMethod]
        public void NameQueryReturns_NameMatchOnly()
        {
            var q = new SimpleTextQueryProcessor();
            ObservableCollection<INode> res = q.Process("pdf", _filledMockNodesList);

            Assert.IsNotNull(res);
            // make sure we only get 1 file and that file has pdf extension
            Assert.AreEqual(res[0], _filledMockNodesList?[1]);
            // make sure there's only 1 file as a result
            Assert.IsTrue(res.Count == 1);
        }

        [TestMethod]
        public void EmptyFilesInputReturn_EmptyList()
        {
            List<INode> nodes = new List<INode>();

            var q = new SimpleTextQueryProcessor();
            ObservableCollection<INode> res = q.Process(".pdf", nodes);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count == 0);
        }

        [TestMethod]
        public void StarSymbolReturns_AllFiles()
        {
            var q = new SimpleTextQueryProcessor();
            ObservableCollection<INode> res = q.Process("*", _filledMockNodesList);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count == 2);
        }

        [TestMethod]
        public void QueryProcessor_TrimsWhiteSpaces()
        {
            var q = new SimpleTextQueryProcessor();
            ObservableCollection<INode> res = q.Process("       *    ", _filledMockNodesList);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count == 2);
        }
    }
}