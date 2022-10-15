using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/[controller]")] //api/commands
    [ApiController]
    public class CommandsController : ControllerBase
    {

        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/commands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Command>>> GetAllCommands()
        {
            var commands = await _repository.GetAllCommandsAsync();
            if (commands.Any())
            {
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
            }
            return NotFound("No commands were found");
        }

        // GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public async Task<ActionResult<Command>> GetCommandById(int id)
        {
            var command = await _repository.GetCommandByIdAsync(id);
            if (command != null)
                return Ok(_mapper.Map<CommandReadDto>(command));
            return NotFound($"No command with the id: {id} was found.");
        }

        [HttpPost()]
        public async Task<ActionResult<CommandReadDto>> CreateCommand(CommandCreateDto cmd)
        {
            try
            {
                var command = _mapper.Map<Command>(cmd);
                _repository.CreateCommand(command);
                if (await _repository.SaveChangesAsync())
                {
                    var dto = _mapper.Map<CommandReadDto>(command);
                    return CreatedAtRoute(nameof(GetCommandById), new { Id = dto.Id }, dto);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCommand(int id, CommandUpdateDto cmd)
        {
            try
            {
                var existingCmd = await _repository.GetCommandByIdAsync(id);
                if (existingCmd is null)
                {
                    return NotFound();
                }

                _mapper.Map(cmd, existingCmd);

                var isUpdateSuccesful = await _repository.SaveChangesAsync();
                if (isUpdateSuccesful)
                {
                    return NoContent();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var existingCmd = await _repository.GetCommandByIdAsync(id);
            if (existingCmd is null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(existingCmd);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, existingCmd);

            var isUpdateSuccesful = await _repository.SaveChangesAsync();
            if (isUpdateSuccesful)
            {
                return NoContent();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingCmd = await _repository.GetCommandByIdAsync(id);
            if (existingCmd is null)
            {
                return NotFound();
            }

            _repository.Delete(existingCmd);
            var isUpdateSuccesful = await _repository.SaveChangesAsync();
            if (isUpdateSuccesful)
            {
                return NoContent();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}