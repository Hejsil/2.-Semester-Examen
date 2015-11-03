using System;
using NUnit.Framework;
using LineSystemCore;
using System.Collections.Generic;

namespace LineSystemCore.Test
{
    [TestFixture]
    public class BuyTransactionTest
    {
        [Test]
        public void ExecuteTest()
        {
            var user = new User("test", "test", "test", "test@test.test", 99);
            var product1 = new BuyTransaction(DateTime.Now, user, new Product("test", 100, true));
            var product2 = new BuyTransaction(DateTime.Now, user, new Product("test", 0, false));
            Assert.Throws<InsufficientCreditsException>(new TestDelegate(() => { product1.Execute(); }));
            Assert.Throws<NotActiveException>(new TestDelegate(() => { product2.Execute(); }));
        }
    }
}
