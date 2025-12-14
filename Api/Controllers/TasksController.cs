using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogger<TasksController> _logger;

        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
        {
            _logger.LogInformation("Task create Api Called");
            var taskId = await _mediator.Send(command);
            return Ok(new {Id=taskId});
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskCommand command)
        {
            _logger.LogInformation("Task update Api Called");
            if (id != command.Id)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return Ok();
        }
        [HttpPut("UpdateStatus/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusCommand command)
        {
            _logger.LogInformation("Task UpdateStatus Api Called");
            if (id != command.Id)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Task Delete Api Called");
            await _mediator.Send(new DeleteTaskCommand { Id = id });
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery { Id = id });
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTasksQuery query)
        {
            var tasks = await _mediator.Send(query);
            return Ok(tasks);
        }
    }
}
