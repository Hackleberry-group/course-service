using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Topic;
using HackleberrySharedModels.Exceptions;

namespace CourseServiceAPI.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITableStorageQueryService _tableStorageQueryService;
        private readonly ITableStorageCommandService _tableStorageCommandService;
        private readonly IExerciseService _exerciseService;
        private const string TableName = EntityConstants.TopicTableName;
        private const string PartitionKey = EntityConstants.TopicPartitionKey;

        public TopicService(ITableStorageQueryService tableStorageQueryService,
            ITableStorageCommandService tableStorageCommandService,
            IExerciseService exerciseService)
        {
            _tableStorageQueryService = tableStorageQueryService;
            _tableStorageCommandService = tableStorageCommandService;
            _exerciseService = exerciseService;
        }

        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
            var topics = await _tableStorageQueryService.GetAllEntitiesAsync<Topic>(TableName);

            foreach (var topic in topics)
            {
                var filter = Guid.Parse(topic.RowKey).ToFilter<Exercise>("TopicId");
                var exercises =
                    await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName,
                        filter);
                topic.ExerciseIds = exercises.Select(e => Guid.Parse(e.RowKey)).ToList();
            }
            return topics;
        }
        public async Task<Topic> GetTopicByIdAsync(Guid id)
        {
            var topic = await _tableStorageQueryService.GetEntityAsync<Topic>(TableName, PartitionKey, id.ToString());
            if (topic == null)
            {
                throw new NotFoundException();
            }

            var filter = id.ToFilter<Exercise>("TopicId");
            var exercises =
                await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName,
                    filter);

            topic.ExerciseIds = exercises.Select(e => Guid.Parse(e.RowKey)).ToList();

            return topic;
        }

        public async Task<Topic> CreateTopicAsync(Topic topic)
        {
            topic.PartitionKey = PartitionKey;
            topic.RowKey = Guid.NewGuid().ToString();
            await _tableStorageCommandService.AddEntityAsync(TableName, topic);
            return topic;
        }

        public async Task<IEnumerable<Topic>> GetTopicsByModuleIdAsync(Guid id)
        {
            var filter = id.ToFilter<Topic>("ModuleId");
            return await _tableStorageQueryService.GetEntitiesByFilterAsync<Topic>(TableName, filter);
        }

        public async Task<Topic> PutTopicByIdAsync(Guid id, Topic topic)
        {
            var existingTopic = await _tableStorageQueryService.GetEntityAsync<Topic>(TableName, PartitionKey, id.ToString());
            if (existingTopic == null)
            {
                throw new NotFoundException();
            }

            topic.PartitionKey = PartitionKey;
            topic.RowKey = id.ToString();
            await _tableStorageCommandService.UpdateEntityAsync(TableName, topic);
            return topic;
        }

        public async Task DeleteTopicAsync(Guid id)
        {
            var topic = await _tableStorageQueryService.GetEntityAsync<Topic>(TableName, PartitionKey, id.ToString());
            if (topic == null)
            {
                throw new NotFoundException();
            }

            var filter = id.ToFilter<Exercise>("TopicId");
            var exercises = await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName, filter);

            var deletedExercises = new List<Exercise>();

            try
            {
                foreach (var exercise in exercises)
                {
                    await _exerciseService.DeleteExerciseAsync(Guid.Parse(exercise.RowKey));
                    deletedExercises.Add(exercise);
                }

                await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
            }
            catch (Exception ex)
            {
                foreach (var exercise in deletedExercises)
                {
                    await _tableStorageCommandService.AddEntityAsync(EntityConstants.ExerciseTableName, exercise);
                }

                throw new InternalErrorException();
            }
        }

    }
}