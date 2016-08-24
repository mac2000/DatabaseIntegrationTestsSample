using System.Configuration;
using System.Transactions;
using DAL;
using DAL.Models;
using DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PostsRepositoryTests
    {
        private PostsRepository _postsRepository;
        private TransactionScope _trans;

        [TestInitialize]
        public void InitializeTest()
        {
            _postsRepository = new PostsRepository(new ConnectionStringSettings("Default", GlobalInitializer.SqlConnectionString));
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _trans.Dispose();
        }

        [TestMethod]
        public void TestMethod0()
        {
            Assert.AreEqual(1, _postsRepository.Total());
        }

        [TestMethod]
        public void TestMethod1()
        {
            var post = _postsRepository.Find(1);
            Assert.IsNotNull(post);
        }

        [TestMethod]
        public void CanSavePost()
        {
            var total = _postsRepository.Total();
            var post = new Post
            {
                Title = "Hello World"
            };
            _postsRepository.Save(post);
            Assert.AreNotEqual(0, post.Id);
            Assert.AreEqual(total + 1, _postsRepository.Total());
        }
    }
}
