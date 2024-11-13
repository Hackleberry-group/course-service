using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers;

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
    public IEnumerable<Exercise> GetExercises()
    {
        return _exerciseService.GetExercises();
    }

    [HttpGet("{id}")]
    public ActionResult<Exercise> GetExerciseById(Guid id)
    {
        var exercise = _exerciseService.GetExerciseById(id);
        if (exercise == null)
        {
            return NotFound();
        }
        return exercise;
    }

    [HttpPost]
    public ActionResult<Exercise> CreateExercise([FromBody] ExerciseDto exerciseDto)
    {
        _logger.LogInformation("Creating exercise with ID: {Id}", exerciseDto.Id);

        // Map DTO to model
        var exercise = new Exercise
        {
            Id = Guid.Parse(exerciseDto.Id),
            TopicId = Guid.Parse(exerciseDto.TopicId),
            IsTopicExam = exerciseDto.IsTopicExam.Value
        };

        var createdExercise = _exerciseService.CreateExercise(exercise);
        return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, createdExercise);
    }

    [HttpPut("{id}")]
    public ActionResult<Exercise> PutExerciseById(Guid id, [FromBody] ExerciseDto exerciseDto)
    {
        _logger.LogInformation("Updating exercise with ID: {Id}", id);

        // Map DTO to model
        var exercise = new Exercise
        {
            Id = id,
            TopicId = Guid.Parse(exerciseDto.TopicId),
            IsTopicExam = exerciseDto.IsTopicExam.Value
        };

        var updatedExercise = _exerciseService.PutExerciseById(id, exercise);
        return Ok(updatedExercise);
    }

    [HttpPost("{id}/complete")]
    public IActionResult CompleteExercise(Guid id, [FromBody] List<List<AnsweredQuestion>> answeredQuestions)
    {
        _logger.LogInformation("Completing exercise with ID: {Id}", id);
        _exerciseService.CompleteExercise(id, answeredQuestions);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExercise(Guid id)
    {
        _logger.LogInformation("Deleting exercise with ID: {Id}", id);
        _exerciseService.DeleteExercise(id);
        return NoContent();
    }
}
