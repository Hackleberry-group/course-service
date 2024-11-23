using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;

namespace CourseServiceAPI.Helpers;

public static class ExerciseMapper
{
    public static ExerciseResponseDTO MapToResponseDto(Exercise exercise)
    {
        return new ExerciseResponseDTO
        {
            Id = exercise.Id,
            Order = exercise.Order,
            IsTopicExam = exercise.IsTopicExam,
            TopicId = exercise.TopicId,
            Questions = exercise.Questions
        };
    }

    public static Exercise MapDtoToExercise(ExerciseRequestDTO exerciseDto)
    {
        return new Exercise
        {
            Order = exerciseDto.Order,
            IsTopicExam = exerciseDto.IsTopicExam != null && exerciseDto.IsTopicExam.Value,
            TopicId = Guid.Parse(exerciseDto.TopicId)
        };
    }
}