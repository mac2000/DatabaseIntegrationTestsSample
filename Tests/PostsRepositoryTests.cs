using System;
using DAL.Models;
using DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PostsRepositoryTests : AbstractRepositoryTests<PostsRepository>
    {
        public override PostsRepository InitializeRepository() => new PostsRepository(GlobalInitializer.ConnectionStringSettings);

        [TestMethod]
        public void AtStartWeHaveOnePost()
        {
            Assert.AreEqual(1, Repository.Total());
        }

        [TestMethod]
        public void CanFindPostsById()
        {
            var post = Repository.Find(1);
            Assert.IsNotNull(post);
        }

        [TestMethod]
        public void CanSavePost()
        {
            var total = Repository.Total();
            var post = new Post
            {
                Title = "Hello World"
            };
            Repository.Save(post);
            Assert.AreNotEqual(0, post.Id);
            Assert.AreEqual(total + 1, Repository.Total());
        }

        [TestMethod]
        public void UpdatesExistingPosts()
        {
            var total = Repository.Total();
            var post = new Post
            {
                Id = 1,
                Title = "Hello World",
                CreatedAt = DateTime.Now.Date
            };
            Repository.Save(post);
            Assert.AreEqual(1, post.Id);
            Assert.AreEqual(total, Repository.Total());
            post = Repository.Find(1);
            Assert.AreEqual(DateTime.Now.Date, post.CreatedAt.Date);
        }
    }
}
