using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Exercise.DTOs;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CourseServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
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

    [HttpGet("{id}/exercises")]
    public async Task<IEnumerable<ExerciseResponseDTO>> GetExercisesByTopicId(Guid id)
    {
        var exercises = await _topicService.GetExercisesByTopicIdAsync(id);
        return exercises.Select(Mapper.MapToExerciseResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicRequestDTO topicDto)
    {
        var topic = Mapper.MapToTopic(topicDto);
        topic.Id = Guid.NewGuid();
        var createdTopic = await _topicService.CreateTopicAsync(topic);
        var createdTopicDto = Mapper.MapToTopicResponseDto(createdTopic);
        return CreatedAtAction(nameof(GetTopicById), new { id = createdTopicDto.Id }, createdTopicDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Topic>> PutTopicById(Guid id, [FromBody] TopicRequestDTO topicDto)
    {
        var topic = Mapper.MapToTopic(topicDto);
        topic.Id = id;
        var updatedTopic = await _topicService.PutTopicByIdAsync(id, topic);
        var response = Mapper.MapToTopicResponseDto(updatedTopic);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTopic(Guid id)
    {
        await _topicService.DeleteTopicAsync(id);
        return NoContent();
    }
}