using Azure;
using Azure.Data.Tables;

namespace CourseServiceAPI.Models;

public class Course : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Title { get; set; }
}