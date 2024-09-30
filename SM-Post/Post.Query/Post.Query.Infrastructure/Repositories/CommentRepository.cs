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
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            this._contextFactory = contextFactory;
        }

        public async Task CreateAsync(ComentEntity comment)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                context.Coments.Add(comment);

                _ = await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid CommentId)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                var comment = await GetByIdAsync(CommentId);
                if (comment == null) { return; }
                context.Coments.Remove(comment);
                _ = await context.SaveChangesAsync();
            }
        }

        public async Task<ComentEntity> GetByIdAsync(Guid CommentId)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                return await context.Coments.FirstOrDefaultAsync(x => x.CommentId == CommentId);
            }
        }

        public async Task UpdateAsync(ComentEntity comment)
        {
            using (DataBaseContext context = _contextFactory.CreateDbContext())
            {
                context.Coments.Update(comment);

                _ = await context.SaveChangesAsync();
            }
        }
    }
}
