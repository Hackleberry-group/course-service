namespace CourseServiceAPI.Models.Exercise.DTOs;

public class ExerciseDto
{
    public string Id { get; set; }

    public string TopicId { get; set; }

    public bool? IsTopicExam { get; set; }
}
