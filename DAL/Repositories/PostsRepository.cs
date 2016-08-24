using System.Configuration;
using System.Data.Entity;
using System.Linq;
using DAL.Models;

namespace DAL.Repositories
{
    public class PostsRepository
    {
        private EntityFrameworkContext _context;

        public PostsRepository(ConnectionStringSettings connectionStringSettings)
        {
            _context = new EntityFrameworkContext(connectionStringSettings);
        }

        public Post Find(int id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public int Total()
        {
            return _context.Posts.Count();
        }

        public void Save(Post post)
        {
            if (post.Id != 0)
            {
                _context.Posts.Attach(post);
                _context.Entry(post).State = EntityState.Modified;
            }
            else
            {
                _context.Posts.Add(post);
            }

            _context.SaveChanges();
        }
    }
}
