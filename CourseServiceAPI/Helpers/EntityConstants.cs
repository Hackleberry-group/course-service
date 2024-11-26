namespace CourseServiceAPI.Helpers;

public static class EntityConstants
{
    public const string ExerciseTableName = "Exercise";
    public const string ExercisePartitionKey = "ExerciseEntity";

    public const string TopicTableName = "Topic";
    public const string TopicPartitionKey = "TopicEntity";

    public const string ModuleTableName = "Module";
    public const string ModulePartitionKey = "ModuleEntity";

    public const string CourseTableName = "Course";
    public const string CoursePartitionKey = "CourseEntity";
}