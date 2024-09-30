using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Domain.Repository
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Guid id);
        Task<PostEntity> GetByIdAsync(Guid id);
        Task<List<PostEntity>> ListAllAsync();
        Task<List<PostEntity>> ListByAuthorAsync(string author);
        Task<List<PostEntity>> ListWithLikeAsync(int noOflike);
        Task<List<PostEntity>> ListWithCommentsAsync(int noOflike);
    }
}
