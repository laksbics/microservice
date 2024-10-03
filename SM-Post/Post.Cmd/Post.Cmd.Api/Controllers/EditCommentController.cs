using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditCommentController : ControllerBase
    {
        private readonly ILogger<EditCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditCommentAsync(Guid id, EditCommentCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = $"Edit Comment Message completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, "Client made a bad request");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message

                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, "Could not retrive aggreage, client has passed incorrect id.");
                return BadRequest(new BaseResponse
                {
                    Message = ex.Message

                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to Edit Comment to a post!";
                _logger.Log(LogLevel.Error, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });

            }
        }
    }
}
