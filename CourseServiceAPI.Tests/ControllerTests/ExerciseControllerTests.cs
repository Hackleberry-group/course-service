using CourseServiceAPI.Controllers;
using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CourseServiceAPI.Tests.ControllerTests
{
    [TestFixture]
    public class ExerciseControllerTests
    {
        private IExerciseService _exerciseService;
        private ExercisesController _exerciseController;

        [SetUp]
        public void SetUp()
        {
            _exerciseService = Substitute.For<IExerciseService>();
            _exerciseController = new ExercisesController(_exerciseService);
        }

        [Test]
        public async Task GetExercises_ShouldReturnAllExercises()
        {
            var exercises = new List<Exercise>
            {
                new() { RowKey = Guid.NewGuid().ToString(), Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() },
                new() { RowKey = Guid.NewGuid().ToString(), Order = 2, IsTopicExam = true, TopicId = Guid.NewGuid() }
            };
            _exerciseService.GetExercisesAsync().Returns(exercises);

            var result = await _exerciseController.GetExercises();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetExerciseById_ShouldReturnExercise()
        {
            var exerciseId = Guid.NewGuid();
            var exercise = new Exercise { RowKey = exerciseId.ToString(), Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() };
            _exerciseService.GetExerciseByIdAsync(exerciseId).Returns(exercise);

            var result = await _exerciseController.GetExerciseById(exerciseId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.InstanceOf<ExerciseResponseDTO>());
            });
        }

        [Test]
        public async Task CreateExercise_ShouldAddExercise()
        {
            var exerciseDto = new ExerciseRequestDTO { TopicId = Guid.NewGuid().ToString(), Order = 1, IsTopicExam = true };
            var exercise = Mapper.MapToExercise(exerciseDto);
            var createdExercise = exercise;
            createdExercise.RowKey = Guid.NewGuid().ToString();
            _exerciseService.CreateExerciseAsync(Arg.Any<Exercise>()).Returns(createdExercise);

            var result = await _exerciseController.CreateExercise(exerciseDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                var createdResult = result.Result as CreatedAtActionResult;
                Assert.That(createdResult.StatusCode, Is.EqualTo(201));
                Assert.That(createdResult.Value, Is.InstanceOf<ExerciseResponseDTO>());
            });
        }

        [Test]
        public async Task PutExerciseById_ShouldUpdateExercise()
        {
            var exerciseId = Guid.NewGuid();
            var exerciseDto = new ExerciseRequestDTO { TopicId = Guid.NewGuid().ToString(), Order = 1, IsTopicExam = true };
            var exercise = Mapper.MapToExercise(exerciseDto);
            exercise.RowKey = exerciseId.ToString();
            _exerciseService.PutExerciseByIdAsync(exerciseId, Arg.Any<Exercise>()).Returns(exercise);

            var result = await _exerciseController.PutExerciseById(exerciseId, exerciseDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.InstanceOf<ExerciseResponseDTO>());
            });
        }

        [Test]
        public async Task DeleteExercise_ShouldReturnNoContent()
        {
            var exerciseId = Guid.NewGuid();

            var result = await _exerciseController.DeleteExercise(exerciseId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                var noContentResult = result as NoContentResult;
                Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
            });
        }
    }
}
