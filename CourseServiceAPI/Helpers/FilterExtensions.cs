namespace CourseServiceAPI.Helpers;

public static class FilterExtensions
{
    public static string ToFilter<T>(this Guid value, string propertyName)
    {
        return $"{propertyName} eq guid'{value}'";
    }
}