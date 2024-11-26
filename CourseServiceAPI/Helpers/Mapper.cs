using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;

namespace CourseServiceAPI.Helpers;

public static class Mapper
{
    public static ExerciseResponseDTO MapToExerciseResponseDto(Exercise exercise)
    {
        return new ExerciseResponseDTO
        {
            Id = Guid.Parse(exercise.RowKey),
            Order = exercise.Order,
            IsTopicExam = exercise.IsTopicExam,
            TopicId = exercise.TopicId,
            Questions = exercise.Questions
        };
    }

    public static Exercise MapToExercise(ExerciseRequestDTO exerciseDto)
    {
        return new Exercise
        {
            Order = exerciseDto.Order,
            IsTopicExam = exerciseDto.IsTopicExam != null && exerciseDto.IsTopicExam.Value,
            TopicId = Guid.Parse(exerciseDto.TopicId)
        };
    }

    public static Topic MapToTopic(TopicRequestDTO dto)
    {
        return new Topic
        {
            Name = dto.Name,
            ModuleId = Guid.Parse(dto.ModuleId),
            Order = dto.Order,
        };
    }

    public static TopicResponseDto MapToTopicResponseDto(Topic topic)
    {
        return new TopicResponseDto
        {
            Id = Guid.Parse(topic.RowKey),
            Name = topic.Name,
            ModuleId = topic.ModuleId,
            Order = topic.Order,
        };
    }
}