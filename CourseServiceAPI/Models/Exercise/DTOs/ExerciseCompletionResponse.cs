namespace CourseServiceAPI.Models.Exercise.DTOs;

public class ExerciseCompletionResponse
{
    public Guid ExerciseId { get; set; }

    public int QuestionsTotal { get; set; }

    public int QuestionsCorrect { get; set; }

    public bool IsPassed { get; set; }

    public double StreakMultiplier { get; set; }

    public int XPEarned { get; set; }

    public int CurrentUserXP { get; set; }
}