using Application.Features.Teams.Commands;
using Application.Features.Teams.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamCommand command)
        {
            var teamId = await _mediator.Send(command);
            return Ok(new {Id=teamId});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTeamCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteTeamCommand { Id = id });
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var team = await _mediator.Send(new GetTeamByIdQuery { Id = id });
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllTeamsQuery query)
        {
            var teams = await _mediator.Send(query);
            return Ok(teams);
        }
    }
}
