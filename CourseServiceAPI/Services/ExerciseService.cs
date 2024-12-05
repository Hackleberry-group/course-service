using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using HackleberrySharedModels.Exceptions;
using HackleberrySharedModels.Requests;
using MassTransit;

namespace CourseServiceAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private readonly IPublishEndpoint _publishEndpoint;
        private const string TableName = EntityConstants.ExerciseTableName;
        private const string PartitionKey = EntityConstants.ExercisePartitionKey;

        public ExerciseService(ITableStorageQueryService tableStorageQueryService, ITableStorageCommandService tableStorageCommandService, IPublishEndpoint publishEndpoint)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<Exercise>> GetExercisesAsync()
        {
            return await _tableStorageQueryService.GetAllEntitiesAsync<Exercise>(TableName);
        }

        public async Task<Exercise> GetExerciseByIdAsync(Guid id)
        {
            var exercise = await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, id.ToString());
            if (exercise == null)
            {
                throw new NotFoundException();
            }
            return exercise;
        }

        public async Task<Exercise> PutExerciseByIdAsync(Guid id, Exercise exercise)
        {
            var existingExercise = await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, id.ToString());
            if (existingExercise == null)
            {
                throw new NotFoundException();
            }
            exercise.PartitionKey = PartitionKey;
            exercise.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, exercise);
            return exercise;
        }

        public async Task<Exercise> CreateExerciseAsync(Exercise exercise)
        {
            exercise.PartitionKey = PartitionKey;
            exercise.RowKey = Guid.NewGuid().ToString();
            await _tableStorageCommandService.AddEntityAsync(TableName, exercise);
            return exercise;
        }

        public async Task DeleteExerciseAsync(Guid id)
        {
            var exercise = await _tableStorageQueryService.GetEntityAsync<Exercise>(TableName, PartitionKey, id.ToString());
            if (exercise == null)
            {
                throw new NotFoundException();
            }
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
            await PublishExerciseDeletedEventAsync(id);
        }

        public async Task PublishExerciseDeletedEventAsync(Guid id)
        {
            var exerciseDelete = new ExerciseDeleted { ExerciseId = id };
            await _publishEndpoint.Publish(exerciseDelete);
        }
    }
}
