using Microsoft.VisualStudio.TestTools.UnitTesting;
using FFS.Services.FileSystemScanner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFS.Services.FileSystemScanner.Tests
{
    [TestClass()]
    public class NTFSScannerServiceTests
    {
        [TestMethod()]
        public void NTFSScannerHasAnySupportedFileSystem()
        {
            NTFSScannerService s = new NTFSScannerService();
            Assert.IsTrue(s.GetSupportedFileSystems().Any());
        }

        [TestMethod()]
        public void NullDriveFailsValidation()
        {
            NTFSScannerService s = new NTFSScannerService();
            Assert.IsFalse(s.IsSupported(null));
        }

        [TestMethod]
        public async Task NullDrivesListScanThrows()
        {
            NTFSScannerService s = new NTFSScannerService();

            await Task.Run(() => s.Scan(null)).ContinueWith(t =>
            {
                Assert.IsTrue(t.IsFaulted);
            });
        }

        [TestMethod]
        public async Task EmptyDrivesListReturnsEmptyResult()
        {
            NTFSScannerService s = new NTFSScannerService();
            var res = await s.Scan(new List<DriveInfo>());
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Count == 0);
        }
    }
}