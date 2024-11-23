using Azure;
using Azure.Data.Tables;

namespace CourseServiceAPI.Models.Exercise
{
    public class Exercise : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public Guid Id { get; set; }

        public int Order { get; set; }

        public bool IsTopicExam { get; set; }

        public Guid TopicId { get; set; }

        public List<Question> Questions { get; set; }

        public Exercise()
        {
            PartitionKey = "Exercise";
            Id = Guid.NewGuid(); // Assign a new Guid value to the Id property
            RowKey = Id.ToString();
        }

    }

    public class Question
    {
        // Define properties for the Question class as needed
    }
}