
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Handler;
using System.Runtime.InteropServices;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourceHandler<PostAggregate> _eventSourceHandler;
        public CommandHandler(IEventSourceHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourceHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditMessage(command.Message);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.AddComment(command.Comment, command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.EditComment(command.CommentId, command.Comment, command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.RemoveComment(command.CommentId , command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = await _eventSourceHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost( command.UserName);

            await _eventSourceHandler.SaveAsync(aggregate);
        }
    }
}
