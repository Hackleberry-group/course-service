namespace CourseServiceAPI.Models.Exercise;

public class ExerciseCompletion
{
    public Guid ExerciseId { get; set; }

    public List<List<AnsweredQuestion>> AnsweredQuestions { get; set; }

}