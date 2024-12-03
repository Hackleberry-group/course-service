namespace CourseServiceAPI.Models.Topic.DTOs;

public class TopicRequestDTO
{
    public string Name { get; set; }

    public string ModuleId { get; set; }

    public int Order { get; set; }
}