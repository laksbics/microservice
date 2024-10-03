using CQRS.Core.Domain;
using CQRS.Core.Messages;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string,string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {

        }
        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvents(new PostCreatedEvent {  Id = id, Author = author, Message = message, DatePosted = DateTime.Now });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not edit the message of an inactive post!");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"THe value of {nameof(message)} cannot be null or empty.");
            }

            RaiseEvents(new MessageUpdatedEvent
            {
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not like the inactive post!");
            }

            RaiseEvents(new PostLikedEvent { Id = _id });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not add comment to the inactive post!");
            }


            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"THe value of {nameof(comment)} cannot be null or empty.");
            }

            RaiseEvents(new CommentAddedEvent { Id = _id, CommentId = Guid.NewGuid(), Comment = comment, UserName = username, CommentDate = DateTime.Now });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not Edit comment to the inactive post!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"THe value of {nameof(comment)} cannot be null or empty.");
            }

            RaiseEvents(new CommentUpdatedEvent { Id = _id, CommentId = commentId, Comment = comment, UserName = username, EditedDate = DateTime.Now });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You can not remove comment  of an inactive post!");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user!");
            }
             
            RaiseEvents(new CommentRemovedEvent { Id = _id, CommentId = commentId  });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }

            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a post that was made by another user!");
            }

            RaiseEvents(new PostRemovedEvent { Id = _id});
        }


        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}
