using System;
using NUnit.Framework;
using LineSystemCore;
using System.Collections.Generic;

namespace LineSystemCore.Test
{
    [TestFixture]
    public class LineSystemTest
    {
        LineSystem lineSystem;
        User testUser;

        public LineSystemTest()
        {
            lineSystem = new LineSystem(null);
            testUser = new User("test", "test", "test", "test@test.test", 100000);
            
            lineSystem.Products.Add(new Product("test1", 101, true, 24));
            lineSystem.Products.Add(new Product("test2", 101, true));
            lineSystem.Products.Add(new Product("test3", 101, true));

            lineSystem.Users.Add(testUser);
            lineSystem.Users.Add(new User("test2", "test2", "test2", "test2@test2.test2"));
            lineSystem.Users.Add(new User("test3", "test3", "test3", "test3@test3.test3"));

            lineSystem.Transactions.Add(new BuyTransaction(DateTime.Now, testUser, new Product("test", 99, true)));
            lineSystem.Transactions.Add(new BuyTransaction(DateTime.Now, lineSystem.Users[1], new Product("test", 99, true)));
            lineSystem.Transactions.Add(new InsertCashTransaction(DateTime.Now, testUser, 100));
            lineSystem.Transactions.Add(new InsertCashTransaction(DateTime.Now, lineSystem.Users[2], 100));
            
        }

        [Test]
        public void NullBuyProductTest()
        {
            Assert.Null(lineSystem.BuyProduct(null, null));
            Assert.Null(lineSystem.BuyProduct(new User("test", "test", "test", "test@test.test"), null));
            Assert.Null(lineSystem.BuyProduct(null, new Product("test", 1337, true)));
            Assert.NotNull(lineSystem.BuyProduct(new User("test", "test", "test", "test@test.test"), new Product("test", 1337, true)));
        }

        [Test]
        public void NullAddCreditsToAccount()
        {
            Assert.Null(lineSystem.AddCreditsToAccount(null, 200));
            Assert.NotNull(lineSystem.AddCreditsToAccount(testUser, 200));
        }

        [Test]
        public void GetTransactionListTest()
        {
            Assert.AreEqual(2, lineSystem.GetTransactionList(testUser).Count);
            Assert.AreEqual(1, lineSystem.GetTransactionList(lineSystem.Users[1]).Count);
            Assert.AreEqual(0, lineSystem.GetTransactionList(new User("tes123t", "tes123t", "te123st", "tes123t@tes123t.tes123t")).Count);
            Assert.AreEqual(0, lineSystem.GetTransactionList(null).Count);
        }

        [Test]
        public void ExecuteTransactionTest()
        {
            Assert.AreEqual(false, lineSystem.ExecuteTransaction(null));
            Assert.AreEqual(false, lineSystem.ExecuteTransaction(new BuyTransaction(DateTime.Now, testUser, new Product("test", 10000000, true))));
            Assert.AreEqual(true, lineSystem.ExecuteTransaction(new BuyTransaction(DateTime.Now, testUser, new Product("test1", 100, true))));
            lineSystem.Transactions.RemoveAt(4);
            Assert.AreEqual(true, lineSystem.ExecuteTransaction(new BuyTransaction(DateTime.Now, testUser, new Product("test2", 99, true))));
            lineSystem.Transactions.RemoveAt(4);
            Assert.AreEqual(true, lineSystem.ExecuteTransaction(new InsertCashTransaction(DateTime.Now, testUser, 100)));
            lineSystem.Transactions.RemoveAt(4);
        }

        [Test]
        public void NullGetProduct()
        {
            Assert.Null(lineSystem.GetProduct(10000));
            Assert.NotNull(lineSystem.GetProduct(24));
        }

        [Test]
        public void NullGetUser()
        {
            Assert.Null(lineSystem.GetUser("test123"));
            Assert.NotNull(lineSystem.GetUser("test"));
        }

        [Test]
        public void GetActiveProductsTest()
        {
            Assert.AreEqual(3, lineSystem.GetActiveProducts().Count);
        }
    }
}
