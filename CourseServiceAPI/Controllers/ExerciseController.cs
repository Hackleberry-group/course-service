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
        private readonly ILogger<ExerciseController> _logger;
        private readonly IExerciseService _exerciseService;

        public ExerciseController(ILogger<ExerciseController> logger, IExerciseService exerciseService)
        {
            _logger = logger;
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IEnumerable<ExerciseResponseDTO>> GetExercises()
        {
            var exercises = await _exerciseService.GetExercisesAsync();

            return exercises.Select(ExerciseMapper.MapToResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseResponseDTO>> GetExerciseById(Guid id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);

            return Ok(exercise);
        }

        [HttpPost]
        public async Task<ActionResult<Exercise>> CreateExercise([FromBody] ExerciseRequestDTO exerciseDto)
        {
            var exercise = ExerciseMapper.MapDtoToExercise(exerciseDto);

            var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Exercise>> PutExerciseById(Guid id, [FromBody] ExerciseRequestDTO exerciseDto)
        {
            _logger.LogInformation("Updating exercise with ID: {Id}", id);

            // Map DTO to model
            var exercise = ExerciseMapper.MapDtoToExercise(exerciseDto);

            var updatedExercise = await _exerciseService.PutExerciseByIdAsync(id, exercise);
            return Ok(updatedExercise);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteExercise(Guid id, [FromBody] List<List<AnsweredQuestion>> answeredQuestions)
        {
            _logger.LogInformation("Completing exercise with ID: {Id}", id);
            await _exerciseService.CompleteExerciseAsync(id, answeredQuestions);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(Guid id)
        {
            _logger.LogInformation("Deleting exercise with ID: {Id}", id);
            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
    }
}
