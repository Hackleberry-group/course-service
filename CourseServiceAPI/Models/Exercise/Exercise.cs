using Azure;
using Azure.Data.Tables;
using CourseServiceAPI.Helpers;

namespace CourseServiceAPI.Models.Exercise
{
    public class Exercise : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
/*        public Guid Id
        {
            get => Guid.Parse(RowKey);
            set => RowKey = value.ToString();
        }*/
        public int Order { get; set; }
        public bool IsTopicExam { get; set; }
        public Guid TopicId { get; set; }
        public Exercise()
        {
            PartitionKey = EntityConstants.ExercisePartitionKey;
            RowKey = Guid.NewGuid().ToString();
        }
    }
}