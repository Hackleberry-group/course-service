using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
using CourseServiceAPI.Models.Exercise;
using CourseServiceAPI.Models.Topic;

namespace CourseServiceAPI.Services;

public class TopicService : ITopicService
{
    private readonly ITableStorageQueryService _tableStorageQueryService;
    private readonly ITableStorageCommandService _tableStorageCommandService;
    private const string TableName = EntityConstants.TopicTableName;
    private const string PartitionKey = EntityConstants.TopicPartitionKey;

    public TopicService(ITableStorageQueryService tableStorageQueryService,
        ITableStorageCommandService tableStorageCommandService)
    {
        _tableStorageQueryService = tableStorageQueryService;
        _tableStorageCommandService = tableStorageCommandService;
    }

    public async Task<IEnumerable<Topic>> GetTopicsAsync()
    {
        var topics = _tableStorageQueryService.GetAllEntitiesAsync<Topic>(TableName);

        foreach (var topic in topics.Result)
        {
            var filter = topic.Id.ToFilter<Exercise>("TopicId");
            var exercises =
                await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName,
                    filter);
            topic.Exercises = exercises.ToList();
        }

        return topics.Result;
    }

    public async Task<IEnumerable<Exercise>> GetExercisesByTopicIdAsync(Guid topicId)
    {
        var filter = topicId.ToFilter<Exercise>("TopicId");
        var exercises = await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName, filter);
        return exercises;
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        topic.PartitionKey = PartitionKey;
        topic.Id = Guid.NewGuid();
        await _tableStorageCommandService.AddEntityAsync(TableName, topic);
        return topic;
    }


    public async Task<Topic> GetTopicByIdAsync(Guid id)
    {
        var topic = await _tableStorageQueryService.GetEntityAsync<Topic>(TableName, PartitionKey, id.ToString());

        var filter = id.ToFilter<Exercise>("TopicId");
        var exercises =
            await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName,
                filter);

        topic.Exercises = exercises.ToList();

        return topic;
    }


    public async Task<Topic> PutTopicByIdAsync(Guid id, Topic topic)
    {
        topic.PartitionKey = PartitionKey;
        topic.RowKey = id.ToString();
        await _tableStorageCommandService.UpdateEntityAsync(TableName, topic);
        return topic;
    }

    public async Task DeleteTopicAsync(Guid id)
    {
        var filter = id.ToFilter<Exercise>("TopicId");
        var exercises = await _tableStorageQueryService.GetEntitiesByFilterAsync<Exercise>(EntityConstants.ExerciseTableName, filter);

        foreach (var exercise in exercises)
        {
            await _tableStorageQueryService.DeleteEntityAsync(EntityConstants.ExerciseTableName, exercise.PartitionKey, exercise.RowKey);
        }

        await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
    }
}