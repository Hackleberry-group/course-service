using Azure;
using Azure.Data.Tables;
using System.Runtime.Serialization;

namespace CourseServiceAPI.Models.Course;

public class Course : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Name { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string TeacherId { get; set; }
    [IgnoreDataMember]
    public List<Guid> ModuleIds { get; set; } = new List<Guid>();
}