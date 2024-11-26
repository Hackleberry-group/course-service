using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly ILogger<ExercisesController> _logger;
        private readonly IExerciseService _exerciseService;

        public ExercisesController(ILogger<ExercisesController> logger, IExerciseService exerciseService)
        {
            _logger = logger;
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

            var createdExercise = await _exerciseService.CreateExerciseAsync(exercise);

            var response = Mapper.MapToExerciseResponseDto(createdExercise);

            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.RowKey }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Exercise>> PutExerciseById(Guid id, [FromBody] ExerciseRequestDTO exerciseDto)
        {
            _logger.LogInformation("Updating exercise with ID: {Id}", id);

            var exercise = Mapper.MapToExercise(exerciseDto);
            exercise.RowKey = id.ToString();
            exercise.PartitionKey = EntityConstants.ExercisePartitionKey;

            var updatedExercise = await _exerciseService.PutExerciseByIdAsync(id, exercise);

            var response = Mapper.MapToExerciseResponseDto(updatedExercise);

            return Ok(response);
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
            await _exerciseService.DeleteExerciseAsync(id);
            _logger.LogInformation("Deleted exercise with ID: {Id}", id);
            return NoContent();
        }
    }
}
