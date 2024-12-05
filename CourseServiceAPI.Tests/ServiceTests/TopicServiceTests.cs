using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Services;
using NSubstitute;
using CourseServiceAPI.Interfaces;

namespace CourseServiceAPI.Tests.CourseServiceTests;

[TestFixture]
public class TopicServiceTests
{
    private ITableStorageQueryService _tableStorageQueryService;
    private ITableStorageCommandService _tableStorageCommandService;
    private IExerciseService _exerciseService;
    private TopicService _topicService;

    [SetUp]
    public void SetUp()
    {
        _tableStorageQueryService = Substitute.For<ITableStorageQueryService>();
        _tableStorageCommandService = Substitute.For<ITableStorageCommandService>();
        _exerciseService = Substitute.For<IExerciseService>();
        _topicService = new TopicService(_tableStorageQueryService, _tableStorageCommandService, _exerciseService);
    }

    [Test]
    public async Task GetTopicsAsync_ShouldReturnAllTopics()
    {
        var topics = new List<Topic>
        {
            new() { RowKey = Guid.NewGuid().ToString(), Name = "Topic 1", ModuleId = Guid.NewGuid(), Order = 1 },
            new() { RowKey = Guid.NewGuid().ToString(), Name = "Topic 2", ModuleId = Guid.NewGuid(), Order = 2 }
        };
        _tableStorageQueryService.GetAllEntitiesAsync<Topic>(Arg.Any<string>()).Returns(topics);

        var result = await _topicService.GetTopicsAsync();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Is.EqualTo(topics));
        });
    }

    [Test]
    public async Task CreateTopicAsync_ShouldAddTopic()
    {
        var topic = new Topic { RowKey = Guid.NewGuid().ToString(), Name = "New Topic", ModuleId = Guid.NewGuid(), Order = 1 };

        var result = await _topicService.CreateTopicAsync(topic);

        await Assert.MultipleAsync(async () =>
        {
            await _tableStorageCommandService.Received(1).AddEntityAsync(Arg.Any<string>(), topic);
            Assert.That(result, Is.EqualTo(topic));
        });
    }

    [Test]
    public async Task GetTopicByIdAsync_ShouldReturnTopicWithExercises()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic { RowKey = topicId.ToString(), Name = "Topic", ModuleId = Guid.NewGuid(), Order = 1 };
        var exercises = new List<Exercise>
        {
            new() { RowKey = Guid.NewGuid().ToString(), Order = 1, IsTopicExam = false, TopicId = topicId },
            new() { RowKey = Guid.NewGuid().ToString(), Order = 2, IsTopicExam = true, TopicId = topicId }
        };
        _tableStorageQueryService.GetEntityAsync<Topic>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(topic);
        _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(Arg.Any<string>(), Arg.Any<string>())
            .Returns(exercises);

        var result = await _topicService.GetTopicByIdAsync(topicId);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RowKey, Is.EqualTo(topicId.ToString()));
            Assert.That(result.Exercises.Count, Is.EqualTo(2));
            Assert.That(result.Exercises, Is.EqualTo(exercises));
        });
    }

    [Test]
    public async Task PutTopicByIdAsync_ShouldUpdateTopic()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic { RowKey = topicId.ToString(), Name = "Updated Topic", ModuleId = Guid.NewGuid(), Order = 1 };

        _tableStorageQueryService.GetEntityAsync<Topic>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(topic);

        var result = await _topicService.PutTopicByIdAsync(topicId, topic);

        await Assert.MultipleAsync(async () =>
        {
            await _tableStorageCommandService.Received(1).UpdateEntityAsync(Arg.Any<string>(), topic);
            Assert.That(result, Is.EqualTo(topic));
        });
    }

    [Test]
    public async Task DeleteTopicAsync_ShouldDeleteTopic()
    {
        var topicId = Guid.NewGuid();
        var topic = new Topic { RowKey = topicId.ToString(), Name = "Topic", ModuleId = Guid.NewGuid(), Order = 1 };

        _tableStorageQueryService.GetEntityAsync<Topic>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(topic);

        await _topicService.DeleteTopicAsync(topicId);

        await _tableStorageQueryService.Received(1)
            .DeleteEntityAsync(Arg.Any<string>(), Arg.Any<string>(), topicId.ToString());
    }
}