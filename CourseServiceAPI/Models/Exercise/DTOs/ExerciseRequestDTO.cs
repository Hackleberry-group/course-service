namespace CourseServiceAPI.Models.Exercise.DTOs;

public class ExerciseRequestDTO
{
    public int Order { get; set; }

    public string TopicId { get; set; }

    public bool? IsTopicExam { get; set; }
}
