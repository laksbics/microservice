using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repository;
using Post.Query.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory )
        {
            this._contextFactory = contextFactory;
        }


        public async Task CreateAsync(PostEntity post)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                context.Posts.Add( post );

                _ = await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
               var post = await GetByIdAsync(id);
                if (post == null) { return; }
                context.Posts.Remove(post);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task<PostEntity> GetByIdAsync(Guid id)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(x => x.PostId == id);
            }
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Posts.AsNoTracking().Include(p => p.Comments).ToListAsync();
            }
        }

        public async  Task<List<PostEntity>>  ListByAuthorAsync(string author)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Posts.Include(p => p.Comments).Where(x => x.Author.Contains(author)).ToListAsync();
            }
        }

        public async  Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Posts.Include(p => p.Comments).Where(x => x.Comments != null && x.Comments.Any()).ToListAsync();
            }
        }

        public async Task<List<PostEntity>> ListWithLikeAsync(int noOflike)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Posts.Include(p => p.Comments).Where(x => x.Likes > noOflike).ToListAsync();
            }
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                context.Posts.Update(post);

                _ = await context.SaveChangesAsync();
            }
        }
    }
}
