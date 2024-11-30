using System.Text.Json;
using CourseServiceAPI.Helpers;
using CourseServiceAPI.Interfaces;
using CourseServiceAPI.Interfaces.Commands;
using CourseServiceAPI.Interfaces.Queries;
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
        return await _tableStorageQueryService.GetAllEntitiesAsync<Topic>(TableName);
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        return await _tableStorageCommandService.AddEntityAsync(TableName, topic);
    }


    public async Task<Topic> GetTopicByIdAsync(Guid id)
    {
        return await _tableStorageQueryService.GetEntityAsync<Topic>(TableName, PartitionKey, id.ToString());
    }

    public async Task<Topic> PutTopicByIdAsync(Guid id, Topic topic)
    {
        topic.PartitionKey = PartitionKey;
        topic.RowKey = id.ToString();
        
        return await _tableStorageCommandService.UpdateEntityAsync(TableName, topic);
    }

    public async Task DeleteTopicAsync(Guid id)
    {
        await _tableStorageQueryService.DeleteEntityAsync(TableName, PartitionKey, id.ToString());
    }
}