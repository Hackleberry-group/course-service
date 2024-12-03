using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Exercise.DTOs;

namespace CourseServiceAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private const string TableName = EntityConstants.ExerciseTableName;
        private const string PartitionKey = EntityConstants.ExercisePartitionKey;

        public ExerciseService(ITableStorageQueryService tableStorageQueryService, ITableStorageCommandService tableStorageCommandService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
        }

        public async Task<IEnumerable<Exercise>> GetExercisesAsync()
        {
            return await _tableStorageQueryService.GetAllEntitiesAsync<Exercise>(TableName);
        }

        public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        {
            return await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, id.ToString());
        }

        public async Task<Exercise> PutExerciseByIdAsync(Guid id, Exercise exercise)
        {
            exercise.PartitionKey = PartitionKey;
            exercise.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, exercise);
            return exercise;
        }

        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            await _tableStorageCommandService.AddEntityAsync(TableName, exercise);
            return exercise;
        }
        
        public async Task DeleteExerciseAsync(Guid id)
        {
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
        }
    }
}
