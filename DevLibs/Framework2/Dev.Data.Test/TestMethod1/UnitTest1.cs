namespace Dev.Data.Test
{
    using Dev.Data.ContextStorage;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTest1
    {
        #region Public Methods and Operators

        [TestMethod]
        public void TestMethod1()
        {
            DbContextManager.InitStorage(new SimpleDbContextStorage());
            DbContextManager.Init("DefaultConnection", new[] { "Dev.Data.Test" });
        }

        #endregion
    }
}