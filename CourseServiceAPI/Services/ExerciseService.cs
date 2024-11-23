using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;

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

        public async Task CompleteExerciseAsync(Guid exerciseId, List<List<AnsweredQuestion>> answeredQuestions)
        {
            // For now add a dummy completion with the exerciseId and answeredQuestions without any database logic
            var completion = new ExerciseCompletion { ExerciseId = exerciseId, AnsweredQuestions = answeredQuestions };

            foreach (var question in completion.AnsweredQuestions)
            {
                Console.WriteLine("Question:");
                foreach (var answeredQuestion in question)
                {
                    Console.WriteLine($"Answered question: {answeredQuestion.QuestionId}");
                    Console.WriteLine($"Answer: {answeredQuestion.AnswerId}");
                }
            }
        }

        public async Task DeleteExerciseAsync(Guid id)
        {
            await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
        }
    }
}
