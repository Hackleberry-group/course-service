namespace CourseServiceAPI.Models.Exercise;

public class Exercise
{
    public Guid Id { get; set; }

    public bool IsTopicExam { get; set; }

    // Placeholder for topicId
    public Guid TopicId { get; set; }

    // A list of questions which will be retrieved using QuestionService microservice with an HTTP request
    //public List<Question> Questions { get; set; }
}