using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }


        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        {
            try
            {
                var id = Guid.NewGuid();
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Id = id,
                    Message = $"New Post Createion request completed successfully"
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
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create new post";
                _logger.Log(LogLevel.Error, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });

            }
        }
    }
}
