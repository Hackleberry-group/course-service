using CourseServiceAPI.Controllers;
using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CourseServiceAPI.Tests.ControllerTests
{
    [TestFixture]
    public class TopicControllerTests
    {
        private ITopicService _topicService;
        private TopicController _topicController;

        [SetUp]
        public void SetUp()
        {
            _topicService = Substitute.For<ITopicService>();
            _topicController = new TopicController(_topicService);
        }

        [Test]
        public async Task GetTopics_ShouldReturnAllTopics()
        {
            var topics = new List<Topic>
            {
                new() { RowKey = Guid.NewGuid().ToString(), Name = "Topic 1" },
                new() { RowKey = Guid.NewGuid().ToString(), Name = "Topic 2" }
            };
            _topicService.GetTopicsAsync().Returns(topics);

            var result = await _topicController.GetTopics();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task GetTopicById_ShouldReturnTopic()
        {
            var topicId = Guid.NewGuid();
            var topic = new Topic { RowKey = topicId.ToString(), Name = "Topic 1" };
            _topicService.GetTopicByIdAsync(topicId).Returns(topic);

            var result = await _topicController.GetTopicById(topicId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.InstanceOf<TopicResponseDto>());
            });
        }

        [Test]
        public async Task CreateTopic_ShouldAddTopic()
        {
            var topicDto = new TopicRequestDTO { ModuleId = Guid.NewGuid().ToString(), Order = 1, Name = "Topic" };
            var topic = Mapper.MapToTopic(topicDto);
            var createdTopic = topic;
            createdTopic.RowKey = Guid.NewGuid().ToString();
            _topicService.CreateTopicAsync(Arg.Any<Topic>()).Returns(createdTopic);

            var result = await _topicController.CreateTopic(topicDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                var createdResult = result.Result as CreatedAtActionResult;
                Assert.That(createdResult.StatusCode, Is.EqualTo(201));
                Assert.That(createdResult.Value, Is.InstanceOf<TopicResponseDto>());
            });
        }

        [Test]
        public async Task PutTopicById_ShouldUpdateTopic()
        {
            var topicId = Guid.NewGuid();
            var topicDto = new TopicRequestDTO { ModuleId = Guid.NewGuid().ToString(), Order = 1, Name = "Updated Topic" };
            var topic = Mapper.MapToTopic(topicDto);
            topic.RowKey = topicId.ToString();
            _topicService.PutTopicByIdAsync(topicId, Arg.Any<Topic>()).Returns(topic);

            var result = await _topicController.PutTopicById(topicId, topicDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.InstanceOf<TopicResponseDto>());
            });
        }

        [Test]
        public async Task DeleteTopic_ShouldReturnNoContent()
        {
            var topicId = Guid.NewGuid();

            var result = await _topicController.DeleteTopic(topicId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NoContentResult>());
                var noContentResult = result as NoContentResult;
                Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
            });
        }
    }
}
