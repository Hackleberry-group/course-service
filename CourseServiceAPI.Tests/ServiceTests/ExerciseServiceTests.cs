using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Services;
using NSubstitute;

namespace CourseServiceAPI.Tests.CourseServiceTests;

[TestFixture]
public class ExerciseServiceTests
{
    private ITableStorageQueryService _tableStorageQueryService;
    private ITableStorageCommandService _tableStorageCommandService;
    private ExerciseService _exerciseService;

    [SetUp]
    public void SetUp()
    {
        _tableStorageQueryService = Substitute.For<ITableStorageQueryService>();
        _tableStorageCommandService = Substitute.For<ITableStorageCommandService>();
        _exerciseService = new ExerciseService(_tableStorageQueryService, _tableStorageCommandService);
    }

    [Test]
    public async Task GetExercisesAsync_ShouldReturnAllExercises()
    {
        var exercises = new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), Order = 2, IsTopicExam = true, TopicId = Guid.NewGuid() }
        };
        _tableStorageQueryService.GetAllEntitiesAsync<Exercise>(Arg.Any<string>()).Returns(exercises);

        var result = await _exerciseService.GetExercisesAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.EqualTo(exercises));
        });
    }

    [Test]
    public async Task GetExerciseByIdAsync_ShouldReturnExercise()
    {
        var exerciseId = Guid.NewGuid();
        var exercise = new Exercise { Id = exerciseId, Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() };
        _tableStorageQueryService.GetEntityAsync<Exercise>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(exercise);

        var result = await _exerciseService.GetExerciseByIdAsync(exerciseId);

        Assert.That(result, Is.EqualTo(exercise));
    }

    [Test]
    public async Task CreateExerciseAsync_ShouldAddExercise()
    {
        var exercise = new Exercise { Id = Guid.NewGuid(), Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() };

        var result = await _exerciseService.CreateExerciseAsync(exercise);

        await Assert.MultipleAsync(async () =>
        {
            await _tableStorageCommandService.Received(1).AddEntityAsync(Arg.Any<string>(), exercise);
            Assert.That(result, Is.EqualTo(exercise));
        });
    }

    [Test]
    public async Task PutExerciseByIdAsync_ShouldUpdateExercise()
    {
        var exerciseId = Guid.NewGuid();
        var exercise = new Exercise { Id = exerciseId, Order = 1, IsTopicExam = false, TopicId = Guid.NewGuid() };

        var result = await _exerciseService.PutExerciseByIdAsync(exerciseId, exercise);

        await Assert.MultipleAsync(async () =>
        {
            await _tableStorageCommandService.Received(1).UpdateEntityAsync(Arg.Any<string>(), exercise);
            Assert.That(result, Is.EqualTo(exercise));
        });
    }

    [Test]
    public async Task DeleteExerciseAsync_ShouldDeleteExercise()
    {
        var exerciseId = Guid.NewGuid();

        await _exerciseService.DeleteExerciseAsync(exerciseId);

        await _tableStorageQueryService.Received(1)
            .DeleteEntityAsync(Arg.Any<string>(), Arg.Any<string>(), exerciseId.ToString());
    }
}