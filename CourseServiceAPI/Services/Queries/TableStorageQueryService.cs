using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Interfaces.Queries;

namespace CourseServiceAPI.Services.Queries;

public class TableStorageQueryService : ITableStorageQueryService
{
    private readonly TableServiceClient _tableServiceClient;

    public TableStorageQueryService(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
    }

    public async Task<IEnumerable<T>> GetAllEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        var entities = new List<T>();

        await foreach (var entity in tableClient.QueryAsync<T>())
        {
            entities.Add(entity);
        }

        return entities;
    }

    public async Task<T> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey)
        where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        try
        {
            return await tableClient.GetEntityAsync<T>(partitionKey, rowKey);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task DeleteEntityAsync(string tableName, string partitionKey, string rowKey)
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }
}