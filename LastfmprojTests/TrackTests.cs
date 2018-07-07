using Microsoft.VisualStudio.TestTools.UnitTesting;
using lastfm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lastfm.Tests
{
    [TestClass()]
    public class TrackTests
    {
        [TestMethod()]
        public void methodTest()
        {
            //Assert.Fail();
            Assert.AreEqual(Track.method(), 4);
        }
    }
}