using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<PostEntity>> handleAsync(FindAllPostQuery query);
        Task<List<PostEntity>> handleAsync(FindPostByIdQuery query);
        Task<List<PostEntity>> handleAsync(FindPostByAuthorQuery query);
        Task<List<PostEntity>> handleAsync(FindPostsWithComments query);
        Task<List<PostEntity>> handleAsync(FindPostsWithLikesQuery query);
    }
}
