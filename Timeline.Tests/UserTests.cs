using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Timeline.Tests
{
    /// <summary>
    /// Tests for user obejcts.
    /// </summary>
    [TestClass]
    public class UserTests
    {
        /// <summary>
        /// Check that password hashing works as expected.
        /// </summary>
        [TestMethod]
        public void PasswordHashing()
        {
            string password = "my_password";
            string hash = Data.User.HashPassword(password);

            Assert.AreNotEqual(password, hash);

            Assert.IsTrue(Data.User.VerifyHash(password, hash));
            Assert.IsFalse(Data.User.VerifyHash(password + "x", hash));
        }
    }
}
