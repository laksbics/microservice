using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Domain.Repository
{
    public interface ICommentRepository
    {
        Task CreateAsync(ComentEntity comment);
        Task UpdateAsync(ComentEntity comment);
        Task<ComentEntity> GetByIdAsync(Guid CommentId);
        Task DeleteAsync(Guid CommentId);
    }
}
