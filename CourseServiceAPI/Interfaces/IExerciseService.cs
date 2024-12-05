using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;

namespace CourseServiceAPI.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<Exercise>> GetExercisesAsync();
        Task<Exercise> CreateExerciseAsync(Exercise exercise);

        Task<Exercise> GetExerciseByIdAsync(Guid id);
        Task<Exercise> PutExerciseByIdAsync(Guid id, Exercise exercise);
        Task DeleteExerciseAsync(Guid id);
        Task<IEnumerable<Exercise>> GetExercisesByTopicIdAsync(Guid topicId);
        Task PublishExerciseDeletedEventAsync(Guid id);
    }
}