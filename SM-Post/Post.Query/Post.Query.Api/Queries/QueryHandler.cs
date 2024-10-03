using Post.Query.Domain.Entities;
using Post.Query.Domain.Repository;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _repository;

        public QueryHandler(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PostEntity>> handleAsync(FindAllPostQuery query)
        {
            return await _repository.ListAllAsync();
        }

        public async Task<List<PostEntity>> handleAsync(FindPostByIdQuery query)
        {
            var post = await _repository.GetByIdAsync(query.Id);
            return new List<PostEntity> { post };
        }

        public async Task<List<PostEntity>> handleAsync(FindPostByAuthorQuery query)
        {
            return await _repository.ListByAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> handleAsync(FindPostsWithComments query)
        {
            return await _repository.ListWithCommentsAsync();
        }

        public async Task<List<PostEntity>> handleAsync(FindPostsWithLikesQuery query)
        {
            return await _repository.ListWithLikeAsync(query.Likes);
        }
    }
}
