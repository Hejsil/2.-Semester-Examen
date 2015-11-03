using System;
using NUnit.Framework;
using LineSystemCore;
using System.Collections.Generic;

namespace LineSystemCore.Test
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        public void NameTest()
        {
            Assert.Throws<ArgumentNullException>(new TestDelegate(() => { var product = new Product(null, 11, true); }));
        }

        [Test]
        public void TwoProductsWithSameID()
        {
            var product = new Product("thing", 11, true, 100032);
            Assert.Throws<ArgumentException>(new TestDelegate(() => { var product2 = new Product("thing", 11, true, 100032); }));
        }
    }
}
