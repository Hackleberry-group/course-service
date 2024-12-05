using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;
using CourseServiceAPI.Models.Topic;
using CourseServiceAPI.Models.Topic.DTOs;
using CourseServiceAPI.Models.Module;
using CourseServiceAPI.Models.Module.DTOs;
using System.Net.NetworkInformation;
using CourseServiceAPI.Models.Course;
using CourseServiceAPI.Models.Course.DTOs;

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
            ExerciseIds = topic.ExerciseIds
        };
    }

    public static Module MapToModule(ModuleRequestDTO dto)
    {
        return new Module
        {
            Name = dto.Name,
            Order = dto.Order,
            CourseId = dto.CourseId,
        };
    }

    public static ModuleResponseDTO MapToModuleResponseDto(Module module)
    {
        return new ModuleResponseDTO
        {
            Id = Guid.Parse(module.RowKey),
            Name = module.Name,
            Order = module.Order,
            CourseId = module.CourseId
        };
    }

    public static Course MapToCourse(CourseRequestDTO dto)
    {
        return new Course
        {
            Name = dto.Name,
            ProgrammingLanguage = dto.ProgrammingLanguage,
            TeacherId = dto.TeacherId.ToString()
        };
    }

    public static CourseResponseDTO MapToCourseResponseDto(Course course)
    {
        return new CourseResponseDTO
        {
            Id = Guid.Parse(course.RowKey),
            Name = course.Name,
            ProgrammingLanguage = course.ProgrammingLanguage,
            TeacherId = course.TeacherId
        };
    }

}