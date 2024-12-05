using Azure;
using Azure.Data.Tables;

namespace CourseServiceAPI.Models.Module
{
    public class Module : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public Guid CourseId { get; set; }
        public List<Guid> TopicIds { get; set; } = new List<Guid>();
    }
}
