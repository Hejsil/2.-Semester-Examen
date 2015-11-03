using System;
using NUnit.Framework;
using LineSystemCore;
using System.Collections.Generic;

namespace LineSystemCore.Test
{
    [TestFixture]
    public class UserTest
    {
        [TestCase(null, "Hansen", "Hanserik", "Hans@Hans.Hans", typeof(ArgumentNullException))]
        [TestCase("Hans", null, "Hanserik", "Hans@Hans.Hans", typeof(ArgumentNullException))]
        [TestCase("Hans", "Hansen", null, "Hans@Hans.Hans", typeof(ArgumentNullException))]
        [TestCase("Hans", "Hansen", "Hanserik!", "Hans@Hans.Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", null, typeof(ArgumentNullException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans@Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans@Hans@Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans.Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans@Hans.Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans@Hans@Hans@Hans.Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@.Hans.Hans", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans.Hans.", typeof(ArgumentException))]
        [TestCase("Hans", "Hansen", "Hanserik", "Hans@Hans.Hans!", typeof(ArgumentException))]
        public void InvalidFieldsTest(string firstName, string lastName, string userName, string eMail, Type expectedException)
        {
            Assert.Throws(expectedException, new TestDelegate(() => { var user = new User(firstName, lastName, userName, eMail); }));
        }

        [Test]
        public void ToStringTest()
        {
            var user = new User("Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");

            Assert.AreEqual("Hans Hansen(Hans@Hans.Hans)", user.ToString());
        }

        [Test]
        public void EqualsTest()
        {
            var user1 = new User("Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");
            var user2 = new User("Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");

            Assert.IsTrue(user1.Equals(user2));

            var user3 = new User("Hanss", "Hansen", "Hanserik", "Hans@Hans.Hans");

            Assert.IsFalse(user1.Equals(user3));
        }

        [Test]
        public void CompareToTest()
        {
            var userList = new List<User>();

            var user1 = new User("1Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");
            var user2 = new User("2Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");
            var user3 = new User("3Hans", "Hansen", "Hanserik", "Hans@Hans.Hans");

            userList.Add(user2);
            userList.Add(user1);
            userList.Add(user3);

            userList.Sort();

            Assert.IsTrue(user1.Equals(userList[0]));
            Assert.IsTrue(user2.Equals(userList[1]));
            Assert.IsTrue(user3.Equals(userList[2]));
        }
    }
}
