using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
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
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            var response = Mapper.MapToExerciseResponseDto(exercise);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Exercise>> CreateExercise([FromBody] ExerciseRequestDTO exerciseDto)
        {
            var exercise = Mapper.MapToExercise(exerciseDto);
            exercise.Id = Guid.NewGuid();
            var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);
            var response = Mapper.MapToExerciseResponseDto(createdExercise);
            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Exercise>> PutExerciseById(Guid id, [FromBody] ExerciseRequestDTO exerciseDto)
        {
            var exercise = Mapper.MapToExercise(exerciseDto);
            exercise.Id = id;
            var updatedExercise = await _exerciseService.PutExerciseByIdAsync(id, exercise);
            var response = Mapper.MapToExerciseResponseDto(updatedExercise);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(Guid id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
    }
}
