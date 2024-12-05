namespace CourseServiceAPI.Models.Topic.DTOs;

public class TopicResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid ModuleId { get; set; }
    public int Order { get; set; }
    public List<Guid> ExerciseIds { get; set; } = new List<Guid>();
}