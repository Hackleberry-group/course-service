using Azure;
using Azure.Data.Tables;

namespace CourseServiceAPI.Models.Course;

public class Course : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Name { get; set; }
    public string ProgrammingLanguage 
    {   get { return ProgrammingLanguage; }
        set {
                if (Enum.TryParse<ProgrammingLanguage>(value, out var language))
                {
                    ProgrammingLanguage = value;
                }
                else
                {
                    throw new ArgumentException("Invalid Programming Language");
                }
        }
    }
    public string TeacherId { get; set; }
}