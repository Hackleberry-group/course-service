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


        public Guid Id { get; set; }


        public int Order { get; set; }


        public bool IsTopicExam { get; set; }


        public Guid TopicId { get; set; }

        public List<Question> Questions { get; set; }

        public Exercise()
        {
            PartitionKey = EntityConstants.ExercisePartitionKey;
            Id = Guid.NewGuid();
            RowKey = Id.ToString();
        }
    }

    // Placeholder
    public class AnsweredQuestion
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }

    public class ExerciseCompletion
    {
        public Guid ExerciseId { get; set; }
        public List<List<AnsweredQuestion>> AnsweredQuestions { get; set; }
    }
    
    // Placeholder
    public class Answer
    {
        public Guid AnswerId { get; set; }
        public string Text { get; set; }
    }

    // Placeholder
    public class Question
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public List<Answer> Answers { get; set; }
        public Guid CorrectAnswerId { get; set; }
    }
}