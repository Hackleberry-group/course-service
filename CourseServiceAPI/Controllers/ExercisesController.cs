using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using HackleberrySharedModels.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IEnumerable<ExerciseResponseDTO>> GetExercises()
        {
            var exercises = await _exerciseService.GetExercisesAsync();
            return exercises.Select(Mapper.MapToExerciseResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseResponseDTO>> GetExerciseById(Guid id)
        {
            try
            {
                var exercise = await _exerciseService.GetExerciseByIdAsync(id);
                if (exercise == null)
                {
                    throw new NotFoundException();
                }
                var response = Mapper.MapToExerciseResponseDto(exercise);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw new InternalErrorException();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Exercise>> CreateExercise([FromBody] ExerciseRequestDTO exerciseDto)
        {
            try
            {
                var exercise = Mapper.MapToExercise(exerciseDto);
                exercise.RowKey = Guid.NewGuid().ToString();
                var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);
                var response = Mapper.MapToExerciseResponseDto(createdExercise);
                return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.RowKey }, response);
            }
            catch (AlreadyExistsException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                throw new InternalErrorException();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Exercise>> PutExerciseById(Guid id, [FromBody] ExerciseRequestDTO exerciseDto)
        {
            try
            {
                var exercise = Mapper.MapToExercise(exerciseDto);
                exercise.RowKey = id.ToString();
                var updatedExercise = await _exerciseService.PutExerciseByIdAsync(id, exercise);
                var response = Mapper.MapToExerciseResponseDto(updatedExercise);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw new InternalErrorException();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(Guid id)
        {
            try
            {
                await _exerciseService.DeleteExerciseAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                throw new InternalErrorException();
            }
        }
    }
}
