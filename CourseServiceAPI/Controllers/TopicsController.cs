using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ILogger<TopicsController> _logger;
    private readonly ITopicService _topicService;

    public TopicsController(ILogger<TopicsController> logger, ITopicService topicService)
    {
        _logger = logger;
        _topicService = topicService;
    }

    [HttpGet]
    public async Task<IEnumerable<TopicResponseDto>> GetTopics()
    {
        var topics = await _topicService.GetTopicsAsync();

        return topics.Select(Mapper.MapToTopicResponseDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TopicResponseDto>> GetTopicById(Guid id)
    {
        var topic = await _topicService.GetTopicByIdAsync(id);

        var response = Mapper.MapToTopicResponseDto(topic);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicRequestDTO topicDto)
    {
        var topic = Mapper.MapToTopic(topicDto);
        var createdTopic = await _topicService.CreateTopicAsync(topic);
        var createdTopicDto = Mapper.MapToTopicResponseDto(createdTopic);
        return CreatedAtAction(nameof(GetTopicById), new { id = createdTopicDto.Id }, createdTopicDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Topic>> PutTopicById(Guid id, [FromBody] TopicRequestDTO topicDto)
    {
        _logger.LogInformation("Updating topic with ID: {Id}", id);

        var topic = Mapper.MapToTopic(topicDto);
        topic.RowKey = id.ToString();
        topic.PartitionKey = EntityConstants.TopicPartitionKey;

        var updatedTopic = await _topicService.PutTopicByIdAsync(id, topic);

        var response = Mapper.MapToTopicResponseDto(updatedTopic);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTopic(Guid id)
    {
        _logger.LogInformation("Deleting topic with ID: {Id}", id);
        await _topicService.DeleteTopicAsync(id);
        return NoContent();
    }
}