using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Interfaces.Commands;

namespace CourseServiceAPI.Services.Commands;

public class TableStorageCommandService : ITableStorageCommandService
{
    private readonly TableServiceClient _tableServiceClient;
    private readonly ILogger<TableStorageCommandService> _logger;

    public TableStorageCommandService(TableServiceClient tableServiceClient, ILogger<TableStorageCommandService> logger)
    {
        _tableServiceClient = tableServiceClient;
        _logger = logger;
    }

    public async Task AddEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        try
        {
            await tableClient.AddEntityAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding an entity to the table storage.");
            throw;
        }
    }

    public async Task UpdateEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
    {
        var tableClient = _tableServiceClient.GetTableClient(tableName);

        try
        {
            await tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new InvalidOperationException("The entity does not exist.", ex);
        }
        catch (RequestFailedException ex) when (ex.Status == 412)
        {
            throw new InvalidOperationException("The entity has been modified since it was last retrieved.", ex);
        }
    }
}