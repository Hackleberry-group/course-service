using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;
using HackleberrySharedModels.Exceptions;
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
        try
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
            {
                throw new NotFoundException();
            }
            var response = Mapper.MapToTopicResponseDto(topic);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            throw new InternalErrorException();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicRequestDTO topicDto)
    {
        try
        {
            var topic = Mapper.MapToTopic(topicDto);
            topic.RowKey = Guid.NewGuid().ToString();
            var createdTopic = await _topicService.CreateTopicAsync(topic);
            var createdTopicDto = Mapper.MapToTopicResponseDto(createdTopic);
            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopicDto.Id }, createdTopicDto);
        }
        catch (AlreadyExistsException)
        {
            return Conflict();
        }
        catch (Exception)
        {
            throw new InternalErrorException();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Topic>> PutTopicById(Guid id, [FromBody] TopicRequestDTO topicDto)
    {
        try
        {
            var topic = Mapper.MapToTopic(topicDto);
            topic.RowKey = id.ToString();
            var updatedTopic = await _topicService.PutTopicByIdAsync(id, topic);
            var response = Mapper.MapToTopicResponseDto(updatedTopic);
            return Ok(response);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            throw new InternalErrorException();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTopic(Guid id)
    {
        try
        {
            await _topicService.DeleteTopicAsync(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            throw new InternalErrorException();
        }
    }
}
