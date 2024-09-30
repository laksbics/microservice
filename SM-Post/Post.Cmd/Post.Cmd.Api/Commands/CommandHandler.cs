
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using System.Runtime.InteropServices;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourceHandler<PostAggregate> _eventSourceHandler;
        public async Task HandlerAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.EditMessage(command.Message);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.LikePost();

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.AddComment(command.Comment, command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.RemoveComment(command.CommentId , command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandlerAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsnyuc(command.Id);
            aggregate.DeletePost( command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }
    }
}
